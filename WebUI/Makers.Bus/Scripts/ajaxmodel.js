'use strict';
var Ajax = Ajax || {};

Ajax.SyncGetPartialView = function (webMethodName, argument, modal) {
    //modal  => #id
    $.ajax({
        type: 'POST',
        url: webMethodName,
        data: argument,
        cache: false,
        async: false,
        beforeSend: function () {
            $("#" + modal + "").html("로딩중...");
        },
        success: function (data) {
            $("#" + modal + "").html(data);
        },
        error: function () { }
    });
}

Ajax.SyncPostService = function (webMethodName, argument) {
    var returnData;
    $.ajax({
        type: 'POST',
        dataType: "json",
        url: webMethodName,
        traditional: true,
        //contentType: "application/json",
        data: argument,
        async: false,
        beforeSend: function (xhr) {

        },
        success: function (result) {
            returnData = result;
        },
        error: function (response) {
            if (response != null && response.status != null && response.status != 200)
                alert(response.status + ' ' + response.statusText);
        }
    });

    return returnData;
}

Ajax.AsyncPostService = function (webMethodName, argument, callBackFunction) {

    $.ajax({
        type: 'POST',
        dataType: "json",
        url: webMethodName,
        data: argument,
        async: true,
        beforeSend: function (xhr) {

        },
        success: function (result) {
            eval(callBackFunction)(result);
        },
        error: function (response) {
            alert(response.status + ' ' + response.statusText);
        }
    });
}

Ajax.SyncGetService = function (webMethodName, argument) {

    var callUrl = webMethodName + "?" + argument;
    var returnData;
    $.ajax({
        type: 'GET',
        dataType: "json",
        url: callUrl,
        async: false,
        beforeSend: function (xhr) {

        },
        success: function (result) {
            returnData = result;
        },
        error: function (response) {
            //alert(response.status + ' ' + response.statusText);
        }
    });

    return returnData;
}

Ajax.AsyncGetService = function (webMethodName, argument, callBackFunction) {

    var callUrl = webMethodName + "?" + argument;
    $.ajax({
        type: 'GET',
        dataType: "json",
        url: callUrl,
        async: true,
        beforeSend: function (xhr) {

        },
        success: function (result) {
            eval(callBackFunction)(result);
        },
        error: function (response) {
            //alert(response.status + ' ' + response.statusText);
        }
    });
}


Ajax.AjaxResponseModelService = function (srcObj, targetUrl, params, onSuccess, onFailure, options) {
    if (targetUrl == null || targetUrl == '') return;

    var settings = {
        type: 'POST',
        async: true,
        contentType: 'application/json;charset=utf-8',
        dataType: 'json'
    };

    options = $.extend(true, {}, settings, options);

    $.ajax({
        url: targetUrl,
        data: JSON.stringify(params),
        type: options.type,
        async: options.async,
        contentType: options.contentType,
        dataType: options.dataType
    })
    .done(function (response) {
        if (typeof (response) != 'undefined') {
            if ($.isFunction(onSuccess)) {
                onSuccess.call(srcObj, response);
            }
        }
    })
    .fail(function (error) {
        var exception = null;
        if (error != null && typeof (error.responseText) != 'undefined') {
            try {
                exception = $.parseJSON(error.responseText);
                if (typeof (exception.ExceptionMessage) != "undefined" && $.trim(exception.ExceptionMessage) != '') {
                    alert(exception.ExceptionMessage);
                }
            } catch (ex) {
                //alert(ex.name); 
            }
        }
        if ($.isFunction(onFailure)) {
            onFailure.call(srcObj, exception);
        }
    });
}

Ajax.AjaxAsyncPostGetParialView = function (targetUrl, params, options, placeholder) {
    var settings = {
        type: 'POST',
        async: true,
        dataType: 'html'
    };

    options = $.extend(true, {}, settings, options);
    $.ajax({
        url: targetUrl,
        data: params,
        type: options.type,
        async: options.async,
        dataType: options.dataType,
        crossDomain: true,
        beforeSend: function () {
            //$(placeholder).addClass('loading');
        },
        success: function (data) {
            $(placeholder).html(data);
        },
        error: function (xhr) {
            if (xhr != null && typeof (xhr.responseText) != 'undefined') {
                try {
                    var exception = $.parseJSON(error.responseText);
                    alert(
                      "Server error" +
                      "\nClassName=" + exception.ExceptionClassName +
                      "\nMessage=" + exception.ExceptionMessage +
                      exception.ExceptionStackTrace);
                } catch (ex) {
                    alert("오류 메시지 해석 중 오류가 발생했습니다.(" + ex.name + ")");
                }
            }
        }
    });
}

Ajax.AjaxFormPostService = function (srcObj, targetForm, targetAction, onSuccess, onFailure) {
    var options = {
        dataType: "json",
        beforeSend: function () {
        },
        success: function (response) {
            if (typeof (response) != 'undefined') {
                if ($.isFunction(onSuccess)) {
                    onSuccess.call(srcObj, response);
                }
            }
        },
        complete: function (response) {
        },
        error: function (error) {
            var exception = null;
            if (error != null && typeof (error.responseText) != 'undefined') {
                try {
                    exception = $.parseJSON(error.responseText);
                    if (typeof (exception.ExceptionMessage) != "undefined" && $.trim(exception.ExceptionMessage) != '') {
                        alert(exception.ExceptionMessage);
                    }
                } catch (ex) {
                }
            }
            if ($.isFunction(onFailure)) {
                onFailure.call(srcObj, exception);
            }
        }
    };
    $('#' + targetForm).attr("action", targetAction).ajaxForm(options).submit();
}


