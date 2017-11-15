var budgetApp = angular.module('BudgetApp', ["xeditable"]);

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

    $scope.users = [
        { id: 1, name: 'awesome user1', status: 2, group: 4, groupName: 'admin' },
        { id: 2, name: 'awesome user2', status: undefined, group: 3, groupName: 'vip' },
        { id: 3, name: 'awesome user3', status: 2, group: null }
    ];

    $scope.statuses = [
        { value: 1, text: 'status1' },
        { value: 2, text: 'status2' },
        { value: 3, text: 'status3' },
        { value: 4, text: 'status4' }
    ];

    $scope.groups = [];
    $scope.loadGroups = function () {
        return $scope.groups.length ? null : $http.get('/groups').success(function (data) {
            $scope.groups = data;
        });
    };

    $scope.showGroup = function (user) {
        if (user.group && $scope.groups.length) {
            var selected = $filter('filter')($scope.groups, { id: user.group });
            return selected.length ? selected[0].text : 'Not set';
        } else {
            return user.groupName || 'Not set';
        }
    };

    $scope.showStatus = function (user) {
        var selected = [];
        if (user.status) {
            selected = $filter('filter')($scope.statuses, { value: user.status });
        }
        return selected.length ? selected[0].text : 'Not set';
    };

    $scope.checkName = function (data, id) {
        if (id === 2 && data !== 'awesome') {
            return "Username 2 should be `awesome`";
        }
    };

    $scope.saveUser = function (data, id) {
        //$scope.user not updated yet
        angular.extend(data, { id: id });
        return $http.post('/saveUser', data);
    };

    // remove user
    $scope.removeUser = function (index) {
        $scope.users.splice(index, 1);
    };

    // add user
    $scope.addUser = function () {
        $scope.inserted = {
            id: $scope.users.length + 1,
            name: '',
            status: null,
            group: null
        };
        $scope.users.push($scope.inserted);
    };
});