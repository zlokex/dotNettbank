var $rows;

$(document).ready(function () {
    $('.search-table').paging({ limit: 10 });

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
    } else { //Kontotag
        output = "<li class='tagli'><div data-type='" + type + "' data-id='" + userID + "' class='tag'><p><i class='fa fa-tags' aria-hidden='true'></i>"
            + " " + firstname + ": " + userID + " " + " <i class='fa fa-times tag_close'></i></p></div></li>";
    }

   
    //alert($('#tabsmenu').has("li[data-id=" + userID + "]").length);
    // Check if tag with this id allready exists (count > 0 or count ===0)
    if ($('#tabsmenu').has("div.tag[data-id=" + userID + "]").length === 0) {
        $("#tabsmenu").append(output);
    }
    
});

$("#tabsmenu").on('click', '.tag_close', function () {
    // .remove(), only removes the element from the DOM, not from Javascript memory
    // therefore we first set data-type to blank:
    //$(this).find("div").data('type', "disabled");
    $(this).closest("li").remove();
    
});







