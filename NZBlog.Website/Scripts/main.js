var SortField = "";//排序字段
var IsAsc = false; //是否升序

//排序列表
function SortTable(o) {
    if (o != null) {
        var obj = $(o);
        $(".sort").each(function () {
            if ($(this).get(0) != obj.get(0)) {
                $(this).attr("class", "headtd sort tbsort");
            }
        });

        SortField = obj.attr("field");
        var tc = obj.attr("class");
        switch (tc) {
            case "headtd sort tbsort":
                obj.attr("class", "headtd sort tbsortdown");
                IsAsc = true;
                break;
            case "headtd sort tbsortdown":
                obj.attr("class", "headtd sort tbsortup");
                IsAsc = false;
                break;
            case "headtd sort tbsortup":
                obj.attr("class", "headtd sort tbsortdown");
                IsAsc = true;
                break;
        }

        $("#hidSortField").val(SortField);
        $("#hidIsAsc").val(IsAsc);
    } 
}

function myAlert(sicon, stitle, scontent, callback, closeback) {
    if (stitle == undefined || stitle == "") stitle = "";
    art.dialog({
        width: 400,
        height: 150,
        id: 'artmsg',
        padding: 0,
        title: stitle,
        content: "<div style=\"width:380px;overflow:hidden;overflow-y:auto;white-space:normal;word-break:break-all;\">" + scontent + "</div>",
        icon: sicon,
        lock: true,
        ok: function () { if (callback != undefined) { callback(); } },
        close: function () { if (closeback != undefined) { closeback(); } }
    });
}

function CheckStatusSucc() {
    $("#btnSubmit").hide();
    $("#btnPrc").show();
}

function CheckStatusErr() {
    $("#btnSubmit").show();
    $("#btnPrc").hide();
}

function NoSelect(v) {
    if(document.getElementById("WarehouseName") != null)
        document.getElementById("WarehouseName").value = v;
    else
        document.getElementById("WarehouseNameEn").value = v;
}

function checkPhoneStatus(id,v) {
    if (v == "1")
        document.getElementById("iStatus_" + id + "").style.display = "";
    else
        document.getElementById("iStatus_" + id + "").style.display = "none";
}

//function goNewUrl() {
//    var newUrl = location.href;
//    var t = newUrl.indexOf("?t");
//    if (t > 0) {
//        newUrl = newUrl.substr(0, t + 3) + Math.random();
//    }
//    else {
//        if (newUrl.indexOf("?") > 0) {
//            newUrl = location.href;
//        }
//        else {
//            newUrl = location.href + "?t=" + Math.random();
//        }
//    }
//    location.href = newUrl;
//}

function ExceptSelect(v) {
    if (v == "12") {
        var id = $("#orderId").val();
        art.dialog.open("/Order/ExceptionReason/" + id + "?t=" + Math.random(), { title: '异常原因(The reason for the exception)', lock: true, width: 500, height: 280, close: function () {
            $("#OrderStatusId").val("");
        }
        });
    }
}

function OpwinByDeliveryPlan(trackno) {
    art.dialog.open("/Inventory/ChageDeliveryPlan?trackno=" + escape(trackno) + "&t=" + Math.random(), { title: '发货计划 - ' + trackno, lock: true, width: 600, height: 350, close: function () { } });
}

function OpwinByDeliveryPlanNew(trackno) {
    art.dialog.open("/Inventory/ChageDeliveryPlanNew?trackno=" + escape(trackno) + "&t=" + Math.random(), { title: '发货计划 - ' + trackno, lock: true, width: 600, height: 350, close: function () { } });
}

function OpwinByLogistics(trackno,type) {
    art.dialog.open("/Tracking/Tracking?TrackNum=" + escape(trackno) + "&TrackType=" + type + "&t=" + Math.random(), { title: 'Logistics - ' + trackno, lock: true, width: 650, height: 500, close: function () { } });
}

function ChageOriginaOrder(id,idlist) {
    art.dialog.open("/Order/OriginalOrderUpdate?id=" + id + "&idlist=" + idlist + "&t=" + Math.random(), { title: '', lock: true, width: 1000, height: 550, close: function () { } });
}

function SelectAdminInfo(UserName) {

    var UserName = UserName.toString();
    art.dialog.open("/Role/AdminInfo?UserName="+UserName+"&t=" + Math.random(), { title: '用户信息', lock: true, width: 430, height: 250, close: function () {
        
    }
    });
}

function GetSubject(id,trueName) {
    var id = id.toString();
    var Name = trueName.toString();
    art.dialog.open("/Assess/AssessAddInfo?id=" + id + "&t=" + Math.random(), { title: '进行考核 - '+Name, lock: true, width: 800, height: 550, close: function () {
        //window.location.href = "/Assess/AssessAdd?t=" + Math.random();
        GetAdminInfo();
    }
    });
}

function GetHistory(id, AssessUser, AssessTime, trueName) {
    var AssessResultId = id.toString();
    var adminName = AssessUser.toString();
    var Time = AssessTime.toString();
    var Name = trueName.toString();
    art.dialog.open("/Assess/AssessHistory?id=" + AssessResultId + "&AssessUser=" + adminName + "&AssessTime=" + Time + "&t=" + Math.random(), { title: '查看历史考核 - ' + Name, lock: true, width: 800, height: 550, close: function () {
    }
    });
}

function GetUpdate(id, AssessUser, AssessTime, trueName) {
    var AssessResultId = id.toString();
    var adminName = AssessUser.toString();
    var Time = AssessTime.toString();
    var Name = trueName.toString();
    art.dialog.open("/Assess/AssessUpdate?id=" + AssessResultId + "&AssessUser=" + adminName + "&AssessTime=" + Time + "&t=" + Math.random(), { title: '修改考核记录 - ' + Name, lock: true, width: 800, height: 550, close: function () {
        window.location.href = "/Assess/AssessAdminIndex?t=" + Math.random();
    }
    });
}

function GetReport(AssessUser, trueName) {
    var adminName = AssessUser.toString();
    var Name = trueName.toString();
    art.dialog.open("/Assess/PersonReport?adminName=" + adminName + "&t=" + Math.random(), { title: '历史考核情况 - ' + Name, lock: true, width: 800, height: 550, close: function () {
        
    }
    });
}

function GetPersonHistory(AssessUser, trueName) {
    var User = AssessUser.toString();
    var Name = trueName.toString();
    art.dialog.open("/Assess/PersonHistoryAdmin?adminName=" + AssessUser + "&t=" + Math.random(), { title: '个人考核情况 - ' + Name, lock: true, width: 800, height: 550, close: function () {
    }
    });
}

function ShowPicture(Id, TitleId, title) {
    art.dialog.open("/MessageManage/ShowPicture?Id=" + Id + "&TitleId=" + TitleId + "&t=" + Math.random(), { title: title, lock: true, width: 800, height: 550, close: function () {
    }
    });
}

function tabClick() {
    if ($(this).hasClass('activeTab'))
        return;
    $('.hd ul li').removeClass('activeTab');
    $(this).addClass('activeTab');
    var tabId = $(this).attr('tabId');
    $('#content > div').hide();
    $('#' + tabId).show();
}
function checkAll(obj) {
    if (obj.checked) {
        $("input[name=selectSub]").each(function (i, item) {
            if (!item.disabled)
                item.checked = true;
        });
    }
    else {
        $("[name=selectSub]").each(function (i, item) {
            item.checked = false;
        });
    }
}

var tr = "<tr><td colspan=\"15\" valign=\"middle\" style=\"height: 300px; text-align: center; background-color: #CCCCCC;color:#535353;\">";
//只能输入数字(不包含组合键)
function onlyNum() {
    if (event.shiftKey) event.returnValue = false;
    if (!(event.keyCode == 46) && !(event.keyCode == 8) && !(event.keyCode == 37) && !(event.keyCode == 39))
        if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)))
            event.returnValue = false;
}
//只能输入数字小数点(不包含组合键，未做小数点限定)
function onlyNumPrice() {
    if (event.shiftKey) event.returnValue = false;
    if (!(event.keyCode == 46) && !(event.keyCode == 8) && !(event.keyCode == 37) && !(event.keyCode == 39))
        if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105 || event.keyCode == 110 || event.keyCode == 190)))
            event.returnValue = false;
} 
//弹出窗体
function OpenUrlWindow(Url) {
    window.open(Url, "", "left=0,top=0,width=" + (screen.availWidth - 10) + ",height=" + (screen.availHeight - 50) + ",location=no,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,status=no");
}
