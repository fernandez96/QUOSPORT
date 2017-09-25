var dataTableUsuario = null;
var formularioMantenimiento = "UsuarioForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'Usuario/Listar';
var urlMantenimiento = baseUrl + 'Usuario/';
var urlListaCargo = baseUrl + 'Usuario/';
var rowUsuario = null;


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
    });

    $('#UsuarioDataTable  tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            dataTableUsuario.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $('#btnAgregarUsuario').on('click', function () {
        LimpiarFormulario();

        $("#UsuarioId").val(0);
        $("#accionTitle").text('Nuevo');
        $("#NuevoUsuario").modal("show");
        $("#Username").prop("disabled", false);
    });

    $('#editarUsuario').on('click', function () {
        rowUsuario = dataTableUsuario.row('.selected').data();
        if (typeof rowUsuario === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else
        {
            checkSession(function () {
                GetUsuarioById();
            });
        }
       
    });      

    $('#eliminarUsuario').on('click', function () {
        rowUsuario = dataTableUsuario.row('.selected').data();
        if (typeof rowUsuario === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            webApp.showDeleteConfirmDialog(function () {
                checkSession(function () {
                    EliminarUsuario();
                });
            }, 'Se eliminará el registro. ¿Está seguro que desea continuar?');
        }
    
    });

    $("#mostarPass").on('click', function () {
        var allInputs = $("#Contrasena").get(0).type;
        if (allInputs === 'text') {
            $("#Contrasena").prop("type", "password");
        }
        if (allInputs === 'password') {
            $("#Contrasena").prop("type", "text");
        }
       });

    $("#mostarPassConf").on('click', function () {
        var allInputs = $("#ContrasenaConf").get(0).type;
        if (allInputs === 'text') {
            $("#ContrasenaConf").prop("type", "password");
        }
        if (allInputs === 'password') {
            $("#ContrasenaConf").prop("type", "text");
        }
    });


    $("#btnSearchUsuario").on("click", function (e) {
        if ($('#UsuarioSearchForm').valid()) {
            checkSession(function () {
            dataTableUsuario.ajax.reload();
            });
        }
        e.preventDefault();
    });

    $("#btnGuardarUsuario").on("click", function (e) { 

        if($('#'+formularioMantenimiento).valid()){

            ////webApp.showConfirmDialog(function () {
                checkSession(function () {
                    GuardarUsuario();
                });
            //});
        }

        e.preventDefault();
    });  

    webApp.validarLetrasEspacio(['Nombre', 'Apellido']);
    $('#Correo').validCampoFranz('@abcdefghijklmnÃ±opqrstuvwxyz_1234567890.');

    webApp.InicializarValidacion(formularioMantenimiento, 
        {
            Username: {
                required: true
      
            },
            Contrasena: {
                required: true
            },
            ContrasenaConf:{
                required: true
            },
            Nombre: {
                required: true,
                noPasteAllowLetterAndSpace: true,
                firstCharacterBeLetter: true
            },
            Apellido: {
                required: true,
                noPasteAllowLetterAndSpace: true,
                firstCharacterBeLetter: true
            },
            CargoId: {
                required: true
            },
            RolId: {
                required: true
            }            
        },
        {
            Username: {
                required: "Por favor ingrese Usuario.",
             
            },
            Contrasena: {
                required: "Por favor ingrese Contraseña.",

            },
            ContrasenaConf: {
                required: "Por favor confirme Contraseña.",

            },
            Nombre: {
                required: "Por favor ingrese Nombre."
            },
            Apellido: {
                required: "Por favor ingrese Apellido."
            },
            
            CargoId: {
                required: "Por favor seleccione Cargo."
            },
            RolId: {
                required: "Por favor seleccione Rol."
            }            
        });
    CargarCargo();
    CargarRol();
    CargarEstado();
        $('[data-toggle="tooltip"]').tooltip();
});

function VisualizarDataTableUsuario() {
    dataTableUsuario = $('#UsuarioDataTable').DataTable({
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
                    UsernameSearch: $("#UsuarioSearch").val(),
                    RolIdSearch : $("#RolIdSearch").val()
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
            { "data": "Username" },
            { "data": "Nombre" },
            { "data": "Apellido" },
            { "data": "Correo" },
            { "data": "RolNombre" },
            { "data": function(obj){
                    if(obj.Estado == "1")
                        return '<span class="label label-info label-sm arrowed-in arrowed-in-right">Activo</span>';
                    else
                        return '<span class="label label-sm arrowed-in arrowed-in-right">Inactivo</span>';
                }
            }
        ],
        "aoColumnDefs": [

            { "bVisible": false,  "aTargets": [0]},
            {"className": "hidden-120",  "aTargets": [1], "width": "10%"},
            { "className": "hidden-120", "aTargets": [2], "width": "18%" },
            { "className": "hidden-992", "aTargets": [3], "width": "18%" },
            { "className": "hidden-768", "aTargets": [4], "width": "20%"},
            { "className": "hidden-600", "aTargets": [5], "width": "10%" },
            { "bSortable": false, "className": "hidden-480", "aTargets": [6], "width": "10%" }

        ],
        "order": [[1, "desc"]],
        "initComplete": function (settings, json) {
           // AddSearchFilter();
        },
        "fnDrawCallback": function (oSettings) {
            
        }
    });
}

function GetUsuarioById() {
        var modelView = {
            Id : rowUsuario.Id
        };        

        webApp.Ajax({
            url: urlMantenimiento + 'GetById',
            parametros: modelView
        }, function(response){            
            if(response.Success){                
                if(response.Warning){                           
                    $.gritter.add({
                        title: 'Alerta',
                        text: response.Message,
                        class_name: 'gritter-warning gritter'
                    });                         
                }else{
                    LimpiarFormulario();
                    var usuario = response.Data;                    
                    $("#Username").val(usuario.Username);
                    $("#Nombre").val(usuario.Nombre);
                    $("#Apellido").val(usuario.Apellido);
                    $("#Correo").val(usuario.Correo);
                    $("#CargoId").val(usuario.CargoId);
                    $("#RolId").val(usuario.RolId);
                    $("#Estado").val(usuario.Estado);
                    $("#UsuarioId").val(usuario.Id);
                    $("#Contrasena").val(usuario.Password);
                    $("#accionTitle").text('Editar');
                    $("#NuevoUsuario").modal("show");
                    $("#ContrasenaConf").val(usuario.ConfirmarPassword);
                    $("#Username").prop("disabled", true);
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

function EliminarUsuario(){
    var modelView = {
        Id: rowUsuario.Id,
        UsuarioRegistro: $("#usernameLogOn strong").text()
    };

    webApp.Ajax({
        url: urlMantenimiento + 'Delete',
        async: false,
        parametros: modelView
    }, function(response){        
        if (response.Success) {            
            if(response.Warning){                           
                $.gritter.add({
                    title: 'Alerta',
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });                         
            } else {
                $("#NuevoUsuario").modal("hide");
                dataTableUsuario.row('.selected').remove().draw();
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

function GuardarUsuario() { 

    var modelView = {
        Id : $("#UsuarioId").val(),    
        Username: $("#Username").val(),
        Password: $("#Contrasena").val(),
        ConfirmarPassword: $("#ContrasenaConf").val(),
        Nombre : $("#Nombre").val(),
        Apellido : $("#Apellido").val(),
        Correo: $("#Correo").val(),
        CargoId: $("#CargoId").val(),
        RolId : $("#RolId").val(),
        Estado: $("#Estado").val(),
        UsuarioRegistro: $("#usernameLogOn strong").text()
    };

    if(modelView.Id == 0)
        action = 'Add';
    else
        action = 'Update';

    webApp.Ajax({
        url: urlMantenimiento + action,
        parametros: modelView
    }, function(response){        
        if (response.Success) {            
            if(response.Warning){                           
                $.gritter.add({
                    title: response.Title,
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });                         
            } else {
                $("#NuevoUsuario").modal("hide");
                dataTableUsuario.ajax.reload();
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

function CargarCargo() {
    var WhereFilter = {
        idtabla: 2
    };
    webApp.Ajax({
        url: urlListaCargo + 'GetAll',
        async: false,
        parametros: WhereFilter,
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
                    $("#CargoId").append('<option value="' + item.Id + '">' + item.tbpd_vdescripcion_detalle + '</option>');
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

function CargarRol(){
    var WhereFilter = {
        idtabla: 3
    };
    webApp.Ajax({
        url: urlMantenimiento + 'GetAll',
        parametros: WhereFilter,
        async: false,
    }, function(response){
        if(response.Success){
            
            if(response.Warning){                           
                $.gritter.add({
                    title: 'Alerta',
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });                         
            } else {
                $("#RolIdSearch").append('<option value=""> - TODOS - </option>');
                $.each(response.Data, function(index, item){
                    $("#RolId,#RolIdSearch").append('<option value="' + item.Id + '">' + item.tbpd_vdescripcion_detalle + '</option>');
                });
                console.log(response.Data);
                webApp.clearForm('UsuarioSearchForm');
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

function CargarEstado() {
    var modelView = {
        idtabla: 1
    };
    webApp.Ajax({
        url: urlMantenimiento + 'GetAll',
        async: false,
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

function LimpiarFormulario(){
    webApp.clearForm(formularioMantenimiento);
    $("#CargoId").val(1);
    $("#RolId").val(1);
    $("#Estado").val(1);
    $("#Username").focus();
    $("#Contrasena").prop("type", "password");
}