angular.module('eidApp.EmitentModule', ['eidApp.EmitentModule.Controllers', 'eidApp.EmitentModule.services', 'ui.router']).config(['$stateProvider', '$locationProvider', function ($stateProvider, $locationProvider) {

    $stateProvider.state('allEmitents', {
        url: '/emitents',
        templateUrl: 'Partials/emitents/emitents.html',
        controller: 'EmitentController'
    });

    $stateProvider.state('singleEmitent', {
        url: '/emitents/:id',
        templateUrl: 'Partials/emitents/singleEmitent.html',
        controller: 'EmitentDetailsController'
    });

}]);
