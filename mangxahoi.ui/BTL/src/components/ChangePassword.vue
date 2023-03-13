<template>
  <div class="cover-forgot-pass">
    <div class="cover-change-pass">
      <!-- header -->
      <div class="header">
        <a class="header-logo" href="../html/Login.html">
          <span style="font-size:30px; color:  #2181e7;"> <b>fakebook</b></span>
        </a>
      </div>

      <!-- body -->
      <div class="body">
        <div class="form-Forgot_password">
          <div class="title-forgot">
            <span style="font-size:25px; color: #000000;margin: auto 0px;">
              <b>Thay đổi mật khẩu mới</b></span
            >
          </div>
          <div class="cover-login">
            <div class="login" style="height: 45px;">
              <div class="email-input">
                <input
                  id="password4"
                  style="width:81%"
                  class="input-name"
                  type="password"
                  placeholder="Nhập mật khẩu cũ"
                  v-model="oldPass"
                />
                <div
                  @click="handleHidePass4()"
                  class="show-pass"
                  :class="{ 'hide-pass': isHidePass4 }"
                ></div>
              </div>
            </div>
            <div class="login" style="height: 45px;">
              <div class="email-input">
                <input
                  id="password5"
                  style="width:81%"
                  class="input-name"
                  type="password"
                  placeholder="Nhập mật khẩu mới"
                  v-model="newPass1"
                />
                <div
                  @click="handleHidePass5()"
                  class="show-pass"
                  :class="{ 'hide-pass': isHidePass5 }"
                ></div>
              </div>
            </div>
            <div class="login" style="height: 45px;">
              <div class="email-input">
                <input
                  id="password6"
                  style="width:81%"
                  class="input-name"
                  type="password"
                  placeholder="Nhập lại mật khẩu mới"
                  v-model="newPass2"
                />
                <div
                  @click="handleHidePass6()"
                  class="show-pass"
                  :class="{ 'hide-pass': isHidePass6 }"
                ></div>
              </div>
            </div>
          </div>

          <!-- <div class="input-pass">
                <input type="text" class="pass" style="margin-top: 15px;" placeholder="Mật khẩu cũ">
                <input type="text" class="pass" placeholder="Mật khẩu mới" >
                <input type="text" class="pass" placeholder="Nhập lại mật khẩu mới">
            </div> -->
          <div class="btn-forgot">
            <div @click="goToNewsFeed()" class="btn-cancel">
              Hủy
            </div>
            <div @click="confirmChangePassWord()" class="btn-confirm">
              Xác nhận
            </div>
          </div>
        </div>
      </div>

      <!-- footer -->
      <div class="footer">
        <h3 class="footer-name">
          Bản quyền thuộc về @Fakebook
        </h3>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapActions } from "vuex";
// import axios from "axios";
import { BASE_URL } from "../configs/index";
export default {
  name: "ChangePassword",
  data() {
    return {
      oldPass: "",
      newPass1: "",
      newPass2: "",
      isHidePass4: true,
      isHidePass5: true,
      isHidePass6: true,
      instance : null
    };
  },
  created(){
    if(!this.$auth.Intance()){
      this.$router.push({ path: "/login" })
    }
  },
  methods: {
    ...mapActions("user", [
      "setDefaultForState"
    ]),
    goToNewsFeed() {
      this.$router.replace({ path: "/newsfeed" });
    },
    confirmChangePassWord() {
      if (this.user.Id) {
        if (this.oldPass == "" || this.newPass1 == "" || this.newPass2 == "") {
          this.showNotification("Yêu cầu nhậu đủ thông tin!", "error");
          return;
        }

        if (this.newPass1.length < 6 || this.newPass2.length < 6) {
          this.showNotification(
            "Độ dài tối thiểu 6 ký tự!",
            "error"
          );
          return;
        }
        if (this.newPass1 != this.newPass2) {
          this.showNotification(
            "Nhập lại mật khẩu chưa khớp với mật khẩu mới!",
            "error"
          );
          return;
        }
        if (this.oldPass == this.newPass2) {
          this.showNotification(
            "Mật khẩu mới không được giống với mật khẩu cũ!",
            "error"
          );
          return;
        }

        this.$auth.Intance()
          .post(`${BASE_URL}Users/change_password`, {
            id: this.user.Id,
            password: this.oldPass,
            newPassword: this.newPass1
          })
          .then((res) => {
            if(res.data.Success == true){
              this.showNotification("Đổi mật khẩu thành công!", "success");
              this.$router.replace({ path: "/login" });
              this.setDefaultForState()
            }else{
                if (this.oldPass != this.user.passWord) {
                  this.showNotification("Mật khẩu cũ không chính xác!", "error");
                return;
              } //passWord
            }
            
          })
          .catch((e) => {
            console.log(e);
            this.showNotification("Chưa bật server!", "error");
          });
      } else {
        this.showNotification(
          "Bạn phải đăng nhập để thực hiện chức năng này!",
          "error"
        );
      }
    },
    showNotification(message, type) {
      this.$notification[type]({
        message,
        duration: 2,
      });
    },
    handleHidePass4() {
      this.isHidePass4 = !this.isHidePass4;
      let passwordField = document.querySelector("#password4");
      if (!this.isHidePass4) {
        passwordField.setAttribute("type", "text");
      } else passwordField.setAttribute("type", "password");
    },
    handleHidePass5() {
      this.isHidePass5 = !this.isHidePass5;
      let passwordField = document.querySelector("#password5");
      if (!this.isHidePass5) {
        passwordField.setAttribute("type", "text");
      } else passwordField.setAttribute("type", "password");
    },
    handleHidePass6() {
      this.isHidePass6 = !this.isHidePass6;
      let passwordField = document.querySelector("#password6");
      if (!this.isHidePass6) {
        passwordField.setAttribute("type", "text");
      } else passwordField.setAttribute("type", "password");
    },
  },
  computed: {
    ...mapState({
      user: (state) => state.user.user,
    }),
  },
};
</script>

<style scoped>
@import "../assets/css/login.css";
@import "../assets/css/forgotpassword.css";
@import "../assets/css/changepassword.css";
</style>
