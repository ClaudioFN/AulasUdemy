(function () {
    'use strict';

    // To access the template HTML for MenuApp - Categories
    angular.module('MenuApp')
    .component('categories', {
        templateUrl: 'menu-src/menu/templates-html/categories.template.html',
        bindings: {
          categoriesList: '<'
        }
    })
  })();