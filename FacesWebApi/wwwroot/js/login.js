$(function () {
    const login_inp = $('#login-form #email');
    const password_inp = $('#login-form #password');

    const login_message = $('#login-form #login-message');

    $('#login-btn').on('click', async function (e) {
        e.preventDefault();

        var isValid = validate_login_form();
        if (isValid == false) return;

        const request_data = {
            email: login_inp.val(),
            password: password_inp.val()
        };

        await api_post('login', request_data, succeed_req, error_req);
    });

    function succeed_req(data) {
        localStorage.setItem('token', data.access_token);

        login_message.text(data.message);
        login_message.removeClass('error');
        login_message.addClass('succeed');

        setTimeout(() => window.location.hash = 'acc', 2000);
    }

    function error_req(data) {
        console.log("Error: ", data.errorText);

        login_message.text(data.errorText);
        login_message.removeClass('succeed');
        login_message.addClass('error');
    }

    function validate_login_form() {
        if (!login_inp.val()) login_inp.addClass('non-valid');
        else login_inp.removeClass('non-valid');

        if (!password_inp.val()) password_inp.addClass('non-valid');
        else password_inp.removeClass('non-valid');

        if (login_inp.hasClass('non-valid') || password_inp.hasClass('non-valid')) return false;

        return true;
    }
});