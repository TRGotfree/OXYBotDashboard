<template>
<div>    
    <header-navbar></header-navbar>
    <loading v-bind:isLoading="isLoading"></loading>
    <main v-if="!isLoading">   
      <div class="alert alert-info" role="alert">
        <h5>Кол-во пользователей бота: {{botUsersCount}}</h5>
      </div>
      <table class="table table-hover">
        <thead>
            <tr>
                <th>Чат Id</th>
                <th>Ник</th>
                <th>Имя и фамилия</th>
                <th>Последняя дата визита</th>
                <th>Кол-во обращений к боту</th>
                <th>Написать пользователю</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="user in botUsers" :key="user.chatId">
                <td>{{user.chatId}}</td>
                <td>{{user.nickName}}</td>
                <td>{{user.firstName}} {{user.lastName}}</td>
                <td>{{user.lastVisitDateTime}}</td>
                <td>{{user.msgCount}}</td>
                <td>
                    <a href="#" v-on:click="showModal=true;telegramUser=user">
                        <i class="material-icons">create</i>
                    </a>
                </td>
            </tr>
        </tbody>
      </table>
      
      <modal-window v-if="showModal" v-on:ok="sendMsg2User();showModal=false;" v-on:cancel="showModal=false;telegramUser = null">
          <h4 slot="header">Сообщение пользователю: {{telegramUser.firstName}} {{telegramUser.lastName}}</h4>
          <textarea slot="body" class="form-control" rows="7" v-model="message4Send" placeholder="Введите текст для отправки" style="border-color: #0275D8"></textarea>
          <span slot="okBtn">Отправить</span>
      </modal-window>
          <div class="pag-container">
            <b-v-pagination class="pag" size="md" :total-rows="botUsersCount" :per-page="dataRowsPerPage" v-model="currentPage"></b-v-pagination>
          </div>
      <message-modal-window v-bind="msgModalWindow" v-on:ok="msgModalWindow.isShow=false"></message-modal-window>    
    </main>

</div>
</template>
<script>
const getUsersUrl = "/api/user/all?beginPage=";
const sendMessage = "/api/user/";
import Vue from "vue"; 
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
const userToken = sessionStorage.getItem("userToken");
import MessageModalWindow from "../MessageModalWindow/messageModalWindow.vue";

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
      botUsers: [], 
      botUsersCount: 0,
      showModal: false,
      isLoading: true,
      currentPage: 1,
      dataRowsPerPage: 15,
      telegramUser: null,
      message4Send: "",
      messageFromServer: "",
      msgModalWindow: {
        isShow: false,
        headerText: "Внимание!",
        message2Show: "",
        timeOut2Show: 2000
      },
    };
  },
  methods: {
    getDataByPage: function(beginPage, endPage) {
      let thisComp = this;
      Vue.axios
        .get(getUsersUrl + beginPage + "&" + "endPage=" + endPage, {
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
            thisComp.botUsers = res.data.botUsers;
            thisComp.botUsersCount = res.data.botUsersCount;
            thisComp.isLoading = false;
          }
        })
        .catch(function(error) {
          thisComp.isLoading = false;
        });
    },

    sendMsg2User: function() {
      let thisComp = this;
      if (
        thisComp.message4Send &&
        thisComp.message4Send.length > 0 &&
        thisComp.telegramUser &&
        thisComp.telegramUser.chatId > 0
      ) {
        let message = thisComp.message4Send;
        axios
          .put(
            sendMessage + thisComp.telegramUser.chatId, {
              headers: jsonHeader(sessionStorage.getItem("userToken"))
            } 
          )
          .then(function(res) {
            if (res.status === 200) {
              thisComponent.messageFromServer = res.message;
            } else if (res.status === 401) {
              thisComp.showMsgModalWindow(
                true,
                ru.attention,
                "Cообщение не отправлено!",
                null
              );
            }
          })
          .catch(function(err) {
            thisComp.showMsgModalWindow(
                true,
                ru.attention,
                "Произошла ошибка при отправке сообщения!",
                null
              );
          });
      }
    },
    showMsgModalWindow: function(isShow, headerText, msgText, timeOut) {
      this.msgModalWindow.timeOut2Show = timeOut;
      this.msgModalWindow.isShow = isShow;
      this.msgModalWindow.headerText = headerText;
      this.msgModalWindow.message2Show = msgText;
    }
  },
  mounted: function () {
     let thisComp = this;
      Vue.axios
        .get("/api/user", {
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
            thisComp.botUsers = res.data.botUsers;
            thisComp.botUsersCount = res.data.botUsersCount;
            thisComp.isLoading = false;
          }
        })
        .catch(function(error) {
          thisComp.isLoading = false;
        });
  },
  watch: {
    currentPage: function(pageIndex) {
      let lastRow = this.currentPage * this.dataRowsPerPage;
      let firstRow = lastRow - this.dataRowsPerPage + 1;
      this.getDataByPage(firstRow, lastRow);
    }
  }
};
</script>
<style>
.pag-container {
  text-align: center;
}
.pag{
  display: inline-flex;
}
</style>


