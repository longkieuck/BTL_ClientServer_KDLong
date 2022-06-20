class Enum {
    MessageBoxType = () =>{
        return {
            // Mặc định
            None : 0,
            // Form xác nhận
            Confirm : 1,
            //Form cảnh báo
            Warning : 2,
            //Form lỗi
            Error : 3
        }
    }

    ActionType = () => {
        return {
            //Không có hành động gì
            None : 0,
            //Xóa
            Delete : 1,
            //Thêm
            Add : 2,
            //Sửa
            Edit : 3,
            //Hủy bỏ (không)
            Cancel : 4,
            Close : 5
        }
    }
}

export default new Enum();