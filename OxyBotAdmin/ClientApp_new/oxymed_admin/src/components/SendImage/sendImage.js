import NavBar from "../Header/Header.vue";
import Vue from "vue";

const url = "/api/send/img";

export default {
    components: {
        NavBar
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
            headerLabel: "Рассылка изображения пользователям",
            selectedImg: null
        }
    },
    methods: {
        chooseImg: function() {
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
              thisComp.messageFromServer = "Изображение не выбрано!";
              return;
            }
      
            if (this.message4Img && this.message4Img.length > 1024) {
              thisComp.showAlert = true;
              thisComp.messageFromServer = "Текст к картинке не может содержать более 1024 символов!";
              return;      
            }
      
            let thisComp = this;
            var formData = new FormData();

            formData.append("file", this.selectedImg);
            formData.append("message", this.message4Img);
            Vue.axios
              .post(url, formData, {
                headers: { "Content-Type": "multipart/form-data" },
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
                  thisComp.messageFromServer = "Изображение успешно отправлено";
                  thisComp.img_src = "https://mdbootstrap.com/img/Photos/Others/placeholder-avatar.jpg";
                  thisComp.message4Img = "";
                } else {
                  thisComp.showAlert = true;
                  thisComp.messageFromServer = "Изображение не отправлено!";
                }
              })
              .catch(function(error) {
                thisComp.showSuccess = false;
                if (error.response && error.response.data) {
                  thisComp.messageFromServer = error.response.data;
                  thisComp.showAlert = true;
                } else {
                  thisComp.messageFromServer = "Изображение не отправлено!";
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
}