var baseUrl = 'http://localhost/debugPWApi';

var tokenKey = "tokenInfoPW";
var loggedIn = sessionStorage.getItem('tokenInfoPW') != null ? true : false;
var balance = 0;
var userName = "";

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
        Balance: balance,
        UserName: userName
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
            var info = getInfo();
          
            userName = info.Name;

            $rootScope.prop = {
                loggedIn: true,
                Balance: info.Balance,
                UserName: info.Name
        };
            $window.location.href = '#!/Operations';
        }
    }
});

pWApp.controller('operationsController', function ($scope, $rootScope) {
   $scope.passPWFunc = function () {
        const balance = sentPW();

        if (balance != null) {
            $rootScope.prop = {
                loggedIn: loggedIn,
                Balance: balance,
                UserName: userName
            };
            $('#recipient_id').val("");
            $('#recipient').val("");
            $('#sum').val("");
        }
    }
});

function getInfo() {
    var result;
    $.ajax({
        type: 'GET',
        url: baseUrl + '/api/Account/GetInfo',
        beforeSend: function (xhr) {
            const token = sessionStorage.getItem(tokenKey);
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        async: false
    }).success(function (data) {

        result = JSON.parse(data);
        console.log(result);

    }).fail(function (data) {
        alert(JSON.parse(data.responseText).Message);
    });

    return result;
}