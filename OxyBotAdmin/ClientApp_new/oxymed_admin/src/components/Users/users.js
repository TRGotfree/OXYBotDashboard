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
                        key: "sendMessage",
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
            dataRowsPerPage: 15
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
        }   
    },
    watch: {
        currentPage: function(pageIndex){
            
            let lastRow = 1;
            if (this.botUsersTotalCount < pageIndex * this.dataRowsPerPage)
                lastRow = this.botUsersTotalCount;
            else
                lastRow = pageIndex * this.dataRowsPerPage;
            
            let firstRow = lastRow - this.dataRowsPerPage + 1;
            this.loadUsers(firstRow, lastRow);
        }
    }
}