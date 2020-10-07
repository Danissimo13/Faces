$(function () {
    api_get('news', null, null, succeed_news_request, error_news_request)

    function succeed_news_request(data) {
        var newsHref = 'news?id=' + data.id;
        $('#last-news').attr('href', newsHref);
        $('#last-news-topic').text(data.topic);
        $('#news-publish-date').text(data.publishDate);
        $('#last-news img').attr('src', data.imageSrc);
        $('#last-news').css('display', 'flex');
    }

    function error_news_request(data) {
        displayModelErrors(data);
        $('#last-news').remove();
    }
});