/* eslint-disable no-console */
import BForm from "bootstrap-vue/es/components/form/form";
import BFormText from "bootstrap-vue/es/components/form/form-text";
import BFormInvalidFeedback from "bootstrap-vue/es/components/form/form-invalid-feedback";
import BFormValidFeedback from "bootstrap-vue/es/components/form/form-valid-feedback";
import BFormRow from "bootstrap-vue/es/components/form/form-row";
import Vue from "vue";

const url = "/login/auth";

export default {
    components: {
        BForm,
        BFormText,
        BFormInvalidFeedback,
        BFormValidFeedback,
        BFormRow
      },
    data: function () {
        return {
            login: "",
            pass: "",
            showAlert: false,
            message: "Неверный логин или пароль"
        }
    },
    methods: {
        onSubmit: function(){
            
            console.log("Submiting...");
            
            this.showAlert = !this.showAlert;
            let thisComp = this;
            this.$router.push({ name: "sendMessageToUsers" });
            // Vue.axios.post(url, thisComp.botAdmin)
            // .then(function (res) {
            //     if (res.status === 200) {
                   
            //         sessionStorage.setItem("userToken", res.data.token);

            //         Vue.axios.interceptors.request.use(function(config){
            //           if (res.data.token) {
            //             config.headers["Authorization"] = "Bearer " + res.data.token;
            //             return config;
            //           }
            //         });
              
            //         this.$router.push({ name: "sendMessageToUsers" });
            //     }
            // })
            // .catch(function (err) {
            //   if (err.response.status === 403) {
            //     thisComp.error = true;
            //     thisComp.messageFromServer = "Неверный логин или пароль!";
            //   } else {
            //     thisComp.error = true;
            //     thisComp.messageFromServer = "Произошла непредвиденная ошибка!";
            //   }
            // });

        }
    },
}