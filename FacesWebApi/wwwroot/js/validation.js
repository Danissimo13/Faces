function displayModelErrors(modelState, infoBox) {
    var errors = '';

    try {
        if (modelState.errors) {
            for (var key in modelState.errors) {
                for (var error in modelState.errors[key]) {
                    console.error('Error: ' + modelState.errors[key][error]);
                    errors += modelState.errors[key][error] + '</br>';
                }
            }
        }
        else {
            for (var key in modelState) {
                for (var error in modelState[key]) {
                    console.error('Error: ' + modelState[key][error]);
                    errors += modelState[key][error] + '</br>';
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
        console.error(modelState);
    }
}