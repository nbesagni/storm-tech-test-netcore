// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("#toggleDoneItems").click(function () {
    $(".doneItem").toggle("slow", function () {
    });
    var text = $('#toggleDoneItemsText').text();
    $('#toggleDoneItemsText').text(
        text === "Hide Completed Items" ? "Show Completed Items" : "Hide Completed Items");
});


$("#toggleRanking").click(function () {
    if ($('#toggleRankingText').text() === "Order By Importance") {
        location.reload();
    }
    else {
        $(".itemOrder li").sort(sort_li).appendTo('.itemOrder');
        var text = $('#toggleRankingText').text();
        $('#toggleRankingText').text(
            text === "Order By Rank" ? "Order By Importance" : "Order By Rank");
    }
    function sort_li(a, b) {
        return ($(a).data('position')) < ($(b).data('position')) ? 1 : -1;
    }
});