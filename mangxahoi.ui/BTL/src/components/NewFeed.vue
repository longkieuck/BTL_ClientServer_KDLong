<template>
  <div class="cover-newfeed" id="coverNewfeed">
    <Header/>
    <div class="content">
      <div class="left-content">
        <div class="left-content-item">
          <div class="avatar" >
            <img :src="bindingUrlImage(user.Id + '_.jpg')"/>
          </div>
          <div class="item-descrip" @click="goToProfie(user.Id)">
            <div class="user-name">
              {{ user.FullName }}
            </div>
            <div class="birthday-age">
              Trang cá nhân
            </div>
          </div>
        </div>
        <div
          @click="changeTypeShow(0)"
          class="left-content-item"
          :class="{ 'active-item': typeShow == 0 }"
        >
          <div class="news-icon"></div>
          <div class="item-descrip">
            <div class="user-name">
              Bảng tin
            </div>
          </div>
        </div>
        <div
          @click="changeTypeShow(1)"
          class="left-content-item"
          :class="{ 'active-item': typeShow == 1 }"
        >
          <div class="group-icon"></div>
          <div class="item-descrip">
            <div class="user-name">
              Thành viên
            </div>
          </div>
        </div>
        <div class="left-content-item" @click="changePass()">
          <div class="changepass-icon"></div>
          <div class="item-descrip">
            <div class="user-name">
              Đổi mật khẩu
            </div>
          </div>
        </div>
        <div
          @click="logout()"
          class="left-content-item"
          style="margin-left: 12px;"
        >
          <div class="logout2-icon"></div>
          <div class="item-descrip">
            <div class="user-name">
              Đăng xuất
            </div>
          </div>
        </div>
        <!-- modal -->
        
        <div class="my-group">
          <div>Nhóm của tôi</div>
          <div class="add-group-icon" @click="addGroup"></div>
        </div>

        <div
          v-for="group in listGroup" :key="group.Id"
          class="left-content-item"
          @click="viewGroup(group)"
          :class="{ 'active-item': typeShow == group.Id }"
        >
          <div class="my-group-icon"></div>
          <div class="item-descrip">
            <div class="user-name my-group-name">
              {{group.GroupName}}
            </div>
          </div>
        </div>
        
      </div>
      <div class="main-content">
        <div class="cover-search" v-show="typeShow == 1 && isShowGroup == false">
          <div class="cover-list-user">
            <div class="post-box list-user-box" style="margin-left: 20px">
              <UserInfoBox 
                v-for="usercontact in listUserContact"
                :key="usercontact.Id"  
                :user = "usercontact"
              />
            </div>
            <div class="cover-pagination-post" style="margin-left: 0px">
              <a-pagination
                class="pagination-post"
                v-model="userPage"
                :total="totalUser"
                :page-size="PAGE_SIZE"
                @change="changeUserPage"
              />
            </div>
          </div>
        </div>

        <div v-show="typeShow == 0 && isShowGroup == false">
          <div class="share-box">
            <div class="cover-content-share">
              <div class="avatar2">
                <img :src="bindingUrlImage(user.Id + '_.jpg')"/>
              </div>
              <textarea
                v-model="postContent"
                class="input-share"
                type="text"
                placeholder="Bạn muốn chia sẻ điều gì?"
                @keyup.enter="share()"
              />
            </div>
            <div class="cover-footer-share">
              <div style="display: flex;">
                <div class="image-button">
                  <div class="icon-image"></div>
                  <div class="text-image">
                    Hình ảnh
                  </div>
                </div>
              </div>
              <input
                class="hide-input-file"
                type="file"
                @change="onFileChange"
                accept="image/*"
                multiple="multiple"
              />
              <div class="cover-img-post">
                <img v-for="(item, index) in urls" :key="index" :style="{'width': item.width, 'padding':'20px' }" :src="item.url" />
              </div>
              <div @click="share()" class="share-button">Chia sẻ</div>
            </div>
          </div>
          <PostBox v-for="(post) in listPost" :key="post.Id" :post="post" @removePost="handleRemovePost"/>
          <AddMemberDialog v-show="false"/>
          <div class="cover-pagination-post" style="margin-left: 0px">
            <a-pagination
              class="pagination-post"
              v-model="postPage"
              :total="totalPost"
              :page-size="PAGE_SIZE"
              @change="changePostPage"
            />
          </div>
          
        </div>

        <div class="cover-group" v-show="isShowGroup == true">
            <div class="cover-header-group">
              <div class="bg-group"></div>
              <div class="bottom-header">
                <div class="left-bottom">
                  <div class="group-name">
                    {{group_now.GroupName}}
                     <div style="font-size: 13px;font-weight: 100;"> &nbsp;&nbsp;( {{group_now.MemberCount}} thành viên)</div>
                  </div>
                </div>
                <div class="right-bottom">
                  <InputSearch 
                    lWidth="180"
                    placeholder="Tìm kiếm bài viết"
                  />
                  <div class="btn-add" @click="addMember()">
                      Thêm, bớt thành viên
                      <div class="add-mem-icon"></div>
                  </div>
                  <a-tooltip placement="topLeft" title="Rời khỏi nhóm">
                    <div class="icon-left-group"></div>
                  </a-tooltip>
                </div>
              </div>
            </div>
            <div class="share-box">
            <div class="cover-content-share">
              <div class="avatar2">
                <img :src="bindingUrlImage(user.Id + '_.jpg')"/>
              </div>
              <textarea
                v-model="postContent"
                class="input-share"
                type="text"
                placeholder="Bạn muốn chia sẻ điều gì?"
                @keyup.enter="share()"
              />
            </div>
            <div class="cover-footer-share">
              <div style="display: flex;">
                <div class="image-button">
                  <div class="icon-image"></div>
                  <div class="text-image">
                    Hình ảnh
                  </div>
                </div>
              </div>
              <input
                class="hide-input-file"
                type="file"
                @change="onFileChange"
                accept="image/*"
                multiple="multiple"
              />
              <div class="cover-img-post">
                <img v-for="(item, index) in urls" :key="index" :style="{'width': item.width, 'padding':'20px' }" :src="item.url" />
              </div>
              <div @click="share()" class="share-button">Chia sẻ</div>
            </div>
            </div>
            <PostBox v-for="(post, index) in listPost" :key="index" :post="post" @removePost="handleRemovePost" />
        </div>
         
      </div>
      <div class="right-content">
        <div class="contact-box" v-if="isShowGroup">
          <div class="top-contact-box">
             <div class="right-content-title">Thành viên</div>
          </div>
          <div
            v-for="oneUser in lstUserByGroup"
            class="right-content-item"
            :key="oneUser.Id"
            @click="openChatBox(oneUser)"
          >
            <div class="avatar">
              <img :src="bindingUrlImage(oneUser.Id + '_.jpg')"/>
            </div>
            <div class="item-descrip">
              <div class="user-name">
                {{ oneUser.FullName }}
              </div>
              <div  class="user-active">
                {{oneUser.Address}}
              </div>
            </div>
          </div>
        </div>
        <div class="contact-box" v-else>
          <div class="top-contact-box">
            <div class="right-content-title">Danh bạ</div>
          </div>
          <div
            v-for="oneUser in listUser"
            class="right-content-item"
            :key="oneUser.Id"
            @click="openChatBox(oneUser)"
          >
            <div class="avatar">
              <img :src="bindingUrlImage(oneUser.Id + '_.jpg')"/>
            </div>
            <div class="item-descrip">
              <div class="user-name">
                {{ oneUser.FullName }}
              </div>

              <div v-if="oneUser.isOnline" class="user-active">
                <div class="ic-active"></div>
                Đang hoạt động
              </div>
              <div v-else class="user-active" style="color: gray">
                Không hoạt động
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="modal" v-if="isShowFormAddGroup">
          <div class="modal-overlay" @click="isShowFormAddGroup = false"></div>
          <div class="modal-body">
            <div class="title-register">
              <div class="title">
                <div class="register-name">
                  <span style="font-size: 25px;"> <b>Tạo nhóm</b> </span>
                </div>
                <div class="register-cancel">
                  <div @click="isShowFormAddGroup = false" class="cancel-icon"></div>
                </div>
              </div>
            </div>
            <div class="form-register">
              <div class="left-register">
                <div class="group-content">
                  <input v-model="group.GroupName" placeholder="Tên nhóm" />
                </div>
                <div class="header-list">
                  <div style="margin:10px">Thành viên:</div>
                  <input v-model="group.search" @input="searchUserInGroup()" placeholder="Tìm kiếm" />
                </div>
                <div class="list-check">
                  <div class="check-list-item" v-for="oneUser in searchUserInGroup" :key="oneUser.Id">
                    <div class="check-box">
                      <input type="checkbox" v-model="oneUser.isCheck">
                    </div>
                    <div class="user-info">
                       <div class="avatar">
                          <img :src="bindingUrlImage(oneUser.Id + '_.jpg')"/>
                        </div>
                        <div class="user-name">
                            {{ oneUser.FullName }}
                        </div>
                    </div>
                  </div>
                </div>
                <div class="in-register">
                  <div class="register-btn" @click="createGroup()">
                    <span style="color: #ffffff; font-size: 20px;">
                      <b>Tạo</b></span
                    >
                  </div>
                </div>
              </div>
          </div>
        </div>
        </div>
    <PostDialog :post="postNotify" v-if="showPostNotify" @hideDialog="hidePostNotify"/>
    <div class="modal" v-if="isShowFormAddMember">
          <div class="modal-overlay" @click="isShowFormAddMember = false"></div>
          <div class="modal-body">
            <div class="title-register">
              <div class="title">
                <div class="register-name">
                  <span style="font-size: 25px;"> <b>Thêm bớt thành viên</b> </span>
                </div>
              </div>
            </div>
            <div class="form-register">
              <div class="left-register">
                <div class="header-list">
                  <div style="margin:10px">Thành viên:</div>
                  <input v-model="group.search" @input="searchUserInGroup()" placeholder="Tìm kiếm" />
                </div>
                <div class="list-check add-member">
                  <div class="check-list-item" v-for="oneUser in searchUserInGroup" :key="oneUser.Id">
                    <div class="check-box">
                      <input type="checkbox" v-model="oneUser.isCheck">
                    </div>
                    <div class="user-info">
                       <div class="avatar">
                          <img :src="bindingUrlImage(oneUser.Id + '_.jpg')"/>
                        </div>
                        <div class="user-name">
                            {{ oneUser.FullName }}
                        </div>
                    </div>
                  </div>
                </div>
                <div class="in-register">
                  <div class="register-btn" @click="editMemberGroup()">
                    <span style="color: #ffffff; font-size: 20px;">
                      <b>Lưu</b>
                    </span
                    >
                  </div>
                </div>
              </div>
          </div>
        </div>
        </div>
    <chat-box/>
  </div>
</template>

<script>
// import axios from "axios";
import { mapState, mapActions } from "vuex";
import { BASE_URL,PAGE_SIZE_CONST } from "../configs/index";
import Header from '../controls/Header.vue';
import ChatBox from '../controls/ChatBox.vue';
import AddMemberDialog from '../controls/AddMemberDialog.vue';
import InputSearch from '../controls/InputSearch.vue';
import UserInfoBox from '../controls/UserInfoBox.vue';
import PostBox from '../controls/PostBox.vue';
import PostDialog from "../controls/PostDialog.vue";
export default {
  name: "NewFeed",
  components:{
    Header,
    ChatBox,
    AddMemberDialog,
    InputSearch,
    PostBox,
    UserInfoBox,
    PostDialog
  },
  data() {
    return {
      usercontact: "",
      userdate: "",
      postContent: "",
      listMessage: [],
      message: "",
      userChat: null,
      typeShow: 0, //0 bài viết,1 liên hệ
      file: null,
      urls: [],
      listUserContact: [],
      listUser: [],
      listPost: [],
      listGroup: [],
      userPage: 1,
      totalUser: null,
      postPage: 1,
      totalPost: 0,
      PAGE_SIZE: PAGE_SIZE_CONST,
      listViewMore: {},
      listLikes: {},
      userID_2: 0,
      isShowFormNotify: false,
      isShowGroup: false,
      isShowFormAddGroup: false,
      group:{
        GroupName: '',
        search: ''
      },
      current_user: {},
      group_now: {},
      listAllPost: [],
      isShowFormAddMember:false,
      lstUserByGroup: [],
      header : {}
    };
  },
  async created() {
    if(!this.$auth.Intance()){
      this.$router.push({ path: "/login" })
    }
    //Lấy dữ liệu current user
    let userData = localStorage.getItem('currentUser');
    if(userData){
      this.current_user = JSON.parse(userData)
    }
    //load list Post
    this.loadListPost();
    // load list user chat
    this.loadUser();
    //Load list Group
    this.loadGroup();
  },
  mounted(){
    if(this.user.Id){
      let me = this
      this.initSocket({
          loadListMessage: () => me.loadListMessage(),
        });
    }else{
      this.$router.replace({ path: "/login" });
    }
  },
  methods: {
    ...mapActions("user", ["setUser",
      "initSocket",
      "hidePostNotify",
      "openChatBox",
      "loadListMessage",
      "setDefaultForState",
      "hideChatBox"
    ]),
    loadUser(){
      this.$auth.Intance().get(
          `${BASE_URL}Users?page=1&record=20`,
        )
        .then((res) => {
          this.listUser = res.data.Data;
          this.listUser = res.data.Data.filter((item) => item.Id != this.user.Id);
        });
    },
    loadListPost() {
      let me = this;
      this.$auth.Intance()
        .get(
          `${BASE_URL}posts?user_id=${me.current_user.Id}&page=${me.postPage}&record=${me.PAGE_SIZE}`,
        )
        .then((res) => {
          me.listPost = res.data.Data;
          me.totalPost = res.data.TotalRecord
        })
        
    },
    loadGroup(){
      this.$auth.Intance()
        .get(
          `${BASE_URL}GroupPosts/${this.current_user.Id}`,
        )
        .then((res) => {
          if(res.data.Success){
            this.listGroup = res.data.Data;
          }
        });
    },
    viewGroup(group){
      let me = this;
      //Load danh sách user của group này
      this.$auth.Intance()
        .get(
          `${BASE_URL}GroupPosts/users?id=${group.Id}`,
        )
        .then((res) => {
          if(res.data.Success){
            me.lstUserByGroup = res.data.Data;
          }
        });
      //Lấy post của group này
      me.isShowGroup = true;
      me.group_now = group;
      me.typeShow = group.Id;
      me.loadPostInGroup()
      
    },

    loadPostInGroup(){
      let me = this;
      this.$auth.Intance()
        .get(
          `${BASE_URL}posts?user_id=${me.current_user.Id}&group_id=${me.group_now.Id}&page=1&record=${me.PAGE_SIZE}`,
        )
        .then((res) => {
          me.listPost = res.data.Data;
        })
    },
    addMember(){
      //Đánh dấu thành viên của nhóm trong listUser
      this.listUser.forEach(user => {
        user.isCheck = false;
        if(this.lstUserByGroup.find(_=>_.Id == user.Id)){
          user.isCheck = true;
        }
      })
      this.group.search = '';
      this.isShowFormAddMember = true;
    },
    editMemberGroup(){
      if(this.user.Id){
        if(this.group_now.Id){
          this.group_now.list_user = this.listUser.filter(_=>_.isCheck == true);
                    this.group.list_user = this.listUser.filter(_=>_.isCheck == true);

          this.$auth.Intance().put(`${BASE_URL}GroupPosts/edit_member`, this.group_now).then((res) => {
            if(res.data.Success){
              this.isShowFormAddMember = false;
            }else{
              this.showNotification(
                "Có lỗi rồi đại vương ơi",
                "error"
              );
            }
          });
        }
      }else{
         this.showNotification(
          "Bạn phải đăng nhập để thực hiện chức năng này!",
          "error"
        );
      }
    },
    bindingUrlImage(fileName){
        return `${BASE_URL}posts/${fileName}`;
    },
    handleRemovePost(postId){
      let index = this.listPost.findIndex(_=>_.Id == postId);
      if(index > -1){
        this.listPost.splice(index, 1);
      }
    },
    scrollToBottomChatBox() {
      let container = this.$el.querySelector('#scrollingChat')
      container.scrollTop = container.scrollHeight
    },
    
    search(e) {
      this.setStringKeyWord(e.target.value);
      this.$router.push({ path: "/search" });
    },
    
    
    loadListUser() {
      this.$auth.Intance().get(
          `${BASE_URL}Users?page=${this.userPage}&record=${this.PAGE_SIZE}`,
        )
        .then((res) => {
          this.listUserContact = res.data.Data;
          this.totalUser = res.data.TotalRecord
        });
    },
   

    logout() {
      let me = this;
      this.$router.replace({ path: "/login" });
      localStorage.setItem('currentUser', null);
      me.setUser(null);
      if (this.webSocket != null) {
        this.webSocket.disconnect();
      }
      this.setDefaultForState();
      this.hideChatBox()
      me.$router.push({ path: "/login" });
    },
    
    changePass() {
      this.$router.push({ path: "/changepassword" });
    },
    changeTypeShow(type) {
      this.typeShow = type;
      this.isShowGroup=false;

      if (type == 1) {
        this.userPage = 1;
        //load list user contact
        this.loadListUser();
        this.group_now = {};
      } else {
        this.postPage = 1;
        this.group_now = {};
        //load list user contact
        this.loadListPost();
      }
    },
    changeUserPage() {
      this.loadListUser();
    },
    changePostPage() {
      this.loadListPost();
      this.scrollToTopPage();
    },
    scrollToTopPage(){
      document.getElementById("coverNewfeed").scrollTo({ top: 0, behavior: 'smooth' });
    },
    onFileChange(e) {
      let me = this;
      let imgs = Array.from(e.target.files);
      if(imgs){
        //tạm thời max 5 cái
        if(imgs.length > 5){
          let width = 100/5;
          for(let i = 0; i < 5; i++){
             me.urls.push({
               url: URL.createObjectURL(imgs[i]),
               width: width + '%'
             });
          }
          me.file = imgs.slice(0, 5);
        }else{
          let width = 100/(imgs.length);
          imgs.forEach(file => {
            me.urls.push(URL.createObjectURL(file));
             me.urls.push({
               url: URL.createObjectURL(file),
               width: width + '%'
             });
          })
          me.file = imgs;
        }
      }
    },
    share() {
      if (this.user.Id) {
        if (this.postContent != "") {
          let formData = new FormData();
          if(this.file){
            this.file.forEach(file => {
            formData.append("objFile", file);
          })
          }
          formData.append("UserId", this.user.Id);
          formData.append("Content",this.postContent);
          formData.append("FullName",this.user.FullName);
          if(this.group_now.Id){
            formData.append("GroupId",this.group_now.Id);
          }
          this.$auth.Intance()
            .post(`${BASE_URL}posts`, formData,
              {headers: {
                "Content-Type": "multipart/form-data",
              },}
            )
            .then((res) => {
              if(res.data.Success){
                this.postContent = "";
                this.file = null;
                this.urls = [];
                this.showNotification("Chia sẻ thành công", "success");
                if(this.typeShow != 0 && this.typeShow != 1){
                  this.loadPostInGroup()
                }else
                  this.loadListPost();
              }
            })
            .catch((e) => {
              console.log(e);
            });
        } else {
          this.showNotification(
            "Vui lòng nhập nội dung bạn muốn chia sẻ!",
            "error"
          );
        }
      } else {
        this.showNotification(
          "Bạn phải đăng nhập để thực hiện chức năng này!",
          "error"
        );
      }
    },
    goToProfie(id) {
      this.$router.push({ name: "profile", params: { id: id } });
    },

    showNotification(message, type) {
      this.$notification[type]({
        message,
        duration: 2,
      });
    },
    addGroup(){
      if(this.listUser){
        this.listUser.forEach((user)=>{
          user.isCheck = false;
        })
      }
      this.isShowFormAddGroup = true;
    },
    createGroup(){
      if(this.user.Id){
        if(this.group.GroupName){
          this.group.UserId = this.user.Id;
          this.group.list_user = this.listUser.filter(_=>_.isCheck == true);
          this.$auth.Intance().post(`${BASE_URL}GroupPosts`, this.group).then((res) => {
            if(res.data.Success){
              this.isShowFormAddGroup = false;
              this.listGroup.push(res.data.Data);
            }else{
              this.showNotification(
                "Có lỗi rồi đại vương ơi",
                "error"
              );
            }
          });
        }else{
          this.showNotification(
            "Tên nhóm không được bỏ trống",
            "error"
          );
        }
      }else{
         this.showNotification(
          "Bạn phải đăng nhập để thực hiện chức năng này!",
          "error"
        );
      }
    }
  },
  computed: {
    ...mapState({
      user: (state) => state.user.user,
      webSocket: (state) => state.user.webSocket,
      userOnline: (state) => state.user.userOnline,
      postNotify: (state) => state.user.postNotify,
      showPostNotify: (state) => state.user.showPostNotify
    }),
    searchUserInGroup(){
      if(this.listUser && this.group.search){
        return this.listUser.filter(_=>_.FullName.toLowerCase().includes(this.group.search.toLowerCase()));
      }else{
        return this.listUser;
      }
    }
  },
  watch: {
    userOnline(newVal) {
      this.listUser.forEach((user) => {
        let isOnline = Object.values(newVal).find(
          (userId) => userId == user.Id
        );
        // user.isOnline = isOnline ? true : false;
        this.$set(user, 'isOnline', isOnline ? true : false)
      });
    },
  },
};
</script>

<style>
@import "../assets/css/newfeed.css";
@import "../assets/css/search.css";

.my-group-name{
  width: 136px;
    overflow: hidden;
    display: inline-block;
    text-overflow: ellipsis;
    white-space: nowrap;
}
.avatar2 img, .avatar img{
  width: 100%;
  height: 100%;
  border-radius: 50%;
}
.modal {
    position: fixed;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    display: flex;
}
.modal-overlay {
    position: absolute;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.4);
}
.modal-body {
    width: 500px;
    height: 500px;
    margin: auto;
    background: #ffffff;
    position: relative;
    z-index: 1;
    border-radius: 5px;
}
.title-register {
    height: 50px;
    display: flex;
    justify-content: center;
    align-items: center;
    border-bottom: 1px solid;
}
.title {
    width: 93%;
    height: 60%;
    background-color: #ffffff;
    display: flex;
}
.register-name {
    width: 80%;
    display: flex;
    flex-direction: column;
}
.in-register {
    height: 20px;
    display: flex;
    justify-content: center;
    align-items: center;
}
.form-register {
  height: 400px;
}
.in-register {
    height: 50px;
    display: flex;
    justify-content: center;
    align-items: center;
    margin-top: 10px;
}
.register-btn {
    background-color: #00A400;
    width: 140px;
    height: 40px;
    border-radius: 5px;
    border: 1px none;
    display: flex;
    align-items: center;
    justify-content: center;
}
.register-btn:hover {
    background-color: rgb(72, 226, 41);
    cursor: pointer;
}
.left-register .group-content input{
  width: 96% !important;
  margin-left: 2% !important;
}
.list-check{
  height: 275px;
  max-height: 275px;
  overflow-y: auto;
  width: 480px;
  margin: 0px 0px 10px 10px;
  border: 1px solid #ccc;
}
.list-check.add-member{
  height: 330px;
  max-height: 330px;
}
.check-list-item{
  width: 100%;
  display: flex;
  height: 50px;
  justify-content: flex-start;
  align-items: center;
  border-bottom: 1px solid #ccc;
  width: 96%;
  margin-left: 2%;
}
.check-list-item .check-box{
  width: 50px;
  height: 50px;
  display: flex;
  align-items: center;
  justify-content: center;
}
.check-list-item .check-box input{
  width: 50%;
  margin: 10px;
}
.check-list-item .user-info,
.header-list{
  display: flex;
  justify-content: space-between;
  align-items: center;
}
.user-info .user-name{
  margin-left: 10px !important;
}
.header-list input{
  width: 50%;
  margin: 10px;
}
.checkmark:after {
  left: 9px;
  top: 5px;
  width: 5px;
  height: 10px;
  border: solid white;
  border-width: 0 3px 3px 0;
  -webkit-transform: rotate(45deg);
  -ms-transform: rotate(45deg);
  transform: rotate(45deg);
}
.cover-group .share-box{
  width: 100% !important;
  margin: 0px !important;
}
.cover-group .post-box{
  width: 100% !important;
  margin: 30px 0px 0px 0px  !important;
}
.cover-group .btn-add{
  width: 180px;
}
</style>
