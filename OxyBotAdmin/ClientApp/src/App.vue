<template>
  <router-view></router-view>
</template>
<script>
export default {
  created: function() {
    this.$on("userAuthorised", function(res) {   
      sessionStorage.setItem("userToken", res.data.token);
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

