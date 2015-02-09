function okshow(alertContent, Callback, closeCallback) {
    if (!alertContent) alertContent = '操作成功！';
    myAlert('succeed', '', alertContent, Callback, closeCallback);
}
function exshow(alertContent, Callback, closeCallback) {
    if (!alertContent) alertContent = '操作失败！';
    myAlert('error', '错误提示', alertContent, Callback, closeCallback);
}
function warnshow(alertContent, Callback, closeCallback) {
    if (!alertContent) alertContent = '操作有误！';
    myAlert('warning', '系统提示', alertContent, Callback, closeCallback);
}
function loadingshow(alertContent, Callback, closeCallback) {
    if (!alertContent) alertContent = '请稍后，数据正在处理...';
    myAlert('loading.gif', '等待提示', alertContent, Callback, closeCallback);
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
function openNew(url, title, width, height, closeCallback) {
    if (!width) width = 800;
    if (!height) height = 500;
    if (!closeCallback) closeCallback = function () { };
    if (url.indexOf('?') < 0) { url += '?' + Math.random(); } else { url += '&' + Math.random(); }
    art.dialog.open(url, { title: title, lock: true, width: width, height: height, close: closeCallback });
}