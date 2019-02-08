<template>
<div>
    <header-navbar></header-navbar>
    <loading v-bind:isLoading="isLoading"></loading>
    
    <main v-if="!isLoading">

    <div class="btn-group create-action-btn">
        <button class="btn btn-danger" v-on:click="createAction()">Создать новую акцию</button>

    </div>   

      <table class="table table-hover">
            <thead>
                <tr>
                    <th>Название акции</th>
                    <th>Описание акции</th>
                    <th>Команда в телеграме</th>
                    <th>Дата начала</th>
                    <th>Дата окончания</th>
                    <th>Статус акции</th>
                    <th>Подроднее</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="action in actions" :key="action.id">
                    <td>{{action.nameOfAction}}</td>
                    <td>{{action.advertisingTextShort}}</td>
                    <td>{{action.commandText}}</td>
                    <td>{{action.formattedDateBegin}}</td>
                    <td>{{action.formattedDateEnd}}</td>
                    <td>
                        <a href="#">
                            <i v-if="action.state" class="material-icons done_all" v-on:click="selectedAction=action; selectedAction.state=false; saveAction(selectedAction, $event)">done_all</i>
                            <i v-else class="material-icons done" v-on:click="selectedAction=action; selectedAction.state=true; saveAction(selectedAction, $event)">done</i>
                        </a>
                    </td>
                    <td>
                        <a href="#" class="material-icons search" v-on:click="selectedAction=action;showModal=true;">search</a>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="pag-container">
          <b-v-pagination class="pag" size="md" :total-rows="actionsTotalCount" :per-page="dataRowsPerPage" v-model="currentPage"></b-v-pagination>            
        </div>

        <modal-window v-if="showModal" v-on:ok="showModal=!showModal;saveAction(selectedAction)" v-on:cancel="showModal=false">
            <h4 slot="header">Акция: {{selectedAction.nameOfAction}}</h4>
            <form slot="body">
                <div class="form-group row">
                    <label for="action-name" class="col-3 col-form-label">Название</label>
                    <div class="col-9">
                        <input type="text" class="form-control" v-model="selectedAction.nameOfAction" id="action-name" placeholder="Название акции">
                    </div>
                </div>
                <div class="form-group row">
                    <label for="command-text" class="col-3 col-form-label">Текст команды</label>
                    <div class="col-9">
                        <input type="text" class="form-control" v-model="selectedAction.commandText" id="command-text" placeholder="Команда в телеграмме">
                    </div>
                </div>
                <div class="form-group row">
                    <label for="about-action" class="col-3 col-form-label">Описание</label>
                    <div class="col-9">
                        <textarea rows="5" class="form-control" placeholder="Описание акции" id="about-action" v-model="selectedAction.advertisingText"></textarea>
                    </div>
                </div>
                <div class="form-group row">
                    <label for="action-date-begin" class="col-3 col-form-label">Дата начала</label>
                    <div class="col-3">
                        <input type="datetime" class="form-control" id="action-date-begin" :value="selectedAction.formattedDateBegin">
                    </div>
                    <label for="action-date-end" class="col-3 col-form-label">Дата окочания</label>
                    <div class="col-3">
                        <input type="datetime" class="form-control" id="action-date-end" :value="selectedAction.formattedDateEnd">
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
const getAdvertActionsUrl = "/api/advertising?beginPage=";
const updInsertAction = "/api/advertising";
import axios from "axios";
import { ru } from "../../lang/ru-RU.js";
import HeaderNavbar from "../HeaderNavbar/header.vue";
import { authorizationHeader } from "../../helper.js";
import { jsonHeader } from "../../helper.js";
import BVPagination from "bootstrap-vue/es/components/pagination/pagination";
import ModalWindow from "../ModalWindow/ModalWindow.vue";
import Loading from "../Loading/loading.vue";
import MessageModalWindow from "../MessageModalWindow/messageModalWindow.vue";

let validateActionsFields = function(action) {
  let res = "";
  if (action) { //TO-DO: Добавить дополнительные проверки, переделать как в drugStore.vue
    if (
      action.actionId < 0 ||
      action.nameOfAction.length === 0 ||
      action.advertisingText.length === 0 ||
      action.commandText.length === 0 ||
      action.formattedDateBegin.length === 0 ||
      action.formattedDateEnd.length === 0
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
    BVPagination,
    ModalWindow,
    Loading,
    MessageModalWindow
  },
  data: function() {
    return {
      actions: [],
      selectedAcion: null,
      showModal: false,
      messageFromServer: "",
      isLoading: true,
      currentPage: 1,
      dataRowsPerPage: 15,
      actionsTotalCount: 0,
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
    getActions: function(beginPage, endPage) {
      let thisComp = this;
      axios
        .get(getAdvertActionsUrl + beginPage + "&" + "endPage=" + endPage, {
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
            thisComp.actions = res.data.actions;
            thisComp.actionsTotalCount = res.data.totalCount;
            thisComp.isLoading = false;
          } else {
            thisComp.showMsgModalWindow(true, ru.attention, "Данные по акциям не загружены!", null);
            thisComp.isLoading = false;
          }
        })
        .catch(function(error) {
          thisComp.showMsgModalWindow(true, ru.error, "Произошла ошибка!", null);
          thisComp.isLoading = false;
        });
    },
    saveAction: function(action, event) {      
      let thisComp = this;
      if (action && validateActionsFields(action).length === 0) {
        if (action.actionId == 0) {
          let advertAction = action;
          axios
            .post(updInsertAction, advertAction, {
              headers: authorizationHeader(sessionStorage.getItem("userToken"))
            })
            .then(function(res) {
              if (res.status === 200) {
                thisComp.showModal = false;
                thisComp.showMsgModalWindow(
                  true,
                  ru.attention,
                  ru.actionDataSaved,
                  2000
                );

                thisComp.actions.push(advertAction);
              
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
        } else if (action.actionId > 0) {
          let advertAction = action;
          axios
            .put(updInsertAction, action, {
              headers: authorizationHeader(sessionStorage.getItem("userToken"))
            })
            .then(function(res) {
              if (res.status === 200) {
                thisComp.showModal = false;
                thisComp.showMsgModalWindow(
                  true,
                  ru.attention,
                  ru.actionDataSaved,
                  2000
                );
              } else {
                thisComp.showModal = false;
                thisComp.showMsgModalWindow(true, ru.attention, res, null);
              }
            })
            .catch(function(error) {
              thisComp.showModal = false;
              thisComp.showMsgModalWindow(true, ru.error, ru.errorHappend, null);
            });
        }
      } else {
        let validatorMsg = validateActionsFields(action);
        thisComp.showMsgModalWindow(true, ru.attention, validatorMsg, null);
      }
    },
    createAction: function() {
      this.selectedAction = {
        actionId: 0,
        nameOfAction: "Новая акция",
        formattedDateBegin: "",
        formattedDateEnd: "",
        advertisingTextShort: "Новая акция",
        commandText: "/newAction"
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
    this.getActions(1, 15);
  },
  watch: {
    currentPage: function(pageIndex) {
      let lastRow = this.currentPage * this.dataRowsPerPage;
      let firstRow = lastRow - this.dataRowsPerPage + 1;
      this.getActions(firstRow, lastRow);
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
.create-action-btn {
  float: right;
  margin: 0 20pt 20pt 0;
}
</style>

