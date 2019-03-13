/* eslint-disable no-console */
import cardService from "../../services/cardService";
import userService from "../../services/usersService";
import NavBar from "../Header/Header.vue";
import Loading from "../../views/Loading.vue";

export default {
    components: {
        NavBar,
        Loading
    },
    created: function () {
        this.loadCards(1, 15);
    },
    data: function () {
        return {
            tableData: {
                fields: [{
                        key: "chatId",
                        label: "Id в ТГ",
                        sortable: true
                    },
                    {
                        key: "cardId",
                        label: "Id карты",
                        sortable: true
                    },
                    {
                        key: "userFIO",
                        label: "Имя клиента",
                        sortable: true
                    },
                    {
                        key: "birthDate",
                        label: "Дата рождения",
                        sortable: true
                    },
                    {
                        key: "phone",
                        label: "Телефон",
                        sortable: true
                    },
                    {
                        key: "email",
                        label: "Email",
                        sortable: true
                    },
                    {
                        key: "dateTimeEntered",
                        label: "Дата получения данных ботом",
                        sortable: true
                    },
                    {
                        key: "isUserWantsToGetUpdates",
                        label: "Клиент подписан на новости",
                        sortable: true
                    },
                    {
                        key: "isRegistered",
                        label: "Карта активирована",
                        sortable: true
                    }
                ],
                items: []
            },
            discountCardsTotalCount: 0,
            isLoading: false,
            modalTitle: "Внимание!",
            modalText: "",
            isModalWindowShowing: false,
            currentPage: 1,
            dataRowsPerPage: 15,
            isQuestionModalWindowShowing: false,
            selectedCard: {},
            showDangerAlert: false,
            alertMessage: "",
            showSuccessAlert: false,
            updatedProperty: null
        }
    },
    methods: {
        loadCards: async function (beginPage = 1, endPage = 15) {
            try {
                this.isLoading = true;

                const responseFromServer = await cardService.getCards(beginPage, endPage);

                if (responseFromServer && responseFromServer.isSuccessfully) {
                    this.tableData.items = responseFromServer.data.discounts;
                    this.discountCardsTotalCount = responseFromServer.data.totalCount;
                } else {
                    this.modalText = responseFromServer.message;
                    this.isModalWindowShowing = true;
                }

            } catch (error) {
                this.modalText = "Ошибка не удалось загрузить данные!";
                if (process.env.VUE_APP_IS_DEV)
                    console.log(error.toString());
                this.isModalWindowShowing = true;
            }
            this.isLoading = false;
        },
        async updateCardData() {
            try {

                if (!this.selectedCard || !this.selectedCard.cardId)
                    throw new Error("Данные по карте не указаны!");

                if (!this.selectedCard.chatId)
                    throw new Error("Id пользователя в телеграме не определено!");

                this.selectedCard[this.updatedProperty] = !this.selectedCard[this.updatedProperty];

                const resultFromServer = await cardService.updateCardData(this.selectedCard);
               
                if (resultFromServer.isSuccessfully) {
                    this.alertMessage = resultFromServer.message ? resultFromServer.message : "Данные успешно обновлены!";
                    
                    await this.sendMessage();

                    this.showSuccessAlert = true;
                    this.isQuestionModalWindowShowing = false;

                } else {
                    this.alertMessage = resultFromServer.message ? resultFromServer.message : "Данные не обновлены!";
                    this.showDangerAlert = true;
                }

            } catch (error) {
                this.showDangerAlert = true;
                this.alertMessage = error.toString();
            }

            this.updatedProperty = null;
        },
        askAndChangePropValue(textToShow, propNameForChange) {           
            this.modalText = textToShow;
            this.isQuestionModalWindowShowing = true;
            this.updatedProperty = propNameForChange; 
        },
        async sendMessage(){
            try {
                let messageForUser = "";
                if (this.updatedProperty === "isRegistered") {
                    if (this.selectedCard.isRegistered === true)
                        messageForUser = "✅✅✅Ваша дисконтная карта успешно зарегистрирована в системе! Теперь Вы можете использовать её в аптеках OXYmed!\n➖➖➖➖➖➖➖➖➖➖➖\n✅✅✅Sizning diskont kartangiz tizimda muvaffaqiyatli ro'yxatdan o'tgan! Endi Siz OXYmed dorixonalarida undan foydalanishingiz mumkin!";
                    else
                        messageForUser = "❌Ваша дисконтная карта заблокирована!❌\n➖➖➖➖➖➖➖➖➖➖➖\n❌Sizning diskont kartangiz bekor qilindi!❌";
                }else if (this.updatedProperty === "isUserWantsToGetUpdates"){
                    if (this.selectedCard.isUserWantsToGetUpdates === true)
                        messageForUser = "👍Вы подписаны на получение новостных сообщений!\n➖➖➖➖➖➖➖➖➖➖➖\n👍Yangiliklarni qabul qilish uchun obuna bo'lgansiz!";
                    else
                        messageForUser = "❌Вы отписаны от новостных сообщений!❌\n➖➖➖➖➖➖➖➖➖➖➖\n❌Sizning xabar obunangiz bekor qilindi!❌";
                }else{
                    throw new Error("This type of property is not recognized!");
                }

                await userService.sendMessageToUser(messageForUser, this.selectedCard.chatId)

            } catch (error) {
                this.showDangerAlert = true;
                this.alertMessage = error.toString();
            }
        }
    },
    watch: {
        currentPage(pageIndex) {

            let lastRow = 1;
            if (this.discountCardsTotalCount < pageIndex * this.dataRowsPerPage)
                lastRow = this.discountCardsTotalCount;
            else
                lastRow = pageIndex * this.dataRowsPerPage;

            let firstRow = lastRow - this.dataRowsPerPage + 1;
            this.loadCards(firstRow, lastRow);
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
        isQuestionModalWindowShowing(isShowing) {
            if (!isShowing) {
                this.selectedCard = {};
                this.alertMessage = "";
            }
        }
    }
}