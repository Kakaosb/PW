//var baseUrl = 'http://localhost/pwAPI/';

var baseUrl = 'http://localhost/debugPWApi';

var tokenKey = "tokenInfoPW";
var loggedIn = sessionStorage.getItem('tokenInfoPW') != null ? true : false;

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

pWApp.controller('mainController', function ($scope, $rootScope) {
    
    $rootScope.prop = { loggedIn: loggedIn };

    $scope.logout = function () {
        sessionStorage.removeItem(tokenKey);

        $rootScope.prop = { loggedIn: false };
    }
});

pWApp.controller('registerController', function ($scope) {
   
    $scope.register = register;
});

pWApp.controller('loginController', function ($scope, $rootScope) {
   
    $scope.loggedInFunc = function () {
        $rootScope.prop = { loggedIn: loggedInFunc() };
    }
});

pWApp.controller('operationsController', function ($scope) {

});
