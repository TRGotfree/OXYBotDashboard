export default {
    data: function () {
        return {
            message4Send: "",
            showAlert: false,
            errorMessage: "Ошибка!"
        }
    },
    methods: {
        sendMsg2AllUsers: function () {
            if (message4Send) {
                
            }else{
                this.errorMessage = "Введите текст сообщения!";
                this.showAlert = true;
            }
        }
    },
}