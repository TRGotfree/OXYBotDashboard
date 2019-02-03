import axios from 'axios';
import {
  ru
} from '../../lang/ru-RU';

const url = "/login/auth";

export default {
  data: function () {
    let componentData = {
      botAdmin: {
        Login: "",
        Password: ""
      },
      messageFromServer: ru.wrongPassOrLogin,
      error: false
    }
    return componentData;
  },
  methods: {

    checkAuth: function () {
      let thisComp = this;
      let parentComp = this.$parent;
      axios.post(url, thisComp.botAdmin)
        .then(function (res) {
          if (res.status === 200) {
            parentComp.$emit('userAuthorised', res);
          }
        })
        .catch(function (err) {
          if (err.response.status === 403) {
            thisComp.error = true;
            thisComp.messageFromServer = ru.wrongPassOrLogin;
          } else {
            thisComp.error = true;
            thisComp.messageFromServer = ru.errorHappend;
          }
        });
    }
  }
}