import Vue from 'vue'
import Router from 'vue-router'
import Login from './components/login/Login.vue'
import SendMessage2Users from './components/SendMessage/SendMessage.vue'
import SendImageToUsers from './components/SendImage/SendImage.vue';
import Users from "./components/Users/Users.vue";
import Actions from "./components/Actions/Actions.vue";
import DrugStore from "./components/DrugStore/DrugStore.vue";
import UserRequests from "./components/UserRequests/UserRequest.vue";
import Annotations from "./components/Annotations/Annotation.vue";
import CreateAnnotation from "./components/Annotations/CreateAnnotation.vue";
import Card from "./components/Card/Card.vue";

Vue.use(Router)

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [{
      path: '/',
      name: 'login',
      component: Login
    },
    {
      path: "/api/send/msg",
      name: "sendMessageToUsers",
      component: SendMessage2Users
    },
    {
      path: "/api/send/img",
      name: "sendImage",
      component: SendImageToUsers
    },
    {
      path: "/api/user",
      component: Users,
      name: "users"
    },
    {
      path: "/api/actions",
      component: Actions,
      name: "actions"
    }, 
    {
      path: "/api/drugstore",
      component: DrugStore,
      name: "drugStores"
    },
    {
      path: "/api/request",
      component: UserRequests,
      name: "userRequests"
    },
    {
      path: "/api/annotation",
      component: Annotations,
      name: "annotations"
    },
    {
      path: "/api/annotation/create",
      component: CreateAnnotation,
      name: "annotation_create"
    },
    {
      path: "/api/discount",
      component: Card,
      name: "discount_card"
    }
    // ,
    // {
    //   path: '/about',
    //   name: 'about',
    //   // route level code-splitting
    //   // this generates a separate chunk (about.[hash].js) for this route
    //   // which is lazy-loaded when the route is visited.
    //   component: () => import( /* webpackChunkName: "about" */ './views/About.vue')
    // }
  ],
  linkActiveClass: "active",
  linkExactActiveClass: "active"
})