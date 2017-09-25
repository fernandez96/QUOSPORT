var dataTableTabla = null;
var dataTableTablaDetalle = null;
var formularioMantenimientoTablaDetalle = "TablaDetalleForm";
var formularioMantenimientoTabla = "TablaForm";
var delRowPos = null;
var delRowID = 0;
var urlListarCabezera = baseUrl + 'TipoOpciones/ListarCabezera';
var urlListarDetalle = baseUrl + 'TipoOpciones/ListarDetalle';
var urlMantenimiento = baseUrl + 'TipoOpciones/';
var urlMantenimientoReport = baseUrl + 'Reporte/';

var rowTabla = null;
var rowTablaDetalle = null;
var selected = [];



Array.prototype.unique = function (a) {
    return function () { return this.filter(a) }
}(function (a, b, c) {
    return c.indexOf(a, b + 1) < 0
});

$(document).ready(function () {

    $.extend($.fn.dataTable.defaults, {
        language: { url: baseUrl + 'Content/js/dataTables/Internationalisation/es.txt' },
        responsive: true,
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "bProcessing": true,
        "dom": 'fltip'
    });

    checkSession(function () {
        VisualizarDataTableTabla();
     
    });


    $('#btnAgregarTabla').on('click', function () {
        LimpiarFormularioTabla();
        $("#UsuarioId").val(0);
        $("#accionTitle").text('Nuevo');
        $("#codigo").prop("disabled", false);
        $("#NuevoTabla").modal("show");
        GetCorrelativoCab();
    });

    $('#btnaccesoTable').on('click', function () {
        rowTabla = dataTableTabla.row('.selected').data();
        if (typeof rowTabla === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {

            $("#UsuarioId").val(0);
            $("#accionTitleDetalle").text('Nuevo');
            $("#DetalleTabla").modal("show");
            $("#idtable").text(rowTabla.tbpc_vcod_tabla_opciones + ' - ' + rowTabla.tbpc_vdescripcion);
            $("#nombreTabla").text(rowTabla.tbpc_vdescripcion);
           
            VisualizarDataTableTablaDetalle();
            dataTableTablaDetalle.clear();
            dataTableTablaDetalle.ajax.reload();
           
               $('#TablaDetalleDataTable  tbody').on('click', 'tr', function () {
                  if ($(this).hasClass('selected')) {
                      $(this).removeClass('selected');
                      
                  }
                  if (!$(this).hasClass('selected')) {
                      dataTableTablaDetalle.$('tr.selected').removeClass('selected');
                      $(this).addClass('selected');
                  }
                
              });
        
        }
    });

  

    $('#btnAgregarTablaDetalle').on('click', function () {
        LimpiarFormularioTablaDetalle();
        $("#TablaDetalleId").val(0);
        $("#accionTitleTablaDetalle").text('Nuevo');
        $("#codigoDetalle").prop("disabled", false);
        $("#NuevoTablaDetalle").modal("show");
        GetCorrelativoDet(rowTabla.Id);
    });


    $('#TablaDataTable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            dataTableTabla.$('tr.selected').removeClass('selected');
          
            $(this).addClass('selected');
        }
    });


    $('#btnEditarTablaDetalle').on('click', function () {
        rowTablaDetalle = dataTableTablaDetalle.row('.selected').data();
        if (typeof rowTablaDetalle === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            checkSession(function () {
                GetTablaDetalleById(rowTablaDetalle.Id);
            });
        }
        
    });

    $('#btnEditarTable').on('click', function () {
        checkSession(function () {
            GetTablaById();
        });
    });
    $('#btprint').on('click', function () {
        rowTabla = dataTableTabla.row('.selected').data();
        if (typeof rowTabla === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            checkSession(function () {
                Imprimir(rowTabla.Id);
            });
        }
        
    });

    
   


    $('#btnEliminarTablaDetalle').on('click', function () {
        rowTablaDetalle = dataTableTablaDetalle.row('.selected').data();
        if (typeof rowTablaDetalle === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            webApp.showDeleteConfirmDialog(function () {
                checkSession(function () {
                    Eliminartabladetalle(rowTablaDetalle.Id);
                });
            }, 'Se eliminará el registro. ¿Está seguro que desea continuar?');
        }

    });

    $("#btnSearchTabla").on("click", function (e) {
        if ($('#TablaSearchForm').valid()) {
            checkSession(function () {
                dataTableTabla.ajax.reload();
            });
        }
        e.preventDefault();
    });

    $("#btnSearchTablaDetalle").on("click", function (e) {
        if ($('#TablaDetalleSearchForm').valid()) {
            checkSession(function () {
                dataTableTablaDetalle.ajax.reload();
            });
        }
        e.preventDefault();
    });


    $("#btnGuardarTablaDetalle").on("click", function (e) {

        if ($('#' + formularioMantenimientoTablaDetalle).valid()) {

            //webApp.showReConfirmDialog(function () {
                checkSession(function () {
                    GuardarTablaDetalle();
                });
            //});
        }

        e.preventDefault();
    });



    $("#btnGuardarTabla").on("click", function (e) {

        if ($('#' + formularioMantenimientoTabla).valid()) {

            //webApp.showReConfirmDialog(function () {
                checkSession(function () {
                    GuardarTabla();
                });
            //});
        }

        e.preventDefault();
    });


    webApp.validarLetrasEspacio(['descripcion','descripcionDetalle']);
    webApp.InicializarValidacion(formularioMantenimientoTabla,
        {
            codigo: {
                required: true,
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
                required: "Por favor ingrese Descripción."
            }
        });
    webApp.InicializarValidacion(formularioMantenimientoTablaDetalle,
     {
  
         codigoDetalle: {
             required: true
         },
         descripcionDetalle: {
             required: true
         }

     },
     {
         codigoDetalle: {
             required: "Por favor ingrese Codigo.",

         },
         descripcionDetalle: {
             required: "Por favor ingrese Descripción."
         }
     });
    $('[data-toggle="tooltip"]').tooltip();
});


function VisualizarDataTableTablaDetalle() {
    dataTableTablaDetalle = $('#TablaDetalleDataTable').DataTable({
        "bFilter": false,
        "bProcessing": true,
        "serverSide": true,
        //"scrollY": "350px",              
        "ajax": {
            "url": urlListarDetalle,
            "type": "POST",
            "data": function (request) {
                request.filter = new Object();

                request.filter = {
                    idTablaSearch: rowTabla.Id,
                    codigoSearch: $("#CodigoDetallesearch").val(),
                    descripcionSearch: $("#DescripcionDetallesearch").val()
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
            { "data": "tbpd_vcod_tabla_opciones_det" },
            { "data": "tbpd_vdescripcion_detalle" }
        ],
        "aoColumnDefs": [
            { "bVisible": false, "aTargets": [0] },
            { "className": "hidden-120 center", "aTargets": [1], "width": "6%" },
            { "className": "hidden-120", "aTargets": [2], "width": "20%" },
        ],
        "order": [[1, "desc"]],
        "initComplete": function (settings, json) {
        },
        "fnDrawCallback": function (oSettings) {

        }
    });
}

function VisualizarDataTableTabla() {
    dataTableTabla = $('#TablaDataTable').DataTable({
        "bFilter": false,
        "bProcessing": true,
        "serverSide": true,
        //"scrollY": "350px",              
        "ajax": {
            "url": urlListarCabezera,
            "type": "POST",
            "data": function (request) {
                request.filter = new Object();

                request.filter = {
                    codigoSearch: $("#Codigosearch").val(),
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
            { "data": "tbpc_vcod_tabla_opciones" },
            { "data": "tbpc_vdescripcion" },
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
            { "bVisible": false, "aTargets": [0] },
            { "className": "hidden-120 center", "aTargets": [1], "width": "6%" },
            { "className": "hidden-120", "aTargets": [2], "width": "18%" },
            { "className": "hidden-1200", "aTargets": [3], "width": "4%" },
           { "bSortable": false, "className": "hidden-1200", "aTargets": [3], "width": "4%" }


        ],
        "order": [[1, "desc"]],
        "initComplete": function (settings, json) {
        },
        "fnDrawCallback": function (oSettings) {

        }
    });
}

function GetTablaById() {
    rowTabla = dataTableTabla.row('.selected').data();
    if (typeof rowTabla === "undefined") {
        webApp.showMessageDialog("Por favor seleccione un registro.");
    }
    
    else {
        var modelView = {
            Id: rowTabla.Id
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
                    LimpiarFormularioTabla();
                    var tipodocumento = response.Data;
                    $("#codigo").val(tipodocumento.tbpc_vcod_tabla_opciones);
                    $("#descripcion").val(tipodocumento.tbpc_vdescripcion);
                    $("#TablaId").val(tipodocumento.Id);
                    $("#codigo").prop("disabled", true);
                    $("#accionTitle").text('Editar');
                    $("#NuevoTabla").modal("show");
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


}

function GetTablaDetalleById(id) {

        var modelView = {
            Id: id
        };
        webApp.Ajax({
            url: urlMantenimiento + 'GetByIdDetalle',
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
                    LimpiarFormularioTablaDetalle();
                    var tipodocumento = response.Data;
                    $("#codigoDetalle").val(tipodocumento.tbpd_vcod_tabla_opciones_det);
                    $("#descripcionDetalle").val(tipodocumento.tbpd_vdescripcion_detalle);
                    $("#TablaDetalleId").val(tipodocumento.Id);
                    $("#codigoDetalle").prop("disabled", true);
                    $("#accionTitleTablaDetalle").text('Editar');
                    $("#NuevoTablaDetalle").modal("show");
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

function GuardarTabla() {

    var modelView = {
        Id: $("#TablaId").val(),
        tbpc_vcod_tabla_opciones: $("#codigo").val(),
        tbpc_vdescripcion: $("#descripcion").val(),
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
                $("#NuevoTabla").modal("hide");
                dataTableTabla.ajax.reload();
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

function GuardarTablaDetalle() {
  
    var modelView = {
        IdTable: $("#TablaDetalleId").val(),
        Id: rowTabla.Id,
        tbpc_iid_tabla_opciones: rowTabla.Id,
        tbpd_vcod_tabla_opciones_det: $("#codigoDetalle").val(),
        tbpd_vdescripcion_detalle: $("#descripcionDetalle").val(),
        UsuarioRegistro: $("#usernameLogOn strong").text()
    };

    if (modelView.IdTable == 0){
        action = 'AddDetalle';
    }
    else {
        action = 'UpdateDetalle';
        modelView.Id = rowTablaDetalle.Id

    }
        

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
                $("#NuevoTablaDetalle").modal("hide");
                dataTableTablaDetalle.clear();
                dataTableTablaDetalle.ajax.reload();
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
                    LimpiarFormularioTabla();
                    var tipodocumento = response.Data;
                    if (tipodocumento==null) {
                        $("#codigo").val('00' + 1);
                    }
                   else if (tipodocumento.correlativaCab < 10) {
                        $("#codigo").val('00' + tipodocumento.correlativaCab);
                    }
                   else if (tipodocumento.correlativaCab >= 10) {
                       $("#codigo").val('0' + tipodocumento.correlativaCab);
                    }
                   else if (tipodocumento.correlativaCab >99 ) {
                       $("#codigo").val(tipodocumento.correlativaCab);
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


function GetCorrelativoDet(id) {
    var modelView = {
        Id: id
    };
    webApp.Ajax({
        url: urlMantenimiento + 'GetCorrelativoDet',
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
                LimpiarFormularioTablaDetalle();
                var tipodocumento = response.Data;
                if (tipodocumento==null) {
                    $("#codigoDetalle").val('00' +1);
                }
               else if (tipodocumento.correlativaDet < 10) {
                    $("#codigoDetalle").val('00' + tipodocumento.correlativaDet);
                }
                else if (tipodocumento.correlativaDet >= 10) {
                    $("#codigoDetalle").val('0' + tipodocumento.correlativaDet);
                }
                else if (tipodocumento.correlativaDet > 99) {
                    $("#codigoDetalle").val(tipodocumento.correlativaDet);
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

function Eliminartabladetalle(id) {
    var modelView = {
        Id: id,
        UsuarioRegistro: $("#usernameLogOn strong").text()
    };

    webApp.Ajax({
        url: urlMantenimiento + 'DeleteDetalle',
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
                dataTableTablaDetalle.ajax.reload();
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

function Imprimir(id) {
     webApp.Ajax({
        url: urlMantenimientoReport + 'StimulsoftControl',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        datatype: 'html',
        parametros: {
            parametros: [
                          { key: 'TablaId', value: id }
                        ],
            controllerGetSnapshot: 'TipoOpciones',
        },
            
    
    }, function (result) {
        $('#divReport').html(result);
        $("#modal-print-tabla").modal("show");
    });
}

function LimpiarFormularioTabla() {
    webApp.clearForm(formularioMantenimientoTabla);
}

function LimpiarFormularioTablaDetalle() {
    webApp.clearForm(formularioMantenimientoTablaDetalle);
}

