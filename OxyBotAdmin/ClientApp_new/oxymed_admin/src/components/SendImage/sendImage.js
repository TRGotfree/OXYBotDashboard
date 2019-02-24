/* eslint-disable no-console */
import NavBar from "../Header/Header.vue";
import sendImageService from "../../services/sendImageOrFileService";

export default {
  components: {
    NavBar
  },
  data: function () {
    return {
      img_src: "",
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
      isImageChooseShowing: true,
      isImagePreviewShowing: false
    }
  },
  methods: {
    imageSelected(e) {

      if (!e.target.files || e.target.files.length === 0 || !e.target.files[0])
        return;

      this.selectedImg = e.target.files[0];
      this.img_src = URL.createObjectURL(this.selectedImg);
    },

    fileChoosed(e) {
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
            textForImage: !this.message4Img ? "" : this.message4Img
          };
          
          sendResult = await sendImageService.sendImageToAllUsers(imageData);

        } else if (!this.selectedImg && this.fileForSend) {
          
          const fileData = {
            file: this.fileForSend,
            textForFile: !this.message4Img ? "" : this.message4Img
          };

          sendResult = await sendImageService.sendFileToAllUsers(fileData);
           
        } else {
          throw new Error("Something goes wrong!");
        }

        this.alertMessage = sendResult.message;

        if (sendResult.isSucessfully) {
          this.showSuccessAlert = true;
          this.img_src = "";
          this.message4Img = "";
          this.selectedImg = null;
          this.fileForSend = null;
          this.isFileChooseShowing, this.isImageChooseShowing = true
        } else
          this.showDangerAlert = true;

      } catch (error) {
        this.showDangerAlert = true;
        this.alertMessage = "Произошла непредвиденная ошибка!";
      }

      this.isSendButtonDisabled = false;
    },

    cancel() {
      this.selectedImg= null;
      this.fileForSend= null;
      this.isSendButtonDisabled= false;
      this.isFileChooseShowing= true;
      this.isImageChooseShowing= true;
      this.isImagePreviewShowing= false;
      this.$refs.imageInput.reset();
      this.$refs.fileInput.reset();
    }

  },
  computed: {
    showSendBtn() {
      if (this.selectedImg || this.fileForSend)
        return true;
      else
        return false;  
    }
  },
  watch: {
    showDangerAlert(isShow) {
      if (isShow) {
        this.showWarningAlert = false;
        this.showSuccessAlert = false;
      }
    },
    showWarningAlert(isShow) {
      if (isShow) {
        this.showDangerAlert = false;
        this.showSuccessAlert = false;
      }
    },
    showSuccessAlert(isShow) {
      if (isShow) {
        this.showWarningAlert = false;
        this.showSuccessAlert = false;
      }
    },
    fileForSend(file) {
      if (file) {
        this.selectedImg = null;
        this.isImageChooseShowing = false;
        this.img_src = "";
      }else{
        this.isImageChooseShowing = true;
      }
    },
    selectedImg(image) {
      if (image) {
        this.fileForSend = null;
        this.isFileChooseShowing = false;
      }else{
        this.isFileChooseShowing = true;
        this.img_src = "";
      }
    },
    img_src(imageSrc) {
      if (imageSrc) {
         this.isImagePreviewShowing = true;
      }else{
        this.isImagePreviewShowing = false;
      }
    },
    alertMessage(message){
      if (!message) {
          this.showDangerAlert = false;
          this.showWarningAlert = false;
          this.showSuccessAlert = false;
      }
  }
  },
}