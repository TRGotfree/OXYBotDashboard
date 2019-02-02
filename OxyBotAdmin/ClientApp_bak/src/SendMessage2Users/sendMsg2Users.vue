<template>
<div>
<header-navbar></header-navbar>
 <main>
    <h1>{{headerLabel}}</h1>
    <div class="col-md-8 pt-3" style="display: inline-block">
        <textarea class="form-control" rows="7" v-model="message4Send" placeholder="Введите текст для рассылки" style="border-color: #0275D8"></textarea>       
         <button type="button" class="btn btn-danger" style="margin: 10pt" v-on:click.prevent="sendMsg2AllUsers(message4Send)">Отправить сообшение</button>
         <div v-if="showAlert">
           <error-alert v-bind:message-from-server="messageFromServer"></error-alert>
          </div>
          <div v-else-if="showSuccess">
            <success-alert v-bind:message-from-server="messageFromServer"></success-alert>
          </div>
    </div>
 </main>
</div>
</template>

<script>
const url = "/api/send/msg";
import axios from "axios";
import ErrorAlert from "../Alerts/errorAlert.vue";
import SuccessAlert from "../Alerts/successAlert.vue";
import { ru } from "../../lang/ru-RU.js";
import HeaderNavbar from "../HeaderNavbar/header.vue";
import { jsonHeader } from "../../helper.js";

export default {
  components: {
    HeaderNavbar,
    ErrorAlert,
    SuccessAlert
  },
  data: function() {
    return {
      message4Send: "",
      messageFromServer: "",
      alert: {},
      showAlert: false,
      showSuccess: false,
      headerLabel: ru.sendMessage2Users
    };
  },
  methods: {
    sendMsg2AllUsers: function(msg, event) {
      let thisComponent = this;
      let message = msg;
      axios
        .post(url, JSON.stringify(message), {
          headers: jsonHeader(sessionStorage.getItem("userToken"))
        })
        .then(function(res) {
          if (res.status === 200) {
            thisComponent.messageFromServer = res.message;
          } else if (res.status === 401) {
            setTimeout(() => {
              thisComponent.messageFromServer = ru.notAuthorized;
              thisComponent.alert = thisComponent.ErrorAlert;
              thisComponent.showAlert = true;
            }, 3000);
            alert = {};
          }
        })
        .catch(function(err) {
          alert = thisComponent.ErrorAlert;
          thisComponent.showAlert = true;
          thisComponent.messageFromServer = ru.errorHappend;
        });
    }
  }
};
</script>

<style>
</style>

