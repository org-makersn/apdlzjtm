var mypageIndex = 0;

$(function () {

    $('#stx').focus(function () {
        $('.line').addClass('on');
        $("._search_btn").addClass('on');
        //alert('test1');			
    });
    $('#stx').focusout(function () {
        $('.line').removeClass('on');
        $("._search_btn").removeClass('on');
        //alert('test2');			
    });

    //$("._search_btn").click(function () {
    //    //기본
    //    //alert(0);


    //    //var $sfl = $('#sfl').val();
    //    //var $stx = $('#stx').val();
    //    //if ($('#stx').val().trim() == '') {
    //    //    alert('검색어를 입력해 주세요.');
    //    //    $('#stx').focus();
    //    //    return false;
    //    //}

    //    //if ($stx) $stx = '/sfl/' + $sfl + '/stx/' + sEncode($stx);

    //    //location.href = '/items/search/page/1' + $stx;
    //    //return false;
    //});

    //$("#stx").keypress(function (event) {


    //    if (event.keyCode == 13) {
    //        event.preventDefault();
    //        $("._search_btn").trigger('click');
    //    }
    //});


    /* 팝업닫기 실행 */
    $('.popupArea .bgArea').css('opacity', 0.52);
    $('.popupArea .bgArea').css('height', $(document).height());
    $('.popupArea .bgArea').on('click', function (e) {
        if (!$(this).hasClass("dontClose")) {
            e.preventDefault();
            popupClose();
        }
    });


    // 상단 로그인 버튼
    $('.userLogin').hover(function () {
        $('#loginArea').attr("src", "content/images/common/btn/btn_login_on.png");
    }, function () {
        $('#loginArea').attr("src", "content/images/common/btn/btn_login.png");
    })


    // 프로필편집
    $('.btnProfileGo .editProfile').on('click', function (e) {
        e.preventDefault();
        $('.btnProfileGo .photoUpZone').show();
    })

    $('.btnProfileGo .photoUp').hover(function () {
        $(this).addClass('on')
    }, function () {
        $(this).removeClass('on')
    })


    $('.profileImg').mouseleave(function () {
        $('.btnProfileGo .photoUpZone').hide();
    })


    /* 마이페이지 영역 제외 클릭시 마이페이지 팝업 닫기 */
    $(".wrap").click(function (e) {
        if ($(".topMypage").css("display") == "block") {
            console.log(e.target.id);
            if (e.target.id != "showTop" && e.target.id != "showTopNew") {
                mypageOut();
            }
        }
    });

    $(".wrap").click(function (e) {
        if (e.target.id == "divJoinOut") {
            popupClose();
        }
    });
});



function sEncode(val) {
    return encodeURIComponent(val).replace(/%/g, '.');
}

function encode(val) {
    //return encodeURIComponent(val);
    return encodeURIComponent(val).replace(/.com/g, '^com');
}

// 상단 로그인 & 마이페이지 영역 	
function mypageGo() {
    //if (mypageIndex == 0) {
    //    popupClose();
    //    $('#loginArea').attr("src", "content/images/common/profile_photo.jpg");
    //    $('.topMypage').show();
    //    mypageIndex = 1;
    //}
    //else {
    //    popupClose();
    //    $('.topMypage').hide();
    //    mypageIndex = 0;
    //}
    popupClose();
    $('#loginArea').attr("src", "content/images/common/profile_photo.jpg");
    if ($('.topMypage').css("display") == 'none') {
        $('.topMypage').show();
    }
    else {
        $('.topMypage').hide();
    }
}

function mypageOut() {
    popupClose();
    //$('#loginArea').attr("src", "content/images/common/btn/btn_login.png");
    $('.topMypage').hide();
}



$('.btnCoverGo .photoUp').hover(function () {
    $(this).addClass('on')
}, function () {
    $(this).removeClass('on')
})





/* 팝업닫기 */
function popupClose() {
    $('.popupArea').hide();
    $('.btnRe').hide();
}




/* 로그인 */
function loginGo() {
    popupClose();
    $('.popupArea .bgArea').css('height', $(document).height());
    $('.popupArea.loginBox').show();
    console.log($(window).height())
}


/* 회원가입 */
function joinGo() {
    alert("현재 클로즈 베타 테스트 중으로 \n초대를 통해서만 이용할 수 있습니다.");
    return false;

    //popupClose();
    //$('.popupArea.join').show();
}

/* 회원가입완료 */
function joinCom() {
    popupClose();
    $('.popupArea.joinComplete').show();
}

/* 비밀번호 찾기 */
function pwSearch() {
    popupClose();
    $('.popupArea.pwSearch').show();
}


/* 비밀번호 발송 */
function pwSend() {
    popupClose();
    $('.popupArea.pwSend').show();
}


/* Contact 발송확인 */
function contactSend() {
    popupClose();
    $('.popupArea.contact').show();
}

/* 업로드 가이드 팝업 */
function upguidePop() {
    popupClose();
    $('.popupArea.upGuide').show();
    $('.popupArea .bgArea').css('height', $(document).height());
}

/* 게시확인 팝업 */
function uploadEnd() {
    popupClose();
    $('.popupArea.uploadEnd').show();
    $('.popupArea .bgArea').css('height', $(document).height());
}


/* 임시저장 팝업 */
function thumbPop() {
    popupClose();
    $('.popupArea.thumbPop').show();
    $('.popupArea .bgArea').css('height', $(document).height());
}



/* 회원탈퇴 팝업 */
function joinOutPop() {
    popupClose();
    $('.popupArea.joinOut').show();
    $('.popupArea .bgArea').css('height', $(document).height());
}



/* 회원탈퇴 확인 */
function joinOutComPop() {
    popupClose();
    $('.popupArea.joinOutCom').show();
    $('.popupArea .bgArea').css('height', $(document).height());
}

/* 썸네일이미지저장 팝업*/
function thumImgPop() {
    popupClose();
    $('.popupArea.thumnailPop').show();
    $('.popupArea .bgArea').css('height', $(document).height());
}


/* 사용자 중복 확인 팝업*/
function userCheckPop() {
    popupClose();
    $('.popupArea.userIdCheck').show();
    $('.popupArea .bgArea').css('height', $(document).height());
}

/***** 링크 *****/
function go_url(url) {
    location.href = url + ".html";
}









