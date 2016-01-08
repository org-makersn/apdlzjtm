'use strict';
function Models() { }

Models.Init = function () {

    $('#code_all').addClass('selected');

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
}