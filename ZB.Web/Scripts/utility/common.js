function getParamURL(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = location.search.substr(1).match(reg);
    if (r != null) return unescape(decodeURI(r[2])); return null;
}

String.prototype.format = function () {
    /// <summary> 类似C#的string.format </summary>
    var args = arguments;
    return this.replace(/\{(\d+)\}/g,
function (m, i) {
    return args[i];
});
}


function getHtmlVersion()
{
    return 1;
}

//-------------------------层
/* 
* url 目标url 
* arg 需要替换的参数名称 
* arg_val 替换后的参数的值 
* return url 参数替换后的url 
*/
function changeUrlArg(url, arg, arg_val) {
    /// <summary> 修改url中某个指定的参数的值 </summary>
    var pattern = arg + '=([^&]*)';
    var replaceText = arg + '=' + arg_val;
    if (url.match(pattern)) {
        var tmp = '/(' + arg + '=)([^&]*)/gi';
        tmp = url.replace(eval(tmp), replaceText);
        return tmp;
    } else {
        if (url.match('[\?]')) {
            return url + '&' + replaceText;
        } else {
            return url + '?' + replaceText;
        }
    }
    return url + '\n' + arg + '\n' + arg_val;
}
var _dropDownFrameCount = 0;
var _messageParentElement = 'body';
function dropDownFrame(options) {
    /// <summary> 弹出层 </summary>
    _dropDownFrameCount += 1;

    var frame = $('#iframeDropDownContent');
    if (frame.length > 0) {
        frame[0].contentWindow.document.write('');
        frame[0].contentWindow.close();
        frame.attr('src', "about:blank");
        frame.remove();
    }

    var url = changeUrlArg(options.src, 'ver', getHtmlVersion());
    var frameId = 'frame' + _dropDownFrameCount;
    var message = "<iframe id='iframeDropDownContent' frameborder='0' scrolling='no' style='height:" + options.height + "px;width:" + options.width + "px'></iframe>";
    var html = '<div id="{0}" class="dropDownFrame"><a class="dropDownFrameCloseButton" onclick="dropDownFrameClose()"></a>{1}</div>'.format(frameId, message);
    $(_messageParentElement).append(html);

    var closeButtonWidth = options.width - 10;
    $('.dropDownFrameCloseButton').css('top', 0);
    $('.dropDownFrameCloseButton').css('left', closeButtonWidth - 28);
    $('.dropDownFrameCloseButton').css('z-index', 999999);
    $('.dropDownFrameCloseButton').show();

    //$('#iframeDropDownContent').attr("src", 'about:blank');

    $('#' + frameId).igDialog({
        state: "closed",
        showHeader: false,
        modal: true,
        draggable: false,
        resizable: false,
        height: options.height,
        width: options.width,
        stateChanging: function (evt, ui) {
            if (ui.action == "close" && $.isFunction(options.close)) {
                return options.close(evt, ui.owner);
            }
        }
    });
    $('#' + frameId).igDialog("open");

    setTimeout(function () {
        $('#iframeDropDownContent').attr("src", url);
    }, 10)

}

function dropDownFrameClose() {
    /// <summary> 关闭弹出层 </summary>

    //$('.dropDownFrame').hide();
    //try {
    $('#frame' + _dropDownFrameCount).igDialog('close')
    //$('#frame' + _dropDownFrameCount).igDialog('destroy')
    //$('#frame' + _dropDownFrameCount).igDialog('destroy')
    //setTimeout(function () {
    //  $('.dropDownFrame').remove();
    //}, 10)
    //}
    //catch (e) { }
}


//-------------------------------------end 层