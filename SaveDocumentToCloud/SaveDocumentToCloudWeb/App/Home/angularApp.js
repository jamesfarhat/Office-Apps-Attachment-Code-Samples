/// <reference path="../../Scripts/angular.min.js" />

actApp = angular.module("actApp", []);

// define custom directive
actApp.directive('mainView', function () {
    return {
        restrict: 'A',// We have created Attribute because in some browsers Element creates issue.
        templateUrl: '../View/main-view.html'
    };
});










