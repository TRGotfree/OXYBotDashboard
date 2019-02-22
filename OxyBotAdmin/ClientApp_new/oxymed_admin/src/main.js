import '@babel/polyfill'
import Vue from 'vue'
import './plugins/axios'
import './plugins/bootstrap-vue'
import App from './App.vue'
import router from './router'

Vue.config.productionTip = false

const token = sessionStorage.getItem("userToken");
if (token) {
   Vue.axios.defaults.headers.common['Authorization'] = token;
}

new Vue({
  router,
  render: h => h(App)
}).$mount('#app')
