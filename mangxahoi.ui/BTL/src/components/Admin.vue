<template>
  <div class="cover-newfeed cover-search" id="coverSearch">
    <div class="header" style="display: flex; justify-content: space-between">
      <div class="cover-logo">
        <div class="logo"></div>
        <div class="app-name">FAKEBOOK</div>
      </div>
      <search-box />
      <div class="cover-right-header-admin">
        <div class="avatar"></div>
        <div class="full-name">ADMIN</div>

        <div class="logout-icon" @click="logout()"></div>
      </div>
    </div>

    <div class="content">
      <div class="left-content">
        <div class="filter-title">
          <div>MENU</div>
        </div>
        <div
          @click="addUser()"
          class="left-content-item"
          :class="{ 'active-item': typeShow == 0 }"
        >
          <div class="add-group-icon" style="margin-left: 10px"></div>
          <div class="item-descrip">
            <div class="user-name">Thêm mới thành viên</div>
          </div>
        </div>
        <div
          @click="changeTypeShow('User')"
          class="left-content-item"
          :class="{ 'active-item': typeShow == 'User' }"
        >
          <div class="group-icon"></div>
          <div class="item-descrip">
            <div class="user-name">Danh sách thành viên</div>
          </div>
        </div>
        <div
          @click="changeTypeShow('Post')"
          class="left-content-item"
          :class="{ 'active-item': typeShow == 'Post' }"
        >
          <div class="news-icon2"></div>
          <div class="item-descrip">
            <div class="user-name">Bài viết</div>
          </div>
        </div>
      </div>
      <div class="main-content-admin" v-if="typeShow == 'User'">
        <InputSearch
          class="ip-search-admin"
          lWidth="200"
          placeholder="Tìm kiếm thành viên"
          v-model="strSearch"
          @keyup="searchHandle"
        />
        <a-table style="margin-top: 70px;" 
          bordered 
          :data-source="userData" 
          :columns="userColumns" 
          :scroll="{ x: 1750, y: 370 }"
          :pagination="pagination"
          @change="handleTableChange"
        >
          <template slot="operation" slot-scope="text, record">
            <a style="margin-right: 10px" @click="onEdit(record)">Sửa</a>
            <a-popconfirm
              ok-text="Có" 
              cancel-text="Không"
              v-if="userData.length"
              title="Bạn có chắc chắn muốn xoá thành viên này?"
              @confirm="() => onDelete(record.Id)"
            >
              <a href="javascript:;">Xoá</a>
            </a-popconfirm>

          </template>
        </a-table>
      </div>
      <div class="main-content-admin" v-else>
        <InputSearch
          class="ip-search-admin"
          lWidth="200"
          placeholder="Tìm kiếm bài viết"
          v-model="strSearch"
          @keyup="searchHandle"
        />
        <a-table style="margin-top: 70px;" 
          bordered 
          :data-source="postData" 
          :columns="postColumns" 
          :scroll="{ x: 1700, y: 370 }"
          :pagination="pagination"
          @change="handleTableChange"
        >
          <template slot="operation" slot-scope="text, record">
            <a-popconfirm
              ok-text="Có" 
              cancel-text="Không"
              v-if="postData.length"
              title="Bạn có chắc chắn muốn xoá bài đăng này?"
              @confirm="() => onDeletePost(record.Id)"
            >
              <a href="javascript:;">Xoá</a>
            </a-popconfirm>

          </template>
        </a-table>
      </div>
    </div>
    <UserInfoDialog
      v-if="isShowDialog" 
      @closeDialog="closeDialog()" 
      @saveSuccess="handleSignUpSuccess" 
      :formMode="formMode"
      :userEdit="userEdit"
    />
  </div>
</template>

<script>
// import axios from "axios";
import { mapState } from "vuex";
import { BASE_URL,PAGE_SIZE_CONST } from "../configs/index";
// import moment from "moment";
import UserInfoDialog from "../controls/UserInfoDialog.vue";
import InputSearch from "../controls/InputSearch.vue";
import _ from 'lodash'
export default {
  name: "Search",
  components: {
    UserInfoDialog,
    InputSearch
  },
  created() {
    if(!this.$auth.Intance()){
      this.$router.push({ path: "/login" })
    }
    //load list user
    if(this.user.UserName == 'admin')
      this.getUserByCodition()
    else{
      this.$router.replace({ path: "/newfeed" });
    }
  },
  data() {
    return {
      formMode:'Add',
      typeShow: 'User',
      isShowDialog: false,
      userData: [],
      postData:[],
      strSearch:'',
      pagination:{current : 1,pageSize : PAGE_SIZE_CONST },
      userColumns: [
        {
          title: "Họ & Tên",
          dataIndex: "FullName",
          width: "200",
          fixed: 'left'
        },
        {
          title: "Giới tính",
          dataIndex: "GenderString",
          width: "150",
        },
        {
          title: "Ngày sinh",
          dataIndex: "Birthday",
          width: "150",
        },
        {
          title: "Địa chỉ",
          dataIndex: "Address",
          width: "150",
        },

        {
          title: "Số điện thoại",
          dataIndex: "PhoneNumber",
          width: "150",
        },
        {
          title: "Email",
          dataIndex: "Email",
          width: "150",
        },
        {
          title: "Phòng ban",
          dataIndex: "Department",
          width: "150",
        },
        {
          title: "Chức vụ",
          dataIndex: "Position",
          width: "150",
        },
        {
          title: "operation",
          dataIndex: "operation",
          scopedSlots: { customRender: "operation" },
          width: "100",
          fixed: 'right',
          align:'center'
        },
      ],
      postColumns: [
        {
          title: "Họ & Tên",
          dataIndex: "FullName",
          width: "500"
        },
        {
          title: "Nội dung",
          dataIndex: "Content",
          width: "500",
        },
        {
          title: "Thời gian tạo",
          dataIndex: "CreateDateString",
          width: "150",
        },
        {
          title: "Tổng số lượt thích",
          dataIndex: "LikesCount",
          width: "150",
        },

        {
          title: "Tổng số bình luận",
          dataIndex: "CommentCount",
          width: "150",
        },
        {
          title: "operation",
          dataIndex: "operation",
          scopedSlots: { customRender: "operation" },
          width: "100",
          fixed: 'right',
          align:'center'
        },
      ],
      header : null
    };
  },
  methods: {
    changeTypeShow(mode){
      this.typeShow = mode
    },
    searchHandle:_.debounce(function(){
      if(this.typeShow == 'User'){
        this.getUserByCodition()
      }else{
        this.getPostByCodition()
      }
    },500),
    getUserByCodition(){
      const me = this;
      this.$auth.Intance().get(`${BASE_URL}Users?search=${this.strSearch}&page=${this.pagination.current}&record=${this.pagination.pageSize}`)
                .then(res =>{
                    me.userData = res.data.Data
                    me.userData.forEach((user)=>{
                      user.Birthday = me.convertDate(user.Birthday);
                    })
                    const pagination = { ...me.pagination };
                    // Read total count from server
                    // pagination.total = data.totalCount;
                    pagination.total = res.data.TotalRecord;
                    me.pagination = pagination;
                  }
                )
                .catch(e =>{
                    console.log(e)
                    this.showNotification("Chưa bật server","error")
                })
    },
    getPostByCodition(){
      this.$auth.Intance().get(`${BASE_URL}Posts?search=${this.strSearch}&page=${this.pagination.current}&record=${this.pagination.pageSize}`)
                .then(res =>{
                    this.postData = res.data.Data
                    const pagination = { ...this.pagination };
                    // Read total count from server
                    // pagination.total = data.totalCount;
                    pagination.total = res.data.TotalRecord;
                    this.pagination = pagination;
                  }
                )
                .catch(e =>{
                    console.log(e)
                    this.showNotification("Chưa bật server","error")
                })
    },
    handleTableChange(pagination) {
      //strSearch
      this.pagination = {...pagination}
      this.getUserByCodition()
    },
    onEdit(record){
        this.isShowDialog = true;
        this.formMode = 'Edit'
        this.userEdit = {...record}
        
    },
    addUser() {
      this.formMode = 'Add'
      this.isShowDialog = true;
    },
    closeDialog() {
      this.isShowDialog = false;
    },
    logout() {
      this.$router.replace({ path: "/login" });
    },
    onCellChange(key, dataIndex, value) {
      const dataSource = [...this.userData];
      const target = dataSource.find(item => item.key === key);
      if (target) {
        target[dataIndex] = value;
        this.userData = dataSource;
      }
    },
    onDelete(Id) {
      const dataSource = [...this.userData];
      const me = this;
      me.userData = dataSource.filter(item => item.Id !== Id);
      this.$auth.Intance().delete(`${BASE_URL}Users/remove?id=${Id}`)
                .then(res =>{
                    if(res.data.Success == true){
                      me.showNotification("Xoá thành công!","success")
                    }else{
                      me.showNotification("Xoá thất bại","error")
                    }
                }
                )
                .catch(e =>{
                    console.log(e)
                    me.showNotification("Chưa bật server","error")
                })

    },
    onDeletePost(Id) {
      const dataSource = [...this.userData];
      this.userData = dataSource.filter(item => item.Id !== Id);
      this.$auth.Intance().delete(`${BASE_URL}Posts/remove?id=${Id}`)
                .then(res =>{
                    if(res.data.Success == true){
                      this.showNotification("Xoá thành công!","success")
                    }else{
                      this.showNotification("Xoá thất bại","error")
                    }
                }
                )
                .catch(e =>{
                    console.log(e)
                    this.showNotification("Chưa bật server","error")
                })

    },
    handleSignUpSuccess(){
      this.pagination.current = 1
      this.getUserByCodition()
    },
    showNotification(message, type) {
        this.$notification[type]({
            message,
            duration: 2,
        });
      },
    convertDate(str) {
      var date = new Date(str),
        mnth = ("0" + (date.getMonth() + 1)).slice(-2),
        day = ("0" + date.getDate()).slice(-2);
      return [mnth,day, date.getFullYear()].join("/");
    }
  },
  computed: {
    ...mapState({
      user: (state) => state.user.user
    }),
  },
  watch: {
    // whenever question changes, this function will run
    typeShow(newValue) {
      this.pagination.current = 1
      if(newValue == 'User'){
        this.getUserByCodition()
      }else{
        this.getPostByCodition()
      }
    }
  },
};
</script>
<style>
@import "../assets/css/newfeed.css";
@import "../assets/css/search.css";
.header {
  background-color: var(--blue);
  height: 64px;
  display: flex;
  z-index: 1;
  position: sticky;
}
.cover-logo {
  height: 64px;
  width: 250px;
  display: flex;
  align-items: center;
  justify-content: space-around;
}
.logo {
  cursor: pointer;
  background-image: url("../assets/img/icon1.svg");
  height: 50px;
  width: 50px;
  min-height: 50px;
  min-width: 50px;
  background-position: -597px -748px;
}
.app-name {
  font-size: 23px;
  font-weight: 700;
  color: #fff;
  cursor: pointer;
  width: auto;
  margin-right: 32px;
  min-width: 118px;
}
.cover-right-header-admin {
  height: 64px;
  width: 200px;
  display: flex;
  align-items: center;
  justify-content: space-around;
}
item-descrip .avatar {
  background-image: url("../assets/img/icon1.svg");
  min-width: 32px;
  min-height: 32px;
  background-position: -352px -894px;
  height: 32px;
  width: 32px;
  border-radius: 50%;
  margin-left: 10px;
}
.full-name {
  margin-right: 10px;
  color: white;
}
.mess-icon {
  background-image: url("../assets/img/icon1.svg");
  min-width: 24px;
  min-height: 24px;
  background-position: -368px -88px;
  height: 24px;
  width: 24px;
  margin-right: 10px;
  cursor: pointer;
}
.count-mess-noti {
  width: 15px;
  height: 15px;
  border-radius: 50%;
  background-color: red;
  font-size: 10px;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-top: 10px;
  margin-left: 10px;
}
.noti-icon {
  background: url("../assets/img/icon1.svg") no-repeat -256px -88px;
  min-width: 24px;
  min-height: 24px;
  height: 24px;
  width: 24px;
  margin-right: 10px;
  cursor: pointer;
}
.logout-icon {
  background: url("../assets/img/icon2.svg") no-repeat -143px -252px;
  min-width: 34px;
  min-height: 34px;
  height: 34px;
  width: 34px;
  /* margin-right: 10px; */
  transform: rotate(90deg);
  cursor: pointer;
}
.main-content-admin {
  margin-left: 250px;
  width: calc(100% - 250px);
  padding-left:10px ;
}
.editable-cell {
  position: relative;
}
.editable-cell-input-wrapper,
.editable-cell-text-wrapper {
  padding-right: 24px;
}
.editable-cell-text-wrapper {
  padding: 5px 24px 5px 5px;
}
.editable-cell-icon,
.editable-cell-icon-check {
  position: absolute;
  right: 0;
  width: 20px;
  cursor: pointer;
}
.editable-cell-icon {
  line-height: 18px;
  display: none;
}
.editable-cell-icon-check {
  line-height: 28px;
}
.editable-cell:hover .editable-cell-icon {
  display: inline-block;
}
.editable-cell-icon:hover,
.editable-cell-icon-check:hover {
  color: #108ee9;
}
.editable-add-btn {
  margin-bottom: 8px;
}
.ant-table-tbody{
    background-color: white !important;
}
.ip-search-admin {
    position: absolute;
    right: 6px;
    margin-top: 36px;
}
.ant-table-fixed-left, .ant-table-fixed-right{
  z-index: 0 !important;
}
</style>