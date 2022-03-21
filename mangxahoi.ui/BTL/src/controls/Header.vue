<template>
    <div class="header">
      <div class="cover-logo">
        <div class="logo" @click="goToNewFeed()"></div>
        <div class="app-name">
          FAKEBOOK
        </div>
      </div>
      <search-box/>
      <div class="cover-right-header">
        <div class="avatar">
          <img :src="bindingUrlImage(user.Id + '_.jpg')"/>
        </div>
        <div class="full-name">
          {{ user.FullName }}
        </div>
        <div class="mess-icon" @click="handleMessBox()">
          <div v-if="messCount != 0" class="count-mess-noti">
            {{messCount}}
          </div>
        </div>

        <div class="noti-icon" @click="handleNotifyBox()">
          <div v-if="notiCount != 0" class="count-mess-noti">
            {{notiCount}}
          </div>
        </div>

        <div class="logout-icon" @click="logout()"></div>
      </div>
      <MesageNotifyBox v-if="showMessBox" :listMessNotify="listMessNotify"/>
      <NotifyBox v-if="showNotifyBox" :listNotify="listNotify"/>
    </div>
</template>

<script>
import SearchBox from '../controls/SearchBox.vue'
import MesageNotifyBox from '../controls/MesageNotifyBox.vue'
import NotifyBox from '../controls/NotifyBox.vue'
import { mapActions, mapState  } from "vuex";
import { BASE_URL } from "../configs/index";
// import axios from "axios";
export default {
    components:{
        SearchBox,
        MesageNotifyBox,
        NotifyBox
    },
    data(){
      return {
        currentItem: {},
        showNotifyBox: false,
        showMessBox:false,
      
        // notiCount:0,
        // listNotify:[]
      }
    },
    created(){
      let me = this;
      let userData = localStorage.getItem('currentUser');
      if((!me.user || (me.user && !me.user.Id)) && userData){
        me.currentItem = JSON.parse(userData);
        me.currentItem.avatar = me.currentItem.Id + '_.jpg';
        me.setUser(me.currentItem);
      }

      this.loadNotify()
      this.loadMessNotify()
      // document.body.addEventListener("click", this.clickHideBox, true);
    },
    methods:{
        ...mapActions("user", ["setUser", "setDefaultForState","loadNotify","loadMessNotify" ]),
        handleNotifyBox(){
          this.showNotifyBox = !this.showNotifyBox
          this.showMessBox = false
        },
        handleMessBox(){
          this.showMessBox = !this.showMessBox
          this.showNotifyBox = false
        },
        goToNewFeed() {
            this.$router.push({ path: "/newfeed" });
        },
        logout(){
          let me = this;
          this.$router.replace({ path: "/login" });
          localStorage.setItem('currentUser', null);
          me.setUser(null);
          if (this.webSocket != null) {
            this.webSocket.disconnect();
          }
          this.setDefaultForState();
          me.$router.push({ path: "/login" });
        },
        bindingUrlImage(fileName){
            return `${BASE_URL}posts/${fileName}`;
        },
        showNotification(message, type) {
        this.$notification[type]({
            message,
            duration: 2,
        });
      },
    },
    computed:{
      ...mapState({
        user:(state)=>state.user.user,
        webSocket:(state)=>state.user.webSocket,
        notiCount:(state)=>state.user.notiCount,
        listNotify:(state)=>state.user.listNotify,
        messCount:(state)=>state.user.messCount,
        listMessNotify:(state)=>state.user.listMessNotify,
      })
    }
}
</script>

<style>
.header {
    background-color: var(--blue);
    height: 64px;
    display: flex;
    z-index: 1;
    position: sticky;
    top: 0;
}
.cover-logo {
    height: 64px;
    width: 250px;
    display: flex;
    align-items: center;
    justify-content: space-around;
}

.logo {
    cursor: pointer;
    background-image: url('../assets/img/icon1.svg');
    height: 50px;
    width: 50px;
    min-height: 50px;
    min-width: 50px;
    background-position: -597px -748px;
}
.app-name {
    font-size: 23px;
    font-weight: 700;
    color: #fff;
    cursor: pointer;
    width: auto;
    margin-right: 32px;
    min-width: 118px;
}
.cover-right-header {
    height: 64px;
    width: 350px;
    display: flex;
    align-items: center;
    justify-content: space-around;
}item-descrip
.avatar {
    background-image: url('../assets/img/icon1.svg');
    min-width: 32px;
    min-height: 32px;
    background-position: -352px -894px;
    height: 32px;
    width: 32px;
    border-radius: 50%;
    margin-left: 10px;
}
.avatar img{
  width: 100%;
  height: 100%;
  border-radius: 50%;
}
.full-name {
    margin-right: 10px;
    color: white;
}
.mess-icon {
    background-image: url('../assets/img/icon1.svg');
    min-width: 24px;
    min-height: 24px;
    background-position: -368px -88px;
    height: 24px;
    width: 24px;
    margin-right: 10px;
    cursor: pointer;
}
.count-mess-noti {
    width: 15px;
    height: 15px;
    border-radius: 50%;
    background-color: red;
    font-size: 10px;
    color: white;
    display: flex;
    align-items: center;
    justify-content: center;
    margin-top: 10px;
    margin-left: 10px;
}
.noti-icon {
    background: url('../assets/img/icon1.svg') no-repeat -256px -88px;
    min-width: 24px;
    min-height: 24px;
    height: 24px;
    width: 24px;
    margin-right: 10px;
    cursor: pointer;
}

.logout-icon {
    background: url('../assets/img/icon2.svg') no-repeat -143px -252px;
    min-width: 34px;
    min-height: 34px;
    height: 34px;
    width: 34px;
    /* margin-right: 10px; */
    transform: rotate(90deg);
    cursor: pointer;
}
</style>