<template>
  <div>
    <header-navbar></header-navbar>
    <loading v-bind:isLoading="isLoading"></loading>
    <main v-if="!isLoading">
      <table class="table table-hover">
        <thead>
          <tr>
            <th>Дата и время запроса</th>
            <th>Чат Id</th>
            <th>Ник</th>
            <th>Имя и фамилия</th>
            <th>Текст запроса</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="request in requests" :key="request.id">
            <td>{{request.requestDateTime}}</td>
            <td>{{request.chatId}}</td>
            <td>{{request.userName}}</td>
            <td>{{request.userFirstName}} {{request.userLastName}}</td>
            <td>{{request.requestText}}</td>
          </tr>
        </tbody>
      </table>
      <div class="pag-container">
        <b-v-pagination
          class="pag"
          size="md"
          :total-rows="requestsTotalCount"
          :per-page="dataRowsPerPage"
          v-model="currentPage"
        ></b-v-pagination>
      </div>
    </main>
  </div>
</template>
<script>
const getRequestsUrl = "/api/request?beginPage=";
const updateInsertRequest = "/api/request";
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

let validateActionsFields = function(request) {
  let res = "";
  if (request) {
    //TO-DO: Добавить дополнительные проверки, переделать как в drugStore.vue
    if (
      request.actionId < 0 ||
      request.nameOfAction.length === 0 ||
      request.advertisingText.length === 0 ||
      request.commandText.length === 0 ||
      request.formattedDateBegin.length === 0 ||
      request.formattedDateEnd.length === 0
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
      requests: [],
      selectedAcion: null,
      showModal: false,
      messageFromServer: "",
      isLoading: true,
      currentPage: 1,
      dataRowsPerPage: 15,
      requestsTotalCount: 0,
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
    getRequests: function(beginPage, endPage) {
      let thisComp = this;
      axios
        .get(getRequestsUrl + beginPage + "&" + "endPage=" + endPage, {
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
            thisComp.requests = res.data.requests;
            thisComp.requestsTotalCount = res.data.requestTotalCount;
            thisComp.isLoading = false;
          } else {
            thisComp.showMsgModalWindow(true, ru.attention, res, null);
            thisComp.isLoading = false;
          }
        })
        .catch(function(error) {
          thisComp.showMsgModalWindow(true, ru.error, error, null);
          thisComp.isLoading = false;
        });
    },
    showMsgModalWindow: function(isShow, headerText, msgText, timeOut) {
      this.msgModalWindow.timeOut2Show = timeOut;
      this.msgModalWindow.isShow = isShow;
      this.msgModalWindow.headerText = headerText;
      this.msgModalWindow.message2Show = msgText;
    }
  },
  mounted: function() {
    this.getRequests(1, 15);
  },
  watch: {
    currentPage: function(pageIndex) {
      let lastRow = this.currentPage * this.dataRowsPerPage;
      let firstRow = lastRow - this.dataRowsPerPage + 1;
      this.getRequests(firstRow, lastRow);
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
.create-request-btn {
  float: right;
  margin: 0 20pt 20pt 0;
}
</style>

