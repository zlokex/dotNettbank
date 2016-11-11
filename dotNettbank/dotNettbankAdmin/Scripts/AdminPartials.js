

$(document).ready(function () {
    setPagingCount();

    var $rows;
    $rows = $('.search-table tbody tr');
    $('#search').keyup(function () {
        
        var val = '^(?=.*\\b' + $.trim($(this).val()).split(/\s+/).join('\\b)(?=.*\\b') + ').*$',
          reg = RegExp(val, 'i'),
          text;

        $rows.show().filter(function () {
            text = $(this).text().replace(/\s+/g, ' ');
            return !reg.test(text);
        }).hide();
        

    });

});

function setPagingCount() {
    var rowsInt = calculateTableRowsByAvailableHeight();
    $('.paging-table').paging({ limit: rowsInt });

    // Set rows for audit-table at twice the amount (because of the hidden tr)
    var rowsAudit = rowsInt * 2;
    // Make sure that number of rows is even (so that hidden tr of last row won't be displayed on the next page)
    if (rowsAudit % 2 != 0) {
        rowsAudit -= 1;
    }
    $('#audit-table').paging({ limit: rowsAudit });
}

function calculateTableRowsByAvailableHeight() {
    //var documentHeight = $(document).height();
    var windowHeight = $(window).height();

    var navHeight = $("nav").height();
    var searchHeight = $("#search").height();
    var theadHeight = $("thead").height();
    var panelHeadingHeight = $(".panel-heading").height();
    var mainFooterHeight = $(".main-footer").height();

    var trHeight = $("tr").height();

    var availableHeigth = windowHeight - navHeight - searchHeight - panelHeadingHeight - mainFooterHeight - theadHeight;

    var rows = availableHeigth / trHeight;
    rows = rows * 0.65;
    var rowsInt = parseInt(rows);

    //alert(windowHeight);
    //alert(avaliableHeigth);
    //alert(rowsInt);
    return rowsInt;
}


$('.useraddtab').click(function () {
    var userID = $(this).data('id');
    var name = $(this).data('name');
    var type = $(this).data('type');
    var firstname = $(this).data('firstname');
    var lastname = $(this).data('lastname');

    
        //KundeTag
        if (lastname != null) {
            output = "<li class='tagli'><div data-type='" + type + "' data-id='" + userID + "' class='tag'><p><i class='fa fa-tags' aria-hidden='true'></i>"
                    + " " + firstname + " " + lastname + " " + userID + " <i class='fa fa-times tag_close'></i></p></div></li>";
            // Check if tag with this id dosen't allready exists (count ===0)
            if ($('#customer-tags').has("div.tag[data-id=" + userID + "]").length === 0) {
                $("#customer-tags").append(output);
            }
        } else { //Kontotag
            output = "<li class='tagli'><div data-type='" + type + "' data-id='" + userID + "' class='tag'><p><i class='fa fa-tags' aria-hidden='true'></i>"
            + " " + firstname + ": " + userID + " " + " <i class='fa fa-times tag_close'></i></p></div></li>";
            // Check if tag with this id dosen't allready exists (count ===0)
            if ($('#account-tags').has("div.tag[data-id=" + userID + "]").length === 0) {
                $("#account-tags").append(output);
            }
        }
});









