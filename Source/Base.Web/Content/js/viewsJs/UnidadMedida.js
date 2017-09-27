var dataTableUnidadMedida = null;
var formularioMantenimiento = "UnidadMedidaForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'UnidadMedida/Listar';
var urlMantenimiento = baseUrl + 'UnidadMedida/';
var rowUnidadMedida = null;


$(document).ready(function () {
    $.extend($.fn.dataTable.defaults, {
        language: { url: baseUrl + 'Content/js/dataTables/Internationalisation/es.txt' },
        responsive: true,
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "bProcessing": true,
        "dom": 'fltip'
    });

    checkSession(function () {
        VisualizarDataTableUnidadMedida();
    });

    $('#UnidadMedidaDataTable  tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            dataTableUnidadMedida.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $('#btnAgregarUnidadMedida').on('click', function () {
        LimpiarFormulario();

        $("#UnidadMedidaId").val(0);
        $("#accionTitle").text('Nuevo');
        $("#NuevoUnidadMedida").modal("show");
        $("#codigo").prop("disabled", false);
    });

    $('#btnEditarUnidadMedida').on('click', function () {
        rowUnidadMedida = dataTableUnidadMedida.row('.selected').data();
        if (typeof rowUnidadMedida === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            checkSession(function () {
                GetUnidadMedidaById();
            });
        }

    });

    $('#btnEliminarUnidadMedida').on('click', function () {
        rowUnidadMedida = dataTableUnidadMedida.row('.selected').data();
        if (typeof rowUnidadMedida === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            webApp.showDeleteConfirmDialog(function () {
                checkSession(function () {
                    EliminarUnidadMedida();
                });
            }, 'Se eliminará el registro. ¿Está seguro que desea continuar?');
        }

    });

    $("#btnSearchUnidadMedida").on("click", function (e) {
        if ($('#UnidadMedidaSearchForm').valid()) {
            checkSession(function () {
                dataTableUnidadMedida.ajax.reload();
            });
        }
        e.preventDefault();
    });

    $("#btnGuardarUnidadMedida").on("click", function (e) {
        if ($('#' + formularioMantenimiento).valid()) {
            checkSession(function () {
                GuardarUnidadMedida();
            });
        }

        e.preventDefault();
    });

    webApp.InicializarValidacion(formularioMantenimiento,
        {
            codigo: {
                required: true

            },
            descripcion: {
                required: true
            }
        },
        {
            codigo: {
                required: "Por favor ingrese Codigo.",

            },
            descripcion: {
                required: "Por favor ingrese Descripción.",

            }
        });
    $('[data-toggle="tooltip"]').tooltip();
});

function VisualizarDataTableUnidadMedida() {
    dataTableUnidadMedida = $('#UnidadMedidaDataTable').DataTable({
        "bFilter": false,
        "bProcessing": true,
        "serverSide": true,
        //"scrollY": "350px",              
        "ajax": {
            "url": urlListar,
            "type": "POST",
            "data": function (request) {
                request.filter = new Object();

                request.filter = {
                    codigoSearch: $("#CodigoSearch").val(),
                    descripcionSearch: $("#Descripcionsearch").val()
                }
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
            { "data": "Id" },
            { "data": "umec_vcod_unidad_medida" },
            { "data": "umec_vdescripcion" }, 
            {
                "data": function (obj) {
                    if (obj.Estado == 1)
                        return '<span class="label label-info label-sm arrowed-in arrowed-in-right">Activo</span>';
                    else
                        return '<span class="label label-sm arrowed-in arrowed-in-right">Inactivo</span>';
                }
            }
        ],
        "aoColumnDefs": [

            { "bVisible": false, "aTargets": [0] },
            { "className": "hidden-120", "aTargets": [1], "width": "10%" },
            { "className": "hidden-120", "aTargets": [2], "width": "18%" },         
            { "bSortable": false, "className": "hidden-480", "aTargets": [3], "width": "5%" }

        ],
        "order": [[1, "desc"]],
        "initComplete": function (settings, json) {
            // AddSearchFilter();
        },
        "fnDrawCallback": function (oSettings) {

        }
    });
}

function GetUnidadMedidaById() {
    var modelView = {
        Id: rowUnidadMedida.Id
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
                LimpiarFormulario();
                var unidad = response.Data;
                $("#codigo").val(unidad.umec_vcod_unidad_medida);
                $("#UnidadMedidaId").val(unidad.Id);
                $("#descripcion").val(unidad.umec_vdescripcion);
                $("#accionTitle").text('Editar');
                $("#NuevoUnidadMedida").modal("show");
                $("#codigo").prop("disabled", true);
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

function EliminarUnidadMedida() {
    var modelView = {
        Id: rowUnidadMedida.Id,
        UsuarioRegistro: $("#usernameLogOn strong").text()
    };

    webApp.Ajax({
        url: urlMantenimiento + 'Delete',
        async: false,
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
                $("#NuevoUnidadMedida").modal("hide");
                dataTableUnidadMedida.row('.selected').remove().draw();
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

function GuardarUnidadMedida() {

    var modelView = {
        Id: $("#UsuarioId").val(),
        umec_vcod_unidad_medida: $("#codigo").val(),
        umec_vdescripcion: $("#descripcion").val(),
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
                $("#NuevoUnidadMedida").modal("hide");
                dataTableUnidadMedida.ajax.reload();
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

function LimpiarFormulario() {
    webApp.clearForm(formularioMantenimiento);
    $("#codigo").focus();
}