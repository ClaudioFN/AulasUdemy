/*( function (){
'use strict';
angular.module('myAngularAPP', [])
       .controller('MyFirstController', function ($scope){
            $scope.name = "Claudio";
            $scope.sayHello = function () { return"Test creating a function here!"; };
        });
})();*/

(function() {
'use strict';

angular.module('NameCalculator', [])
.controller('NameCalculatorController', function ($scope){
    $scope.name = "";
    $scope.totalValue = 0;
    $scope.displayNumeric = function () {
        var totalNameValue = calculatNumericForString($scope.name);
        $scope.totalValue = totalNameValue;
    };

    function calculatNumericForString(string) {
        var totalStringValue = 0;
        for (var i = 0; i < string.length; i++){
            totalStringValue += string.charCodeAt(i);
        }
        return totalStringValue;
    }

});

})();