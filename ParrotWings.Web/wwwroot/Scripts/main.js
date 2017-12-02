var baseUrl = 'http://localhost/debugPWApi';

var tokenKey = "tokenInfoPW";
var loggedIn = sessionStorage.getItem('tokenInfoPW') != null ? true : false;
var balance = 0;

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
        loggedIn: loggedIn,
        Balance: balance
    };

    $scope.logout = function () {
        sessionStorage.removeItem(tokenKey);
        $rootScope.prop = { loggedIn: false };
        $window.location.href = '#!/Login';
    }
});

pWApp.controller('registerController', function ($scope, $rootScope, $window) {
    $scope.register = function () {
        if (register()) {
            $window.location.href = '#!/Login';
        }
    }
});

pWApp.controller('loginController', function ($scope, $rootScope, $window) {
    $scope.loggedInFunc = function () {

        if (loggedInFunc()) {
            $rootScope.prop = {
                loggedIn: true,
                Balance: getBalance()
        };
            $window.location.href = '#!/Operations';
        }
    }
});

pWApp.controller('operationsController', function ($scope, $rootScope) {
    $rootScope.prop = {
        loggedIn: true,
        Balance: getBalance()
    };

    $scope.passPWFunc = function () {
        const balance = sentPW();

        if (balance != null) {
            $rootScope.prop = {
                loggedIn: loggedIn,
                Balance: balance
            };
            $('#recipient_id').val("");
            $('#recipient').val("");
            $('#sum').val("");
        }
    }
});

function getBalance() {
    var result;
    $.ajax({
        type: 'GET',
        url: baseUrl + '/api/Account/GetBalance',
        beforeSend: function (xhr) {
            const token = sessionStorage.getItem(tokenKey);
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        async: false
    }).success(function (data) {

        result = data;

    }).fail(function (data) {
        alert(JSON.parse(data.responseText).Message);
    });

    return result;
}