angular.module('eidApp.EmitentModule.Controllers', []).controller('EmitentController', function ($scope, emitentService) {

    $scope.emitents = emitentService.query(); 

}).controller('EmitentDetailsController', ['$stateParams', '$state', '$scope', 'emitentService', function ($stateParams, $state, $scope, emitentService) {

    $scope.single = emitentService.get({ id: $stateParams.id });

}]);