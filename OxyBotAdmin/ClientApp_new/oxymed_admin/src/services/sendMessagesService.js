import Vue from "vue";
let messageFromServer = {
    message: "",
    isSucessfully: false
};

const sendMessageToAllUsers = function (message, url = "/api/send/msg") {
    try {
        
        if (!message)
            throw new Error("Для отправки сообщения необходимо указать его текст!");
        if (!url) {
            throw new Error("Url required for sending message");
        }

        Vue.axios.post(url, message)
            .then(res => {
                messageFromServer.message = "Сообщение отправлено!";
                messageFromServer.isSucessfully = true;
            }).catch(error => {
                messageFromServer.message = error.response.data;
                messageFromServer.isSucessfully = false;
            });
        return messageFromServer;

    } catch (error) {
        throw error;
    }
}

module.exports = { sendMessageToAllUsers };