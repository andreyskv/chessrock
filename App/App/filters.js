(function (angular) {
    'use strict';
    var filters = angular.module('filters', []);

    filters.filter('note', function () {
        return function (input) {
            switch (input) {
                case 1:
                    return "one";
                case 2:
                    return "two";
            }
        }
    });

})(window.angular);



