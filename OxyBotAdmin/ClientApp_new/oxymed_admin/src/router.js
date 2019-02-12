import Vue from 'vue'
import Router from 'vue-router'
import Login from './components/Login/Login.vue'
import SendMessage2Users from './components/SendMessage/SendMessage.vue'

Vue.use(Router)

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
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
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (about.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import(/* webpackChunkName: "about" */ './views/About.vue')
    }
  ],
  linkActiveClass: "active",
  linkExactActiveClass: "active"
})
