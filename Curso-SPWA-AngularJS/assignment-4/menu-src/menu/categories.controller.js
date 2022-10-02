(function() {
    'use strict';
    
    // Controller to access the list of itens of a category
    angular.module('MenuApp')
    .controller('CategoriesController', CategoriesController);
    
    CategoriesController.$inject = ['categoriesList'];
    function CategoriesController (categoriesList) {
      var ctrl = this;
  
      this.categoriesList = categoriesList.data;
    }
}());