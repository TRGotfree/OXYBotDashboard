/* eslint-disable no-console */
import annotationService from "../../services/actionService";
import NavBar from "../Header/Header.vue";
import Loading from "../../views/Loading.vue";

export default {
    components: {
        NavBar,
        Loading
    },
    created: function () {
        this.loadAnnotations(1, 15);
    },
    data: function () {
        return {
            tableData: {
                fields: [
                    {
                        key: "annotationId",
                        label: "Id аннотации",
                        sortable: true
                    },
                    {
                        key: "drugName",
                        label: "Название товара",
                        sortable: true
                    },
                    {
                        key: "producer",
                        label: "Производитель",
                        sortable: true
                    }
                    //,
                    // {
                    //     key: "usingWay",
                    //     label: "Способ применения"
                    // },
                    // {
                    //     key: "forWhatIsUse",
                    //     label: "Способ применения"
                    // },
                    // {
                    //     key: "specialInstructions",
                    //     label: "Спец. указания"
                    // },
                    // {
                    //     key: "сontraIndicators",
                    //     label: "Противопоказания"
                    // }
                ],
                items: []
            },
            selectedAnnotation: null,
            showModal: false,
            messageFromServer: "",
            isLoading: true,
            currentPage: 1,
            dataRowsPerPage: 15,
            annotationsTotalCount: 0,
            showAlert: false,
            textForEdit: "",
            editAnnotationPropertyText: "",
            editProperty: {},
            selectedImg: {},
            img_src: "",    
            modalTitle: "Внимание!",
            modalText: "",
            isModalWindowShowing: false,
            showDangerAlert: false,
            alertMessage: "",
            showSuccessAlert: false
        }
    },
    methods: {
        async loadAnnotations(beginPage = 1, endPage = 15) {
            try {
                this.isLoading = true;

                const responseFromServer = await actionService.getActions(beginPage, endPage);

                if (responseFromServer && responseFromServer.isSuccessfully) {
                    this.tableData.items = responseFromServer.data.actions;
                    this.actionTotalCount = responseFromServer.data.totalCount;
                } else {
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
        async saveAction() {
            try {

                if (!this.selectedAction)
                    throw new Error("Данные по акции пусты!");

                if (!this.selectedAction.nameOfAction)
                    throw new Error("Необходимо указать название акции!");

                if (!this.selectedAction.advertisingText)
                    throw new Error("Необходимо указать описание акции!");

                if (!this.selectedAction.commandText || !this.selectedAction.commandText.match(/^\/[a-zA-Z_0-9]+/gm))
                    throw new Error("Заполните правильно поле \"Команда в телеграме\"");

                if (!this.selectedAction.formattedDateBegin || !this.selectedAction.formattedDateEnd)
                    throw new Error("Укажите дату начала и дату окончания акции!");

                let resultFromServer = null;

                if (this.selectedAction.actionId <= 0)
                    resultFromServer = await actionService.saveNewAction(this.selectedAction);
                else
                    resultFromServer = await actionService.updateAction(this.selectedAction);

                if (resultFromServer.isSuccessfully) {
                    this.alertMessage = resultFromServer.message ? resultFromServer.message : "Данные по акции успешно сохранены!";
                    this.showSuccessAlert = true;

                    if (this.selectedAction <= 0) {
                        setTimeout(() => {
                            this.isAnnotationEditWindowShowing = false;
                            this.loadAnnotations(1, 15);
                        }, 3000);
                    } else {
                        setTimeout(() => {
                            this.isAnnotationEditWindowShowing = false;
                        }, 3000);
                    }

                } else {
                    this.alertMessage = resultFromServer.message ? resultFromServer.message : "Данные не сохранены!";
                    this.showDangerAlert = true;
                }

            } catch (error) {
                this.showDangerAlert = true;
                this.alertMessage = error.toString();
            }
        },
        createNewAction() {
            this.hideAlerts();
            this.selectedAction = {
                actionId: 0,
                commandText: "/new_action_123",
                formattedDateBegin: "2020-01-01",
                formattedDateEnd: "",
                nameOfAction: "",
                advertisingText: ""
            };
            this.selectedAction.actionId = 0;
            this.selectedAction.commandText = "";
            this.selectedAction.formattedDateBegin = "2020-01-01";
            this.selectedAction.formattedDateEnd = "2020-01-31";
            this.selectedAction.state = true;
            this.isAnnotationEditWindowShowing = true;
        },
        hideAlerts() {
            this.showDangerAlert = false;
            this.showSuccessAlert = false;
        }
    },
    watch: {
        currentPage(pageIndex) {

            let lastRow = 1;
            if (this.actionTotalCount < pageIndex * this.dataRowsPerPage)
                lastRow = this.actionTotalCount;
            else
                lastRow = pageIndex * this.dataRowsPerPage;

            let firstRow = lastRow - this.dataRowsPerPage + 1;
            this.loadUsers(firstRow, lastRow);
        },
        showDangerAlert(isShowing) {
            if (isShowing)
                this.showSuccessAlert = false;
        },
        showSuccessAlert(isShowing) {
            if (isShowing)
                this.showDangerAlert = false;
        },
        alertMessage(message) {
            if (!message) {
                this.showDangerAlert = false;
                this.showSuccessAlert = false;
            }
        },
        isAnnotationEditWindowShowing(isShowing) {
            if (!isShowing) {
                this.selectedAction = {};
                this.alertMessage = "";
            }
        }
    }
}