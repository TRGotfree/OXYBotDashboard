/* eslint-disable no-console */
import NavBar from "../Header/Header.vue";

export default {

    components: {
        NavBar
    },

    data: function () {
        return {
            message4Send: "",
            showAlert: false,
            errorMessage: "Ошибка!",
            headerLabel: "Рассылка сообщения пользователям"
        }
    },
    methods: {
        sendMsg2AllUsers: function () {
            if (this.message4Send) {
                console.log("Message sending...");
            }else{
                this.errorMessage = "Введите текст сообщения!";
                this.showAlert = true;
            }
        }
    },
}