var formularioMantenimiento = "AlertaForm";
var urlMantenimiento = baseUrlApiService + 'Alerta/';
$(document).ready(function () {

    webApp.validarNumerico(['CantidadAmarillo', 'CantidadRojo']);
 

    webApp.InicializarValidacion(formularioMantenimiento, 
        {
            CantidadAmarillo: {
                required: true,
                noPasteOnlyNumberAndMin0: true,
                
            },           
            CantidadRojo: {
                required: true,
                noPasteOnlyNumberAndMin0: true
            }               
        },
        {
            CantidadAmarillo: {
                required: "Por favor ingrese Cantidad Amarillo"
            },
            CantidadRojo: {
                required: "Por favor ingrese Cantidad Rojo"
            }                     
        });    

    $("#btnGuardarAlerta").on("click", function (e) { 
        if ($('#' + formularioMantenimiento).valid()) {
            var CantidadAmarillo= $("#CantidadAmarillo").val();
            var CantidadRojo = $("#CantidadRojo").val();
            if (parseInt(CantidadAmarillo) > parseInt(CantidadRojo))
            {
                webApp.showReConfirmDialog(function () {
                    checkSession(function () {
                        GuardarAlerta();
                    });
                });
            }
        else {
                webApp.showMessageDialog("Por favor la cantidad amarilla deber ser mayor a la cantidad roja.");
             }
        }
        e.preventDefault();
    });

    var regex4 = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;// Email address
    $('#Correos').tagsInput({
        width: 'auto',
        pattern: regex4,
        delimiter: ';',
        defaultText:'Correo Electr&oacute;nico',
        width:'400px',
        height:'200px',
    });    

	CargarAlerta();
});	


function CargarAlerta(){
    
    webApp.Ajax({
        url: urlMantenimiento + 'GetCount',
        async: false
       
    }, function(response){
        if(response.Success){            
            if(response.Warning){                           
                $.gritter.add({
                    title: 'Alerta',
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });                         
            }else{
                var alertaResult = response.Data;
                $("#AlertaId").val(alertaResult.Id);
                $("#CantidadAmarillo").val(alertaResult.CantidadAmarillo);
                $("#CantidadRojo").val(alertaResult.CantidadRojo);
                $.each(alertaResult.Correos.split(';'), function(index, item){
                    $('#Correos').addTag(item);
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

function GuardarAlerta(){
    var modelView = {
        Id: $("#AlertaId").val(),
        CantidadAmarillo: $("#CantidadAmarillo").val(),
        CantidadRojo: $("#CantidadRojo").val(),
        Correos: $("#Correos").val(),
        UsuarioRegistro: $("#usernameLogOn strong").text(),
    };
        webApp.Ajax({
            url: urlMantenimiento + 'Update',
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

