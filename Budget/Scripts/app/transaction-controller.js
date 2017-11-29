var budgetApp = angular.module('BudgetApp', ["xeditable", "ui.bootstrap"]);
var url = '/api/Transactions'

budgetApp.run(function (editableOptions) {
    editableOptions.theme = 'bs3';
});

budgetApp.controller('TransactionCtrl', function ($scope, $filter, $http, $locale) {

    $locale.NUMBER_FORMATS.GROUP_SEP = '\'';

    $scope.getTransactionBloc = function () {
        $scope.transactionBlocArray = [];
        $scope.debitByOperationArray = [];

        $http({
            method: 'GET',
            url: url + '/GetTransactionBloc/2010-11-01'
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

        var transaction = {
            Id: transactionId,
            OperationId: operationId,
            Amount: data.TransactionAmount,
            Remark: data.TransactionRemark,
            Date: data.TransactionDate,
            TypeId: 1
        };

        var result = '';
        if (transactionId != null)
            return $.ajax({
                type: "PUT",
                url: url + "/" + transactionId,
                data: transaction,
                dataType: "json",
                success: function () {
                    toastr.success("La transaction a correctement été modifiée.");
                },
                error: function (xhr, status, err) {
                    toastr.error("Erreur, la transaction n'a pas été modifiée. Cause : " + err);
                }
            });
        else {
            transaction.Id = 0;
            return $.ajax({
                type: "POST",
                url: url,
                data: transaction,
                dataType: "json",
                success: function () {
                    toastr.success("La transaction a correctement été ajoutée.");
                },
                error: function (xhr, status, err) {
                    toastr.error("Erreur, la transaction n'a pas été ajoutée. Cause : " + err);
                }
            });
        }
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
    $scope.addTransaction = function (operationId) {
        $scope.inserted = {
            DateFormatted: null,
            OperationIcon: '',
            OperationId: operationId,
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