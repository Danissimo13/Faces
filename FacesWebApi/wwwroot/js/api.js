$(function () {
    window.api_post = async function api_post(controller, body, succeed, error) {
        const apiUrl = 'api/' + controller;
        const response = await fetch(apiUrl, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body)
        });

        const data = await response.json();

        if (response.ok == true) {
            succeed(data);
        }
        else {
            error(data);
        }
    }
});