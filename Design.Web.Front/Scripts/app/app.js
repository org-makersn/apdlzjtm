//'use strict';
var designApp = angular.module('main', ['ngRoute', 'ngGrid', 'ngResource', 'design.services', 'design.directives']);     //Define the main module

designApp
    .config(['$routeProvider', function ($routeProvider) {
        $routeProvider
            .when('/main', { templateUrl: '/Main/Index', controller: 'MainController' })
            .when('/about', { templateUrl: '/Info/About', controller: 'InfoController' })
            .when('/blog', { templateUrl: '/Info/blog', controller: 'InfoController' })
            .when('/customer', { templateUrl: '/Info/customer', controller: 'InfoController' })
            .when('/notice', { templateUrl: '/Info/notice', controller: 'InfoController' })
            .when('/license', { templateUrl: '/Info/license', controller: 'InfoController' })
            .when('/terms', { templateUrl: '/Info/terms', controller: 'InfoController' })
            .when('/privacy', { templateUrl: '/Info/privacy', controller: 'InfoController' })
            //.otherwise({ redirectTo: '/main' });
    }])
    .controller('RootController', ['$scope', '$route', '$routeParams', '$location', function ($scope, $route, $routeParams, $location) {
        $scope.$on('$routeChangeSuccess', function (e, current, previous) {
            $(".back-to-top").trigger('click');
            $scope.activeViewPath = $location.path();
        });
    }]);
