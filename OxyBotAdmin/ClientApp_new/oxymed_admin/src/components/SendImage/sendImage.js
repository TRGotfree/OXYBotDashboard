import NavBar from "../Header/Header.vue";
import sendImageService from "../../services/sendImageService";

export default {
  components: {
    NavBar
  },
  data: function () {
    return {
      img_src: "https://mdbootstrap.com/img/Photos/Others/placeholder-avatar.jpg",
      alertMessage: "",
      message4Img: "",
      showDangerAlert: false,
      showWarningAlert: false,
      showSuccessAlert: false,
      headerLabel: "Рассылка файла или изображения пользователям",
      selectedImg: null,
      fileForSend: null,
      isSendButtonDisabled: false,
      isFileChooseShowing: true,
      isImageChooseShowing: true
    }
  },
  methods: {
    imageSelected: function (e) {

      if (!e.target.files || e.target.files.length === 0 || !e.target.files[0])
        return;

      this.selectedImg = e.target.files[0];
      this.img_src = URL.createObjectURL(this.selectedImg);
    },

    fileChoosed: function (e) {
      if (!e.target.files || e.target.files.length === 0 || !e.target.files[0])
        return;

      this.fileForSend = e.target.files[0];
    },

    send: async function () {
      try {

        if (!this.selectedImg && !this.fileForSend) {
          this.showDangerAlert = true;
          this.messageFromServer = "Изображение или файл не выбраны!";
        }

        if (this.message4Img && this.message4Img.length > 1024) {
          this.showDangerAlert = true;
          this.messageFromServer = "Текст к картинке или файлу не может содержать более 1024 символов!";
        }

        this.isSendButtonDisabled = true;
        this.showWarningAlert = true;
        this.alertMessage = "Идёт рассылка... Не закрывайте окно браузера!";

        let sendResult = null;

        if (this.selectedImg && !this.fileForSend) {
          const imageData = {
            image: this.selectedImg,
            textForImage: this.message4Img
          };
          sendResult = await sendImageService.sendImageToAllUsers(imageData);
        } else if (!this.selectedImg && this.fileForSend) {
          const fileData = {
            file: this.selectedImg,
            textForfile: this.message4Img
          };
          sendResult = await sendImageService.sendFileToAllUsers(fileData);
        } else {
          throw new Error("Something goes wrong!");
        }

        this.alertMessage = sendResult.message;
        if (sendResult.isSucessfully) {
          this.showSuccessAlert = true;
          this.img_src = "https://mdbootstrap.com/img/Photos/Others/placeholder-avatar.jpg";
          this.message4Img = "";
          this.selectedImg = null;
          this.fileForSend = null;
          this.isFileChooseShowing, this.isImageChooseShowing = true
        } else
          this.showDangerAlert = true;

      } catch (error) {
        this.showDangerAlert = true;
        this.alertMessage = error.toString();
      }

      this.isSendButtonDisabled = false;
    }
  },
  computed: {
    showSendBtn: function () {
      if (this.selectedImg)
        return true;
    }
  },
  watch: {
    showDangerAlert: function (isShow) {
      if (isShow) {
        this.showWarningAlert = false;
        this.showSuccessAlert = false;
      }
    },
    showWarningAlert: function (isShow) {
      if (isShow) {
        this.showDangerAlert = false;
        this.showSuccessAlert = false;
      }
    },
    showSuccessAlert: function (isShow) {
      if (isShow) {
        this.showWarningAlert = false;
        this.showSuccessAlert = false;
      }
    },
    fileForSend: function (file) {
      if (file) {
        this.selectedImg = null;
        this.isImageChooseShowing = false;
        this.img_src = "";
      }
    },
    selectedImg: function (image) {
      if (image) {
        this.fileForSend = null;
        this.isFileChooseShowing = false;
      }
    }
  },
}