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
                  <label for="drugStore-name" class="col-3 col-form-label text-right">Название</label>
                  <div class="col-9">
                    <input type="text" class="form-control" v-model="selectedDrugStore.drugStoreName" id="drugStore-name" placeholder="Название аптеки">
                  </div>  
                </div>   
              <div class="form-group row"> 
                <label for="drugStore-shortname" class="col-3 col-form-label text-right">Краткое название</label>
                <div class="col-9">
                  <input type="text" class="form-control" v-model="selectedDrugStore.shortName" id="drugStore-shortname" placeholder="Краткое название">
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
                      {{selectedDistrict}}    
                    </button>
                    <div class="dropdown-menu scrollable-menu" aria-labelledby="dropdownMenuButton">
                        <a v-for="distr in districts" :key="distr" class="dropdown-item" href="#" v-on:click="selectedDistrict=distr">{{distr}}</a>
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
import axios from "axios";
import ErrorAlert from "../Alerts/errorAlert.vue";
import SuccessAlert from "../Alerts/successAlert.vue";
import { ru } from "../../lang/ru-RU.js";
import HeaderNavbar from "../HeaderNavbar/header.vue";
import { authorizationHeader } from "../../helper.js";
import { jsonHeader } from "../../helper.js";
import BVPagination from "bootstrap-vue/es/components/pagination/pagination";
import ModalWindow from "../ModalWindow/ModalWindow.vue";
import Loading from "../Loading/loading.vue";
import MessageModalWindow from "../MessageModalWindow/messageModalWindow.vue";

let validateDrugStoreFields = function(drugStore) {
  let validationResult = {
    isValid: false,
    msg: ru.someFieldsAreEmptyOrNotValid
  };
  try {
    if (drugStore) {
      if (
        drugStore.id >= 0 &&
        drugStore.drugStoreName.length > 0 &&
        drugStore.address.length > 0 &&
        drugStore.phone.length > 0 &&
        drugStore.workTime.length > 0 &&
        drugStore.orientir.length > 0 &&
        drugStore.district.length > 0 &&
        drugStore.shortName.length > 0
      ) {
        if (drugStore.district.match(/^\/[a-z_]+$/) === false) {
          validationResul.isValid = false;
          validationResult.msg = ru.districtFormatIsNotValid;
          return validationResult;
        }

        if (drugStore.shortName.match(/^\D+\s№\d+$/) === false) {
          validationResul.isValid = false;
          validationResult.msg = ru.drugStoreShortNameIsNotValid;
          return validationResult;
        }

        validationResult.isValid = true;
        validationResult.msg = "";
      }
    }
  } catch (error) {
    validationResult.isValid = false;
    validationResult.msg = ru.dataNotSaved;
  }
  return validationResult;
};

export default {
  components: {
    HeaderNavbar,
    ErrorAlert,
    SuccessAlert,
    BVPagination,
    ModalWindow,
    Loading,
    MessageModalWindow
  },
  
  data: function() {
    return {
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
    getDistricts: function() {
      let thisComp = this;
      try {
        axios
          .get(getDistricts, {
            headers: authorizationHeader(sessionStorage.getItem("userToken"))
          })
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
      axios
        .get(getDrugStoresUrl + beginPage + "&" + "endPage=" + endPage, {
          headers: authorizationHeader(sessionStorage.getItem("userToken")),
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

      console.log("DrugStoreId is: " + drugStore.drugStoreId);
      console.log("DrugStoreStats is: " + drugStore.status);

      if (drugStore && validateDrugStoreFields(drugStore).isValid === true) {
        if (drugStore.drugStoreId == 0) {
          let ds = drugStore;

          console.log(drugStore.drugStoreId);

          axios
            .post(updInsertDrugStoreUrl, ds, {
              headers: authorizationHeader(sessionStorage.getItem("userToken"))
            })
            .then(function(res) {
              if (res.status === 200) {
                thisComp.showModal = false;
                thisComp.showMsgModalWindow(
                  true,
                  ru.attention,
                  ru.drugStoreSavedSuccessfully,
                  2000
                );
                thisComp.drugStores.push(ds);
                thisComp.showModal = false;
              } else {
                thisComp.showMsgModalWindow(
                  true,
                  ru.attention,
                  res.value,
                  null
                );
              }
            })
            .catch(function(error) {
              thisComp.showMsgModalWindow(
                true,
                ru.error,
                ru.dataNotSavedTryAgain,
                null
              );
            });
        } else if (drugStore.id > 0) {
          let ds = drugStore;
          console.log("Updating drug store");
          axios
            .put(updInsertDrugStoreUrl, ds, {
              headers: authorizationHeader(sessionStorage.getItem("userToken"))
            })
            .then(function(res) {
              if (res.status === 200) {
                thisComp.showModal = false;
                thisComp.showMsgModalWindow(
                  true,
                  ru.attention,
                  ru.drugStoreSavedSuccessfully,
                  2000
                );
                thisComp.showModal = false;
              } else {
                thisComp.showMsgModalWindow(true, ru.attention, res, null);
              }
            })
            .catch(function(error) {
              thisComp.showMsgModalWindow(
                true,
                ru.error,
                error.toString(),
                null
              );
            });
        }
      } else {
        let validatorMsg = validateDrugStoreFields(drugStore).msg;
        console.log(validatorMsg);
        thisComp.showMsgModalWindow(true, ru.attention, validatorMsg, null);
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

