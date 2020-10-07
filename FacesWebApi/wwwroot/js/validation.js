function displayModelErrors(modelState, infoBox) {
    var errors = '';

    try {
        if (modelState.errors) {
            for (var key of modelState.errors) {
                for (var error of key) {
                    console.error('Error: ' + error);
                    errors += error + '</br>';
                }
            }
        }
        else {
            for (var kay of modelState) {
                for (var error of key) {
                    console.error('Error: ' + error);
                    errors += error + '</br>';
                }
            }
        }

        if (infoBox && errors) {
            infoBox.removeClass('succeed');
            infoBox.addClass('error');
            infoBox.html(errors);
        }
    }
    catch (err) {
        console.error(err);
    }
}