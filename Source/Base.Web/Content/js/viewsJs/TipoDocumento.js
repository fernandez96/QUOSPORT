var dataTableTipoDocumento = null;
var dataTableModulo = null;
var formularioMantenimiento = "tipodocumentoForm";
var divCheckList = "idContentModulos";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'TipoDocumento/Listar';
var urlListarModulo = baseUrl + 'TipoDocumento/ListarModulo';
var urlMantenimiento = baseUrl + 'TipoDocumento/';
var urlListaCargo = baseUrl + 'TipoDocumento/';
var rowUsuario = null;
var rowModulo = null;
var selected = [];
var IdModulocheck = [];
var countCheck = [];
var checkselect = [];
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
        VisualizarDataTableUsuario();
        VisualizarDataTableModulo();
        CargarModulo();
     
    });

    $('input[name=checkbox]').on('click', function () {
        IdModulocheck.push($(this).attr("data-permiso"));
    
    });
    


    $('#btnAgregarTipodocumento').on('click', function () {
        LimpiarFormulario();
        $("#UsuarioId").val(0);
        $("#accionTitle").text('Nuevo');
        $("#codigo").prop("disabled", false);
        $("#NuevoTipoDocumento").modal("show");
    });
    $('#btnacceso').on('click', function () {
        rowUsuario = dataTableTipoDocumento.row('.selected').data();
        if (typeof rowUsuario === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {

            $("#UsuarioId").val(0);
            $("#accionTitleacceso").text('');
            $("#Nuevoacceso").modal("show");
            $("#iddescripcion").text(rowUsuario.tdocc_vdescripcion);
            cleanInputCheckList();
            CargarselecinadoModulos(rowUsuario.Id);
            IdModulocheck.length = 0;
         
        }    
    });

    $('#TipoDocumentoDataTable  tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            dataTableTipoDocumento.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
            rowUsuario = this;
        }
    });
    $('#btnEditar').on('click', function () {
    
        checkSession(function () {
         
            GetTipoDocumentoById();
        });
    });

    $('#btnEliminar').on('click', function () {
        rowUsuario = dataTableTipoDocumento.row('.selected').data();
        if (typeof rowUsuario === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            webApp.showDeleteConfirmDialog(function () {
                checkSession(function () {
                    EliminarUsuario(rowUsuario.Id);
                });
            }, 'Se eliminará el registro. ¿Está seguro que desea continuar?');
        }
    
    });

    $('body').on('click', 'a.btnBuscarUsuario', function () {

        $("#searchFilterUsuario").modal("show");
    });

    $("#btnSearchTipoDocumento").on("click", function (e) {
        if ($('#TipoDocumentoSearchForm').valid()) {
            checkSession(function () {
                var aData = $("#TipoDocumentoDataTable  >tbody >tr").length;
                dataTableTipoDocumento.ajax.reload();
                
                 $("#idresult").text(aData);
            });
        }
        e.preventDefault();
    });

    $("#btnGuardarTipodocumento").on("click", function (e) {

        if ($('#' + formularioMantenimiento).valid()) {

            //webApp.showReConfirmDialog(function () {
                checkSession(function () {
                    GuardarUsuario();
                });
            //});
        }

        e.preventDefault();
    });


  
    $("#btnGuardarAccesoModulo").on("click", function (e) {
          CountInputCheckList();
        if (countCheck.unique().length <= 0) {
            webApp.showMessageDialog("Por favor seleccione un modulo.");
        }
        else {
            if (IdModulocheck.unique().length==0) {
                $("#Nuevoacceso").modal("hide");
            }
            else {
                webApp.showReConfirmDialog(function () {
                    checkSession(function () {
                        GuardarAccesoModulo(rowUsuario.Id);
                    });
                });
                e.preventDefault();
            }
        
          
        }
    });
    

    webApp.validarLetrasEspacio(['descripcion']);
    webApp.InicializarValidacion(formularioMantenimiento,
        {
            codigo: {
                required: true
            },
            descripcion: {
                required: true,
      
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
    $('[data-toggle="tooltip"]').tooltip();
});


function VisualizarDataTableUsuario() {
    dataTableTipoDocumento = $('#TipoDocumentoDataTable').DataTable({
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
            { "data": "tdocc_vabreviatura_tipo_doc" },
            { "data": "tdocc_vdescripcion" },
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
            //{ "bSortable": false, "sClass": "center", "aTargets": [0], "width": "10%" },
            { "bVisible": false, "aTargets": [0] },
            { "className": "hidden-120 center", "aTargets": [1], "width": "6%" },
            { "className": "hidden-120", "aTargets": [2], "width": "18%" },
            { "className": "hidden-1200", "aTargets": [3], "width": "4%" },
           { "bSortable": false, "className": "hidden-1200", "aTargets": [3], "width": "4%" }
          

        ],
        "order": [[1, "desc"]],
        "initComplete": function (settings, json) {
            //AddSearchFilter();
        },
        "fnDrawCallback": function (oSettings) {

        }
    });
}

function VisualizarDataTableModulo() {
   
    dataTableModulo = $('#AccesoDataTable').DataTable({
        "bFilter":false,
        "ordering": false,
        "info":     false,
        "bProcessing": true,
        "serverSide": true,
        //"scrollY": "350px",
        
        "ajax": {
            "url": urlListarModulo,
            "type": "POST",
            "data": function (request) {
                request.filter = new Object();

                request.filter = {
                    NombreSearch: '',
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
        "rowCallback": function( row, data ) {
            if ( $.inArray(data.Id, selected) !== -1 ) {
                $(row).addClass('selected');
            }
        },
        "bAutoWidth": false,
        "columns": [
            { "data": "Id" },
            { "data": "tablc_vdescripcion" },
            {
                "data": function (obj) {
                    if (obj.Id) {
                        return '<div class="action-buttons">\
                        <input id="idcheckok" type="checkbox">\
                        </div>';
                    }                  
                }
            }
        ],
        "aoColumnDefs": [
             { "bSortable": false, "sClass": "center", "aTargets": [2], "width": "10%" },
            { "bVisible": false, "aTargets": [0] },
            { "className": "hidden-120", "aTargets": [1], "width": "6%" },
            { "bSortable": false, "className": "hidden-120 select-checkbox", "aTargets": [2], "width": "4%" },
        
        ],
        
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        "order": [[1, "desc"]],
        "initComplete": function (settings, json) {
            //AddSearchFilter();
        },
        "fnDrawCallback": function (oSettings) {

        }
    });

}

function GetTipoDocumentoById() {
    rowUsuario = dataTableTipoDocumento.row('.selected').data();
    if (typeof rowUsuario === "undefined") {
        webApp.showMessageDialog("Por favor seleccione un registro.");
    }
    else {
        var modelView = {
            Id: rowUsuario.Id
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
                    var tipodocumento = response.Data;
                    $("#codigo").val(tipodocumento.tdocc_vabreviatura_tipo_doc);
                    $("#descripcion").val(tipodocumento.tdocc_vdescripcion);
                    $("#UsuarioId").val(tipodocumento.Id);
                    $("#codigo").prop("disabled", true);
                    $("#accionTitle").text('Editar');
                    $("#NuevoTipoDocumento").modal("show");
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

function GuardarUsuario() {

    var modelView = {
        Id: $("#UsuarioId").val(),
        tdocc_vabreviatura_tipo_doc: $("#codigo").val(),
        tdocc_vdescripcion: $("#descripcion").val(),
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
                $("#NuevoTipoDocumento").modal("hide");
                dataTableTipoDocumento.ajax.reload();
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

function GuardarAccesoModulo(idTipoDocumento) {
    var idModulo = new Object();
    var modelView ;
    var tipodocumentoDTO = [];
    $.each(IdModulocheck.unique(), function (item,elem) {
        tipodocumentoDTO.push({ tablc_icod_modulo: elem, Id: idTipoDocumento, UsuarioRegistro: $("#usernameLogOn strong").text() });
        modelView = {
            tipodocumentoDTO: tipodocumentoDTO
        };
    });
        var idmodel = {
            IdModulo: $("#ModuloId").val(),
        }

        if (idmodel.IdModulo == 0)
            action = 'AddModulo';
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
                    $("#Nuevoacceso").modal("hide");
                    dataTableModulo.ajax.reload();
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


function EliminarUsuario(id) {
        var modelView = {
            Id: id,
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
                    $("#NuevoTipoDocumento").modal("hide");
                    dataTableTipoDocumento.row('.selected').remove().draw();

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

function LimpiarFormulario() {
    webApp.clearForm(formularioMantenimiento);
}

function CargarModulo() {
    webApp.Ajax({
        url: urlMantenimiento + 'GetAllModulos',
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
                $.each(response.Data, function (index, item) {
                    var contect = '<div class="ckbox ckbox-success col-sm-4">'
                                + ' <input name="checkbox" class="ace ace-checkbox-2" type="checkbox" id="TipoPermiso' + item.Id + '" data-permiso=' + item.Id + '>'
                                + ' <span class="lbl"> ' + item.tablc_vdescripcion + '</span>'
                                + '</div>';
                    $("#idContentModulos").append(contect);
                });
              
                //<input type="checkbox" id="TipoPermiso1" data-permiso="1"><label for="TipoPermiso1">Mostrar</label>
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

function CargarselecinadoModulos(id) {
    var modelView = {
        Id: id
    };
    webApp.Ajax({
        url: urlMantenimiento + 'GetIdModulo',
        async: false,
        parametros: modelView
    }, function (response) {
        if (response.Success) {
            if (response.Warning) {
                //$.gritter.add({
                //    title: 'Alerta',
                //    text: response.Message,
                //    class_name: 'gritter-warning gritter'
                //});
            } else {
                if (response.Data.length > 0) {
                    jQuery.each(response.Data, function (index, value) {
                        if (value.tablc_icod_modulo != 0) {
                            jQuery('#TipoPermiso' + value.tablc_icod_modulo).prop('checked', true);
                        }
                    });
               
                }
                else {
                    $('input[name=checkbox]').each(function (index, item) {
                        $(this).prop('checked', false);
                    });
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
    }, function (XMLHttpRequest, textStatus, errorThrown) {
    });
    //end
}

function cleanInputCheckList() {
    jQuery('#' + divCheckList + ' input[type=checkbox]').each(function (index, item) {
        jQuery(this).prop('checked', false);
    });
}


function CountInputCheckList() {
    jQuery('#' + divCheckList + ' input[type=checkbox]').each(function (index, item) {
        if ($(this).prop('checked')) {
            countCheck.push($(this).attr("data-permiso"));
        }
 
    });
}

//check fin