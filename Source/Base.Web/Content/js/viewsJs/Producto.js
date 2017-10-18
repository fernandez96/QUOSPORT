var dataTableProducto = null;
var formularioMantenimiento = "ProductoForm";
var formularioMantenimientoDatosGenerales = "datosProductoForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'Producto/Listar';
var urlMantenimiento = baseUrl + 'Producto/';
var rowProducto = null;
var tablaClasificacion = 6;
var Categoria = new Array();
var Linea = new Array();
var SubLinea = new Array();
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
        VisualizarDataTableProducto();
    });

    $('#ProductoDataTable  tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            dataTableProducto.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $('#btnAgregarProducto').on('click', function () {
        LimpiarFormulario();
        $("#Linea").empty();
        $("#SubLinea").empty();
        $("#UsuarioId").val(0);
        $("#accionTitle").text('Nuevo');
        $("#NuevoProducto").modal("show");
        $("#Username").prop("disabled", false);
    });

    $('#btnEditarProducto').on('click', function () {
        rowProducto = dataTableProducto.row('.selected').data();
        if (typeof rowProducto === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            if (rowProducto.Estado==estadoInactivo) {
                webApp.showMessageDialog('El Producto ' + '<b>' + rowProducto.prdc_vdescripcion + '</b>' + ' se encuentra  ' + '<span class="label label-warning arrowed-in arrowed-in-right">Inactivo</span>' + '.Si desea hacer un modificación le recomendamoos cambiarle de estado a dicho producto.');
            } else {
                checkSession(function () {
                    $("#Linea").empty();
                    $("#SubLinea").empty();
                    GetProductoById();
                });
            }          
        }

    });

    $('#btnestadoProducto').on('click', function () {
        rowProducto = dataTableProducto.row('.selected').data();
        if (typeof rowProducto === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            if (rowProducto.Estado == estadoInactivo) {
                webApp.showConfirmResumeDialog(function () {
                    checkSession(function () {
                        CambiarStatus(rowProducto.Id,estadoActivo);
                    });
                  
                }, 'El estado del producto ' + '<b>' + rowProducto.prdc_vdescripcion + '</b>' + '  es ' + '<span class="label label-warning arrowed-in arrowed-in-right">Inactivo</span>' + ', por lo tanto pasara a estado activo' + '</n>' + ' ¿Desea continuar?');
            } else if (rowProducto.Estado==estadoActivo) {
                webApp.showConfirmResumeDialog(function () {
                    checkSession(function () {
                        CambiarStatus(rowProducto.Id,estadoInactivo);
                    });

                }, 'El estado del producto ' + '<b>' + rowProducto.prdc_vdescripcion + '</b>' + '  es ' + '<span class="label label-info label-sm arrowed-in arrowed-in-right">Activo</span>' + ', por lo tanto pasara a estado Inactivo,' + '</n>' + ' ¿Desea continuar?');
            }
        }

    });

    $('#btnEliminarProducto').on('click', function () {
        rowProducto = dataTableProducto.row('.selected').data();
        if (typeof rowProducto === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            webApp.showDeleteConfirmDialog(function () {
                checkSession(function () {
                    EliminarProducto();
                });
            }, 'Se eliminará el registro. ¿Está seguro que desea continuar?');
        }

    });



    $("#btnSearchProducto").on("click", function (e) {
        if ($('#ProductoSearchForm').valid()) {
            checkSession(function () {
                dataTableProducto.ajax.reload();
            });
        }
        e.preventDefault();
    });

    $("#btnGuardarProducto").on("click", function (e) {
        if ($('#' + formularioMantenimiento).valid()) {
            checkSession(function () {
                GuardarProducto();
            });
        }
        e.preventDefault();
    });
    $('body').on('change', '#Categoria', function () {
        var id = $(this).val().split(',')[0]
        $("#Linea").empty();
        $.each(Linea, function (index, item) {
            if (item.ctgcc_iid_categoria === parseInt(id)) {
                $("#Linea").append('<option value="' + item.Id + ',' + item.linc_vcod_linea + '">' + item.linc_vdescripcion + '</option>');
            }
        });
    });
    $('body').on('change', '#Linea', function () {
        var id = $(this).val().split(',')[0];
        $("#SubLinea").empty();
        $.each(SubLinea, function (index, item) {
            if (item.idLinea === parseInt(id)) {
                $("#SubLinea").append('<option value="' + item.Id + ',' + item.lind_vcod_sublinea + '">' + item.lind_vdescripcion + '</option>');
            }
        });
    });
 
  
    $("#serie,#UPC,#EAN").mask("9999-9999999");
    webApp.validarLetrasEspacio(['descripcion','color']);
    webApp.validarNumerico(['UPC', 'serie', 'orden', 'EAN']);
    webApp.InicializarValidacion(formularioMantenimiento,
        {
            Categoria: {
                required: true
            },
            Linea: {
                required: true
            },
            SubLinea: {
                required: true
            },
            descripcion:{
                required: true
            },
            unidad: {
                required: true
            }
        },
        {
            Categoria: {
                required: "Por favor seleccione Categoria.",
            },
            Linea: {
                required: "Por favor seleccione Linea.",
            },
            SubLinea: {
                required: "Por favor seleccione Sub-Linea.",

            },
            descripcion: {
                required: "Por favor seleccione Descripción.",
            },
            unidad: {
                required: "Por favor seleccione Unidad medida.",

            }
        });

    CargarMedidas();
    CargarClasificaciones();
    CargarCategoria();
    CargarLinea();
    CargarSubLineas();
    CargarEstado();
    $('[data-toggle="tooltip"]').tooltip();
 

    $('textarea.limited').inputlimiter({
        remText: '%n caracter%s restantes...',
        limitText: 'permite maximo : %n.'
    });
});

function VisualizarDataTableProducto() {
    dataTableProducto = $('#ProductoDataTable').DataTable({
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
                    //codigoSearch: $("#Codigosearch").val(),
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
            //{ "data": "prdc_vcod_producto" },
            { "data": "prdc_vdescripcion" },
            { "data": "ctgc_v_categoria" },
            { "data": "linc_v_linea" },
             { "data": "lind_v_sublinea" },
            { "data": "tablc_v_iclasif_prod" },
            { "data": "prdc_vorder_code" },
            { "data": "umec_v_unidad_medida" },
            {
                "data": function (obj) {
                    if (obj.Estado == 1){
                        return '<span class="label label-info label-sm arrowed-in arrowed-in-right">Activo</span>';
                    }
                    else if(obj.Estado==2){
                        return '<span class="label label-warning arrowed-in arrowed-in-right">Inactivo</span>';
                    }
                       
                }
            }
        ],
        "aoColumnDefs": [

            { "bVisible": false, "aTargets": [0] },
            //{ "className": "hidden-120", "aTargets": [1], "width": "10%" },
            { "className": "hidden-120", "aTargets": [1], "width": "20%" },
            { "className": "hidden-992", "aTargets": [2], "width": "12%" },
            { "className": "hidden-768", "aTargets": [3], "width": "12%" },
            { "className": "hidden-600", "aTargets": [4], "width": "12%" },
            { "className": "hidden-600", "aTargets": [5], "width": "10%" },
            { "className": "hidden-1200", "aTargets": [6], "width": "10%" },
              { "className": "hidden-1200", "aTargets": [7], "width": "7%" },
            { "bSortable": false, "className": "hidden-480", "aTargets": [8], "width": "7%" }

        ],
        "order": [[1, "desc"]],
        "initComplete": function (settings, json) {
            // AddSearchFilter();
        },
        "fnDrawCallback": function (oSettings) {

        }
    });
}

function GetProductoById() {
    var modelView = {
        Id: rowProducto.Id
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
                var producto = response.Data;
       
                Categoria.filter(function(obj){
                    if (obj.Id===producto.ctgc_iid_categoria) {
                        $("#Categoria").val(producto.ctgc_iid_categoria + ',' + obj.ctgcc_vcod_categoria);
                        return true;
                    }
                    else {
                        return false;
                    }
                });

                Linea.filter(function (obj) {
                    if (obj.ctgcc_iid_categoria === producto.ctgc_iid_categoria && obj.Id === producto.linc_iid_linea) {
                        $("#Linea").append('<option value="' + producto.linc_iid_linea + ',' + obj.linc_vcod_linea + '">' + obj.linc_vdescripcion + '</option>');
                        $("#Linea").val(producto.linc_iid_linea + ',' + obj.linc_vcod_linea);
                        return true;
                    }
                    else {
                        return false;
                    }
                });

                SubLinea.filter(function (obj) {
                    if (obj.idLinea === producto.linc_iid_linea && obj.Id === producto.lind_iid_sublinea) {
                        $("#SubLinea").append('<option value="' + producto.lind_iid_sublinea + ',' + obj.lind_vcod_sublinea + '">' + obj.lind_vdescripcion + '</option>');
                        $("#SubLinea").val(producto.lind_iid_sublinea + ',' + obj.lind_vcod_sublinea);
                        return true;
                    }
                    else {
                        return false;
                    }
                });
                $("#correlativo").val(producto.prdc_vcod_producto.toString().substring(3,producto.prdc_vcod_producto.length));
                $("#descripcion").val(producto.prdc_vdescripcion);
                $("#clasificacion").val(producto.tablc_iid_iclasif_prod);
                $("#stock").val(producto.prdc_dstock_minimo);
                $("#precio").val(producto.prdc_dpeso_unitario);
                $("#unidad").val(producto.umec_iid_unidad_medida);
                $("#material").val(producto.prdc_vmaterial1);
                $("#material2").val(producto.prdc_vmaterial2);
                $("#EAN").val(producto.prdc_vcodigo_ean);
                $("#UPC").val(producto.prdc_vcodigo_upc);
                $("#orden").val(producto.prdc_vorder_code);
                $("#color").val(producto.prdc_vcolor);
                $("#serie").val(producto.prdc_vnumero_serie);
                $("#ProductoId").val(producto.Id);
                $("#textDescripcion").val(producto.prdc_vcaracteristicas);
                $("#Estado").val(producto.Estado);
                $("#accionTitle").text('Editar');
                $("#NuevoProducto").modal("show");
                
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

function EliminarProducto() {
    var modelView = {
        Id: rowProducto.Id,
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
                $("#NuevoProducto").modal("hide");
                dataTableProducto.row('.selected').remove().draw();
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

function GuardarProducto() {

    var modelView = {
        Id: $("#ProductoId").val(),
        ctgc_iid_categoria: $("#Categoria").val().split(',')[0],
        linc_iid_linea: $("#Linea").val().split(',')[0],
        lind_iid_sublinea: $("#SubLinea").val().split(',')[0],
        prdc_vcod_producto: $("#Categoria").val().split(',')[1] + '-' + $("#Linea").val().split(',')[1] + '-' + $("#SubLinea").val().split(',')[1],
        prdc_vdescripcion: $("#descripcion").val(),
        tablc_iid_iclasif_prod: $("#clasificacion").val(),
        prdc_dstock_minimo: $("#stock").val(),
        prdc_dpeso_unitario: $("#precio").val(),
        umec_iid_unidad_medida: $("#unidad").val(),
        prdc_vmaterial1: $("#material").val(),
        prdc_vmaterial2: $("#material2").val(),
        prdc_vcodigo_ean: $("#EAN").val(),
        prdc_vcodigo_upc: $("#UPC").val(),
        prdc_vorder_code: $("#orden").val(),
        prdc_vcolor: $("#color").val(),
        prdc_vnumero_serie: $("#serie").val(),
        prdc_vcaracteristicas: $("#textDescripcion").val(),
        UsuarioRegistro: $("#usernameLogOn strong").text(),
        Estado:$("#Estado").val()
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
                $("#NuevoProducto").modal("hide");
                dataTableProducto.ajax.reload();
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

function CargarCategoria() {
    $("#Categoria").empty();
    webApp.Ajax({
        url: urlMantenimiento + 'ListarCategoria',
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
                if (response.Data !==null) {
                    $.each(response.Data, function (index, item) {
                        $("#Categoria").append('<option value="' + item.Id + ',' + item.ctgcc_vcod_categoria + '">' + item.ctgcc_vdescripcion + '</option>');
                    });
                    Categoria = response.Data;
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

function CargarLinea() {
    $("#Linea").empty();
    Linea.length = 0;
    webApp.Ajax({
        url: urlMantenimiento + 'ListarLinea',
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
              
                Linea = response.Data;
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

function CargarSubLineas() {
    $("#SubLinea").empty();
    SubLinea.length = 0;
    webApp.Ajax({
        url: urlMantenimiento + 'ListarSubLinea',
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
                SubLinea = response.Data;
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

function CargarMedidas() {
    webApp.Ajax({
        url: urlMantenimiento + 'GetAllUnidades',
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
                    $("#unidad").append('<option value="' + item.Id + '">' + item.umec_vdescripcion + '</option>');
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

function CargarClasificaciones() {
    var modelView = {
        idtabla: tablaClasificacion
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
                    $("#clasificacion").append('<option value="' + item.Id + '">' + item.tbpd_vdescripcion_detalle + '</option>');
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


function CargarEstado() {
    var modelView = {
        idtabla: 1
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
                    $("#Estado").append('<option value="' + item.Id + '">' + item.tbpd_vdescripcion_detalle + '</option>');
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

function CambiarStatus(Id,estado) {
    var modelView = {
        Id: Id,
        Estado:estado
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
                dataTableProducto.ajax.reload();
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
        if ($('#ProductoSearchForm').valid()) {
            checkSession(function () {
                dataTableProducto.ajax.reload();
            });
        }
    }
}

function AddSearchFilter() {
    $("#UsuarioDataTable_wrapper").prepend($("#searchFilterDiv").html());
}

function LimpiarFormulario() {
    webApp.clearForm(formularioMantenimiento);
    $("#clasificacion").val(8);
    $("#textDescripcion").val('');
    $("#unidad").val(1);
    $("#descripcion").focus();
    $("#Estado").val(1);

}