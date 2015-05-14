(function (angular) {
    'use strict';

    var controllers = angular.module('controllers', []);

    //#region HomeCtrl
    controllers.controller('HomeCtrl', ['$scope', '$signalR', '$route', '$routeParams', 'CurrentGame', function ($scope, $signalR, $route, $routeParams, CurrentGame) {
        $scope.title = 'Home';        
       

        //var lastRoute = $route.current;
        //$scope.$on('$locationChangeSuccess', function (event) {
        //    debugger;
        //    if (!$routeParams.id)
        //        $route.current = lastRoute;
        //});

        //$scope.$on('$routeChangeStart', function (e, next, last) {
        //    debugger;
        //    if (next.$$route.controller === last.$$route.controller) {
        //        e.preventDefault();
        //        $route.current = last.$$route;
        //        //do whatever you want in here!
        //    }
        //});
        debugger;
        var lastRoute = $route.current;
        $scope.$on('$locationChangeSuccess', function (event) {
            
            //if (lastRoute.$$route.originalPath === $route.current.$$route.originalPath) {
            if (lastRoute.$$route.controller === $route.current.$$route.controller) {
                debugger;
//                $route.current.
                var params = $route.current.params;
                if (params.id)
                    event.currentScope.init(params.id);

                $route.current = lastRoute;
            }
        });
        
        $scope.movesBackStack = [];

        var gameTypeId = $routeParams.id;
   
        $scope.reset = function reset() {            
            $scope.game.reset();
            $scope.board.start();
            $scope.movesBackStack = [];
        };

        $scope.init = function init(type) {
            
            if (type)
                $scope.reset();

            switch (type) {
                case 'playwhite':
                    $scope.board.orientation('white');
                    break;
                case 'playblack':                    
                    $scope.board.orientation('black');
                    $scope.makeEngineMove();
                    break;
                default:
                    $scope.load();
            };

        };
  
        $scope.$watchGroup(['game', 'board'], function () {            
            if ($scope.game && $scope.board) {                
                $scope.init(gameTypeId);                
            }
        });


        $scope.makeEngineMove = function engineMove() {
            var position = $scope.game.fen()
            $signalR.$emit('chessClientMoveEvent', position);
        };
        
        $scope.onBoardChanged = function onBoardChanged(oldPosition, newPosition) {
            //if ($scope.game.turn() == 'b')
            //  $scope.makeEngineMove();
            $scope.save();
        };

        $signalR.$on('chessServerMoveEvent', function (e, moveFromServer) {            
            $scope.game.move(moveFromServer);
            $scope.board.position($scope.game.fen());
        });


        $scope.moveBack = function moveBack() {

            var move = $scope.game.undo();
            if (move) {
                $scope.movesBackStack.push(move);
                $scope.board.position($scope.game.fen())
            }
        };

        $scope.moveForward = function moveForward() {
            var move = $scope.movesBackStack.pop();
            $scope.game.move(move);
            $scope.board.position($scope.game.fen());
        };

       
        $scope.save = function save() {

            var data = { pgn: $scope.game.pgn(), boardorientation: $scope.board.orientation() }
            CurrentGame.save(data);
        };

        $scope.load = function load() {

            var curGameFromServer = CurrentGame.get(function (data) {
                $scope.game.load_pgn(curGameFromServer.pgn);
                $scope.board.orientation(curGameFromServer.boardorientation);
                $scope.board.position($scope.game.fen());
            });
        };


    }]);
    //#endregion

    //#region TodosCtrl
    controllers.controller('TodosCtrl', ['$scope', '$safeApply', 'TodoList', 'TodoItem',
        function ($scope, $safeApply, TodoList, TodoItem) {
            $scope.newTodoListName = '';
            debugger;
            $scope.todoLists = TodoList.query();

            $scope.expand = function (todoList) {
                if (todoList.todoItems) return;
                todoList.todoItems = TodoList.todos({ id: todoList.id });
            };

            $scope.addTodoList = function () {                
                var todoList = new TodoList();
                todoList.title = $scope.newTodoListName;
                todoList.$save(function (data) {
                    $safeApply($scope, function () {
                        $scope.todoLists.push(data);
                        $scope.newTodoListName = '';
                    });
                });
            };
            
            $scope.removeTodoList = function (todoList) {
                var idTodoList = todoList.id;
                TodoList.remove(todoList, function () {
                    for (var i = 0; i < $scope.todoLists.length; i++) {
                        if ($scope.todoLists[i].id == idTodoList) {
                            $scope.todoLists.splice(i, 1);
                            break;
                        }
                    }
                });
            };
            
            $scope.addTodoItem = function (todoList) {
                var todoItem = new TodoItem();
                todoItem.title = todoList.newTodoItemName;
                todoItem.todoListId = todoList.id;
                debugger;
                todoItem.$save(function(data) {
                    $safeApply($scope, function () {
                        todoList.todoItems.push(data);
                        todoList.newTodoItemName = '';
                    });
                });
            };

            $scope.saveTodoItem = function(todoItem) {
                TodoItem.update(todoItem);
            };

            $scope.removeTodoItem = function(todoList, todo) {
                var idTodo = todo.id;
                TodoItem.remove(todo, function() {
                    for (var i = 0; i < todoList.todoItems.length; i++) {
                        if (todoList.todoItems[i].id == idTodo) {
                            todoList.todoItems.splice(i, 1);
                            break;
                        }
                    }
                });
            };
        }]);
    //#endregion
    
    //#region AboutCtrl
    controllers.controller('AboutCtrl', ['$scope', function ($scope) {
        $scope.title = 'About';
    }]);
    //#endregion
    
    //#region SettingsCtrl
    controllers.controller('SettingsCtrl', ['$scope', function ($scope) {
        $scope.title = 'Settings';
    }]);
    //#endregion

    //#region NavCtrl
    controllers.controller('NavCtrl', ['$scope', '$location',
        function ($scope, $location) {
            $scope.getClass = function (button) {
                var path = $location.path();
                if (path.indexOf(button) === 0) {
                    return 'active';
                } else {
                    return '';
                }
            };
        }]);
    //#endregion

    //#region LoginCtrl
    controllers.controller('LoginCtrl', ['$scope', '$safeApply', 'authService', '$rootScope', '$auth',
        function($scope, $safeApply, authService, $rootScope, $auth) {
            $scope.userName = '';
            $scope.password = '';
            $scope.rememberMe = false;

            $scope.signIn = function() {
                $auth.login($scope.userName, $scope.password, $scope.rememberMe)
                    .then(function (data) {
                        $safeApply($scope, function() {
                            $rootScope.userName = data.userName;
                        });
                        authService.loginConfirmed({
                                user: data.userName
                            },
                            function(config) {
                                config.headers.Authorization = data.Authorization;
                                return config;
                            });
                    });
            };

            $scope.cancel = function() {
                authService.loginCanceled();
            };

        }]);
    //#endregion
    
    //#region LogsCtrl
    controllers.controller('LogsCtrl', ['$scope', '$safeApply', '$signalR',
        function ($scope, $safeApply, $signalR) {
            $scope.logs = [];

            var logsType = [];
            logsType['FATAL'] = 'alert alert-error repeat-item';
            logsType['ERROR'] = 'alert alert-error repeat-item';
            logsType['WARN'] = 'alert alert-info repeat-item';
            logsType['INFO'] = 'alert alert-success repeat-item';
            logsType['DEBUG'] = 'alert alert-info repeat-item';

            $signalR.$on('loggedEvent', function (e, loggedEvent) {
                $safeApply($scope, function() {
                    loggedEvent.class = logsType[loggedEvent.Level];
                    $scope.logs.splice(0, 0, loggedEvent);
                });
            });

            $scope.clearLogs = function () {
                $scope.logs.length = 0;
            };
            
        }]);
    //#endregion

})(window.angular);