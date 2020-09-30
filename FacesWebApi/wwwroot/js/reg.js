$(function () {
    const nick_inp = $('#reg-form #nick');
    const email_inp = $('#reg-form #email');
    const password_inp = $('#reg-form #password');

    const reg_message = $('#reg-form #reg-message');

    $('#reg-btn').on('click', async function (e) {
        e.preventDefault();

        var isValid = validate_reg_form();
        if (!isValid) return;

        const request_data = {
            nickname: nick_inp.val(),
            email: email_inp.val(),
            password: password_inp.val()
        }

        await api_post('user', request_data, succeed_req, error_req);
    });

    function succeed_req(data) {
        reg_message.text(data.message);
        reg_message.removeClass('error');
        reg_message.addClass('succeed');
    }

    function error_req(data) {
        console.log("Error: ", data.errorText);

        reg_message.text(data.errorText);
        reg_message.removeClass('succeed');
        reg_message.addClass('error');
    }

    function validate_reg_form() {
        if (!nick_inp.val()) nick_inp.addClass('non-valid');
        else nick_inp.removeClass('non-valid');

        if (!email_inp.val()) email_inp.addClass('non-valid');
        else email_inp.removeClass('non-valid');

        if (!password_inp.val()) password_inp.addClass('non-valid');
        else password_inp.removeClass('non-valid');

        if (nick_inp.hasClass('non-valid') ||
            email_inp.hasClass('non-valid') ||
            password_inp.hasClass('non-valid')) return false;

        return true;
    }
});