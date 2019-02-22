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
      message: ""
    }
  },
  methods: {
    onSubmit: function () {

      let thisComp = this;

      if (!this.login) {
        this.message = "Введите логин!"
        this.showAlert = true;
        return;
      }

      if (!this.pass) {
        this.message = "Введите пароль!"
        this.showAlert = true;
        return;
      }

      const botAdmin = {
        login: this.login,
        password: this.pass
      };

      Vue.axios.post(url, botAdmin)
        .then(function (res) {

          const userToken = "Bearer " + res.data.token;
          sessionStorage.setItem("userToken", userToken);

          Vue.axios.defaults.headers.common["Authorization"] = userToken;
          Vue.axios.defaults.headers.common["Content-Type"] = "application/json";
          // Vue.axios.interceptors.request.use(function (config) {
          //   if (res.data.token) {
          //     config.headers["Authorization"] = userToken;
          //     config.headers["Content-Type"] = "application/json";
          //     return config;
          //   }
          // });

          thisComp.$router.push({
            name: "sendMessageToUsers"
          });

        })
        .catch(function (err) {
          if (err.response && err.response.status === 403) {
            thisComp.error = true;
            thisComp.message = "Неверный логин или пароль!";
          } else {
            thisComp.error = true;
            thisComp.message = "Произошла непредвиденная ошибка!";
          }
          thisComp.showAlert = true;
        });

    }
  },
}