function checkSession(callback) {
    webApp.Ajax({
        url: baseUrl + "Account/VerifySession",
        async: false
    }, function (response) {
        if (response.Success) {
            if (response.Warning) {
                redireccionarLogin(response.Title, response.Message);
            } else {
                if (callback != null && typeof (callback) == "function")
                    callback();
            }
        } else {
            $.gritter.add({
                title: 'Error',
                text: response.Message,
                class_name: 'gritter-error gritter'
            });
        }
    }, function (response) {
        $.gritter.add({
            title: 'Error',
            text: response,
            class_name: 'gritter-error gritter'
        });
    }, function (XMLHttpRequest, textStatus, errorThrown) {
        $.gritter.add({
            title: 'Error',
            text: "Status: " + textStatus + "<br/>Error: " + errorThrown,
            class_name: 'gritter-error gritter'
        });
    });
}

function redireccionarLogin(titulo, mensaje) {
    $.gritter.add({
        title: titulo,
        text: mensaje,
        class_name: 'gritter-warning gritter'
    });

    setTimeout(function () {

        $.blockUI({
            message: "Redireccionando a Login",
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: 1,
                color: '#fff'
            },
            onBlock: function () {
                pageBlocked = true;
            }
        });
    }, 5);

    setTimeout(function () {
        var urlLogin = baseUrl + 'Account/Login';
        window.location.href = urlLogin;
    }, 1500);
}