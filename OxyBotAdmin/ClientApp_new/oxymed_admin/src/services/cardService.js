import Vue from "vue";
const responseFromServer = {
    data: [],
    message: "",
    isSuccessfully: false
}

export default {

    async getCards(beginPage, endPage, url = "/api/discount?beginPage=") {
        try {

            if (!beginPage || !endPage)
                throw new Error("Couldn't load users because begin or end page not specified!");

            if (!url)
                throw new Error("Couldn't get users data because url parameter not specified!");

            url = url + beginPage + "&" + "endPage=" + endPage;

            let response = await Vue.axios.get(url);

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

    async updateCardData(cardData, url="/api/discount") {
        try {
             
            if (!cardData)
                throw new Error("Couldn't save card data! Discount card data is null or undefined!");

            if (!url)
                throw new Error("Couldn't save discount card data because url parameter not specified!");

            const response = Vue.axios.post(url, cardData);
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