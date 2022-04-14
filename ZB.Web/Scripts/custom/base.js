var ng = angular.module('base', []);
if (typeof listDirective == 'function')
    ng.directive("list", listDirective);


if (typeof sysContextProvider == 'function')
    ng.provider('$sysContext', sysContextProvider);

//rds.config(function ($locationProvider) {
//    // 设置api根目录
//    //RestangularProvider.setBaseUrl(webApiRootPath());
//    $locationProvider.html5Mode({
//        enabled: true,
//        requireBase: false//必须配置为false，否则<base href=''>这种格式带base链接的地址才能解析
//    });
//});


ng.config(function ($httpProvider) {//RestangularProvider, 
    // 初始化Restangular 设置api根目录  
    //RestangularProvider.setBaseUrl(webApiRootPath());
    // 设置可获取http状态
    //RestangularProvider.setFullResponse(true);

    // 拦截请求
    $httpProvider.interceptors.push('authHttpInterceptor');

    $.ajaxSetup({
        beforeSend: function (xhr, settings) {
            var content = settings.data ? JSON.stringify(settings.data) : '';
            var httpMethod = settings.type;
            var authorization = getHmacAuthorization(httpMethod, content);

            xhr.setRequestHeader('Authorization', authorization);
            xhr.setRequestHeader('CurrentLanguage', getCurrentLanguage());
        }
    });
});
ng.factory('authHttpInterceptor', function () {
    return {
        request: function (config) {
            var content = config.data ? JSON.stringify(config.data) : '';
            var httpMethod = config.method;
            var authorization = getHmacAuthorization(httpMethod, content);

            config.headers.Authorization = authorization;
            config.headers.CurrentLanguage = getCurrentLanguage();
            return config;
        }
    };
});

function getHmacAuthorization(httpMethod, content) {
    var schemaName = 'ZB ';

    var loginInfo = getCurrentLoginInfo();
    if (loginInfo == null || !loginInfo.AppId)
        return '';

    var appId = loginInfo.AppId;
    var apiKey = loginInfo.ApiKey;
    var loginName = Base64.encode(loginInfo.LoginName);
    var timestamp = new Date().getTime() / 1000 | 0;

    var contentMd5 = '';
    if (httpMethod == 'GET')
        contentMd5 = '';
    else
        contentMd5 = md5(content);
    var hmacText = '{0}{1}{2}{3}'.format(appId, httpMethod, timestamp, contentMd5);
    console.info('content', content)
    console.info('contentMd5', contentMd5)
    //var b = Base64.encode('apiKey');
    //var hmacObj1 = new jsSHA('SHA-256', 'TEXT');
    //hmacObj1.setHMACKey('apiKey', 'B64');
    //var dd=hmacObj1.update('hmacText');
    //var hmacOutput1 = hmacObj1.getHMAC('B64');


    // calc hmac
    var hmacObj = new jsSHA('SHA-256', 'TEXT');
    hmacObj.setHMACKey(apiKey,'TEXT');//hmacObj.setHMACKey(apiKey, 'B64');
    hmacObj.update(hmacText);
    var hmacOutput = hmacObj.getHMAC('B64');
    var base64sign = hmacOutput;
    
    var authorization = '{0} {1}:{2}:{3}'.format(schemaName, appId, base64sign, timestamp);

    return authorization;
}
function getSessionData(key) {
    if (typeof $.cookie == 'undefined')
        alert('未引用 jquery.component.combine.js');
    return $.cookie(key);
}

function setSessionData(key, value, timeoutSeconds, path) {
    if (typeof $.cookie == 'undefined')
        alert('未引用 jquery.component.combine.js');

    if (path == null)
        path = '/';
    if (timeoutSeconds == null)
        $.cookie(key, value, { path: path });
    else {
        var cookieTime = new Date();
        cookieTime.setTime(cookieTime.getTime() + (timeoutSeconds * 1000)); // 秒数

        $.cookie(key, value, { path: path, expires: cookieTime });
    }
}

function clearSessionData(key, path) {
    if (typeof $.cookie == 'undefined')
        alert('未引用 jquery.component.combine.js');
    if (path == null)
        path = '/';

    $.removeCookie(key, { path: path });
}