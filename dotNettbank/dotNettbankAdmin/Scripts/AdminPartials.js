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
    var name = $(this).data('name')

    output = "<li class='tag'><div data-id='" + userID + "' class='tag'><p><i class='fa fa-tags' aria-hidden='true'></i>"
            + " " + name + ", " + userID + " " + " <i class='fa fa-times'></i></p></div></li>"
    $("#tabsmenu").append(output);
});

$("#tabsmenu").on('click', '.tag', function () {
    $(this).remove();
});






