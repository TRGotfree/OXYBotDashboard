/* eslint-disable no-console */
import actionService from "../../services/actionService";
import NavBar from "../Header/Header.vue";
import Loading from "../../views/Loading.vue";

export default {
    components: {
        NavBar,
        Loading
    },
    created: function(){
        this.loadActions(1, 15);
    },
    data: function () {
        return {
            tableData: {
                fields: [
                    {
                        key: "actionId",
                        label: "Id акции",
                        sortable: true
                    },
                    {
                        key: "nameOfAction",
                        label: "Название акции",
                        sortable: true
                    },
                    {
                        key: "advertisingTextShort",
                        label: "Описание акции",
                        sortable: true
                    },
                    {
                        key: "formattedDateBegin",
                        label: "Дата начала акции",
                        sortable: true
                    },
                    {
                        key: "formattedDateEnd",
                        label: "Дата окончания акции",
                        sortable: true
                    },
                    {
                        key: "state",
                        label: "Активна/Не активна"
                    },
                    {
                        key: "moreInfo",
                        label: "Подробнее"
                    }
                ],
                items: []
            },
            actionTotalCount: 0,
            isLoading: false,
            modalTitle: "Внимание!",
            modalText: "",
            isModalWindowShowing: false,
            currentPage: 1,
            dataRowsPerPage: 15,
            isActionEditWindowShowing: false,
            selectedAction: {},
            showDangerAlert: false,
            alertMessage: "",
            showSuccessAlert: false
        }       
    },
    methods: {
        loadActions: async function(beginPage=1, endPage=15){
            try {
                this.isLoading = true;

                const responseFromServer = await actionService.getActions(beginPage, endPage);
                
                if (responseFromServer && responseFromServer.isSuccessfully) {
                    this.tableData.items = responseFromServer.data.botUsers;
                    this.actionTotalCount = responseFromServer.data.botUsersCount
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
        async saveAction(){
            try {
                
                if (!this.selectedAction)
                    throw new Error("Данные по акции пусты!");

                if (!this.selectedAction.nameOfAction)
                    throw new Error("Необходимо указать название акции!");
                
                if (!this.selectedAction.advertisingText)
                    throw new Error("Необходимо указать описание акции!");
                
                if (!this.selectedAction.commandText || !this.selectedAction.commandText.match(/^\/[a-zA-Z_0-9]+/gm))
                    throw new Error("Заполните правильно поле \"Команда в телеграме\"");  

                if (!this.selectedAction.dateBegin || !this.selectedAction.dateEnd)
                    throw new Error("Укажите дату начала и дату окончания акции!");
            
                let resultFromServer = null;
            
                if (this.selectedAction.actionId <= 0)
                    resultFromServer = await actionService.saveNewAction(this.selectedAction);  
                else 
                    resultFromServer = await actionService.updateAction(this.selectedAction);          
                  
                if (resultFromServer.isSuccessfully) {
                    this.alertMessage = resultFromServer.message ? resultFromServer.message : "Данные по акции успешно сохранены!";
                    this.showSuccessAlert = true;
                } else {
                    this.alertMessage = resultFromServer.message ? resultFromServer.message : "Данные не сохранены!";
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
            if (this.actionTotalCount < pageIndex * this.dataRowsPerPage)
                lastRow = this.actionTotalCount;
            else
                lastRow = pageIndex * this.dataRowsPerPage;
            
            let firstRow = lastRow - this.dataRowsPerPage + 1;
            this.loadUsers(firstRow, lastRow);
        },
        showDangerAlert(isShowing){
            if (isShowing)
                this.showSuccessAlert = false;
        },
        showSuccessAlert(isShowing){
            if (isShowing)
                this.showDangerAlert = false;
        },
        alertMessage(message){
            if (!message) {
                this.showDangerAlert = false;
                this.showSuccessAlert = false;
            }
        },
        isActionEditWindowShowing(isShowing){
            if (!isShowing) {
                this.selectedAction = {};
                this.alertMessage = "";
            }
        }
    }
}