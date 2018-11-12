// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Show/hide done items in item list
$("#toggleDoneItems").click(function () {
    $(".doneItem").toggle("slow", function () {
    });
    var text = $('#toggleDoneItemsText').text();
    $('#toggleDoneItemsText').text(
        text === "Hide Completed Items" ? "Show Completed Items" : "Hide Completed Items");
});

// Order list items by ranking/importance
$(".sortableArrow").hide();
$("#toggleRanking").click(function () {
    if ($('#toggleRankingText').text() === "Order By Importance") {
        location.reload();
    }
    else {
        $(".itemOrder li").sort(sort_li).appendTo('.itemOrder');
        var text = $('#toggleRankingText').text();
        $('#toggleRankingText').text(
            text === "Order By Rank" ? "Order By Importance" : "Order By Rank");
        $(".sortableArrow").show();
    }
    function sort_li(a, b) {
        return ($(a).data('position')) > ($(b).data('position')) ? 1 : -1;

    }
    $(function () {
        $("#sortable").sortable({
            items: "li:not(.ui-state-disabled)",
            cursor: 'move',
            start: function (event, ui) {
                // Get the start index so no database call 
                //is made if item is dropped in the same order
                $StartIndex = ui.item.index() + 1;
            },
            stop: function (event, ui) {
                // At the end of the drag, if item is in different ordinal position, 
                // update the database using the moveListItem function
                idListItem = ui.item[0].id.replace('item_', '');
                newListIndex = ui.item.index() + 1;
                if ($StartIndex !== newListIndex) {
                    moveListItem(idListItem, newListIndex);
                } 
                function moveListItem(listItemId, newListIndex) {
                    newRank = newListIndex - 2;
                    listItemId = parseInt(listItemId);
                    var data = JSON.stringify({ "todoListItemId": listItemId, "newRank": newRank });
                    $.ajax({
                        type: "POST",
                        url: "/api/TodoItemsRankAPI",
                        data: data,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                        }

                    });
                    $('#item_' + listItemId).attr("data-position", newRank);
                }

            }
        });
        $("#sortable").disableSelection();


    });

});

// Show/Hide 'Add new item' panel in item list
$("#addNewItemPanel").hide();
$("#addNewItemBtn").click(function () {
    $("#addNewItemPanel").toggle("slow", function () {
    });
    var text = $('#addNewItemBtn').text();
    $('#addNewItemBtn').text(
        text === "Add New Item" ? "Cancel New Item" : "Add New Item");
});