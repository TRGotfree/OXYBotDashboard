<template>
  <router-view></router-view>
</template>
<script>
import axios from "axios";

export default {

  created: function() {
    this.$on("userAuthorised", function(res) {   
      sessionStorage.setItem("userToken", res.data.token);

      axios.interceptors.request.use(function(config){
        if (userToken) {
          config.headers.Authorization = userToken;
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

