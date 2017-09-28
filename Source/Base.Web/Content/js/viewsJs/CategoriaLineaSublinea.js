var dataTableLinea = null;
var dataTableSubLinea = null;
var formularioMantenimientoLinea = "LineaForm";
var formularioMantenimientoSubLinea = "SubLineaForm";

var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'Usuario/Listar';
var urlMantenimiento = baseUrl + 'Usuario/';
var urlListaCargo = baseUrl + 'Usuario/';
var rowLinea = null;
var rowSubLinea = null;
var categoriaProductoLinea = new Array();
var categoriaProductoSubLinea = new Array();

$(document).ready(function () {
    $.extend($.fn.dataTable.defaults, {
        language: { url: baseUrl + 'Content/js/dataTables/Internationalisation/es.txt' },
        responsive: true,
        "lengthMenu": [[5], [5]],
        "bProcessing": true,
        "dom": 'fltip'
    });

    var $validation = true;
    $('#modal-wizard-container')
	.ace_wizard({
	    //step: 2, //optional argument. wizard will jump to step "2" at first
	    //buttons: '.wizard-actions:eq(0)'
	})
	.on('actionclicked.fu.wizard', function (e, info) {
	    if (info.step == 1) {
	        if (info.direction == "next") {
	            if (!$('#frmCategoriaProducto').valid()) {
	                e.preventDefault();
	            
	            } else {	             
	                    var wizard = $('#modal-wizard-container').data('fu.wizard')
	                    wizard.currentStep = 1;
	                    wizard.setState();
	                    $("#frmCategoriaProducto .form-group").removeClass('has-error');
	                    $("#frmCategoriaProducto .help-block").remove();
	            }
	        }

	        if (info.direction == "previous") {

	        }
	    }

	    if (info.step == 2) {
	        if (info.direction == "next") {
	            if (categoriaProductoLinea.length == 0) {
	                var wizard = $('#modal-wizard-container').data('fu.wizard')
	                wizard.currentStep = 3;
	                wizard.setState();
	                e.preventDefault();
	            }
	            else {
	                if (categoriaProductoLinea<=0) {
	                    webApp.showMessageDialog("Por favor debe ingresar una categoria de producto - Linea.");
	                    wizard.currentStep = 2;
	                    wizard.setState();
	                
	                }	               
	            }
	        }
	        if (info.direction == "previous") {
	        }
	    }


	    if (info.step == 3) {
	        if (info.direction == "next") {
	            if (categoriaProductoSubLinea.length == 0) {
	                var wizard = $('#modal-wizard-container').data('fu.wizard')
	                wizard.currentStep = 4;
	                wizard.setState();
	                e.preventDefault();
	            }
	            else {
	                if (categoriaProductoSubLinea.length <= 0) {
	                    webApp.showMessageDialog("Por favor debe ingresar una categoria de producto - Sub-Linea.");
	                    var wizard = $('#modal-wizard-container').data('fu.wizard')
	                    wizard.currentStep = 3;
	                    wizard.setState();
	                }
	            }
	        }
	        if (info.direction == "previous") {
	        }
	    }

	    if (info.step == 4) {
	        if (info.direction == "previous") {
	                var wizard = $('#modal-wizard-container').data('fu.wizard')
	                wizard.currentStep = 3;
	                wizard.setState();
	                e.preventDefault();
	            
	        }
	    }
	})
	.on('finished.fu.wizard', function (e) {
	    if ($('#frmCanje').valid()) {
	        webApp.showConfirmResumeDialog(function () {
	            checkSession(function () {
	                GuardarCanje();
	            });
	        },
			GetResumen());
	    } else {
	        webApp.showMessageDialog("Debe ingresar los datos correctamente");
	    }

	}).on('stepclick.fu.wizard', function (e) {
	    //e.preventDefault();//this will prevent clicking and selecting steps
	});

    //carga de Linea 
    checkSession(function () {
        VisualizarDataTableLinea();
        VisualizarDataTableSubLinea()
    });
    $('#CategoriaLineaDataTable  tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            dataTableLinea.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
            rowLinea = $(this).closest("tr.selected").get(0);
            console.log(rowLinea);
        }
    });

 
    //acciones de Categoria
    $('#btnAgregarCategoria').on('click', function () {
        $("#NuevoTipoCategoriaLinea").modal("show");
    });

    $('#btnEditarCategoria').on('click', function () {
        rowUsuario = dataTableUsuario.row('.selected').data();
        if (typeof rowUsuario === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            checkSession(function () {
                GetUsuarioById();
            });
        }

    });

    $('#btnEliminarCategoria').on('click', function () {
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

    //acciones de Categoria Linea
    $('#btnAgregarCategoriaLinea').on('click', function () {
        LimpiarFormulario();
        $("#CategoriaLineaId").val(0);
        $("#accionTitleLinea").text('Nuevo');
        $("#NuevoCategoriaLinea").modal("show");
        $("#codigoL").prop("disabled", false);
    });
    $('#btnEditarCategoriaLinea').on('click', function () {
        DrawEditLinea();
    });
 

    $("#btnGuardarLinea").on("click", function (e) {
        if ($('#' + formularioMantenimientoLinea).valid()) { 
            checkSession(function () {
                DrawAddLinea();
            });
        }
        e.preventDefault();
    });
    webApp.validarNumerico(['DniTitular']);
    webApp.InicializarValidacion(formularioMantenimientoLinea,
        {
            codigoL: {
                required: true,
            },
            descripcionL: {
                required: true,
            },
            
        },
        {
            codigoL: {
                required: "Por favor ingrese Codigo",
            },
            descripcionL: {
                required: "Por favor ingrese Descripción",
            },
           
        }

        );
  
    webApp.InicializarValidacion(formularioMantenimientoSubLinea,
        {
            codigoSL: {
                required: true
            },
            descripcionSL: {
                required: true
            }
        },
        {
            codigoSL: {
                required: "Por favor ingrese Codigo"
            },
            descripcionSL: {
                required: "Por favor ingrese Descripción"
            }
        });
    webApp.InicializarValidacion('frmCategoriaProducto',
       {
           codigoCP: {
               required: true
           },
           descripcionCP: {
               required: true
           }
       },
       {
           codigoCP: {
               required: "Por favor ingrese Codigo"
           },
           descripcionCP: {
               required: "Por favor ingrese Descripción"
           }
       });
});




function VisualizarDataTableLinea() {
    dataTableLinea = $('#CategoriaLineaDataTable').dataTable({
        "bFilter": false,
        "bProcessing": true,
        "serverSide": false,
        //"scrollY": "350px",  

        "data": [],
        //"scrollY": "350px",  
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
            { "data": "codigoL" },
            { "data": "descripcionL" },
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
            { "className": "center hidden-120", "aTargets": [1], "width": "10%" },
            { "className": "hidden-120", "aTargets": [2], "width": "18%" },
            { "bSortable": false, "className": "hidden-480", "aTargets": [3], "width": "10%" }

        ],
        "order": [[1, "asc"]],
        "initComplete": function (settings, json) {
            // AddSearchFilter();
        },
        "fnDrawCallback": function (oSettings) {

        }
    });
}

function VisualizarDataTableSubLinea() {
    dataTableSubLinea = $('#CategoriaSubLineaDataTable').DataTable({
        "bFilter": false,
        "bProcessing": true,
        //"serverSide": true,
        //"scrollY": "350px",  

        "data": [],
        //"scrollY": "350px",  
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
            { "data": "Codigo" },
            { "data": "descripcion" },
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

function DrawAddLinea() {
    var Id = $("#CategoriaLineaId").val();
    var codigoL = $("#codigoL").val();
    var descripcionL = $("#descripcionL").val();
    var Estado = 1;
    var categoriaLinea = null;
    var exito = true;
    if (Id==0) {
        if (categoriaProductoLinea.length > 0) {
            $.each(categoriaProductoLinea, function (index, value) {
               
                if (codigoL === value.codigoL || descripcionL === value.descripcionL) {
                    $.gritter.add({
                        title: 'Alerta',
                        text: 'Registro ya existe.',
                        class_name: 'gritter-warning gritter'
                    });
                    exito = false;
                    debugger;
                }               

            });
            if (exito) {
                debugger;
                categoriaLinea = { "Id": categoriaProductoLinea.length + 1, "codigoL": codigoL, "descripcionL": descripcionL, "Estado": Estado };
                $("#NuevoCategoriaLinea").modal("hide");
                $.gritter.add({
                    title: 'Registro Satisfactorio',
                    text: 'Se realizo el registro satifactoriamente.',
                    class_name: 'gritter-success gritter'
                });
                categoriaProductoLinea.push(categoriaLinea);
                dataTableLinea.fnClearTable();
                dataTableLinea.fnAddData(categoriaProductoLinea);
            }
        }
        else {
            categoriaLinea = { "Id": 1, "codigoL": codigoL, "descripcionL": descripcionL, "Estado": Estado };
      
            $("#NuevoCategoriaLinea").modal("hide");
            $.gritter.add({
                title: 'Registro Satisfactorio',
                text: 'Se realizo el registro satifactoriamente.',
                class_name: 'gritter-success gritter'
            });
            categoriaProductoLinea.push(categoriaLinea);
            dataTableLinea.fnClearTable();
            dataTableLinea.fnAddData(categoriaProductoLinea);
        }
 
    }
    else {
        $.each(categoriaProductoLinea, function (index, value) {
            if (value.Id == parseInt(Id)) {
                debugger;
                value.descripcionL = descripcionL;
            }
        });

        dataTableLinea.draw();
        $.gritter.add({
            title: 'Actualización Satisfactorio',
            text: 'Se realizo la actualización satifactoriamente.',
            class_name: 'gritter-success gritter'
        });
        $("#NuevoCategoriaLinea").modal("hide");
     
     
    }
}

function DrawEditLinea() {
    //rowLinea = dataTableLinea.row('.selected').data();
  
    var aPos = dataTableLinea.fnGetPosition(rowLinea);
    var aData = dataTableLinea.fnGetData(aPos[0]);
    console.log(aData);
    if (aData.Id === "undefined") {
        webApp.showMessageDialog("Por favor seleccione un registro.");
    }
    else {
        $("#accionTitleLinea").text('Editar');
        $("#NuevoCategoriaLinea").modal("show");
        $("#codigoL").val(aData[0].codigoL);
        $("#descripcionL").val(aData[0].descripcionL);
        $("#codigoL").prop("disabled", true);
        $("#CategoriaLineaId").val(aData[0].Id);
       
    }
}


function GetUsuarioById() {
    var modelView = {
        Id: rowUsuario.Id
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

function EliminarUsuario() {
    var modelView = {
        Id: rowUsuario.Id,
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
                $("#NuevoUsuario").modal("hide");
                dataTableUsuario.row('.selected').remove().draw();
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

function GuardarUsuario() {

    var modelView = {
        Id: $("#UsuarioId").val(),
        Username: $("#Username").val(),
        Password: $("#Contrasena").val(),
        ConfirmarPassword: $("#ContrasenaConf").val(),
        Nombre: $("#Nombre").val(),
        Apellido: $("#Apellido").val(),
        Correo: $("#Correo").val(),
        CargoId: $("#CargoId").val(),
        RolId: $("#RolId").val(),
        Estado: $("#Estado").val(),
        UsuarioRegistro: $("#usernameLogOn strong").text()
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
                $("#NuevoUsuario").modal("hide");
                dataTableUsuario.ajax.reload();
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

function LimpiarFormulario() {
    webApp.clearForm(formularioMantenimientoLinea);
    $("#CargoId").val(1);
    $("#RolId").val(1);
    $("#Estado").val(1);
    $("#Username").focus();
    $("#Contrasena").prop("type", "password");
}