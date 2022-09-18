(function() {
    'use strict';
    
    var shoppingList1 = [
        {
            name: "Water",
            quantity: "1"
        },
        {
            name: "Cheese",
            quantity: "2"
        },
        {
            name: "Toothpast",
            quantity: "3"
        },
        {
            name: "Chicken",
            quantity: "4"
        },
        {
            name: "Soap",
            quantity: "5"
        }  
    ];

    angular.module('ShoppingListCheckOff', [])
    .controller('ToBuyController', ToBuyController)
    .controller('AlreadyBoughtController', AlreadyBoughtController)
    .service('ShoppingListCheckOffService', ShoppingListCheckOffService);
    
    ToBuyController.$inject = ['ShoppingListCheckOffService', '$scope'];
    function ToBuyController(ShoppingListCheckOffService, $scope) {
      var list = this;
    
      list.items = ShoppingListCheckOffService.getItems();
    
      list.removeItem = function (itemIndex) {
        ShoppingListCheckOffService.removeItem(itemIndex);
        if(!list.items[0]){
            $scope.messageToBuy = "true";
        } else {
            $scope.messageBought = "";
        }
      };
    }
    
    AlreadyBoughtController.$inject = ['ShoppingListCheckOffService'];
    function AlreadyBoughtController(ShoppingListCheckOffService) {
      var listBought = this;
      listBought.messageBought = "Nothing bought yet!";
      listBought.items = ShoppingListCheckOffService.getItemsBought();
    }
    
    // If not specified, maxItems assumed unlimited
    function ShoppingListCheckOffService() {
      var service = this;
    
      // List of shopping items
      var items = shoppingList1;
      var itemsBought = [];
    
      service.removeItem = function (itemIndex) {
        itemsBought.push(items[itemIndex]);
        items.splice(itemIndex, 1);
      };
    
      service.getItems = function () {
        return items;
      };

      service.getItemsBought = function () {
        return itemsBought;
      };      
    }
    
})();