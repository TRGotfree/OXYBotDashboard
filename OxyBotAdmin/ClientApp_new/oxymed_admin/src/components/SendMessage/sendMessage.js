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
            headerLabel: "Рассылка сообщения пользователям",
            isSendButtonDisabled: false,
            showDangerAlert: false,
            showWarningAlert: false,
            showSuccessAlert: false,
            alertMessage: ""
        }
    },
    methods: {
        sendMsg2AllUsers: async function () {
            try {

                const message4Send = this.message4Send;

                if (message4Send) {
                          
                    this.isSendButtonDisabled = true
                    this.alertMessage = "Рассылка сообщения, подождите и не закрывайте окно браузера!";
                    this.showWarningAlert = true;
                    let sendResult = await sendMessageService.sendMessageToAllUsers(message4Send);

                    if (sendResult.isSucessfully) {
                        this.showSuccessAlert = true;
                        this.alertMessage = !sendResult.message ? "Сообщение успешно отправлено!" : sendResult.message;
                    } else {
                        this.showDangerAlert = true;
                        this.alertMessage = !sendResult.message ? "Сообщение не отправлено!" : sendResult.message;
                    }

                } else {
                    this.alertMessage = "Введите текст сообщения!";
                    this.showDangerAlert = true;
                }
            } catch (error) {               
                this.showDangerAlert = true;
                this.alertMessage = "Произошла непредвиденная ошибка!"
            }
            this.isSendButtonDisabled = false;
        }
    },
    watch: {
        showDangerAlert: function (isShow) {
            if (isShow) {
                this.showWarningAlert = false;
                this.showSuccessAlert = false;
            }
        },
        showWarningAlert: function (isShow) {
            if (isShow) {
                this.showDangerAlert = false;
                this.showSuccessAlert = false;
            }
        },
        showSuccessAlert: function (isShow) {
            if (isShow) {
                this.showWarningAlert = false;
                this.showSuccessAlert = false;
            }
        }
    },
}