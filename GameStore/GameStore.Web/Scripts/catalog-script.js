$(function () {
    var checkboxes, minPrice, maxPrice, name, itemsPerPage, sortType, pageFlag = 0;
    $(document).ready(function() {
        checkboxes = $(".side input:checked");
        minPrice = $("input[name='MinPrice']").val();
        maxPrice = $("input[name='MaxPrice']").val();
        name = $("input[name='NameFilter']").val();
        itemsPerPage = $("select[name='ItemsPerPage']").val();
        sortType = $("select[name='SortType']").val();
    });
    $("#filterSubmit").click(function (e) {
        e.preventDefault();
        FireSubmit();
    });
    $("select[name='SortType']").change(function (e) {
        e.preventDefault();
        FireSubmit();
    });
    $("select[name='ItemsPerPage']").change(function (e) {
        e.preventDefault();
        FireSubmit();
    });
    $(".btn-group button").click(function (e) {
        e.preventDefault();
        pageFlag = 1;
        $("input[name='Page']").val($(this).val());
        FireSubmit();
    });
    $(".side__filter-reset").click(function (e) {
        e.preventDefault();
        window.location = "/games";
    });
    function FireSubmit() {
        if (CompareFilters()) {
            $('body,html').animate({ scrollTop: 0 }, 500);
            return;
        }
        if ($("input[name='NameFilter']").val().length >= 1 && $("input[name='NameFilter']").val().length < 3) {
            $(".error-name").css("visibility", "visible");
            return;
        }
        var minPrice = $("input[name='MinPrice']").val();
        var maxPrice = $("input[name='MaxPrice']").val();
        if (minPrice != null && maxPrice != null && parseInt(minPrice) > parseInt(maxPrice)) {
            $(".error-price").css("visibility", "visible");
            return;
        }
        $("input[name='SortType']").val($("select[name='SortType'] option:selected").val());
        $("input[name='ItemsPerPage']").val($("select[name='ItemsPerPage'] option:selected").val());
        $("#filterForm").submit();
    }
    function CompareFilters() {
        var actual = $(".side input:checked");
        var actualMinPrice = $("input[name='MinPrice']").val();
        var actualMaxPrice = $("input[name='MaxPrice']").val();
        var actualName = $("input[name='NameFilter']").val();
        var actualItemsPerPage = $("select[name='ItemsPerPage']").val();
        var actualSortType = $("select[name='SortType']").val();
        if (checkboxes.length !== actual.length
            || minPrice !== actualMinPrice
            || maxPrice !== actualMaxPrice
            || name !== actualName
            || itemsPerPage !== actualItemsPerPage
            || sortType !== actualSortType
            || pageFlag !== 0) {
            return false;
        }
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].value !== actual[i].value) {
                return false;
            }
        }
        return true;
    }
});