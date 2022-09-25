(function () {
    'use strict';
    
    angular.module('NarrowItDownApp', [])
    .controller('NarrowItDownController', NarrowItDownController)
    .service('MenuSearchService', MenuSearchService)
    .directive('foundItems', FoundItemsDirective)
    .constant('ApiBasePath', "https://davids-restaurant.herokuapp.com");
    
    
    NarrowItDownController.$inject = ['MenuSearchService'];
    function NarrowItDownController(MenuSearchService) {
      var menu = this;
    
      menu.search = function (searchTerm) {

        if (searchTerm === "" || searchTerm === undefined || !searchTerm){
          menu.error = "Search can't be done beacause nothing was type!";
          return;
        } else {
          menu.error = "";
        }

        var promise = MenuSearchService.getMatchedMenuItems(searchTerm);
    
        promise.then(function (response) {
           menu.items = response;        
        })
        .catch(function (error) {
            console.log(error);
        })
      };

      menu.removeItem = function(index) {
        menu.items.splice(index, 1);
      };
    
    }

    MenuSearchService.$inject = ['$http', 'ApiBasePath'];
    function MenuSearchService($http, ApiBasePath) {
      var service = this;

      service.getMatchedMenuItems = function(searchTerm) {
        return $http({
          method: 'GET',
          url: (ApiBasePath + '/menu_items.json')
        }).then(function (result) {
          var items = result.data.menu_items;

          var foundItems = [];

          for (var i = 0; i < items.length; i++) {
            // Searching the items in the restaurant list, but only saving the itens 
            // with the description typed
            if (items[i].description.toLowerCase().indexOf(searchTerm.toLowerCase()) >= 0) {
                foundItems.push(items[i]);
            }
          }

          return foundItems;
        });
      };
    }

    // Directive
    function FoundItemsDirective() {
      var ddo = {
        templateUrl: 'found.html',
        scope: {
          found: '<',
          onRemove: '&'
        },
        controller: FoundItemsDirectiveController,
        controllerAs: 'list',
        bindToController: true
      };

      return ddo;
    }
    
    // Controller of the Directive
    function FoundItemsDirectiveController() {
      var list = this;
  
      list.isEmpty = function() {
        return list.found != undefined && list.found.length === 0;
      }
    }
    
    })();