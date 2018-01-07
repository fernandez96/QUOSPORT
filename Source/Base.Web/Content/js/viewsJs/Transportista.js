﻿var dataTableAlmacen = null;
var formularioMantenimiento = "TransportitaForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'Transportista/Listar';
var urlMantenimiento = baseUrl + 'Transportista/';
var rowAlmacen = null;
var urlcargarEstado = baseUrl + 'Transportista/GetAllEstado';
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
        VisualizarDataTableTransprotita();
    });

    $('#Transportista tbody').on('click', 'tr', function () {
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
        $("#TransportitaId").val(0);
        $("#accionTitle").text('Nuevo');
        $("#NuevoTransportita ").modal("show");
        $("#codigo").prop("disabled", false);
        GetCorrelativoCab();
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

                }, 'El estado del almacen ' + '<b>' + rowAlmacen.tranc_vnombre_transportista + '</b>' + '  es ' + '<span class="label label-warning arrowed-in arrowed-in-right">Inactivo</span>' + ', por lo tanto pasara a estado activo' + '</n>' + ' ¿Desea continuar?');
            } else if (rowAlmacen.Estado == estadoActivo) {
                webApp.showConfirmResumeDialog(function () {
                    checkSession(function () {
                        CambiarStatus(rowAlmacen.Id, estadoInactivo);
                    });

                }, 'El estado del almacen ' + '<b>' + rowAlmacen.tranc_vnombre_transportista + '</b>' + '  es ' + '<span class="label label-info label-sm arrowed-in arrowed-in-right">Activo</span>' + ', por lo tanto pasara a estado Inactivo,' + '</n>' + ' ¿Desea continuar?');
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
            checkSession(function () {
                GuardarAlmacen();
            });
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
            razonSocial: {
                required: true
            },
            direccion: {
                required: true
            },
            telefono: {
               // required: true,
                strippedminlength: {
                    param: 6
                },
            },
            vehMarPlac: {
                required: true,
                strippedminlength: {
                    param: 6
                },
            },
            CertIncripcion: {
                required: true
            },
            licencia: {
                required: true
            },
            ruc_: {
                required: true,
                strippedminlength: {
                    param: 11
                },

            }

        },
        {
            codigo: {
                required: "Por favor ingrese Codigo.",

            },
            razonSocial: {
                required: "Por favor ingrese Razón social.",

            },
            direccion: {
                required: "Por favor seleccione Dirección.",

            },
            telefono: {
                strippedminlength: "Por favor ingrese al menos 6 caracteres."
               // required: "Por favor ingrese Telefono."
               
            },
            vehMarPlac: {
                required: "Por favor ingrese Vehiculo/Marca/Placa",
                strippedminlength: "Por favor ingrese al menos 6 caracteres."
            },
            CertIncripcion: {
                required: "Por favor ingrese Certificación de Inscripción"
            },
            licencia: {
                required: "Por favor ingrese Licencia"
            },
            ruc_: {
                required: "Por favor ingrese RUC",
                strippedminlength: "Por favor ingrese al menos 11 caracteres."
            },

        });
    CargarEstado();
    $('[data-toggle="tooltip"]').tooltip();
    GetCorrelativoCab();
});

function VisualizarDataTableTransprotita() {
    dataTableAlmacen = $('#Transportista').DataTable({
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
            { "data": "tranc_vid_transportista" },
            { "data": "tranc_vnombre_transportista" },
            { "data": "tranc_vdireccion" },
            { "data": "tranc_vnumero_telefono" },
            { "data": "tranc_vnum_marca_placa" },
            { "data": "tranc_vnum_certif_inscrip" },
            { "data": "tranc_vnum_licencia_conducir" },
            { "data": "tranc_ruc" },
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
             { "className": "hidden-1200", "aTargets": [8], "width": "10%" },
            { "bSortable": false, "className": "hidden-1200", "aTargets": [9], "width": "7%" }

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
                var transportista = response.Data;
                $("#codigo").val(transportista.tranc_vid_transportista);
                $("#razonSocial").val(transportista.tranc_vnombre_transportista);
                $("#direccion").val(transportista.tranc_vdireccion);
                $("#telefono").val(transportista.tranc_vnumero_telefono);
                $("#vehMarPlac").val(transportista.tranc_vnum_marca_placa);
                $("#CertIncripcion").val(transportista.tranc_vnum_certif_inscrip);
                $("#ruc_").val(transportista.tranc_ruc);
                $("#licencia").val(transportista.tranc_vnum_licencia_conducir);
                $("#Estado").val(transportista.Estado);
                $("#TransportitaId").val(transportista.Id);
                $("#accionTitle").text('Editar');
                $("#NuevoTransportita").modal("show");
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
                $("#NuevoTransportita").modal("hide");
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
        Id: $("#TransportitaId").val(),
        tranc_vid_transportista: $("#codigo").val(),
        tranc_vnombre_transportista: $("#razonSocial").val(),
        tranc_vnumero_telefono: $("#telefono").val(),
        tranc_vdireccion: $("#direccion").val(),
        tranc_vnum_marca_placa: $("#vehMarPlac").val(),
        tranc_vnum_certif_inscrip: $("#CertIncripcion").val(),
        tranc_ruc: $("#ruc_").val(),
        tranc_vnum_licencia_conducir: $("#licencia").val(),
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
                $("#NuevoTransportita").modal("hide");
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

function GetCorrelativoCab() {
   
    webApp.Ajax({
        url: urlMantenimiento + 'GetCorrelativoCab'
    }, function (response) {
        if (response.Success) {
            if (response.Warning) {
                $.gritter.add({
                    title: 'Alerta',
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });
            } else {
                var notaingreso = response.Data + 1;

                if (notaingreso < 10) {
                    $("#codigo").val('00' + notaingreso);
                }
                else if (notaingreso >= 100) {
                    $("#codigo").val('0' + notaingreso);
                }
                else if (notaingreso >= 1000) {
                    $("#codigo").val(notaingreso);
                }
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
        if ($('#AlmacenSearchForm').valid()) {
            checkSession(function () {
                dataTableAlmacen.ajax.reload();
            });
        }
    }
}

function LimpiarFormulario() {
    webApp.clearForm(formularioMantenimiento);
    $("#Estado").val(1);
    $("#Username").focus();
}