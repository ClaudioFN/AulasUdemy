(function () {
  'use strict';
  
  angular.module('ShoppingListDirectiveApp', [])
  //.controller('ShoppingListController1', ShoppingListController1)
  .controller('ShoppingListController', ShoppingListController)
  //.controller('ShoppingListController2', ShoppingListController2)
  //.controller('ShoppingListDirectiveController', ShoppingListDirectiveController)
  .factory('ShoppingListFactory', ShoppingListFactory)
  //.directive('shoppingList', ShoppingList)
  .directive('shoppingList', ShoppingListDirective);
  
  // Return the template 
  //function ShoppingList() {
  //  var ddo = {
  //    templateUrl: 'shoppingList.html',
  //    scope: {
  //      list: '=myList',
  //      title: '@title'
  //    }
  //  };
  //
  //  return ddo;
  //}

  // Return the template 
  function ShoppingListDirective() {
    var ddo = {
      templateUrl: 'shoppingList.html',
      scope: {
        list: '<',
        title: '@'
      },
      controller: ShoppingListDirectiveController,
      controllerAs: 'list',
      bindToController: true
    };
  
    return ddo;
  }

  function ShoppingListDirectiveController() {
    var list = this;
  
    list.cookiesInList = function () {
      for (var i = 0; i < list.items.length; i++) {
        var name = list.items[i].name;
        if (name.toLowerCase().indexOf("cookie") !== -1) {
          return true;
        }
      }
  
      return false;
    };
  }
  
  //Controller
  ShoppingListController.$inject = ['ShoppingListFactory'];
  function ShoppingListController(ShoppingListFactory) {
    var list = this;
  
    // Use factory to create new shopping list service
    var shoppingList = ShoppingListFactory();
  
    list.items = shoppingList.getItems();
    var origTitle = "Shopping List #1";
    list.title = origTitle + " (" + list.items.length + " items)";  // Dinamically change title in the page
  
    list.itemName = "";
    list.itemQuantity = "";
  
    list.addItem = function () {
      shoppingList.addItem(list.itemName, list.itemQuantity);
      list.title = origTitle + " (" + list.items.length + " items)"; // Dinamically change title in the page
    };
  
    list.removeItem = function (itemIndex) {
      shoppingList.removeItem(itemIndex);
      list.title = origTitle + " (" + list.items.length + " items)"; // Dinamically change title in the page
    };
  }
  
  // LIST #1 - Controller
  ShoppingListController1.$inject = ['ShoppingListFactory'];
  function ShoppingListController1(ShoppingListFactory) {
    var list = this;
  
    // Use factory to create new shopping list service
    var shoppingList = ShoppingListFactory();
  
    list.items = shoppingList.getItems();
    var origTitle = "Shopping List #1";
    list.title = origTitle + " (" + list.items.length + " items)";  // Dinamically change title in the page
  
    list.itemName = "";
    list.itemQuantity = "";
  
    list.addItem = function () {
      shoppingList.addItem(list.itemName, list.itemQuantity);
      list.title = origTitle + " (" + list.items.length + " items)"; // Dinamically change title in the page
    };
  
    list.removeItem = function (itemIndex) {
      shoppingList.removeItem(itemIndex);
      list.title = origTitle + " (" + list.items.length + " items)"; // Dinamically change title in the page
    };
  }
  
  
  // LIST #2 - controller
  ShoppingListController2.$inject = ['ShoppingListFactory'];
  function ShoppingListController2(ShoppingListFactory) {
    var list = this;
  
    // Use factory to create new shopping list service
    var shoppingList = ShoppingListFactory(3);
  
    list.items = shoppingList.getItems();

    var origTitle = "Shopping List #2 (limited to 3 items - ";
    list.title = origTitle + " actual with " + list.items.length + " items)";

    list.itemName = "";
    list.itemQuantity = "";
  
    list.addItem = function () {
      try {
        shoppingList.addItem(list.itemName, list.itemQuantity);
        list.title = origTitle + " actual with " + list.items.length + " items)";
      } catch (error) {
        list.errorMessage = error.message;
      }
  
    };
  
    list.removeItem = function (itemIndex) {
      shoppingList.removeItem(itemIndex);
      list.title = origTitle + " actual with " + list.items.length + " items)";
    };
  }
  
  
  // If not specified, maxItems assumed unlimited
  function ShoppingListService(maxItems) {
    var service = this;
  
    // List of shopping items
    var items = [];
  
    service.addItem = function (itemName, quantity) {
      if ((maxItems === undefined) ||
          (maxItems !== undefined) && (items.length < maxItems)) {
        var item = {
          name: itemName,
          quantity: quantity
        };
        items.push(item);
      }
      else {
        throw new Error("Max items (" + maxItems + ") reached.");
      }
    };
  
    service.removeItem = function (itemIndex) {
      items.splice(itemIndex, 1);
    };
  
    service.getItems = function () {
      return items;
    };
  }
  
  
  function ShoppingListFactory() {
    var factory = function (maxItems) {
      return new ShoppingListService(maxItems);
    };
  
    return factory;
  }
  
  })();