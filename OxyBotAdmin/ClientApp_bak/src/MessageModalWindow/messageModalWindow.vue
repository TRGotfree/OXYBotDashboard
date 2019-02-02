<template>
<div v-if="isShow">
   <transition name="modal">
    <div class="modal-mask">
      <div class="modal-wrapper">
        <div class="msg-modal-container">
          <div class="modal-header">
            <h3>{{headerText}}</h3>
          </div>
          <div class="modal-body">
            <h4>{{message2Show}}</h4>
          </div>
          <div class="modal-footer">
            <button
                 class="modal-default-button btn btn-primary" @click="$emit('ok')">OK
            </button>
          </div>
        </div>
       </div>
    </div>
    </transition>
    </div>
</template>
<script>
export default {
    props: {
        isShow:{
            required: true,
            type: Boolean,
            default: false
        },
        headerText:{
            required: false,
            type: String,
            default: "Внимание!"
        },
        message2Show:{
            required: true,
            type: String
        },
        timeOut2Show:{
            required: false,
            type: Number,
            default: 2000
        }
    },
    watch:{
        isShow: function(){
            let thisComp = this;
            if (this.timeOut2Show && this.timeOut2Show > 0) {
                setTimeout(function(){
                  thisComp.$emit('ok');
                }, this.timeOut2Show)
            }
        }
    }
}
</script>
<style>
.modal-mask {
  position: fixed;
  z-index: 9998;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, .5);
  transition: opacity .3s ease;
}
.msg-modal-container {
  width: 60%;
  margin: 0px auto;
  margin-top: 10%;
  padding: 5px 8px;
  background-color: #fff;
  border-radius: 2px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, .33);
  transition: all .3s ease;
  font-family: Helvetica, Arial, sans-serif;
}
.modal-header h3 {
  margin-top: 0;
  color: #42b983;
}
.modal-body {
  margin: 5px 0;
}
.modal-default-button {
  float: right;
}
.modal-enter {
  opacity: 0;
}
.modal-leave-active {
  opacity: 0;
}
.modal-enter .modal-container,
.modal-leave-active .modal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}
</style>
