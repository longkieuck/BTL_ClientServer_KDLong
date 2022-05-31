import axios from "axios";
import {  BASE_URL,defaultState } from '../../configs/index'
import io from "socket.io-client";
const keyJwt = "Jwt";
const resToken = localStorage.getItem(keyJwt) ? JSON.parse(localStorage.getItem(keyJwt)) : localStorage.getItem(keyJwt);
// const intance = axios.create({
//     headers: {
//         'Authorization': `Bearer ${resToken}`
//     }
// });
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
        await context.state.webSocket.on("notifyLoad", (userId) => {
            if (userId == context.state.user.Id) {
                context.dispatch('loadNotify')
            }
        })

        await context.state.webSocket.on("notifyMessLoad", (userId) => {
            if (userId == context.state.user.Id) {
                context.dispatch('loadMessNotify')
            }
        })
    },
    showPostNotify(context,postId){
        axios.get(`${BASE_URL}Posts/detail?id=${postId}`,{
            headers: {
                'Authorization': `Bearer ${resToken}`
            }
        })
                .then(res =>{
                    context.state.postNotify = res.data.Data.post
                    context.state.showPostNotify = true;
                    context.state.notiCount = context.state.notiCount - 1;
                  }
                )
                .catch(e =>{
                    console.log(e)
                })
    //     postNotify:{},
    // showPostNotify:false
    },
    loadNotify(context){
        axios.get(`${BASE_URL}Notify?user_id=${context.state.user.Id}&page=1&record=20`,{
            headers: {
                'Authorization': `Bearer ${resToken}`
            }
        })
            .then(res =>{
                context.state.notiCount = res.data.TotalRecord
                context.state.listNotify = res.data.Data
                }
            )
            .catch(e =>{
                console.log(e)

            })
    },
    loadMessNotify(context){
        axios.get(`${BASE_URL}Notify/mess_notify?user_id=${context.state.user.Id}&page=1&record=20`,{
            headers: {
                'Authorization': `Bearer ${resToken}`
            }
        })
            .then(res =>{
                    context.state.listMessNotify = res.data.Data
                    if(JSON.stringify(res.data.Data) != JSON.stringify(context.state.loadMessNotify)){
                        context.state.messCount = 1
                    }else{
                        context.state.messCount = 0
                    }
                }
            )
            .catch(e =>{
                console.log(e)
            })
    },
    hidePostNotify(context){
        context.state.postNotify = {}
        context.state.showPostNotify = false;
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
    },
    hideChatBox(context){
        context.state.isShowChatBox = false;
    },
    openChatBox(context,oneUser){
        let userData = localStorage.getItem('currentUser');
        context.state.userChat = { ...oneUser}
        context.state.isShowChatBox = true;
        axios
        .get(
          `${BASE_URL}ChatBoxs/chatbox_id?userId1=${JSON.parse(userData).Id}&userId2=${oneUser.Id}`,
            {
                headers: {
                    'Authorization': `Bearer ${resToken}`
                }
            }
        )
        .then((res) => {
            context.state.chatBoxData = {...res.data.Data};
            let isOnline = Object.values(context.state.userOnline).find(
                (userId) => userId == context.state.userChat.Id
              );
            context.state.userChat.isOnline = isOnline
            context.dispatch('loadListMessage')
        });
    },
    openChatBoxWithId(context, chatBoxId){
        context.state.chatBoxData.Id = chatBoxId
        context.state.messCount = 0
        axios
        .get(
        `${BASE_URL}ChatBoxs/info?id=${chatBoxId}`,
            {
                headers: {
                    'Authorization': `Bearer ${resToken}`
                }
            }
        )
        .then((res) => {
            context.state.chatBoxData = {...res.data.Data}
            if(res.data.Data.UserId1 == context.state.user.Id){
                context.state.userChat.Id = res.data.Data.UserId2
                context.state.userChat.FullName = res.data.Data.UserName2
            }else{
                context.state.userChat.Id = res.data.Data.UserId1
                context.state.userChat.FullName = res.data.Data.UserName1
            }
            context.dispatch('loadListMessage')
            context.state.isShowChatBox = true;

            let isOnline = Object.values(context.state.userOnline).find(
                (userId) => userId == context.state.userChat.Id
              );
            context.state.userChat.isOnline = isOnline
            
        });

    },
    loadListMessage(context){
        //Load list tin nhắn  ban đầu 20 bản ghi
        axios
        .get(
        `${BASE_URL}ChatBoxs/detail?chat_box_id=${context.state.chatBoxData.Id}&page=1&record=20`,
        {
            headers: {
                'Authorization': `Bearer ${resToken}`
            }
        }
        )
        .then((resMess) => {
            context.state.listMessage = [...resMess.data.Data];
        });
    },
    sendMsg(context,{msg,callback}){
        let userData = JSON.parse(localStorage.getItem('currentUser'));

        axios
            .post(
                `${BASE_URL}ChatBoxs/add_message`,
                {
                    content: msg,
                    userId: userData.Id,
                    isLike: msg == 'isLike' ? 1 : 0,
                    userName: userData.FullName,
                    chatBoxId: context.state.chatBoxData.Id
                },
                {
                    headers: {
                        'Authorization': `Bearer ${resToken}`
                    }
                }
            )
            .then((res)=>{
                context.state.listMessage = [...context.state.listMessage,res.data.Data]
            })
            .then(()=>callback())
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