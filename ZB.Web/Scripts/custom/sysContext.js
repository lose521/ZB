
/*
 多语言
*/

// 翻译存储对象
var _translationStore = {
    cn: {},
    en: {}
}

// 翻译的json文件
var _translationStringFiles = [];

// 添加 string 翻译
function addString(stringCode, stringCN, stringEN) {
    if (typeof _translationStore.cn[stringCode] == 'undefined')
        _translationStore.cn[stringCode] = stringCN;
    if (typeof _translationStore.en[stringCode] == 'undefined')
        _translationStore.en[stringCode] = stringEN;
}
// 修改 string 翻译
function updateString(stringCode, stringCN, stringEN) {
    _translationStore.cn[stringCode] = stringCN;
    _translationStore.en[stringCode] = stringEN;
}

// 添加 string 翻译json文件
function addStringFile(stringFile) {
    _translationStringFiles.push(stringFile);
}

function getString(stringCode, language) {
    if (language == null)
        language = getSessionData('currentLanguage') == 'en' ? 'en' : 'cn';
    var translations;
    if (language === 'cn')
        translations = _translationStore.cn;
    else
        translations = _translationStore.en;
    if (translations[stringCode])
        return translations[stringCode]
    else
        return stringCode;
}

var stringLoaderFactory = function ($q, $http, $timeout) {

    return function (options) {
        var deferred = $q.defer(),
            translations;

        if (typeof addTranslation == 'function') {
            addTranslation(options.key);
            //var translation = addTranslation();
            //angular.extend(_translationStore.cn, translation.cn);
            //angular.extend(_translationStore.en, translation.en);
        }

        if (options.key === 'cn')
            translations = _translationStore.cn;
        else
            translations = _translationStore.en;

        // 读ifca.framework.{lang}.json
        var frameworkStringFile = ifcaFrameworkLibPath() + 'js/string/ifca.framework.' + options.key + '.json';
        _translationStringFiles.push(frameworkStringFile);

        var promises = [];
        angular.forEach(_translationStringFiles, function (stringFile) {
            promises.push($http.get(stringFile)
            )
        });

        $q.all(promises).then(function (datas) {
            angular.forEach(datas, function (data) {
                angular.extend(translations, data.data);
            })


            deferred.resolve(translations);
        }, function (err) {
            console.log("$q.all: ", err);
        })


        return deferred.promise;
    };
};


var getStringFilter = function ($parse, $translate) {
    var translateFilter = function (translationId, forceLanguage, interpolateParams, interpolation) {
        if (typeof translationId == 'undefined' || translationId == null)
            return '';
        if (!angular.isObject(interpolateParams)) {
            interpolateParams = $parse(interpolateParams)(this);
        }
        translationId = translationId + "";
        var str = $translate.instant(translationId, interpolateParams, interpolation, forceLanguage);
        if (str == '' || str == translationId) {
            var currentLanguage = getCurrentLanguage();
            if (currentLanguage == 'en')// 防止angular未加载时导致显示stringcode
            {
                var transStr = _translationStore.en[translationId];
                if (typeof transStr == 'string')
                    return transStr;
            }
            else {
                var transStr = _translationStore.cn[translationId];
                if (typeof transStr == 'string')
                    return transStr;
            }
        }
        return str;
    };

    if ($translate.statefulFilter()) {
        translateFilter.$stateful = true;
    }

    return translateFilter;
}

var getCurrentLoginName = function () {
    var loginInfo = getCurrentLoginInfo();
    if (loginInfo && loginInfo.LoginName)
        return loginInfo.LoginName;
    return null;
}
var getCurrentUserId = function () {
    var loginInfo = getCurrentLoginInfo();
    if (loginInfo && loginInfo.UserId)
        return loginInfo.UserId;
    return null;
}


var getCurrentLoginInfo = function () {
    // 压力测试
    //return { LoginName: 'zhaobin4', UserId: 1, AppId: 'StressTest_zhaobin4', ApiKey: 'StressTest', ExpireTime: new Date(2033, 1, 1), HtmlVersion: new Date().getTime() };

    try {
        //var encryptedText = getSessionData('currentLoginInfo');
        //if (isNullOrEmpty(encryptedText))
        //    return null;
        //var encryptedText = decodeURI(encryptedText);
        //var jsonString = aesDecrypt(encryptedText, 'ifca1234');

        var encryptedText = getSessionData('currentLoginInfo');
        if (isNullOrEmpty(encryptedText) || encryptedText == "null")
            return null;
        var encryptedText = decodeURI(encryptedText);
        var jsonString = Base64.decode(encryptedText);

        //var jsonString = getSessionData('currentLoginInfo');
        if (isNullOrEmpty(jsonString))
            return null;

        var info = JSON.parse(jsonString);
        if (typeof info == 'object')
            return info;
        return null;
    }
    catch (e) {
        console.log(e);
        return null;
    }
}

var clearCurrentLoginInfo = function () {
    setSessionData('currentLoginInfo', null);
}

// sysContext
// sysContext存储对象
var _sysContextStore = {
    currentLanguage: '',
    cn: {},
    en: {}
}

var getCurrentLanguage = function () {
    return _sysContextStore.currentLanguage;
}

var sysContextProvider = function () {
    //this.$get = function ($translate, $window) {
    //    this.$translate = $translate;
    //    this.$window = $window;
    //    return this;
    //};
    this.$get = function () {

        return this;
    };
    this.initialize = function (options) {
        if (options && options.language)
            _sysContextStore.currentLanguage = options.language;
        var cn = _sysContextStore.cn;
        if (!cn.language) {
            cn.language = 'cn';
            cn.dateFormat = 'yyyy-MM-dd';
            cn.dateTimeFormat = 'yyyy-MM-dd HH:mm';
            cn.timeFormat = 'HH:mm:ss';
        }

        var en = _sysContextStore.en;
        if (!en.language) {
            en.language = 'en';
            en.dateFormat = 'MM-dd-yyyy';
            en.dateTimeFormat = 'MM-dd-yyyy HH:mm';
            en.timeFormat = 'HH:mm:ss';
        }


    }
    this.currentLanguage = function (lang) {
        //if (!this.$cookie) {
        //    angular.injector(['ngCookies']).invoke(['$cookies', function (_$cookies_) {
        //        this.$cookies = _$cookies_;
        //    }]);
        //}

        if (!lang) {
            if (_sysContextStore.currentLanguage != '')
                return _sysContextStore.currentLanguage;
            var currentLanguage = getSessionData('currentLanguage') == 'en' ? 'en' : 'cn';
            _sysContextStore.currentLanguage = currentLanguage;
            return _sysContextStore.currentLanguage;
        }
        _sysContextStore.currentLanguage = lang;
        setSessionData('currentLanguage', lang);
        this.$translate.use(lang);
    }
    this.currentLoginInfo = function (loginInfo) {
        if (!loginInfo) {
            return getCurrentLoginInfo();
        }
        var jsonString = JSON.stringify(loginInfo);
        //var encryptedText = aesEncrypt(jsonString, 'ifca1234');
        //encryptedText = encodeURI(encryptedText);
        //setSessionData('currentLoginInfo', encryptedText);

        var encryptedText = Base64.encode(jsonString);
        encryptedText = encodeURI(encryptedText);
        setSessionData('currentLoginInfo', encryptedText);

        //setSessionData('currentLoginInfo', jsonString);
        if (loginInfo.HtmlVersion)
            setHtmlVersion(loginInfo.HtmlVersion)

    }
    this.clearCurrentLoginInfo = function () {
        setSessionData('currentLoginInfo', null)
    }

    this.currentLoginName = function () {
        var loginInfo = this.currentLoginInfo();
        if (loginInfo == null)
            return null;
        return loginInfo.LoginName;
    }
    this.currentContext = function () {
        var lang = this.currentLanguage();
        return _sysContextStore[lang];
    }
    this.dateFormat = function (format) {
        var context = this.currentContext();
        if (!format)
            return context.dateFormat;
        context.dateFormat = format;
    }
    this.dateTimeFormat = function (format) {
        var context = this.currentContext();
        if (!format)
            return context.dateTimeFormat;
        context.dateTimeFormat = format;
    }
    this.timeFormat = function (format) {
        var context = this.currentContext();
        if (!format)
            return context.timeFormat;
        context.timeFormat = format;
    }
   
};


