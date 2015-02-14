function okshow(alertContent) {
    if (!alertContent) alertContent = '操作成功！';
    //myAlert('succeed', '', alertContent, Callback, closeCallback);
    art.dialog.tips('succeed', alertContent);
}
function exshow(alertContent, Callback, closeCallback) {
    if (!alertContent) alertContent = '操作失败！';
    myAlert('error', '错误提示', alertContent, Callback, closeCallback);
}
function warnshow(alertContent, Callback, closeCallback) {
    if (!alertContent) alertContent = '操作有误！';
    myAlert('warning', '系统提示', alertContent, Callback, closeCallback);
}
function loadingshow(alertContent) {
    if (!alertContent) alertContent = '请稍后，数据正在处理...';
    art.dialog.tips('loading', alertContent);
}
function myAlert(sicon, stitle, scontent, callback, closeback) {
    if (stitle == undefined || stitle == "") stitle = "";
    var content = "<div style=\"width:380px;overflow:hidden;overflow-y:auto;white-space:normal;word-break:break-all;\">" + scontent + "</div>";
    art.dialog({
        width: 400,
        height: 150,
        id: 'artmsg',
        padding: 0,
        title: stitle,
        content: content,
        icon: sicon,
        lock: true,
        ok: function () { if (callback != undefined) { callback(); } },
        close: function () { if (closeback != undefined) { closeback(); } }
    });
}
artDialog.tips = function (sicon, content) {
    var imgLoad = '';
    if (sicon == 'loading') { sicon = ''; imgLoad = '<img src="/Images/loading.gif"/>'; }
    var artTips = artDialog({
        id: 'artmsg',
        title: false,
        cancel: false,
        fixed: true,
        icon: sicon,
        lock: true,
    })
    .content('<div style="padding: 0 1em;">' + imgLoad + ' ' + content + '</div>')
    if (sicon != '') artTips.time(1);
    return artTips;
};

function openNew(url, title, width, height, closeCallback) {
    if (!width) width = 800;
    if (!height) height = 500;
    if (!closeCallback) closeCallback = function () { };
    if (url.indexOf('?') < 0) { url += '?' + Math.random(); } else { url += '&' + Math.random(); }
    art.dialog.open(url, { title: title, lock: true, width: width, height: height, close: closeCallback });
}