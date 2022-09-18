
(function() {
'use strict';

angular.module('LunchCheck', [])
.controller('LunchCheckController', LunchCheckController);

LunchCheckController.$inject = ['$scope'];
function LunchCheckController ($scope) {
    $scope.values = "";
    $scope.message = "";
    $scope.color = "";

    $scope.sayMessage = function (){
        return "Random message for test!";
    };

    $scope.checkValues = function () {
        var vals = $scope.values;
        var re = /\s*,\s*/;
        if (vals == '') {
            $scope.message = "Please enter data first!";
            $scope.color = "red";
        } else {
            var splitValues = vals.split(re);
            if (vals.search(",") < 0 && splitValues[0] == "") {
                $scope.message = "Please enter a valid data first!";
                $scope.color = "red";
            } else {
                var totalValues = 0;
                if (splitValues.length != 1){
                    for (var i = 0; i < splitValues.length; i++){
                        if (splitValues[i] != ""){
                            totalValues += 1;
                        }
                    }
                }else{
                    totalValues = 1;
                }

                $scope.color = "green";
                if (totalValues > 3){
                    $scope.message = totalValues + " Item(s) = Too much!";
                } else {
                    $scope.message = totalValues + " Item(s) = Enjoy!"; 
                }
            }
        }
    };
}

})();