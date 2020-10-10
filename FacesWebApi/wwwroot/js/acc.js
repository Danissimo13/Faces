$(function () {
    const user_id = $('#acc-page #user-id');
    const user_name = $('#acc-page #user-name');
    const user_email = $('#acc-page #user-email');
    const user_role = $('#acc-page #user-role');
    const user_last_request = $('#acc-page #last-request');
    const user_change_name = $('#acc-page #change-nickname');
    const user_change_email = $('#acc-page #change-email');
    const user_change_password = $('#acc-page #change-password');
    const user_change_message = $('#acc-page #change-message');

    const payload = parseJwt(sessionStorage.getItem('token'));
    const url = new window.URL(location.href);
    var id = url.searchParams.get('id');

    var query_data = [
        { name: 'WithRole', value: true },
        { name: 'WithRequests', value: true },
        { name: 'WithRequestImages', value: true },
        { name: 'WithRequestResponses', value: true },
        { name: 'WithResponseImages', value: true },
        { name: 'FromRequest', value: 0 },
        { name: 'RequsetsCount', value: 1 },
    ];
    api_get('user', id, query_data, succeed_req, error_req);

    function succeed_req(data) {
        user_id.text(data.userId);
        user_name.text(data.nickname);
        user_email.text(data.email);
        user_role.text(data.role);
        $('#information').css('display', 'flex');

        if (data.requests && data.requests[0]) {
            $('#last-request #request-type').text(data.requests[0].requestType);
            $('#last-request #request-id').text(data.requests[0].requestId);
            if (data.requests[0].images) {
                $('#last-request #request-img').attr('src', data.requests[0].images[0].imageSrc);
            }

            if (data.requests[0].response && data.requests[0].response.images) {
                $('#last-request #response-img').attr('src', data.requests[0].response.images[0].imageSrc);
            }

            user_last_request.css('display', 'flex');
        }
        else {
            user_last_request.remove();
        }

        if (payload.Id == data.userId) {
            user_change_name.val(data.nickname);
            user_change_email.val(data.email);

            $('#change-user h3').on('click', () => $('#change-form').slideToggle());
            $('#change-form input[type="submit"]').on('click', function (e) {
                e.preventDefault();

                var isValid = validate_change_form();
                if (isValid == false) return;

                var changedData = {
                    nickname: user_change_name.val(),
                    email: user_change_email.val(),
                    password: user_change_password.val()
                };

                api_put('user', payload.Id, changedData, succeed_change_req, error_change_req);
            });
            $('#change-form').slideUp(0, () => $('#change-user').css('display', 'flex'));

            $('#delete').on('click', function (e) {
                e.stopPropagation();
                $('#delete-confirmation').css('display', 'flex');
            });
            $('#delete-cancel').on('click', function (e) {
                e.stopPropagation();
                $('#delete-confirmation').css('display', 'none');
            });
            $('#delete-agree').on('click', function () {
                api_delete('user', payload.Id, succeed_delete_req, error_delete_req);
            });

            $('#delete').css('display', 'flex');

            $('#exit').on('click', function () {
                sessionStorage.removeItem('token');
                navTo('login');
            });
            $('#exit').css('display', 'flex');
        }
        else {
            $('#change-user').remove();
            $('#delete').remove();
            $('#exit').remove();
        }
    }

    function error_req(data) {
        notFound();
    }

    function succeed_change_req(data) {
        if (data.logout) {
            sessionStorage.removeItem('token');
            navTo('login');
        }
        else {
            $('#change-message').removeClass('error');
            $('#change-message').addClass('succeed');
            $('#change-message').text('Data successfully updated.')

            user_name.text(data.nickname);
            user_email.text(data.email);
        }
    }

    function error_change_req(data) {
        displayModelErrors(data, user_change_message);
    }

    function succeed_delete_req(data) {
        sessionStorage.removeItem('token');
        navTo('login');
    }

    function error_delete_req(data) {
        displayModelErrors(data);
    }

    function validate_change_form() {
        if (user_change_email.val()) user_change_email.removeClass('non-valid');
        else user_change_email.addClass('non-valid');

        if (user_change_name.val()) user_change_name.removeClass('non-valid');
        else user_change_name.addClass('non-valid');

        if (!user_change_email.val() || !user_change_name.val()) return false;
        return true;
    }
});