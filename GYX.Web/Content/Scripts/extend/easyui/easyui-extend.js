//easyui扩展方法

/*------------------------------tree方法扩展、开始------------------------------*/
$.extend($.fn.tree.methods, {
    //1、获取tree的某个节点的一级子节点。
    //示例：$("#tree").tree('getFirstLevelChildren', node.target);
    getFirstLevelChildren: function (jq, params) {
        var nodes = [];
        $(params).next().children().children("div.tree-node").each(function () {
            nodes.push($(jq[0]).tree('getNode', this));
        });
        return nodes;
    },
    //2、获取tree中节点级别
    //示例：$("#tree").tree('getLevel', node.target);
    getLevel: function (jq, target) {
        var l = $(target).parentsUntil("ul.tree", "ul");
        return l.length + 1;
    }
});
/*------------------------------tree方法扩展、结束------------------------------*/

/*------------------------------treegrid方法扩展、开始------------------------------*/
$.extend($.fn.treegrid.methods, {
    //1、获取tree的某个节点的一级子节点。
    //示例：$("#treegrid").treegrid('getFirstLevelChildren', id);
    getFirstLevelChildren: function (jq, id) {
        var nodes = [];
        var source = $.data(jq[0], "treegrid").data;
        var idFieldName = $.data(jq[0], "treegrid").options.idField;
        var curData = findFromTreeData(source, id, idFieldName, 'children');
        if (curData['children']) {
            nodes = curData['children'];
        }
        return $.map(nodes, function (obj) {
            return $.extend(true, {}, obj);//返回对象的深拷贝
        });
    }
});

/*------------------------------treegrid方法扩展、结束------------------------------*/


/*------------------------------tabs方法扩展、结束------------------------------*/
$.extend($.fn.tabs.methods, {
    //1、根据id获取tab
    //示例：$("#tab").treegrid('getTabById', id);
    getTabById: function (jq, id) {
        var tabs = $.data(jq[0], 'tabs').tabs;
        for (var i = 0; i < tabs.length; i++) {
            var tab = tabs[i];
            if (tab.panel('options').id == id) {
                return tab;
            }
        }
        return null;
    }
});
/*------------------------------tabs方法扩展、结束------------------------------*/



//从树形的对象中查找数据
//source数据源，id：查找键值，idFieldName：主键名称，childrenName：子节点名称
function findFromTreeData(source, id, idFieldName, childrenName) {
    var cc = [source];
    while (cc.length) {
        var c = cc.shift();
        for (var i = 0; i < c.length; i++) {
            var curData = c[i];
            if (curData[idFieldName] == id) {
                return curData;
            } else {
                if (curData[childrenName]) {
                    cc.push(curData[childrenName]);
                }
            }
        }
    }
    return null;
}


/*------------------------------datagrid方法扩展、开始------------------------------*/
//默认的datagrid配置
$.fn.iDatagrid = function (options) {
    options = options == null ? {} : options;
    var default_config = {
        fit: true,
        toolbar: '#searchDiv',
        title: '',
        rownumbers: true,
        singleSelect: false,         //单选
        idField: 'Id',               //唯一列
        pagination: true,            //是否分页
        striped: true,              //是否显示斑马线效果
        remoteSort: false,          //是否从服务器对数据进行排序
        pageList: [10, 15, 20, 30, 50],
        onBeforeLoad: function (param) {
            param.pageIndex = param.page;
            param.pageSize = param.rows;
        },
        onLoadSuccess: function () {
            $(this).datagrid('clearSelections');
            $(this).datagrid('clearChecked');
        }
    };

    $.each(default_config, function (item, value) {
        options[item] = options[item] || value;
    });

    $(this).datagrid(options);
}
/*------------------------------datagrid方法扩展、开始------------------------------*/


/*------------------------------在最外一层打开dialog开始------------------------------*/
//确定按钮事件confirmClick(data)
//确定按钮后选中事件：selectClick(data)
function doSelect(options) {
    options = options == null ? {} : options;
    options.confirmClick = function (data) {
        var resultData = getIframe("dlgIframe" + data.randomId).getData();
        if (options.selectClick != null) {
            //options.selectClick(resultData);
            options.selectClick.call(this, resultData);
        }
    }

    openDialog(options);
}

//确定按钮事件confirmClick(data)
//确定按钮成功后触发事件afterSuccess(data)
function doForm(options) {
    options = options == null ? {} : options;
    if (options.afterSuccess == null)
        options.afterSuccess = function (data) {
            top.$.messager.alert('提示信息', '保存成功', 'info');
        };
    if (options.afterError == null)
        options.afterError = function (data) {
            top.$.messager.alert('错误提示', '保存失败', 'error');
        };
    options.confirmClick = function (data) {
        var resultData = getIframe("dlgIframe" + data.randomId).saveData();
        if (resultData.isSuccess == true) {
            options.afterSuccess.call(this, resultData);
        }
        else if (resultData.isSuccess == false) {
            options.afterError.call(this, resultData);
            return false;
        }
        else {
            return false;
        }
    }
    openDialog(options);
}

function getIframe(iframeId) {
    if (top.frames[iframeId].contentWindow != undefined &&
        ($.getBrowsername() == "Chrome" || $.getBrowsername() == "FF" || $.getBrowsername() == "Edge")) {

        return top.frames[iframeId].contentWindow;
    }
    else {
        return top.frames[iframeId];
    }
}
//确定按钮confirmClick，返回false中断
function openDialog(options) {
    options = options == null ? {} : options;
    var g = "";
    var i = 32;
    while (i--) {
        g += Math.floor(Math.random() * 16.0).toString(16);
    }
    var html = '<div id="dlg' + g + '" style="height:0px;"><iframe id="dlgIframe' + g + '" scrolling="auto" frameborder="0" style="width:100%;height:99%;"></iframe></div>' +
        '<div id="dlgButton' + g + '" class="my-dialog-button" style="display:none;">' +
        '<label class="fl"><input id="chkIsConfirmCloseDlg' + g + '" type="checkbox" checked="checked" />确认并关闭窗口</label>' +
        '<div class="fr"><span id="btnDlgConfirm' + g + '" class="my-btn-1">确定</span><span id="btnDlgClose' + g + '" class="my-btn-2">关闭</span></div>' +
        '</div>';

    var actionWindow = window;
    while (actionWindow != actionWindow.parent)
        actionWindow = actionWindow.parent;
    $W = $(actionWindow);

    if (!$($W[0].document).find('#dlg' + g)[0]) {
        $($W[0].document.getElementsByTagName('body')).append(html);
    }
    var $dialog = $($W[0].document.getElementById('dlg' + g));

    var default_config = {
        title: 'My Dialog',
        width: 800,
        height: 400,
        closed: false,
        cache: false,
        modal: true,
        buttons: '#dlgButton' + g,
        onClose: function () {
            $($dialog).remove();
        }
    }

    jQuery.each(default_config,
        function (item, value) {
            options[item] = options[item] || value;
        });
    if (!!options.url) {
        //options.content = '<iframe src="' + options.url + '" scrolling="auto" frameborder="0" style="width:100%;height:100%;"></iframe>';
        $($dialog).find('#dlgIframe' + g).attr('src', options.url);
    }

    //关闭按钮
    top.$('#btnDlgClose' + g).unbind();
    top.$('#btnDlgClose' + g).bind('click', function () {
        top.$('#dlg' + g).dialog('close');
    });

    //确定按钮
    top.$('#btnDlgConfirm' + g).unbind();
    top.$('#btnDlgConfirm' + g).bind('click', function () {
        if (options.confirmClick != null) {
            if (options.confirmClick.call(this, { randomId: g }) == false)
                return false;
        }
        if (top.$('#chkIsConfirmCloseDlg' + g).prop("checked")) {
            top.$('#dlg' + g).dialog('close');
        }
    });


    //options.buttons = [{
    //    text: '保存',
    //    handler: options.save
    //}, {
    //    text: '关闭',
    //    handler: options.close
    //}];

    top.$($dialog).dialog(options);
    //top.$('#dlg' + g).dialog('center');
    //top.$('.dialog-button').css('text-align', 'center');
}
/*------------------------------在最外一层打开dialog结束------------------------------*/