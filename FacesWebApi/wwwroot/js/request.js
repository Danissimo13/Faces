$(function () {
    var request_id = $('#request-id');
    var request_type = $('#request-type');
    var request_images = $('#request-images');
    var response_images = $('#response-images');
    var user_ref = $('#user-reference');

    const payload = parseJwt(sessionStorage.getItem('token'));
    const url = new window.URL(location.href);
    var id = url.searchParams.get('id');

    api_get('request', id, [], succeed_req, error_req)

    function succeed_req(data) {
        request_id.text(data.request.requestId);
        request_type.text(data.request.requestType);
        for (var reqImage of data.request.images) {
            var imgHtml = '<img class="inverted" src="' + reqImage.imageSrc + '" onclick="scope(this)" />';
            var newImg = $(imgHtml);
            request_images.append(newImg);
        }

        if (data.response) {
            for (var resImage of data.response.images) {
                var imgHtml = '<img class="inverted" src="' + resImage.imageSrc + '" onclick="scope(this)" />';
                var newImg = $(imgHtml);
                response_images.append(newImg);
            }
        }
        else {
            $('#response').remove();
        }

        if (data.user) {
            user_ref.attr('href', 'acc?id=' + data.user.userId);
            user_ref.text(data.user.username);
        }
        else {
            console.log('hello');
            $('#user').remove();
        }
    }

    function error_req(data) {
        notFound();
    }
});