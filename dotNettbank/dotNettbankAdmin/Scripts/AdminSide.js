﻿$(document).ready(function () {
   
    $('.sidebar-menu').on('click', 'li', function () {
        $('.sidebar-menu li.active').removeClass('active');
        $(this).addClass('active');
    });

    /*
    $(".tag").click(function () {
        alert("Remove");
        $(this).remove();
    });
*/
    // When Sidebar toggle button is clicked: check if body has sidebar-collapse (same as sidebar being collapsed
    // If so, show the tabs menu, and then hide when closing
    $(".sidebar-toggle").click(function () {
        if ($('body').hasClass('sidebar-collapse')) {
            $("#tabsmenu").show();
        } else {
            $("#tabsmenu").hide();
        }
    });

});


$(".getPartial").click(function () {
    var arg = $(this).data("id");
    getPartial(arg);
});

function getPartial(viewName) {
    birthTag = $("#tabsmenu").find("[data-type='customer']");
    //var birthNo = birthTag.data('id');
    // Get array of birth numbers from tags:
    var birthNoArray = birthTag.map(function () {
        return this.getAttribute("data-id");
    }).get();

    var accountTag = $("#tabsmenu").find("[data-type='account']");
    //var accountNo = accountTag.data('id');
    var accountNoArray = accountTag.map(function () {
        return this.getAttribute("data-id");
    }).get();

    $.ajax({
        type: 'POST',
        url: viewName,
        data: { birthNo: birthNoArray, accountNo: accountNoArray },
        beforeSend: function () {
            $('#productsPlace').css('display', 'block');
            $('#productsPlace').animate({ opacity: 0 }, 0);
            $('#productsPlace').html("<img class='center-block' src='/Files/spin.svg' />").animate({ opacity: 1, top: "-10px" }, '500');
        },
    }).done(function (result) {
        $('#productsPlace').html(result);
    })
}




