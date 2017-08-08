/****************************************************
*angular指令方式：将datePicker的值和angular的参数绑定
*****************************************************/
angular.module('datePicker', []).directive('datePicker', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        //          scope: {
        //              minDate: '',
        //          },
        link: function (scope, element, attr, ngModel) {

            element.val(ngModel.$viewValue);

            //function onpicking(dp) {
            //    var date = dp.cal.getNewDateStr();
            //    scope.$apply(function () {
            //        ngModel.$setViewValue(date);
            //    });
            //}
            function onpicked(dp) {
                scope.$apply(function () {
                    ngModel.$setViewValue(element.val());

                });
            }
            function oncleared() {
                scope.$apply(function () {
                    ngModel.$setViewValue('');
                });
            }
            element.bind('click', function () {
                WdatePicker({
                    //onpicking: onpicking,
                    oncleared: oncleared,
                    onpicked: onpicked,
                    dateFmt: (attr.datefmt || 'yyyy-MM-dd'),
                    minDate: (attr.mindate || ''),
                    maxDate: (attr.maxdate || '')
                    //minDate: (scope.minDate || '%y-%M-%d'),
                })
            });
        }
    };
});


/****************************************************
*jq方式：将datePicker的值和angular的参数绑定
*****************************************************/
//$(document).ready(function () {
//    $(".wdatePicker").bind('click', function () {
//        WdatePicker({
//            //onpicking: onpicking,
//            oncleared: oncleared,
//            //onpicked: onpicked
//            //dateFmt: (attr.datefmt || 'yyyy-MM-dd')
//            ////minDate: (scope.minDate || '%y-%M-%d'),
//        })
//    });
//    $(".wdatePicker").attr('onchange', 'changeDatePicker(this)');
//    //$(".wdatePicker").attr('onclick', 'WdatePicker()');
//    //$(".wdatePicker").attr('onpicked', 'fun1()');
//});
//function changeDatePicker(obj) {
//    var modelName = obj.getAttribute('ng-model');
//    var modelValue = obj.value;
//    //var appElement = document.querySelector('[ng-controller=NoticeController]');
//    var appElement = document.querySelector('[ng-controller]');
//    var $scope = angular.element(appElement).scope();
//    var nameAddress = modelName.split(".");
//    var temp = $scope;
//    $.each(nameAddress, function (i, item) {
//        if (i == nameAddress.length - 1) {
//            temp[item] = modelValue;
//        }
//        else {
//            temp = temp[item];
//        }
//    })
//    $scope.$apply();
//}
//function oncleared() {
//    var modelName = this.getAttribute('ng-model');
//    var modelValue = this.value;
//    //var appElement = document.querySelector('[ng-controller=NoticeController]');
//    var appElement = document.querySelector('[ng-controller]');
//    var $scope = angular.element(appElement).scope();
//    var nameAddress = modelName.split(".");
//    var temp = $scope;
//    $.each(nameAddress, function (i, item) {
//        if (i == nameAddress.length - 1) {
//            temp[item] = modelValue;
//        }
//        else {
//            temp = temp[item];
//        }
//    })
//    $scope.$apply();
//}



