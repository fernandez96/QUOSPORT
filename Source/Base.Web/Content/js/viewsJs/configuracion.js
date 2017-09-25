var formularioMantenimiento = "ConfiguracionForm";
var urlMantenimiento = baseUrlApiService + 'Configuracion/';

$(document).ready(function () {

    webApp.InicializarValidacion(formularioMantenimiento,
        {
            NumeroDiaCicloVida: {
                required: true,
                noPasteOnlyNumberAndMax365: true
            }
        },
        {
            NumeroDiaCicloVida: {
                required: "Por favor ingrese N&uacute;mero de D&iacute;as de Ciclo de Vida"
            }
        });

    $("#btnGuardarConfiguracion").on("click", function (e) {

        if ($('#' + formularioMantenimiento).valid()) {

            webApp.showReConfirmDialog(function () {
                GuardarConfiguracion();
            });
        }

        e.preventDefault();
    });

    CargarConfiguracion();   
});

function CargarConfiguracion() {

    webApp.Ajax({
        url: baseUrlApiService + 'Configuracion/GetActive',
        async: false,
    }, function (response) {
        if (response.Success) {

            if (response.Warning) {
                $.gritter.add({
                    title: 'Alerta',
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });
            } else {
                var configuracionResult = response.Data;
                $("#ConfiguracionId").val(configuracionResult.Id);
                $("#NumeroDiaCicloVida").val(configuracionResult.NumeroDiaCicloVida);
                $("#Estado").val(configuracionResult.Estado);
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

function GuardarConfiguracion() {

    var modelView = {
        Id: $("#ConfiguracionId").val(),
        NumeroDiaCicloVida: $("#NumeroDiaCicloVida").val(),
        Estado: $("#Estado").val(),
        UsuarioRegistro: $("#usernameLogOn strong").text()
    };

    if(modelView.Id == 0)
        action = 'Add';
    else
        action = 'Update';

    webApp.Ajax({
        url: urlMantenimiento + action,
        parametros: modelView
    }, function(response){        
        if (response.Success) {            
            if(response.Warning){                           
                $.gritter.add({
                    title: 'Alerta',
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });                         
            } else {
                $.gritter.add({
                    title: 'Registro Satisfactorio',
                    text: response.Message,
                    class_name: 'gritter-success gritter'
                });
            }
        }else{
            $.gritter.add({
                title: 'Error',
                text: response.Message,
                class_name: 'gritter-error gritter'
            });                     
        }
    }, function(response){
        $.gritter.add({
            title: 'Error',
            text: response,
            class_name: 'gritter-error gritter'
        });
    }, function(XMLHttpRequest, textStatus, errorThrown){
        $.gritter.add({
            title: 'Error',
            text: "Status: " + textStatus + "<br/>Error: " + errorThrown,
            class_name: 'gritter-error gritter'
        });
    });
} 