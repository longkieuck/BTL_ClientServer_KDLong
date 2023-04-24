<template>
  <div class="cover-login">
    <!-- header -->
    <div class="header"></div>
    
    <!-- body -->
    <div class="body">
      <div class="container">
        <div class="container-title">
          <div class="title-name">
            <div class="name">

              <h1 style="font-size: 55px;color: #2181e7">FakeBook Mạng xã hội</h1>
            </div>
            <div class="imformation-name">
              <div class="flex">
                <h1 class="name-details">
                  Lan tỏa văn hóa doanh nghiệp
                </h1>
              </div>
            </div>
          </div>
        </div>
        <div class="container-login">
          <div class="form-login">
            <div class="login">
              <div class="email-input">
                <input
                  class="input-name"
                  type="text"
                  placeholder="Tài khoản"
                  v-model="UserName"
                  @keyup.enter="login()"
                />
              </div>
            </div>
            <div class="login">
              <div class="email-input">
                <input
                  id="password"
                  style="width: 81%"
                  class="input-name"
                  type="password"
                  placeholder="Mật khẩu"
                  v-model="Password"
                  @keyup.enter="login()"
                />
                <div
                  @click="handleHidePass()"
                  class="show-pass"
                  :class="{ 'hide-pass': isHidePass }"
                ></div>
              </div>
            </div>
            <div class="login">
              <div class="btn-login" @click="login()">
                <span style="font-size: 20px; color: #ffffff"
                  ><b>Đăng nhập</b></span
                >
              </div>
            </div>
            <div class="login">
              <div class="login-forgot">
                <router-link to="/forgotpassword" class="forgot-pass">
                  Quên mật khẩu?
                </router-link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- footer -->
    <div class="footer">
      <h3 class="footer-name">Bản quyền thuộc về @FakeBook</h3>
    </div>
     <!-- modal -->
  </div>
</template>

<script>
// import axios from "axios";
// import { BASE_URL } from "../configs/index";
import { mapActions } from "vuex";
export default {
  name: "Login",
  methods: {
    ...mapActions("user", ["setUser"]),
    showFormRegister() {
      this.isShowFormRegister = true;
    },
    hideFormRegister() {
      this.isShowFormRegister = false;
    },
    async login() {
      const me = this;
      if (me.UserName && me.Password) {
        let params = {
          UserName : me.UserName,
          Password : me.Password
        }
        let res = await this.$auth.loginUser(params);
        if (res && res.Data && res.Data["user_data"]) {
          let userData = res.Data["user_data"][0]; 
          delete userData.Password;
          this.setUser(userData);
          localStorage.setItem("currentUser", JSON.stringify(userData));
          if(this.UserName == 'admin'){
            this.$router.push({ path: "/admin" });
          }else{
            this.$router.push({ path: "/newsfeed" });
          }
        } else {
          this.showNotification(res.Message, "error");
          this.UserName = "";
          this.Password = "";
        }
        
      } else {
        this.showNotification(
          "Vui lòng nhập đầy đủ Tài khoản và Mật khẩu!",
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
    handleHidePass() {
      this.isHidePass = !this.isHidePass;
      let passwordField = document.querySelector("#password");
      if (!this.isHidePass) {
        passwordField.setAttribute("type", "text");
      } else passwordField.setAttribute("type", "password");
    },
    onFileChange(e) {
      this.file = e.target.files[0];
      this.url = URL.createObjectURL(this.file);
    },
  },
  data() {
    return {
      isShowFormRegister: false,
      UserName: "",
      Password: "",
      isHidePass: true,
      dateFormat: "DD/MM/YYYY",
      url: null,
      file: null,
    };
  },
};
</script>

<style scoped>
@import "../assets/css/login.css";
#preview {
  display: flex;
  justify-content: center;
  align-items: center;
}

#preview img {
  max-width: 100%;
  max-height: 200px;
}
</style>
