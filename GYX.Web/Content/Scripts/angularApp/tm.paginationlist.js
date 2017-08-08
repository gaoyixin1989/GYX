/**
 * Angularjs环境下分页
 * name: tm.pagination
 * Version: 1.0.0
 */
angular.module('tm.pagination', []).directive('tmPagination', [function () {
    return {
        restrict: 'EA',

        template: '<div class="paging">' +
                        '<span>共 {{conf.totalItems}} 条，页次 <b>{{conf.currentPage+"/"+conf.numberOfPages}}</b>，第<input type="text" ng-model="jumpPageNum"/>页</span>' +
                        '<a href="javascript:void(0);" class="a-1" ng-click="jumpToPage($event)"></a>' +
                        '<a href="javascript:void(0);" class="a-2" ng-class="{disabled: conf.currentPage == 1}" ng-click="firstPage()"></a>' +
                        '<a href="javascript:void(0);" class="a-3" ng-class="{disabled: conf.currentPage == 1}" ng-click="prevPage()"></a>' +
                        '<a href="javascript:void(0);" class="a-4" ng-class="{disabled: conf.currentPage == conf.numberOfPages}" ng-click="nextPage()"></a>' +
                        '<a href="javascript:void(0);" class="a-5" ng-class="{disabled: conf.currentPage == conf.numberOfPages}" ng-click="lastPage()"></a>' +
                    '</div>',

        replace: true,
        scope: {
            conf: '='
        },
        link: function (scope, element, attrs) {

            // 定义分页的长度必须为奇数 (default:9)
            scope.conf.pagesLength = parseInt(scope.conf.pagesLength) ? parseInt(scope.conf.pagesLength) : 9;
            if (scope.conf.pagesLength % 2 === 0) {
                // 如果不是奇数的时候处理一下
                scope.conf.pagesLength = scope.conf.pagesLength - 1;
            }

            // conf.erPageOptions该变量不需要
            if (!scope.conf.perPageOptions) {
                scope.conf.perPageOptions = [10, 15, 20, 30, 50];
            }

            // prevPage
            scope.prevPage = function () {
                if (scope.conf.currentPage > 1) {
                    scope.conf.currentPage -= 1;
                }
            };
            // nextPage
            scope.nextPage = function () {
                if (scope.conf.currentPage < scope.conf.numberOfPages) {
                    scope.conf.currentPage += 1;
                }
            };
            //firstPage
            scope.firstPage = function () {
                if (scope.conf.currentPage > 1) {
                    scope.conf.currentPage = 1;
                }
            }
            //lastPage
            scope.lastPage = function () {
                if (scope.conf.currentPage < scope.conf.numberOfPages) {
                    scope.conf.currentPage = scope.conf.numberOfPages;
                }
            }
            // 跳转页
            scope.jumpToPage = function () {
                scope.jumpPageNum = scope.jumpPageNum.replace(/[^0-9]/g, '');
                if (scope.jumpPageNum !== '') {
                    scope.conf.currentPage = scope.jumpPageNum;
                }
            };

            // 修改每页显示的条数
            scope.changeItemsPerPage = function () {
                // 清除本地存储的值方便重新设置
                if (scope.conf.rememberPerPage) {
                    localStorage.removeItem(scope.conf.rememberPerPage);
                }
            };



            function getPagination() {
                scope.conf.currentPage = parseInt(scope.conf.currentPage) ? parseInt(scope.conf.currentPage) : 1;
                scope.conf.totalItems = parseInt(scope.conf.totalItems);
                scope.conf.numberOfPages = Math.ceil(scope.conf.totalItems / scope.conf.itemsPerPage);
                if (scope.conf.currentPage < 1) {
                    scope.conf.currentPage = 1;
                }
                if (scope.conf.currentPage > scope.conf.numberOfPages) {
                    scope.conf.currentPage = scope.conf.numberOfPages;
                }
                scope.jumpPageNum = scope.conf.currentPage;

            }

            scope.$watch(function () {
                var newValue = scope.conf.currentPage + ' ' + scope.conf.totalItems + ' ';
                // 如果直接watch perPage变化的时候，因为记住功能的原因，所以一开始可能调用两次。
                //所以用了如下方式处理
                if (scope.conf.rememberPerPage) {
                    // 由于记住的时候需要特别处理一下，不然可能造成反复请求
                    // 之所以不监控localStorage[scope.conf.rememberPerPage]是因为在删除的时候会undefind
                    // 然后又一次请求
                    if (localStorage[scope.conf.rememberPerPage]) {
                        newValue += localStorage[scope.conf.rememberPerPage];
                    } else {
                        newValue += scope.conf.itemsPerPage;
                    }
                } else {
                    newValue += scope.conf.itemsPerPage;
                }
                return newValue;

            }, getPagination);


        }
    };
}]);
