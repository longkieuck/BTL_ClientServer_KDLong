<template>
  <div class="cover-newfeed cover-search" id="coverSearch">
    <Header/>

    <div class="content">
      <div class="left-content">
        <div class="filter-title">
          <div>
            Điều kiện lọc:
          </div>
          <div class="string-key-word">{{ stringKeyWord }}</div>
          <div
            class="x-stringkw"
            v-if="stringKeyWord != ''"
            @click="removeStringKeyWord()"
          >
            x
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
              Tất cả
            </div>
          </div>
        </div>
        <div
          @click="changeTypeShow(1)"
          class="left-content-item"
          :class="{ 'active-item': typeShow == 1 }"
        >
          <div class="news-icon2"></div>
          <div class="item-descrip">
            <div class="user-name">
              Bài viết
            </div>
          </div>
        </div>
        <div
          @click="changeTypeShow(2)"
          class="left-content-item"
          :class="{ 'active-item': typeShow == 2 }"
        >
          <div class="group-icon"></div>
          <div class="item-descrip">
            <div class="user-name">
              Liên hệ
            </div>
          </div>
        </div>
      </div>
      <div class="main-content">
        <div
          class="cover-list-user"
          v-if="typeShow != 1 && listUser.length > 0"
        >
          <div class="post-box list-user-box">
            <UserInfoBox 
              v-for="userSearch in listUser"
              :key="userSearch.Id"
              :user="userSearch"
              />
          </div>
          <div class="cover-pagination-post" style="margin-left: 200px">
            <a-pagination
              class="pagination-post"
              v-model="userPage"
              :total="totalUser"
              :page-size="PAGE_SIZE"
              @change="changeUserPage"
            />
          </div>
        </div>
        <div
          class="cover-list-post"
          v-if="typeShow != 2 && listPost.length > 0"
        >
          <PostBox v-for="(post, index) in listPost" :key="index" :post="post" @removePost="handleRemovePost"/>
          <div class="cover-pagination-post" style="margin-left: 200px">
            <a-pagination
              class="pagination-post"
              v-model="postPage"
              :total="totalPost"
              :page-size="PAGE_SIZE"
              @change="changePostPage"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { mapState, mapActions } from "vuex";
import { BASE_URL,PAGE_SIZE_CONST } from "../configs/index";
// import moment from "moment";
import Header from '../controls/Header.vue'
import UserInfoBox from '../controls/UserInfoBox.vue'
import PostBox from '../controls/PostBox.vue'
export default {
  name: "Search",
  components:{
    Header,
    UserInfoBox,
    PostBox
  },
  created() {
    let me = this;
          let userData = localStorage.getItem('currentUser');
          if((!me.user || (me.user && !me.user.Id)) && userData){
            me.currentItem = JSON.parse(userData);
            me.setUser(me.currentItem);
          }
    this.loadListPost()
    this.loadListUser()
  },
  data() {
    return {
      currentItem:{},
      listUser: [],
      listPost: [],
      userPage: 1,
      totalUser: null,
      postPage: 1,
      totalPost: null,
      PAGE_SIZE: PAGE_SIZE_CONST,
      typeShow: 0, //0 tất cả,1 bài viết,2 liên hệ
    };
  },
  methods: {
    ...mapActions("user", [
      "setStringKeyWord",
      "setDefaultForState",
      "setUser"
    ]),
    handleRemovePost(postId){
      let index = this.listPost.findIndex(_=>_.Id == postId);
      if(index > -1){
        this.listPost.splice(index, 1);
      }
    },
    removeStringKeyWord() {
      this.setStringKeyWord("");
      this.afterSearch();
    },
    goToNewFeed() {
      this.$router.push({ path: "/newfeed" });
    },
    goToProfie(id) {
      this.$router.push({ name: "profile", params: { id: id } });
    },

    changeTypeShow(type) {
      this.typeShow = type;
    },
 
    loadListPost() {
      axios
        .get(
          `${BASE_URL}posts?user_id=${this.user.Id}&search=${this.stringKeyWord}&page=${this.postPage}&record=${this.PAGE_SIZE}`,
        )
        .then((res) => {
          this.listPost = res.data.Data;
          this.totalPost = res.data.TotalRecord
        })
        
    },
    loadListUser() {
      axios
        .get(
          `${BASE_URL}Users?search=${this.stringKeyWord}&page=${this.userPage}&record=${this.PAGE_SIZE}`,
        )
        .then((res) => {
          this.listUser = res.data.Data;
          this.totalUser = res.data.TotalRecord
        });
    },
    changePostPage() {
      this.loadListPost();
      document.getElementById("coverSearch").scrollTop = 0;
    },
    changeUserPage() {
      this.loadListUser();
    },
    showNotification(message, type) {
      this.$notification[type]({
        message,
        duration: 2,
      });
    },
   
  },
  computed: {
    ...mapState({
      stringKeyWord: (state) => state.user.stringKeyWord,
      user: (state) => state.user.user,
    }),
  },
  watch:{
    stringKeyWord(){
      this.loadListUser()
      this.loadListPost()
    }
  }
};
</script>
<style>
@import "../assets/css/newfeed.css";
@import "../assets/css/search.css";
</style>
