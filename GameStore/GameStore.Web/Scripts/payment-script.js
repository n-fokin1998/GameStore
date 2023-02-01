$(function () {
    $(document).ready(function (e) {
        if ($("#successStatus").val() == "1") {
            $('#modDialog').modal('show');
        }
    });
    $(".link-pay--bank").click(function (e) {
        $('#modDialog').modal('show');
        $('#PaymentType').val("0");
        $('#HiddenShipperId').val($("#ShipperId option:selected").val());
        $(".form--pay-order").attr('action', '/Basket/BankTerminal');
        $(".form--pay-order").attr('method', 'POST');
        $(".form--pay-order").submit();
    });
    $(".link-pay--ibox").click(function (e) {
        $('#PaymentType').val("1");
        $('#HiddenShipperId').val($("#ShipperId option:selected").val());
        $(".form--pay-order").attr('action', '/Basket/ShowIBoxView');
        $(".form--pay-order").submit();
    });
    $(".link-pay--visa").click(function (e) {
        $('#PaymentType').val("2");
        $('#HiddenShipperId').val($("#ShipperId option:selected").val());
        $(".form--pay-order").attr('action', '/Basket/ShowVisaView');
        $(".form--pay-order").submit();
    });
    $('#modDialog').on('hidden.bs.modal', function () {
        window.location = "/games";
    });
});