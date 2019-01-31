<template>
  <div>
    <header-navbar></header-navbar>
    <!-- <loading v-bind:isLoading="isLoading"></loading> -->
    <div class="create-annotation">
      <b-form>
        <b-form-group label="ID товара:" label-for="goodId" description="Это идентификатор для связи справочника бота и Аналитики">
          <b-form-input id="goodId" type="number" required placeholder="ID товара" v-model="annotation.annotationId"></b-form-input>
        </b-form-group>
        <b-form-group label="Название товара" label-for="goodName">
          <b-form-input id="goodName" type="text" required placeholder="Название товара" v-model="annotation.drugName"></b-form-input>
        </b-form-group>
        <b-form-group label="Производитель" label-for="producer">
          <b-form-input id="producer" type="text" placeholder="Производитель" v-model="annotation.producer"></b-form-input>
        </b-form-group>
        <b-form-group label="Способ применения" label-for="usingWay">
          <b-form-textarea id="usingWay" rows="5" placeholder="Способ применения" v-model="annotation.usingWay"></b-form-textarea>
        </b-form-group>

        <div v-if="showAlert">
            <error-alert v-bind:message-from-server="messageFromServer"></error-alert>
        </div>
        <div v-if="showSuccess">
            <success-alert v-bind:message-from-server="messageFromServer"></success-alert>
        </div>

        <b-form-group label="Предназначение" label-for="forWhatIsUse">
          <b-form-textarea id="forWhatIsUse" rows="3" placeholder="Предназначение" v-model="annotation.forWhatIsUse"></b-form-textarea>
        </b-form-group>
        <b-form-group label="Спец. указания" label-for="specialInstructions">
          <b-form-textarea id="specialInstructions" rows="3" placeholder="Спец. указания" v-model="annotation.specialInstructions"></b-form-textarea>
        </b-form-group>
        <b-form-group label="Противопоказания" label-for="contraindicators">
          <b-form-textarea id="contraindicators" rows="3" placeholder="Противопоказания" v-model="annotation.contraIndicators"></b-form-textarea>
        </b-form-group>
        <b-form-group label="Побочные эффекты" label-for="sideEffects">
          <b-form-textarea id="sideEffects" rows="3" placeholder="Побочные эффекты" v-model="annotation.sideEffects"></b-form-textarea>
          <b-button variant="primary" type="file" v-on:change="imageSelected">Загрузить фото товара</b-button>
        <div v-if="img_src.length > 0">
            <success-alert v-bind:message-from-server="imageChoosed"></success-alert>
        </div>
        </b-form-group>
        
        <b-button variant="danger" v-on:click="saveAnnotation(annotation)">Сохранить</b-button>
      </b-form>
    </div>
    <message-modal-window v-bind="msgModalWindow" v-on:ok="msgModalWindow.isShow=false"></message-modal-window>
  </div>
</template>
<script>
const updateInsertAnnotation = "/api/annotation";
import axios from "axios";
import ErrorAlert from "../Alerts/errorAlert.vue";
import SuccessAlert from "../Alerts/successAlert.vue";
import { ru } from "../../lang/ru-RU.js";
import HeaderNavbar from "../HeaderNavbar/header.vue";
import { authorizationHeader } from "../../helper.js";
import { jsonHeader } from "../../helper.js";
import BForm from "bootstrap-vue/es/components/form/form";
import BFormText from "bootstrap-vue/es/components/form/form-text";
import BFormInvalidFeedback from "bootstrap-vue/es/components/form/form-invalid-feedback";
import BFormValidFeedback from "bootstrap-vue/es/components/form/form-valid-feedback";
import BFormRow from "bootstrap-vue/es/components/form/form-row";
import ModalWindow from "../ModalWindow/ModalWindow.vue";
import Loading from "../Loading/loading.vue";
import MessageModalWindow from "../MessageModalWindow/messageModalWindow.vue";
import { formDataHeader } from "../../helper.js";

let validateAnnotationsFields = function(annotation) {
  let res = true;
  if (annotation) {
    if (annotation.annotationId < 0 || annotation.drugName.length === 0) {
      res = false;
    }
  } else {
    res = false;
  }
  return res;
};

export default {
  components: {
    HeaderNavbar,
    ErrorAlert,
    SuccessAlert,
    BForm,
    BFormText,
    BFormInvalidFeedback,
    BFormValidFeedback,
    BFormRow,
    ModalWindow,
    Loading,
    MessageModalWindow
  },
  data: function() {
    return {
      annotation: {
        annotationId: 0,
        drugName: "",
        producer: "",
        usingWay: "",
        forWhatIsUse: "",
        specialInstructions: "",
        contraIndicators: "",
        sideEffects: "",
        isImageExists: true,
        totalCountOfAnnotations: 0
      },
      showAlert: false,
      showSuccess: false,
      messageFromServer: "",
      selectedImg: {},
      img_src: "",
      msgModalWindow: {
        isShow: false,
        headerText: "Внимание!",
        message2Show: "",
        timeOut2Show: 2000
      },
      imageChoosed: "Изображение выбрано"    
      //       annotations: [],
      //       selectedAnnotation: null,
      //       showModal: false,
      //       messageFromServer: "",
      //       isLoading: true,
      //       currentPage: 1,
      //       dataRowsPerPage: 15,
      //       annotationsTotalCount: 0,
      //       showAlert: false,
      //       msgModalWindow: {
      //         isShow: false,
      //         headerText: "Внимание!",
      //         message2Show: "",
      //         timeOut2Show: 2000
      //       },
      //       textForEdit: "",
      //       editAnnotationPropertyText: "",
      //       editProperty: {}
    };
  },
  methods: {
    saveAnnotation: function(goodAnnotation) {
      const thisComp = this;

      if (validateAnnotationsFields(goodAnnotation)) {

        var formData = new FormData();
        if (this.selectedImg && this.selectedImg.name) {
            var fileName = this.selectedImg.name;
            formData.append("file", this.selectedImg);
        }

        formData.append("goodAnnotation", goodAnnotation);
        
        axios
          .post(updateInsertAnnotation, formData, {
            headers: formDataHeader(sessionStorage.getItem("userToken")),
            onUploadProgress: function(uploadEvent) {
              thisComp.showAlert = true;  
              let percent = Math.round((uploadEvent.loaded / uploadEvent.total) * 100);
              thisComp.messageFromServer = percent + "%";            
              if (percent >= 100) thisComp.showAlert = false;            
            }
          })
          .then(function(res) {
            if (res.status === 200) {
              thisComp.showMsgModalWindow(
                true,
                ru.attention,
                "Данные успешно сохранены!",
                2000
              );
            } else {
              thisComp.showMsgModalWindow(
                true,
                ru.attention,
                ru.dataNotSavedTryAgain,
                null
              );
            }
          })
          .catch(function(error) {
            thisComp.showMsgModalWindow(
              true,
              ru.error,
              ru.dataNotSavedTryAgain,
              null
            );
          });
      } else {
        thisComp.showMsgModalWindow(
          true,
          ru.error,
          "Заполните обязательные поля формы!",
          null
        );
      }
    },
    chooseImg: function(event) {
      this.$refs.input_img.click();
    },
    imageSelected: function(e) {
      if (!e.target.files || e.target.files.length === 0 || !e.target.files[0]) {
        return;
      }
      this.selectedImg = e.target.files[0];
      this.img_src = URL.createObjectURL(this.selectedImg);
    },
    //     getAnnotations: function(beginPage, endPage) {
    //       let thisComp = this;
    //       axios
    //         .get(getAnnotationUrl + beginPage + "&" + "endPage=" + endPage, {
    //           headers: authorizationHeader(sessionStorage.getItem("userToken")),
    //           onDownloadProgress: function(loadingEvent) {
    //             if (loadingEvent.loaded !== loadingEvent.total) {
    //               thisComp.isLoading = true;
    //             } else {
    //               thisComp.isLoading = false;
    //             }
    //           }
    //         })
    //         .then(function(res) {
    //           if (res && res.status === 200) {
    //             thisComp.annotations = res.data.annotations;
    //             thisComp.annotationsTotalCount = res.data.totalAnnotationCount;
    //             thisComp.isLoading = false;
    //           } else {
    //             thisComp.showMsgModalWindow(
    //               true,
    //               ru.attention,
    //               "Не удалось получить данные с севрвера. Статус: " + res.status,
    //               null
    //             );
    //             thisComp.isLoading = false;
    //           }
    //         })
    //         .catch(function(error) {
    //           thisComp.showMsgModalWindow(
    //             true,
    //             ru.error,
    //             "Произошла ошибка при получении данных с сервера",
    //             null
    //           );
    //           thisComp.isLoading = false;
    //         });
    //     },

    //     updateAnnotation: function(annotation) {
    //       let thisComp = this;
    //       this.showModal = !this.showModal;

    //       axios
    //         .put(updateInsertAnnotation, annotation, {
    //           headers: authorizationHeader(sessionStorage.getItem("userToken"))
    //         })
    //         .then(function(res) {
    //           if (res.status === 200) {
    //             thisComp.showMsgModalWindow(
    //               true,
    //               ru.attention,
    //               "Данные по аннотации сохранены",
    //               2000
    //             );
    //           } else {
    //             thisComp.showMsgModalWindow(true, ru.attention, res.value, null);
    //           }
    //         })
    //         .catch(function(error) {
    //           thisComp.showModal = false;
    //           thisComp.showMsgModalWindow(
    //             true,
    //             ru.error,
    //             ru.dataNotSavedTryAgain,
    //             null
    //           );
    //         });
    //     },
    //     createannotation: function() {
    //       this.selectedAnnotation = {
    //         annotationId: 0,
    //         nameOfannotation: "Новая акция",
    //         formattedDateBegin: "",
    //         formattedDateEnd: "",
    //         advertisingTextShort: "Новая акция",
    //         commandText: "/newannotation"
    //       };
    //       this.showModal = true;
    //     },
    showMsgModalWindow: function(isShow, headerText, msgText, timeOut) {
      this.msgModalWindow.timeOut2Show = timeOut;
      this.msgModalWindow.isShow = isShow;
      this.msgModalWindow.headerText = headerText;
      this.msgModalWindow.message2Show = msgText;
    }
    //     getCertainAnnotation: function(annotation) {
    //       let thisComp = this;
    //       axios
    //         .get(getCertainAnnotationUrl + annotation.annotationId, {
    //           headers: authorizationHeader(sessionStorage.getItem("userToken")),
    //           onDownloadProgress: function(loadingEvent) {
    //             if (loadingEvent.loaded !== loadingEvent.total) {
    //               thisComp.isLoading = true;
    //             } else {
    //               thisComp.isLoading = false;
    //             }
    //           }
    //         })
    //         .then(function(res) {
    //           if (res && res.status === 200) {
    //             thisComp.selectedAnnotation = res.data.annotation;
    //             thisComp.isLoading = false;
    //             thisComp.showModal = true;
    //           } else {
    //             thisComp.showMsgModalWindow(
    //               true,
    //               ru.attention,
    //               "Не удалось получить данные с севрвера. Статус: " + res.status,
    //               null
    //             );
    //             thisComp.isLoading = false;
    //           }
    //         })
    //         .catch(function(error) {
    //           thisComp.showMsgModalWindow(true, ru.error, error.toString(), null);
    //           thisComp.isLoading = false;
    //         });
    //     },
    //     editAnnotation: function(annotationForEdit, annotationPropertyNameForEdit) {
    //       switch (annotationPropertyNameForEdit) {
    //         case "usingWay":
    //           this.selectedAnnotation = annotationForEdit;
    //           this.editAnnotationPropertyText = "Способ применения:";
    //           this.textForEdit = annotationForEdit.usingWay;
    //           this.editProperty = "usingWay";
    //           this.showModal = true;

    //           break;
    //         case "forWhatIsUse":
    //           this.selectedAnnotation = annotationForEdit;
    //           this.editAnnotationPropertyText = "Предназначение";
    //           this.textForEdit = annotationForEdit.forWhatIsUse;
    //           this.editProperty = "forWhatIsUse";
    //           this.showModal = true;

    //           break;
    //         case "specialInstructions":
    //           this.selectedAnnotation = annotationForEdit;
    //           this.editAnnotationPropertyText = "Спец. указания";
    //           this.textForEdit = annotationForEdit.specialInstructions;
    //           this.editProperty = "specialInstructions";
    //           this.showModal = true;
    //           break;
    //         case "contraIndicators":
    //           this.selectedAnnotation = annotationForEdit;
    //           this.editAnnotationPropertyText = "Противопоказания";
    //           this.textForEdit = annotationForEdit.specialInstructions;
    //           this.editProperty = "contraIndicators";
    //           this.showModal = true;
    //           break;

    //         case "sideEffects":
    //           this.selectedAnnotation = annotationForEdit;
    //           this.editAnnotationPropertyText = "Побочные эффекты";
    //           this.textForEdit = annotationForEdit.sideEffects;
    //           this.editProperty = "sideEffects";
    //           this.showModal = true;
    //           break;
    //         case "isImageExists":
    //           this.selectedAnnotation = annotationForEdit;
    //           this.editAnnotationPropertyText = "Изображение товара";

    //           //TO-DO: реализовать показ формы с изображением

    //           break;
    //       }
  }
  //   mounted: function() {
  //     this.getAnnotations(1, 15);
  //   },
  //   watch: {
  //     currentPage: function(pageIndex) {
  //       let lastRow = this.currentPage * this.dataRowsPerPage;
  //       let firstRow = lastRow - this.dataRowsPerPage + 1;
  //       this.getAnnotations(firstRow, lastRow);
  //     }
  //   }
};
</script>
<style>
.pag-container {
  text-align: center;
}
.pag {
  display: inline-flex;
}
.create-annotation {
  width: 70%;
  margin: auto;
  margin-top: 1rem;
}
.create-annotation-btn {
  float: right;
  margin: 0 20pt 20pt 0;
}
.done {
  color: darkgray;
}
</style>

