<template>
    <div class="notify-mess-box" id="notify-box">
      <div class="notify-mess-title">
        Thông báo
      </div>
      <div class="notify-mess-content">
        <div v-for="noti in listNotify" :key="noti.gid" class="a-notify-mess">
          <div
            class="avatar avatar-notify-mess"
            :style="imageByUser(noti.userID1)"
          ></div>
          <div class="text-notify-mess">
            <div class="username-notify-mess">{{ noti.fullNameFriend }}</div>
            <div
              v-if="noti.action == 1"
              class="content-notify-mess"
              :class="{ 'mess-unseen': noti.isSeen != 1 }"
            >
              Đã thích bài viết của bạn
            </div>
            <div
              v-else
              class="content-notify-mess"
              :class="{ 'mess-unseen': noti.isSeen != 1 }"
            >
              Đã bình luận bài viết của bạn
            </div>
            <div class="time-notify-mess">{{ noti.createdTimeST }}</div>
          </div>
        </div>
      </div>
    </div>
</template>

<script>
    export default {
        data(){
            return{
                listNotify:[]
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