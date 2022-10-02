(function() {
    'use strict';
    
    // Controller for items in the Category to be used for the template HTML through the items.component.js
    angular.module('MenuApp')
    .controller('ItemsController', ItemsController);
  
    ItemsController.$inject = ['itemsList'];
  
    function ItemsController (itemsList) {
        var items = this;
        items.itemsList = itemsList.data['menu_items'];
    }
}());