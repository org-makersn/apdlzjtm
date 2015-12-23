'use strict';
var uploadCnt;
var Upload = Upload || {};
var uploadFileCnt = 0;

Upload.Init = function () {
    uploadCnt = $("#uploadCnt").val();
    //브라우저 체크
    Upload.BrowserRecognition();

    Upload.GetArticleFiles();

    $("#imgupload").on("change", function (e) {
        e.preventDefault();
        var fileobj = $("#imgupload");
        if (Upload.ValidExtension("img", fileobj) === false) {
            return false;
        }

        Upload.File("/design/ImgUpload", fileobj);
    });

    $("#stlupload").on("change", function (e) {
        e.preventDefault();
        var fileobj = $("#stlupload");
        if (Upload.ValidExtension("3d", fileobj) === false) {
            return false;
        }

        Upload.File("/design/StlUpload", fileobj);
    });

    //$('#code_all').addClass('selected');

    $('#li_4').addClass('selected');

    $('.lst_value li').on('click', function () {
        $('.lst_value li').removeClass("selected");
        $(this).addClass("selected");
    });

    $('.lst_value2 li').on('click', function () {
        $('.lst_value2 li').removeClass("selected");
        $(this).addClass("selected");
    });

    $('.lst_value li').mouseenter(function () {
        $('.lst_value li').removeClass('selected');
    })

    $('.lst_value2 li').mouseenter(function () {
        $('.lst_value2 li').removeClass('selected');
    })

    // 캡쳐
    $('#save_id,.bgThumOutput').on('click', function () {
        //var ifm = $("#viewer_frame").contents().find("canvas");
        //$('#stl_val').val(ifm[0].toDataURL());
        $('#stl_val').val(thingiview.renderer.domElement.toDataURL());

        var $form_data = $("#img_form").serialize();
        //console.log($form_data);
        thingiview = null;

        $.ajax({
            type: 'POST',
            url: '/design/ImgCapture',
            data: $form_data,
            dataType: 'json',
            beforeSend: function () {
                popupClose();
            },
            success: function (data) {
                Upload.GetArticleFiles();
            }, error: function (e) {
                console.log(e);
            }
        });
    });
}

//파일 업로드
Upload.File = function (action, element) {
    closet(); //페이지 머무르기 실행

    $("#mode").val("temp");
    var $form_data = $("#insert_form").serialize();
    var options = {
        beforeSend: function () {
            //$('.btnLoading').show();
        },
        uploadProgress: function (event, position, total, percentComplete) {
            //console.log(percentComplete)
            $('.btnLoading').show();

            $("#progressbar").find("span").text(percentComplete + "%");
        },
        success: function (response) {
            console.log(response);
            if (response.Success) {
                $(".imgDefualt").show();
                uploadFileCnt++;
                if (uploadFileCnt >= uploadCnt) {
                    Upload.More();
                }
                Upload.GetArticleFiles(response.Result);
                $('.btnLoading').hide();
            }
            else {
                alert(response.Message);
                //alert("success and response false");
            }
        },
        complete: function (response) {
            $(element).val("");
            //alert("complate");
        },
        error: function (error) {
            console.log(error);
            //alert("error");
        }
    };

    $('#insert_form').attr("action", action).ajaxForm(options).submit();
}

//
Upload.StlTrigger = function (no) {
    $("#stl_" + no).trigger('click');
}

//업로드 버튼 클릭
Upload.ArticleUpload = function () {
    uncloset();//페이지 새로고침 끄기
    
    $("#mode").val("upload");

    if (check_msg('main_img', '메인이미지를 선택해 주세요.', 'required') == false) return false;
    if (check_msg('article_title', '제목을 입력해 주세요.', 'required:articleTitle') == false) return false;
    if (check_msg('article_contents', '내용을 입력해 주세요.', 'required:contents') == false) return false;


    if (!$("input:radio[name=lv1]").is(":checked")) {
        alert("카테고리를 선택해주세요.");
        $("#article_title").focus();
        return false;
    }
    //if ($("#article_title").val().length > 30) {
    //    alert("제목은 30자 내외로 적어주세요");
    //    return false;
    //}
    //if ($("#article_contents").val().length > 2000) {
    //    alert("내용은 2000자 내외로 적어주세요");
    //    return false;
    //}

    var $form_data = $("#insert_form").serialize();
    var options = {
        beforeSend: function () {
        },
        uploadProgress: function (event, position, total, percentComplete) {
        },
        success: function (response) {
            if (response.Success) {
                popupClose();

                $('.popupArea.uploadEnd').show();
                $('.popupArea .bgArea').css('height', $(document).height());
                $("#No").val(response.Result);
            }
            else {
                alert(response.Message);
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
                    //alert(ex.name); 
                }
            }
        }
    };

    $('#insert_form').attr("action", "/design/Upload").ajaxForm(options).submit();
}

//임시저장
Upload.TempUpload = function () {
    uncloset();//새로고침 해제

    $("#mode").val("temp");
    if (check_msg('main_img', '메인이미지를 선택해 주세요.', 'required') == false) return false;
    //if (check_msg('article_title', '제목을 입력해 주세요.', 'required') == false) return false;
    //if (check_msg('article_contents', '내용을 입력해 주세요.', 'required') == false) return false;

    //if (!$("input:radio[name=lv1]").is(":checked")) {
    //    alert("카테고리를 선택해주세요.");
    //    $("#article_title").focus();
    //    return false;
    //}

    var $form_data = $("#insert_form").serialize();

    var options = {
        beforeSend: function () {
        },
        uploadProgress: function (event, position, total, percentComplete) {
            
        },
        success: function (response) {
            if (response.Success) {
                $("#No").val(response.Result);
                popupClose();
                $('.popupArea.thumbPop').show();
                $('.popupArea .bgArea').css('height', $(document).height());
            }
            else {
                alert(response.Message);
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
                    //alert(ex.name); 
                }
            }
        }
    };

    $('#insert_form').attr("action", "/design/Upload").ajaxForm(options).submit();
}

//업로드 된 파일 리스트
Upload.GetArticleFiles = function (no) {
    var $form_data = $("#insert_form").serialize();

    var reqUrl = "/design/UploadFilesView";
    //Ajax.SyncGetPartialView(reqUrl, $form_data, "ajax_upload");

    $.post(reqUrl, $form_data, function (result) {
        //alert(result);
        $("#ajax_upload").html(result);
        setTimeout("Upload.StlTrigger(" + no + ")", 500);

    })
    .done(function () {
        //alert("second success");
    })
    .fail(function (response) {
        if (response != null && response.status != null && response.status != 200)
            alert(response.status + ' ' + response.statusText);
    })
    .always(function () {
        //alert("finished");
    });

}

Upload.UISelect = function (obj, valLst, val, check_id) {
    var set = {
        ie6: navigator.userAgent.toLowerCase().indexOf("msie 6") != -1,
        container: $(obj),
        value: $(val),
        list: $(valLst),
        firstVal: $(valLst + "> li:first-child > label").html(),
        li: $(valLst + "> li"),
        label: $(valLst + "> li > label")
    }

    set.list.hide();
    if (check_id == '') {
        set.value.append(set.firstVal);
    }
    else {
        set.value.append($("#" + check_id).next().html());
    }
    set.value.click(function () {

        set.list.show();
        set.list.focusin(function () {
            set.list.show();
        });
        if (set.ie6) {
            set.label.hover(
                function () { $(this).parent().addClass("selected"); },
                function () { $(this).parent().removeClass("selected"); }
            );
        }
        function listHide() {
            setTimeout(function () { set.list.hide(); }, 300);
        }
        set.container.mouseleave(listHide);
        set.label.click(function () {
            set.value.empty().append($(this).html());

            set.list.hide();
        });
    });
}

//파일 확장자 체크
Upload.ValidExtension = function (type, fileobj) {
    var regex;

    if (fileobj[0].files[0].size > 200 * 1024 * 1024) {
        alert('최대 사이즈를 초과하였습니다.');
        return false;
    }

    if (type == "img") {

        regex = /(.jpg|.jpeg|.gif|.png)$/i;
        if (!regex.test(fileobj.val().toLowerCase())) {
            alert('gif, jpg, png 형식 파일만 가능합니다.');
            return false;
        }
    }
    else if(type == "3d")
    {
        regex = /(.stl|.obj)$/i;
        if (!regex.test(fileobj.val().toLowerCase())) {
            alert('stl, obj 형식 파일만 가능합니다.');
            return false;
        }
    }
    else {
        alert('파일 타입을 확인해주세요.');
        return false;
    }
}

// 브라우저체크하여 안내
Upload.BrowserRecognition = function () {
    var is_chrome = navigator.userAgent.indexOf('Chrome') > -1;
    var is_safari = navigator.userAgent.indexOf("Safari") > -1;
    var heightSafari = 0;
    if ((is_chrome) && (is_safari)) { is_safari = false; }

    var Browser = {
        chk: navigator.userAgent.toLowerCase()
    }

    Browser = {
        ie: Browser.chk.indexOf('msie') != -1,
        ie6: Browser.chk.indexOf('msie 6') != -1,
        ie7: Browser.chk.indexOf('msie 7') != -1,
        ie8: Browser.chk.indexOf('msie 8') != -1,
        ie9: Browser.chk.indexOf('msie 9') != -1,
        ie10: Browser.chk.indexOf('msie 10') != -1,
        opera: !!window.opera,
        safari: Browser.chk.indexOf('safari') != -1,
        safari3: Browser.chk.indexOf('applewebkir/5') != -1,
        mac: Browser.chk.indexOf('mac') != -1,
        chrome: Browser.chk.indexOf('chrome') != -1,
        firefox: Browser.chk.indexOf('firefox') != -1
    }

    if ((Browser.ie8) || (Browser.ie9) || (Browser.ie10)) {

        $('.showThum').on('click', function () {

            alert('익스플로러11, 크롬, 파이어폭스를 설치 후 이용해 주세요.')
            return false;
        })
        $('.showThum').removeAttr('onclick')
    }

    if (navigator.userAgent.indexOf('Safari') != -1 && navigator.userAgent.indexOf('Chrome') == -1) {
        $('.showThum').on('click', function () {
            alert('익스플로러11, 크롬, 파이어폭스를 설치 후 이용해 주세요.')
            return false;
        })

        $('.showThum').removeAttr('onclick')
    }
}

Upload.More = function () {
    var addUploadCnt = parseInt(uploadCnt) + 5;
    $("#uploadCnt").val(addUploadCnt);
    uploadCnt = $("#uploadCnt").val();
    Upload.GetArticleFiles();
}

Upload.DeleteMore = function () {
    if (uploadCnt > 10) {
        if (uploadFileCnt <= parseInt(uploadCnt) - 5) {
            var addUploadCnt = parseInt(uploadCnt) - 5;
            $("#uploadCnt").val(addUploadCnt);
            uploadCnt = $("#uploadCnt").val();
            Upload.GetArticleFiles();
        }
        else {
            alert("파일이 추가되어 있어서 삭제 할 수 없습니다");
        }
    }
    else {
        alert("더 이상 삭제 할 수 없습니다");
    }
}

Upload.minusCnt = function () {
    uploadFileCnt--;
}

Upload.ArticleDelete = function () {
    if (confirm("삭제 하시겠습니까?") == true) {
        var articleNo = $("#No").val();

        var $form_data = $("#insert_form").serialize();
        var options = {
            beforeSend: function () {
            },
            uploadProgress: function (event, position, total, percentComplete) {
            },
            success: function (response) {
                location.href = '/profile';
            },
            complete: function (response) {
            },
            error: function (error) {
            }
        };

        $('#insert_form').attr("action", "/design/Delete").ajaxForm(options).submit();
    }
}

Upload.UploadCancle = function () {
    var temp = $("#temp").val();
    var onSuccess = function (response) {
        if (response.Success) {
            location.href = '/';
        }
    }
    Ajax.AjaxResponseModelService(this, "/design/UploadCancle", { temp: temp }, onSuccess);
}

Upload.GoMain = function () {
    location.href = '/';
}
Upload.GoArticle = function () {
    //window.onbeforeunload = null; 동작안함
    var no = $("#No").val();
    var codeNo = $('input[name=lv1]:checked').val()
    location.href = "/design/Detail?no=" + no + "&codeNo=" + codeNo;
}