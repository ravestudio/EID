angular.module('eidApp.EmitentModule.services', []).factory('emitentService', function ($resource, API_ENDPOINT) {

    return $resource(API_ENDPOINT, { id: '@id' });

}).value('API_ENDPOINT', 'http://localhost:61943/api/emitent/:id');;