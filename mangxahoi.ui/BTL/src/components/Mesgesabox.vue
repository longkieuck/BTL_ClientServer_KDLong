<template>
    <div>
        <div class="modal-messagebox"></div>
        <div class="message-box">
            <div class="message-box-header flex">
                <div>{{ title }}</div>
                <div class="close-dialog" @click="closeForm"></div>
            </div>
            <div class="message-box-content">
                {{ message }}
            </div>
            <div class="message-box-footer">
                <div class="btn btn-close" v-if="formType == $enum.MessageBoxType().None">Đóng</div>
                <div class="flex jus-end w-100" v-if="formType == $enum.MessageBoxType().Confirm">
                    <div class="btn btn-no mgr-10" @click="confirm('no')">Không</div>
                    <div class="btn btn-yes" @click="confirm('yes')">Có</div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name:"MessageBox",
        props:{
            formType : {
                default : 0,
                type: Number
            },
            message : {
                type:String,
                default : ""
            },
            title : {
                type:String,
                default : ""
            }
        },
        data(){
            return{
            }
        },
        computed:{},
        methods:{
            /**
                Hàm xử lý sự kiện xác nhận
            */
            confirm(status){
                const me = this;
                let type = status == 'yes' ? me.$enum.ActionType().Delete : (status == 'no' ? me.$enum.ActionType().Cancel : me.$enum.ActionType().None);
                me.actionForm(type);
                me.closeForm()
            },
            /**
                Hàm xử lý close form Messagebox
            */
            closeForm(){
                const me = this;
                me.actionForm(me.$enum.ActionType().Close);
            },

            /**
                Hàm sử lý các sự kiện hành động của form message
            */
            actionForm(type){
                const me = this;
                me.$emit("btnClick",type);
            }
        }
    }
</script>

<style scoped>
.modal-messagebox{
    background-color: #000;
    opacity:0.5;
    position:fixed;
    top: 0;
    left: 0;
    right:0;
    bottom:0;
    z-index:1;
}
.message-box{
    position: absolute;
    z-index: 2;
    width: 500px;
    min-height: 150px;
    left: calc(50% - 230px);
    top: calc(50% - 150px);
    background-repeat: no-repeat;
    background-size: contain;
    margin: 0 2.5%;
    margin-top: 30px;
    border-radius: 4px;
    background-color:#fff;
    padding: 20px;
}
.message-box-header{
    font-weight:bold;
}
.message-box-content{
    padding-top:10px;
}
.message-box-footer{
    width: 100%;
    position:absolute;
    bottom:20px;
    padding-top:10px;
    right: 20px;
}
.close-dialog {
  background-image: url("../assets/img/icon1.svg");
  min-width: 24px;
  min-height: 24px;
  background-position: -144px -144px;
  height: 24px;
  width: 24px;
  position: absolute;
  right: 10px;
  cursor: pointer;
}
.close-dialog:hover{
    background-color: #ccc;
}

.btn{
    cursor:pointer;
    height:36px;
    padding: 5px 10px;
    border-radius:8px;
    display:flex;
    justify-content:center;
    align-items:center;
    font-weight:bold;
}
.btn-close{
    background-color:#e5e5e5;
    min-width:100px;
}
.btn-close:hover{
    background-color:#e5e5e5;
}
.btn-no{
    background-color: #e5e5e5;
    min-width:100px;
}
.btn-no:hover{
    background-color:#e5e5e5;
}
.btn-yes{
    background-color: #2181e7;
    min-width:100px;
    color: #fff;
}
.btn-yes:hover{
    background-color:#2181e7;
}
</style>
