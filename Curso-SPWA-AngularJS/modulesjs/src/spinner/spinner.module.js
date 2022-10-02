(function () {
    'use strict';
    
    angular.module('Spinner', []);
    
    angular.module('Spinner')
    .config(function () {
      console.log("Spinner Config Fired!");
    }).
    run(function () {
      console.log("Spinner Run Fired!");
    });
    
    
    })();