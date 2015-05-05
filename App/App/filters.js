(function (angular) {
    'use strict';
    var filters = angular.module('filters', []);

    filters.filter('turnnote', function () {
        return function (input) {
            switch (input) {
                case 'w':
                    return "White To Move";
                case 'b':
                    return "Black To Move";
            }
        }
    });

})(window.angular);



