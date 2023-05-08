<template>
    <div class="cover-forgot-pass">
        <div class="cover-change-pass">
            <div class="cover-verify">
            <!-- header -->
            <div class="header">
                <a class="header-logo" href="../html/Login.html">
                    <span style="font-size:30px; color:  #2181e7;"> <b>MyLo</b></span>
                </a>
            </div>

            <!-- body -->
            <div class="body">
                <div class="form-Forgot_password">
                    <div class="title-forgot">
                        <span style="font-size:25px; color: #000000;margin: auto 0px;"> <b>Nhập mã xác nhận</b></span>
                    </div>
                    <div class="input-verify">
                        <input 
                            type="text" 
                            class="verify" 
                            placeholder="Mã xác nhận đã gửi đến email của bạn"
                            v-model="inputNumber"
                        >
                    </div>
                    <div class="btn-forgot">
                        <router-link to="/login">
                            <div class="btn-cancel">
                                Hủy
                            </div>
                        </router-link>
                        <div @click="confirmVerifyNumber()" class="btn-confirm" style="margin-top:0px; margin-right:21px">
                            Xác nhận
                        </div>
                    </div>
                </div>
            </div>

            <!-- footer -->
            <div class="footer">
                <h3 class="footer-name">
                    Bản quyền thuộc về @MyLo
                </h3>
            </div>   
            </div>
        </div>
    </div>
</template>

<script>
    import {mapState} from 'vuex'
    export default {
        name:'Verify',
        data(){
            return{
                inputNumber:''
            }
        },
        methods:{
            confirmVerifyNumber(){
                if(this.inputNumber == ''){
                    this.showNotification("Vui lòng nhập mã xác nhận đã được gửi đến email của bạn!","error")
                }else if(this.inputNumber != this.verifyNumber){
                    this.showNotification("Mã xác nhận sai, vui lòng kiểm tra lại trong email của bạn!","error")
                }else{
                    this.$router.push({path:'/createpassword'})
                }
            },
            showNotification(message, type) {
                this.$notification[type]({
                    message,
                    duration: 2,
                });
            }
        },
        computed:{
            ...mapState({
            verifyNumber:state =>state.user.verifyNumber
            })
        }
    }
</script>

<style>
@import "../assets/css/verify.css";
@import "../assets/css/forgotpassword.css";
@import "../assets/css/changepassword.css";
</style>