var myApp = angular.module('BudgetApp', []);

myApp.controller('TransactionCtrl', function ($scope, $http) {

    $scope.getTransactionBloc = function () {
        $scope.transactionBlocArray = [];
        $scope.debitByOperationArray = [];

        // Simple GET request example:
        $http({
            method: 'GET',
            url: '/api/Transactions/GetTransactionBloc/2010-11-01'
        }).then(function successCallback(response) {
            $scope.transactionBlocArray = response.data;

            for (var i = 0; i < $scope.transactionBlocArray.length; i++) {
                if ($scope.transactionBlocArray[i].TransactionType == 3)
                {
                    var toto = getOperationTotal($scope.transactionBlocArray, $scope.transactionBlocArray[i].OperationId);
                    $scope.transactionBlocArray[i].OperationTotal = toto

                }
            }

            console.log($scope.transactionBlocArray);
        }, function errorCallback(response) {
            console.log(response);
        });

        // Simple GET request example:
        $http({
            method: 'GET',
            url: '/api/Transactions/GetDebitByOperation/2010-11-01'
        }).then(function successCallback(response) {
            $scope.debitByOperationArray = response.data;
            // console.log($scope.debitByOperationArray);
        }, function errorCallback(response) {
            console.log(response);
        });
            
    };

    function getOperationTotal(array, operationId) {
        return 400;
    }
});