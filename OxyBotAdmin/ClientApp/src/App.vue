<template>
  <router-view></router-view>
</template>
<script>
import Vue from 'vue';
export default {

  created: function() {
    this.$on("userAuthorised", function(res) {  

      sessionStorage.setItem("userToken", res.data.token);

      Vue.axios.interceptors.request.use(function(config){
        if (res.data.token) {
          config.headers["Authorization"] = "Bearer " + res.data.token;
          return config;
        }
      });

      this.$router.push({ path: "/api/send/msg" });
    });
  }
};
</script>
<style>
html,
body {
  height: 100%;
}
main{
  width: 100%;
  margin-top: 20pt;
  text-align: center;
}
</style>

