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
            // Check if tag with this id allready exists (count > 0 or count ===0)
            if ($('#customer-tags').has("div.tag[data-id=" + userID + "]").length === 0) {
                $("#customer-tags").append(output);
            }
        } else { //Kontotag
            output = "<li class='tagli'><div data-type='" + type + "' data-id='" + userID + "' class='tag'><p><i class='fa fa-tags' aria-hidden='true'></i>"
            + " " + firstname + ": " + userID + " " + " <i class='fa fa-times tag_close'></i></p></div></li>";
            if ($('#account-tags').has("div.tag[data-id=" + userID + "]").length === 0) {
                $("#account-tags").append(output);
            }
        }
});









