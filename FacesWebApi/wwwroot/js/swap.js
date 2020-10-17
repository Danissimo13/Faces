$(function () {
    $('#swap-form').on('submit', function (e) {
        e.preventDefault();

        var formData = new FormData(document.getElementById('swap-form'));

        var isValid = validate_form();
        if (!isValid) return;

        $('#swap-form').css('display', 'none');
        $('#load').css('display', 'block');
        api_post_form('request', formData, succeed_req, error_req);

        function validate_form() {
            if (!$('#from-img').val() || !$('#to-img').val()) {
                $('#swap-form #error').text('You didnt choose images.');
                return false;
            }
            else {
                $('#swap-form #error').text('');
                return true;
            }
        }

        function succeed_req(data) {
            var page = 'request?id=' + data.id;
            navTo(page);
        }

        function error_req(data) {
            $('#load').css('display', 'none');
            $('#swap-form').css('display', 'flex');

            var swap_error = $('#swap-form #error');
            displayModelErrors(data, swap_error);
        }
    });
});