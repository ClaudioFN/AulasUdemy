(function () {
    'use strict';
    
    // Component to use the template HTML for the items in the category
    angular.module('MenuApp')
    .component('items', {
        templateUrl: 'menu-src/menu/templates-html/items.template.html',
        controller: 'ItemsController as items',
        bindings: {
          itemsList: '<'
        }
    })
})();