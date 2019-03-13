<template>
  <div>
    <nav-bar></nav-bar>
    <b-container>
      <b-form-group
        label="ID товара:"
        label-for="goodId"
        description="Идентификатор товара из 'Аналитики' для связи справочника бота"
      >
        <b-form-input
          id="goodId"
          type="number"
          required
          placeholder="ID товара"
          v-model="annotation.annotationId"
        ></b-form-input>
      </b-form-group>

      <b-form-group label="Название товара" label-for="goodName">
        <b-form-input
          id="goodName"
          type="text"
          required
          placeholder="Название товара"
          v-model="annotation.drugName"
        ></b-form-input>
      </b-form-group>

      <b-form-group label="Производитель" label-for="producer">
        <b-form-input
          id="producer"
          type="text"
          placeholder="Производитель"
          v-model="annotation.producer"
        ></b-form-input>
      </b-form-group>

      <b-form-group label="Способ применения" label-for="usingWay">
        <b-form-textarea
          id="usingWay"
          rows="5"
          placeholder="Способ применения"
          v-model="annotation.usingWay"
        ></b-form-textarea>
      </b-form-group>

      <b-form-group label="Предназначение" label-for="forWhatIsUse">
        <b-form-textarea
          id="forWhatIsUse"
          rows="3"
          placeholder="Предназначение"
          v-model="annotation.forWhatIsUse"
        ></b-form-textarea>
      </b-form-group>

      <b-form-group label="Спец. указания" label-for="specialInstructions">
        <b-form-textarea
          id="specialInstructions"
          rows="3"
          placeholder="Спец. указания"
          v-model="annotation.specialInstructions"
        ></b-form-textarea>
      </b-form-group>

      <b-form-group label="Противопоказания" label-for="contraindicators">
        <b-form-textarea
          id="contraindicators"
          rows="3"
          placeholder="Противопоказания"
          v-model="annotation.contraIndicators"
        ></b-form-textarea>
      </b-form-group>

      <b-form-group label="Побочные эффекты" label-for="sideEffects">
        <b-form-textarea
          id="sideEffects"
          rows="3"
          placeholder="Побочные эффекты"
          v-model="annotation.sideEffects"
        ></b-form-textarea>
      </b-form-group>

      <b-form-group v-bind:label="imageChoosed">
        <b-form-file
          ref="imageInput"
          v-show="isImageChooseShowing"
          v-model="selectedImg"
          :state="Boolean(selectedImg)"
          accept="image/jpeg, image/png"
          placeholder="Выберите изображение!"
          drop-placeholder="Перетащите файл сюда!"
          v-on:change="imageSelected"
        />
      </b-form-group>

      <div class="submit-btn">
        <b-button
          variant="danger"
          v-on:click="saveNewAnnotation(annotation)"
        >Сохранить данные по аннотации</b-button>
      </div>

      <b-alert
        variant="danger"
        dismissible
        fade
        :show="showDangerAlert"
        @dismissed="showDangerAlert = !showDangerAlert"
      >{{alertMessage}}</b-alert>
      <b-alert
        variant="success"
        dismissible
        fade
        :show="showSuccessAlert"
        @dismissed="showSuccessAlert = !showSuccessAlert"
      >{{alertMessage}}</b-alert>
    </b-container>
  </div>
</template>
<script>
import annotationService from "../../services/annotationService";
import NavBar from "../Header/Header.vue";

export default {
  components: {
    NavBar
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
      messageFromServer: "",
      selectedImg: null,
      img_src: "",
      imageChoosed: "Изображение не выбрано",
      imageLabelStyle: "color:red",
      showDangerAlert: false,
      alertMessage: "",
      showSuccessAlert: false,
      isImageChooseShowing: true
    };
  },
  methods: {
    async saveNewAnnotation() {
      try {
        if (!this.annotation) throw new Error("Данные по аннотации пусты!");

        if (!this.annotation.annotationId)
          throw new Error("Не указан Id товара для текущей аннотации!");

        if (!this.annotation.drugName)
          throw new Error("Необходимо указать название товара!");

        if (!this.annotation.producer)
          throw new Error("Необходимо указать производителя!");

        let resultFromServer = null;

        this.alertMessage = "Загрузка данных по аннотации на сервер...";
        this.showDangerAlert = true;
        var formData = new FormData();

        if (this.selectedImg) formData.append("file", this.selectedImg);

        for (let key in this.annotation)
          formData.append(key, this.annotation[key]);

        resultFromServer = await annotationService.saveNewAnnotation(formData);

        if (resultFromServer.isSuccessfully) {
          this.alertMessage = resultFromServer.message
            ? resultFromServer.message
            : "Данные по аннотации успешно сохранены!";

          this.showSuccessAlert = true;

          if (this.annotation.annotationId <= 0) {
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
          this.alertMessage = resultFromServer.message
            ? resultFromServer.message
            : "Данные не сохранены!";
          this.showDangerAlert = true;
        }
      } catch (error) {
        this.showDangerAlert = true;
        this.alertMessage = error.toString();
      }
    },

    imageSelected: function(e) {
      if (!e.target.files || e.target.files.length === 0 || !e.target.files[0])
        return;

      this.isImageChooseShowing = true;

      this.selectedImg = e.target.files[0];
    }
  },
  watch: {
    selectedImg: function(imageData) {
      if (imageData) {
        this.imageChoosed = "Изображение товара выбрано";
        this.imageLabelStyle = "color:#33bc2e;";
      }
    },
    showDangerAlert(isShowing) {
      if (isShowing) this.showSuccessAlert = false;
    },
    showSuccessAlert(isShowing) {
      if (isShowing) this.showDangerAlert = false;
    },
    alertMessage(message) {
      if (!message) {
        this.showDangerAlert = false;
        this.showSuccessAlert = false;
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

.submit-btn {
  margin: auto;
  text-align: center;
  margin-bottom: 3%;
}
</style>

