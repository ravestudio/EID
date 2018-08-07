angular.module('eidApp', ['eidApp.ChartModule', 'eidApp.EmitentModule', 'ui.router', 'ngResource']);

angular.module('eidApp').run(['$state', function ($state) {
    $state.go('allEmitents');
}]);