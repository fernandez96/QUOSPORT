var formularioMantenimiento = "parametroForm";
var urlMantenimiento = baseUrl + 'Parametro/';


$(document).ready(function () {
    checkSession(function () {
        CargarRol();
    });

    
    
    //$("#igv").inputmask('Regex', {
    //    regex: "^[0-9]{1,6}(\\.\\d{1,2})?$"
    //});
   
    webApp.InicializarValidacion(formularioMantenimiento,
        {
            Empresa: {
                required: true
            },
            Direccion: {
                required: true
            },
            ruc: {
                required: true,
                
            },
            igv: {
                required: true,
                
            }
        },
        {
            Empresa: {
                required: "Por favor ingrese Empresa",

            },
            Direccion: {
                required: "Por favor ingrese Dirección"
            },
            ruc: {
                required: "Por favor ingrese RUC"
            },
            igv: {
                required: "Por favor ingrese IGV",
                
            }
        });

    $("#btnGuardarParametros").on('click', function () {
        if ($('#' + formularioMantenimiento).valid()) {

            webApp.showReConfirmDialog(function () {
                checkSession(function () {
                    GuardarParametro();
                });
            });
        }

        e.preventDefault();
    });
   
});
function GuardarParametro() {

    var modelView = {
        Id: $("#ParametroId").val(),
        empresa: $("#Empresa").val(),
        direccion: $("#Direccion").val(),
        ruc: $("#ruc").val(),
        igv: $("#igv").val(),
        UsuarioRegistro: $("#usernameLogOn strong").text()
    };
    if (modelView.Id == 0)
        action = 'Add';
    else
        action = 'Update';

    webApp.Ajax({
        url: urlMantenimiento + action,
        parametros: modelView
    }, function (response) {
        if (response.Success) {
            if (response.Warning) {
                $.gritter.add({
                    title: response.Title,
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });
            } else {
                $.gritter.add({
                    title: response.Title,
                    text: response.Message,
                    class_name: 'gritter-success gritter'
                });
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

function CargarRol() {

    webApp.Ajax({
        url: urlMantenimiento + 'GetAll',
        async: false,
    }, function (response) {
        if (response.Success) {

            if (response.Warning) {
                //$.gritter.add({
                //    title: 'Alerta',
                //    text: response.Message,
                //    class_name: 'gritter-warning gritter'
                //});
            } else {
                LimpiarFormulario();
                var parametro = response.Data;

                $("#Empresa").val(parametro.empresa);
                $("#Direccion").val(parametro.direccion);
                $("#ruc").val(parametro.ruc);
                $("#igv").val(parseFloat(parametro.igv)+'.00');
                $("#ParametroId").val(parametro.Id);
         
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

function LimpiarFormulario() {
    webApp.clearForm(formularioMantenimiento);
}