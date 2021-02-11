$(function () {
    if (!sessionStorage.getItem('token')) {
        navTo('login');
        return;
    }
    if (parseJwt(sessionStorage.getItem('token')).Role != 'admin') {
        navTo('acc');
        return;
    }

    $('.action form').slideUp();
    $('.action h2').on('click', function () {
        $(this).next('form').slideToggle();
    });

    $('#news-form').on('submit', function (e) {
        e.preventDefault();

        var body = new FormData(this);
        api_post_form('news', body, succeed_news_req, error_news_req);
    });

    $('#admin-page').css('display', 'flex');

    function succeed_news_req(data) {
        var url = 'news?id=' + data;
        navTo(url)
    }

    function error_news_req(data) {
        displayModelErrors(data, $('#news-error'));
    }
});