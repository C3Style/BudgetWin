angular.module('BudgetApp', [])
    .controller('TransactionCtrl', function ($scope, $http) {

        $scope.getOperationBloc = function () {
            $scope.operationBlocArray = [];

            // Simple GET request example:
            $http({
                method: 'GET',
                url: '/api/Transactions/GetOperationBloc'
            }).then(function successCallback(response) {
                $scope.operationBlocArray = response.data;
                console.log($scope.operationBlocArray);
            }, function errorCallback(response) {
                console.log(response);
            });
            
        };
    });