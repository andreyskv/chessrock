(function (angular, $) {
    'use strict';

    var services = angular.module('services', []);
    
    //#region $auth
    services.factory('$auth', ['$q', '$http', '$path', function ($q, $http, $path) {
        
        var profileUrl = $path('api/Account/Profile');
        var tokenUrl = $path('api/Account/Token');

        function setupAuth(accessToken, remember) {
            var header = 'Bearer ' + accessToken;
            delete $http.defaults.headers.common['Authorization'];
            $http.defaults.headers.common['Authorization'] = header;
            sessionStorage['accessToken'] = accessToken;
            if (remember) {
                localStorage['accessToken'] = accessToken;
            }
            return header;
        }
        
        function clearAuth() {
            sessionStorage.removeItem('accessToken');
            localStorage.removeItem('accessToken');
            delete $http.defaults.headers.common['Authorization'];
        }

        var self = {};
        var userName;

        self.getUserName = function() {
            return userName;
        };

        self.isAuthenticated = function() {
            return userName && userName.length;
        };

        self.loadSaved = function() {
            var deferred = $q.defer();
            var accessToken = sessionStorage['accessToken'] || localStorage['accessToken'];
            if (accessToken) {
                setupAuth(accessToken);
                $http.get(profileUrl, { ignoreAuthModule: true })
                    .success(function(data) {
                        userName = data.userName;
                        deferred.resolve({
                            userName: data.userName
                        });
                    })
                    .error(function() {
                        clearAuth();
                        deferred.reject();
                    });
            } else {
                deferred.reject();
            }

            return deferred.promise;
        };

        self.login = function(user, passw, rememberMe) {
            var deferred = $q.defer();
            $http.post(tokenUrl, { userName: user, password: passw })
                .success(function (data) {
                    var header = setupAuth(data.accessToken, rememberMe);
                    deferred.resolve({
                        userName: data.userName,
                        Authorization: header
                    });
                })
                .error(function() {
                    deferred.reject();
                });

            return deferred.promise;
        };

        return self;
    }]);
    //#endregion

    //#region $safeApply
    services.factory('$safeApply', function () {
        return function($scope, fn) {
            var phase = $scope.$root ? $scope.$root.$$phase : null;
            if (phase == '$apply' || phase == '$digest') {
                if (fn) {
                    $scope.$eval(fn);
                }
            } else {
                if (fn) {
                    $scope.$apply(fn);
                } else {
                    $scope.$apply();
                }
            }
        };
    });
    //#endregion
    
    //#region $path
    services.factory('$path', function () {
        var uri = window.location.href;
        var ind = uri.indexOf('#');

        var baseUrl = uri;

        if (ind > 0) {
            baseUrl = uri.substr(0, ind);
        }

        if (baseUrl[baseUrl.length - 1] != '/') {
            baseUrl = baseUrl + '/';
        }

        return function(url) {
            return baseUrl + url;
        };
    });
    //#endregion
    
    //#region $signalR
    services.factory('$signalR', ['$rootScope', function ($rootScope) {
        var self = $rootScope.$new();
        
        ////Log4Net.SignalR
        //var log4Net = $.connection.signalrAppenderHub;
        //log4Net.client.onLoggedEvent = function (loggedEvent) {
        //    debugger;
        //    self.$emit('loggedEvent', loggedEvent);
        //};
        //$.connection.hub.start();

        var chess = $.connection.chessHub;       
        chess.client.sendChessMoveToClient = function (move) {            
            self.$emit('chessServerMoveEvent', move);
        };

        $.connection.hub.start().done(function () {    
            chess.server.startGame();
            self.$on('chessClientMoveEvent', function (e, fenpos) {                
                chess.server.fen(fenpos);
            });
        });


        //$.connection.hub.start().done(function () {
        //    $('#sendmessage').click(function () {
        //        // Call the Send method on the hub. 
        //        chat.server.send($('#displayname').val(), $('#message').val());
        //        // Clear text box and reset focus for next comment. 
        //        $('#message').val('').focus();
        //    });
        //});

        return self;
    }]);
    //#endregion

})(window.angular, window.jQuery);