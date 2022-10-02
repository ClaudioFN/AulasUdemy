(function () {
    'use strict';
  
    angular.module('MenuApp')
    .config(RoutesConfig);
  
    RoutesConfig.$inject = ['$stateProvider', '$urlRouterProvider'];
    function RoutesConfig($stateProvider, $urlRouterProvider) {
  
      // Redirect to home page if no other URL matches
      $urlRouterProvider.otherwise('/');
  
      // *** Set up UI States - 1: HOME; 2: CATEGORIES; 3: ITEMS; ***
      $stateProvider
      // Home Page - 1
      .state('home', {
        url: '/',
        templateUrl: 'menu-src/menu/templates-html/home.template.html'
      })
      // Categories Page - 2
      .state('categories', {
        url: '/categories',
        templateUrl: 'menu-src/menu/templates-html/categories.template.html',
        controller: 'CategoriesController as categories',
        resolve: {
            categoriesList: ['MenuDataService', function (MenuDataService) {
              return MenuDataService.getAllCategories();
            }]
        }
      })
      // Items Page - 3
      .state('items', {
        url: '/categories/{category}/items',
        templateUrl: 'menu-src/menu/templates-html/items.template.html',
        controller:'ItemsController as items',
        resolve: {
            itemsList: ['MenuDataService', '$stateParams',
                function (MenuDataService, $stateParams) {
                  var result =  MenuDataService.getItemsForCategory($stateParams.category);
                  return result;
                }]
        }
      })
    }
  })();