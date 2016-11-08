﻿$(document).ready(function () {

    $('.search-table').paging({ limit: 10 });

    var $rows = $('.search-table tbody tr');
    $('#search').keyup(function () {

        var val = '^(?=.*\\b' + $.trim($(this).val()).split(/\s+/).join('\\b)(?=.*\\b') + ').*$',
            reg = RegExp(val, 'i'),
            text;

        $rows.show().filter(function () {
            text = $(this).text().replace(/\s+/g, ' ');
            $('#search').on('beforeItemAdd', function (event) {
                if (true) {
                    event.cancel = false;
                }
                else {
                    event.cancel = true;
                }
            });
            return !reg.test(text);
        }).hide();
    });
});