var dataTableEnvio = null;
var formularioMantenimiento = "EnvioForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'Reporte/Listar';
var urlMantenimiento = baseUrlApiService + 'Envio/';
var rowReporte = null;

$(document).ready(function () {
    $.extend($.fn.dataTable.defaults, {
        language: { url: baseUrl + 'Content/js/dataTables/Internationalisation/es.txt' },
        responsive: true,
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "bProcessing": true,
        "dom": 'fltip'
    });
    checkSession(function () {
        VisualizardataTableEnvio();
    });


    $('body').on('click', 'a.searchEnvio', function () {
        $("#BuscarReporte").modal("show");
    });

    $('body').on('click', 'a.downloadEnvio', function () {
        checkSession(function () {
            DescargarEnvio();
        });     
    });

    //or change it into a date range picker
    $('.input-daterange').datepicker({
        autoclose: true,
        language: 'es',
        format: 'yyyy-mm-dd'
    });

    $('body').on('click', 'a.verEnvio', function () {
        
        rowReporte = this;
        $("#DetalleEnvio").modal("show");
        //checkSession(function () {
        //    visualizardataTableVerEnvio();
        //})
    });

    webApp.validarLetrasEspacio(['ClienteSearch']);
    webApp.InicializarValidacion('EnvioSearchForm',
        {
            
         
        },
        {
           
        });

    $("#btnSearchEnvio").on("click", function (e) {

        if ($('#EnvioSearchForm').valid()) {
            checkSession(function () {
                dataTableEnvio.fnReloadAjax();
                $("#BuscarReporte").modal("hide");
            })
            
            
        }

        e.preventDefault();
    });

});

function visualizardataTableVerEnvio() {

    var aPos = dataTableEnvio.fnGetPosition(rowReporte.parentNode.parentNode);
    var aData = dataTableEnvio.fnGetData(aPos[0]);
    var rowID = aData.Id;

    var modelView = {
        Id: aData.Id
    };

    webApp.Ajax({
        url: urlMantenimiento + 'GetByIdLog',
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
                LimpiarFormularioDetalle();
                var envio = response.Data;
                $("#DetalleEnvio").modal("show");
                $("#EnvioId").val(envio.Id);
                $("#TipoPersonaDetalle").val(GetTipoPersona(envio.TipoPersona));
                $("#NombreCompletoDetalle").val(envio.NombreCompleto);
                $("#NumeroDocumentoDetalle").val(envio.NumeroDocumento);
                $("#CorreoElectronicoDetalle").val(envio.CorreoElectronico);

                $("#EstadoEnvioDetalle").val(GetEstadoEnvio(envio.EstadoEnvio));
                $("#FechaCaducidadDetalle").val(GetFechaSubString(envio.FechaCaducidad));

                $("#TipoProductoDetalle").val(envio.TipoProducto);
                $("#MontoDetalle").val(envio.Monto);

                $("#NombreCompletoTitularDetalle").val(envio.NombreCompletoTitular);
                $("#NumeroDocumentoTitularDetalle").val(envio.NumeroDocumentoTitular);
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

function VisualizardataTableEnvio() {
    dataTableEnvio = $('#EnvioDataTable').dataTable({
        "bFilter": false,
        "bProcessing": true,
        "serverSide": true,
        //"scrollY": "350px",              
        "ajax": {
            "url": urlListar,
            "type": "POST",
            "data": function (request) {
                request.filter = new Object();
                request.filter.ClienteSearch = $("#ClienteSearch").val();
                request.filter.AnioSearch = $("#AnioSearch").val();
               
            },
            dataFilter: function (data) {
                if (data.substring(0, 9) == "<!DOCTYPE") {
                    redireccionarLogin("Sesión Terminada", "Se terminó la sesión");
                } else {
                    return data;

                }
            }
        },
        "createdRow": function (row, data, index) {
            $('td', row).eq(4).html('<b>S/  ' + data.Monto + '</b>');
            if (data.Monto * 1 > 100) {
                $('td', row).eq(4).addClass('blue');
            }
        },
        "bAutoWidth": false,
        "columns": [
            {
                "data": function (obj) {

                    return null
                }
            },
         
            { "data": "Id" },
            { "data": "Anio" },
            { "data": "Mes" },
            { "data": "Cliente" },
            { "data": "Monto" },
            { "data": "Cantidad" },
        ],
        "aoColumnDefs": [
            { "bSortable": false, "sClass": "center", "aTargets": [0], "width": "2%" },
            { "bVisible": false, "aTargets": [1] },
            { "bSortable": false, "aTargets": [2], "width": "4%" },
            { "bSortable": false, "className": "hidden-480", "aTargets": [3], "width": "2%" },
            { "className": "hidden-480", "aTargets": [4], "width": "10%" },
            { "className": "hidden-480", "aTargets": [5], "width": "2%" },
            { "bSortable": false, "className": "hidden-480", "aTargets": [6], "width": "2%" },
            //{ "bSortable": false, "aTargets": [7], "width": "3%" },
            //{ "bSortable": false, "aTargets": [8], "width": "8%" },
            //{ "bSortable": false, "aTargets": [9], "width": "8%" },
            //{ "bSortable": false, "className": "hidden-480", "aTargets": [10], "width": "12%" },
            //{ "className": "hidden-480", "aTargets": [11], "width": "9%" },
        ],
        "order": [[1, "desc"]]
    });

}

function LimpiarFormularioDetalle() {
    webApp.clearForm('DetalleEnvioForm');
}







function DescargarEnvio() {

    var reportFilter = {
        NumeroDocumentoSearch: $("#NumeroDocumentoSearch").val(),
        CorreoElectronicoSearch: $("#CorreoElectronicoSearch").val(),
        FechaEnvioInicialSearch: $("#FechaEnvioInicialSearch").val(),
        FechaEnvioFinalSearch: $("#FechaEnvioFinalSearch").val(),
    };


    var url = (baseUrl + 'Reporte/Exportar' + "?modelStr=" + JSON.stringify(reportFilter));

    webApp.ExportarExcel(url);
}

Highcharts.chart('container', {
    chart: {
        plotBackgroundColor: null,
        plotBorderWidth: null,
        plotShadow: false,
        type: 'pie'
    },
    title: {
        text: 'Browser market shares January, 2015 to May, 2015'
    },
    tooltip: {
        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
    },
    plotOptions: {
        pie: {
            allowPointSelect: true,
            cursor: 'pointer',
            dataLabels: {
                enabled: true,
                format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                style: {
                    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                }
            }
        }
    },
    series: [{
        name: 'Brands',
        colorByPoint: true,
        data: [{
            name: 'Microsoft Internet Explorer',
            y: 56.33
        }, {
            name: 'Chrome',
            y: 24.03,
            sliced: true,
            selected: true
        }, {
            name: 'Firefox',
            y: 10.38
        }, {
            name: 'Safari',
            y: 4.77
        }, {
            name: 'Opera',
            y: 0.91
        }, {
            name: 'Proprietary or Undetectable',
            y: 0.2
        }]
    }]
});