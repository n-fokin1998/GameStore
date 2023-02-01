﻿$(function () {
    var commentSpans = document.querySelectorAll("ul .span--comment-body");
    var quoteIds = document.querySelectorAll("ul .quote-id");
    var commentBodies = document.querySelectorAll("ul .comment-hidden-body");
    for (var i = 0; i < quoteIds.length; i++) {
        commentSpans[i].innerHTML = "";
        if (quoteIds[i].value !== "") {
            var quoteElem = $("[data-id-for-qoute='" + quoteIds[i].value + "']");
            if (quoteElem.attr("data-deleted") === "1") {
                commentSpans[i].innerHTML = "<blockquote><img src='/Content/Images/trash.png' width = '25' height = '25' />" + quoteElem.val() + "</blockquote>";
            } else {
                commentSpans[i].innerHTML = "<blockquote>" + quoteElem.val() + "</blockquote>";
            }
        }
        commentSpans[i].innerHTML += commentBodies[i].value;
    }
    $(".link-comment--reply").click(function (e) {
        var name = this.getAttribute("data-name");
        var id = this.getAttribute("data-id");
        $(".span--comment-editor").text(name + ", ");
        $("input[name='ParentCommentId']").val(id);
    });
    $(".link-comment--quote").click(function (e) {
        var body = this.getAttribute("data-body");
        var id = this.getAttribute("data-parentid");
        $(".block--comment-quote").text(" ");
        if (this.getAttribute("data-deleted") === "1") {
            $(".block--comment-quote").append("<blockquote id='quote'><img src='/Content/Images/trash.png' width = '25' height = '25' />" + body + "</blockquote><br />");
        } else {
            $(".block--comment-quote").append("<blockquote id='quote'>" + body + "</blockquote><br />");
        }
        $("input[name='ParentCommentId']").val(id);
        $("input[name='QuoteCommentId']").val(id);
    });
    $.ajaxSetup({ cache: false });
    $(".link-comment--delete").click(function (e) {
        e.preventDefault();
        $.get(this.href, function (data) {
            $('#dialogContent').html(data);
            $('#modDialog').modal('show');
        });
    });
});