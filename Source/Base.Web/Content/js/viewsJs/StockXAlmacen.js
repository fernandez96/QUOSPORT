var dataTableNotaIngreso = null;
var dataTableNotaIngresoDetalle = null;
var dataTableProducto = null;
var formularioMantenimiento = "NotaIngresoForm";
var formularioMantenimientoDetalle = "ProductoDetalleForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'NotaIngreso/Listar';
var urlMantenimiento = baseUrl + 'NotaIngreso/';
var urlMantenimientoAlmacen = baseUrl + 'Almacen/';
var urlListaCargo = baseUrl + 'NotaIngreso/';
var urlMantenimientoReport = baseUrl + 'Reporte/';
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

  
    $('#fechasearch').datepicker({
        autoclose: true,
        language: 'es',
        format: 'dd/mm/yyyy'
    });

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
                    //DescripcionSearch: $("#Descripcionsearch").val()
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
            { "data": "almac_vdescripcion" },
            { "data": "fecha" },
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
            { "className": "center hidden-120", "aTargets": [3], "width": "14%" },
            { "className": "center hidden-120", "aTargets": [4], "width": "10%" },
             { "className": "hidden-992", "aTargets": [5], "width": "30%" },
            { "bSortable": false, "className": "hidden-992", "aTargets": [6], "width": "7%" }

        ],
        "order": [[1, "desc"]],
        "initComplete": function (settings, json) {
            // AddSearchFilter();
        },
        "fnDrawCallback": function (oSettings) {

        }
    });
}