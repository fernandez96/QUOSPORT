//jQuery(document).ready(function () {
//    var alertaHub = $.connection.alertaHub;

//    alertaHub.client.hayAlertasPendientes = function (response) {
//        if (response.Success) {

//            if (response.Warning) {
//                $.gritter.add({
//                    title: 'Alerta',
//                    text: response.Message,
//                    class_name: 'gritter-warning gritter'
//                });
//            } else {
//                var alertaResult = response.Data;

//                $("#contenedorAlerta").remove();
//                $('#listaAlerta li:eq(0)').before('<li id="contenedorAlerta" class="' + alertaResult.Background + '"><a data-toggle="dropdown" class="dropdown-toggle ' + alertaResult.Background + '" href="javascript:void(0)" aria-expanded="false"><i class="ace-icon fa fa-bell icon-animated-bell"></i>        <span id="cantidadGiftCard" class="badge badge-black">' +alertaResult.Cantidad + '</span> </a></li>');
//            }
//        } else {
//            $.gritter.add({
//                title: 'Error',
//                text: response.Message,
//                class_name: 'gritter-error gritter'
//            });
//        }
//    };

//    alertaHub.client.notificarCambioEnData = function () {
//        alertaHub.server.hayAlertasPendientes();
//    };

//    $.connection.hub.start().done(function () {
//        alertaHub.server.hayAlertasPendientes();
//    });
//});

//function NotificarAlerta() {
//    pageBlocked = true;
//    webApp.Ajax({
//        url: baseUrl + 'Alerta/Notificar'

//    }, function (response) {
//    }, function (response) {
//    }, function (XMLHttpRequest, textStatus, errorThrown) {
//    });
//}

//function EnviarCorreoTope() {
//    pageBlocked = true;
//    webApp.Ajax({
//        url: baseUrlApiService + 'Alerta/SendEmail',
//        async: true
//    }, function (response) {
//    }, function (response) {
//    }, function (XMLHttpRequest, textStatus, errorThrown) {
//    });
//}