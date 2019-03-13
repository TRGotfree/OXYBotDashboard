/* eslint-disable no-console */
import requestsService from "../../services/requestsService";
import NavBar from "../Header/Header.vue";
import Loading from "../../views/Loading.vue";

export default {
    components: {
        NavBar,
        Loading
    },
    created: function () {
        this.loadRequests(1, 15);
    },
    data: function () {
        return {
            tableData: {
                fields: [{
                        key: "requestId",
                        label: "Id запроса",
                        sortable: true
                    },
                    {
                        key: "requestDateTime",
                        label: "Дата и время запроса",
                        sortable: true
                    },
                    {
                        key: "chatId",
                        label: "Id пользователя в ТГ.",
                        sortable: true
                    },
                    {
                        key: "userName",
                        label: "Ник пользователя",
                        sortable: true
                    },
                    {
                        key: "userFirstAndLastName",
                        label: "Имя пользователя",
                        sortable: true
                    },
                    {
                        key: "requestText",
                        label: "Текст запроса"
                    }
                ],
                items: []
            },
            requestsTotalCount: 0,
            todayRequestsCount: 0,
            isLoading: false,
            modalTitle: "Внимание!",
            modalText: "",
            isModalWindowShowing: false,
            currentPage: 1,
            dataRowsPerPage: 15,
            selectedRequest: {},
            showDangerAlert: false,
            alertMessage: "",
            showSuccessAlert: false,
            requests: [],
            selectedAcion: null
        }
    },
    methods: {
        async loadRequests(beginPage = 1, endPage = 15) {
            try {
                this.isLoading = true;

                const responseFromServer = await requestsService.getRequests(beginPage, endPage);

                if (responseFromServer && responseFromServer.isSuccessfully) {
                    this.tableData.items = responseFromServer.data.requests;
                    this.requestsTotalCount = responseFromServer.data.requestTotalCount;
                    this.todayRequestsCount = responseFromServer.data.todayRequestsCount;
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
        hideAlerts() {
            this.showDangerAlert = false;
            this.showSuccessAlert = false;
        }
    },
    watch: {
        currentPage(pageIndex) {

            let lastRow = 1;
            if (this.requestsTotalCount < pageIndex * this.dataRowsPerPage)
                lastRow = this.requestsTotalCount;
            else
                lastRow = pageIndex * this.dataRowsPerPage;

            let firstRow = lastRow - this.dataRowsPerPage + 1;
            this.loadRequests(firstRow, lastRow);
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
        }
    }
}