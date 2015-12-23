
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


Ajax.AjaxResponseModelServiceFileUpload = function (srcObj, targetUrl, params, fileElementId, onSuccess, onFailure, options) {
    if (targetUrl == null || targetUrl == '') return;

    var settings = {
        type: 'POST',
        async: true,
        contentType: 'application/json;charset=utf-8',
        dataType: 'json'
    };

    options = $.extend(true, {}, settings, options);
    alert(fileElementId);
    $.ajaxFileUpload({
        url: targetUrl,
        secureuri: false,
        fileElementId: fileElementId,
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

