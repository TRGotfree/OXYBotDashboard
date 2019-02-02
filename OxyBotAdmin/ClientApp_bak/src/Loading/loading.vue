<template>
    <div v-if="isLoading" class="loading">
        <h4>{{message}}</h4>
    </div>
</template>
<script>
export default {
    props: {
        isLoading:{
            type: Boolean,
            required: true,
            default: false
        },
        message:{
            type: String,
            required: false,
            default: "Загрузка данных..."
        }
    }
}
</script>
<style>
.loading {
  width: 100%;
  position: absolute;
  text-align: center;
  top: 50%;
  color: gray;
}
</style>


