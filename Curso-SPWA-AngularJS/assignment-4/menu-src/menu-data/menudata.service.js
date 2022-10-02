(function () {
    'use strict';
  
    angular.module('data')
    .service('MenuDataService', MenuDataService)
    .constant('apiBasePath', ' https://davids-restaurant.herokuapp.com'); // REST API
  
    // MenuDataService - to declare the functions to access the davids-restaurant REST API
    MenuDataService.$inject = ['$http', 'apiBasePath'];
    function MenuDataService ($http, apiBasePath) {
      var service = this;
      
      // Get all the categories at once
      service.getAllCategories = function () {
        var result = $http({
            method: 'GET',
            url: (apiBasePath + '/categories.json')
        });
  
        return result;
      }
      
      // Get itens for a specific category
      service.getItemsForCategory = function (categoryShortName) {
        return $http({
           method: 'GET',
           url: (apiBasePath + '/menu_items.json?category=' + categoryShortName)
        });
      }
    }
  
  })();