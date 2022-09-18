
(function() {
'use strict';

var shoppingList1 = [
    "Milk", "Chocolate", "Sugar", "Rice", "Beans", "Apple", "Banana", "Cheese", "Bread"
];

var shoppingList2 = [
    {
        name: "Milk",
        quantity: "1"
    },
    {
        name: "Chocolate",
        quantity: "2"
    },
    {
        name: "Sugar",
        quantity: "3"
    },
    {
        name: "Rice",
        quantity: "4"
    } 
];

angular.module('ShoppingListApp', [])
.controller('ShoppingListController', ShoppingListController);

ShoppingListController.$inject = ['$scope'];
function ShoppingListController ($scope) {
    $scope.shoppingList1 = shoppingList1;
    $scope.shoppingList2 = shoppingList2;

    $scope.addToList = function (){
        var newItem = {
            name: $scope.newItemName,
            quantity: $scope.newItemQuantity
        };
        $scope.shoppingList2.push(newItem);
    }
    
}

})();