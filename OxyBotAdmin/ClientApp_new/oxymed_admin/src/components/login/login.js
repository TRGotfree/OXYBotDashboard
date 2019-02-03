/* eslint-disable no-console */
import BForm from "bootstrap-vue/es/components/form/form";
import BFormText from "bootstrap-vue/es/components/form/form-text";
import BFormInvalidFeedback from "bootstrap-vue/es/components/form/form-invalid-feedback";
import BFormValidFeedback from "bootstrap-vue/es/components/form/form-valid-feedback";
import BFormRow from "bootstrap-vue/es/components/form/form-row";
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
        }
    },
}