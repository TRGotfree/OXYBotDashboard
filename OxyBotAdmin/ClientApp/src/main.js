import Vue from 'vue';
import VueRouter from 'vue-router';
import 'jquery/dist/jquery.min';
import 'bootstrap/dist/js/bootstrap.min';
import BootstrapVue from 'bootstrap-vue';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';
import App from './App.vue';
import Login from "./LoginForm/login.vue";
import SendMessage2Users from "./SendMessage2Users/sendMsg2Users.vue";
import SendImage from "./SendImage2Users/sendImage.vue";
import Users from "./Users/users.vue";
import Actions from "./Actions/actons.vue";
import DrugStore from "./DrugStore/drugStore.vue";
import UserRequest from "./UserRequests/requests.vue";
import Annotation from "./Annotations/annotations.vue";
import CreateAnnotation from "./Annotations/createAnnotation.vue";

const config = {
  baseURL: "/"
}

const _axios = axios.create(config);

_axios.interceptors.request.use(
  function(config) {
    // Do something before request is sent
    return config;
  },
  function(error) {
    // Do something with request error
    return Promise.reject(error);
  }
);

// Add a response interceptor
_axios.interceptors.response.use(
  function(response) {
    // Do something with response data
    return response;
  },
  function(error) {
    // Do something with response error
    return Promise.reject(error);
  }
);

Plugin.install = function(Vue, options) {
  Vue.axios = _axios;
  window.axios = _axios;
  Object.defineProperties(Vue.prototype, {
    axios: {
      get() {
        return _axios;
      }
    },
    $axios: {
      get() {
        return _axios;
      }
    },
  });
};

Vue.use(Plugin);

Vue.use(BootstrapVue);
Vue.use(VueRouter);

const routes = [{
    path: "/",
    component: Login
  },
  {
    path: "/api/login/auth",
    component: Login
  },
  {
    path: "/api/send/msg",
    component: SendMessage2Users
  },
  {
    path: "/api/send/img",
    component: SendImage
  },
  {
    path: "/api/user",
    component: Users
  },
  {
    path: "/api/advertising",
    component: Actions
  },
  {
    path: "/api/drugstore",
    component: DrugStore
  },
  {
    path: "/api/request",
    component: UserRequest
  },
  {
    path: "/api/annotation",
    component: Annotation
  },
  {
    path: "/api/annotation/create",
    component: CreateAnnotation
  }
];

const router = new VueRouter({
  mode: "history",
  routes: routes,
  linkActiveClass: "active",
  linkExactActiveClass: "active"
});

new Vue({
  el: "#app",
  router,
  render: component => component(App)
});