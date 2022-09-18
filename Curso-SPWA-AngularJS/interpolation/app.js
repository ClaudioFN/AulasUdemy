
(function() {
'use strict';

angular.module('MsgApp', [])
.controller('MsgController', MsgController);

MsgController.$inject = ['$scope'];
function MsgController ($scope) {
    $scope.name = "Claudio";
    $scope.option = 1;

    $scope.sayMessage = function (){
        return "Random message for test!";
    };

    $scope.testButton = function () {
        $scope.option = 2;
    };
}

})();