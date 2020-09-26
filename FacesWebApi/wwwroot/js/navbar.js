$(function () {
    var isHide = true;

    $('#nav-switcher').on('click', async function () {
        if (isHide) {
            $('#references').removeClass('nav-hidden');
            isHide = false;
        }
        else {
            $('#references').addClass('nav-hidden');
            isHide = true;
        }
    });
});