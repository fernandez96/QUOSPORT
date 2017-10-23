var dataTableNotaIngreso = null;
var dataTableNotaIngresoDetalle = null;
var dataTableProducto = null;
var formularioMantenimiento = "NotaIngresoForm";
var formularioMantenimientoDetalle = "ProductoDetalleForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'NotaIngreso/Listar';
var urlMantenimiento = baseUrl + 'NotaIngreso/';
var urlListaCargo = baseUrl + 'NotaIngreso/';
var rowNotaIngreso = null;
var urlListarProductos = baseUrl + 'Producto/Listar';
var ActualizacionFallida = "No se pudo realizar la actualización.";
var ActualizacionSatisfactoria = "Se realizó la actualización satisfactoriamente.";
var RegistroSatisfactorio = "Se realizó el registro satisfactoriamente.";
var RegistroFallido = "No se pudo realizar el registro.";
var EliminacionSatisfactoria = "Se realizó la eliminación satisfactoriamente.";
var EliminacionFallida = "No se pudo realizar la eliminación.";
var YaExisteRegistro = "El registro ya existe.";
var IntenteloMasTarde = "Hubo un error, inténtelo más tarde.";
var TitleRegistro = "Registro Satisfactorio";
var TitleActualizar = "Actualización Satisfactoria";
var TitleEliminar = "Eliminación Satisfactoria";
var TitleAlerta = "Alerta";
var NotaIngresoDetalle = new Array();
var editor;
var rowProducto = null;
var rowNotaIngreso = null;
var agregar = 1;
var editar = 2;
var eliminar = 3;

$(document).ready(function () {
    $.extend($.fn.dataTable.defaults, {
        language: { url: baseUrl + 'Content/js/dataTables/Internationalisation/es.txt' },
        responsive: true,
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "bProcessing": true,
        "dom": 'fltip'
    });
    $.extend($.gritter.options, {
        time: '1000'
    });
    checkSession(function () {
        VisualizarDataTableNotaIngreso();
        VisualizarDataTableNotaIngresoDetalle();
        VisualizarDataTableProducto();
    });

    $('#NotaIngresoDetalleDataTable  tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            dataTableNotaIngresoDetalle.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
            
        }
    });
    $('#ProductoDataTable  tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            dataTableProducto.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
            rowProducto = dataTableProducto.row('.selected').data();
            setTimeout(function () {
                $("#codigoP").val('');
                $("#unidad").val(rowProducto.umec_v_unidad_medida);
                $("#descripcionP").val(rowProducto.prdc_vdescripcion);
                $("#cantidad").focus();
                $("#ProductoModal").modal("hide");
            },100);
            

        }
    });

 
    $('#btnAgregarNotaIngreso').on('click', function () {
        LimpiarFormulario();
        correlativaNotaIngreso();
        $("#NotaIngresoId").val(0);
        $("#accionTitle").text('Nuevo');
        $("#NuevaNotaIngreso").modal("show");
    });
    //agregar detalle
												
    $("#btnAgregarNotaIngresoDetalle").on("click", function () {
        LimpiarFormularioDetalle();
       
        $("#accionTitleProducto").text("Nueva");
        $("#productoBuscar").prop("disabled", false);
        $("#NuevoDetalleProducto").modal("show");

    });

    $("#btnGuardarProducto").on("click", function () {
        if ($("#unidad,#descripcionP").val()=="") {
            webApp.showMessageDialog("Por favor seleccione un Producto.");
        }
        else {
            if ($('#' + formularioMantenimientoDetalle).valid()) {
                checkSession(function () {
                    AddProductoDraw();
                });
            }
        }
       
      
    });

    $("#btnEditarTablaDetalle").on("click", function () {
        rowNotaIngreso = dataTableNotaIngresoDetalle.row('.selected').data();
        if (typeof rowNotaIngreso === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            checkSession(function () {
                GetByProductoDraw();
            });
        }
    });

    $("#btnEliminarTablaDetalle").on("click", function () {
        rowNotaIngreso = dataTableNotaIngresoDetalle.row('.selected').data();
        if (typeof rowNotaIngreso === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else{
            webApp.showDeleteConfirmDialog(function () {
                checkSession(function () {
                    DeleteProductoDraw();
                });
            }, 'Se eliminará el registro. ¿Está seguro que desea continuar?');
        }
    });

    $("#productoBuscar").on("click", function () {
        dataTableProducto.ajax.reload();
        $("#ProductoModal").modal("show");
    });
  
    webApp.validarNumerico(['costo', 'cantidad']);
    webApp.InicializarValidacion(formularioMantenimientoDetalle,
       {
           costo: {
               required: true,
           },
           cantidad: {
               required: true,
           },

       },
       {
           costo: {
               required: "Por favor ingrese Costo",
           },
           cantidad: {
               required: "Por favor ingrese Cantidad",
           },

       }

       );

    $('[data-toggle="tooltip"]').tooltip();
    //or change it into a date range picker
    
    $('#observacion').inputlimiter({
        remText: '%n caracter%s restantes...',
        limitText: 'permite maximo : %n.'
    });
   
    //$("#costo").numeric({ decimal: ".", negative: false, scale: 3 });
    //$("#costo").inputmask('00.00', { regex: "^[0-9]{1,6}(\\.\\d{1,2})?$" });
    //$('#costo').mask('#,##0.00', { reverse: true });
    //$('#cantidad').mask('#,##0.00', { reverse: true });
});
function VisualizarDataTableNotaIngreso() {
    dataTableNotaIngreso = $('#NotaIngresoDataTable').DataTable({
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
            { "data": "ningc_numero_nota_ingreso" },
            { "data": "ningc_fecha_nota_ingreso" },
            { "data": "ningc_v_motivo" },
            { "data": "ningc_observaciones" },
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
            { "className": "center hidden-120", "aTargets": [1], "width": "10%" },
            { "className": "center hidden-120", "aTargets": [2], "width": "14%" },
            { "className": "center hidden-120", "aTargets": [3], "width": "10%" },
             { "className": "hidden-992", "aTargets": [4], "width": "35%" },
            { "bSortable": false, "className": "hidden-992", "aTargets": [5], "width": "7%" }

        ],
        "order": [[1, "desc"]],
        "initComplete": function (settings, json) {
            // AddSearchFilter();
        },
        "fnDrawCallback": function (oSettings) {

        }
    });
}

function VisualizarDataTableNotaIngresoDetalle() {
    dataTableNotaIngresoDetalle = $('#NotaIngresoDetalleDataTable').DataTable({
        "bFilter": false,
        "bProcessing": true,
        responsive: true,
        "lengthMenu": [[5], [5]],
        "dom": 'fltip',
        "data": NotaIngresoDetalle,
        dataFilter: function (data) {
            if (data.substring(0, 9) == "<!DOCTYPE") {
                redireccionarLogin("Sesión Terminada", "Se terminó la sesión");
            } else {
                return data;
                //var json = jQuery.parseJSON(data);
                //return JSON.stringify(json); // return JSON string
            }
        },
     
        "bAutoWidth": false,
        "columns": [
            { "data": "id"},
            { "data": "item"},
            { "data": "Producto" ,className:'editable'},
            { "data": "Descripcion", editField: "Descripcion" },
            { "data": "UM" },
            { "data": "Costo", render: $.fn.dataTable.render.number( ',', '.', 0, 'S/ ' )  },
            { "data": "Cantidad" }
         
        ],
        "aoColumnDefs": [

            { "bVisible": false, "aTargets": [0] },
            { "className": "center hidden-120", "aTargets": [1], "width": "5%" },
            { "className": "hidden-120", "aTargets": [2], "width": "18%" },
            { "className": "hidden-992", "aTargets": [3], "width": "27%" },
            { "className": "hidden-992", "aTargets": [4], "width": "10%" },
            { "className": "center hidden-992", "aTargets": [5], "width": "7%" },
            { "className": "center hidden-120", "aTargets": [6], "width": "7%" },
       

        ],
        "order": [[1, "asc"]],
        "initComplete": function (settings, json) {
            // AddSearchFilter();
        },
        "fnDrawCallback": function (oSettings) {

        }
    });
}

function VisualizarDataTableProducto() {
    dataTableProducto = $('#ProductoDataTable').DataTable({
        "bFilter": false,
        "bProcessing": true,
        "serverSide": true,
        //"scrollY": "350px",              
        "ajax": {
            "url": urlListarProductos,
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
            { "data": "umec_v_unidad_medida" },
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
            //{ "className": "hidden-120", "aTargets": [1], "width": "10%" },
            { "className": "hidden-120", "aTargets": [1], "width": "20%" },
            { "className": "hidden-120", "aTargets": [2], "width": "12%" },
            { "className": "hidden-120", "aTargets": [3], "width": "12%" },
       
            { "bSortable": false, "className": "hidden-480", "aTargets": [3], "width": "7%" }

        ],
        "order": [[1, "desc"]],
        "initComplete": function (settings, json) {
            // AddSearchFilter();
        },
        "fnDrawCallback": function (oSettings) {

        }
    });
}

function AddProductoDraw() {
    var idProducto = $("#ProductoId").val();
    var codigo = $("#codigoP").val();
    var unidad = $("#unidad").val();
    var descripcion = $("#descripcionP").val();
    var cantidad = $("#cantidad").val()+".00";
    var costo = $("#costo").val()+".00";
    var detalle = null;
    var exito = true;
    var editarStatus = false;
    if (idProducto == 0) {
        if (NotaIngresoDetalle.length > 0) {
            $.each(NotaIngresoDetalle, function (index, value) {
                if (value.Id == rowProducto.Id && value.status != eliminar) {
                    exito = false;
                }
            });
            if (exito) {
                detalle = { "Id": rowProducto.Id, "dninc_nro_item": CorrelativoProducto(), "Producto": codigo, "Descripcion": descripcion, "UM": unidad, "Costo": costo, "Cantidad": cantidad, "Estado": 1, "status": agregar };
                NotaIngresoDetalle.push(detalle);
                dataTableNotaIngresoDetalle.clear();
                dataTableNotaIngresoDetalle.rows.add(NotaIngresoDetalle.filter(function (e) {
                    if (e.status != eliminar) {
                        return true
                    }
                    else {
                        return false;
                    }

                })).draw();
                $.gritter.add({
                    title: TitleRegistro,
                    text: RegistroSatisfactorio,
                    class_name: 'gritter-success gritter'
                });
                $("#NuevoDetalleProducto").modal("hide");
            }
            else {
                $.gritter.add({
                    title: TitleAlerta,
                    text: YaExisteRegistro,
                    class_name: 'gritter-warning gritter',
                });
            }
        }
        else {
            detalle = { "Id": rowProducto.Id, "item": CorrelativoProducto(), "Producto": codigo, "Descripcion": descripcion, "UM": unidad, "Costo": costo, "Cantidad": cantidad, "Estado": 1, "status": agregar };
            NotaIngresoDetalle.push(detalle);
            dataTableNotaIngresoDetalle.rows.add(NotaIngresoDetalle).draw();
            $("#NuevoDetalleProducto").modal("hide");
            $.gritter.add({
                title: TitleRegistro,
                text: RegistroSatisfactorio,
                class_name: 'gritter-success gritter'
            });
        
        }
    }
    else {

        $.each(NotaIngresoDetalle, function (index, value) {
            if (value.Id==rowNotaIngreso.Id && value.status!=eliminar) {
                value.Costo = costo;
                value.Cantidad = cantidad;
                value.status = editar;
                editarStatus = true;
            }

        });
        if (editarStatus) {
            dataTableNotaIngresoDetalle.clear().draw();

            dataTableNotaIngresoDetalle.rows.add(NotaIngresoDetalle.filter(function (obj) {
                if (obj.status!=eliminar) {
                    return true;
                }
                else {
                    return false;
                }
            })).draw();
       
         
            $.gritter.add({
                title: TitleActualizar,
                text: ActualizacionSatisfactoria,
                class_name: 'gritter-success gritter'
            });
            $("#NuevoDetalleProducto").modal("hide");
        }
        else {
            $.gritter.add({
                title: TitleAlerta,
                text: IntenteloMasTarde,
                class_name: 'gritter-warning gritter',
            });
        }

    }

}

function GetByProductoDraw() {
    $("#accionTitleProducto").text("Editar");
    $("#productoBuscar").prop("disabled", true);
    $("#ProductoId").val(rowNotaIngreso.Id);
    $("#codigoP").val(rowNotaIngreso.Producto);
    $("#unidad").val(rowNotaIngreso.UM);
    $("#descripcionP").val(rowNotaIngreso.Descripcion);
    $("#cantidad").val(rowNotaIngreso.Cantidad);
    $("#costo").val(rowNotaIngreso.Costo);
    $("#NuevoDetalleProducto").modal("show");
}

function DeleteProductoDraw() {
    var statusEliminacion = false;

    $.each(NotaIngresoDetalle, function (index, value) {
        if (value.Id==rowNotaIngreso.Id && value.status!=eliminar) {
            value.status = eliminar;
            statusEliminacion = true;
        } 

    });
    if (statusEliminacion) {
        dataTableNotaIngresoDetalle.clear().draw();
        dataTableNotaIngresoDetalle.rows.add(NotaIngresoDetalle.filter(function (obj) {
            if (obj.status != eliminar) {
                return true;
            }
            else {
                return false;
            }
        })).draw();
        $.gritter.add({
            title: TitleEliminar,
            text: EliminacionSatisfactoria,
            class_name: 'gritter-success gritter'
        });
    }
    else {
        $.gritter.add({
            title: TitleAlerta,
            text: IntenteloMasTarde,
            class_name: 'gritter-warning gritter'
        });
    }
}

function correlativaNotaIngreso() {
    var correlativo = '000001';
    $("#nroNI").val(correlativo);
    
}

function CorrelativoProducto() {
    var correlativo = "";
    if (NotaIngresoDetalle.length>0) {
        var jquery = JSLINQ(NotaIngresoDetalle)
                      .Where(function (v) { return v.status!=eliminar})
                      .OrderByDescending(function (o) { return o.item })
                      .Select(function (item) { return parseInt(item.item) });
        var correlativoValue = parseInt(jquery.items[0] + 1);
        if (correlativoValue < 10) {
            correlativo = '00' + correlativoValue;
        }
        if (correlativoValue >= 10) {
            correlativo='0' + correlativoValue;
        }
        if (correlativoValue >= 100) {
            correlativo=correlativoValue;
        }
    }
    else {
        correlativo = "001";
    }
    return correlativo;
}

function createRows(data) {

    return (
    `<tr>` +
      //`<td>${data.id=$("tbody tr").length + 1}</td>` +
      `<td> ${data.item}</td>` +
      `<td> ${contenect} </td>` +
      `<td>${data.Descripcion}</td>` +
      `<td>${data.UM}</td>` +
      `<td><input type="text"></input></td>` +
      `<td>${data.Estado}</td>` +
    `</tr>`
  );
    setTimeout(function () {
        $('#chosenProducto').chosen({ allow_single_deselect: true });
        $('#chosenProducto').trigger('chosen:updated');
    }, 1000);
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

function AddSearchFilter() {
    $("#UsuarioDataTable_wrapper").prepend($("#searchFilterDiv").html());
}

function buscar(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    if (tecla == 13) {
        if ($('#UsuarioSearchForm').valid()) {
            checkSession(function () {
                dataTableUsuario.ajax.reload();
            });
        }
    }
}

function LimpiarFormulario() {
    webApp.clearForm(formularioMantenimiento);
    NotaIngresoDetalle.length = 0;
    dataTableNotaIngresoDetalle.clear().draw();
    $("#CargoId").val(1);
    $('#fecha').datepicker({
        autoclose: true,
        language: 'es',
        format: 'dd/mm/yyyy'
    }).datepicker('setDate', new Date());


}

function LimpiarFormularioDetalle() {
    webApp.clearForm(formularioMantenimientoDetalle);
}