$(function () {
    var page = window.location.pathname.substr(1);

    if (!sessionStorage.getItem('token') && ((page != 'login') && (page != 'reg'))) {
        navTo('login');
    }

    if (sessionStorage.getItem('token') && ((page == 'login') || (page == 'reg'))) {
        navTo('acc');
    }
});