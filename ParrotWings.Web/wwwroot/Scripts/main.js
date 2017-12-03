const baseUrl = 'http://localhost/debugPWApi';

const tokenKey = "tokenInfoPW";
const userNameKey = "userName";
var loggedInG = sessionStorage.getItem(tokenKey) != null ? true : false;
var userNameG = sessionStorage.getItem(userNameKey) != null ? sessionStorage.getItem(userNameKey) : "";

var pWApp = angular.module('pWApp', ['ngRoute']);

pWApp.config(function ($routeProvider) {
    $routeProvider

        .when('/', {
            templateUrl: 'Pages/Register.html',
            controller: 'registerController'
        })

        .when('/Register', {
            templateUrl: 'Pages/Register.html',
            controller: 'registerController'
        })

        .when('/Login', {
            templateUrl: 'Pages/Login.html',
            controller: 'loginController'
        })

        .when('/Operations', {
            templateUrl: 'Pages/Operations.html',
            controller: 'operationsController'
        });
});

pWApp.controller('mainController', function ($scope, $rootScope, $window) {
    $rootScope.prop = {
        LoggedIn: loggedInG,
        Balance: "",
        UserName: userNameG
    };

    $scope.logout = function () {
        sessionStorage.removeItem(tokenKey);
        sessionStorage.removeItem(userNameKey);
        $rootScope.prop.LoggedIn = false;
        userNameG = "";
        $window.location.href = '#!/Login';
    }
});

pWApp.controller('registerController', function ($scope, $rootScope, $window) {
    if (sessionStorage.getItem(tokenKey) != null)
        $window.location.href = '#!/Operations';

    $scope.register = function () {
        register(function() { $window.location.href = '#!/Login'; });
    }
});

pWApp.controller('loginController', function ($scope, $rootScope, $window) {
    if (sessionStorage.getItem(tokenKey) != null)
        $window.location.href = '#!/Operations';

    $scope.loggedInFunc = function () {

        loggedInFunc(setPropAndRedirectToOpertion);

        function setPropAndRedirectToOpertion() {
            getName(function () {
                loggedInG = true;
                $window.location.href = '#!/Operations';
            }
            );
        }
    }
});

pWApp.controller('operationsController', function ($scope, $rootScope) {
    $rootScope.prop.UserName = userNameG;
    $rootScope.prop.LoggedIn = loggedInG;
  
    getBalance(function (balance) { $rootScope.prop.Balance = balance });
 
    $scope.passPWFunc = function () {
        sentPW(function(balance) {
            $rootScope.prop.Balance = balance;
        });

        $('#recipient_id').val("");
        $('#recipient').val("");
        $('#sum').val("");
    }
});

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