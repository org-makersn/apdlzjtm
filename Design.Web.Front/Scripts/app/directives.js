var designDirectives = angular.module('design.directives', []);

designDirectives.directive('testDirective', function () {
    //use as 'test-directive' in HTML
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            console.log('Directive linked.');
        }
    };
});