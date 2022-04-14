function isNullOrEmpty(val) {
    /// <summary> 是否为空 </summary>
    if (typeof val == 'undefined')
        return true;
    if (val === null || val === '')
        return true;
    if (toStr(val).length == 0)
        return true;
    if (toStr(val).trim().length == 0)
        return true;
    return false;
}


function isNull(val) {
    /// <summary> 是否为null </summary>
    return val == null
}




String.prototype.format = function () {
    /// <summary> 类似C#的string.format </summary>
    var args = arguments;
    return this.replace(/\{(\d+)\}/g,
function (m, i) {
    return args[i];
});
}

String.prototype.formatEncode = function () {
    /// <summary> 类似C#的string.format并htmlEncode </summary>
    var args = arguments;
    return this.replace(/\{(\d+)\}/g,
function (m, i) {
    return htmlEncode(args[i]);
});
}

function newGuid() {
    /// <summary> 生成guid </summary>
    var S4 = function () {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    };
    return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
}

function getUrlParam() {
    /// <summary> 获取url参数 </summary>
    var url, param;
    if (arguments.length == 1) {
        url = location.href;
        param = arguments[0];
    } else {
        url = arguments[0];
        param = arguments[1];
    }
    var re = new RegExp("(\\\?|&)" + param + "=([^&]+)(&|$)", "i"); var m = url.match(re); if (m)
        return m[2]; else
        return '';
}
function getPageName() {
    /// <summary> 获取页面名称 </summary>
    var a = location.href;
    var b = a.split("/");
    var c = b.slice(b.length - 1, b.length).toString(String).split(".");
    return c.slice(0, 1)[0];
}
function formatMoney(v, len) {
    /// <summary>格式化金额 </summary>

    var split = split || ",", len = Math.abs((+len) % 20 || 2);
    v = parseFloat((v + "").replace(/[^\d\.-]/g, "")).toFixed(len) + "";

    var data = v.replace(/\d+/, function (v) {
        var lit = v.length % 3 == 0;
        var index = lit ? v.length - 3 : -1;
        return v.split('').reverse().join('').replace(/\d{3}/g, function (k, l) {
            return k + ((l == index && lit) ? "" : split);
        }).split('').reverse().join('')
    }
     );
    return data;
}

function formatInt(s) {
    /// <summary>格式化整数</summary>
    var n = 0;
    s = parseFloat((s + "").replace(/[^\d\.-]/g, "")).toFixed(n) + "";
    var l = s.split(".")[0].split("").reverse(),
    r = s.split(".")[1];
    t = "";
    for (i = 0; i < l.length; i++) {
        t += l[i] + ((i + 1) % 3 == 0 && (i + 1) != l.length ? "," : "");
    }
    return t.split("").reverse().join("");
}

function fomatDatetime(x, y) {
    var z = { M: x.getMonth() + 1, d: x.getDate(), h: x.getHours(), m: x.getMinutes(), s: x.getSeconds() };
    y = y.replace(/(M+|d+|h+|m+|s+)/g, function (v) { return ((v.length > 1 ? "0" : "") + eval('z.' + v.slice(-1))).slice(-2) });
    return y.replace(/(y+)/g, function (v) { return x.getFullYear().toString().slice(-v.length) });
}

function htmlEncode(str) {
    /// <summary> xml转义编码 </summary>
    return str
        .replace(/&/g, '&amp;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
}

function htmlDecode(str) {
    /// <summary> xml转义解码 </summary>
    return str
        .replace(/&quot;/g, '"')
        .replace(/&#39;/g, "'")
        .replace(/&lt;/g, '<')
        .replace(/&gt;/g, '>')
        .replace(/&amp;/g, '&');
}





function loadScriptSync(src) {
    /// <summary> 动态同步方式添加js文件 </summary>

    var script = loadFileSync(src);
    var se = document.createElement('script');
    se.type = "text/javascript";
    se.text = script;
    document.getElementsByTagName('head')[0].appendChild(se);
}
function loadFileSync(url) {
    /// <summary> 同步方式获取文件 </summary>

    // get some kind of XMLHttpRequest
    var xhrObj = XMLHttpRequest();
    // open and send a synchronous request
    xhrObj.open('GET', url, false);
    xhrObj.send('');
    return xhrObj.responseText;
}

function loadScript(url, callback) {
    /// <summary> 动态加载js脚本文件 </summary>
    var script = document.createElement("script");
    script.type = "text/javascript";
    if (callback != null) {
        if (script.readyState) { // IE
            script.onreadystatechange = function () {
                if (script.readyState == "loaded" || script.readyState == "complete") {
                    script.onreadystatechange = null;
                    callback();
                }
            };
        } else { // FF, Chrome, Opera, ...
            script.onload = function () {
                callback();
            };
        }
    }
    script.src = url;
    document.getElementsByTagName("head")[0].appendChild(script);
}

function loadScriptText(code) {
    /// <summary> 动态加载js脚本 </summary>

    var script = document.createElement("script");
    script.type = "text/javascript";
    try {
        // firefox、safari、chrome和Opera
        script.appendChild(document.createTextNode(code));
    } catch (ex) {
        // IE早期的浏览器 ,需要使用script的text属性来指定javascript代码。
        script.text = code;
    }
    document.body.appendChild(script);
}
function loadCss(url) {
    /// <summary> 动态加载css文件 </summary>

    var link = document.createElement("link");
    link.type = "text/css";
    link.rel = "stylesheet";
    link.href = url;
    document.getElementsByTagName("head")[0].appendChild(link);
}

function loadCssText(cssText) {
    /// <summary> 动态加载css脚本 </summary>

    var style = document.createElement("style");
    style.type = "text/css";
    try {
        // firefox、safari、chrome和Opera
        style.appendChild(document.createTextNode(cssText));
    } catch (ex) {
        // IE早期的浏览器 ,需要使用style元素的stylesheet属性的cssText属性
        style.styleSheet.cssText = cssText;
    }
    document.getElementsByTagName("head")[0].appendChild(style);
}


function isHTML(str) {
    /// <summary> 检查文本中是否有html元素，简单判断 </summary>
    return /^<.*?>$/.test(str) && !!$(str)[0];
}


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

var formatDate = function (formatDate, formatString) {
    if (formatDate instanceof Date) {
        var months = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
        var yyyy = formatDate.getFullYear();
        var yy = yyyy.toString().substring(2);
        var m = formatDate.getMonth() + 1;
        var mm = m < 10 ? "0" + m : m;
        var mmm = months[m];
        var d = formatDate.getDate();
        var dd = d < 10 ? "0" + d : d;

        var h = formatDate.getHours();
        var hh = h < 10 ? "0" + h : h;
        var n = formatDate.getMinutes();
        var nn = n < 10 ? "0" + n : n;
        var s = formatDate.getSeconds();
        var ss = s < 10 ? "0" + s : s;

        formatString = formatString.replace(/yyyy/i, yyyy);
        formatString = formatString.replace(/yy/i, yy);
        formatString = formatString.replace(/mmm/i, mmm);
        formatString = formatString.replace(/mm/i, mm);
        formatString = formatString.replace(/m/i, m);
        formatString = formatString.replace(/dd/i, dd);
        formatString = formatString.replace(/d/i, d);
        formatString = formatString.replace(/hh/i, hh);
        formatString = formatString.replace(/h/i, h);
        formatString = formatString.replace(/nn/i, nn);
        formatString = formatString.replace(/n/i, n);
        formatString = formatString.replace(/ss/i, ss);
        formatString = formatString.replace(/s/i, s);

        return formatString;
    } else {
        return "";
    }
}

var getAbsolutePath = function (base, relative) {
    /// <summary> 根据网页的相当路径获取绝对路径 </summary>
    /// <param name="base" type="string">基本路径，一般为网页的当前路径</param>
    /// <param name="relative" type="string">相对路径</param>
    var stack = base.split("/"),
        parts = relative.split("/");
    stack.pop(); // remove current file name (or empty string)
    // (omit if "base" is the current folder without trailing slash)
    for (var i = 0; i < parts.length; i++) {
        if (parts[i] == ".")
            continue;
        if (parts[i] == "..")
            stack.pop();
        else
            stack.push(parts[i]);
    }
    return stack.join("/");
}

String.prototype.replaceAll = function (s1, s2) {
    /// <summary> 替换所有字符 </summary>
    var r = new RegExp(s1.replace(/([\(\)\[\]\{\}\^\$\+\-\*\?\.\"\'\|\/\\])/g, "\\$1"), "ig");
    return this.replace(r, s2);
}
String.prototype.trim = function () {
    /// <summary> 去除前后空格 </summary>
    return this.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
}
var toFloat = function (v) {
    /// <summary> 转换为浮点数，如果无法转换，则返回0 </summary>
    if (v == null)
        return 0.0;
    if (typeof (v) == 'number')
        return v;
    var result = parseFloat(v.replaceAll(',', ''));
    if (isNaN(result))
        return 0.0;
    return result;
}
var toFloatWithDigits = function (value, decimalPlace) {
    /// <summary> 转换为浮点数，带小数点 </summary>
    return toFloat(toFloat(value).toFixed(decimalPlace));
}

var formatFloat = function (value, decimalPlace) {
    /// <summary> 格式化数字，带小数点 </summary>
    if (decimalPlace == null)
        decimalPlace = 0;
    return toFloat(value).toFixed(decimalPlace);
}

var toInt = function (v) {
    /// <summary> 转换为整数，如果无法转换，则返回0 </summary>
    return parseInt(toFloat(v));
}

var toStr = function (v) {
    /// <summary> 转换为字符串，如果无法转换，则返回空字符串'' </summary>
    if (v == null)
        return '';
    return v + '';
}

var toDate = function (v) {
    /// <summary> 转换为日期，如果无法转换，则返回null </summary>
    if (v == null)
        return null;
    if (v instanceof Date)
        return v;
    if (typeof v == 'string') {
        var converted = Date.parse(v);
        return new Date(converted);
    }
    return null;
}

var _defaultPassword = "WoAb5pzb0bLs0p70Tt3Ulcea";
var _defaultIv = "DQ1xDQx6IqVXyVMP7OkzCNdF";
var formatPassword = function (password) {
    return (password + _defaultPassword).substr(0, 16);
}
var formatIv = function (iv) {
    return (iv + _defaultIv).substr(0, 16);
}

var aesEncrypt = function (text, key, iv) {
    /// <summary> AES加密 </summary>
    if (key == null)
        key = '';
    if (iv == null)
        iv = '';
    var key = CryptoJS.enc.Utf8.parse(formatPassword(key));
    var iv = CryptoJS.enc.Utf8.parse(formatIv(iv));
    //var cfg = { mode: CryptoJS.mode.CBC, iv: iv, padding: CryptoJS.pad.Pkcs7 }; 
    var cfg = { iv: iv };
    var encryptedData = CryptoJS.AES.encrypt(text, key, cfg);
    var encryptedText = CryptoJS.enc.Base64.stringify(encryptedData.ciphertext);
    return encryptedText;
}

var aesDecrypt = function (encryptedText, key, iv) {
    /// <summary> AES解密 </summary>
    if (key == null)
        key = '';
    if (iv == null)
        iv = '';
    var key = CryptoJS.enc.Utf8.parse(formatPassword(key));
    var iv = CryptoJS.enc.Utf8.parse(formatIv(iv));
    //var cfg = { mode: CryptoJS.mode.CBC, iv: iv, padding: CryptoJS.pad.Pkcs7 }; 
    var cfg = { iv: iv };
    var decryptedData = CryptoJS.AES.decrypt(encryptedText, key, cfg);
    var decryptedText = decryptedData.toString(CryptoJS.enc.Utf8)
    return decryptedText;
}

var isIE9OrBelow = function () {
    /// <summary> 判断是否为IE9或以下版本 </summary>
    if (navigator.appName == "Microsoft Internet Explorer" && parseInt(navigator.appVersion.split(";")[1].replace(/[ ]/g, "").replace("MSIE", "")) <= 9) {
        return true;
    }
    return false;
}

var copyTextToClipboard = function (copyText, callback) {
    /// <summary> 复制文本到剪贴板 </summary>
    if (typeof (window.clipboardData) == "undefined") {
        // 使用https://clipboardjs.com/组件
        var copyTo = function () {
            sysAlert('点击确定按钮把行信息到剪贴板', function () {
                $(document.body).append('<button class="clipboardTempButton"></button>');
                var clipboard = new Clipboard('.clipboardTempButton', {
                    text: function () {
                        return copyText;
                    }
                });
                $('.clipboardTempButton').click();
                $('.clipboardTempButton').remove();

                if (typeof callback == 'function')
                    callback();
            })
        }
        if (typeof Clipboard == 'undefined') {
            var src = webAppRootPath() + 'scripts/utility/clipboard.min.js';
            loadScript(src, copyTo)
        }
        else {
            copyTo();
        }
        return;
    }
    else {
        window.clipboardData.setData("Text", copyText);
        if (typeof callback == 'function')
            callback();
    }
}


Array.prototype.insert = function (index, item) {
    this.splice(index, 0, item);
};

// 判断浏览器为 IE
function isIE() { //ie?
    if (!!window.ActiveXObject || "ActiveXObject" in window)
        return true;
    else
        return false;
}
