<template>
  <div class="cover-login">
    <div class="modal">
      <div class="cover-dialog"></div>
      <div class="modal-body">
        <div class="title-register">
          <div class="title">
            <div class="register-name">
              <span style="font-size: 25px"> <b>Thông tin thành viên</b> </span>
              <span style="opacity: 0.8">Thông tin chi tiết của thành viên</span>
            </div>
            <div class="register-cancel">
              <div class="cancel-icon" @click="closeDialog()"></div>
            </div>
          </div>
        </div>
        <div class="form-register">
          <div class="left-register">
            <input  v-model="userInfo.FullName" placeholder="Họ tên" />
            <a-date-picker
              v-model="userInfo.Birthday"
              class="date-of-birth"
              placeholder="Ngày sinh"
              :format="dateFormat"
            />
            <div class="gender-register">
              <div style="font-weight: 700">Giới tính</div>
              <a-radio-group
                name="radioGroup"
                :default-value="1"
                 v-model="userInfo.Gender"
              >
                <a-radio :value="0"> Nam </a-radio>
                <a-radio :value="1" style="margin-left: 8px"> Nữ </a-radio>
                <a-radio :value="2" style="margin-left: 8px"> Khác </a-radio>
              </a-radio-group>
            </div>
            <input
              v-model="userInfo.PhoneNumber"
              placeholder="Số điện thoại"
            />
            <input v-model="userInfo.Email" placeholder="Email" />
            <input v-model="userInfo.Address" placeholder="Địa chỉ" />
            <input v-model="userInfo.Department" placeholder="Phòng ban" />
            <input v-model="userInfo.Position" placeholder="Vị trí" />
          </div>
          <div class="right-register">
            <input v-model="userInfo.UserName" placeholder="Tên đăng nhập" />
            <div
              class="email-input"
              style="witch: 91%; height: 37px; margin: 12px 0px 0px 14px"
            >
              <input
                id="password1"
                style="width: 100%; height: 100%"
                class="input-name"
                type="password"
                placeholder="Mật khẩu"
                v-model="userInfo.Password"
              />
              <div
                @click="handleHidePass1()"
                class="show-pass"
                :class="{ 'hide-pass': isHidePass1 }"
              ></div>
            </div>
            <div class="image-button">
              <div class="icon-image"></div>
              <div class="text-image">Ảnh đại diện</div>
            </div>
            <input
              class="hide-input-file"
              type="file"
              @change="onFileChange"
              accept=".jpg"
            />
            <div class="cover-avatar-antd">
              <div id="preview">
                <img v-if="url" :src="url" />
              </div>
            </div>
          </div>
        </div>
        <div class="in-register">
          <div class="register-btn" @click="signUp()">
            <span style="color: #ffffff; font-size: 20px"> <b>Cất</b></span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions } from "vuex";
import axios from "axios";
import { BASE_URL } from "../configs/index";
export default {
    props:{
      formMode: {
        type: String,
        default: 'ADD'
      },
      userEdit:{
        type: Object,
        default: null
      }
    },
    created(){
      if(this.userEdit != null && this.formMode == 'Edit'){
        this.userInfo = {...this.userEdit}
        this.url = this.bindingUrlImage(this.userEdit.Id)
      }
    },
    data(){
        return{
            dateFormat: "DD/MM/YYYY",
            url:null,
            file:null,
            isHidePass1:true,
            userInfo:{
              FullName: "",
              Birthday: null,
              Gender: 0,
              Address: "",
              Email: "",
              UserName: "",
              Password: "",
              PhoneNumber: "",
              Position: "",
              Department: ""
            }
        }
    },
    methods:{
      ...mapActions("user", [
        "openChatBox",
        "loadListMessage"
      ]),
      bindingUrlImage(fileName){
        return `${BASE_URL}posts/${fileName}_.jpg`;
      },
      closeDialog(){
        this.$emit('closeDialog')
      },
      handleHidePass1() {
        this.isHidePass1 = !this.isHidePass1;
        let passwordField = document.querySelector("#password1");
        if (!this.isHidePass1) {
          passwordField.setAttribute("type", "text");
        } else passwordField.setAttribute("type", "password");
      },
      onFileChange(e) {
        this.file = e.target.files[0];
        this.url = URL.createObjectURL(this.file);
      },
      signUp() {
        if(this.userInfo && this.userInfo.UserName && this.userInfo.Password){
          if(this.formMode == 'Add'){
            let formData = new FormData();
          formData.append("objFile",  this.file);
          for(let prop in this.userInfo){
            if(prop == 'Birthday'){
              let dateTemp = new Date(this.userInfo[prop]);
              this.userInfo[prop] = dateTemp.getFullYear() + "/" + (dateTemp.getMonth() + 1) + "/" + (dateTemp.getUTCDate());
            }
            formData.append(prop,  this.userInfo[prop]);
          }
          axios
            .post(`${BASE_URL}Users`, formData,
              {headers: {
                    "Content-Type": "multipart/form-data",
                  },})
            .then((res) => {
              if(res && res.data.Success){
                this.UserName = res.data.Data.UserName;
                this.Password = res.data.Data.Password;
                this.showNotification("Đăng ký thành công","success");
                this.$emit("saveSuccess");
                this.closeDialog();
              }else if(res){
                this.showNotification(res.data.Message,"error");
              }
            })
            .catch((e) => {
              console.log(e);
              this.showNotification("Có lỗi sảy ra!", "error");
            });
          }else{
            axios
            .put(`${BASE_URL}Users/edit`, this.userInfo)
            .then((res) => {
              if(res && res.data.Success){
                this.showNotification("Sửa thành công","success");
                this.$emit("saveSuccess");
                this.closeDialog();
              }else if(res){
                this.showNotification(res.data.Message,"error");
              }
            })
            .catch((e) => {
              console.log(e);
              this.showNotification("Có lỗi sảy ra!", "error");
            });
          }
        }else{
          this.showNotification("Vui lòng nhập đầy đủ thông tin!", "error");
        }
      },
      showNotification(message, type) {
        this.$notification[type]({
            message,
            duration: 2,
        });
      },
    }
};
</script>

<style>
@import "../assets/css/login.css";
.cover-dialog {
  position: absolute;
  height: 100vh;
  width: 100vw;
  top: 0;
  left: 0;
  background-color: black;
  opacity: 0.3;
  z-index: 1;
}
#preview img{
  width: 170px;
  height: 200px;
}
</style>