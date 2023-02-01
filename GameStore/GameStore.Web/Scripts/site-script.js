$(function () {
    $(".footer__link-up").bind('click', function (e) {
        e.preventDefault();
        $('body,html').animate({ scrollTop: 0 }, 300);
    });
});