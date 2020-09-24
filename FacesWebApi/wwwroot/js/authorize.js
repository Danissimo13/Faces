$(function () {
    if (!localStorage.getItem('token') && (window.location.hash != '#login') && (window.location.hash != '#reg')) {
        window.location.hash = '#login';
    }

    if (localStorage.getItem('token') && ((window.location.hash == '#login') || (window.location.hash == '#reg'))) {
        window.location.hash = '#acc';
    }
});