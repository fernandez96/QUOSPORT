var AccountLogin = function () {

    var eventos = function () {
        $('#frmLogin').on('submit', function (e) {
            if ($('#Username').val() == "") {
                $.gritter.add({
                    title: 'Alerta',
                    text: "Ingrese Username",
                    class_name: 'gritter-warning'
                });
                e.preventDefault();
            } else if ($('#Password').val() == "") {
                $.gritter.add({
                    title: 'Alerta',
                    text: "Ingrese Password",
                    class_name: 'gritter-warning'
                });
                e.preventDefault();
            } else {
                
                webApp.showLoading();
                
            }
            return true;
        });
    };
    return {
        //main function to initiate page
        init: function () {
            eventos();
        }
    };
}();

$(function () { AccountLogin.init(); });

//Logout
$("#Logout").on("click", function () {
    var urlLogout = baseUrl + 'Account/Logout';
    webApp.showConfirmResumeDialog(function () {
        window.location.href = urlLogout;
    }, '¿Está seguro de salir del sistema?');

});