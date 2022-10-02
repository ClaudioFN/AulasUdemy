(function () {
    'use strict';
    
    angular.module('ShoppingList')
    .controller('ItemDetailController', ItemDetailController);
    
    // 'item' is injected through state's resolve
    ItemDetailController.$inject = ['$stateParams', 'items'] // Including $stateParams to use Nested View
    function ItemDetailController($stateParams, items) {
      var itemDetail = this;
      var item = items[$stateParams.itemId]; // To Nested View
      itemDetail.name = item.name;
      itemDetail.quantity = item.quantity;
      itemDetail.description = item.description;
    }
    
    })();