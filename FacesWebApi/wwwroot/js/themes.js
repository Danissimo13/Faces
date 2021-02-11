$(function () {
    if ($.cookie('theme')) {
        var selector = '.theme#' + $.cookie('theme');
        $(selector).addClass('active');
    }

    $('.theme').on('click', function () {
        const theme = $(this).attr('id');

        $("#theme-switcher").addClass('active');

        $(document.documentElement).attr('theme', theme);
        $.cookie('theme', theme);

        $('.theme').removeClass('active');
        $(this).addClass('active');
    });
});