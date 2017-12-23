var dataTableAlmacen = null;
var formularioMantenimiento = "AlmacenForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'Almacen/Listar';
var urlMantenimiento = baseUrl + 'Almacen/';
var rowAlmacen = null;
var urlcargarEstado = baseUrl + 'Usuario/GetAll';
var estadoActivo = 1;
var estadoInactivo = 2;
var tablaEstado = 1;
var tablaTipo = 7;
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
        VisualizarDataTableAlmacen();
    });

    $('#Almacen tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            dataTableAlmacen.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $('#btnAgregarAlmacen').on('click', function () {
        LimpiarFormulario();
        $("#AlmacenId").val(0);
        $("#accionTitle").text('Nuevo');
        $("#NuevoAlmacen ").modal("show");
        $("#codigo").prop("disabled", false);
        $("#tipo").prop("disabled", false);
    });

    $('#btnEditarAlmacen').on('click', function () {
        rowAlmacen = dataTableAlmacen.row('.selected').data();
        if (typeof rowAlmacen === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            if (rowAlmacen.Estado === estadoInactivo) {
                webApp.showMessageDialog('El Almacen ' + '<b>' + rowAlmacen.almac_vdescripcion + '</b>' + ' se encuentra  ' + '<span class="label label-warning arrowed-in arrowed-in-right">Inactivo</span>' + '.Si desea hacer un modificación le recomendamoos cambiarle de estado a dicho Almacen.');
            }
            else {
                checkSession(function () {
                    GetAlmacenById();
                });
            }

        }

    });

    $('#btnEliminarAlmacen').on('click', function () {
        rowAlmacen = dataTableAlmacen.row('.selected').data();
        if (typeof rowAlmacen === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            webApp.showDeleteConfirmDialog(function () {
                checkSession(function () {
                    EliminarAlmacen();
                });
            }, 'Se eliminará el registro. ¿Está seguro que desea continuar?');
        }

    });


    $('#btnEstadoAlmacen').on('click', function () {
        rowAlmacen = dataTableAlmacen.row('.selected').data();
        if (typeof rowAlmacen === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            if (rowAlmacen.Estado == estadoInactivo) {
                webApp.showConfirmResumeDialog(function () {
                    checkSession(function () {
                        CambiarStatus(rowAlmacen.Id, estadoActivo);
                    });

                }, 'El estado del almacen ' + '<b>' + rowAlmacen.almac_vdescripcion + '</b>' + '  es ' + '<span class="label label-warning arrowed-in arrowed-in-right">Inactivo</span>' + ', por lo tanto pasara a estado activo' + '</n>' + ' ¿Desea continuar?');
            } else if (rowAlmacen.Estado == estadoActivo) {
                webApp.showConfirmResumeDialog(function () {
                    checkSession(function () {
                        CambiarStatus(rowAlmacen.Id, estadoInactivo);
                    });

                }, 'El estado del almacen ' + '<b>' + rowAlmacen.almac_vdescripcion + '</b>' + '  es ' + '<span class="label label-info label-sm arrowed-in arrowed-in-right">Activo</span>' + ', por lo tanto pasara a estado Inactivo,' + '</n>' + ' ¿Desea continuar?');
            }
        }

    });


    $("#btnSearchAlmacen").on("click", function (e) {
        if ($('#AlmacenSearchForm').valid()) {
            checkSession(function () {
                dataTableAlmacen.ajax.reload();
            });
        }
        e.preventDefault();
    });

    $("#btnGuardarAlmacen").on("click", function (e) {

        if ($('#' + formularioMantenimiento).valid()) {

            ////webApp.showConfirmDialog(function () {
            checkSession(function () {
                GuardarAlmacen();


            });
            //});
        }

        e.preventDefault();
    });

    // webApp.validarLetrasEspacio(['Nombre', '']);
    webApp.validarNumerico(['telefono']);
    $('#correo').validCampoFranz(' @abcdefghijklmnÃ±opqrstuvwxyz_1234567890.');

    webApp.InicializarValidacion(formularioMantenimiento,
        {
            codigo: {
                required: true
            },
            descripcion: {
                required: true
            },
            tipo: {
                required: true
            },
            ubicacion: {
                required: true
            },
            telefono: {
                strippedminlength: {
                    param: 6
                },
            },
            correo: {
                email: true
            }

        },
        {
            codigo: {
                required: "Por favor ingrese Codigo.",

            },
            descripcion: {
                required: "Por favor ingrese Descripción.",

            },
            tipo: {
                required: "Por favor seleccione Tipo.",

            },
            ubicacion: {
                required: "Por favor ingrese Ubicación."
            },
            telefono: {
                strippedminlength: "Por favor ingrese al menos 6 caracteres."
            },
            correo: {
                email: "Por favor ingrese Correo válido"
            }

        });
    CargarTipo();
    CargarEstado();
    $('[data-toggle="tooltip"]').tooltip();
});

function VisualizarDataTableAlmacen() {
    dataTableAlmacen = $('#Almacen').DataTable({
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
            { "data": "almac_vcod_almacen" },
            { "data": "almac_vdescripcion" },
            { "data": "almac_vtipo" },
            { "data": "almac_vubicacion" },
            { "data": "almac_vresponsable" },
            { "data": "almac_vtelefono" },
            { "data": "almac_vcorreo" },
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
            { "className": "hidden-992", "aTargets": [3], "width": "10%" },
            { "className": "hidden-120", "aTargets": [4], "width": "18%" },
            { "className": "hidden-600", "aTargets": [5], "width": "18%" },
            { "className": "hidden-1200", "aTargets": [6], "width": "10%" },
            { "className": "hidden-1200", "aTargets": [7], "width": "10%" },
            { "bSortable": false, "className": "hidden-1200", "aTargets": [8], "width": "7%" }

        ],
        "order": [[1, "asc"]],
        "initComplete": function (settings, json) {
            // AddSearchFilter();
        },
        "fnDrawCallback": function (oSettings) {

        }
    });
}

function GetAlmacenById() {
    var modelView = {
        Id: rowAlmacen.Id
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
                var almacen = response.Data;
                $("#codigo").val(almacen.almac_vcod_almacen);
                $("#descripcion").val(almacen.almac_vdescripcion);
                $("#tipo").val(almacen.almac_itipo);
                $("#ubicacion").val(almacen.almac_vubicacion);
                $("#responsable").val(almacen.almac_vresponsable);
                $("#telefono").val(almacen.almac_vtelefono);
                $("#correo").val(almacen.almac_vcorreo);
                $("#Estado").val(almacen.Estado);
                $("#AlmacenId").val(almacen.Id);
                $("#accionTitle").text('Editar');
                $("#NuevoAlmacen").modal("show");
                $("#codigo").prop("disabled", true);
                $("#tipo").prop("disabled", true);
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

function EliminarAlmacen() {
    var modelView = {
        Id: rowAlmacen.Id,
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
                $("#NuevoAlmacen").modal("hide");
                dataTableAlmacen.row('.selected').remove().draw();
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

function GuardarAlmacen() {

    var modelView = {
        Id: $("#AlmacenId").val(),
        almac_vcod_almacen: $("#codigo").val(),
        almac_vdescripcion: $("#descripcion").val(),
        almac_vubicacion: $("#ubicacion").val(),
        almac_vresponsable: $("#responsable").val(),
        almac_vtelefono: $("#telefono").val(),
        almac_vcorreo: $("#correo").val(),
        almac_itipo: $("#Tipo").val(),
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
                $("#NuevoAlmacen").modal("hide");
                dataTableAlmacen.ajax.reload();
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

function CargarTipo() {
    var modelView = {
        idtabla: tablaTipo
    };
    webApp.Ajax({
        url: urlMantenimiento + 'GetAll',
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
                    $("#Tipo").append('<option value="' + item.Id + '">' + item.tbpd_vdescripcion_detalle + '</option>');
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
                dataTableAlmacen.ajax.reload();
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

function AddSearchFilter() {
    $("#UsuarioDataTable_wrapper").prepend($("#searchFilterDiv").html());
}

function buscar(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    if (tecla == 13) {
        if ($('#AlmacenSearchForm').valid()) {
            checkSession(function () {
                dataTableAlmacen.ajax.reload();
            });
        }
    }
}

function LimpiarFormulario() {
    webApp.clearForm(formularioMantenimiento);
    $("#Tipo").val(9);
    $("#Estado").val(1);
    $("#Username").focus();
    $("#Contrasena").prop("type", "password");
}