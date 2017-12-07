pWApp.controller('registerController', function ($scope, $rootScope, $window) {
    if (sessionStorage.getItem(tokenKey) != null)
        $window.location.href = '#!/Operations';

    $scope.register = function () {
        register(function () { $window.location.href = '#!/Login'; });
    }
});