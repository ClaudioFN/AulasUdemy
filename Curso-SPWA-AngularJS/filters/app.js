
(function() {
'use strict';

angular.module('MsgApp', [])
.controller('MsgController', MsgController)
.filter('fun', FunFilter)
.filter('truth', TruthFilter);

MsgController.$inject = ['$scope', 'funFilter'];
function MsgController ($scope, funFilter) {
    $scope.name = "Claudio";
    $scope.option = 2;

    $scope.sayMessage = function (){
        return "Random message for test!";
    };

    $scope.sayFunMessage = function (){
        var msg = "Random message for test!";
        msg = funFilter(msg);
        return msg;
    };    

    $scope.testButton = function () {
        $scope.option = 1;
    };
}

function FunFilter(){
    return function (input){
        input = input || "";
        input = input.replace("test", "FUN");
        return input;
    }
}

function TruthFilter(){
    return function (input, target, replace) {
        input = input || "";
        input = input.replace(target, replace);
        return input;
    }
}

})();