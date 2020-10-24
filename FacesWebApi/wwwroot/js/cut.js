$(function () {
    $('#cut-form').on('submit', function (e) {
        e.preventDefault();

        var formData = new FormData(this);

        var isValid = validate_form();
        if (!isValid) return;

        $('#cut-form').css('display', 'none');
        $('#load').css('display', 'block');
        api_post_form('request', formData, succeed_req, error_req);

        function validate_form() {
            if (!$('#from-img').val() || !$('#to-img').val()) {
                $('#cut-form #error').text('You didnt choose images.');
                return false;
            }
            else {
                $('#cut-form #error').text('');
                return true;
            }
        }

        function succeed_req(data) {
            var page = 'request?id=' + data.id;
            navTo(page);
        }

        function error_req(data) {
            $('#load').css('display', 'none');
            $('#cut-form').css('display', 'flex');

            var swap_error = $('#cut-form #error');
            displayModelErrors(data, swap_error);
        }
    });
});