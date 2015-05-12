(function (angular) {
    'use strict';

    var controllers = angular.module('controllers', []);

    //#region HomeCtrl
    controllers.controller('HomeCtrl', ['$scope', '$signalR', '$routeParams', 'CurrentGame', function ($scope, $signalR, $routeParams, CurrentGame) {
        $scope.title = 'Home';        
       
        //$scope.$watch('game', function () {
        //    if ($scope.game) {                
        //        $scope.history = $scope.game.history;
        //        //$scope.$safeApply();
        //    }
        //})

        var gameTypeId = $routeParams.id;
        if (!gameTypeId) gameTypeId = 1;


        $scope.movesBackStack = [];

        $scope.makeEngineMove = function engineMove() {
            var position = $scope.game.fen()
            $signalR.$emit('chessClientMoveEvent', position);
        }
        
        $scope.onBoardChanged = function onBoardChanged(oldPosition, newPosition)
        {         
            //if ($scope.game.turn() == 'b')
              //  $scope.makeEngineMove();
        }

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
        }

        $scope.moveForward = function moveForward(){
            var move = $scope.movesBackStack.pop();
            $scope.game.move(move);
            $scope.board.position($scope.game.fen());
        }

       
        $scope.save = function save() {
           
            debugger;
            //CurrentGame.update($scope.game.pgn());
           // curGame.data = $scope.game.pgn();
            //var curGame = new CurrentGame();
            //CurrentGame.update({ id: 3}, {data: "testtest" });

            var curGameFromServer = CurrentGame.get();

            //var curGame = new CurrentGame();
            //curGame.data = "test";// $scope.game.pgn();
            //curGame.$save();

            //CurrentGame.save({ id: 3 }, { data: 'testtest' });
            CurrentGame.save({ id: 3 }, 'testtest');

            //curGame.$save(function (data) {
            //    //$safeApply($scope, function () {
            //    //    $scope.todoLists.push(data);
            //    //    $scope.newTodoListName = '';
            //    //});
            //});

        }

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