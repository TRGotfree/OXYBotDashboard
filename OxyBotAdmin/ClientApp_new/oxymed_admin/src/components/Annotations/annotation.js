/* eslint-disable no-console */
import annotationService from "../../services/annotationService";
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
                fields: [{
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
                    ,
                    {
                        key: "usingWay",
                        label: "Способ применения"
                    },
                    {
                        key: "forWhatIsUse",
                        label: "Предназначение"
                    },
                    {
                        key: "specialInstructions",
                        label: "Спец. указания"
                    },
                    {
                        key: "сontraIndicators",
                        label: "Противопоказания"
                    },
                    {
                        key: "isImageExists",
                        label: "Есть изображение"
                    }
                ],
                items: []
            },
            selectedAnnotation: {},
            isAnnotationEditWindowShowing: false,
            annotationModalHeader: "",
            messageFromServer: "",
            isLoading: true,
            currentPage: 1,
            dataRowsPerPage: 15,
            annotationsTotalCount: 0,
            goodsCountWithImages: 0,
            goodsCountWithoutImages: 0,
            showAlert: false,
            textForEdit: "",
            editAnnotationPropertyText: "",
            editProperty: "",
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

                const responseFromServer = await annotationService.getAnnotations(beginPage, endPage);

                if (responseFromServer && responseFromServer.isSuccessfully) {
                    console.log(JSON.stringify(responseFromServer));
                    this.tableData.items = responseFromServer.data.annotations;
                    this.annotationsTotalCount = responseFromServer.data.totalAnnotationCount;
                    this.goodsCountWithImages = responseFromServer.data.withImages;
                    this.goodsCountWithoutImages = responseFromServer.data.withoutImages;

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
        async saveNewAnnotation() {
            try {

                if (!this.selectedAnnotation)
                    throw new Error("Данные по аннотации пусты!");

                if (!this.selectedAnnotation.annotationId)
                    throw new Error("Не указан Id товара для текущей аннотации!");

                if (!this.selectedAnnotation.drugName)
                    throw new Error("Необходимо указать название товара!");

                if (!this.selectedAnnotation.producer)
                    throw new Error("Необходимо указать производителя!");

                let resultFromServer = null;

                resultFromServer = await annotationService.saveNewAnnotation(this.selectedAnnotation);

                if (resultFromServer.isSuccessfully) {

                    this.alertMessage = resultFromServer.message ? resultFromServer.message : "Данные по аннотации успешно сохранены!";
                    this.showSuccessAlert = true;

                    if (this.selectedAnnotation <= 0) {
                        
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
        async updateAnnotation() {
            try {

                if (!this.selectedAnnotation)
                    throw new Error("Данные по аннотации пусты!");

                if (!this.selectedAnnotation.annotationId)
                    throw new Error("Не указан Id товара для текущей аннотации!");

                if (!this.selectedAnnotation.drugName)
                    throw new Error("Необходимо указать название товара!");

                if (!this.selectedAnnotation.producer)
                    throw new Error("Необходимо указать производителя!");

                let resultFromServer = null;

                resultFromServer = await annotationService.updateAnnotation(this.selectedAnnotation);

                if (resultFromServer.isSuccessfully) {

                    this.alertMessage = resultFromServer.message ? resultFromServer.message : "Данные по аннотации успешно сохранены!";
                    this.showSuccessAlert = true;

                    if (this.selectedAnnotation <= 0) {
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
        hideAlerts() {
            this.showDangerAlert = false;
            this.showSuccessAlert = false;
        },
        editAnnotation(annotationForEdit, annotationPropertyNameForEdit) {
            switch (annotationPropertyNameForEdit) {

                case "usingWay":
                    this.selectedAnnotation = annotationForEdit;
                    this.editAnnotationPropertyText = "Способ применения:";
                    this.textForEdit = annotationForEdit.usingWay;
                    this.editProperty = "usingWay";
                    this.isAnnotationEditWindowShowing = true;

                    break;

                case "forWhatIsUse":
                    this.selectedAnnotation = annotationForEdit;
                    this.editAnnotationPropertyText = "Предназначение";
                    this.textForEdit = annotationForEdit.forWhatIsUse;
                    this.editProperty = "forWhatIsUse";
                    this.isAnnotationEditWindowShowing = true;

                    break;

                case "specialInstructions":
                    this.selectedAnnotation = annotationForEdit;
                    this.editAnnotationPropertyText = "Спец. указания";
                    this.textForEdit = annotationForEdit.specialInstructions;
                    this.editProperty = "specialInstructions";
                    this.isAnnotationEditWindowShowing = true;
                    break;

                case "сontraIndicators":
                    this.selectedAnnotation = annotationForEdit;
                    this.editAnnotationPropertyText = "Противопоказания";
                    this.textForEdit = annotationForEdit.specialInstructions;
                    this.editProperty = "contraIndicators";
                    this.isAnnotationEditWindowShowing = true;
                    break;

                case "sideEffects":
                    this.selectedAnnotation = annotationForEdit;
                    this.editAnnotationPropertyText = "Побочные эффекты";
                    this.textForEdit = annotationForEdit.sideEffects;
                    this.editProperty = "sideEffects";
                    this.isAnnotationEditWindowShowing = true;
                    break;
            }
        },
        chooseImg() {
            this.$refs.input_img.click();
        },
        async imageSelected(e) {
            try {

                if (!e.target.files || e.target.files.length === 0 || !e.target.files[0])
                    return;

                if (!this.selectedAnnotation)
                    throw new Error("Данные по аннотации пусты!");

                if (!this.selectedAnnotation.annotationId)
                    throw new Error("Не указан Id товара для текущей аннотации!");

                this.selectedImg = e.target.files[0];
                this.img_src = URL.createObjectURL(this.selectedImg);

                var formData = new FormData();

                if (this.selectedImg)
                    formData.append("file", this.selectedImg);

                formData.append("annotationId", this.selectedAnnotation.annotationId);

                this.modalText = "Фото загружается на сервер подождите...";
                this.isModalWindowShowing = true;

                await annotationService.saveImage(formData);

                this.modalText = "Фото успешно загружено на сервер!";

                setTimeout(() => {
                    this.isModalWindowShowing = false;
                }, 3000);

            } catch (error) {
                this.modalText = "Произошла ошибка во время сохранения изображения товара! Попробуйте ещё раз."
                this.isModalWindowShowing = true;
            }
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
            this.loadAnnotations(firstRow, lastRow);
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
                this.selectedAnnotation = {};
                this.alertMessage = "";
            }
        },
        selectedAnnotation(annotationData) {
            if (annotationData)
                this.annotationModalHeader = "Товар: " + annotationData.drugName + "<br>Производитель: " + annotationData.producer;
        }
    }
}