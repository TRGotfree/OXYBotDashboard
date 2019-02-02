let jsonHeader = function (token) {
    return {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json;charset=UTF-8'
    };
}

let formDataHeader = function (token) {
    return {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'multipart/form-data'
    };
}

let authorizationHeader = function (token){
    return{
        'Authorization': 'Bearer ' + token
    }
}

module.exports = {
    jsonHeader,
    formDataHeader,
    authorizationHeader
};