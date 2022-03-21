<template>
  <div
    class="cover-profile cover-newfeed"
    style="overflow-y: unset; background: #F0F2F5; height: auto;"
  >
    <!-- header -->
    <Header style="right:0px"/>
    <div class="body">
      <div class="body-top">
        <div class="top-image">
          <div class="image-daidien">
            <div class="daidien">
              <img :src="bindingUrlImage(userProfile.Id + '_.jpg')" style="width: 170px;"/>
            </div>
          </div>
          <div class="top-name">
            <div class="name">
              <div class="name-right" style="margin-left: -20px;">
                <span style="font-size: 20px; color: #1890ff;"
                  ><b>{{ userProfile.FullName }}</b></span
                >
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="body-bottom">
        <div class="bottom-center">
          <div class="information">
            <div class="information-in">
              <div class="in-box" style="height:40px">
                <span> <b>Thông tin chung</b> </span>
              </div>
              <div class="in-box">
                <div class="box-name">
                  <div class="name-icon1"></div>
                  <div class="name-infor">
                    <span style="font-size: 15px; margin: auto 0px;"
                      >Ngày sinh</span
                    >
                  </div>
                </div>
                <div class="box-infor">
                  <div class="infor-title">
                    <span
                      style="font-size: 15px; margin: auto 0px; color: #005ABD;"
                      >{{ userProfile.Birthday | filterDate}}</span
                    >
                  </div>
                </div>
              </div>
              <div class="in-box">
                <div class="box-name">
                  <div class="name-icon2"></div>
                  <div class="name-infor">
                    <span style="font-size: 15px; margin: auto 0px;"
                      >Số điện thoại</span
                    >
                  </div>
                </div>
                <div class="box-infor">
                  <div class="infor-title">
                    <span
                      style="font-size: 15px; margin: auto 0px; color: #005ABD;"
                      >{{ userProfile.PhoneNumber }}</span
                    >
                  </div>
                </div>
              </div>
              <div class="in-box">
                <div class="box-name">
                  <div class="name-icon3"></div>
                  <div class="name-infor">
                    <span style="font-size: 15px; margin: auto 0px;"
                      >Email</span
                    >
                  </div>
                </div>
                <div class="box-infor">
                  <div class="infor-title">
                    <span
                      style="font-size: 15px; margin: auto 0px; word-break: break-all;"
                    >
                      <a style="text-decoration: none;">{{
                        userProfile.Email
                      }}</a>
                    </span>
                  </div>
                </div>
              </div>
              <div class="in-box">
                <div class="box-name">
                  <div class="name-icon4"></div>
                  <div class="name-infor">
                    <span style="font-size: 15px; margin: auto 0px;"
                      >Giới tính</span
                    >
                  </div>
                </div>
                <div class="box-infor">
                  <div class="infor-title">
                    <span
                      style="font-size: 15px; margin: auto 0px; color: #005ABD;"
                      >{{ userProfile.Gender | filterGender }}</span
                    >
                  </div>
                </div>
              </div>
              <div class="in-box">
                <div class="box-name">
                  <div class="name-icon5"></div>
                  <div class="name-infor">
                    <span style="font-size: 15px; margin: auto 0px;"
                      >Địa chỉ</span
                    >
                  </div>
                </div>
                <div class="box-infor">
                  <div class="infor-title">
                    <span
                      style="font-size: 15px; margin: auto 0px; word-break: break-all; color: #005ABD;"
                      >{{ userProfile.Address }}</span
                    >
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="personal">
            <PostBox v-for="(post, index) in listPost" :key="index" :post="post" @removePost="handleRemovePost"/>
            <div class="cover-pagination-post">
              <a-pagination
                class="pagination-post"
                v-model="postPage"
                :total="totalPost"
                :page-size="PAGE_SIZE"
                @change="changePagePost"
              />
            </div>
          </div>
          
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapActions } from "vuex";
import axios from "axios";
import { BASE_URL,PAGE_SIZE_CONST } from "../configs/index";
import moment from "moment";
import Header from '../controls/Header.vue'
import PostBox from '../controls/PostBox.vue';
export default {
  name: "Profile",
  components:{
    Header,
    PostBox
  },
  data() {
    return {
      totalPost: 0,
      postPage: 1,
      PAGE_SIZE: PAGE_SIZE_CONST,
      listPost: [],
      listViewMore: {},
      userProfile:{},
      current_user: {}
    };
  },
  computed: {
    ...mapState({
      user: (state) => state.user.user
    }),
  },
  created() {
    //Lấy dữ liệu current user
    let userData = localStorage.getItem('currentUser');
    if(userData || this.user.Id){
      
      if(userData){
        this.current_user = JSON.parse(userData)
      }
      if (this.$route.params.id == this.user.Id) {
        this.userProfile = this.user
      } else {
        axios
          .get(
            `${BASE_URL}Users/detail?id=${this.$route.params.id}`
          )
          .then((res) => {
            this.userProfile = res.data.Data.user_data
          });
      }

      this.loadListPost();
    }else{
      this.$router.replace({ path: "/newfeed" });
    }
    //load tong so post

  },
  methods: {
    ...mapActions("user", [
      "setDefaultForState"
    ]),
    scrollToTop(){
      window.scrollTo({ top: 0, behavior: 'smooth' })
    },
    loadListPost() {
      axios
        .get(
          `${BASE_URL}Posts/post_by_id?user_id_current=${this.current_user.Id}&user_id_search=${this.$route.params.id}&page=${this.postPage}&record=${this.PAGE_SIZE}`,
        )
        .then((res) => {
          this.listPost = res.data.Data;
          this.totalPost = res.data.TotalRecord
        })
        .then(()=>this.$nextTick(()=>this.scrollToTop()));
    },
    changePagePost(){
      this.loadListPost()
    },

    handleRemovePost(postId){
      let index = this.listPost.findIndex(_=>_.Id == postId);
      if(index > -1){
        this.listPost.splice(index, 1);
      }
    },
    bindingUrlImage(fileName){
                return `${BASE_URL}posts/${fileName}`;
            },
    logout() {
      this.$router.replace({ path: "/login" });
      if (this.webSocket != null) {
        this.webSocket.emit("userOff", this.user.Id);
        this.webSocket.disconnect();
      }
      this.setDefaultForState()
    },

    goToNewFeed() {
      this.$router.push({ path: "/newfeed" });
    },
    search(e) {
      this.setStringKeyWord(e.target.value);
      this.$router.push({ path: "/search" });
    },

    showNotification(message, type) {
      this.$notification[type]({
        message,
        duration: 2,
      });
    },

  },
  filters: {
    filterGender(gender) {
      if (gender == 0) return "Nam";
      if (gender == 1) return "Nữ";
      return "Khác";
    },
    filterDate(date){
      return  moment(new Date(date)).format("DD/MM/YYYY");
    }
  },
};
</script>

<style>
@import "../assets/css/profile.css";
@import "../assets/css/newfeed.css";
.right-content-item:hover {
  background-color: white;
}
</style>
