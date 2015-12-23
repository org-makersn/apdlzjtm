'use strict';
var count = 0;

$(function () {
    //로그인
    $("#login_submit").click(function (e) {
        e.preventDefault();

        if ($("#UserId").val() == "") {
            alert('아이디를 입력해 주세요.');
            $("#UserId").focus();
            return true;
        } else if ($("#Password").val() == "") {
            alert('패스워드를 입력해 주세요.');
            $("#Password").focus();
            return true;
        }

        var data = {
            returnUrl: location.pathname + location.search,
            UserId: $("#UserId").val(),
            Password: $("#Password").val(),
            RememberMe: $("#login").attr("checked") == "checked"
        }

        var onSuccess = function (result) {
            if (result.Success) {
                $(location).attr("href", result.Result);
            }
            else {
                if (result.Result != null) {
                    alert(result.Result + "\n이메일 인증을 해주세요");
                }
                else {
                    alert("아이디 혹은 비밀번호가 틀립니다."); $("#Password").val("");
                }
            }
        }
        Ajax.AjaxResponseModelService(this, "/account/logon", data, onSuccess);
    });

    //검색
    $("#btnSearch").click(function () {
        var text = $("#searchTxt").val().trim();
        if (check_msg("searchTxt", "검색어를 입력해주세요.", "required:search") == false) return false;
        //if (text == "") { alert("검색어를 입력해 주세요"); $("#searchTxt").val(''); $('#searchTxt').focus(); return false; }
        location.href = '/design/Search?text=' + encodeURI(text);

        $('.searchArea').addClass('on');
    });

    //검색창 엔터 이벤트
    $('#searchTxt').keypress(function (event) {
        if (event.keyCode == 13) {
            $("#btnSearch").click();
        }
    });

    //프린터 검색
    $("#btnSearchPrt").click(function () {
        var text = $("#searchPrt").val().trim();
        if (check_msg("searchPrt", "검색어를 입력해주세요.", "required:search") == false) return false;
        location.href = '/Printing/PrtSearch?text=' + encodeURI(text);

        $('.searchArea').addClass('on');
    });

    //프린터 검색창 엔터 이벤트
    $('#searchPrt').keypress(function (event) {
        if (event.keyCode == 13) {
            $("#btnSearchPrt").click();
        }
    });

    //로그인 엔터
    $("#Password").keypress(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            $("#login_submit").trigger('click');
        }
    });
    $("#UserId").keypress(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            $("#login_submit").trigger('click');
        }
    });

    //비밀번호 찾기 엔터
    $("#TemporaryEmail").keypress(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            TemporaryPw();
        }
    });


    $("#JoinRePassword").keypress(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            $("#join_submit").click();
        }
    });

    //회원가입 클릭
    $("#join_submit").click(function () {

        if (check_msg("JoinName", "이름을 입력해주세요.", "required:name") == false) return false;
        if (check_msg("JoinEmail", "이메일을 입력해주세요.", "required:email") == false) return false;
        if (check_msg("JoinPassword", "비밀번호를 입력해주세요.", "required:joinPassword") == false) return false;


        if ($("#JoinPassword").val() != $("#JoinRePassword").val()) {
            alert("입력한 비밀번호가 일치하지 않습니다.");
            $("#JoinPassword").val("");
            $("#JoinRePassword").val("");
            $("#JoinPassword").focus();
            return false;
        }
        if ($("#JoinName").val() == "") {
            alert('아이디를 입력해 주세요.');
            $("#JoinName").focus();
            return true;
        } else if ($("#JoinEmail").val() == "") {
            alert('이메일을 입력해 주세요.');
            $("#JoinEmail").focus();
            return true;
        } else if ($("#JoinPassword").val() == "") {
            alert('패스워드를 입력해 주세요.');
            $("#JoinPassword").focus();
            return true;
        }

        var data = {
            returnUrl: location.pathname + location.search,
            JoinName: $("#JoinName").val(),
            JoinEmail: $("#JoinEmail").val(),
            JoinPassword: $("#JoinPassword").val()
        }

        var onSuccess = function (result) {
            if (result.Success) {
                alert("이메일을 발송하였습니다.\n이메일 인증을 해주세요.");
                location.reload(true);
            }
            else {
                switch (result.Result) {
                    case 1:
                        alert("이미 사용중인 email 이거나\n양식에 맞지 않는 email 입니다.");
                        break;
                    case 2:
                        alert("비밀번호 길이를 확인해주세요");
                        break;
                }
            }
        }
        Ajax.AjaxResponseModelService(this, "/account/joinon", data, onSuccess);

    });

})

/* 로그인 */
function loginPop(val) {
    if (val == 0) {
        popupClose();
        $('.popupArea .bgArea').css('height', $(document).height());
        $('.popupArea.loginBox').show();
        //console.log($(window).height())
        return true;
    }
}

//로그아웃
function LogOff() {
    $("#logoutForm").submit();
}


////좋아요 기능
//function SetLikes(val, loginChk) {
//    if (loginPop(loginChk)) { count = 0; return false; }

//    var onSuccess = function (result) {
//        if (result.Result == 0) {
//            $("#likeChk_" + val).text(parseInt($("#likeChk_" + val).text()) - 1);
//            $("#likeChk_" + val).removeClass("on");
//            $("#likes").find(".cnt").text(parseInt($("#likes").find(".cnt").text(), 10) - 1);
//        }
//        else {
//            $("#likeChk_" + val).text(parseInt($("#likeChk_" + val).text()) + 1);
//            $("#likeChk_" + val).addClass("on");
//            $("#likes").find(".cnt").text(parseInt($("#likes").find(".cnt").text(), 10) + 1);
//        }
//        count = 0;
//    }
//    Ajax.AjaxResponseModelService(this, "/design/SetLikes", { articleNo: val }, onSuccess);
//}

//임시 비밀번호 발급
function TemporaryPw() {
    var email = $("#TemporaryEmail").val();
    if (check_msg("TemporaryEmail", "이메일을 입력해주세요.", "required:email") == false) return false;

    var onSuccess = function (result) {
        if (result.Success) {
            $("#TemporaryEmail").val("");
            pwSend();
        }
        else {
            alert("존재하지 않는 email입니다.");
        }
    }
    Ajax.AjaxResponseModelService(this, "/account/SendMail", { email: email }, onSuccess);
}

//search box focus
$('#searchTxt').focus(function () {
    $('.line').addClass('on');
    $("._search_btn").addClass('on');
    //alert('test1');			
});
$('#searchTxt').focusout(function () {
    $('.line').removeClass('on');
    $("._search_btn").removeClass('on');
    //alert('test2');			
});



function check_msg(element_id, msg, patton) {
    var array_is = /[,]/;
    var val_is;
    var element_id1 = false;
    var element_id2 = false;

    if (array_is.test(element_id) == true) {
        element = element_id.split(",");
        if (element[0]) element_id = element[0];
        if (element[1]) element_id1 = element[1];
        if (element[2]) element_id2 = element[2];
    }

    if ($('#' + element_id).val() == '') val_is = false;
    else val_is = true;

    //var re_id = /^[a-z0-9_-]{3,16}$/;												// 아이디 검사식
    var re_pw = /^[a-z0-9_-]{4,18}$/;												// 비밀번호 검사식
    var re_mail = /^([\w\.-]+)@([a-z\d\.-]+)\.([a-z\.]{2,6})$/;						// 이메일 검사식
    var re_url = /^(https?:\/\/)?([a-z\d\.-]+)\.([a-z\.]{2,6})([\/\w\.-]*)*\/?$/;	// URL 검사식
    var re_tel = /^[0-9]{8,11}$/;													// 전화번호 검사식
    var re_jumin = /^\d{6}[1234]\d{6}$/;											// 주민번호
    var re_num = /^[0-9]{1,}/;														// 숫자만 alphabet
    var re_alpha = /^[a-z_-]{1,}$/;													// 영문만 alphabet
    var re_name = /^[가-힣|a-z|A-Z|0-9|\s]+$/; // 한글 또는 영문 사용(혼용 X)

    if (patton != undefined) {
        var patton_arr = patton.split(":");
        for (i = 0; i < patton_arr.length; i++) {

            if (patton_arr[i] == 'required') {
                if (val_is == false) {
                    alert(msg);
                    //alert(msg); 
                    $('#' + element_id).focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'password' && val_is == true) {
                var pw_error = false;
                var error_msg = '';
                if ($('#password').val().length < 6 || $('#password').val().length > 20) {
                    error_msg = '패스워드가 6자 이상 20자 이하 이어야 합니다.';
                    pw_error = true;

                } else if ($('#password').val() != $('#re_password').val()) {
                    error_msg = "패스워드가 일치하지 않습니다.\n다시 확인해서 입력해 주세요.";
                    pw_error = true;
                }

                if (pw_error == true) {
                    alert(error_msg);
                    $('#password').val('');
                    $('#re_password').val('');
                    $('#password').focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'joinPassword' && val_is == true) {
                var pw_error = false;
                var error_msg = '';
                if ($('#JoinPassword').val().length < 6 || $('#JoinPassword').val().length > 20) {
                    error_msg = '패스워드가 6자 이상 20자 이하 이어야 합니다.';
                    pw_error = true;

                } else if ($('#JoinPassword').val() != $('#JoinRePassword').val()) {
                    error_msg = "패스워드가 일치하지 않습니다.\n다시 확인해서 입력해 주세요.";
                    pw_error = true;
                }

                if (pw_error == true) {
                    alert(error_msg);
                    $('#JoinPassword').val('');
                    $('#JoinRePassword').val('');
                    $('#JoinPassword').focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'name' && val_is == true) {
                var pw_error = false;
                var error_msg = '';
                if ($('#' + element_id).val().length > 20) {
                    error_msg = '이름은 20자 이하로 적어주세요.';
                    pw_error = true;
                }
                else {
                    console.log($('#' + element_id).val());
                    if (re_name.test($('#' + element_id).val()) != true) {
                        alert('한글 또는 영문 숫자조합만 가능합니다.');

                        //$('#' + element_id).val('');
                        $('#' + element_id).focus();

                        return false;
                    }
                }

                if (pw_error == true) {
                    alert(error_msg);
                    //$('#' + element_id).val('');
                    $('#' + element_id).focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'title' && val_is == true) {
                var pw_error = false;
                var error_msg = '';
                if ($('#' + element_id).val().length > 20) {
                    error_msg = '제목은 20자 이하로 적어주세요.';
                    pw_error = true;
                }

                if (pw_error == true) {
                    alert(error_msg);
                    //$('#' + element_id).val('');
                    $('#' + element_id).focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'articleTitle' && val_is == true) {
                var pw_error = false;
                var error_msg = '';
                if ($('#' + element_id).val().length > 50) {
                    error_msg = '제목은 50자 이하로 적어주세요.';
                    pw_error = true;
                }

                if (pw_error == true) {
                    alert(error_msg);
                    //$('#' + element_id).val('');
                    $('#' + element_id).focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'contents' && val_is == true) {
                var pw_error = false;
                var error_msg = '';
                if ($('#' + element_id).val().length > 4000) {
                    error_msg = '내용 4000자 이하로 적어주세요.';
                    pw_error = true;
                }
                if (pw_error == true) {
                    alert(error_msg);
                    //$('#' + element_id).val('');
                    $('#' + element_id).focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'search' && val_is == true) {
                var pw_error = false;
                var error_msg = '';
                //if ($('#' + element_id).val().length > 50) {
                //    error_msg = '검색어는 50자 이하로 적어주세요.';
                //    pw_error = true;
                //}
                if (pw_error == true) {
                    alert(error_msg);
                    //$('#' + element_id).val('');
                    $('#' + element_id).focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'report' && val_is == true) {
                var pw_error = false;
                var error_msg = '';
                if ($('#' + element_id).val().length > 2000) {
                    error_msg = '신고 내용은 2000자 이하로 적어주세요.';
                    pw_error = true;
                }
                if (pw_error == true) {
                    alert(error_msg);
                    //$('#' + element_id).val('');
                    $('#' + element_id).focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'message' && val_is == true) {
                var pw_error = false;
                var error_msg = '';
                if ($('#' + element_id).val().length > 200) {
                    error_msg = '메시지는 200자 이하로 적어주세요.';
                    pw_error = true;
                }
                if (pw_error == true) {
                    alert(error_msg);
                    //$('#' + element_id).val('');
                    $('#' + element_id).focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'profile_msg' && val_is == true) {
                var pw_error = false;
                var error_msg = '';
                if ($('#' + element_id).val().length > 150) {
                    error_msg = '자기소개는 150자 이하로 적어주세요.';
                    pw_error = true;
                }
                if (pw_error == true) {
                    alert(error_msg);
                    //$('#' + element_id).val('');
                    $('#' + element_id).focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'email') {
                if (element_id1) {
                    var $email = $('#' + element_id);
                    var $email1 = $('#' + element_id1);
                    if ($email.val() != '') {
                        var $email_val = $email.val() + '@' + $email1.val();

                        if (re_mail.test($email_val) != true) {
                            alert('이메일 형식이 틀렸습니다. 다시 입력해 주세요.');
                            //$email.val('');
                            //$email1.val('');
                            $email.focus();
                            return false;
                        }
                    }
                } else {
                    var $email = $('#' + element_id);
                    if ($email.val().length > 50) {
                        alert('이메일 길이가 너무 깁니다.');
                        $email.focus();
                        return false;
                    }
                    if (re_mail.test($email.val()) != true) {
                        alert('이메일 형식이 틀렸습니다. 다시 입력해 주세요.');
                        //$email.val('');
                        $email.focus();
                        return false;
                    }
                }
            }

            if (patton_arr[i] == 'select') {
                var $selects = $('select[name=' + element_id + ']');
                if ($selects.val() == '') {
                    alert(msg + ' 항목을 입력해 주세요.');
                    $selects[0].focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'radio') {
                var $radios = $('input:radio[name=' + element_id + ']');
                if (!$radios.is(":checked")) {
                    alert(msg);
                    $radios[0].focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'checkbox') {
                var $checkClass = $('.' + element_id);
                if (!$checkClass.is(":checked")) {
                    if (msg == '') alert('선택된 항목이 없습니다.');
                    else alert(msg);

                    $checkClass[0].focus();

                    return false;
                }

            }


            if (patton_arr[i] == 'agree1') {
                if (!($("#agree1").is(":checked"))) {
                    alert(msg);
                    $("#agree1").focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'agree3') {
                if (!($("#agree3").is(":checked"))) {
                    alert(msg);
                    $("#agree3").focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'alphabet' && val_is == true) {
                if (re_alpha.test($('#' + element_id).val()) != true) {
                    alert('영문과 하이폰만 입력 가능합니다.');
                    $('#' + element_id).focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'number' && val_is == true) {
                if (re_num.test($('#' + element_id).val()) != true) {
                    alert('숫자만 입력 하시기 바랍니다.');
                    $('#' + element_id).focus();
                    $('#' + element_id).val('');
                    return false;
                }
            }

            if (patton_arr[i] == 'homepage' && val_is == true) {
                if (re_url.test($('#' + element_id).val()) != true) {
                    alert('홈페이지 주소를 바르게 입력해 주세요.');

                    $('#' + element_id).focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'jumin' && val_is == true) {
                $jumin = $('#register_no1').val() + $('#register_no2').val();

                if (re_jumin.test($jumin) != true) {

                    alert('주민번호 형식이 틀렸습니다. 다시 입력해 주세요.');
                    $('#register_no1').val('');
                    $('#register_no2').val('');
                    $('#register_no1').focus();

                    return false;
                }
            }

            if (patton_arr[i] == 'phone3' && val_is == true) {
                console.log($('#' + element_id).val().length);
                if ($('#' + element_id).val().length < 3) {
                    alert('전화번호를 확인해 주세요');
                    $('#' + element_id).focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'phone4' && val_is == true) {
                if ($('#' + element_id).val().length < 4) {
                    alert('전화번호를 확인해 주세요');
                    $('#' + element_id).focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'cashReceiptsCard4' && val_is == true) {
                if ($('#' + element_id).val().length < 4) {
                    alert('현금영수증 카드번호를 확인해 주세요');
                    $('#' + element_id).focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'cashReceiptsCard6' && val_is == true) {
                if ($('#' + element_id).val().length < 6) {
                    alert('현금영수증 카드번호를 확인해 주세요');
                    $('#' + element_id).focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'companyNum3' && val_is == true) {
                if ($('#' + element_id).val().length < 3) {
                    alert('사업자등록번호를 확인해 주세요');
                    $('#' + element_id).focus();
                    return false;
                }
            }
            if (patton_arr[i] == 'companyNum2' && val_is == true) {
                if ($('#' + element_id).val().length < 2) {
                    alert('사업자등록번호를 확인해 주세요');
                    $('#' + element_id).focus();
                    return false;
                }
            }
            if (patton_arr[i] == 'companyNum5' && val_is == true) {
                if ($('#' + element_id).val().length < 5) {
                    alert('사업자등록번호를 확인해 주세요');
                    $('#' + element_id).focus();
                    return false;
                }
            }

        }
    }
}