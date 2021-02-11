$(function () {
    var searchOptions = [
        { name: "from", value: 0 },
        { name: "count", value: 1 }
    ]
    api_get('news', null, searchOptions, succeed_news_request, error_news_request);

    function succeed_news_request(data) {
        if (data && data[0]) {
            var newsHref = 'news?id=' + data[0].id;
            $('#last-news').attr('href', newsHref);
            $('#last-news-topic').text(data[0].topic);
            $('#news-publish-date').text(data[0].publishDate);
            $('#last-news img').attr('src', data[0].imageSrc);
            $('#last-news').css('display', 'flex');
        }
        else {
            $('#last-news').remove();
        }
    }

    function error_news_request(data) {
        displayModelErrors(data);
        $('#last-news').remove();
    }
});