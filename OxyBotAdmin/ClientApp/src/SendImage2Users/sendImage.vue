<template>
<div>
<header-navbar></header-navbar>
    <main>         
        <div class="container">
              <h2>{{headerLabel}}</h2>
              <img ref="img-4-send" v-bind:src="img_src" class="img-fluid" alt="Выберите изображение">
              <textarea rows="3" v-model="message4Img" placeholder="Введите текст к картинке"></textarea>           
            <div v-if="showAlert">
              <error-alert v-bind:message="messageFromServer"></error-alert>
            </div>
            <div v-else-if="showSuccess">
              <success-alert v-bind:message="messageFromServer"></success-alert>
            </div>
            <div style="margin-top: 10pt">
              <div class="btn-group">
                <button class="btn btn-primary" v-on:click="chooseImg">Выбрать изображение</button>
              </div>
              <input ref="input_img" type="file" v-on:change="imageSelected" style="display: none">
              <div v-if="showSendBtn === true" class="btn-group">
                <button class="btn btn-danger" v-on:click="sendImage">Отправить изображение</button>
              </div>
            </div>
        </div>   
    </main>
    </div>
</template>
<script>
const url = "/api/send/img";
import axios from 'axios';
import ErrorAlert from "../Alerts/errorAlert.vue";
import SuccessAlert from "../Alerts/successAlert.vue";
import { ru } from "../../lang/ru-RU.js";
import HeaderNavbar from "../HeaderNavbar/header.vue";
import { formDataHeader } from "../../helper.js";
export default {
  components: {
    HeaderNavbar,
    ErrorAlert,
    SuccessAlert
  },
  data: function() {
    return {
      img_src:
        "https://mdbootstrap.com/img/Photos/Others/placeholder-avatar.jpg",
      messageFromServer: "",
      message4Img: "",
      alert: {},
      showAlert: false,
      showSuccess: false,
      headerLabel: ru.sendImg2Users,
      selectedImg: null
    };
  },
  methods: {
    chooseImg: function(event) {
      this.$refs.input_img.click();
    },
    imageSelected: function(e) {
      if (
        !e.target.files ||
        e.target.files.length === 0 ||
        !e.target.files[0]
      ) {
        return;
      }
      this.selectedImg = e.target.files[0];
      this.img_src = URL.createObjectURL(this.selectedImg);
    },

    sendImage: function() {
      
      if (!this.selectedImg) {
        thisComp.showAlert = true;
        thisComp.messageFromServer = ru.imgNotChoosed;
        return;
      }

      if (this.message4Img && this.message4Img.length > 1024) {
        thisComp.showAlert = true;
        thisComp.messageFromServer = "Текст к картинке не может одержать более 1024 символов!";
        return;      
      }

      let thisComp = this;
      var formData = new FormData();
      var fileName = this.selectedImg.name;
      formData.append("file", this.selectedImg);
      formData.append("message", this.message4Img);
      axios
        .post(url, formData, {
          headers: formDataHeader(sessionStorage.getItem("userToken")),
          onUploadProgress: function(uploadEvent) {
            thisComp.showAlert = true;
            thisComp.messageFromServer = "Загрузка изображения на сервер: " +
              Math.round((uploadEvent.loaded / uploadEvent.total) * 100) + "%";
            if (Math.round((uploadEvent.loaded / uploadEvent.total) * 100) >= 100) {
              thisComp.messageFromServer = "Рассылка изображения пользователям...";
            }   
          }
        })
        .then(function(res) {
          if (res && res.status === 200) {
            thisComp.showAlert = false;
            thisComp.showSuccess = true;
            thisComp.messageFromServer = ru.imgLoadedSuccessfully;
            thisComp.img_src = "https://mdbootstrap.com/img/Photos/Others/placeholder-avatar.jpg";
            thisComp.message4Img = "";
          } else {
            thisComp.showAlert = true;
            thisComp.messageFromServer = ru.imgNotUploaded;
          }
        })
        .catch(function(error) {
          if (error.response && error.response.data) {
            thisComp.messageFromServer = error.response.data;
            thisComp.showAlert = true;
          } else {
            thisComp.messageFromServer = ru.imgNotUploaded;
            thisComp.showAlert = true;
          }
        });
    }
  },
  computed: {
    showSendBtn: function() {
      if (this.selectedImg) {
        return true;
      }
    }
  }
};
</script>
<style>
.img-fluid {
  width: auto;
  height: auto;
  max-width: 100%;
  max-height: 220pt;
}
.container {
  max-width: 350pt;
  position: relative;
  text-align: center;
}
textarea {
  border-color: #0275d8;
  margin-top: 10pt;
  width: 100%;
}
</style>
