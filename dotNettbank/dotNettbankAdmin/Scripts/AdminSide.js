$(document).ready(function () {
   
    $('.sidebar-menu').on('click', 'li', function () {
        $('.sidebar-menu li.active').removeClass('active');
        $(this).addClass('active');
    });


    $(".tag").click(function () {
        alert("Remove");
        $(this).remove();
    });

});


$(".getPartial").click(function () {
    var arg = $(this).data("id");
    $.ajax({
        type: 'POST',
        url: arg ,
        beforeSend: function () {
            $('#productsPlace').css('display', 'block');
            $('#productsPlace').animate({ opacity: 0 }, 0);
            $('#productsPlace').html("<img class='center-block' src='/Files/spin.svg' />").animate({ opacity: 1, top: "-10px" }, '500');
        },
    }).done(function (result) {
        $('#productsPlace').html(result);
    })
});



