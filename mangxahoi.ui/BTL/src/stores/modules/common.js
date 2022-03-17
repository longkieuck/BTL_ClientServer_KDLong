import axios from "axios";
import { BASE_URL } from '../../configs/index'
const state = {
    isShowChatBox : false,
    userChat:null,
    chatBoxData : null,
    listMessage : []
}

const actions = {
    hideChatBox(context){
        context.state.isShowChatBox = false;
    },
    openChatBox(context,oneUser){
        console.log(oneUser)
        let userData = localStorage.getItem('currentUser');
        context.state.userChat = { ...oneUser}
        context.state.isShowChatBox = true;
        axios
        .get(
          `${BASE_URL}ChatBoxs/chatbox_id?userId1=${JSON.parse(userData).Id}&userId2=${oneUser.Id}`,
        )
        .then((res) => {
            context.state.chatBoxData = {...res.data.Data};
            context.dispatch('loadListMessage')
        });
    },
    loadListMessage(context){
        //Load list tin nhắn  ban đầu 20 bản ghi
        axios
        .get(
        `${BASE_URL}ChatBoxs/detail?chat_box_id=${context.state.chatBoxData.Id}&page=1&record=20`,
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