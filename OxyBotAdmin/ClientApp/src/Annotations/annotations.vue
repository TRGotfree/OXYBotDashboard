<template>
<div>
    <header-navbar></header-navbar>
    <loading v-bind:isLoading="isLoading"></loading>
    
    <main v-if="!isLoading">

    <div class="btn-group create-annotation-btn">
        <button class="btn btn-danger" v-on:click="createannotation()">Создать новую акцию</button>

    </div>   

      <table class="table table-hover">
            <thead>
                <tr>
                    <th>Название товара</th>
                    <th>Производитель</th>
                    <th>Способ применения</th>
                    <th>Предназначение</th>
                    <th>Специальные указания</th>
                    <th>Противопоказания</th>
                    <th>Побочные эффекты</th>
                    <th>Имеется изображение</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="annotation in annotations" :key="annotation.annotationid">
                    <td>{{annotation.drugName}}</td>
                    <td>{{annotation.producer}}</td>
                    <td>{{annotation.usingWay}}</td>
                    <td>{{annotation.forWhatIsUse}}</td>
                    <td>{{annotation.specialInstructions}}</td>
                    <td>{{annotation.contraIndicators}}</td>
                    <td>{{annotation.sideEffects}}</td>                  
                    <td>
                       <a href="#">
                            <i v-if="annotation.isImageExists" class="material-icons done_all" v-on:click="selectedAnnotation=annotation; saveAnnotation(selectedAnnotation, $event)">done_all</i>
                            <i v-else class="material-icons done" v-on:click="selectedAnnotation=annotation; saveAnnotation(annotation, $event)">done</i>
                        </a>
                    </td>
                    <td>
                        <a href="#" class="material-icons edit" v-on:click="selectedAnnotation=annotation;showModal=true;">edit</a>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="pag-container">
          <b-v-pagination class="pag" size="md" :total-rows="annotationsTotalCount" :per-page="dataRowsPerPage" v-model="currentPage"></b-v-pagination>            
        </div>

        <modal-window v-if="showModal" v-on:ok="showModal=!showModal;saveAnnotation(selectedAnnotation)" v-on:cancel="showModal=false">
            <h4 slot="header">Товар: {{selectedAnnotation.drugName}}. Производитель: {{selectedAnnotation.producer}}</h4>
            <form slot="body">
                <div class="form-group row">
                    <label for="using-way" class="col-3 col-form-label">Способ применения</label>
                    <div class="col-9">
                        <textarea rows="5" class="form-control" v-model="selectedAnnotation.usingWay" id="using-way" placeholder="Способ применения"></textarea>
                    </div>
                </div>
                <div class="form-group row">
                    <label for="for-what-is-using" class="col-3 col-form-label">Предназначение</label>
                    <div class="col-9">
                        <textarea rows="3" class="form-control" v-model="selectedAnnotation.forWhatIsUse" id="for-what-is-using" placeholder="Предназначение"></textarea>
                    </div>
                </div>
                <div class="form-group row">
                    <label for="special-instructions" class="col-3 col-form-label">Специальные указания</label>
                    <div class="col-9">
                        <textarea rows="3" class="form-control" placeholder="Специальные указания" id="special-instructions" v-model="selectedAnnotation.specialInstructions"></textarea>
                    </div>
                </div>
                <div class="form-group row">
                    <label for="contraindicators" class="col-3 col-form-label">Противопоказания</label>
                    <div class="col-9">
                        <textarea rows="3" class="form-control" placeholder="Противопоказания" id="contraindicators" v-model="selectedAnnotation.contraIndicators"></textarea>
                    </div>
                </div>
                    <div class="form-group row">
                    <label for="side-effects" class="col-3 col-form-label">Побочные эффекты</label>
                    <div class="col-9">
                        <textarea rows="3" class="form-control" placeholder="Побочные эффекты" id="side-effects" v-model="selectedAnnotation.sideEffects"></textarea>
                    </div>
                </div>
            </form>
            <span slot="okBtn">Сохранить</span>
        </modal-window>
        <message-modal-window v-bind="msgModalWindow" v-on:ok="msgModalWindow.isShow=false"></message-modal-window>
    </main>
</div>
</template>
<script>
const getAnnotationUrl = "/api/annotation?beginPage=";
const updateInsertAnnotation = "/api/annotation";
import axios from "axios";
import ErrorAlert from "../Alerts/errorAlert.vue";
import SuccessAlert from "../Alerts/successAlert.vue";
import { ru } from "../../lang/ru-RU.js";
import HeaderNavbar from "../HeaderNavbar/header.vue";
import { authorizationHeader } from "../../helper.js";
import { jsonHeader } from "../../helper.js";
import BVPagination from "bootstrap-vue/es/components/pagination/pagination";
import ModalWindow from "../ModalWindow/ModalWindow.vue";
import Loading from "../Loading/loading.vue";
import MessageModalWindow from "../MessageModalWindow/messageModalWindow.vue";

let validateannotationsFields = function(annotation) {
  let res = "";
  if (annotation) { //TO-DO: Добавить дополнительные проверки, переделать как в drugStore.vue
    if (
      annotation.annotationId < 0 ||
      annotation.nameOfannotation.length === 0 ||
      annotation.advertisingText.length === 0 ||
      annotation.commandText.length === 0 ||
      annotation.formattedDateBegin.length === 0 ||
      annotation.formattedDateEnd.length === 0
    ) {
      return ru.someFieldsAreEmptyOrNotValid;
    }
  } else {
    return ru.dataNotSaved;
  }
  return res;
};

export default {
  components: {
    HeaderNavbar,
    ErrorAlert,
    SuccessAlert,
    BVPagination,
    ModalWindow,
    Loading,
    MessageModalWindow
  },
  data: function() {
    return {
      annotations: [],
      selectedAnnotation: null,
      showModal: false,
      messageFromServer: "",
      isLoading: true,
      currentPage: 1,
      dataRowsPerPage: 15,
      annotationsTotalCount: 0,
      showAlert: false,
      msgModalWindow: {
        isShow: false,
        headerText: "Внимание!",
        message2Show: "",
        timeOut2Show: 2000
      }
    };
  },
  methods: {
    getAnnotations: function(beginPage, endPage) {
      let thisComp = this;
      axios
        .get(getAnnotationUrl + beginPage + "&" + "endPage=" + endPage, {
          headers: authorizationHeader(sessionStorage.getItem("userToken")),
          onDownloadProgress: function(loadingEvent) {
            if (loadingEvent.loaded !== loadingEvent.total) {
              thisComp.isLoading = true;
            } else {
              thisComp.isLoading = false;
            }
          }
        })
        .then(function(res) {
          if (res && res.status === 200) {
            thisComp.annotations = res.data.annotations;
            thisComp.annotationsTotalCount = res.data.totalAnnotationCount;
            thisComp.isLoading = false;
          } else {
            thisComp.showMsgModalWindow(true, ru.attention, "Не удалось получить данные с севрвера. Статус: " + res.status, null);
            thisComp.isLoading = false;
          }
        })
        .catch(function(error) {
          thisComp.showMsgModalWindow(true, ru.error, "Произошла ошибка при получении данных с сервера", null);
          thisComp.isLoading = false;
        });
    },
    saveAnnotation: function(annotation, event) {      
      let thisComp = this;
      if (annotation && validateannotationsFields(annotation).length === 0) {
        if (annotation.annotationId == 0) {
          let advertannotation = annotation;
          axios
            .post(updateInsertAnnotation, advertannotation, {
              headers: authorizationHeader(sessionStorage.getItem("userToken"))
            })
            .then(function(res) {
              if (res.status === 200) {
                thisComp.showModal = false;
                thisComp.showMsgModalWindow(
                  true,
                  ru.attention,
                  ru.annotationDataSaved,
                  2000
                );

                thisComp.annotations.push(advertannotation);
              
              } else {
                thisComp.showModal = false;
                thisComp.showMsgModalWindow(
                  true,
                  ru.attention,
                  res.value,
                  null
                );
              }
            })
            .catch(function(error) {
              thisComp.showModal = false;
              thisComp.showMsgModalWindow(
                true,
                ru.error,
                ru.dataNotSavedTryAgain,
                null
              );
            });
        } else if (annotation.annotationId > 0) {
          let advertannotation = annotation;
          axios
            .put(updateInsertAnnotation, annotation, {
              headers: authorizationHeader(sessionStorage.getItem("userToken"))
            })
            .then(function(res) {
              if (res.status === 200) {
                thisComp.showModal = false;
                thisComp.showMsgModalWindow(
                  true,
                  ru.attention,
                  ru.annotationDataSaved,
                  2000
                );
              } else {
                thisComp.showModal = false;
                thisComp.showMsgModalWindow(true, ru.attention, res, null);
              }
            })
            .catch(function(error) {
              thisComp.showModal = false;
              thisComp.showMsgModalWindow(true, ru.error, error, null);
            });
        }
      } else {
        let validatorMsg = validateannotationsFields(annotation);
        thisComp.showMsgModalWindow(true, ru.attention, validatorMsg, null);
      }
    },
    createannotation: function() {
      this.selectedAnnotation = {
        annotationId: 0,
        nameOfannotation: "Новая акция",
        formattedDateBegin: "",
        formattedDateEnd: "",
        advertisingTextShort: "Новая акция",
        commandText: "/newannotation"
      };
      this.showModal = true;
    },
    showMsgModalWindow: function(isShow, headerText, msgText, timeOut) {
      this.msgModalWindow.timeOut2Show = timeOut;
      this.msgModalWindow.isShow = isShow;
      this.msgModalWindow.headerText = headerText;
      this.msgModalWindow.message2Show = msgText;
    }
  },
  mounted: function() {
    this.getAnnotations(1, 15);
  },
  watch: {
    currentPage: function(pageIndex) {
      let lastRow = this.currentPage * this.dataRowsPerPage;
      let firstRow = lastRow - this.dataRowsPerPage + 1;
      this.getAnnotations(firstRow, lastRow);
    }
  }
};
</script>
<style>
.pag-container {
  text-align: center;
}
.pag {
  display: inline-flex;
}
.create-annotation-btn {
  float: right;
  margin: 0 20pt 20pt 0;
}
</style>

