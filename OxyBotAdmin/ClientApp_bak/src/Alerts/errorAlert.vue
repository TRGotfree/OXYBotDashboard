<template>
    <div class="alert alert-danger" role="alert" style="margin: 10pt, 0pt, 10pt, 0pt">{{message}}</div>
</template>
<script>
export default {
    props: {
        message:{
            type: String,
            required: true,
            default: "Ошибка!!!"
        }
    }
}
</script>
<style></style>