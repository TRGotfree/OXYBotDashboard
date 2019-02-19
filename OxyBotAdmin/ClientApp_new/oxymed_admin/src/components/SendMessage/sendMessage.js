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
        sendMsg2AllUsers: async function (message4Send) {
            if (message4Send) {
                  this.alertMessage = "Идет отправка сообщения пользователям, подождите и не закрывайте окно браузера!";
                  this.showWarningAlert = true;   
                  let sendResult = await sendMessageService.sendMessageToAllUsers(message4Send);

                  if (sendResult.isSucessfully) {
                      this.showSuccessAlert = true;
                      this.alertMessage = !sendResult.message ? "Сообщение успешно отправлено!" : sendResult.message;
                  }else{
                       this.showDangerAlert = true;
                       this.alertMessage = !sendResult.message ? "Сообщение не отправлено!" : sendResult.message;
                  }
            }else{
                this.alertMessage = "Введите текст сообщения!";
                this.showDangerAlert= true;
            }
        }
    },
    watch: {
        showDangerAlert: function(isShow){
            if (isShow) {
                this.showWarningAlert = false;
                this.showSuccessAlert = false;
            }
        },
        showWarningAlert: function(isShow){
            if (isShow) {
                this.showDangerAlert = false;
                this.showSuccessAlert = false;
            }
        },
        showSuccessAlert: function(isShow){
            if (isShow) {
                this.showWarningAlert = false;
                this.showSuccessAlert = false;
            }
        }
    },
}