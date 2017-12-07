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