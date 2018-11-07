// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("#toggleDoneItems").click(function () {
    $(".doneItem").toggle("slow", function () {
    });
    var text = $('#toggleDoneItemsText').text();
    $('#toggleDoneItemsText').text(
        text == "Hide Completed Items" ? "Show Completed Items" : "Hide Completed Items");
});