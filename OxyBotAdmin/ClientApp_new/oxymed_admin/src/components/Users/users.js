/* eslint-disable no-console */
import usersService from "../../services/usersService";
import NavBar from "../Header/Header.vue";
import Loading from "../../views/Loading.vue";

export default {
    components: {
        NavBar,
        Loading
    },
    created: function(){
        this.loadUsers(1, 15);
    },
    data: function () {
        return {
            tableData: {
                fields: [
                    {
                        key: "chatId",
                        label: "Id в ТГ",
                        sortable: true
                    },
                    {
                        key: "nickName",
                        label: "Ник",
                        sortable: true
                    },
                    {
                        key: "firstAndLastName",
                        label: "Имя и фамилия",
                        sortable: true
                    },
                    {
                        key: "lastVisitDateTime",
                        label: "Последняя дата визита",
                        sortable: true
                    },
                    {
                        key: "msgCount",
                        label: "Кол-во сообщений",
                        sortable: true
                    },
                    {
                        key: "messageForUser",
                        label: "Написать сообщение"
                    }
                ],
                items: []
            },
            botUsersTotalCount: 0,
            isLoading: false,
            modalTitle: "Внимание!",
            modalText: "",
            isModalWindowShowing: false,
            currentPage: 1,
            dataRowsPerPage: 15,
            isSendMessageWindowShowing: false,
            selectedUser: {},
            messageForSend: "",
            showDangerAlert: false,
            alertMessage: "",
            showSuccessAlert: false
        }       
    },
    methods: {
        loadUsers: async function(beginPage=1, endPage=15){
            try {
                this.isLoading = true;

                const responseFromServer = await usersService.getUsers(beginPage, endPage);
                
                if (responseFromServer && responseFromServer.isSuccessfully) {
                    this.tableData.items = responseFromServer.data.botUsers;
                    this.botUsersTotalCount = responseFromServer.data.botUsersCount
                }else{
                    this.modalText = responseFromServer.message;
                    this.isModalWindowShowing = true;
                }
                
            } catch (error) {
                this.modalText = "Ошибка не удалось загрузить данные!";
                if (process.env.VUE_APP_IS_DEV) {
                    console.log(error.toString());
                }
                this.isModalWindowShowing = true;
            }
            this.isLoading = false;
        },
        sendMessage: async function(){
            try {
                
                if (!this.selectedUser && !this.selectedUser.chatId)
                    throw new Error("Пользователь не выбран!");
                
                if (!this.messageForSend)    
                    throw new Error("Сообщение для пользователя не может быть пустым!");

                const resultFromServer = await usersService.sendMessageToUser(this.messageForSend, this.selectedUser.chatId);    
                if (resultFromServer.isSuccessfully) {
                    this.alertMessage = resultFromServer.message ? resultFromServer.message : "Сообщение успешно отправлено!";
                    this.showSuccessAlert = true;
                }else{
                    this.alertMessage = resultFromServer.message ? resultFromServer.message : "Сообщение не отправлено!";
                    this.showDangerAlert = true;
                }

            } catch (error) {
                this.showDangerAlert = true;
                this.alertMessage = error.toString();
            }
        }   
    },
    watch: {
        currentPage(pageIndex){
            
            let lastRow = 1;
            if (this.botUsersTotalCount < pageIndex * this.dataRowsPerPage)
                lastRow = this.botUsersTotalCount;
            else
                lastRow = pageIndex * this.dataRowsPerPage;
            
            let firstRow = lastRow - this.dataRowsPerPage + 1;
            this.loadUsers(firstRow, lastRow);
        },
        showDangerAlert(isShowing){
            if (isShowing) {
                this.showSuccessAlert = false;
            }
        },
        showSuccessAlert(isShowing){
            if (isShowing) {
                this.showDangerAlert = false;
            }
        },
        alertMessage(message){
            if (!message) {
                this.showDangerAlert = false;
                this.showSuccessAlert = false;
            }
        },
        isSendMessageWindowShowing(isShowing){
            if (!isShowing) {
                this.selectedUser = {};
                this.messageForSend = "";
                this.alertMessage = "";
            }
        }
    }
}