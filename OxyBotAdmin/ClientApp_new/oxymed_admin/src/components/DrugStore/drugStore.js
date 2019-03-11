/* eslint-disable no-console */
import drugStoreService from "../../services/drugStoresService";
import NavBar from "../Header/Header.vue";
import Loading from "../../views/Loading.vue";

const validationOK = "OK";

export default {
    components: {
        NavBar,
        Loading
    },
    created: function () {
        this.loadDrugStores(1, 15);
        this.loadDistricts();
    },
    data: function () {
        return {
            tableData: {
                fields: [
                    {
                        key: "drugStoreId",
                        label: "Id аптеки",
                        sortable: true
                    },
                    {
                        key: "drugStoreName",
                        label: "Название аптеки",
                        sortable: true
                    },
                    {
                        key: "address",
                        label: "Адрес аптеки",
                        sortable: true
                    },
                    {
                        key: "phone",
                        label: "Телефон",
                        sortable: true
                    },
                    {
                        key: "district",
                        label: "Район / Город",
                        sortable: true
                    },
                    {
                        key: "status",
                        label: "Открыта / Закрыта",
                        sortable: true
                    },
                    {
                        key: "moreInfo",
                        label: "Подробнее"
                    }
                ],
                items: []
            },
            drugStoreTotalCount: 0,
            isLoading: false,
            modalTitle: "Внимание!",
            modalText: "",
            isModalWindowShowing: false,
            currentPage: 1,
            dataRowsPerPage: 15,
            isDrugStoreEditWindowShowing: false,
            selectedDrugStore: {},
            showDangerAlert: false,
            alertMessage: "",
            showSuccessAlert: false,
            districts: []
        }
    },
    methods: {
        validateFields: function (drugStore) {
            let validationResultMsg = "OK";
            try {

                if (!drugStore)
                    validationResultMsg = "Что то пошло не так! Не удалось определить объект аптеки!"

                if (!drugStore.drugStoreId || drugStore.drugStoreId === 0)
                    validationResultMsg = "Укажите id аптеки из \"Аналитики\", оно нужно для синхронизации остатков товара!";

                if (!drugStore.drugStoreName || !drugStore.drugStoreName.match(/^Аптека №\d+|^Аптека№\d+|Аптека №\s\d+|^Аптека №\d+\W\d+/gm))
                    validationResultMsg = "Укажите правильное название аптеки оно должно начинаться на слово \"Аптека\" и содержать номер аптеки! К примеру \"Аптека №1\"";

                if (!drugStore.address || !drugStore.address.trim())
                    validationResultMsg = "Укажите адрес аптеки!";

                if (!drugStore.workTime || !drugStore.workTime.trim())
                    validationResultMsg = "Укажите режим работы аптеки!";

                if (!drugStore.phone || !drugStore.phone.trim())
                    validationResultMsg = "Укажите телефон аптеки!";

                if (!drugStore.orientir || !drugStore.orientir.trim())
                    validationResultMsg = "Укажите ориентир аптеки!";

                if (!drugStore.district || !drugStore.district.trim() || this.districts.indexOf(drugStore.district) < 0)
                    validationResultMsg = "Выберите район из списка!";

            } catch (error) {
                validationResultMsg = "Не удалось проверить соответствие заполненных Вами данных!";
            }
            return validationResultMsg;
        },
        async loadDrugStores(beginPage = 1, endPage = 15) {
            try {
                this.isLoading = true;

                const responseFromServer = await drugStoreService.getDrugStores(beginPage, endPage);

                if (responseFromServer && responseFromServer.isSuccessfully) {
                    this.tableData.items = responseFromServer.data.drugStores;
                    this.drugStoreTotalCount = responseFromServer.data.totalCount;
                } else {
                    this.modalText = responseFromServer.message;
                    this.isModalWindowShowing = true;
                }

            } catch (error) {
                this.modalText = "Ошибка не удалось загрузить данные по аптекам!";
                if (process.env.VUE_APP_IS_DEV) {
                    console.log(error.toString());
                }
                this.isModalWindowShowing = true;
            }
            this.isLoading = false;
        },
        async loadDistricts() {
            try {
                this.isLoading = true;

                const responseFromServer = await drugStoreService.getDistricts();

                if (responseFromServer && responseFromServer.isSuccessfully) {
                    this.districts = responseFromServer.data.districts;
                } else {
                    this.modalText = responseFromServer.message;
                    this.isModalWindowShowing = true;
                }

            } catch (error) {
                this.modalText = "Ошибка не удалось загрузить данные по районам!";
                if (process.env.VUE_APP_IS_DEV) {
                    console.log(error.toString());
                }
                this.isModalWindowShowing = true;
            }
            this.isLoading = false;
        },
        async saveDrugStore() {
            try {

                if (this.validateFields(this.selectedDrugStore) === validationOK) {
                    
                    this.alertMessage = "";

                    this.selectedDrugStore.shortName = this.selectedDrugStore.drugStoreName.match(/^Аптека №\d+|^Аптека№\d+|Аптека №\s\d+|^Аптека №\d+\W\d+/gm)[0];

                    this.selectedDrugStore.drugStoreTotalCount = undefined;

                    let resultFromServer = await drugStoreService.saveDrugStore(this.selectedDrugStore);

                    if (resultFromServer.isSuccessfully) {

                        this.alertMessage = resultFromServer.message ? resultFromServer.message : "Данные по акции успешно сохранены!";
                        this.showSuccessAlert = true;

                        if (this.selectedDrugStore.id <= 0) {
                            setTimeout(() => {
                                this.isDrugStoreEditWindowShowing = false;
                                this.loadDrugStores(1, 15);
                            }, 3000);
                        } else {
                            setTimeout(() => {
                                this.isDrugStoreEditWindowShowing = false;
                            }, 3000);
                        }

                    } else {
                        this.alertMessage = resultFromServer.message ? resultFromServer.message : "Данные не сохранены!";
                        this.showDangerAlert = true;
                    }
                } else {
                    this.alertMessage = this.validateFields(this.selectedDrugStore);
                    this.showDangerAlert = true;
                }
            } catch (error) {
                this.showDangerAlert = true;
                this.alertMessage = error.toString();
            }
        },
        createNewDrugStore() {
            this.hideAlerts();
            this.selectedDrugStore = {
                id: 0,
                drugStoreId: 0,
                drugStoreName: "",
                address: "",
                phone: "",
                workTime: "07:00-24:00",
                district: "г. Ташкент Мирабадский район",
                shortName: "",
                status: true,
                orientir: ""
            };

            this.isDrugStoreEditWindowShowing = true;
        },
        hideAlerts() {
            this.alertMessage = "";
        }
    },
    watch: {
        currentPage(pageIndex) {

            let lastRow = 1;
            if (this.drugStoreTotalCount < pageIndex * this.dataRowsPerPage)
                lastRow = this.drugStoreTotalCount;
            else
                lastRow = pageIndex * this.dataRowsPerPage;

            let firstRow = lastRow - this.dataRowsPerPage + 1;
            this.loadDrugStores(firstRow, lastRow);
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
        isDrugStoreEditWindowShowing(isShowing) {
            if (!isShowing) {
                this.selectedDrugStore = {};
                this.alertMessage = "";
                this.showDangerAlert = false;
                this.showSuccessAlert = false;
            }
        }
    }
}