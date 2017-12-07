pWApp.controller('operationsController', function ($scope, $rootScope) {
    $rootScope.prop.UserName = userNameG;
    $rootScope.prop.LoggedIn = loggedInG;

    getBalance(function (balance) { $rootScope.prop.Balance = balance });

    $scope.passPWFunc = function () {
        sentPW(function (balance) {
            $rootScope.prop.Balance = balance;
        });

        $('#recipient_id').val("");
        $('#recipient').val("");
        $('#sum').val("");
    }
});