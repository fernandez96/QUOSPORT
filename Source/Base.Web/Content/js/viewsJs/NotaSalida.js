var dataTableNotaIngreso = null;
var dataTableNotaIngresoDetalle = null;
var dataTableProducto = null;
var formularioMantenimiento = "NotaIngresoForm";
var formularioMantenimientoDetalle = "ProductoDetalleForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'NotaSalida/Listar';
var urlMantenimiento = baseUrl + 'NotaSalida/';
var urlMantenimientoAlmacen = baseUrl + 'Almacen/';
var urlListaCargo = baseUrl + 'NotaIngreso/';
var urlMantenimientoReport = baseUrl + 'Reporte/';
var urlListarProductoStock = baseUrl + 'Producto/ListarProductoStockGetAll';
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
var tipoMovimiento = 1;
var TitleAlerta = "Alerta";
var NotaIngresoDetalle = new Array();
var ProductoArray = new Array();
var editor;
var rowNotaIngreso = null;
var rowProducto = null;
var rowNotaIngresoDetalle = null;
var agregar = 1;
var editar = 2;
var eliminar = 3;
var tipoDocumento = 2;
var stockActual;
var fechaIncio = null;
var fecchaFin = null;
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
        CargarAlmacen();
        CargarMotivo();
    });
    $('#NotaIngresoDataTable  tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            dataTableNotaIngreso.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
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
                stockActual = rowProducto.prdc_dstock_minimo;
                $("#cantidad").focus();
                $("#ProductoModal").modal("hide");
            }, 100);
        }
    });


    $('#btnAgregarNotaIngreso').on('click', function () {
        LimpiarFormulario();
        GetCorrelativoCab($("#almacen").val());
        CargarProducto($("#almacen").val());
        
        $("#NotaIngresoId").val(0);
        $("#accionTitle").text('Nuevo');
        $("#NuevaNotaIngreso").modal("show");
    });
    //agregar detalle

    $("#btnAgregarNotaIngresoDetalle").on("click", function () {
        LimpiarFormularioDetalle();
        $("#NotaIngresoDetalleId").val(0);
        $("#accionTitleProducto").text("Nueva");
        $("#productoBuscar").prop("disabled", false);
        $("#NuevoDetalleProducto").modal("show");

    });
    $("#btnEditarNotaIngreso").on("click", function () {
        rowNotaIngreso = dataTableNotaIngreso.row('.selected').data();
        if (typeof rowNotaIngreso === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            checkSession(function () {
                GetNotaIngresoById(rowNotaIngreso.Id);
                GetNotaIngresoDetalleAll(rowNotaIngreso.Id);
            });
        }
    });
    $('#btnEliminarNotaIngreso').on('click', function () {
        rowNotaIngreso = dataTableNotaIngreso.row('.selected').data();
        if (typeof rowNotaIngreso === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            webApp.showDeleteConfirmDialog(function () {
                checkSession(function () {
                    EliminarNataIngreso(rowNotaIngreso.Id);
                });
            }, 'Se eliminará el registro. ¿Está seguro que desea continuar?');
        }
    });

    $("#btnGuardarNotaIngreso").on("click", function () {
        if (NotaIngresoDetalle.filter(function (obj) { if (obj.status != eliminar) { return true } else { return false } }).length <= 0) {
            webApp.showMessageDialog("Por favor debe ingresar al menos un producto.");
        }
        else {
            if ($('#' + formularioMantenimiento).valid()) {
                checkSession(function () {
                    GuardarNotaIngreso();
                });
            }
        }
    });

    $("#btnGuardarProducto").on("click", function () {
        if ($("#unidad,#descripcionP").val() == "") {
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
        rowNotaIngresoDetalle = dataTableNotaIngresoDetalle.row('.selected').data();
        if (typeof rowNotaIngresoDetalle === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            checkSession(function () {
                GetByProductoDraw();
            });
        }
    });

    $("#btnEliminarTablaDetalle").on("click", function () {
        rowNotaIngresoDetalle = dataTableNotaIngresoDetalle.row('.selected').data();
        if (typeof rowNotaIngresoDetalle === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            webApp.showDeleteConfirmDialog(function () {
                checkSession(function () {
                    DeleteProductoDraw();
                });
            }, 'Se eliminará el registro. ¿Está seguro que desea continuar?');
        }
    });

    $("#productoBuscar").on("click", function () {
        SelectProductoStock();
        $("#ProductoModal").modal("show");
    });

    $('#btnImprimir').on('click', function () {
        rowNotaIngreso = dataTableNotaIngreso.row('.selected').data();
        if (typeof rowNotaIngreso === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            checkSession(function () {
                Imprimir(rowNotaIngreso.Id);
            });
        }
    });

    $('body').on('change', '#almacen', function () {
        GetCorrelativoCab(this.value);
        dataTableProducto.clear().draw();
        CargarProducto(this.value);
        console.log(ProductoArray);
    });

    $("#btnSearchNotaIngreso").on("click", function (e) {
        if ($('#NotaIngresoSearchForm').valid()) {
            checkSession(function () {
         
                dataTableNotaIngreso.ajax.reload();
            });
        }
        e.preventDefault();
    });

    $("#btnSearchTablaproducto").on("click", function (e) {
        if ($('#ProductoSearchForm').valid()) {
            checkSession(function () {
        
            });
        }
        e.preventDefault();
    });
    webApp.validarNumerico(['costo', 'cantidad']);
    webApp.InicializarValidacion(formularioMantenimientoDetalle,
       {
           cantidad: {
               required: true,
           },

       },
       {
           cantidad: {
               required: "Por favor ingrese Cantidad",
           },
       }
       );

    webApp.InicializarValidacion(formularioMantenimiento,
      {
          almacen: {
              required: true,
          },
          nroNI: {
              required: true,
          },
          fecha: {
              required: true
          },
          motivo: {
              required: true
          }
      },
      {
          almacen: {
              required: "Por favor seleccione Almacen.",
          },
          nroNI: {
              required: "Por favor falta generar numero de nota de ingreso.",
          },
          fecha: {
              required: "Por favor seleccione fecha.",
          },
          motivo: {
              required: "Por favor seleccione Motivo.",
          },
      }
      );

    $('[data-toggle="tooltip"]').tooltip();

    $('#observacion').inputlimiter({
        remText: '%n caracter%s restantes...',
        limitText: 'permite maximo : %n.'
    });
    //$('#fechasearch').datepicker({
    //    autoclose: true,
    //    language: 'es',
    //    format: 'dd/mm/yyyy'
    //});
  
    $('#fechasearch').daterangepicker({
        'applyClass': 'btn-sm btn-success',
        'cancelClass': 'btn-sm btn-default',
        language: 'es',
        locale: {
            applyLabel: 'Aplicar',
            cancelLabel: 'Cancelar',
        }
    })
    .prev().on(ace.click_event, function () {
        $(this).next().focus();
    });
    $('#fechasearch').on('apply.daterangepicker', function (ev, picker) {
        $(this).val('De : '+ picker.startDate.format('DD/MM/YYYY') + '   Hasta : ' + picker.endDate.format('DD/MM/YYYY'));
        fechaIncio = picker.startDate.format('DD/MM/YYYY');
        fecchaFin = picker.endDate.format('DD/MM/YYYY');
    });

    $('#fechasearch').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
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
                    numeroSearch: $("#numerosearch").val(),
                    fechaInicioSearch: fechaIncio,
                    fechaFinSearch:fecchaFin
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
            { "data": "nsalc_numero_nota_salida" },
            { "data": "almac_vdescripcion" },
            { "data": function (obj) { return GetFechaSubString(obj.fecha);}},
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
              { "className": "center hidden-120", "aTargets": [2], "width": "13%" },
            { "className": "center hidden-120", "aTargets": [3], "width": "8%" },
            { "className": "center hidden-992", "aTargets": [4], "width": "16%" },
             { "className": "hidden-1200", "aTargets": [5], "width": "30%" },
            { "bSortable": false, "className": "hidden-1200", "aTargets": [6], "width": "7%" }

        ],
        "order": [[1, "ASC"]],
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
            { "data": "Id" },
            { "data": "nsald_nro_item" },
            { "data": "prdc_vdescripcion" },
            { "data": "dninc_v_unidad" },
            { "data": "nsald_cantidad", render: $.fn.dataTable.render.number(',', '.', 2) }

        ],
        "aoColumnDefs": [

            { "bVisible": false, "aTargets": [0] },
            { "className": "center hidden-120", "aTargets": [1], "width": "5%" },
            { "className": "hidden-120", "aTargets": [2], "width": "27%" },
            { "className": "hidden-992 center", "aTargets": [3], "width": "10%" },
            { "className": "center hidden-120", "aTargets": [4], "width": "7%" },


        ],
        "order": [[1, "ASC"]],
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
        "serverSide": false,
        //"scrollY": "350px",              
        "data":ProductoArray,
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
            { "data": "Id" },
            //{ "data": "prdc_vcod_producto" },
            { "data": "prdc_vdescripcion" },
            { "data": "umec_v_unidad_medida" },
            { "data": "prdc_dstock_minimo", render: $.fn.dataTable.render.number(',', '.', 2) },
        ],
        "aoColumnDefs": [

            { "bVisible": false, "aTargets": [0] },
            //{ "className": "hidden-120", "aTargets": [1], "width": "10%" },
            { "className": "hidden-120", "aTargets": [1], "width": "20%" },
            { "className": "hidden-120", "aTargets": [2], "width": "12%" },
            { "className": "hidden-120", "aTargets": [3], "width": "12%" },


        ],
        "order": [[1, "ASC"]],
        "initComplete": function (settings, json) {
            // AddSearchFilter();
        },
        "fnDrawCallback": function (oSettings) {

        }
    });
}

function AddProductoDraw() {
    var idNotaIngresoDetalle = $("#NotaIngresoDetalleId").val();
    var codigo = $("#codigoP").val();
    var unidad = $("#unidad").val();
    var descripcion = $("#descripcionP").val();
    var cantidad = $("#cantidad").val();
    var detalle = null;
    var exito = true;
    var editarStatus = false;
    if (idNotaIngresoDetalle == 0) {
        if (NotaIngresoDetalle.length > 0) {
            $.each(NotaIngresoDetalle, function (index, value) {
                if (value.prdc_icod_producto == rowProducto.Id && value.status != eliminar) {
                    exito = false;
                }
            });
            if (exito) {
                if (parseInt(stockActual) >= parseInt(cantidad)) {
                    detalle = { "Id": NotaIngresoDetalle.length + 1, "prdc_icod_producto": rowProducto.Id, "kardc_icod_correlativo": NotaIngresoDetalle.length + 1, "nsald_nro_item": CorrelativoProducto(), "prdc_vdescripcion": descripcion, "dninc_v_unidad": unidad, "nsald_cantidad": cantidad, "Estado": 1, "status": agregar, "kardc_tipo_movimiento": tipoMovimiento };
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
                    $.each(ProductoArray, function (index, value) {
                        if (value.Id===rowProducto.Id) {
                            value.prdc_dstock_minimo = parseInt(value.prdc_dstock_minimo) - parseInt(cantidad);
                        }
                    });
                    $("#NuevoDetalleProducto").modal("hide");
                } else {
                    webApp.showMessageDialog("La cantidad ingresada es mayor al stock del producto "+ descripcion +",se corrimienda que debe ser menor a "+ stockActual +".");
                }
                
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
            if (parseInt(stockActual)>= parseInt(cantidad)) {
                detalle = { "Id": 1, "prdc_icod_producto": rowProducto.Id, "kardc_icod_correlativo": 1, "nsald_nro_item": CorrelativoProducto(), "prdc_vdescripcion": descripcion, "dninc_v_unidad": unidad, "nsald_cantidad": cantidad, "Estado": 1, "status": agregar, "kardc_tipo_movimiento": tipoMovimiento };
                NotaIngresoDetalle.push(detalle);
                dataTableNotaIngresoDetalle.rows.add(NotaIngresoDetalle).draw();
                $("#NuevoDetalleProducto").modal("hide");
                $.gritter.add({
                    title: TitleRegistro,
                    text: RegistroSatisfactorio,
                    class_name: 'gritter-success gritter'
                });
                $.each(ProductoArray, function (index, value) {
                    if (value.Id === rowProducto.Id) {
                        value.prdc_dstock_minimo = parseInt(value.prdc_dstock_minimo) - parseInt(cantidad);
                    }
                });
            }
            else
            {
                webApp.showMessageDialog("La cantidad ingresada es mayor al stock del producto " + descripcion + ", se corrimienda que debe ser menor a " + stockActual + ".");
            }

        }
    }
    else {

        $.each(NotaIngresoDetalle, function (index, value) {
            if (value.prdc_icod_producto == rowNotaIngresoDetalle.prdc_icod_producto && value.status != eliminar) {
                value.nsald_cantidad = cantidad;
                value.status = editar;
                editarStatus = true;
            }

        });
        if (editarStatus) {
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
    $("#NotaIngresoDetalleId").val(rowNotaIngresoDetalle.prdc_icod_producto);
    //$("#codigoP").val(rowNotaIngresoDetalle.prdc_icod_producto);
    $("#unidad").val(rowNotaIngresoDetalle.dninc_v_unidad);
    $("#descripcionP").val(rowNotaIngresoDetalle.prdc_vdescripcion);
    $("#cantidad").val(rowNotaIngresoDetalle.nsald_cantidad);
    $("#NuevoDetalleProducto").modal("show");
}

function DeleteProductoDraw() {
    var statusEliminacion = false;

    $.each(NotaIngresoDetalle, function (index, value) {
        if (value.Id == rowNotaIngresoDetalle.Id && value.status != eliminar) {
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


function GetCorrelativoCab(id) {
    var modelview = {
        almac_icod_almacen: id
    }
    webApp.Ajax({
        url: urlMantenimiento + 'GetCorrelativo',
        parametros: modelview,
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
                    $("#nroNI").val('00000' + notaingreso);
                }
                else if (notaingreso >= 10) {
                    $("#nroNI").val('0000' + notaingreso);
                }
                else if (notaingreso >= 100) {
                    $("#nroNI").val('000' + notaingreso);
                }
                else if (notaingreso >= 1000) {
                    $("#nroNI").val('00' + notaingreso);
                }
                else if (notaingreso >= 10000) {
                    $("#nroNI").val('0' + notaingreso);
                }
                else if (notaingreso >= 100000) {
                    $("#nroNI").val(notaingreso);
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

function CorrelativoProducto() {
    var correlativo = "";
    if (NotaIngresoDetalle.length > 0) {
        var jquery = JSLINQ(NotaIngresoDetalle)
                      .Where(function (v) { return v.status != eliminar })
                      .OrderByDescending(function (o) { return o.nsald_nro_item })
                      .Select(function (item) { return parseInt(item.nsald_nro_item) });
        var correlativoValue = parseInt(jquery.items[0] + 1);
        if (correlativoValue < 10) {
            correlativo = '00' + correlativoValue;
        }
        if (correlativoValue >= 10) {
            correlativo = '0' + correlativoValue;
        }
        if (correlativoValue >= 100) {
            correlativo = correlativoValue;
        }
    }
    else {
        correlativo = "001";
    }
    return correlativo;
}


function GuardarNotaIngreso() {
    var stockActual = 0;
    var statusStock = false;
    var modelView = {
        Id: $("#NotaIngresoId").val(),
        nsalc_numero_nota_salida: $("#nroNI").val(),
        tdocc_icod_tipo_doc: tipoDocumento,
        nsalc_fecha_nota_salida_: $("#fecha").val(),
        nsalc_iid_motivo: $("#motivo").val(),
        almac_icod_almacen: $("#almacen").val(),
        Estado: agregar,
        listaDetalleNS: NotaIngresoDetalle,
        nsalc_observaciones: $("#observacion").val(),
        nsalc_referencia: "",
        UsuarioRegistro: $("#usernameLogOn strong").text()
    };
    //Evaluacion de stock de producto por #sinllorar
    $.each(modelView.listaDetalleNS, function (index, value) {
        var stockFunction = StockProducto(value.prdc_icod_producto, modelView.almac_icod_almacen);
        stockFunction.success(function (data) {
            stockActual = data.Data;
        });
        //evalua stock del producto
   
        if ( parseInt(value.dninc_cantidad)> parseInt(stockActual)) {
            statusStock=true;
        }
    });
    if (!statusStock) {
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
                    dataTableNotaIngreso.ajax.reload();
                    $("#NuevaNotaIngreso").modal("hide");
                    LimpiarFormulario();
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
                    class_name: 'gritter-warning gritter'
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

    } else {
        webApp.showMessageDialog("La cantidad ingresada es mayor al stock del producto " + descripcion + ",se corrimienda que debe ser menor a " + stockActual + ".");

    }


}

function GetNotaIngresoById(id) {
    var modelView = {
        Id: id
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
                //LimpiarFormulario();
                var notaingreso = response.Data;
                $("#NotaIngresoId").val(notaingreso.Id);
                $("#almacen").val(notaingreso.almac_icod_almacen);
                $("#nroNI").val(notaingreso.nsalc_numero_nota_salida);
                $("#fecha").val(notaingreso.fecha);
                $("#motivo").val(notaingreso.nsalc_iid_motivo);
                $("#observacion").val(notaingreso.nsalc_observaciones);
                $("#NuevaNotaIngreso").modal("show");
                $("#accionTitle").text('Editar');
                $('#fecha').datepicker({
                    autoclose: true,
                    language: 'es',
                    format: 'dd/mm/yyyy'
                }).datepicker('setDate', new Date());


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

function EliminarNataIngreso(id) {
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

                dataTableNotaIngreso.ajax.reload();
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

function GetNotaIngresoDetalleAll(id) {
    var modelView = {
        Id: id
    };
    webApp.Ajax({
        url: urlMantenimiento + 'ListarNotaSalidaDetalle',
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
                dataTableNotaIngresoDetalle.clear().draw();
                NotaIngresoDetalle = response.Data;


                dataTableNotaIngresoDetalle.rows.add(NotaIngresoDetalle).draw();

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

function CargarProducto(id) {
    var modelView = {
        idAlamcen:id
    }
    webApp.Ajax({
        url: urlListarProductoStock,
        parametros:modelView
    }, function (response) {
        if (response.Success) {

            if (response.Warning) {
                $.gritter.add({
                    title: 'Alerta',
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });
            } else {
                dataTableProducto.clear();
                ProductoArray = response.Data;
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

function Imprimir(id) {
    webApp.Ajax({
        url: urlMantenimientoReport + 'StimulsoftControl',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        datatype: 'html',
        parametros: {
            parametros: [
                          { key: 'NotaIngresoId', value: id }
            ],
            controllerGetSnapshot: 'NotaIngreso',
        },


    }, function (result) {
        $('#divReport').html(result);
        $("#modal-print-tabla").modal("show");
    });
}

function AddSearchFilter() {
    $("#UsuarioDataTable_wrapper").prepend($("#searchFilterDiv").html());
}

function buscar(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    if (tecla == 13) {
        if ($('#NotaIngresoSearchForm').valid()) {
            checkSession(function () {
                dataTableNotaIngreso.ajax.reload();
            });
        }
    }
}

function buscarProducto(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    if (tecla == 13) {
        if ($('#ProductoSearchForm').valid()) {
            checkSession(function () {
                dataTableProducto.ajax.reload();
            });
        }
    }
}

function LimpiarFormulario() {
    webApp.clearForm(formularioMantenimiento);
    NotaIngresoDetalle.length = 0;
    dataTableNotaIngresoDetalle.clear().draw();
    $("#almacen").val(1);
    $("#motivo").val(10);
    $('#fecha').datepicker({
        autoclose: true,
        language: 'es',
        format: 'dd/mm/yyyy'
    }).datepicker('setDate', new Date());


}

function LimpiarFormularioDetalle() {
    webApp.clearForm(formularioMantenimientoDetalle);
}

function CargarAlmacen() {

    webApp.Ajax({
        url: urlMantenimientoAlmacen + 'GetAllAlmacen',
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
                    $("#almacen").append('<option value="' + item.Id + '">' + item.almac_vdescripcion + '</option>');
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

function CargarMotivo() {
    var modelView = {
        idtabla: 5
    };
    webApp.Ajax({
        url: urlMantenimiento + 'GetAll',
        parametros: modelView,
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
                    $("#motivo").append('<option value="' + item.Id + '">' + item.tbpd_vdescripcion_detalle + '</option>');
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

function SelectProductoStock() {
    if (ProductoArray!=null) {
        dataTableProducto.clear().draw();
        dataTableProducto.rows.add(ProductoArray.filter(function (obj) {
            if (parseInt(obj.prdc_dstock_minimo) > 0)
                return true;
            else
                return false;
        })).draw();
    }

}

var StockProducto = function (id,idalamcen) {
    var stock = 0;
    var modelView = {
        idproducto: id,
        idalmacen:idalamcen
    };
    return $.ajax({
        type: "POST",
        dataType: 'json',
        url: urlMantenimiento + 'stockProducto',
        data:modelView,
        success: function (data) {
            return data.Data;
        }
    });  
}

function GetFechaSubString(fecha) {
    var respuesta = "";
    if (fecha != null && fecha.trim() != "")

        var sTmp = String(fecha);
    arrfch = sTmp.split('/');
    respuesta = arrfch[1] + '/' + arrfch[0] + '/' + arrfch[2];
    return respuesta;
}