<template>
    <div class="notify-mess-box mess-box" id="mess-box">
      <div class="notify-mess-title">
        Tin nhắn
      </div>
      <div class="notify-mess-content">
        <div
          v-for="msgnt in listMsgNotify"
          :key="msgnt.gid"
          class="a-notify-mess"
        >
          <div
            v-if="user.gid != msgnt.userID_1"
            class="avatar avatar-notify-mess"
          ></div>
          <div
            v-else
            class="avatar avatar-notify-mess"
          ></div>
          <div class="text-notify-mess">
            <div v-if="user.gid != msgnt.userID_1" class="username-notify-mess">
              {{ msgnt.fullName1 }}
            </div>
            <div v-else class="username-notify-mess">{{ msgnt.fullName2 }}</div>
            <div v-if="msgnt.isLike" class="icon-like-chat1"></div>
            <div
              v-else
              class="content-notify-mess"
              :class="{ 'mess-unseen': msgnt.isSeen != 1 }"
            >
              {{
                user.gid == msgnt.userID_1
                  ? "Bạn: " + msgnt.message
                  : msgnt.message
              }}
            </div>
            <div class="time-notify-mess">{{ msgnt.createdTimeST }}</div>
          </div>
        </div>
      </div>
    </div>
</template>

<script>
    export default {
        data(){
            return{
                listMsgNotify:[]
            }
        }
    }
</script>

<style>
.notify-mess-box {
    position: fixed;
    height: 370px;
    width: 270px;
    background-color: white;
    z-index: 1;
    right: 60px;
    top: 50px;
    border-radius: 4px;
    box-shadow: 0 1px 12px rgb(0 0 0 / 40%);
}
.mess-box {
    right: 150px;
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
.icon-like-chat1 {
    background-image: url('../assets/img/icon2.svg');
    height: 24px;
    width: 24px;
    background-position: -240px -120px;
}
.content-notify-mess {
    font-size: 13px;
    max-width: 200px;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}
.mess-unseen {
    color: #1876f2;
    font-weight: 700;
}
.time-notify-mess {
    font-size: 11px;
    color: #616161;
}
</style>