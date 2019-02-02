import Vue from 'vue';
import VueRouter from 'vue-router';
import 'jquery/dist/jquery.min';
import 'bootstrap/dist/js/bootstrap.min';
import BootstrapVue from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-vue/dist/bootstrap-vue.css'
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