(function(angular) {
    'use strict';

    angular.module('interceptors', [])
        .config(['$httpProvider', function ($httpProvider) {
        //    $httpProvider.interceptors.push('httpRequestInterceptorIECacheSlayer');
         //   $httpProvider.interceptors.push('errorHttpInterceptor');
            $httpProvider.interceptors.push('httpAjaxInterceptor');
        }])

        //#region httpRequestInterceptorIECacheSlayer
        // IE 8 cache problem - Request Interceptor - https://github.com/angular/angular.js/issues/1418#issuecomment-11750815
        //.factory('httpRequestInterceptorIECacheSlayer', ['$log', function($log) {
        //    return {
        //        request: function(config) {
        //            if (config.url.indexOf("App/") == -1) {
        //                var d = new Date();
        //                config.url = config.url + '?cacheSlayer=' + d.getTime();
        //            }
        //            $log.info('request.url = ' + config.url);

        //            return config;
        //        }
        //    };
        //}])
        //#endregion

        //#region errorHttpInterceptor
        //.factory('errorHttpInterceptor', ['$q',
        //    function($q) {
        //        return {
        //            response: function (response) {
        //                if (response.status == 401) {
        //                    return response;
        //                } else if (response.status == 400 && response.data && response.data.message) {
        //                    toastr.error(response.data.message);
        //                    return $q.reject(response);
        //                } else if (response.status === 0) {
        //                    toastr.error('Server connection lost');
        //                    return $q.reject(response);
        //                } else if (response.status >= 400 && response.status < 500) {
        //                    toastr.error('Server was unable to find' +
        //                        ' what you were looking for... Sorry!!');
        //                    return $q.reject(response);
        //                }
        //                return response;
        //            }
        //        };
        //    }])
        //#endregion
    
        //#region httpAjaxInterceptor
        .factory('httpAjaxInterceptor', ['$q', '$location', '$rootScope', '$timeout',
            function($q, $location, $rootScope, $timeout) {
                var queue = [];
                var timerPromise = null;
                var timerPromiseHide = null;

                function processRequest() {
                    queue.push({});
                    if (queue.length == 1) {
                        timerPromise = $timeout(function() {
                            if (queue.length) {
                                $rootScope.$broadcast('event:ajax-show');
                            }
                        }, 500);
                    }
                }

                function processResponse() {
                    queue.pop();
                    if (queue.length == 0) {
                        //Since we don't know if another XHR request will be made, pause before
                        //hiding the overlay. If another XHR request comes in then the overlay
                        //will stay visible which prevents a flicker
                        timerPromiseHide = $timeout(function() {
                            //Make sure queue is still 0 since a new XHR request may have come in
                            //while timer was running
                            if (queue.length == 0) {
                                $rootScope.$broadcast('event:ajax-hide');
                                if (timerPromiseHide) $timeout.cancel(timerPromiseHide);
                            }
                        }, 500);
                    }
                }

                return {
                    request: function(config) {
                        processRequest();
                        return config || $q.when(config);
                    },
                    response: function(response) {
                        processResponse();
                        return response || $q.when(response);
                    },
                    responseError: function(rejection) {
                        processResponse();
                        return rejection || $q.when(rejection);
                    }
                };
            }]);
        //#endregion

})(angular);

