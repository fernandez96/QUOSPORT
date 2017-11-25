var dataTableStockXAlmacen = null;
var dataTableKardex = null;
var dataTableProducto = null;
var formularioMantenimiento = "NotaIngresoForm";
var formularioMantenimientoDetalle = "ProductoDetalleForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'StockXAlmacen/Listar';
var urlMantenimiento = baseUrl + 'StockXAlmacen/';
var urlMantenimientoAlmacen = baseUrl + 'Almacen/';
var urlListaCargo = baseUrl + 'NotaIngreso/';
var urlMantenimientoReport = baseUrl + 'Reporte/';
var urlMantenimientoAlmacen = baseUrl + 'Almacen/';
var idProducto = 0;
var idAlmacen = 0;
var stockxalmacen = new Array();
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
        VisualizarDataTableStockXAlmacen();
        VisualizarDataTableKardex();

    });

 
    $('#StockXAlmacenDataTable  tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            dataTableStockXAlmacen.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
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


    $("#productoBuscar").on("click", function () {
        dataTableProducto.ajax.reload();
        $("#ProductoModal").modal("show");
    });

    $('#btnImprimir').on('click', function () {
        rowNotaIngreso = dataTableStockXAlmacen.row('.selected').data();
        if (typeof rowNotaIngreso === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            checkSession(function () {
                //Imprimir(rowNotaIngreso.Id);
            });
        }
    });

    $('body').on('change', '#almacen', function () {
        GetCorrelativoCab(this.value);
    });

    $("#btnSearchStockXAlmacen").on("click", function (e) {
        if ($('#stockXAlmacenSearchForm').valid()) {
            checkSession(function () {
                dataTableStockXAlmacen.ajax.reload();
            });
        }
        e.preventDefault();
    });

    $("#btnSearchTablaproducto").on("click", function (e) {
        if ($('#ProductoSearchForm').valid()) {
            checkSession(function () {
                dataTableProducto.ajax.reload();
            });
        }
        e.preventDefault();
    });

    $("#btnVerkardex").on("click", function (e) {
        rowNotaIngreso = dataTableStockXAlmacen.row('.selected').data();
        if (typeof rowNotaIngreso === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            checkSession(function () {
                idAlmacen = rowNotaIngreso.almac_iid_almacen;
                idProducto = rowNotaIngreso.prdc_iid_producto;
                $("#strproducto").text(' - '+rowNotaIngreso.almac_vdescripcion + ' - ' + rowNotaIngreso.prdc_vdescripcion)
                dataTableKardex.ajax.reload();
                $("#Kardex").modal("show");
            });
        }
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


    $('[data-toggle="tooltip"]').tooltip();

  
    $('#fechasearch').datepicker({
        autoclose: true,
        language: 'es',
        format: 'dd/mm/yyyy'
    }).datepicker('setDate', new Date());;

    //$('#fechasearch').daterangepicker({
    //    'applyClass': 'btn-sm btn-success',
    //    'cancelClass': 'btn-sm btn-default',
    //    language: 'es',
    //    locale: {
    //        applyLabel: 'Aplicar',
    //        cancelLabel: 'Cancelar',
    //    }
    //})
    //.prev().on(ace.click_event, function () {
    //    $(this).next().focus();
    //});
    CargarAlmacen();
});
function VisualizarDataTableStockXAlmacen() {
    dataTableStockXAlmacen = $('#StockXAlmacenDataTable').DataTable({
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
                    almacenSearch: $("#almacensearch").val(),
                    descripcionSearch: $("#descripcionProducto").val(),
                    FechaInicialSearch: '01/11/2017',
                    FechaFinalSearch: $("#fechasearch").val()
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
            { "data": "prdc_iid_producto" },
           { "data": "almac_iid_almacen" },
            { "data": "almac_vdescripcion" },
            { "data": "prdc_vdescripcion" },
            { "data": "umec_vdescripcion" },
            { "data": "stockactual", render: $.fn.dataTable.render.number(',', '.', 2) },

        ],
      

        "aoColumnDefs": [

            { "bVisible": false, "aTargets": [0] },
            { "bVisible": false, "aTargets": [1] },
            { "className": "center hidden-120", "aTargets": [2], "width": "10%" },
            { "className": "hidden-120", "aTargets": [3], "width": "30%" },
            { "className": "center hidden-992", "aTargets": [4], "width": "10%" },
            { "className": "center hidden-120", "aTargets": [5], "width": "10%" },
          

        ],
        "order": [[1, "desc"]],
        "initComplete": function (settings, json) {
            // AddSearchFilter();
        },
        "fnDrawCallback": function (oSettings) {

        }
    });
}

function VisualizarDataTableKardex() {
    dataTableKardex = $('#KardexDataTable').DataTable({
        "bFilter": false,
        "bProcessing": true,
        "serverSide": true,
        //"scrollY": "350px",              
        "ajax": {
            "url": urlMantenimiento + 'ListarKardex',
            "type": "POST",
            "data": function (request) {
                request.KardexDTO = new Object();
                request.KardexDTO={
                    prdc_icod_producto: idProducto,
                    almac_icod_almacen:idAlmacen
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
            { "data": "prdc_icod_producto" },
            { "data": "strDocumento" },
            { "data": "kardc_fecha_movimiento" },
            { "data": "strMotivo" },
            { "data": "dblIngreso", render: $.fn.dataTable.render.number(',', '.', 2) },
            { "data": "dblSalida", render: $.fn.dataTable.render.number(',', '.', 2) },
            { "data": "dblSaldo", render: $.fn.dataTable.render.number(',', '.', 2) },
            { "data": "kardc_observaciones"},

        ],


        "aoColumnDefs": [

            { "bVisible": false, "aTargets": [0] },
            { "className": "center hidden-120", "aTargets": [1], "width": "10%" },
            { "className": "center hidden-992", "aTargets": [2], "width": "10%" },
            { "className": "center hidden-992", "aTargets": [3], "width": "10%" },
            { "className": "center hidden-120", "aTargets": [4], "width": "10%" },
            { "className": "center hidden-120", "aTargets": [5], "width": "10%" },
            { "className": "center hidden-120", "aTargets": [6], "width": "10%" },
            { "className": "center hidden-992", "aTargets": [7], "width": "10%" },

        ],
        "order": [[1, "desc"]],
        "initComplete": function (settings, json) {
            // AddSearchFilter();
        },
        "fnDrawCallback": function (oSettings) {

        }
    });
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
                    $("#almacensearch").append('<option value="' + item.Id + '">' + item.almac_vdescripcion + '</option>');
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

function buscar(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    if (tecla == 13) {
        if ($('#stockXAlmacenSearchForm').valid()) {
            checkSession(function () {
                dataTableStockXAlmacen.ajax.reload();
            });
        }
    }
}
