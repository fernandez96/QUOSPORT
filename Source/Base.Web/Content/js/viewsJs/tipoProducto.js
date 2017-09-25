var dataTableTipoProducto = null;
var formularioMantenimiento = "TipoProductoForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'TipoProducto/Lista';
var urlMantenimiento = baseUrlApiService + 'TipoProducto/'
var rowTipoProducto = null;

$(document).ready(function () {
    $.extend($.fn.dataTable.defaults, {
        language: { url: baseUrl + 'Content/js/dataTables/Internationalisation/es.txt' },
        responsive: true,
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "bProcessing": true,
        "dom": 'fltip'
    });
    checkSession(function () {
        VisualizarDataTableTipoProducto();
    });


    $('body').on('click', 'button.btnAgregarTipoProducto', function () {
        LimpiarFormulario();
     
        $("#NuevoTipoProducto").modal("show");
    });

    $('body').on('click', 'a.verTipoProducto', function () {
        rowTipoProducto = this;
        checkSession(function () {
        VisualizarHTML();
        });
    });

    $('body').on('click', 'a.editarTipoProducto', function () {
        rowTipoProducto = this;
        checkSession(function () {
            GetTipoProductoById();
        });
    });


    $('body').on('click', 'a.eliminarTipoProducto', function () {
        var aPos = dataTableTipoProducto.fnGetPosition(this.parentNode.parentNode);
        var aData = dataTableTipoProducto.fnGetData(aPos[0]);
        var rowID = aData.Id;

        delRowPos = aPos[0];
        delRowID = rowID;

        webApp.showDeleteConfirmDialog(function () {
            checkSession(function () {
                Eliminar();
            });
        }, 'Se eliminará el registro.  ¿Está seguro que desea continuar?');
    });

    //$('textarea.limited').inputlimiter({
    //    remText: '%n caracteres restantes...',
    //    limitText: 'máximo permitido : %n.'
    //});

    $("#btnVisualizar").on("click", function (e) {
        checkSession(function () {
            e.preventDefault();
            if (document.getElementById('iframePieza').contentWindow.document.body != null)
                document.getElementById('iframePieza').contentWindow.document.body.innerHTML = "";
            if (document.getElementById('iframePieza').contentWindow.document.head != null)
                document.getElementById('iframePieza').contentWindow.document.head.innerHTML = "";
            document.getElementById('iframePieza').contentWindow.document.write($("#Contenido").val());
            $("#tipoTitle").text($("#Nombre").val());
            $("#VisualizadorPieza").modal("show");
            var $contents = $('#iframePieza').contents();
            $contents.scrollTop(0);
        });
    });

    $("#btnGuardarTipoProducto").on("click", function (e) {

        if ($('#' + formularioMantenimiento).valid()) {
            webApp.showReConfirmDialog(function () {
                checkSession(function () {
                    GuardarTipoProducto();
                });
            });
        }

        e.preventDefault();
    });
    webApp.validarLetrasEspacio(['Nombre']);
    webApp.InicializarValidacion(formularioMantenimiento,
        {
            Nombre: {
                required: true,
                noPasteAllowLetterAndSpace: true,
            },
            Contenido: {
                required: true
            }
        },
        {
            Nombre: {
                required: "Por favor ingrese Nombre",
            },
            Contenido: {
                required: "Por favor Ingrese Contenido",
            }  
        });

});

function VisualizarHTML()
{
    var aPos = dataTableTipoProducto.fnGetPosition(rowTipoProducto.parentNode.parentNode);
    var aData = dataTableTipoProducto.fnGetData(aPos[0]);

    if (document.getElementById('iframePieza').contentWindow.document.body != null)
        document.getElementById('iframePieza').contentWindow.document.body.innerHTML = "";
    if (document.getElementById('iframePieza').contentWindow.document.head != null)
        document.getElementById('iframePieza').contentWindow.document.head.innerHTML = "";
    document.getElementById('iframePieza').contentWindow.document.write(aData.Contenido);
    $("#VisualizadorPieza").modal("show");
    $("#tipoTitle").text(aData.Nombre);
    var $contents = $('#iframePieza').contents();
    $contents.scrollTop(0);
}

function VisualizarDataTableTipoProducto() {
    dataTableTipoProducto = $('#TipoProductoDataTable').dataTable({
        "bFilter": false,
        "bProcessing": true,
        "serverSide": true,
        //"scrollY": "350px",              
        "ajax": {
            "url": urlListar,
            "type": "POST",
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
                    <a class="blue verTipoProducto"    href="javascript:void(0);"><i class="ace-icon fa fa-eye bigger-130"   ></i></a>\
                    <a class="green editarTipoProducto" href="javascript:void(0)"><i class="ace-icon fa fa-pencil bigger-130"></i></a>\
                    <a class="red eliminarTipoProducto" href="javascript:void(0)"><i class="ace-icon fa fa-trash-o bigger-130"></i></a>\
                    </div>';
                }
            },
            { "data": "Id" },
            { "data": "Nombre" },
            { "data": "Contenido" },
            {
                "data": function (obj) {
                    if (obj.Estado == "1")
                        return '<span class="label label-info label-sm arrowed-in arrowed-in-right">Activo</span>';
                    else
                        return '<span class="label label-sm arrowed-in arrowed-in-right">Inactivo</span>';
                }
            }
        ],
        "aoColumnDefs": [
            { "bSortable": false, "sClass": "center", "aTargets": [0], "width": "10%" },
            { "bVisible": false, "aTargets": [1] },
            { "aTargets": [2], "width": "50%" },
            { "bVisible": false, "aTargets": [3] },
            { "bSortable": false, "aTargets": [4], "width": "30%" }
        ],
        "order": [[1, "desc"]]
    });
}

function GetTipoProductoById() {
    var aPos = dataTableTipoProducto.fnGetPosition(rowTipoProducto.parentNode.parentNode);
    var aData = dataTableTipoProducto.fnGetData(aPos[0]);
    var rowID = aData.Id;

    var modelView = {
        Id: aData.Id
    };

    webApp.Ajax({
        url: urlMantenimiento + 'GetById',
        parametros: modelView

    }, function (response) {
        if (response.Success) {
            if (response.warning) {
                $.gritter.add({
                    title: 'Alerta',
                    text: response.message,
                    class_name: 'gritter-warning gritter'
                });
            } else {
                LimpiarFormulario();
                var tipoProducto = response.Data;
                $("#Nombre").val(tipoProducto.Nombre);
                $("#Contenido").val(tipoProducto.Contenido);
                $("#Estado").val(tipoProducto.Estado);
                $("#TipoProductoId").val(tipoProducto.Id);
                setTimeout(function () {
                    $("#Contenido").scrollTop(0);
                }, 500);

                $("#accionTitle").text('Editar');
                $("#NuevoTipoProducto").modal("show");
            }

        } else {
            $.gritter.add({
                title: 'Error',
                text: response.message,
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
    $("#Estado").val(1);
    $("#Nombre").focus();
}

function Eliminar() {
    var modelView = {
        Id: delRowID,
        UsuarioRegistro: $("#usernameLogOn strong").text()
    };

    webApp.Ajax({
        url: urlMantenimiento + 'Delete',
        async: false,
        parametros: modelView
        
    }, function (response) {
        $("#NuevoTipoProducto").modal("hide");
        if (response.Success) {
            dataTableTipoProducto.fnDeleteRow(delRowPos);
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
    delRowPos = null;
    delRowID = 0;
}

function GuardarTipoProducto() {
    var modelView = {
        Id: $("#TipoProductoId").val(),
        Nombre: $("#Nombre").val(),
        Contenido: $("#Contenido").val(),
        Estado: $("#Estado").val(),
        UsuarioRegistro: $("#usernameLogOn strong").text(),
    };

    if (modelView.Id == 0)
        action = 'Add';
    else
        action = 'Update';

    webApp.Ajax({
        url: urlMantenimiento + action,
        parametros:modelView
      
    }, function (response) {
        if (response.Success) {
            dataTableTipoProducto.fnReloadAjax();
            if (response.Warning) {
                $.gritter.add({
                    title: response.Title,
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });
            } else {
                $("#NuevoTipoProducto").modal("hide");
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