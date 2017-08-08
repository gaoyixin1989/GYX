/**
 * 数字输入框，
 * Version: 1.0.0
 *目前仅实现了输入数字，待完善2016-04-18，gyx
 */
angular.module('number', []).directive("number", function () {
    return {
        restrict: "A",//A-只限属性使用
        require: "ngModel",  //控制器是指令标签对应的ngModel
        link: function (scope, element, attr, ngModel) {
            //element.on('keydown', function (e) {  //注册onChange事件，设置viewValue
            //    //ctrl.$setViewValue('hello');
            //    var ss = window.event || e;
            //    if (ss.keyCode != 8 && (ss.keyCode < 48 || ss.keyCode > 57)) {
            //        ss.preventDefault();
            //    }
            //});
            element.on('change', function (e) {  //注册onChange事件，设置viewValue
                //ctrl.$setViewValue('hello');
                if (this.value != null && this.value != '') {
                    if (isNaN(this.value)) {//非数值
                        //去掉0-9和-.之外的字符
                        this.value = this.value.replace(/[^0-9 . -]/g, '');
                        //第一位为“.”时，前面加0,如.12=>0.12
                        if (this.value.charAt(0) == '.') {
                            this.value = '0' + this.value;
                        }
                        //第一二位为“-.”时，-.12=>-0.12
                        if (this.value.indexOf("-.") == 0)
                            this.value = this.value.replace("-.", "-0.");

                        var index = 0;
                        while (index < this.value.length) {
                            var n = -1;
                            for (index = 0; index < this.value.length; index++) {
                                if (this.value.charAt(index) == '-' && index != 0) {//“-”不是在第一个位置的就删除
                                    this.value = this.value.substring(0, index) + this.value.substring(index + 1, this.value.length); n = -1; break;
                                }
                                if (this.value.charAt(index) == '.') {//“.”不是第一次出现就删除
                                    if (n == -1) { n = index; }
                                    else { this.value = this.value.substring(0, index) + this.value.substring(index + 1, this.value.length); n = -1; break; }
                                }
                                //最后一位为“-”“.”则删除
                                if (index == this.value.length - 1 && (this.value.charAt(index) == '.' || this.value.charAt(index) == '-')) {
                                    this.value = this.value.substring(0, index) + this.value.substring(index + 1, this.value.length); n = -1; break;
                                }
                            }
                        }

                    }

                    if (this.value != '' && !isNaN(this.value)) {
                        var numValue = parseFloat(this.value);
                        if ((!!attr.min) && (!isNaN(attr.min))) {//最小值
                            if (numValue < parseFloat(attr.min)) { this.value = attr.min; }
                        }
                        if ((!!attr.max) && (!isNaN(attr.max))) {//最大值
                            if (numValue > parseFloat(attr.max)) { this.value = attr.max; }
                        }
                        if ((!!attr.precision) && (!isNaN(attr.precision))) {//精度，小数点位数
                            var precision = parseInt(attr.precision);
                            if (precision >= 0) {
                                this.value = parseFloat(this.value).toFixed(precision);
                            }
                        }
                    }

                }
            });
        }
    }
})