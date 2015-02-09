var ajaxLoad = {}; 
ajaxLoad.jsonArray = []; //当前分页数据数组
ajaxLoad.childrenData = {};//子级数据绑定模板
formatfunc = {};//格式化方法命名空间
ajaxLoad.SortField = '';//排序字段名
ajaxLoad.IsAsc = false;//是否倒序排列，默认倒序，非倒序设为true
ajaxLoad.parm = {};//加载数据时传的参数键值对
ajaxLoad.isnotShow = false;
//加入jquery对象扩展方法
jQuery.fn.extend({
    loadModel: function () {//加载模板
        ajaxLoad.GridBody = this;//模板对象
        ajaxLoad.startData = $(this).html();//获取模板数据

        var re = /\{[\-\_A-Za-z1-9]+\}/g;//模板中父级的数据字段
        ajaxLoad.arrData = ajaxLoad.startData.match(re);

        ajaxLoad.count = $(this).find('td').length;//获取表格中td的数量
        $('#chkAll').click(function () { checkAll(this) });
    },
    sort: function (callback) {//排序的扩展方法
        this.prepend('<span class="glyphicon glyphicon-sort"></span>');
        this.click(function () { SortTable(this); if (callback) callback(); });
    },
    bindModel: function () {//绑定数据
        LoadPage(1);
    },
    action: function (url, callback, confirmMsg) {//点击列表上的按钮将会发出请求，并将选中checkbox的value提交
        if (url.indexOf('?') < 0) { url += '?' + Math.random(); } else { url += '&' + Math.random(); }
        this.click(function () {
            var ids = GetGridCheckIds();
            if (ids.length == 0) { warnshow('请选择至少一条数据！'); return; }
            if (confirmMsg && confirmMsg != '') {
                if (!confirm(confirmMsg)) return;
            }
            var _btn = $(this);
            postdata = { 'ids': ids.join(',') };
            _btn.attr('disabled', true);
            $.mpost(url, postdata, function (data, status) {
                _btn.removeAttr('disabled');
                if (status == 0) {
                    exshow(data);
                } else { callback(data); }
            });
        });
    },
    trim: function () {//去除文本框输入值的两边空白字符
        return $.trim(this.val());
    },
    getParams: function () {//获取对象里的文本框及下拉框的值,序列化为键值对，与jq的serialize()不同的是后者是序列化的字符串的格式，不方便更改
        var params = {};
        this.find('input,select,textarea').each(function () {
            var _name = $(this).attr('name');
            if (!_name || $.trim(_name) == '') return;
            var _val = $(this).trim();
            params[_name] = _val;
        });
        return params;
    }
});

var trLast = '';
function LoadPage(index) {//获取数据
    ajaxLoad.parm.pageIndex = index;
    ajaxLoad.parm.pageSize = ajaxLoad.parm.pageSize || 20;
    ajaxLoad.parm.sortField = ajaxLoad.SortField;
    ajaxLoad.parm.isAsc = ajaxLoad.IsAsc;
    trLast = "<tr><td colspan=\"" + ajaxLoad.count + "\" valign=\"middle\" style=\"height: 300px; text-align: center; background-color: #CCCCCC;color:#535353;;\">";
    $.ajax({
        url: ajaxLoad.url, //设置url
        global: false,
        timeout: 30000,
        type: "POST",
        datatype: "html",
        data: ajaxLoad.parm, //设置parm
        beforeSend: function () {
            var newtr = trLast + "<img src=\"/Images/loading.gif\" align=\"absmiddle\" /></td></tr>";
            ajaxLoad.GridBody.empty().append(newtr);
        },
        success: function (data) {
            ajaxLoad.jsonArray = data.list; //设置jsonArray
            LoadData(data);
            ajaxLoad.GridBody.find('tr').mouseover(function () { this.bgColor = '#d5f4fe' }).mouseout(function () { this.bgColor = '#FFFFFF' });
            if (ajaxLoad.edit) { ajaxLoad.GridBody.find('tr').dblclick(function () { $(this).editData(); }) };
            if (ajaxLoad.succfunc)
                ajaxLoad.succfunc();
            formatData();
        },
        error: function (xmlHttpRequest, error) {
            $("#mytable tr:gt(0)").remove();
            if (error == "timeout") {
                errtr = trLast + " 请求超时. <a href=\"javascript:LoadPage(" + index + ");\" style='color:blue;'> 重试 </a></td></tr>";
            } else {
                errtr = trLast + " 登录已超时. 请重新<a href=\"/Account\" style='color:blue;'> 登陆 </a></td></tr>";
            }
            ajaxLoad.GridBody.empty().append(errtr);
        }
    });
}
function LoadData(data) {//装载数据
    ajaxLoad.GridBody.empty();
    if (ajaxLoad.jsonArray.length == 0) {
        ajaxLoad.GridBody.append(trLast + data.Msg + "</td></tr>");
    } else {
        for (var i = 0; i < ajaxLoad.jsonArray.length; i++) {
            var datamodel = getModelData(ajaxLoad.jsonArray[i]);
            var $data = $(datamodel);
            ajaxLoad.GridBody.append($data);
            var lastTr = $data.attr('id', 'row' + i).attr('row', i);
            var lev = 1;
            var json = ajaxLoad.jsonArray[i];
            GetChildrenData(lastTr, lev, json);
        }
        var pageCount = parseInt(data.total / ajaxLoad.parm.pageSize, 10) + (data.total % ajaxLoad.parm.pageSize > 0 ? 1 : 0);
        $("#pager").pager({ pagenumber: ajaxLoad.parm.pageIndex, pagecount: pageCount, totalcount: data.total, buttonClickCallback: LoadPage });
    }
}
function getModelData(json) {//替换模板获取单行文本
    var rer = /[\{\}]/g;
    var NewData = ajaxLoad.startData;
    $.each(ajaxLoad.arrData, function (i, n) {
        var name = n.replace(rer, '');
        var reg = new RegExp("\\{" + name + "\\}", "g");
        NewData = NewData.replace(reg, json[name]);
    });
    return NewData;
}

function getModelChildrenData(json, startData, arrData) {//替换模板获取单行文本（子级数据）
    var rer = /[\{\}]/g;
    var NewData = startData;
    $.each(arrData, function (i, n) {
        var name = n.replace(rer, '');
        var realName = name.replace(/[\.]/g, '');
        var reg = new RegExp("\\{" + name + "\\}", "g");
        NewData = NewData.replace(reg, json[realName]);
    });
    return NewData;
}
function GetChildrenData(lastTr, lev, json) {//装载子级数据，【bug待解决：子级不能使纯文本；第一个子级，要么都有lev属性，要么都没有】
    var childrenStartData = lastTr.find('[parentField][lev=' + lev + ']');
    if (lev == 1) {
        var childrenStartDataOne = lastTr.find('[parentField]').not('[lev]');
        if (childrenStartData.length === 0) childrenStartData = childrenStartDataOne;
    }
    if (childrenStartData && childrenStartData.length > 0) {
        lev++;
        childrenStartData.each(function () {
            var parentField = $(this).attr('parentField');
            if (!parentField || $.trim(parentField) == '') return;
            var startDataNew = $(this).html(); $(this).empty();
            var jsonDataArrayNew = json[parentField];
            if (!jsonDataArrayNew || jsonDataArrayNew.length == 0) return;
            var reg = new RegExp("\\{" + GetHeng(lev) + "[A-Za-z1-9]+\\}", "g");
            var arrDataNew = startDataNew.match(reg);
            for (var j = 0; j < jsonDataArrayNew.length; j++) {
                var targetDataNew = getModelChildrenData(jsonDataArrayNew[j], startDataNew, arrDataNew);
                var $targetDataNew = $(targetDataNew);
                $(this).append($targetDataNew);
                GetChildrenData($targetDataNew, lev, jsonDataArrayNew[j]);
            }
        })
    }
}
function GetHeng(cnt) {//获取子级字段（用于分辨层级别）
    var heng = '';
    for (var i = 0; i < cnt-1; i++) {
        heng += '\\.';
    }
    return heng;
}
function formatData() {//格式化数据
    for (var callFuncName in formatfunc) {
        ajaxLoad.GridBody.find('[formatData]').each(function () {
            var data = $.trim($(this).text());
            var funcName = $(this).attr('formatData');
            if (funcName && funcName != '' && callFuncName == funcName) {
                var callfunc = formatfunc[callFuncName];
                var resultData = callfunc(data);
                if (resultData) $(this).html(resultData);
            }
        });
    }
}
//全选
function checkAll(obj) {
    ajaxLoad.GridBody.find(':checkbox:enabled').each(function () {
        this.checked = obj.checked;
    });
}
//获取所有选中checkbox的value值，返回数组对象
function GetGridCheckIds() {
    var chkArr = new Array();
    ajaxLoad.GridBody.find(':checkbox:enabled:checked').each(function () {
        var iid = $(this).val();
        chkArr.push(iid);
    })
    return chkArr;
}

document.onkeydown = function (event) {//回车搜索
    var e = event || window.event || arguments.callee.caller.arguments[0];
    if (e && e.keyCode == 13) { // enter 键 要做的事情
        var searchBtn = $("#btnSearch").get(0);
        if (searchBtn) searchBtn.click();
    }
};

//排序列表
function SortTable(o) {
    if (o != null) {
        var obj = $(o);
        $(".sort").each(function () {
            if ($(this).get(0) != obj.get(0)) {
                $(this).find('span').attr("class", "glyphicon glyphicon-sort");
            }
        });

        ajaxLoad.SortField = obj.attr("field");
        var _span = obj.find('span');
        var tc = _span.attr("class");
        switch (tc) {
            case "glyphicon glyphicon-sort":
                _span.attr("class", "glyphicon glyphicon-sort-by-attributes");
                ajaxLoad.IsAsc = true;
                break;
            case "glyphicon glyphicon-sort-by-attributes":
                _span.attr("class", "glyphicon glyphicon-sort-by-attributes-alt");
                ajaxLoad.IsAsc = false;
                break;
            case "glyphicon glyphicon-sort-by-attributes-alt":
                _span.attr("class", "glyphicon glyphicon-sort-by-attributes");
                ajaxLoad.IsAsc = true;
                break;
        }
    }
}

$.extend({//ajax请求封装（此处加上了验证，并有提示和异常捕获）
    mpost: function (url, parm, callback) {
        var status = 0; //状态消息
        var result = ''
        if (ajaxLoad.validataObj)//是否验证
            result = ajaxLoad.validata();
        if (result != '') { callback(result, status); return; }
        $.ajax({
            url: url, //设置url
            global: false,
            timeout: 30000,
            type: "POST",
            data: parm, //设置parm 
            beforeSend: function () {
                if (!ajaxLoad.isnotShow)//加载提示
                    loadingshow();
            },
            success: function (data) {
                status = 1; //状态消息成功返回
                if (!ajaxLoad.isnotShow) {
                    var dialog = art.dialog.get('artmsg');//关闭加载提示
                    if (dialog) dialog.close();
                }
                callback(data, status);
            },
            error: function (xmlHttpRequest, error) {
                if (!ajaxLoad.isnotShow) {
                    var dialog = art.dialog.get('artmsg');//关闭加载提示
                    if (dialog) dialog.close();
                }
                result = "Exception:" + error;//发生异常
                callback(result, status);
            }
        })
    }
});