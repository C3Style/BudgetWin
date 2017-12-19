budgetApp.run(function (editableOptions) {
    editableOptions.theme = 'bs3';
});

budgetApp.controller('TransactionCtrl', function ($rootScope, $scope, $filter, $http, $locale, CurrentDate) {

    var url = '/api/Transactions';

    $locale.NUMBER_FORMATS.GROUP_SEP = '\'';

    $scope.isEdited = false;

    $scope.getTransactionBloc = function () {
        $scope.transactionBlocArray = [];
        $scope.debitByOperationArray = [];

        var date = CurrentDate.Value;
        var month = date.getMonth() + 1;
        var day = date.getDate();
        var param = date.getFullYear() + '-' +
            (('' + month).length < 2 ? '0' : '') + month + '-' +
            (('' + day).length < 2 ? '0' : '') + day;

        $http({
            method: 'GET',
            url: url + '/GetTransactionBloc/' + param
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
                    var percent = 100 - (operationTotal / $scope.transactionBlocArray[i].TransactionAmount * 100);
                    $scope.transactionBlocArray[i].Percent = Math.round(percent);
                    $scope.transactionBlocArray[i].IsNegatif = percent < 0;
                }
            }

        }, function errorCallback(response) {
            toastr.error("Erreur lors de la récupération des transactions. Cause : " + response);
        });            
    };

    $scope.saveUser = function (data, transactionId, operationId) {

        var transaction = {
            Id: transactionId,
            OperationId: operationId,
            Amount: data.TransactionAmount,
            Remark: data.TransactionRemark,
            Date: data.DateFormatted.toJSON(),
            TypeId: 1
        };

        $scope.isEdited = false; 

        if (transactionId != null)
        {
            var updatedTransaction = $.grep($scope.transactionBlocArray, function (e) {
                return e.TransactionId == transactionId;
            });
            updatedTransaction[0].TransactionAmount = data.TransactionAmount;

            return $.ajax({
                type: "PUT",
                url: url + "/" + transactionId,
                data: transaction,
                dataType: "json",
                success: function () {
                    toastr.success("La transaction a correctement été modifiée.");
                    updateTotalAmount(operationId);
                },
                error: function (xhr, status, err) {
                    toastr.error("Erreur, la transaction n'a pas été modifiée. Cause : " + err);
                }
            });
        } else {
            transaction.Id = 0;
            return $.ajax({
                type: "POST",
                url: url,
                data: transaction,
                dataType: "json",
                success: function (result) {
                    toastr.success("La transaction a correctement été ajoutée.");

                    var newAdded = $.grep($scope.transactionBlocArray, function (e) {
                        return e.TransactionId == null;
                    });

                    newAdded[0].TransactionId = result.Id;
                    newAdded[0].TransactionAmount = result.Amount;
                    updateTotalAmount(operationId);
                },
                error: function (xhr, status, err) {
                    toastr.error("Erreur, la transaction n'a pas été ajoutée. Cause : " + err);
                }
            });
        }
    };

    $scope.edit = function () { $scope.isEdited = true; };
    $scope.cancel = function () { $scope.isEdited = false; };
    $scope.isDisabled = function () { return $scope.isEdited; };

    $scope.removeTransaction = function (index, transactionId) {

        var result = confirm("Etes-vous sûr de vouloir supprimer cette transaction ?");
        if (!result) {
            return;
        }

        var transaction = $.grep($scope.transactionBlocArray, function (e) {
            return e.TransactionId == transactionId;
        });

        var data = $.grep($scope.transactionBlocArray, function (e) {
            return e.TransactionId != transactionId;
        });

        $scope.transactionBlocArray = data;

        updateTotalAmount(transaction[0].OperationId);

        $scope.isEdited = false;

        if (transactionId != null) {
            return $.ajax({
                type: "DELETE",
                url: url + "/" + transactionId,
                dataType: "json",
                success: function (data) {
                    toastr.success("La transaction a correctement été supprimée.");
                },
                error: function (xhr, status, err) {
                    toastr.error("Erreur, la transaction n'a pas été supprimée. Cause : " + err);
                }
            });
        }
    };

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
        $scope.isEdited = true; 
    };

    $scope.checkNotEmpty = function (data, id, isNumeric) {
        if (data === undefined || data === null || data === '')
            return "<i class='fa fa-exclamation-triangle'></i> Champ requis !";

        if (isNumeric && (!$.isNumeric(data) || data <= 0))
            return "<i class='fa fa-exclamation-triangle'></i> Erreur !";
    };

    $rootScope.$on('ChangeDate', function (event, args) {
        $scope.getTransactionBloc();
    });

    function getOperationTotal(array, operationId) {
        var total = 0;
        for (var i = 0; i < array.length; i++) {
            if (array[i].TransactionType == 1 && array[i].OperationId == operationId) {
                total += parseInt(array[i].TransactionAmount);
            }
        }
        return total;
    }

    function updateTotalAmount(operationId) {
        var transactionBudget = $.grep($scope.transactionBlocArray, function (e) {
            return e.OperationId == operationId && e.TransactionType == 3;
        });
        var operationTotal = getOperationTotal($scope.transactionBlocArray, operationId);
        transactionBudget[0].OperationTotal = operationTotal
        transactionBudget[0].Solde = transactionBudget[0].TransactionAmount - operationTotal;
        var percent = 100 - (operationTotal / transactionBudget[0].TransactionAmount * 100);
        transactionBudget[0].Percent = Math.round(percent);
        transactionBudget[0].IsNegatif = percent < 0;
    }


});