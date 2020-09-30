$(function () {
    window.api_post = async function api_post(controller, body, succeed, error) {
        const apiUrl = 'api/' + controller;
        const response = await fetch(apiUrl, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body)
        });

        var data = {};

        try {
            data = await response.json();
        }
        catch(err) {
            data = { errorText: err };
        }

        if (response.ok == true) {
            succeed(data);
        }
        else {
            error(data);
        }
    }
});