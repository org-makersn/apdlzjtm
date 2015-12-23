'use strict';
var count = 0;

$(function () {
    //로그인
    $("#login_submit").click(function (e) {
        e.preventDefault();

        if ($("#UserId").val() == "") {
            alert('Please enter User ID.');
            $("#UserId").focus();
            return true;
        } else if ($("#Password").val() == "") {
            alert('Please enter password.');
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
                    alert(result.Result + "\nPlease confirm your email");
                }
                else {
                    alert("The username or password is invalid."); $("#Password").val("");
                }
            }
        }
        Ajax.AjaxResponseModelService(this, "/account/logon", data, onSuccess);
    });

    //검색
    $("#btnSearch").click(function () {
        var text = $("#searchTxt").val().trim();
        if (check_msg("searchTxt", "Please enter a search word.", "required:search") == false) return false;
        //if (text == "") { alert("Please enter a search word"); $("#searchTxt").val(''); $('#searchTxt').focus(); return false; }
        location.href = '/design/Search?text=' + encodeURI(text);

        $('.searchArea').addClass('on');
    });

    //검색창 엔터 이벤트
    $('#searchTxt').keypress(function (event) {
        if (event.keyCode == 13) {
            $("#btnSearch").click();
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
            alert("The password is invalid.");
            $("#JoinPassword").val("");
            $("#JoinRePassword").val("");
            $("#JoinPassword").focus();
            return false;
        }
        if ($("#JoinName").val() == "") {
            alert('Please enter User ID.');
            $("#JoinName").focus();
            return true;
        } else if ($("#JoinEmail").val() == "") {
            alert('Please enter your email.');
            $("#JoinEmail").focus();
            return true;
        } else if ($("#JoinPassword").val() == "") {
            alert('Please enter password.');
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
                alert("email has been sent.\nPlease confirm your email.");
                location.reload(true);
            }
            else {
                switch (result.Result) {
                    case 1:
                        alert("The email is already in use or is in wrong format.");
                        break;
                    case 2:
                        alert("Please check the length of the password.");
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
    if (check_msg("TemporaryEmail", "Please enter your email.", "required:email") == false) return false;

    var onSuccess = function (result) {
        if (result.Success) {
            $("#TemporaryEmail").val("");
            pwSend();
        }
        else {
            alert("The email does not exist.");
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
                        alert('Please use Korean, English or numbers only.');

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
                if ($('#' + element_id).val().length > 50) {
                    error_msg = '검색어는 50자 이하로 적어주세요.';
                    pw_error = true;
                }
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
                            alert('email format is invalid. Please enter again.');
                            //$email.val('');
                            //$email1.val('');
                            $email.focus();
                            return false;
                        }
                    }
                } else {
                    var $email = $('#' + element_id);
                    if ($email.val().length > 50) {
                        alert('The length of email is too long.');
                        $email.focus();
                        return false;
                    }
                    if (re_mail.test($email.val()) != true) {
                        alert('email format is invalid. Please enter again.');
                        //$email.val('');
                        $email.focus();
                        return false;
                    }
                }
            }

            if (patton_arr[i] == 'select') {
                var $selects = $('select[name=' + element_id + ']');
                if ($selects.val() == '') {
                    alert(msg + ' Please select a category.');
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
                    if (msg == '') alert('Category not selected.');
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
                    alert('Please us English and hyphen only.');
                    $('#' + element_id).focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'number' && val_is == true) {
                if (re_num.test($('#' + element_id).val()) != true) {
                    alert('Please enter numbers only.');
                    $('#' + element_id).focus();
                    $('#' + element_id).val('');
                    return false;
                }
            }

            if (patton_arr[i] == 'homepage' && val_is == true) {
                if (re_url.test($('#' + element_id).val()) != true) {
                    alert('Please enter the website address correctly.');

                    $('#' + element_id).focus();
                    return false;
                }
            }

            if (patton_arr[i] == 'jumin' && val_is == true) {
                $jumin = $('#register_no1').val() + $('#register_no2').val();

                if (re_jumin.test($jumin) != true) {

                    alert('ID number format is invalid. Please try again.');
                    $('#register_no1').val('');
                    $('#register_no2').val('');
                    $('#register_no1').focus();

                    return false;
                }
            }
        }
    }
}