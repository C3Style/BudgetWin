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
                    var operationTotal = getOperationTotal($scope.transactionBlocArray, $scope.transactionBlocArray[i].OperationId);
                    $scope.transactionBlocArray[i].OperationTotal = operationTotal
                    $scope.transactionBlocArray[i].Solde = $scope.transactionBlocArray[i].TransactionAmount - operationTotal;
                }
            }

        }, function errorCallback(response) {
            console.log(response);
        });            
    };

    function getOperationTotal(array, operationId) {
        var total = 0;
        for (var i = 0; i < array.length; i++) {
            if (array[i].TransactionType == 1 && array[i].OperationId == operationId) {
                total += array[i].TransactionAmount;
            }
        }
        return total;
    }
});