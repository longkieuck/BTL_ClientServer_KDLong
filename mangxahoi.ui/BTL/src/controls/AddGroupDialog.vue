<template>
  <div>
    <div class="cover-dialog"></div>
    <div class="add-group-dialog">
        <div class="cover-title-dialog">
            <div class="title-dialog">
                Thêm nhóm mới
            </div>
            <div class="close-dialog">
            </div>
        </div>
        <div class="group-name-box">
            <div class="group-name" style="display: flex; font-size:13px">
                Tên nhóm <p style="color: red">*</p>
            </div>
            <input
                class="input-group-name"
                placeholder="Tên nhóm"
            >
        </div>
        <div class="descrip-box">
            <div style="font-weight :600;font-size:13px;">
                Mô tả
            </div>
            <textarea
                class="input-descrip"
                placeholder="Mô tả"
            />
        </div>
        <div class="footer-box">
            <div class="cancel-button button">
                Huỷ
            </div>
            <div class="save-button button">
                Lưu
            </div>
        </div>
    </div>
  </div>
</template>

<script>
export default {};
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
  width: 524px;
  height: 300px;
  left: calc(50% - 262px);
  top: calc(50% - 150px);
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
.title-dialog{
    color: #1f1f1f;
    font-weight: 700;
    font-size: 17px;
}
.cover-title-dialog{
    display: flex;
    padding: 10px 10px;
}

.group-name-box ,.descrip-box{
    padding: 0 10px 10px 10px;
}
.group-name{
    height: 20px;
    font-weight: 600;
    margin-top:6px
}
.input-descrip{
    outline: none;
    box-sizing: border-box;
    border: 1px solid #babec5;
    border-radius: 4px;
    padding: 4px 10px;
    width: 100%;
    height: 65px;
    max-height: 65px;
     margin-top:6px
}
.input-group-name{
    outline: none;
    box-sizing: border-box;
    border: 1px solid #babec5;
    border-radius: 4px;
    height: 30px;
    padding: 10px;
    width: 100%;
     margin-top:6px
}

.close-dialog:hover{
    background-color: #ccc;
}
.footer-box{
    width: 100%;
    padding: 10px;
    height: 76px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    background-color: #f5f5f5;
    border-radius: 0 0 4px 4px;
}

/* Button footer */
.button{
    cursor: pointer;
    height: 32px;
    width: 54px;
    border-radius: 4px;
    display: flex;
    align-items: center;
    justify-content: center;
    border: 1px solid #bfbfbf;
}
.cancel-button{
    background-color: white;

}
.cancel-button:hover{
    background-color: #f5f5f5;
}
.save-button{
    background-color: #005abd;
    color: #f1f1f1;
}
.save-button:hover{
    background-color: #1890ff;
}
</style>