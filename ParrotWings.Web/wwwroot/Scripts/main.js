const baseUrl = 'http://localhost/debugPWApi';

const tokenKey = "tokenInfoPW";
const userNameKey = "userName";
var loggedInG = sessionStorage.getItem(tokenKey) != null ? true : false;
var userNameG = sessionStorage.getItem(userNameKey) != null ? sessionStorage.getItem(userNameKey) : "";

function getName(redirect) {
    $.ajax({
        type: 'GET',
        url: baseUrl + '/api/Users/GetName',
        beforeSend: function (xhr) {
            const token = sessionStorage.getItem(tokenKey);
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        }
    }).success(function (data) {

        userNameG = data;
        sessionStorage.setItem(userNameKey, userNameG);
        redirect();
    }).fail(function (data) {
        alert(JSON.parse(data.responseText).Message);
    });
}

function getBalance(setBalance) {
    $.ajax({
        type: 'GET',
        url: baseUrl + '/api/Users/GetBalance',
        beforeSend: function (xhr) {
            const token = sessionStorage.getItem(tokenKey);
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        async: false
    }).success(function (data) {
       
       setBalance(data);

    }).fail(function (data) {
        alert(JSON.parse(data.responseText).Message);
    });
}