<template>
    <div
        class="user-box"
    >
        <div
        class="avatar-user-if"
        >
            <img :src="bindingUrlImage(user.Id + '_.jpg')" class="avatar-real"/>
        </div>
        <div class="infor-user">
        <div class="name-user" @click="goToProfie(user.Id)">
            {{user.FullName}}
        </div>
        <div class="gender-user">{{user.GenderString}}</div>
        <div class="address-user">{{user.Address}}</div>
        </div>
    </div>
</template>

<script>
    import { BASE_URL } from "../configs/index";
    export default {
        props:{
            user:{
                type: Object,
                default: null
            }
        },
        methods:{
            bindingUrlImage(fileName){
                return `${BASE_URL}posts/${fileName}`;
            },
            goToProfie(id) {
                this.$router.push({ name: "profile", params: { id: id } });
            },
        }
    }
</script>

<style>
.user-box {
    display: flex;
    flex-direction: row;
    align-items: center;
    width: 49%;
    height: 90px;
    background-color: white;
    padding: 0px 16px;
    border-radius: 4px;
    margin-bottom: 10px;
}

.user-box .avatar-user-if {
    width: 60px;
    height: 60px;
    background-size: contain;
    border-radius: 50%;
    margin-right: 8px;
    cursor: pointer;
}
.avatar-real{
    width: 60px;
    height: 60px;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    border-radius: 50%;
    margin-right: 8px;
}
.user-box .infor-user {
    width: calc(100% - 68px);
    height: 80px;
    display: flex;
    flex-direction: column;
    justify-content: center;
}
</style>