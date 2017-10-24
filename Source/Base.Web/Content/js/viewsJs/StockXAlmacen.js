var dataTableStockXAlmacen = null;
var dataTableNotaIngresoDetalle = null;
var dataTableProducto = null;
var formularioMantenimiento = "NotaIngresoForm";
var formularioMantenimientoDetalle = "ProductoDetalleForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'NotaIngreso/Listar';
var urlMantenimiento = baseUrl + 'StockXAlmacen1/';
var urlMantenimientoAlmacen = baseUrl + 'Almacen/';
var urlListaCargo = baseUrl + 'NotaIngreso/';
var urlMantenimientoReport = baseUrl + 'Reporte/';

var stockxalmacen = new Array();
$(document).ready(function () {
    var stockXAlmacenHub = $.connection.stockXAlmacenHub;

    stockXAlmacenHub.client.listar = function (response) {
        if (response.Success) {

            if (response.Warning) {
                $.gritter.add({
                    title: 'Alerta',
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });
            } else {
                stockxalmacen.length = 0;
                dataTableStockXAlmacen.clear().draw();
                stockxalmacen = response.Data;
                dataTableStockXAlmacen.rows.add(stockxalmacen).draw();
            }
        } else {
            $.gritter.add({
                title: 'Error',
                text: response.Message,
                class_name: 'gritter-error gritter'
            });
        }

    };

    stockXAlmacenHub.client.listarStockXAlmacenData = function () {
        //var modelview = {
        //    dataTableModel: {
        //        filter: {
        //            almacenSearch:,
        //            descripcionSearch:,
        //            FechaInicialSearch:,
        //            FechaFinalSearch:,
        //        }
        //    }
        //};
        stockXAlmacenHub.server.listar();
    };

    $.connection.hub.start().done(function () {
        stockXAlmacenHub.server.listar();
    });



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
        ListarStockXAlmacen();
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
                dataTableProducto.ajax.reload();
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


    $('[data-toggle="tooltip"]').tooltip();

  
    $('#fechaFinalsearch,#fechaIniciosearch').datepicker({
        autoclose: true,
        language: 'es',
        format: 'dd/mm/yyyy'
    });

    $('input[name=date-range-picker]').daterangepicker({
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

});
function VisualizarDataTableStockXAlmacen() {
    dataTableStockXAlmacen = $('#StockXAlmacenDataTable').DataTable({
        "bFilter": false,
        "bProcessing": true,
        "serverSide": false,
        //"scrollY": "350px",              
        "data":stockxalmacen,
        "dom": 'fltip',
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
            { "data": "prdc_iid_producto" },
            { "data": "almac_vdescripcion" },
            { "data": "prdc_vdescripcion" },
            { "data": "umec_vdescripcion" },
            { "data": "stockactual" },
        ],
        "aoColumnDefs": [

            { "bVisible": false, "aTargets": [0] },
            { "className": "center hidden-120", "aTargets": [1], "width": "10%" },
            { "className": "hidden-120", "aTargets": [2], "width": "30%" },
            { "className": "center hidden-992", "aTargets": [3], "width": "10%" },
            { "className": "center hidden-120", "aTargets": [4], "width": "10%" },
          

        ],
        "order": [[1, "desc"]],
        "initComplete": function (settings, json) {
            // AddSearchFilter();
        },
        "fnDrawCallback": function (oSettings) {

        }
    });
}

