$(function () {
    window.api_post_form = async function api_post(controller, body, succeed, error) {
        const apiUrl = 'api/' + controller;

        $.ajax({
            type: 'POST',
            url: apiUrl,
            contentType: false,
            processData: false,
            data: body,
            xhrFields: {
                withCredentials: true,
            },
            beforeSend: function (xhr) {
                var token = getAuthToken();
                xhr.setRequestHeader('Authorization', token);
            },
            success: succeed,
            error: (xhr) => error(JSON.parse(xhr.responseText))
        });
    }

    window.api_post = async function api_post(controller, body, succeed, error) {
        const apiUrl = 'api/' + controller;

        const response = await fetch(apiUrl, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': getAuthToken()
            },
            body: JSON.stringify(body)
        });

        await doResponseActions(response, succeed, error);
    }

    window.api_get = async function api_get(controller, param, args, succeed, error) {
        var apiUrl = 'api/' + controller;

        if (param) {
            apiUrl += '/' + param;
        }

        if (args) {
            apiUrl += '?';
            for (var arg of args) {
                apiUrl += arg.name + '=' + arg.value + '&';
            }
        }

        console.log(apiUrl);

        const response = await fetch(apiUrl, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': getAuthToken()
            },
        });

        await doResponseActions(response, succeed, error);
    }

    window.api_put = async function api_put(controller, param, body, succeed, error) {
        var apiUrl = 'api/' + controller;

        if (param) {
            apiUrl += '/' + param;
        }

        const response = await fetch(apiUrl, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': getAuthToken()
            },
            body: JSON.stringify(body)
        });

        await doResponseActions(response, succeed, error);
    }

    window.api_delete = async function api_delete(controller, param, succeed, error) {
        var apiUrl = 'api/' + controller;
        if (param) {
            apiUrl += '/' + param;
        }

        const response = await fetch(apiUrl, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': getAuthToken()
            },
        });

        await doResponseActions(response, succeed, error);
    }

    function getAuthToken() {
        return "Bearer " + sessionStorage.getItem('token');
    }

    async function doResponseActions(response, succeed, error) {
        var data;
        try {
            data = await response.json();
        }
        catch (err) {
            data = { errorText: err };
        }

        if (response.ok == true) {
            succeed(data);
        }
        else {
            if (response.status == 401) {
                sessionStorage.removeItem('token');
                navTo('login');
            }
            else {
                error(data);
            }
        }
    }
});