// import axios from "axios";
import {  defaultState } from '../../configs/index'
import io from "socket.io-client";
const state = {...defaultState }

const actions = {
    async initSocket(context,payload) {
        context.state.webSocket = await io("http://localhost:3000");
        await context.state.webSocket.on("messageLoad", (userId) => {
            if (userId == context.state.user.Id) {
                if(payload){
                    payload.loadListMessage()
                }
                // context.dispatch('loadListMsgNotify')
            }
        })
        await context.state.webSocket.emit("userOn", context.state.user.Id)
        await context.state.webSocket.on("onlineLoad", (userOnline) => {
            context.state.userOnline = Object.assign({},
                userOnline.filter((userId) => userId != context.state.user.Id)
                
            );
            console.log('user online',userOnline)
        })
        // await context.state.webSocket.on("notifyLoad", (userId) => {
        //     if (userId == context.state.user.gid) {
        //         payload.loadListPost()
        //         context.dispatch('loadListNotify')
        //     }
        // })
    },

    decreeCountMsgNew(context) {
        context.state.countMsgNew -= 1
    },
    setUser(context, user) {
        context.state.user = {...user }
    },
    setUserProfile(context, user) {
        context.state.userProfile = {...user }
    },
    setVerifyNumber(context, number) {
        context.state.verifyNumber = number
    },
    setUserName(context, username) {
        context.state.userName = username
    },
    setStringKeyWord(context, text) {
        context.state.stringKeyWord = text
    },
    setDefaultForState(context) {
        context.state = {...defaultState }
    }
}

const mutations = {

}

export default {
    namespaced: true,
    state,
    actions,
    mutations
}