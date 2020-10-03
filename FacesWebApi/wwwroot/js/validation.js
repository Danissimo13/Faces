function displayModelErrors(modelState, infoBox) {
    var errors = '';
    for (var key in modelState.errors) {
        for (var error of modelState.errors[key]) {
            console.error('Error: ' + error);
            errors += error + '</br>';
        }
    }

    if (infoBox && errors) {
        infoBox.removeClass('succeed');
        infoBox.addClass('error');
        infoBox.html(errors);
    }
}