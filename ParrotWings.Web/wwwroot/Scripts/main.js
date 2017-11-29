var baseUrl = 'http://localhost/pwAPI/';
var tokenKey = "tokenInfoPW";
var loggedIn = sessionStorage.getItem('tokenInfoPW') != null ? true : false;

var pWApp = angular.module('pWApp', ['ngRoute']); //, "kendo.directives"

console.log(loggedIn);

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

    $scope.message = 'Everyone come and see how good I look!';

    $rootScope.prop = { loggedIn: loggedIn };

    $scope.logout = function() {
        sessionStorage.removeItem(tokenKey);

        $rootScope.prop = { loggedIn: false};
    }
});

pWApp.controller('registerController', function ($scope) {
    $scope.message = 'Register';

    $scope.register = register;
});

pWApp.controller('loginController', function ($scope, $rootScope) {
    $scope.message = 'Login';

    $scope.loggedInFunc = loggedInFunc;

    $scope.loggedInFunc = function () {
        $rootScope.prop = { loggedIn: loggedInFunc() };
    }

    var tt = sessionStorage.getItem('tokenInfoPW');

    console.log(tt);
});

pWApp.controller('operationsController', function ($scope) {
    $scope.message = 'Operations';

    

});
