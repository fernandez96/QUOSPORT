var dataTableNotaIngreso = null;
var dataTableNotaIngresoDetalle = null;
var dataTableProducto = null;
var formularioMantenimiento = "NotaIngresoForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'NotaIngreso/Listar';
var urlMantenimiento = baseUrl + 'NotaIngreso/';
var urlListaCargo = baseUrl + 'NotaIngreso/';
var rowNotaIngreso = null;
var urlListarProductos = baseUrl + 'Producto/Listar';
var NotaIngreso = new Array();
var editor;
var rowProducto = null;
$(document).ready(function () {
    $.extend($.fn.dataTable.defaults, {
        language: { url: baseUrl + 'Content/js/dataTables/Internationalisation/es.txt' },
        responsive: true,
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "bProcessing": true,
        "dom": 'fltip'
    });
    checkSession(function () {
        VisualizarDataTableNotaIngresoDetalle();
        VisualizarDataTableProducto();
    });

    editor = new $.fn.dataTable.Editor({
        table: "#NotaIngresoDetalleDataTable",
  
    });
    $('#NotaIngresoDetalleDataTable  tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            dataTableProducto.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    $('#ProductoDataTable  tbody').on('dblclick', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            dataTableProducto.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
            rowProducto = dataTableProducto.row('.selected').data();
            $("#codigoP").val('');
            $("#unidad").val(rowProducto.umec_v_unidad_medida);
            $("#descripcionP").val(rowProducto.prdc_vdescripcion);
            $("#cantidad").focus();
            $("#ProductoModal").modal("hide");

        }
    });

  

    $('#btnAgregarNotaIngreso').on('click', function () {
        LimpiarFormulario();

        $("#NotaIngresoId").val(0);
        $("#accionTitle").text('Nuevo');
        $("#NuevaNotaIngreso").modal("show");
        $("#Username").prop("disabled", false);
    });
    //agregar detalle
												
    $("#btnAgregarNotaIngresoDetalle").on("click", function () {
        $("#NuevoDetalleProducto").modal("show");
      
    });

    $("#productoBuscar").on("click", function () {
        $("#ProductoModal").modal("show");
    });
  

    $('[data-toggle="tooltip"]').tooltip();
});

function VisualizarDataTableNotaIngresoDetalle() {
    dataTableNotaIngresoDetalle = $('#NotaIngresoDetalleDataTable').DataTable({
        "bFilter": false,
        "searching": false,
        "bProcessing": true,
        "serverSide": false,
        responsive: true,
        "bProcessing": true,
        "dom": 'fltip',
        //"scrollY": "350px",              
        "data": NotaIngreso,
        dataFilter: function (data) {
            if (data.substring(0, 9) == "<!DOCTYPE") {
                redireccionarLogin("Sesión Terminada", "Se terminó la sesión");
            } else {
                return data;
                //var json = jQuery.parseJSON(data);
                //return JSON.stringify(json); // return JSON string
            }
        },
        "footerCallback": function ( row, data, start, end, display ) {
            var api = this.api(), data;
            // Remove the formatting to get integer data for summation
            var intVal = function ( i ) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '')*1 :
                    typeof i === 'number' ?
                    i : 0;
            };
            // Total over all pages
            total = api
                .column( 5 )
                .data()
                .reduce( function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0 );
            // Update footer
            $( api.column(6).footer() ).html(
                'S/. '+total+''
            );
        },
        "bAutoWidth": false,
        "columns": [
            { "data": "id"},
            { "data": "item"},
            { "data": "Producto" ,className:'editable'},
            { "data": "Descripcion", editField: "Descripcion" },
            { "data": "UM" },
            { "data": "Cantidad"},
            {
                "data": function (obj) {
                    if (obj.Estado == 1)
                        return '<span class="label label-info label-sm arrowed-in arrowed-in-right">Activo</span>';
                    else
                        return '<span class="label label-sm arrowed-in arrowed-in-right">Inactivo</span>';
                }
            }
        ],
        "aoColumnDefs": [

            { "bVisible": false, "aTargets": [0] },
            { "className": "hidden-120", "aTargets": [1], "width": "10%" },
            { "className": "hidden-120", "aTargets": [2], "width": "18%" },
            { "className": "hidden-992", "aTargets": [3], "width": "18%" },
            { "className": "hidden-768", "aTargets": [4], "width": "20%" },
            { "className": "hidden-600", "aTargets": [5], "width": "10%" },
            { "bSortable": false, "className": "hidden-480", "aTargets": [6], "width": "10%" }

        ],
        //select: {
        //    style: 'os',
        //    selector: 'td:first-child'
        //},
        //buttons: [
        //  { extend: "create", editor: editor },
        //  { extend: "edit",   editor: editor },
        //  { extend: "remove", editor: editor }
        //],

        "order": [[1, "desc"]],
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
    $("#CargoId").val(1);
    $("#RolId").val(1);
    $("#Estado").val(1);
    $("#Username").focus();
    $("#Contrasena").prop("type", "password");
}