export default {
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
            }
        }       
    },
    methods: {
        loadUsers: async function(){
            try {
                //TO-DО: Дописать загрузку данных
            } catch (error) {
                
            }
        }   
    },
}