budgetApp.controller('LayoutCtrl', function ($rootScope, $scope, $filter, $http, CurrentDate) {

    $scope.currentDate = CurrentDate;

    $scope.change = function () {
        var args = [];
        $rootScope.$emit('ChangeDate', args);
    };
});