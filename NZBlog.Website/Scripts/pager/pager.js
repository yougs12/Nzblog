﻿(function ($) {
    $.fn.pager = function (options) {
        var opts = $.extend({}, $.fn.pager.defaults, options);
        return this.each(function () {
            $(this).empty().append(renderpager(parseInt(options.pagenumber), parseInt(options.pagecount), options.buttonClickCallback));
            $('.pages li').mouseover(function () { document.body.style.cursor = "pointer"; }).mouseout(function () { document.body.style.cursor = "auto"; });
        });
    };

    function renderpager(pagenumber, pagecount, buttonClickCallback) {
        var $pager = $('<ul class="pages"></ul>');
        $pager.append(renderButton('首页', pagenumber, pagecount, buttonClickCallback)).append(renderButton('上一页', pagenumber, pagecount, buttonClickCallback));
        var startPoint = 1;
        var endPoint = 9;
        var thpoint = "<li class='thpoint'>...</li>";
        if (pagenumber > 4) {
            startPoint = pagenumber - 4;
            endPoint = pagenumber + 4;
        }
        if (endPoint > pagecount) {
            startPoint = pagecount - 8;
            endPoint = pagecount;
            thpoint = "";
        }
        if (startPoint < 1) {
            startPoint = 1;
        }
        for (var page = startPoint; page <= endPoint; page++) {
            var currentButton = $('<li class="page-number">' + (page) + '</li>');
            page == pagenumber ? currentButton.addClass('pgCurrent') : currentButton.click(function () {
                buttonClickCallback(this.firstChild.data);
            });
            currentButton.appendTo($pager);
        }
        $pager.append(thpoint).append(renderButton('下一页', pagenumber, pagecount, buttonClickCallback)).append(renderButton('末页', pagenumber, pagecount, buttonClickCallback));
        $pager.append("<li class='thpoint' style='margin-top:3px;'>共: " + pagecount + " 页</li>");
        var strgoto = $("<li class='thpoint'>当前<input type='text' value='" + pagenumber + "'maxlength='6' id='gotoval' style='width:20px; height:16px;margin-top:-3px;padding-top:2px;padding-left:10px;'/>页</li>");
        //$pager.append(strgoto);
        //$pager.append(changepage('go', pagecount, buttonClickCallback));
        return $pager;
    }
    function changepage(buttonLabel, pagecount, buttonClickCallback) {
        var $btngoto = $('<li class="pgNext">' + buttonLabel + '</li>');
        $btngoto.click(function () {
            var gotoval = $('#gotoval').val();
            var patrn = /^[1-9]{1,20}$/;
            if (!patrn.exec(gotoval)) {
                alert("请输入非零的正整数！");
                return false;
            }
            var intval = parseInt(gotoval);
            if (intval > pagecount) {
                alert("您输入的页面超过总页数 " + pagecount);
                return;
            }
            buttonClickCallback(intval);
        });
        return $btngoto;
    }

    function renderButton(buttonLabel, pagenumber, pagecount, buttonClickCallback) {
        var $Button = $('<li class="pgNext">' + buttonLabel + '</li>');
        var destPage = 1;
        switch (buttonLabel) {
            case "首页":
                destPage = 1;
                break;
            case "上一页":
                destPage = pagenumber - 1;
                break;
            case "下一页":
                destPage = pagenumber + 1;
                break;
            case "末页":
                destPage = pagecount;
                break;
        }
        if (buttonLabel == "首页" || buttonLabel == "上一页") {
            pagenumber <= 1 ? $Button.addClass('pgEmpty') : $Button.click(function () { buttonClickCallback(destPage); });
        }
        else {
            pagenumber >= pagecount ? $Button.addClass('pgEmpty') : $Button.click(function () { buttonClickCallback(destPage); });
        }
        return $Button;
    }

    $.fn.pager.defaults = {
        pagenumber: 1,
        pagecount: 1
    };
})(jQuery);