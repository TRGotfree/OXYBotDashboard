import Vue from "vue";

let messageFromServer = {
    message: "",
    isSucessfully: false
};

export default {

    async sendImageToAllUsers(imageData, url = "/api/send/img") {
        try {

            if (!imageData)
                throw new Error("Для отправки сообщения необходимо указать его текст!");

            if (!imageData.image)
                throw new Error("Couldn't send empty image!");

            if (!url)
                throw new Error("Url is required for sending image!");

            var formData = new FormData();

            formData.append("file", imageData.image);
            formData.append("message", imageData.textForImage);

            let response = await Vue.axios
                .post(url, formData, {
                    headers: {
                        "Content-Type": "multipart/form-data"
                    }
                });

            if (response && response.data && response.data.message)
                messageFromServer.message = response.data.message
            else
                messageFromServer.message = "Изображение успешно отправлено!"

            messageFromServer.isSucessfully = true;

            return messageFromServer;

        } catch (error) {
            messageFromServer.isSucessfully = false;
            if (error.response && error.response.data) {
                messageFromServer.message = error.response.data;
            } else {
                messageFromServer.message = "Изображение не загружено!";
            }
        }
    },

    async sendFileToAllUsers(fileData, url="/api/send/file"){
        try {

            if (!fileData)
                throw new Error("Для отправки сообщения необходимо указать его текст!");

            if (!fileData.file)
                throw new Error("Couldn't send empty image!");

            if (!url)
                throw new Error("Url is required for sending image!");

            var formData = new FormData();

            formData.append("file", fileData.file);
            formData.append("message", fileData.textForFile);

            let response = await Vue.axios
                .post(url, formData, {
                    headers: {
                        "Content-Type": "multipart/form-data"
                    }
                });

            if (response && response.data && response.data.message)
                messageFromServer.message = response.data.message
            else
                messageFromServer.message = "Файл успешно отправлен!"

            messageFromServer.isSucessfully = true;

            return messageFromServer;

        } catch (error) {
            messageFromServer.isSucessfully = false;
            if (error.response && error.response.data) {
                messageFromServer.message = error.response.data;
            } else {
                messageFromServer.message = "Файл не загружен!";
            }
        }
    }
}