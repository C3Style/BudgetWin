var budgetApp = angular.module('BudgetApp', ["xeditable", "ui.bootstrap"]);

budgetApp.run(function (editableOptions) {
    // editableOptions.theme = 'bs3';
});

budgetApp.controller('TransactionCtrl', function ($scope, $filter, $http) {

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
                    console.log(percent);
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

    $scope.saveUser = function (data, id) {
        //$scope.user not updated yet
        angular.extend(data, { id: id });
        console.log(data);
        return true;
        // return $http.post('/saveUser', data);
    };

    // remove user
    $scope.removeUser = function (index) {
        $scope.users.splice(index, 1);
    };

    $scope.user = {
        dob: new Date(1984, 4, 15)
    };

    $scope.opened = {};

    $scope.open = function ($event, elementOpened) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.opened[elementOpened] = !$scope.opened[elementOpened];
    };


    // add user
    $scope.addUser = function () {
        $scope.inserted = {
            identi: null,
            name: '',
            status: null,
            group: null
        };
        $scope.users.push($scope.inserted);
    };
});