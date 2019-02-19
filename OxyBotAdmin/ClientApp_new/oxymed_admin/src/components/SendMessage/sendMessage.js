/* eslint-disable no-console */
import NavBar from "../Header/Header.vue";
import sendMessageService from "../../services/sendMessagesService"

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
        sendMsg2AllUsers: function (message4Send) {
            if (message4Send) {
                  (message4Send)
            }else{
                this.errorMessage = "Введите текст сообщения!";
                this.showAlert = true;
            }
        }
    },
}