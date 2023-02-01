$(function () {
    $.ajaxSetup({ cache: false });
    $(".link-genre--delete").click(function (e) {
        e.preventDefault();
        $.get(this.href, function (data) {
            $('#dialogContent').html(data);
            $('#modDialog').modal('show');
        });
    });
});