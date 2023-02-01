$(function () {
    $.ajaxSetup({ cache: false });
    $(".link-platform-type--delete").click(function (e) {
        e.preventDefault();
        $.get(this.href, function (data) {
            $('#dialogContent').html(data);
            $('#modDialog').modal('show');
        });
    });
});