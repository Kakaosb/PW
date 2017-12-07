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