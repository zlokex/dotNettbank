var $rows;

$(document).ready(function () {
    $('.search-table').paging({ limit: 10 });

    var counter = 0;
    var tagString = "";
    var listItems = $("#tabsmenu li");
    listItems.each(function (idx, li) {
        var tag = $(li);
        if(counter >0){
            var tagValue = tag.children().data("id")
            tagString += tagValue + " ";
        }
        counter++;
    });

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

    if (counter > 1) {
        $('#search').val(tagString);
        $('#search').keyup();
    }

  
});




$('.useraddtab').click(function () {
    var userID = $(this).data('id');
    var firstname = $(this).data('firstname');
    var lastname = $(this).data('lastname');

    //KundeTag
    if (lastname != null){
    output = "<li class='tags'><div data-id='" + userID + "' class='tag'><p><i class='fa fa-tags' aria-hidden='true'></i>"
            + " " + firstname + " " + lastname + " " + " <i class='fa fa-times tag_close'></i></p></div></li>";
    } else { //Kontotag
        output = "<li class='tags'><div data-id='" + userID + "' class='tag'><p><i class='fa fa-tags' aria-hidden='true'></i>"
            + " " + firstname + " " + userID + " " + " <i class='fa fa-times tag_close'></i></p></div></li>";

    }
    //alert($('#tabsmenu').has("li[data-id=" + userID + "]").length);
    // Check if tag with this id allready exists (count > 0 or count ===0)
    if ($('#tabsmenu').has("div.tag[data-id=" + userID + "]").length === 0) {
        $("#tabsmenu").append(output);
    }
    
});

$("#tabsmenu").on('click', '.tag_close', function () {
    $(this).closest("li").remove();
});










