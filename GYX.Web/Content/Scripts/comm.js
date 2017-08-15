// 公共方法 Create by 高怡鑫 2015-08-05
$.extend({
    /** 
    1. 设置cookie的值，把name变量的值设为value   
    example $.cookie(’name’, ‘value’);
    2.新建一个cookie 包括有效期 路径 域名等
    example $.cookie(’name’, ‘value’, {expires: 7, path: ‘/’, domain: ‘jquery.com’, secure: true});
    3.新建cookie
    example $.cookie(’name’, ‘value’);
    4.删除一个cookie
    example $.cookie(’name’, null);
    5.取一个cookie(name)值给myvar
    var account= $.cookie('name');
    **/
    cookieHelper: function (name, value, options) {
        if (typeof value != 'undefined') { // name and value given, set cookie
            options = options || {};
            if (value === null) {
                value = '';
                options.expires = -1;
            }
            var expires = '';
            if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
                var date;
                if (typeof options.expires == 'number') {
                    date = new Date();
                    date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
                } else {
                    date = options.expires;
                }
                expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
            }
            var path = options.path ? '; path=' + options.path : '';
            var domain = options.domain ? '; domain=' + options.domain : '';
            var secure = options.secure ? '; secure' : '';
            document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
        } else { // only name given, get cookie
            var cookieValue = null;
            if (document.cookie && document.cookie != '') {
                var cookies = document.cookie.split(';');
                for (var i = 0; i < cookies.length; i++) {
                    var cookie = jQuery.trim(cookies[i]);
                    // Does this cookie string begin with the name we want?
                    if (cookie.substring(0, name.length + 1) == (name + '=')) {
                        cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                        break;
                    }
                }
            }
            return cookieValue;
        }
    }

});


//获取request
function request(strParame) {
    var args = new Object();
    var query = location.search.substring(1);

    var pairs = query.split("&"); // Break at ampersand 
    for (var i = 0; i < pairs.length; i++) {
        var pos = pairs[i].indexOf('=');
        if (pos == -1) continue;
        var argname = pairs[i].substring(0, pos);
        var value = pairs[i].substring(pos + 1);
        value = decodeURIComponent(value);
        args[argname] = value;
    }
    return args[strParame];
}

//提交form数据到后台
function ajaxSubmit(url, fnbeforeSubmit, fnSuccess) {
    var options = {
        cache: false,
        async: false,
        beforeSubmit: fnbeforeSubmit,
        success: fnSuccess,
        url: url,
        type: "post"
    };
    //表单提交前进行校验 create by ssz
    try {
        if (!$("#form1").validate()) {
            return false;
        }
    }
    catch (e) {
    }

    $("#form1").ajaxSubmit(options);
    return false;
}

//$.ajax方法统一使用post
function ajaxPost(obj) {
    if (!obj.type) { obj.type = 'POST' }
    if (!obj.dataType) { obj.dataType = 'json' }
    if (!obj.error) {
        obj.error = function () {
            $.messager.alert('提示', 'ajax出错', 'error');
        }
    }
    $.ajax(obj);
}


//序列化表单数据为JSON字符串
$.fn.formtojsonStr = function () {
    var o = {};
    var a = $(this).serializeArray();
    $.each(a, function (i, item) {
        if (o[item.name] !== undefined) {
            if (!o[item.name].push) {
                o[item.name] = [o[item.name]];
            }
            o[item.name].push(item.value || '');
        } else {
            o[item.name] = item.value || '';
        }
    });
    return JSON.stringify(o);
}
//序列化表单数据为JSON对象
$.fn.formtojsonObj = function () {
    var o = {};
    var a = $(this).serializeArray();
    $.each(a, function (i, item) {
        if (o[item.name] !== undefined) {
            if (!o[item.name].push) {
                o[item.name] = [o[item.name]];
            }
            o[item.name].push(item.value || '');
        } else {
            o[item.name] = item.value || '';
        }
    });
    return o;
}

//将json绑定到对象上
function bindJsonToForm(formid, data) {
    var mainform = $("#" + formid);
    for (var p in data) {
        var ele = $("[name=" + p + "]", mainform);
        //针对复选框和单选框 处理
        if (ele.is(":checkbox,:radio")) {
            ele[0].checked = data[p] ? true : false;
        }
        else if (ele.is("label,span"))
            ele.html(data[p]);
        else {
            ele.val(data[p]);
        }
    }
}
//清空表单
function clearForm(formid) {
    document.getElementById(formid).reset();
    var arrItem = $('#' + formid + ' [name]');
    $(arrItem).each(function (i, item) {
        //        if (item.tagName.toLowerCase() == 'label' || item.tagName.toLowerCase() == 'span')
        //            $(item).html('');
        //        else if (item.tagName.toLowerCase() == 'input') {

        //        }

        var jqItem = $(item);
        if (jqItem.is("label,span"))
            jqItem.html('');
        else if (jqItem.is("input")) {
            jqItem.not(":button, :submit, :reset, :hidden").val("").removeAttr("checked").remove("selected");
        }
        else
            jqItem.val('');
    })
}


//格式化时间
function parseToDate(value) {
    if (value == null || value == '') {
        return undefined;
    }

    var dt;
    if (value instanceof Date) {
        dt = value;
    }
    else {
        if (!isNaN(value)) {
            dt = new Date(value);
        }
        else if (value.indexOf('/Date') > -1) {
            value = value.replace(/\/Date\((-?\d+)\)\//, '$1');
            dt = new Date();
            dt.setTime(value);
        } else if (value.indexOf('/') > -1) {
            dt = new Date(Date.parse(value.replace(/-/g, '/')));
        } else {
            dt = new Date(value);
        }
    }
    return dt;
}
//格式化日期时间
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "H+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}
Date.prototype.addDays = function (d) {
    this.setDate(this.getDate() + d);
    return this;
};
Date.prototype.addWeeks = function (w) {
    this.addDays(w * 7);
    return this;
};
Date.prototype.addMonths = function (m) {
    var d = this.getDate();
    this.setMonth(this.getMonth() + m);
    if (this.getDate() < d)
        this.setDate(0);
    return this;
};
Date.prototype.addYears = function (y) {
    var m = this.getMonth();
    this.setFullYear(this.getFullYear() + y);
    if (m < this.getMonth()) {
        this.setDate(0);
    }
    return this;
};


//根据name属性值获取checkbox的Value
function CheckboxRadio_getValuesByName(name) {
    var arrBox = $("[name='" + name + "']");
    var value = [];
    $(arrBox).each(function (i, item) {
        if (item.checked) {
            value.push($(item).attr('value'));
        }
    });
    return value;
}
//根据name属性值获取checkbox的Text
function CheckboxRadio_getAttrByName(name, attr) {
    var arrBox = $("[name='" + name + "']");
    var value = [];
    $(arrBox).each(function (i, item) {
        if (item.checked) {
            value.push($(item).attr(attr));
        }
    });
    return value;
}
//根据name属性和arrValue设置checkbox的选中状态
function CheckboxRadio_setValueByName(name, arrValue) {
    var arrBox = $("[name='" + name + "']");
    $(arrBox).each(function (i, item) {
        if ($.inArray($(item).attr('value'), arrValue) >= 0)
            item.checked = true;
        else
            item.checked = false;
    });
}

//创建radio单选框html代码字符串
//data：数据源、name元素name值、DictCode：值、DictText：文本、
//hasDefault：是否初始默认值（设置默认值时，数据键名称为IsDefalut）
$.fn.createRadioHtml = function (option) {
    $(this).html('');
    var labForRadio = $('<div class="labForRadio"></div>')
    $(this).append(labForRadio);

    if (!option.data) { option.data = []; }
    if (!option.name) { option.name = '' }
    if (!option.DictCode) { option.DictCode = 'DictCode' }
    if (!option.DictText) { option.DictText = 'DictText' }
    if (!option.hasDefault) { option.hasDefault = false }

    $.each(option.data, function (i, item) {
        var strHtml = '<label><input type="radio" name="' + option.name + '" value="' + item[option.DictCode] + '" /><span>' + item[option.DictText] + '</span></label>';
        var curObj = $(strHtml);
        if (option.OnClick != null) {
            curObj.find('[type="radio"]').click(function () {
                var value=curObj.find('[type="radio"]:checked').val();
                option.OnClick.call(this, value);
            });
        }
        labForRadio.append(curObj);
    });
    
    if (option.hasDefault) {
        $.each(option.data, function (i, item) {
            if (item["IsDefalut"] == true) {
                labForRadio.find('[value="' + item[option.DictCode] + '"]').click();
            }
        });
    }

}

//根据字典数据生成easyui的下拉框
//data必填
$.fn.createEasyuiComboboxByDictData = function (options) {
    options = options == null ? {} : options;
    var default_config = {
        data:[],
        valueField: 'DictCode',
        textField: 'DictText',
        panelHeight: 'auto',
        height: 28,
        width: 163,
        multiple: false,
        hasDefault: false,//是否选中默认值
        onLoadSuccess: function () {
            if (options.hasDefault) {
                var selectedValues = [];
                $.each(options.data, function (i, item) {
                    if (item["IsDefalut"] == true) {
                        selectedValues.push(item["DictCode"]);
                    }
                });
                $(this).combobox('setValues', selectedValues);
            }
            
            if (options.onInitSuccess != null)
                options.onInitSuccess.call(this);
        }
    }
    $.each(default_config, function (item, value) {
        options[item] = options[item] || value;
    });

    $(this).combobox(options);
}

//根据字典数据生成easyui的下拉框树
//data必填
$.fn.createEasyuiCombotreeByDictData = function (options) {
    options = options == null ? {} : options;
    var default_config = {
        data: [],
        panelHeight: 'auto',
        height: 28,
        width: 163,
        //hasDefault: false,//是否选中默认值
        onLoadSuccess: function () {
            var selectedValues = [];
            if (options.onInitSuccess != null)
                options.onInitSuccess.call(this);
        }
    }
    $.each(default_config, function (item, value) {
        options[item] = options[item] || value;
    });

    $(this).combotree(options);
}

//返回打勾或打叉的img字符串
function getStateImgStr(isOk) {
    var str = '';
    isOk = isOk || false;
    if (isOk == true || isOk.toString().toLowerCase() == "true" || isOk == '1') {
        str = "<img src='/Content/Scripts/easyui/themes/icons/ok.png'></img>";
    }
    else {
        str = "<img src='/Content/Scripts/easyui/themes/icons/no.png'></img>";
    }
    return str;
}
//显示状态样式
//True或1-打勾，其他打叉
function formatState(value) {
    if (value == null) value = '';
    if (value == true || value.toString().toLowerCase() == "true" || value == "1") {
        return getStateImgStr(true);
    } else {
        return getStateImgStr(false);
    }
}
//显示状态样式_相反
//False或0-打勾，其他打叉
function formatStateAgainst(value) {
    if (value == null) value = '';
    if (value == false || value.toString().toLowerCase() == "false" || value == "0") {
        return getStateImgStr(true);
    } else {
        return getStateImgStr(false);
    }
}



//获取数组arr中的某个属性值，返回数组
function getAttrByArr(arr, attr) {
    var result = [];
    if (arr != null)
        $.each(arr, function (i, item) {
            result.push(item[attr]);
        });
    return result;
}

//深度克隆
function deepClone(obj) {
    var result, oClass = isClass(obj);
    //确定result的类型
    if (oClass === "Object") {
        result = {};
    } else if (oClass === "Array") {
        result = [];
    } else {
        return obj;
    }
    for (key in obj) {
        var copy = obj[key];
        if (isClass(copy) == "Object") {
            result[key] = arguments.callee(copy);//递归调用
        } else if (isClass(copy) == "Array") {
            result[key] = arguments.callee(copy);
        } else {
            result[key] = obj[key];
        }
    }
    return result;
}
//返回传递给他的任意对象的类
function isClass(o) {
    if (o === null) return "Null";
    if (o === undefined) return "Undefined";
    return Object.prototype.toString.call(o).slice(8, -1);
}


$.getBrowsername = function () {
    var userAgent = navigator.userAgent; //取得浏览器的userAgent字符串
    var isOpera = userAgent.indexOf("Opera") > -1;
    if (isOpera) {
        return "Opera"
    }; //判断是否Opera浏览器
    if (userAgent.indexOf("Firefox") > -1) {
        return "FF";
    } //判断是否Firefox浏览器
    if (userAgent.indexOf("Edge") > -1) {
        return "Edge";
    } //判断是否Firefox浏览器
    if (userAgent.indexOf("Chrome") > -1) {
        if (window.navigator.webkitPersistentStorage.toString().indexOf('DeprecatedStorageQuota') > -1) {
            return "Chrome";
        } else {
            return "360";
        }
    }//判断是否Chrome浏览器//360浏览器
    if (userAgent.indexOf("Safari") > -1) {
        return "Safari";
    } //判断是否Safari浏览器
    if (userAgent.indexOf("compatible") > -1 && userAgent.indexOf("MSIE") > -1 && !isOpera) {
        return "IE";
    }; //判断是否IE浏览器
}