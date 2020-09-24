$(function () {
    const login_inp = $('#login-form #email');
    const password_inp = $('#login-form #password');
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
        window.location.hash = '#acc';
    }

    function error_req(data) {
        console.log("Error: ", data.errorText);
        $('#login-form #login-validate').text(data.errorText);
    }

    function validate_login_form() {
        if (!login_inp.val()) {
            login_inp.css('border-color', 'var(--danger-color)');
            return false;
        }
        else {
            login_inp.css('border-color', 'var(--violet-color)');
        }

        if (!password_inp.val()) {
            password_inp.css('border-color', 'var(--danger-color)');
            return false;
        }
        else {
            password_inp.css('border-color', 'var(--violet-color)');
        }

        return true;
    }
});