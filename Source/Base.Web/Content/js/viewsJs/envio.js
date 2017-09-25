var dataTableEnvio = null;
var formularioMantenimiento = "EnvioForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'Envio/Listar';
var urlMantenimiento = baseUrlApiService + 'Envio/';
var rowEnvio = null;
$(document).ready(function () { 
    $.extend($.fn.dataTable.defaults, {
        language: { url: baseUrl + 'Content/js/dataTables/Internationalisation/es.txt' },
        responsive: true,
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "bProcessing": true,
        "dom": 'fltip'
    });    
    checkSession(function () {
        VisualizarDataTableEnvio();
    });

    $('body').on('click', 'button.btnBuscarEnvio', function() {
        LimpiarFormularioSearch();

        $("#BuscarEnvio").modal("show");
    });

    //or change it into a date range picker
    $('.input-daterange').datepicker({
        autoclose:true, 
        language: 'es',
        format: 'dd-mm-yyyy',
    });

    $('body').on('click', 'a.editarEnvio', function () {
        rowEnvio = this;
        checkSession(function () {
            GetEnvioById();
        });
    });    

    webApp.validarNumerico(['NumeroDocumentoSearch']);
    webApp.InicializarValidacion('EnvioSearchForm', 
        {
            CorreoElectronicoSearch: {
                email:true
            },
            NumeroDocumentoSearch:{
                noPasteAllowNumber: true
            }
        
        },
        {            
            CorreoElectronicoSearch: {
                email: "Por favor ingrese correo válido."
            }                      
        }); 

    webApp.InicializarValidacion('DetalleEnvioForm',
        {
            CorreoElectronicoDetalle: {
                required: true,
                email:true
            }                             
        },
        {            
            CorreoElectronicoDetalle: {
                required: "Por favor ingrese correo.",
                email: "Por favor ingrese correo válido."
            }                      
        });    

    $("#btnSearchEnvio").on("click", function (e) { 

        if ($('#EnvioSearchForm').valid()) {
            checkSession(function () {
                dataTableEnvio.fnReloadAjax();
                LimpiarFormularioSearch();
                $("#BuscarEnvio").modal("hide");
            });
        }
        e.preventDefault();
    }); 

    $("#btnDetalleEnvio").on("click", function (e) { 
        if ($('#DetalleEnvioForm').valid()) {
            checkSession(function () {
                ReenviarCorreo($('#EnvioId').val(), $('#CorreoElectronicoDetalle').val(), $("#usernameLogOn strong").text());
            });        
        }
        e.preventDefault();
    });

});

function VisualizarDataTableEnvio()
{
    dataTableEnvio = $('#EnvioDataTable').dataTable({
        "bFilter": false,
        "bProcessing": true,
        "serverSide": true,
        //"scrollY": "350px",              
        "ajax": {
            "url": urlListar,
            "type": "POST",
            "data": function (request) {
                request.filter = new Object();
                request.filter.NumeroDocumentoSearch = $("#NumeroDocumentoSearch").val();
                request.filter.CorreoElectronicoSearch = $("#CorreoElectronicoSearch").val();
                request.filter.FechaEnvioInicialSearch = $("#FechaEnvioInicialSearch").val();
                request.filter.FechaEnvioFinalSearch = $("#FechaEnvioFinalSearch").val();
            },
            dataFilter: function (data) {
                if (data.substring(0, 9) == "<!DOCTYPE") {
                    redireccionarLogin("Sesión Terminada", "Se terminó la sesión");
                } else {
                    return data;
                    //var json = jQuery.parseJSON(data);
                    //return JSON.stringify(json); // return JSON string
                }
            }
        },
        "bAutoWidth": false,
        "columns": [
            {
                "data": function (obj) {

                    return '<div class="action-buttons">\
                    <a class="blue editarEnvio" href="javascript:void(0)"><i class="ace-icon fa fa-envelope bigger-130"></i></a>\
                    </div>';
                }
            },
            { "data": "Id" },
            {
                "data": function (obj) {
                    return GetTipoPersona(obj.TipoPersona);
                }
            },
            { "data": "NombreCompleto" },
            { "data": "NumeroDocumento" },
            { "data": "CorreoElectronico" },
            {
                "data": function (obj) {
                    return GetEstadoEnvio(obj.EstadoEnvio);
                }
            },
            { "data": "NumeroEnvios" },
            {
                "data": function (obj) {
                    return GetFechaSubString(obj.FechaHoraEnvio);
                }
            },
            {
                "data": function (obj) {
                    return GetFechaSubString(obj.FechaHoraReEnvio);
                }
            }
        ],
        "aoColumnDefs": [
            { "bSortable": false, "sClass": "center", "aTargets": [0], "width": "5%" },
            { "bVisible": false, "aTargets": [1] },
            { "bSortable": false, "aTargets": [2], "width": "8%" },
            { "bSortable": false, "className": "hidden-480", "aTargets": [3], "width": "20%" },
            { "className": "hidden-480", "aTargets": [4], "width": "10%" },
            { "className": "hidden-480", "aTargets": [5], "width": "20%" },
            { "bSortable": false, "className": "hidden-480", "aTargets": [6], "width": "8%" },
            { "bSortable": false, "aTargets": [7], "width": "5%" },
            { "bSortable": false, "aTargets": [8], "width": "10%" },
            { "bSortable": false, "aTargets": [9], "width": "10%" }
        ],
        "order": [[1, "desc"]]
    });
}

function LimpiarFormularioSearch(){
    webApp.clearForm('EnvioSearchForm');
    $("#NumeroDocumentoSearch").focus();
}

function LimpiarFormularioDetalle(){
    webApp.clearForm('DetalleEnvioForm');
}

function GetEstadoEnvio(estadoEnvio){
    var respuesta = "";
    switch(estadoEnvio) {
        case 1:
            respuesta = "Enviado";
            break;
        case 2:
            respuesta = "No Enviado";
            break;
        case 3:
            respuesta = "Reenviado";
            break;
        default:
            respuesta = "";
    }    

    return respuesta;
}

function GetTipoPersona(tipoPersona){
    var respuesta = "";
    switch(tipoPersona) {
        case 0:
            respuesta = "Titular";
            break;
        case 1:
            respuesta = "Beneficiario";
            break;
        default:
            respuesta = "";
    }    

    return respuesta;
}

function GetFechaSubString(fecha){
    var respuesta = "";
    if(fecha!= null && fecha.trim()!="")
        respuesta = fecha.substring(0,10);
    return respuesta;
}

function GetEnvioById()
{
    var aPos = dataTableEnvio.fnGetPosition(rowEnvio.parentNode.parentNode);
    var aData = dataTableEnvio.fnGetData(aPos[0]);
    var rowID = aData.Id;

    var modelView = {
        Id: aData.Id
    };

    webApp.Ajax({
        url: urlMantenimiento + 'GetById',
        parametros: modelView

    }, function (response) {

        if (response.Success) {
            if (response.Warning) {
                $.gritter.add({
                    title: 'Alerta',
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });
            } else {
                LimpiarFormularioDetalle();
                var envio = response.Data;
                $("#DetalleEnvio").modal("show");
                $("#EnvioId").val(envio.Id);
                $("#TipoPersonaDetalle").val(GetTipoPersona(envio.TipoPersona));
                $("#NombreCompletoDetalle").val(envio.NombreCompleto);
                $("#NumeroDocumentoDetalle").val(envio.NumeroDocumento);
                $("#CorreoElectronicoDetalle").val(envio.CorreoElectronico);

                $("#EstadoEnvioDetalle").val(GetEstadoEnvio(envio.EstadoEnvio));
                $("#FechaCaducidadDetalle").val(GetFechaSubString(envio.FechaCaducidad));

                $("#TipoProductoDetalle").val(envio.TipoProducto);
                $("#MontoDetalle").val(envio.Monto);
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

function ReenviarCorreo(id, correo, usuario){
    pageBlocked = false;

    var modelView = {
        Id: id,
        CorreoElectronico: correo,
        UsuarioRegistro:usuario
    };

    webApp.Ajax({
        url: urlMantenimiento + 'ReenviarCorreo',
        parametros:  modelView
        
    }, function(response){
        if(response.Success){
            if(response.Warning){                           
                $.gritter.add({
                    title: response.Title,
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });                         
            }else{                            
                $.gritter.add({
                    title: response.Title,
                    text: response.Message,
                    class_name: 'gritter-success gritter'
                });
                $("#DetalleEnvio").modal("hide");
                dataTableEnvio.fnReloadAjax();
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