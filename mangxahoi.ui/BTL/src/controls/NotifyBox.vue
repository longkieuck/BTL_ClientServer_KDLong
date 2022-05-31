<template>
  <div class="notify-mess-box" id="notify-box">
    <div class="notify-mess-title">Thông báo</div>
    <div class="notify-mess-content">
      <div
        v-for="noti in listNotify"
        :key="noti.Id"
        class="a-notify-mess"
        @click="showPostDialog(noti.Id,noti.PostId)"
      >
        <div class="avatar avatar-notify-mess">
          <img :src="bindingUrlImage(noti.UserIdAction + '_.jpg')" />
        </div>
        <div class="text-notify-mess">
          <div class="content-notify-mess-n">
            {{ noti.Content }}
          </div>
          <div class="time-notify-mess">{{ noti.CreateDateString }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
// import axios from "axios";
import { BASE_URL } from "../configs/index";

import { mapActions } from "vuex";
export default {
  // components: {
  //   PostDialog,
  // },
  created() {
    if(!this.$auth.Intance()){
      this.$router.push({ path: "/login" })
    }
    console.log(this.listNotify);
  },
  props: {
    listNotify: {
      type: Array,
      default: null,
    },
  },
  data() {
    return {
      post: null,
    };
  },
  methods: {
    ...mapActions("user", ["showPostNotify"]),
    bindingUrlImage(fileName) {
      return `${BASE_URL}posts/${fileName}`;
    },
    showPostDialog(notiId,postId) {
      this.showPostNotify(postId);
      this.$auth.Intance().put(`${BASE_URL}Notify`,{
        id : notiId
      }).then((res) => {
            console.log(res)
          });
    },
    showNotification(message, type) {
      this.$notification[type]({
        message,
        duration: 2,
      });
    },
  },
};
</script>

<style>
.notify-mess-box {
  position: fixed;
  height: 370px;
  width: 270px;
  background-color: white;
  z-index: 1;
  right: 100px;
  top: 50px;
  border-radius: 4px;
  box-shadow: 0 1px 12px rgb(0 0 0 / 40%);
}
.notify-mess-title {
  height: 35px;
  width: 100%;
  font-size: 17px;
  font-weight: bold;
  padding: 10px 0 0 10px;
}
.notify-mess-content {
  width: 100%;
  height: 310px;
  overflow-y: auto;
  box-sizing: border-box;
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
.avatar-notify-mess {
  margin-left: 10px;
  margin-top: 4px;
}
.text-notify-mess {
  margin-left: 10px;
}
.username-notify-mess {
  font-size: 14px;
  font-weight: 600;
}
.content-notify-mess-n {
  font-size: 13px;
  max-width: 200px;
}
.mess-unseen {
  color: #1876f2;
  font-weight: 700;
}
.time-notify-mess {
  font-size: 11px;
  color: #616161;
}
.a-notify-mess {
  width: 100%;
  height: 58px;
  cursor: pointer;
  display: flex;
}
.a-notify-mess:hover {
  background-color: var(--gray-hover);
  border-radius: 4px;
}
</style>