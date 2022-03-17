<template>
  <div>
    <div class="cover-dialog"></div>
    <div class="add-group-dialog">
      <div class="cover-title-dialog">
        <div class="title-dialog">Thêm thành viên</div>
        <div class="close-dialog"></div>
      </div>
      <div class="content-box">
        <InputSearch
          class="ip-search"
          lWidth="200"
          placeholder="Tìm kiếm nhân viên"
        />

        <div>
          <div style="margin-bottom: 12px">
            <span style="margin-left: 24px">
              <template v-if="hasSelected">
                {{ `${selectedRowKeys.length} đang chọn` }}
              </template>
            </span>
          </div>
          <a-table
            :row-selection="{
              selectedRowKeys: selectedRowKeys,
              onChange: onSelectChange,
            }"
            :columns="columns"
            :data-source="data"
            :scroll="{ y: 300 }"
          />
        </div>
      </div>
      <div class="footer-box">
        <div class="cancel-button button">Huỷ</div>
        <div class="save-button button">Thêm</div>
      </div>
    </div>
  </div>
</template>
<script>
import InputSearch from "./InputSearch.vue";
const columns = [
  {
    title: 'Name',
    dataIndex: 'name',
  },
  {
    title: 'Age',
    dataIndex: 'age',
  },
  {
    title: 'Address',
    dataIndex: 'address',
  },
];

const data = [];
for (let i = 0; i < 46; i++) {
  data.push({
    key: i,
    name: `Edward King ${i}`,
    age: 32,
    address: `London, Park Lane no. ${i}`,
  });
}

export default {
  components: {
    InputSearch,
  },
  data() {
    return {
      data,
      columns,
      selectedRowKeys: [], // Check here to configure the default column
      loading: false,
    };
  },
  computed: {
    hasSelected() {
      return this.selectedRowKeys.length > 0;
    },
  },
  methods: {
    start() {
      this.loading = true;
      // ajax request after empty completing
      setTimeout(() => {
        this.loading = false;
        this.selectedRowKeys = [];
      }, 1000);
    },
    onSelectChange(selectedRowKeys) {
      console.log('selectedRowKeys changed: ', selectedRowKeys);
      this.selectedRowKeys = selectedRowKeys;
    },
  },
};
</script>
<style>
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
.add-group-dialog {
  position: absolute;
  z-index: 2;
  width: 800px;
  height: 540px;
  left: calc(50% - 400px);
  top: calc(50% - 270px);
  border-radius: 4px;
  background-color: white;
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
.title-dialog {
  color: #1f1f1f;
  font-weight: 700;
  font-size: 17px;
}
.cover-title-dialog {
  display: flex;
  padding: 10px 10px;
}

.group-name-box,
.descrip-box {
  padding: 0 10px 10px 10px;
}
.content-box {
  height: 440px;
}
.input-search-mem {
  outline: none;
  box-sizing: border-box;
  border: 1px solid #babec5;
  border-radius: 4px;
  height: 30px;
  padding: 10px;
  width: 100%;
  margin-top: 6px;
}

.close-dialog:hover {
  background-color: #ccc;
}
.footer-box {
  width: 100%;
  padding: 10px;
  height: 56px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  background-color: #f5f5f5;
  border-radius: 0 0 4px 4px;
}
.ip-search {
  position: absolute;
  right: 10px;
}
.ant-table-pagination.ant-pagination{
    margin: 10px !important;
}
.ant-pagination-item,.ant-pagination-prev,.ant-pagination-next{
    min-width: unset !important;
    height: unset !important;
    line-height: unset !important;
}
/* Button footer */
.button {
  cursor: pointer;
  height: 32px;
  width: 54px;
  border-radius: 4px;
  display: flex;
  align-items: center;
  justify-content: center;
  border: 1px solid #bfbfbf;
}
.cancel-button {
  background-color: white;
}
.cancel-button:hover {
  background-color: #f5f5f5;
}
.save-button {
  background-color: #005abd;
  color: #f1f1f1;
}
.save-button:hover {
  background-color: #1890ff;
}
</style>