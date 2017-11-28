var budgetApp = angular.module('BudgetApp', ["xeditable", "ui.bootstrap"]);

budgetApp.run(function (editableOptions) {
    editableOptions.theme = 'bs3';
});

budgetApp.controller('TransactionCtrl', function ($scope, $filter, $http, $locale) {

    $locale.NUMBER_FORMATS.GROUP_SEP = '\'';

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

                var date = new Date($scope.transactionBlocArray[i].TransactionDate);
                $scope.transactionBlocArray[i].DateFormatted = date;

                if ($scope.transactionBlocArray[i].TransactionType == 3)
                {
                    var operationTotal = getOperationTotal($scope.transactionBlocArray, $scope.transactionBlocArray[i].OperationId);
                    $scope.transactionBlocArray[i].OperationTotal = operationTotal
                    $scope.transactionBlocArray[i].Solde = $scope.transactionBlocArray[i].TransactionAmount - operationTotal;
                    var percent = ($scope.transactionBlocArray[i].TransactionAmount / operationTotal * 100) - 100;
                    $scope.transactionBlocArray[i].Percent = Math.round(percent);
                    $scope.transactionBlocArray[i].IsNegatif = percent < 0;
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

    $scope.saveUser = function (data, transactionId, operationId) {
        angular.extend(data, { TransactionId: transactionId, OperationId: operationId });
        var url = '../api/Transactions/';
        var result = '';
        if (transactionId != null)
            result = $http.put(url + transactionId, data);
        else
            result = $http.post(url, data);

        console.log(result);
        return result;
    };

    // remove user
    $scope.removeUser = function (index) {
        console.log(index);
        $scope.transactionBlocArray.splice(index, 1);
    };

    $scope.opened = {};

    $scope.open = function ($event, elementOpened) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.opened[elementOpened] = !$scope.opened[elementOpened];
    };

    $scope.checkNotEmpty = function (data, id, isNumeric) {
        if (data === undefined || data === null || data === '')
            return "<i class='fa fa-exclamation-triangle'></i> Champ requis !";

        if (isNumeric && (!$.isNumeric(data) || data <= 0))
            return "<i class='fa fa-exclamation-triangle'></i> Erreur !";
    };

    // add user
    $scope.addTransaction = function () {
        $scope.inserted = {
            DateFormatted: null,
            OperationIcon: '',
            OperationId: 93,
            OperationName: '',
            TransactionAmount: 0,
            TransactionDate: null,
            TransactionId: null,
            TransactionRemark: '',
            TransactionType: 1
        };
        $scope.transactionBlocArray.push($scope.inserted);
    };
});