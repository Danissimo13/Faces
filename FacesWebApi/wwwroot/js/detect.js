$(function () {
    $('#detect-form').on('submit', function (e) {
        e.preventDefault();

        var formData = new FormData(this);

        var isValid = validate_form();
        if (!isValid) return;

        $('#detect-form').css('display', 'none');
        $('#load').css('display', 'block');
        api_post_form('request', formData, succeed_req, error_req);

        function validate_form() {
            if (!$('#from-img').val()) {
                $('#detect-form #error').text('You didnt choose images.');
                return false;
            }
            else {
                $('#detect-form #error').text('');
                return true;
            }
        }

        function succeed_req(data) {
            var page = 'request?id=' + data.id;
            navTo(page);
        }

        function error_req(data) {
            $('#load').css('display', 'none');
            $('#detect-form').css('display', 'flex');

            var swap_error = $('#detect-form #error');
            displayModelErrors(data, swap_error);
        }
    });
});