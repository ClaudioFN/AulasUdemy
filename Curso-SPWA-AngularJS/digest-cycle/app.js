
(function() {
'use strict';

angular.module('CounterApp', [])
.controller('CounterController', CounterController);

CounterController.$inject = ['$scope', '$timeout'];
function CounterController ($scope, $timeout) {
    $scope.onceCounter = 0;
    $scope.counter = 0;

    $scope.showNumberOfWatchers = function (){
        console.log("Number of watchers: ", $scope.$$watchersCount);
    };

    $scope.countOnce = function (){
        $scope.onceCounter = 1;
    };

    $scope.upCounter = function (){
        $timeout (function (){
                $scope.counter++;
                console.log("Counter Incremented!");
        }, 2000);
    }

    //$scope.upCounter = function (){
    //    setTimeout (function (){
    //        $scope.$apply(function () {
    //            $scope.counter++;
    //            console.log("Counter Incremented!");
    //        });
    //    }, 2000);
    //}

    //$scope.$watch('onceCounter', function (newValue, oldValue){
    //    console.log("onceCounter Old Value: ", oldValue);
    //    console.log("onceCounter New Value: ", newValue);
    //});
    //
    //$scope.$watch('counter', function (newValue, oldValue){
    //    console.log("upCounter Old Value: ", oldValue);
    //    console.log("upCounter New Value: ", newValue);
    //});
    
    
}

})();