$(document).ready(function () {
    $('.sidebar-menu').on('click', 'li', function () {
        $('.sidebar-menu li.active').removeClass('active');
        $(this).addClass('active');
    });

});
