var dataTableVale = null;
var formularioMantenimiento = "ValeForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'Vale/Lista';
var urlMantenimiento = baseUrlApiService + 'Vale/'
var urlGetAllListProducto = baseUrlApiService + 'TipoProducto/'
var rowVale = null;

$(document).ready(function () { 
    $.extend($.fn.dataTable.defaults, {
        language: { url: baseUrl + 'Content/js/dataTables/Internationalisation/es.txt' },
        responsive: true,
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "bProcessing": true,
        "dom": 'fltip'
    });    


    checkSession(function () {
        visualizardataTableVale();
    });


    $('body').on('click', 'button.btnAgregarVale', function() {
        LimpiarFormulario();

        $("#accionTitle").text('Nuevo');
        $("#NuevoVale").modal("show");
    });

    $('body').on('click', 'a.editarVale', function() {        
        rowVale = this;
        checkSession(function () {
            GetValeById();
        });
    });      

    $('body').on('click', 'a.eliminarVale', function() {        
        var aPos = dataTableVale.fnGetPosition(this.parentNode.parentNode);
        var aData = dataTableVale.fnGetData(aPos[0]);
        var rowID = aData.Id;

        delRowPos = aPos[0];
        delRowID = rowID;

        webApp.showDeleteConfirmDialog(function () {
            
            checkSession(function () {
                Eliminar();
            });
            },'Se eliminará el registro.  ¿Está seguro que desea continuar?');
    });

    $("#btnGuardarVale").on("click", function (e) { 

        if($('#'+formularioMantenimiento).valid()){

            webApp.showReConfirmDialog(function () {
                checkSession(function () {
                    GuardarVale();
                });
            });
        }

        e.preventDefault();
    });

  
    webApp.InicializarValidacion(formularioMantenimiento, 
        {
            Monto: {
                required: true,
                noPasteAllowNumber: true
            },
            Puntos: {
                required: true,
                noPasteAllowNumber: true
      
            },
            TipoProductoId: {
                required: true
            } 
        },
        {
            Monto: {
                required: "Por favor ingrese Monto",
            },
            Puntos: {
                required: "Por favor ingrese Puntos",
            },
            TipoProductoId: {
                required: "Por favor seleccione Tipo Producto"
            }
        });
   CargarTipoProducto();
});  

function LimpiarFormulario(){
    webApp.clearForm(formularioMantenimiento);
    $("#Estado").val(1);
    $("#TipoProductoId").focus();
}

function CargarTipoProducto(){
    var WhereFilters = {
        whereFilter: 'WHERE Estado IN (1,2)'
    };
    webApp.Ajax({
        url: urlGetAllListProducto + 'GetAll',
        parametros: WhereFilters,
        async: false,
    }, function(response){
        if(response.Success){
            if(response.warning){                           
                $.gritter.add({
                    title: 'Alerta',
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });                         
            }else{
                $.each(response.Data, function(index, item){
                    $("#TipoProductoId").append('<option value="'+ item.Id +'">' + item.Nombre + '</option>');
                });
            }
        }else{
            $.gritter.add({
                title: 'Error',
                text: response.message,
                class_name: 'gritter-error gritter'
            });                     
        }
    }, function(response){
        $.gritter.add({
            title: 'Error',
            text: response,
            class_name: 'gritter-error gritter'
        });
    }, function(XMLHttpRequest, textStatus, errorThrown){
        $.gritter.add({
            title: 'Error',
            text: "Status: " + textStatus + "<br/>Error: " + errorThrown,
            class_name: 'gritter-error gritter'
        });
    });
}

function Eliminar(){
    var modelView = {
        Id: delRowID,
        UsuarioRegistro: $("#usernameLogOn strong").text(),
    };

    webApp.Ajax({
        url: urlMantenimiento + 'Delete',
        async: false,
        parametros: modelView
        
    }, function(response){
        $("#NuevoVale").modal("hide");
        if(response.Success){
            dataTableVale.fnDeleteRow(delRowPos);
            if(response.Warning){                           
                $.gritter.add({
                    title: response.Title,
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });                         
            }else{
                $.gritter.add({
                    title: response.Title,
                    text: response.Message,
                    class_name: 'gritter-success gritter'
                });
            }           

        }else{
            $.gritter.add({
                title: 'Error',
                text: response.Message,
                class_name: 'gritter-error gritter'
            });                     
        }
    }, function(response){
        $.gritter.add({
            title: 'Error',
            text: response,
            class_name: 'gritter-error gritter'
        });
    }, function(XMLHttpRequest, textStatus, errorThrown){
        $.gritter.add({
            title: 'Error',
            text: "Status: " + textStatus + "<br/>Error: " + errorThrown,
            class_name: 'gritter-error gritter'
        });
    });
    delRowPos = null;
    delRowID = 0;
} 

function GuardarVale() { 
    
    var modelView = {
        Id : $("#ValeId").val(),  
        TipoProductoId: $("#TipoProductoId").val(),
        Monto : $("#Monto").val(),
        Puntos : $("#Puntos").val(),
        Estado: $("#Estado").val(),
        UsuarioRegistro: $("#usernameLogOn strong").text(),
    };

    if(modelView.Id == 0)
        action = 'Add';
    else
        action = 'Update';

    webApp.Ajax({
        url: urlMantenimiento + action,
        parametros: modelView
        
    }, function(response){
        if(response.Success){
            dataTableVale.fnReloadAjax();
            if(response.Warning){                           
                $.gritter.add({
                    title: response.Title,
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });                         
            } else {
                $("#NuevoVale").modal("hide");
                $.gritter.add({
                    title: response.Title,
                    text: response.Message,
                    class_name: 'gritter-success gritter'
                });
            }           
        }else{
            $.gritter.add({
                title: 'Error',
                text: response.Message,
                class_name: 'gritter-error gritter'
            });                     
        }
    }, function(response){
        $.gritter.add({
            title: 'Error',
            text: response,
            class_name: 'gritter-error gritter'
        });
    }, function(XMLHttpRequest, textStatus, errorThrown){
        $.gritter.add({
            title: 'Error',
            text: "Status: " + textStatus + "<br/>Error: " + errorThrown,
            class_name: 'gritter-error gritter'
        });
    });
}

function visualizardataTableVale() {
    dataTableVale = $('#ValeDataTable').dataTable({
        "bFilter": false,
        "bProcessing": true,
        "serverSide": true,
        //"scrollY": "350px",              
        "ajax": {
            "url": urlListar,
            "type": "POST",
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
            {
                "data": function (obj) {

                    return '<div class="action-buttons">\
                    <a class="green editarVale" href="javascript:void(0)"><i class="ace-icon fa fa-pencil bigger-130"></i></a>\
                    <a class="red eliminarVale" href="javascript:void(0)"><i class="ace-icon fa fa-trash-o bigger-130"></i></a>\
                    </div>';
                }
            },
            { "data": "Id" },
            { "data": "TipoProductoNombre" },
            { "data": "Monto" },
            { "data": "Puntos" },
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
            { "bSortable": false, "sClass": "center", "aTargets": [0], "width": "10%" },
            { "bVisible": false, "aTargets": [1] },
            { "aTargets": [2], "width": "20%" },
            { "aTargets": [3], "width": "20%" },
            { "className": "hidden-480", "aTargets": [4], "width": "30%" },
            { "bSortable": false, "aTargets": [5], "width": "20%" }
        ],
        "order": [[1, "desc"]]
    });

}

function GetValeById() {
    var aPos = dataTableVale.fnGetPosition(rowVale.parentNode.parentNode);
    var aData = dataTableVale.fnGetData(aPos[0]);
    var rowID = aData.Id;

    var modelView = {
        Id: aData.Id
    };

    webApp.Ajax({
        url: urlMantenimiento + 'GetById',
        parametros: modelView

    }, function (response) {
        if (response.Success) {
            if (response.warning) {
                $.gritter.add({
                    title: 'Alerta',
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });
            } else {
                LimpiarFormulario();
                var vale = response.Data;
                $("#Monto").val(parseInt(vale.Monto));
                $("#Puntos").val(vale.Puntos);
                $("#Estado").val(vale.Estado);
                $("#ValeId").val(vale.Id);
                $("#TipoProductoId").val(vale.TipoProductoId);

                $("#accionTitle").text('Editar');
                $("#NuevoVale").modal("show");
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