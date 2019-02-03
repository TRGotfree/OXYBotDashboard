<template>
  <div>
    <header-navbar></header-navbar>
    <!-- <loading v-bind:isLoading="isLoading"></loading> -->
    <div class="create-annotation">
      <!-- <b-form> -->
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
        </b-form-group>
        <b-form-group v-bind:label="imageChoosed" v-bind:style="imageLabelStyle">
          <div class="btn-group">
            <button class="btn btn-primary" v-on:click="chooseImg">Выбрать изображение</button>
          </div>
          <input ref="input_img" type="file" v-on:change="imageSelected" style="display: none">
        </b-form-group>
        
        <div class="submit-btn">
          <b-button variant="danger" v-on:click="saveAnnotation(annotation)">Сохранить данные по аннотации</b-button>
        </div>
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
      imageChoosed: "Изображение не выбрано",
      imageLabelStyle: "color:red"
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

        for(let key in goodAnnotation) {
          formData.append(key, goodAnnotation[key]);
        }

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
    showMsgModalWindow: function(isShow, headerText, msgText, timeOut) {
      this.msgModalWindow.timeOut2Show = timeOut;
      this.msgModalWindow.isShow = isShow;
      this.msgModalWindow.headerText = headerText;
      this.msgModalWindow.message2Show = msgText;
    }
  },
  watch: {
    img_src: function(imageSrc) {
      if (imageSrc && imageSrc.length > 0) {
        this.imageChoosed = "Изображение товара выбрано";
        this.imageLabelStyle = "color:#33bc2e;"
      }    
    }
  }
};
</script>
<style>
.create-annotation {
  width: 70%;
  margin: auto;
  margin-top: 1rem;
  margin-bottom: 1rem;
}
.create-annotation-btn {
  float: right;
  margin: 0 20pt 20pt 0;
}
.done {
  color: darkgray;
}

.submit-btn{
  margin: auto;
  text-align: center;
}

</style>

