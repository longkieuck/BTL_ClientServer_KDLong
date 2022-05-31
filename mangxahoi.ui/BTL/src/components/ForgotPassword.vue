<template>
    <div class="cover-forgot-pass">
        <div class="header">
            <a class="header-logo">
                <span style="font-size:30px; color:  #2181e7;"> <b>fakebook</b></span>
            </a>
        </div>

        <!-- body -->
        <div class="body">
            <div class="form-Forgot_password">
                <div class="title-forgot">
                    <span style="font-size:25px; color: #000000;margin: auto 0px;"> <b>Tìm tài khoản của bạn</b></span>
                </div>
                <div class="input-forgot">
                    <div class="input-tilte">
                        <span style="font-size:20px; color: #000000;">Vui lòng nhập Tài khoản và Email khôi phục  của bạn để tiếp tục.</span>
                    </div>
                    <div class="input-phone">
                        <input
                            class="phone-email" 
                            type="text" 
                            placeholder="Tài khoản"
                            v-model="username"
                        >
                        <input 
                            class="phone-email" 
                            type="text" 
                            placeholder="Email khôi phục"
                            v-model="email"
                        >
                    </div>
                       
                </div>
                <div class="btn-forgot">
                    <div class="btn-cancel">
                            <router-link to="/login" class="link-forgot">
                                <div class="btn-cancel-x">
                                <b>Hủy</b>
                                </div>
                            </router-link>
                    </div>
                    <div class="btn-seach" @click="findAccount()">
                        <div class="seach">
                            <span style="font-size: 20px; color: #ffffff;"><b>Tìm kiếm</b></span>
                        </div>
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
</template>

<script>
    import {BASE_URL} from '../configs/index'
    import {mapActions} from "vuex"
    export default {
        name:'ForgotPassword',
        data(){
            return{
                username:'',
                email:'',
            }
        },
        created(){
            if(!this.$auth.Intance()){
                this.$router.push({ path: "/login" })
            }
        },
        methods:{
            ...mapActions(
            'user',
            [
                'setVerifyNumber',
                'setUser'
            ]
            ),
            findAccount(){
                this.$auth.Intance().get(`${BASE_URL}Users/verify_email?userName=${this.username}&email=${this.email}`)
                .then(res =>{
                    if(res.data.Success == false){
                        this.showNotification("Sai Tài khoản hoặc email vui lòng nhập lại!","error")
                    }else{
                        this.setVerifyNumber(res.data.Data.verify_code)
                        this.setUser(res.data.Data.user_data)
                        this.$router.push({path:'/verify'})
                    }
                }
                )
                .catch(e =>{
                    console.log(e)
                    this.showNotification("Chưa bật server","error")
                })
            },
            showNotification(message, type) {
                this.$notification[type]({
                    message,
                    duration: 2,
                });
            },
        }
    }
</script>

<style>
@import "../assets/css/forgotpassword.css";
</style>