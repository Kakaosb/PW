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