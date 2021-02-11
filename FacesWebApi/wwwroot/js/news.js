$(function () {
    const news_topic = $('#news-topic');
    const news_image = $('#news-img');
    const news_body = $('#news-body');
    const news_publish_date = $('#news-publish-date');

    const url = new window.URL(location.href);
    const id = url.searchParams.get('id');
    api_get('news', id, [], succeed_req, error_req);

    function succeed_req(data) {
        news_topic.text(data.topic);
        news_image.attr('src', data.imageSrc);
        news_body.text(data.body);
        news_publish_date.text(data.publishDate);

        $('#news-page').css('display', 'flex');
    }

    function error_req(data) {
        displayModelErrors(data);
        notFound();
    }
});