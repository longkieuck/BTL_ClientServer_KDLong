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
                <span style="font-size:25px; color: #000000;margin: auto 0px;"> <b>Tạo mật khẩu mới</b></span>
            </div>
            <div class="cover-login">
                <div class="login" style="height: 45px; margin-top: 30px;">
                    <div class="email-input">
                        <input
                            id="password2"
                            style="width:81%"
                            class="input-name"
                            type="password"
                            placeholder="Nhập mật khẩu mới"
                            v-model="pass1"
                        />
                        <div
                            @click="handleHidePass2()"
                            class="show-pass"
                            :class="{ 'hide-pass': isHidePass2 }"
                        ></div>
                    </div>
                </div>
                <div class="login" style="height: 45px; margin-bottom: 20px;">
                    <div class="email-input">
                        <input
                            id="password3"
                            style="width:81%"
                            class="input-name"
                            type="password"
                            placeholder="Nhập lại mật khẩu mới"
                            v-model="pass2"
                        />
                        <div
                            @click="handleHidePass3()"
                            class="show-pass"
                            :class="{ 'hide-pass': isHidePass3 }"
                        ></div>
                    </div>
                </div>
            </div>
            
            <div class="btn-forgot">
             
                <div class="btn-cancel" @click="goToLogin()">
                    Hủy
                </div>

                <div class="btn-confirm" @click="confirmCreatePassWord()">
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
    import {BASE_URL} from '../configs/index'
    import axios from 'axios'
    import {mapState,mapActions} from 'vuex'
    export default {
        name:'CreatePassword',
        data(){
            return{
                pass1:'',
                pass2:'',
                isHidePass2: true,
                isHidePass3: true,
            }
        },
        computed:{
            ...mapState({
                user:state =>state.user.user
            })
        },
        methods:{
            ...mapActions("user", [
                "setDefaultForState"
            ]),
            goToLogin(){
                this.$router.replace({path:'/login'})
            },
            confirmCreatePassWord(){
                if(this.pass1 == '' || this.pass2 == ''){
                    this.showNotification("Yêu cầu nhậu đủ thông tin!","error")
                }
                else if(this.pass1 != this.pass2){
                    this.showNotification("Mật khẩu chưa khớp!","error")
                }else{
                    axios.post(`${BASE_URL}Users/change_password`, {
                        id: this.user.Id,
                        password: this.user.Password,
                        newPassword: this.pass1
                    })
                    .then(res=>{
                        if(res.data.Success){
                            this.showNotification("Lấy lại mật khẩu thành công!","success")
                            this.setDefaultForState()
                            this.$router.replace({path:'/login'})
                        }
                    })
                    .catch(e =>{
                        console.log(e)
                        this.showNotification("Chưa bật server!","error")
                    })
                    
                }
            },
            showNotification(message, type) {
                this.$notification[type]({
                    message,
                    duration: 2,
                });
            },
            handleHidePass2() {
                this.isHidePass2 = !this.isHidePass2;
                let passwordField = document.querySelector("#password2");
                if (!this.isHidePass2) {
                    passwordField.setAttribute("type", "text");
                } else passwordField.setAttribute("type", "password");
            },
             handleHidePass3() {
                this.isHidePass3 = !this.isHidePass3;
                let passwordField = document.querySelector("#password3");
                if (!this.isHidePass3) {
                    passwordField.setAttribute("type", "text");
                } else passwordField.setAttribute("type", "password");
            },
        },
    }
</script>

<style>
@import "../assets/css/login.css";
@import "../assets/css/forgotpassword.css";
@import "../assets/css/changepassword.css";
</style>