/* eslint-disable no-console */
import Vue from "vue";
const responseFromServer = {
    data: [],
    message: "",
    isSuccessfully: false
}

export default {

    async getDrugStores(beginPage, endPage, url = "/api/drugstore?beginPage=") {
        try {

            if (!beginPage || !endPage)
                throw new Error("Couldn't load drugStores because begin or end page not specified!");

            if (!url)
                throw new Error("Could't get drugStores data because url parameter not specified!");

            url = url + beginPage + "&endPage=" + endPage;
            const response = await Vue.axios.get(url);

            if (response && response.data) {
                responseFromServer.data = response.data ? response.data : [];
                responseFromServer.message = response.data.message ? response.data.message : "200. Данные успешно получены!"
                responseFromServer.isSuccessfully = true;

            } else {
                responseFromServer.data = [];
                responseFromServer.message = response.data.message ? response.data.message : "Данные не получены!"
                responseFromServer.isSuccessfully = false;
            }

        } catch (error) {
            responseFromServer.data = [];
            if (process.env.VUE_APP_IS_DEV)
                responseFromServer.message = JSON.stringify(error);
            else
                responseFromServer.message = error.response.data ? error.response.data : "Данные не получены!"
            responseFromServer.isSuccessfully = false;
        }

        return responseFromServer;
    },

    async saveDrugStore(drugstore, url = "/api/drugstore") {
        try {

            if (!drugstore)
                throw new Error("Couldn't save action! Action data is null or undefined!");

            if (!url)
                throw new Error("Could't save action data because url parameter not specified!");

            const response = await Vue.axios.post(url, drugstore);
            responseFromServer.isSuccessfully = true;
            responseFromServer.message = "Данные успешно сохранены!";

            if (response && response.data)
                responseFromServer.message = response.data.message ? response.data.message : responseFromServer.message;

        } catch (error) {

            if (process.env.VUE_APP_IS_DEV)
                responseFromServer.message = JSON.stringify(error);
            else
                responseFromServer.message = error.response.data ? error.response.data : "Данные не сохранены!"

            responseFromServer.isSuccessfully = false;
        }

        return responseFromServer;
    },

    async getDistricts(url = "/api/district") {
        try {
            if (!url)
                throw new Error("Could't get users data because url parameter not specified!");
 
            const response = await Vue.axios.get(url);

            if (response && response.data) {
                responseFromServer.data = response.data ? response.data : [];
                responseFromServer.message = response.data.message ? response.data.message : "200. Данные успешно получены!"
                responseFromServer.isSuccessfully = true;

            } else {
                responseFromServer.data = [];
                responseFromServer.message = response.data.message ? response.data.message : "Данные не получены!"
                responseFromServer.isSuccessfully = false;
            }
        } catch (error) {
            responseFromServer.data = [];
            if (process.env.VUE_APP_IS_DEV)
                responseFromServer.message = JSON.stringify(error);
            else
                responseFromServer.message = error.response.data ? error.response.data : "Данные не получены!"
            responseFromServer.isSuccessfully = false;
        }

        return responseFromServer;
    }

}