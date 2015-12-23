var designServices = angular.module('design.services', ['ngResource']);

designServices.factory('TestService', [function () {
    var service = {};
    service.doWork = function () {
        console.log('Did some work !')
    }
    return service;
}]);