import Vue from "vue";
let messageFromServer = {
    message: "",
    isSucessfully: false
};

export default {

async sendMessageToAllUsers (message, url = "/api/send/msg") {
    try {
        
        if (!message)
            throw new Error("Для отправки сообщения необходимо указать его текст!");
        if (!url)
            throw new Error("Url required for sending message");
        
        let response = await Vue.axios.post(url, message);
        messageFromServer.isSucessfully = true;
        messageFromServer.message = response.data;

        return messageFromServer;
    } catch (error) {
        messageFromServer.isSucessfully = false;
        if (error.response) {
            messageFromServer.message = error.response.data;           
        }else{
            messageFromServer.message = "Something goes wrong! Please try again later!";
        }     
    }
}
}
 