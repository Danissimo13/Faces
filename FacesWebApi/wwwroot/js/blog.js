$(function () {
    const newsOnPage = 6;

    const page_controller = {
        prev: $('#prev-page'),
        next: $('#next-page')
    }

    const url = new window.URL(location.href);
    var page = url.searchParams.get('page');
    if (!page) page = 0;

    if (page < 0) notFound();

    if (page > 0) page_controller.prev.attr('href', 'blog?page=' + (parseInt(page) - 1));
    else page_controller.prev.remove();

    var searchOptions = [
        { name: 'from', value: page * newsOnPage },
        { name: 'count', value: newsOnPage }
    ];
    api_get('news', null, searchOptions, succeed_req, error_req);

    function succeed_req(data) {
        if (data.length == newsOnPage) page_controller.next.attr('href', 'blog?page=' + (parseInt(page) + 1));
        else page_controller.next.remove();

        if (data.length < 1) notFound();

        for (var news of data) {
            var newsHtml = '<a class="news" link="ajax" href="news?id=' + news.id + '"></a>'
            var topicHtml = '<h2 class="topic">' + news.topic + '</h2>'
            var imgHtml = '<img class="news-img inverted" src="' + news.imageSrc + '"/>' 
            var publishHtml = '<h3 class="publish-date">' + news.publishDate + '</h3>';

            var newsElement = $(newsHtml);
            var topicElement = $(topicHtml);
            var imtElement = $(imgHtml);
            var publishElement = $(publishHtml);

            newsElement.append(topicElement);
            newsElement.append(imtElement);
            newsElement.append(publishElement);

            $('#news-list').append(newsElement);
        }

        $('#blog-page').css('display', 'flex');
    }

    function error_req(data) {
        displayModelErrors(data);
        notFound();
    }
});