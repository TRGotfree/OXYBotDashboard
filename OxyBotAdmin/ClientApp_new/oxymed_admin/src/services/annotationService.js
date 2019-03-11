/* eslint-disable no-console */
import Vue from "vue";
const responseFromServer = {
    data: [],
    message: "",
    isSuccessfully: false
}

export default {

    async getAnnotations(beginPage, endPage, url = "/api/annotation?beginPage=") {
        try {

            if (!beginPage || !endPage)
                throw new Error("Couldn't load users because begin or end page not specified!");

            if (!url)
                throw new Error("Could't get users data because url parameter not specified!");

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

    async getCertainAnnotation(annotationId, url = "/api/annotation/") {
        try {
            if (!annotationId)
                throw new Error("Couldn't load certain annotation because annotationId input param is not specified!");

            if (!url)
                throw new Error("Could't get certain annotation data because url parameter is not specified!");

            url = url + annotationId;
            const response = await Vue.axios.get(url);

            if (response && response.data) {
                responseFromServer.data = response.data ? response.data : {};
                responseFromServer.message = response.data.message ? response.data.message : "200. Данные успешно получены!"
                responseFromServer.isSuccessfully = true;

            } else {
                responseFromServer.data = {};
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

    async saveNewAnnotation(annotationData, url = "/api/annotation") {
        try {

            if (!annotationData)
                throw new Error("Couldn't save annotation! Annotation data is null or undefined!");

            if (!url)
                throw new Error("Could't save annotation data because url parameter not specified!");

            const response = await Vue.axios.post(url, annotationData);
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

    async updateAnnotation(annotationData, url = "/api/annotation") {
        try {
            if (!annotationData)
                throw new Error("Couldn't save annotation! Annotation data is null or undefined!");

            if (!url)
                throw new Error("Couldn't save annotation data because url parameter not specified!");

            const response = Vue.axios.put(url, annotationData);
            responseFromServer.isSuccessfully = true;
            responseFromServer.message = "Данные успешно обновлены!";

            if (response && response.data)
                responseFromServer.message = response.data.message ? response.data.message : responseFromServer.message;

        } catch (error) {

            if (process.env.VUE_APP_IS_DEV)
                responseFromServer.message = JSON.stringify(error);
            else
                responseFromServer.message = error.response.data ? error.response.data : "Данные не обновлены!"

            responseFromServer.isSuccessfully = false;
        }

        return responseFromServer;
    }

}