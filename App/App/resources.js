(function (angular) {
    'use strict';

    var resources = angular.module('resources', []);

    //#region TodoItem
    resources.factory('TodoItem', ['$resource', '$path', function ($resource, $path) {
        return $resource($path('api/TodoItem/:id'), { id: '@Id' }, {
            update: { method: 'PUT' }
        });
    }]);
    //#endregion

    //#region TodoList
    resources.factory('TodoList', ['$resource', '$path', function ($resource, $path) {
        return $resource($path('api/TodoList/:id/:action'), { id: '@Id' }, {
            todos: { method: 'GET', isArray: true, params: { action: 'Todos' } }
        });
    }]);
    //#endregion

    resources.factory('CurrentGame',['$resource', '$path', function ($resource, $path) {
        return $resource($path('api/PgnGame/current'), {
            get: { method: 'GET' }            
        });
    }]);

    resources.factory('PgnGame', ['$resource', '$path', function ($resource, $path) {
        return $resource($path('api/PgnGame/:id'), { id: '@Id' }, {
            get: { method: 'GET', isArray: true },
            update: { method: 'PUT' }
        });
    }]);

})(window.angular);
