var dataTableUnidadMedida = null;
var formularioMantenimiento = "UnidadMedidaForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'UnidadMedida/Listar';
var urlMantenimiento = baseUrl + 'UnidadMedida/';
var urlcargarEstado = baseUrl + 'Usuario/GetAll';
var rowUnidadMedida = null;
var tablaEstado = 1;
var estadoActivo = 1;
var estadoInactivo = 2;

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
            if (rowUnidadMedida.Estado===estadoInactivo) {
                webApp.showMessageDialog('La unidad medida ' + '<b>' + rowUnidadMedida.umec_vdescripcion + '</b>' + ' se encuentra  ' + '<span class="label label-warning arrowed-in arrowed-in-right">Inactivo</span>' + '.Si desea hacer una modificación le recomendamos cambiarle de estado a dicha unidad medida.');
            }
            else {
                checkSession(function () {
                    GetUnidadMedidaById();
                });
            }
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



    $('#btnestadoUnidadMedida').on('click', function () {
        rowUnidadMedida = dataTableUnidadMedida.row('.selected').data();
        if (typeof rowUnidadMedida === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            if (rowUnidadMedida.Estado == estadoInactivo) {
                webApp.showConfirmResumeDialog(function () {
                    checkSession(function () {
                        CambiarStatus(rowUnidadMedida.Id, estadoActivo);
                    });

                }, 'El estado de la unidad medida ' + '<b>' + rowUnidadMedida.umec_vdescripcion + '</b>' + '  es ' + '<span class="label label-warning arrowed-in arrowed-in-right">Inactivo</span>' + ', por lo tanto pasara a estado activo' + '</n>' + ' ¿Desea continuar?');
            } else if (rowUnidadMedida.Estado == estadoActivo) {
                webApp.showConfirmResumeDialog(function () {
                    checkSession(function () {
                        CambiarStatus(rowUnidadMedida.Id, estadoInactivo);
                    });

                }, 'El estado de la unidad medida ' + '<b>' + rowUnidadMedida.umec_vdescripcion + '</b>' + '  es ' + '<span class="label label-info label-sm arrowed-in arrowed-in-right">Activo</span>' + ', por lo tanto pasara a estado Inactivo,' + '</n>' + ' ¿Desea continuar?');
            }
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
    CargarEstado();
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
                    CodigoSearch: $("#Codigosearch").val(),
                    DescripcionSearch: $("#Descripcionsearch").val()
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
                    if (obj.Estado == 1) {
                        return '<span class="label label-info label-sm arrowed-in arrowed-in-right">Activo</span>';
                    }
                    else if (obj.Estado == 2) {
                        return '<span class="label label-warning arrowed-in arrowed-in-right">Inactivo</span>';
                    }
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
                $("#Estado").val(unidad.Estado);
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
        Id: $("#UnidadMedidaId").val(),
        umec_vcod_unidad_medida: $("#codigo").val(),
        umec_vdescripcion: $("#descripcion").val(),
        Estado: $("#Estado").val(),
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

function CambiarStatus(Id, estado) {
    var modelView = {
        Id: Id,
        Estado: estado
    };
    webApp.Ajax({
        url: urlMantenimiento + 'UpdateStatus',
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
                $.gritter.add({
                    title: response.Title,
                    text: response.Message,
                    class_name: 'gritter-success gritter'
                });
                dataTableUnidadMedida.ajax.reload();
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

function CargarEstado() {
    var modelView = {
        idtabla: tablaEstado
    };
    webApp.Ajax({
        url: urlcargarEstado,
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
                $.each(response.Data, function (index, item) {
                    $("#Estado").append('<option value="' + item.Id + '">' + item.tbpd_vdescripcion_detalle + '</option>');
                });
                webApp.clearForm('UsuarioSearchForm');
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

function buscar(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    if (tecla == 13) {
        if ($('#UnidadMedidaSearchForm').valid()) {
            checkSession(function () {
                dataTableUnidadMedida.ajax.reload();
            });
        }
    }
}

function LimpiarFormulario() {
    webApp.clearForm(formularioMantenimiento);
    $("#Estado").val(1);
    $("#codigo").focus();
}