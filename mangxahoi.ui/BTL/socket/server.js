const express = require('express')
const http = require('http')
const { Server } = require('socket.io')

const app = express()
const server = http.createServer(app)
const io = new Server(server, {
    cors: {
        origin: '*',
        methods: ['GET', 'POST']
    }
})
var userOnline = []
io.on('connection', (socket) => {
    // console.log(`user ${socket.id} is connected!`)
    console.log('list userID online:',userOnline)

    socket.emit('onlineLoad', userOnline)

    socket.on('message', (userId) => {
        socket.broadcast.emit('messageLoad', userId)
        console.log("onmessage server")
    })

    socket.on('notify', (userId) => {
        socket.broadcast.emit('notifyLoad', userId)
        console.log("id owner post", userId)
    })

    socket.on('userOn', (userId) => {
        userOnline = userOnline.filter(item => item != userId)
        userOnline.push(userId)
        socket.broadcast.emit('onlineLoad', userOnline)
        console.log(userId + ' is connected!')
    })

    socket.on('userOff', (userId) => {
        userOnline = userOnline.filter(item => item != userId)
        socket.broadcast.emit('onlineLoad', userOnline)
        console.log(userId + ' is disconnect!')
    })

    socket.on('disconnect', () => {
        console.log(`user ${socket.id} left!`)
    })
})

server.listen(3000, () => {
    console.log('Server is running on 3000')
})