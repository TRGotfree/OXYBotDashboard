<template>
<div>
    <header-navbar></header-navbar>
    <loading v-bind:isLoading="isLoading"></loading>
    
    <main v-if="!isLoading">

    <div class="btn-group create-drugStore-btn">
        <button class="btn btn-danger" v-on:click="createDrugStore()">Добавить аптеку</button>
    </div>   

      <table class="table table-hover">
            <thead>
                <tr>
                    <th>Название аптеки</th>
                    <th>Адрес</th>
                    <th>Телефон</th>
                    <th>Район</th>
                    <th>Статус</th>
                    <th>Подробнее</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="drugStore in drugStores" :key="drugStore.id">
                    <td>{{drugStore.drugStoreName}}</td>
                    <td>{{drugStore.address}}</td>
                    <td>{{drugStore.phone}}</td>
                    <td>{{drugStore.district}}</td>
                    <td>
                        <a href="#">
                            <i v-if="drugStore.status" class="material-icons done_all" v-on:click="drugStore.status=false; saveDrugStore(drugStore, $event)">done_all</i>
                            <i v-else class="material-icons done" v-on:click="drugStore.status=true; saveDrugStore(drugStore, $event)">done</i>
                        </a>
                    </td>
                    <td>
                        <a href="#" class="material-icons search" v-on:click="selectedDrugStore=drugStore;showModal=true;">search</a>
                    </td>
                </tr>
            </tbody>
      </table>

        <div class="pag-container">
          <b-v-pagination class="pag" size="md" :total-rows="drugStoreTotalCount" :per-page="dataRowsPerPage" v-model="currentPage"></b-v-pagination>            
        </div>

        <modal-window v-if="showModal" v-on:ok="saveDrugStore(selectedDrugStore)" v-on:cancel="showModal=false">
            <h4 slot="header">{{selectedDrugStore.drugStoreName}}</h4>
            <form slot="body">
                <div class="form-group row">
                  <label for="drugStore-id" class="col-3 col-form-label text-right">Id аптеки из "Аналитики"</label>
                  <div class="col-9">
                    <input type="number" class="form-control" v-model="selectedDrugStore.drugStoreId" id="drugStore-id">
                  </div> 
                </div>
                <div class="form-group row">                                   
                  <label for="drugStore-name" class="col-3 col-form-label text-right">Название</label>
                  <div class="col-9">
                    <input type="text" class="form-control" v-model="selectedDrugStore.drugStoreName" id="drugStore-name" placeholder="Название аптеки">
                  </div>  
                </div>
                <div class="form-group row">
                  <label for="drugStore-orientir" class="col-3 col-form-label text-right">Ориентир</label>
                  <div class="col-9">
                    <input type="text" class="form-control" id="drugStore-orientir" v-model="selectedDrugStore.orientir">
                  </div>                  
                </div>
                <div class="form-group row">
                  <label for="drugStore-phone" class="col-3 col-form-label text-right">Телефон</label>
                  <div class="col-3">
                    <input type="text" class="form-control" placeholder="Телефон" id="drugStore-phone" v-model="selectedDrugStore.phone"/>
                  </div>                  
                  
                  <label for="drugStore-worktime" class="col-3 col-form-label text-right">Режим работы</label>
                  <div class="col-3">
                    <input type="text" class="form-control" placeholder="Режим работы" id="drugStore-worktime" v-model="selectedDrugStore.workTime">
                  </div>
                  
                </div>

                <div class="form-group row">
                  <label for="dropdownMenuButton" class="col-3 col-form-label text-right">Город/Район</label>
                  <div class="col-9 dropdown">
                    <button class="btn btn-default dropdown-toggle text-left drop-down-btn" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                      {{selectedDrugStore.district}}    
                    </button>
                    <div class="dropdown-menu scrollable-menu" aria-labelledby="dropdownMenuButton">
                        <a v-for="distr in districts" :key="distr" class="dropdown-item" href="#" v-on:click="selectedDrugStore.district=distr">{{distr}}</a>
                    </div>
                  </div>
                </div>

                 <div class="form-group row">
                  <label for="drugStore-address" class="col-3 col-form-label text-right">Адрес</label>
                  <div class="col-9">
                    <input type="text" class="form-control" placeholder="Адрес" id="drugStore-address" v-model="selectedDrugStore.address">
                  </div>
                </div>

            </form>
            <span slot="okBtn">Сохранить</span>
        </modal-window>
        <message-modal-window v-bind="msgModalWindow" v-on:ok="msgModalWindow.isShow=false"></message-modal-window>
    </main>
</div>
</template>

<script>

const getDrugStoresUrl = "/api/drugstore?beginPage=";
const updInsertDrugStoreUrl = "/api/drugstore";
const getDistricts = "/api/district";
import Vue from "vue";
import { ru } from "../../lang/ru-RU.js";
import HeaderNavbar from "../HeaderNavbar/header.vue";
import { authorizationHeader } from "../../helper.js";
import { jsonHeader } from "../../helper.js";
import BVPagination from "bootstrap-vue/es/components/pagination/pagination";
import ModalWindow from "../ModalWindow/ModalWindow.vue";
import Loading from "../Loading/loading.vue";
import MessageModalWindow from "../MessageModalWindow/messageModalWindow.vue";

const validationOK = "OK";

export default {
  components: {
    HeaderNavbar,
    BVPagination,
    ModalWindow,
    Loading,
    MessageModalWindow
  },
  
  data: function() {
    return {
      selectedDrugStore: {
        id: 0,
        drugStoreId: 0,
        drugStoreName: "",
        address: "",
        phone: "",
        workTime: "07:00-24:00",
        district: "/mirabad",
        shortName: "",
        status: true
      },
      drugStores: [],
      showModal: false,
      messageFromServer: "",
      isLoading: true,
      currentPage: 1,
      dataRowsPerPage: 15,
      drugStoreTotalCount: 0,
      showAlert: false,
      msgModalWindow: {
        isShow: false,
        headerText: "Внимание!",
        message2Show: "",
        timeOut2Show: 2000
      },
      districts: [],
      selectedDistrict: "Выберите город/район"
    };
  },

  methods: {
    validateFields: function(drugStore){
      let validationResultMsg = "OK";
      try {
     
        if (!drugStore)
           validationResultMsg = "Что то пошло не так! Не удалось определить объект аптеки!"
        
        if (!drugStore.drugStoreId || drugStore.drugStoreId === 0)
           validationResultMsg = "Укажите id аптеки из \"Аналитики\", оно нужно для синхронизации остатков товара!";

        if (!drugStore.drugStoreName || !drugStore.drugStoreName.match(/^Аптека №\d+|^Аптека№\d+|Аптека №\s\d+|^Аптека №\d+\W\d+/gm))
           validationResultMsg = "Укажите правильное название аптеки оно должно начинаться на слово \"Аптека\" и содержать номер аптеки! К примеру \"Аптека №1\""; 

        if (!drugStore.address || !drugStore.address.trim())
          validationResultMsg = "Укажите адрес аптеки!";

        if (!drugStore.workTime || !drugStore.workTime.trim())
          validationResultMsg = "Укажите режим работы аптеки!";
        
        if(!drugStore.phone || !drugStore.phone.trim())
           validationResultMsg = "Укажите телефон аптеки!";

        if (!drugStore.orientir || !drugStore.orientir.trim())
          validationResultMsg = "Укажите ориентир аптеки!";
        
        if (!drugStore.district || !drugStore.district.trim() || this.districts.indexOf(drugStore.district) < 0)
          validationResultMsg = "Выберите район из списка!";
        
      } catch (error) {
          validationResultMsg = "Не удалось проверить соответствие заполненных Вами данных!";
      }
      return validationResultMsg;
    },
    getDistricts: function() {
      let thisComp = this;
      try {
        Vue.axios
          .get(getDistricts)
          .then(function(res) {
            if (res.status === 200) {
              thisComp.districts = res.data.districts;
            } else {
              thisComp.showMsgModalWindow(
                true,
                ru.attention,
                res.toString(),
                null
              );
              thisComp.isLoading = false;
            }
          })
          .catch(function(error) {
            thisComp.showMsgModalWindow(true, ru.error, error.toString(), null);
            thisComp.isLoading = false;
          });
      } catch (error) {
        thisComp.showMsgModalWindow(true, ru.error, error.toString(), null);
        thisComp.isLoading = false;
      }
    },
    getDrugStores: function(beginPage, endPage) {
      let thisComp = this;
      Vue.axios
        .get(getDrugStoresUrl + beginPage + "&" + "endPage=" + endPage, {
          onDownloadProgress: function(loadingEvent) {
            if (loadingEvent.loaded !== loadingEvent.total) {
              thisComp.isLoading = true;
            } else {
              thisComp.isLoading = false;
            }
          }
        })
        .then(function(res) {
          if (res && res.status === 200) {
            thisComp.drugStores = res.data.drugStores;
            thisComp.drugStoreTotalCount = res.data.totalCount;
            thisComp.isLoading = false;
          } else {
            thisComp.showMsgModalWindow(true, ru.attention, res, null);
            thisComp.isLoading = false;
          }
        })
        .catch(function(error) {
          thisComp.showMsgModalWindow(true, ru.error, error, null);
          thisComp.isLoading = false;
        });
    },
    saveDrugStore: function(drugStore, event) {
      let thisComp = this;
      try {
      
      if (this.validateFields(drugStore) === validationOK) {
          let ds = drugStore;
          drugStore.shortName = drugStore.drugStoreName.match(/^Аптека №\d+|^Аптека№\d+|Аптека №\s\d+|^Аптека №\d+\W\d+/gm)[0];

          Vue.axios
            .post(updInsertDrugStoreUrl, ds)
            .then(function(res) {
              if (res.status === 200) {
                thisComp.showModal = false;
                thisComp.showMsgModalWindow(true, ru.attention, ru.drugStoreSavedSuccessfully, 2000);
                thisComp.drugStores.push(ds);
                thisComp.showModal = false;
                thisComp.getDrugStores(1, 15);
              } else {
                thisComp.showMsgModalWindow(true, ru.attention, res.value, null);
              }
            })
            .catch(function(error) {
              thisComp.showMsgModalWindow(true, ru.error, ru.dataNotSavedTryAgain, null);
            });
      } else {
        let validatorMsg = thisComp.validateFields(drugStore);
        thisComp.showMsgModalWindow(true, ru.attention, validatorMsg, null);
      }     
      } catch (error) {
         thisComp.showMsgModalWindow(true, ru.attention, "Произошла ошибка при сохранении данных!", null);   
      }
    },
    createDrugStore: function() {
      this.selectedDrugStore = {
        id: 0,
        drugStoreId: 0,
        drugStoreName: "",
        address: "",
        phone: "",
        workTime: "07:00-24:00",
        district: "/mirabad",
        shortName: "",
        status: true
      };
      this.showModal = true;
    },
    showMsgModalWindow: function(isShow, headerText, msgText, timeOut) {
      this.msgModalWindow.timeOut2Show = timeOut;
      this.msgModalWindow.isShow = isShow;
      this.msgModalWindow.headerText = headerText;
      this.msgModalWindow.message2Show = msgText;
    }
  },

  mounted: function() {
    this.getDrugStores(1, 15);
    this.getDistricts();
  },

  watch: {
    currentPage: function(pageIndex) {
      let lastRow = this.currentPage * this.dataRowsPerPage;
      let firstRow = lastRow - this.dataRowsPerPage + 1;
      this.getDrugStores(firstRow, lastRow);
    }
  }
};
</script>

<style>
.pag-container {
  text-align: center;
}
.pag {
  display: inline-flex;
}
.create-drugStore-btn {
  float: right;
  margin: 0 20pt 20pt 0;
}
.scrollable-menu {
    height: auto;
    max-height: 200px;
    overflow-x: hidden;
    width: 100%;
    text-align: left;   
}
.drop-down-btn{
  width: 100%;
  background-color: white;
  border: solid #CED4DA 1px;
}
</style>

