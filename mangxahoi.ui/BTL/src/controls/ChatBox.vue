<template>
  <div class="chat-box" v-if="isShowChatBox">
    <div class="top-chat-box">
      <div class="user-chat-box">
        <div class="avatar avt-chat" @click="goToProfie(userChat.Id)">
          <img :src="bindingUrlImage(userChat.Id + '_.jpg')" />
        </div>
        <div class="item-descrip">
          <div class="user-name">
            {{ userChat.FullName }}
          </div>
          <div v-if="userChat.isOnline" class="user-active">
            <div class="ic-active"></div>
            Đang hoạt động
          </div>
          <div v-else class="user-active" style="color: gray">
            Không hoạt động
          </div>
        </div>
      </div>
      <div class="close-button" @click="hideChatBox()">
        <div class="close-icon"></div>
      </div>
    </div>
    <div class="content-chat-box" id="scrollingChat">
      <div v-for="msg in listMessage" :key="msg.Id">
        <div v-if="msg.UserId != user.Id" class="friend-chat-without-avt">
          <div v-if="msg.IsLike" class="icon-like-chat1"></div>
          <div v-else class="friend-mess">
            {{ msg.Content }}
          </div>
        </div>
        <div v-else class="cover-my-chat">
          <div
            v-if="msg.IsLike"
            class="icon-like-chat1"
            style="margin-left: 5px"
          ></div>
          <div v-else class="my-chat">
            {{ msg.Content }}
          </div>
        </div>
      </div>
    </div>
    <div class="footer-chat-box">
      <div class="cover-input-chat">
        <input
          type="text"
          class="input-chat"
          placeholder="Viết tin nhắn..."
          v-model="message"
          @keyup.enter="sendMsgControl(message)"
        />
        <div @click="sendMsgControl(message)" class="icon-send-chat"></div>
        <div class="icon-like-chat" @click="sendMsgControl('isLike')"></div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapActions } from "vuex";
import { BASE_URL } from "../configs/index";

export default {
  data() {
    return {
      message: "",
    };
  },
  methods: {
    ...mapActions("user", ["hideChatBox", "sendMsg"]),
    sendMsgControl(msg) {
      let me = this;
      this.sendMsg({ msg: msg, callback: () => me.scrollToBottomChatBox() });
      this.message = "";
    },
    bindingUrlImage(fileName) {
      return `${BASE_URL}posts/${fileName}`;
    },
    scrollToBottomChatBox() {
      this.webSocket.emit("message", this.userChat.Id);
      this.webSocket.emit("notifyMess", this.userChat.Id);
      // let container = this.$el.querySelector('#scrollingChat')
      // container.scrollTop = container.scrollHeight
    },
    goToProfie(id) {
      this.$router.push({ name: "profile", params: { id: id } });
    },
  },
  computed: {
    ...mapState({
      user: (state) => state.user.user,
      isShowChatBox: (state) => state.user.isShowChatBox,
      userChat: (state) => state.user.userChat,
      chatBoxData: (state) => state.user.chatBoxData,
      listMessage: (state) => state.user.listMessage,
      webSocket: (state) => state.user.webSocket,
    }),
  },
  watch: {
    isShowChatBox() {
      this.scrollToBottomChatBox();
    },
  },
};
</script>

<style>
.chat-box {
  box-shadow: 0 1px 12px rgb(0 0 0 / 40%);
  height: 350px;
  width: 250px;
  position: fixed;
  bottom: 0;
  right: 260px;
  border-top-left-radius: 4px;
  border-top-right-radius: 4px;
  background-color: white;
}
.top-chat-box {
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
  border-bottom: 2px solid var(--gray);
}
.user-chat-box {
  display: flex;
  padding-top: 5px;
  padding-bottom: 5px;
}
.avatar {
  background-image: url("../assets/img/icon1.svg");
  min-width: 32px;
  min-height: 32px;
  background-position: -352px -894px;
  height: 32px;
  width: 32px;
  border-radius: 50%;
  margin-left: 10px;
}
.item-descrip {
  margin-left: 10px;
}
.user-name {
  font-weight: 500;
  margin-left: 0 !important;
}
.user-active {
  color: green;
  font-size: 12px;
  display: flex;
}
.ic-active {
  width: 9px;
  height: 9px;
  background-color: green;
  border-radius: 50%;
  margin-left: -19px;
  margin-top: 1px;
  margin-right: 11px;
}
.close-button {
  height: 24px;
  width: 24px;
  display: flex;
  justify-content: center;
  align-items: center;
  margin-bottom: 10px;
  margin-right: 5px;
}

.close-button:hover {
  background-color: var(--gray-hover);
  border-radius: 50%;
}
.close-icon {
  cursor: pointer;
  background-image: url("../assets/img/icon1.svg");
  height: 16px;
  width: 16px;
  min-height: 16px;
  min-width: 16px;
  background-position: -1176px -360px;
}
.content-chat-box {
  box-sizing: border-box;
  padding: 5px;
  overflow-y: auto;
  height: 260px;
  width: 100%;
}
.friend-chat-without-avt {
  margin: 5px 0 5px 0px;
}
.icon-like-chat1 {
  background-image: url("../assets/img/icon2.svg");
  height: 24px;
  width: 24px;
  background-position: -240px -120px;
}
.friend-mess {
  margin-left: 5px;
  font-size: 14px;
  color: #050505;
  max-width: 150px;
  border-radius: 8px;
  background-color: #e4e6eb;
  padding: 5px;
  width: fit-content;
}
.cover-my-chat {
  width: 100%;
  display: flex;
  justify-content: flex-end;
}
.my-chat {
  font-size: 14px;
  color: white;
  max-width: 150px;
  border-radius: 8px;
  background-color: #0084ff;
  padding: 5px;
  width: fit-content;
  margin-bottom: 5px;
}
.footer-chat-box {
  height: 45px;
  width: 100%;
  align-items: center;
  display: flex;
}
.cover-input-chat {
  width: 100%;
  height: 100%;
}
.input-chat {
  margin-top: 4px;
  margin-left: 2%;
  width: 85%;
  outline: none;
  box-sizing: border-box;
  border: none;
  border-radius: 24px;
  background-color: #f0f2f5;
  padding: 8px;
  padding-right: 20px;
  max-height: 200px;
}
.icon-send-chat {
  cursor: pointer;
  background-image: url("../assets/img/icon2.svg");
  height: 16px;
  width: 16px;
  position: relative;
  top: -24px;
  right: -78%;
  background-position: -700px -460px;
}
.icon-like-chat {
  cursor: pointer;
  background-image: url("../assets/img/icon2.svg");
  height: 24px;
  width: 24px;
  position: relative;
  top: -45px;
  right: -89%;
  background-position: -240px -120px;
}
.avt-chat{
  cursor: pointer;
}
</style>