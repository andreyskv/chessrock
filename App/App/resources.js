(function (angular) {
    'use strict';

    var resources = angular.module('resources', []);
    
    resources.factory('TodoItem', ['$resource', '$path', function ($resource, $path) {
        return $resource($path('api/TodoItem/:id'), { id: '@Id' }, {
            update: { method: 'PUT' }
        });
    }]);

    resources.factory('TodoList', ['$resource', '$path', function ($resource, $path) {
        return $resource($path('api/TodoList/:id/:action'), { id: '@Id' }, {
            todos: { method: 'GET', isArray: true, params: { action: 'Todos' } }
        });
    }]);    


})(window.angular);
