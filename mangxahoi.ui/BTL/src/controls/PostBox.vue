<template>
  <div class="post-box">
    <div class="header-post">
      <div class="user-post">
        <div class="avatar">
          <img :src="bindingUrlImage(post.UserId + '_.jpg')" />
        </div>
        <div class="item-descrip">
          <div
            class="user-name user-name-post"
            @click="goToProfie(post.UserId)"
          >
            {{ post.FullName }}
          </div>
          <div class="timing-comment">
            {{ post.CreateDateString }}
          </div>
        </div>
      </div>
      <div class="edit-post" v-if="post.UserId == user.Id">
        <div class="div" @click="showEdit = true">...</div>
        <div class="edit-option" v-show="showEdit">
          <div class="div" @click="postOptionClick('Edit')">Sửa</div>
          <div class="div" @click="postOptionClick('Delete')">Xóa</div>
        </div>
      </div>
    </div>
    <div class="content-post">
      <div class="post-text-box">
        {{ post.Content }}
      </div>
      <!-- <div v-if="post.isAvatar == 1" class="post-image-box"> -->
      <div class="post-image" :style="imageByPost(post.Id)">
        <img
          v-for="(img, index) in post.post_image"
          :key="index"
          :src="bindingUrlImage(img.Url)"
          :style="{
            width: 100 / post.post_image.length + '%',
            padding: '10px',
          }"
          @click="showDialogDetail(img.Url)"
        />
      </div>
    </div>
    <div class="interact-summary">
      <div class="count-like-box">
        <div class="like1-icon"></div>
        <div class="like-comment-count">
          {{ post.LikesCount }}
        </div>
      </div>
      <div class="count-comment-box">
        <div class="like-comment-count">{{ post.CommentCount }} bình luận</div>
      </div>
    </div>
    <div class="like-comment-box">
      <div @click="likePost()" class="like-comment-button">
        <div
          class="icon-like-chat1"
          :class="{ 'like2-icon': !post.isLike }"
        ></div>
        <div class="like-comment-text" v-show="!post.isLike">Thích</div>
        <div class="like-comment-text" v-show="post.isLike">Bỏ thích</div>
      </div>
      <div class="like-comment-button">
        <div class="comment-icon"></div>
        <div class="like-comment-text">Bình luận</div>
      </div>
    </div>
    <div class="more-comment">
      <!-- tổng số like và cmt -->
      <div class="more-comment-text" @click="showOldComment()" v-if="isShowOldComment && post.CommentCount > 0">
        Xem bình luận
      </div>
    </div>
    <div class="comment-box">
      <div v-for="(cmt, i) in post.lstCmt" :key="i" class="a-comment">
        <div class="avatar-comment" @click="goToProfie(cmt.UserId)">
            <img :src="bindingUrlImage(cmt.UserId + '_.jpg')"/>
        </div>
        <div class="content-comment">
          <div class="cover-username-text-comment">
            <div class="username-comment" @click="goToProfie(cmt.UserId)">
              {{ cmt.UserName }}
            </div>
            <div class="text-comment">
              {{ cmt.Content }}
            </div>
          </div>
          <div class="like-time-comment">
            <div class="time-cmt-text">
              {{ cmt.CreateDateString }}
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="my-comment-box">
      <div class="avatar-comment">
        <img :src="bindingUrlImage(user.Id + '_.jpg')" />
      </div>
      <div class="cover-input-comment">
        <input
          type="text"
          class="input-comment"
          placeholder="Viết bình luận..."
          v-model="post.Comment"
          @keyup.enter="postComment()"
        />
        <div @click="postComment()" class="icon-send-comment"></div>
      </div>
    </div>
    <PostDialog :post="post" :urlImg="url_img" v-if="showDialog" @hideDialog="hideDialog"/>
  </div>
</template>

<script>
import { BASE_URL } from "../configs/index";
import axios from "axios";
import { mapState, mapActions } from "vuex";
import PostDialog from "../controls/PostDialog.vue";
export default {
  components: {
    PostDialog,
  },
  props: {
    post: {
      type: Object,
      default: null,
    },
  },
  created() {
    document.body.addEventListener("click", this.clickToBody, true);
  },
  data() {
    return {
      showEdit: false,
      showDialog: false,
      url_img: '',
      isShowOldComment: true,
    };
  },
  computed: {
    ...mapState({
      user: (state) => state.user.user,
      webSocket: (state) => state.user.webSocket,
      userOnline: (state) => state.user.userOnline,
    }),
  },
  methods: {
    ...mapActions("user", []),
    
    goToProfie(id) {
      this.$router.push({ name: "profile", params: { id: id } });
    },
    showOldComment() {
      let me = this;
      axios
        .get(`${BASE_URL}posts/post_comment?Id=${me.post.Id}&page=1&record=50`)
        .then((res) => {
          if (!me.post.lstCmt) {
            me.post.lstCmt = [];
          }
          res.data.Data.forEach((cmt) => {
            if (!me.post.lstCmt.find((_) => _.Id == cmt.Id)) {
              me.post.lstCmt.push(cmt);
            }
          });
          this.isShowOldComment = false

        });
    },
    hideDialog(){
        let me = this;
        me.showDialog = false;
    },
    postComment() {
      let me = this;
      if (me.post.Comment) {
        axios
          .post(`${BASE_URL}posts/add_comment`, {
            Content: me.post.Comment,
            PostId: me.post.Id,
            UserName: me.user.FullName,
            UserId: me.user.Id,
          })
          .then((res) => {
            if (res.data.Success) {
              if (!Array.isArray(me.post.lstCmt)) {
                me.post.lstCmt = [];
              }
              me.post.lstCmt.push(res.data.Data);
              me.post.Comment = "";
              me.post.CommentCount++;
              me.webSocket.emit("notify", me.post.UserId);
            }
          });
      }
    },
    imageByPost() {
      let requireURL = "anhdd.jpg";
      return {
        backgroundImage: `../assets/img(${requireURL})`,
      };
    },
    bindingUrlImage(fileName) {
      return `${BASE_URL}posts/${fileName}`;
    },
    clickToBody() {
      if (this.showEdit) {
        this.showEdit = false;
      }
    },
    postOptionClick(option) {
      switch (option) {
        case "Edit":
          break;
        case "Delete":
          this.deletePost();
          break;
        default:
          break;
      }
    },
    deletePost() {
      if (this.post.Id) {
        axios
          .delete(`${BASE_URL}posts/remove?id=${this.post.Id}`)
          .then((res) => {
            if (res.data.Success) {
              this.showNotification("Xóa thành công", "success");
              this.$emit("removePost", this.post.Id);
            }
          });
      }
    },
    showNotification(message, type) {
      this.$notification[type]({
        message,
        duration: 2,
      });
    },
    likePost() {
      let me = this;
      axios
        .post(`${BASE_URL}posts/like_post`, {
          PostId: me.post.Id,
          UserId: me.user.Id,
        })
        .then((res) => {
          if (res.data.Success) {
            if (res.data.Data.isLike) {
              me.post.LikesCount++;
              me.post.isLike = true;
              me.webSocket.emit("notify", me.post.UserId);
            } else {
              me.post.LikesCount--;
              me.post.isLike = false;
            }
          }
        });
    },
    showDialogDetail(url){
        let me = this;
        me.showDialog = true;
        me.url_img = url;
    }
  },
};
</script>

<style>
.post-box {
  background-repeat: no-repeat;
  background-size: contain;
  background-color: white;
  width: 95%;
  margin: 0 2.5%;
  margin-top: 30px;
  border-radius: 4px;
}
.header-post {
  padding: 5px;
  display: flex;
  justify-content: space-between;
}
.edit-post {
  margin-right: 8px;
  cursor: pointer;
  position: relative;
}
.edit-option {
  width: 50px;
  height: 50px;
  border: 1px;
  border-radius: 10px;
  position: absolute;
  top: 22px;
  right: -22px;
  background: #474444;
}
.edit-option .div {
  padding: 2px 12px;
  color: #fff;
}
.user-post {
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
}
.timing-comment,
.like-comment-count {
  font-size: 12px;
  color: #666;
}
.post-text-box {
  padding: 0 14px;
  color: #212121;
  font-size: 14px;
  overflow: hidden;
  word-break: break-word;
  line-height: 1.5;
  letter-spacing: 0.196px;
}
.post-image-box {
  margin: 0 14px;
  padding: 10px 0;
}
.post-image {
  /* background-image: url('../img/post-image.png'); */
  background-size: contain;
  background-repeat: no-repeat;
  background-position: center;
  width: 90%;
  margin: auto;
  height: 90%;
}
.interact-summary {
  display: flex;
  width: 100%;
  justify-content: space-between;
}
.count-like-box {
  display: flex;
  align-items: center;
  margin-left: 16px;
}
.timing-comment,
.like-comment-count {
  font-size: 12px;
  color: #666;
}

.like1-icon {
  background-image: url("../assets/img/icon2.svg");
  width: 24px;
  height: 24px;
  background-position: -216px -192px;
  background-repeat: no-repeat;
}

.comment-box {
  margin-top: 14px;
}

.text-comment {
  word-break: break-word;
  line-height: 1.5;
  letter-spacing: 0.196px;
  color: #212529;
  font-size: 14px;
}

.count-comment-box {
  display: flex;
  align-items: center;
  margin-right: 20px;
}

.like-comment-box {
  color: #1f1f1f;
  height: 40px;
  width: 100%;
  background-color: white;
  border-top: 0.5px solid var(--gray);
  border-bottom: 0.5px solid var(--gray);
  display: flex;
  align-items: center;
}

.like2-icon {
  background-image: url("../assets/img/icon2.svg");
  width: 24px;
  height: 24px;
  background-position: -24px -360px;
  background-repeat: no-repeat;
}

.comment-icon {
  background-image: url("../assets/img/icon2.svg");
  width: 24px;
  height: 24px;
  background-position: -48px -360px;
  background-repeat: no-repeat;
}

.like-comment-button {
  display: flex;
  align-items: center;
  cursor: pointer;
  margin-left: 15px;
  height: 40px;
  padding: 0 5px;
}

.like-comment-button:hover {
  background-color: rgba(0, 90, 189, 0.05);
  border-radius: 4px;
}

.like-comment-text {
  margin-left: 10px;
  font-size: 14px;
}

.more-comment-text {
  cursor: pointer;
  font-size: 14px;
  color: #005abd;
  margin-left: 14px;
}

.more-comment-text:hover {
  text-decoration: underline;
}

.count-comment {
  margin-right: 14px;
  font-size: 14px;
}

.more-comment {
  display: flex;
  width: 100%;
  justify-content: space-between;
  margin-top: 10px;
}

.comment-box {
  width: 100%;
  height: fit-content;
}

.a-comment {
  display: flex;
}

.avatar-comment {
  background-image: url("../assets/img/icon1.svg");
  min-width: 32px;
  min-height: 32px;
  background-position: -352px -894px;
  height: 32px;
  width: 32px;
  border-radius: 50%;
  margin-left: 10px;
  cursor: pointer;
}

.like-time-comment {
  display: flex;
  margin-left: 15px;
}

.like-cmt-text {
  font-size: 12px;
  color: #666;
  font-weight: 600;
  cursor: pointer;
}

.time-cmt-text {
  font-size: 12px;
  color: #666;
  margin-left: 10px;
}

.content-comment {
  width: 95%;
}

.cover-username-text-comment {
  max-width: 97%;
  width: 97%;
  padding: 7px 12px;
  border-radius: 16px;
  background-color: #f5f6f8;
  border: 1px solid #fff;
  margin-bottom: 4px;
  display: inline-block;
  min-height: 32px;
  margin-left: 10px;
  margin-right: 10px;
}

.username-comment {
  color: #005abd;
  font-weight: 500;
  cursor: pointer;
  font-size: 14px;
}

.username-comment:hover,
.like-cmt-text:hover {
  text-decoration: underline;
}

.my-comment-box {
  display: flex;
  width: 100%;
  margin-top: 10px;
}

.input-comment {
  margin-left: 10px;
  width: 97%;
  outline: none;
  box-sizing: border-box;
  border: none;
  border-radius: 24px;
  background-color: #f5f6f8;
  padding: 8px;
  max-height: 200px;
}

.cover-input-comment {
  width: 100%;
}

.icon-send-comment {
  cursor: pointer;
  background-image: url("../assets/img/icon2.svg");
  height: 16px;
  width: 16px;
  position: relative;
  top: -24px;
  right: -95%;
  background-position: -700px -460px;
}
.avatar-comment img {
  width: 100%;
  height: 100%;
  border-radius: 50%;
}
.user-name-post:hover {
  cursor: pointer;
  text-decoration: underline;
}
</style>