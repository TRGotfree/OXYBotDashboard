import Vue from "vue";
const responseFromServer = {
    data: [],
    message: "",
    isSuccessfully: false
}

export default {

    async getUsers(beginPage, endPage, url = "/api/user/all?beginPage=") {
        try {

            if (!beginPage || !endPage)
                throw new Error("Couldn't load users because begin or end page not specified!");

            if (!url)
                throw new Error("Could't get users data because url parameter not specified!");

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

    async sendMessageToUser(message, chatId, url="/api/user/") {
        try {

            if (typeof message !== "string")
                throw new Error("Невозможно отправить пустое сообщение!");

            if (typeof chatId !== "number")
                throw new Error("Couldn't send message, chatId is required!");

           const response = await Vue.axios.put(url + chatId, JSON.stringify(message)); 
           
           responseFromServer.isSuccessfully = true;
           responseFromServer.message = "Сообщение успешно отправлено!"; 

           if (response && response.data)
              responseFromServer.message = response.data.message ? response.data.message : "Сообщение успешно отправлено!";           

        } catch (error) {
            if (process.env.VUE_APP_IS_DEV)
                responseFromServer.message = JSON.stringify(error);
            else
                responseFromServer.message = error.response.data ? error.response.data : "Сообщение не отправлено!"
            responseFromServer.isSuccessfully = false;
        }
        return responseFromServer;
    }
}