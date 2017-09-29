﻿var dataTableLinea = null;
var dataTableSubLinea = null;
var dataTableCategoria = null;
var formularioMantenimientoLinea = "LineaForm";
var formularioMantenimientoSubLinea = "SubLineaForm";
var formularioResumen = "ResumenForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'TipoDocumento/Listar';
var urlMantenimiento = baseUrl + 'CategoriaLineaSublinea/';
var urlListaCargo = baseUrl + 'Usuario/';
var rowLinea = null;
var rowSubLinea = null;
var categoriaProductoLinea = new Array();
var categoriaProductoSubLinea = new Array();
var agregar = 1;
var actualizar = 2;
var eliminar = 3;
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

$(document).ready(function () {
    $.extend($.gritter.options, {
        time: '1000'
    });

    $.extend($.fn.dataTable.defaults, {
        language: { url: baseUrl + 'Content/js/dataTables/Internationalisation/es.txt' },
   
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
	            if (categoriaProductoLinea.length > 0) {
	                var wizard = $('#modal-wizard-container').data('fu.wizard')
	                wizard.currentStep = 3;
	                wizard.setState();
	                e.preventDefault();
	            }
	            else {
	                if (categoriaProductoLinea<=0) {
	                    webApp.showMessageDialog("Por favor debe ingresar una categoria de producto - Linea.");
	                    //wizard.currentStep = 2;
	                    //wizard.setState();
	                    e.preventDefault();
                    }
	            }
	        }
	        if (info.direction == "previous") {
	        }
	    }

	    if (info.step == 3) {
	        if (info.direction == "next") {
	            if (categoriaProductoSubLinea.length > 0) {
	                var wizard = $('#modal-wizard-container').data('fu.wizard')
	                wizard.currentStep = 4;
	                wizard.setState();
	                e.preventDefault();
	                getResumen();
	            }
	            else {
	                if (categoriaProductoSubLinea.length <= 0) {
	                    webApp.showMessageDialog("Por favor debe ingresar una categoria de producto - Sub-Linea.");
	                    e.preventDefault();
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
	    if ($('#ResumenForm').valid()) {
	        webApp.showConfirmResumeDialog(function () {
	            checkSession(function () {
	                GuardarCategoria();
	            });
	        },'Esta seguro de realizar la operación.');
	    } else {
	        webApp.showMessageDialog("Debe ingresar los datos correctamente");
	    }

	}).on('stepclick.fu.wizard', function (e) {
	    //e.preventDefault();//this will prevent clicking and selecting steps
	});

    //carga de Linea y sub-linea
    checkSession(function () {
        VisualizarDataTableCategoria();
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
        }
    });
    $('#CategoriaSubLineaDataTable  tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            dataTableSubLinea.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    $('#CategoriaDataTable  tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            dataTableCategoria.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    //acciones de Categoria
    $('#btnAgregarCategoria').on('click', function () {
        rest();
        $("#NuevoTipoCategoriaLinea").modal("show");
        $("#IdCategoria").val(0);
       
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

    //#region acciones de Categoria Linea
    $('#btnAgregarCategoriaLinea').on('click', function () {
        LimpiarFormularioLinea();
        $("#LineaId").val(0);
        $("#accionTitleLinea").text('Nuevo');
        $("#NuevoCategoriaLinea").modal("show");
        $("#codigoL").prop("disabled", false);
    });

    $('#btnEditarCategoriaLinea').on('click', function () {
        DrawEditLinea();
    });
 
    $('#btnEliminarCategoriaLinea').on('click', function () {
        rowLinea = dataTableLinea.row('.selected').data();
        if (typeof rowLinea === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            webApp.showDeleteConfirmDialog(function () {
                checkSession(function () {
 
                    DrawEliminarLinea(rowLinea.Id);
                });
            }, 'Se eliminará el registro. ¿Está seguro que desea continuar?');
        }
    });

    $("#btnGuardarLinea").on("click", function (e) {
        if ($('#' + formularioMantenimientoLinea).valid()) { 
            checkSession(function () {
                DrawAddLinea();
            });
        }
        e.preventDefault();
    });
    //#endregion acciones de Categoria Linea

    //#region acciones de Categoria SubLinea
    $('#btnAgregarCategoriaSubLinea').on('click', function () {
        LimpiarFormularioSubLinea();
        $("#SubLineaId").val(0);
        $("#accionTitleSubLinea").text('Nuevo');
        $("#NuevoCategoriaSubLinea").modal("show");
        $("#codigoLS").prop("disabled", false);
    });

    $('#btnEditarCategoriaSubLinea').on('click', function () {
        DrawEditSubLinea();
    });

    $('#btnEliminarCategoriaSubLinea').on('click', function () {
        rowSubLinea = dataTableSubLinea.row('.selected').data();
        if (typeof rowSubLinea === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            webApp.showDeleteConfirmDialog(function () {
                checkSession(function () {

                    DrawEliminarSubLinea(rowSubLinea.Id);
                });
            }, 'Se eliminará el registro. ¿Está seguro que desea continuar?');
        }
    });

    $("#btnGuardarSubLinea").on("click", function (e) {
        if ($('#' + formularioMantenimientoSubLinea).valid()) {
            checkSession(function () {
                DrawAddSubLinea();
            });
        }
        e.preventDefault();
    });
    //#endregion acciones de Categoria SubLinea
    $("#btnCancelar").on("click", function () {
        webApp.showConfirmResumeDialog(function () {
            $("#NuevoTipoCategoriaLinea").modal('hide');
            rest();
        }, '¿Está seguro de salir?');

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
            codigoLS: {
                required: true
            },
            descripcionLS: {
                required: true
            }
        },
        {
            codigoLS: {
                required: "Por favor ingrese Codigo"
            },
            descripcionLS: {
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


function VisualizarDataTableCategoria() {
    dataTableCategoria = $('#CategoriaDataTable').DataTable({
        "bFilter": false,
        "bProcessing": true,
        "serverSide": true,
        responsive: true,
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "bProcessing": true,
        "dom": 'fltip',
        //"scrollY": "350px",              
        "ajax": {
            "url": urlListar,
            "type": "POST",
            "data": function (request) {
                request.filter = new Object();

                request.filter = {
                    codigoSearch: $("#Codigosearch").val(),
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
            { "data": "tdocc_vabreviatura_tipo_doc" },
            { "data": "tdocc_vdescripcion" },
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
            //{ "bSortable": false, "sClass": "center", "aTargets": [0], "width": "10%" },
            { "bVisible": false, "aTargets": [0] },
            { "className": "hidden-120 center", "aTargets": [1], "width": "6%" },
            { "className": "hidden-120", "aTargets": [2], "width": "18%" },
            { "className": "hidden-1200", "aTargets": [3], "width": "4%" },
            { "bSortable": false, "className": "hidden-1200", "aTargets": [3], "width": "4%" }


        ],
        "order": [[1, "desc"]],
        "initComplete": function (settings, json) {
            //AddSearchFilter();
        },
        "fnDrawCallback": function (oSettings) {

        }
    });
}

function VisualizarDataTableLinea() {
    dataTableLinea = $('#CategoriaLineaDataTable').DataTable({
        "bFilter": false,
        "bProcessing": true,
        "serverSide": false,
        responsive: true,
        "lengthMenu": [[5], [5]],
        "bProcessing": true,
        "dom": 'fltip',
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
            { "data": "linc_vcod_linea" },
            { "data": "linc_vdescripcion" },
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
        responsive: true,
        "lengthMenu": [[5], [5]],
        "bProcessing": true,
        "dom": 'fltip',
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
            { "data": "lind_vcod_sublinea" },
            { "data": "lind_vdescripcion" },
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

//#region operaciones Linea
function DrawAddLinea() {
    var Id = $("#LineaId").val();
    var codigoL = $("#codigoL").val();
    var descripcionL = $("#descripcionL").val();
    var Estado = 1;
    var categoriaLinea = null;
    var exito = true;
    var exitoedit = true;
    if (Id==0) {
        if (categoriaProductoLinea.length > 0) {
            $.each(categoriaProductoLinea, function (index, value) {
               
                if (codigoL === value.linc_vcod_linea || descripcionL === value.linc_vdescripcion) {
                    $.gritter.add({
                        title: TitleAlerta,
                        text: YaExisteRegistro,
                        class_name: 'gritter-warning gritter',
                        
                    });
                    exito = false;
                    debugger;
                }               

            });
            if (exito) {
                debugger;
                categoriaLinea = { "Id": categoriaProductoLinea.length + 1, "linc_vcod_linea": codigoL, "linc_vdescripcion": descripcionL, "Estado": Estado, 'status': agregar };
                $("#NuevoCategoriaLinea").modal("hide");
                $.gritter.add({
                    title: TitleRegistro,
                    text: RegistroSatisfactorio,
                    class_name: 'gritter-success gritter',
               
                });
                categoriaProductoLinea.push(categoriaLinea);
                dataTableLinea.clear();
                dataTableLinea.rows.add(categoriaProductoLinea).draw();
            }
        }
        else {
            categoriaLinea = { "Id": 1, "linc_vcod_linea": codigoL, "linc_vdescripcion": descripcionL, "Estado": Estado, 'status': agregar };
      
            $("#NuevoCategoriaLinea").modal("hide");
            $.gritter.add({
                title: TitleRegistro,
                text: RegistroSatisfactorio,
                class_name: 'gritter-success gritter',
            });
            categoriaProductoLinea.push(categoriaLinea);
            dataTableLinea.clear();
            dataTableLinea.rows.add(categoriaProductoLinea).draw();
        }
 
    }
    else {
        $.each(categoriaProductoLinea, function (index, value) {
            if (value.linc_vdescripcion == descripcionL) {
                if (value.Id != parseInt(Id)) {
                    $.gritter.add({
                        title: TitleAlerta,
                        text: YaExisteRegistro,
                        class_name: 'gritter-warning gritter',
                    });
                    exitoedit = false;
                }
            }
            else {
                if (value.Id == parseInt(Id)) {
                    if (value.linc_vdescripcion == descripcionL) {
                        value.linc_vdescripcion = descripcionL;
                        value.status = actualizar;
                    } else {
                        value.linc_vdescripcion = descripcionL;
                        value.status = actualizar;
                    }
                }
            }
        });

        if (exitoedit) {
            dataTableLinea.clear();
            dataTableLinea.rows.add(categoriaProductoLinea).draw();
            $.gritter.add({
                title: TitleActualizar,
                text: ActualizacionSatisfactoria,
                class_name: 'gritter-success gritter',

            });
            $("#NuevoCategoriaLinea").modal("hide");
        }
  

    }
}

function DrawEditLinea() {
    rowLinea = dataTableLinea.row('.selected').data();
    if (typeof rowLinea === "undefined") {
        webApp.showMessageDialog("Por favor seleccione un registro.");
    }
    else {
        $("#accionTitleLinea").text('Editar');
        $("#NuevoCategoriaLinea").modal("show");
        $("#codigoL").val(rowLinea.linc_vcod_linea);
        $("#descripcionL").val(rowLinea.linc_vdescripcion);
        $("#codigoL").prop("disabled", true);
        $("#LineaId").val(rowLinea.Id);
       
    }
}

function DrawEliminarLinea(id) {

    var idEliminacion = -1;
    $.each(categoriaProductoLinea, function (index, value) {
        debugger;
        if (value.Id == id) {
            idEliminacion = index;   
            }
    });
    if (idEliminacion > -1) {
        categoriaProductoLinea.splice(idEliminacion, 1);
        dataTableLinea.clear();
        dataTableLinea.rows.add(categoriaProductoLinea).draw();
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
            class_name: 'gritter-success gritter'
        });
    }
}
// #endregion operaciones linea

//#region operaciones sub-linea
function DrawAddSubLinea() {
    var Id = $("#SubLineaId").val();
    var codigoLS = $("#codigoLS").val();
    var descripcionLS = $("#descripcionLS").val();
    var Estado = 1;
    var categoriaSubLinea = null;
    var exito = true;
    var exitoedit = true;
    if (Id == 0) {
        if (categoriaProductoSubLinea.length > 0) {
            $.each(categoriaProductoSubLinea, function (index, value) {

                if (codigoLS === value.lind_vcod_sublinea || descripcionLS === value.lind_vdescripcion) {
                    $.gritter.add({
                        title: TitleAlerta,
                        text: YaExisteRegistro,
                        class_name: 'gritter-warning gritter',
                    });
                    exito = false;
                    debugger;
                }
            });
            if (exito) {
                debugger;
                categoriaSubLinea = { "Id": categoriaProductoSubLinea.length + 1, "lind_vcod_sublinea": codigoLS, "lind_vdescripcion": descripcionLS, "Estado": Estado, 'status': agregar };
                $("#NuevoCategoriaSubLinea").modal("hide");
                $.gritter.add({
                    title: TitleRegistro,
                    text: RegistroSatisfactorio,
                    class_name: 'gritter-success gritter',

                });
                categoriaProductoSubLinea.push(categoriaSubLinea);
                dataTableSubLinea.clear();
                dataTableSubLinea.rows.add(categoriaProductoSubLinea).draw();
            }
        }
        else {
            categoriaSubLinea = { "Id": 1, "lind_vcod_sublinea": codigoLS, "lind_vdescripcion": descripcionLS, "Estado": Estado, 'status': agregar };

            $("#NuevoCategoriaSubLinea").modal("hide");
            $.gritter.add({
                title: TitleRegistro,
                text: RegistroSatisfactorio,
                class_name: 'gritter-success gritter',
            });
            categoriaProductoSubLinea.push(categoriaSubLinea);
            dataTableSubLinea.clear();
            dataTableSubLinea.rows.add(categoriaProductoSubLinea).draw();
        }

    }
    else {
        $.each(categoriaProductoSubLinea, function (index, value) {
            if (value.lind_vdescripcion == descripcionLS) {
                if (value.Id != parseInt(Id)) {
                    $.gritter.add({
                        title: TitleAlerta,
                        text: YaExisteRegistro,
                        class_name: 'gritter-warning gritter',
                    });
                    exitoedit = false;
                }
            }
            else {
                if (value.Id == parseInt(Id)) {
                    if (value.lind_vdescripcion == descripcionLS) {
                        value.lind_vdescripcion = descripcionLS;
                        value.status = actualizar;
                    }
                    else {
                        value.lind_vdescripcion = descripcionLS;
                        value.status = actualizar;
                    }
                }
            }
        });

        if (exitoedit) {
            dataTableSubLinea.clear();
            dataTableSubLinea.rows.add(categoriaProductoSubLinea).draw();
            $.gritter.add({
                title: TitleActualizar,
                text: ActualizacionSatisfactoria,
                class_name: 'gritter-success gritter',

            });
            $("#NuevoCategoriaSubLinea").modal("hide");
        }


    }
}

function DrawEditSubLinea() {
    rowSubLinea = dataTableSubLinea.row('.selected').data();
    if (typeof rowSubLinea === "undefined") {
        webApp.showMessageDialog("Por favor seleccione un registro.");
    }
    else {
        $("#accionTitleSubLinea").text('Editar');
        $("#NuevoCategoriaSubLinea").modal("show");
        $("#codigoLS").val(rowSubLinea.lind_vcod_sublinea);
        $("#descripcionLS").val(rowSubLinea.lind_vdescripcion);
        $("#codigoLS").prop("disabled", true);
        $("#SubLineaId").val(rowSubLinea.Id);

    }
}

function DrawEliminarSubLinea(id) {

    var idEliminacion = -1;
    $.each(categoriaProductoSubLinea, function (index, value) {
        debugger;
        if (value.Id == id) {
            idEliminacion = index;
        }
    });
    if (idEliminacion > -1) {
        categoriaProductoSubLinea.splice(idEliminacion, 1);
        dataTableSubLinea.clear();
        dataTableSubLinea.rows.add(categoriaProductoSubLinea).draw();
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
            class_name: 'gritter-success gritter'
        });
    }
}

//#endregion operaciones sub-linea

function getResumen() {
    LimpiarFormularioResumen();
    $("#categoriaProductoRsm").text($("#descripcionCP").val());
    $("#countLineaRsm").text(categoriaProductoLinea.length);
    $("#countSubLineaRsm").text(categoriaProductoSubLinea.length);

};




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
                LimpiarFormularioLinea();
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

function GuardarCategoria() {

    var modelView = {
        Id: $("#IdCategoria").val(),
        ctgcc_vcod_categoria: $("#codigoCP").val(),
        ctgcc_vdescripcion: $("#descripcionCP").val(),
        detalleLinea: categoriaProductoLinea,
        detalleSubLinea:categoriaProductoSubLinea,
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
                $("#NuevoTipoCategoriaLinea").modal("hide");
                dataTableCategoria.ajax.reload();
                categoriaProductoLinea.length = 0;
                categoriaProductoSubLinea.length = 0;
                
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

function LimpiarFormularioLinea() {
    webApp.clearForm(formularioMantenimientoLinea);
    $("#codigoL").focus();
}
function LimpiarFormularioSubLinea() {
    webApp.clearForm(formularioMantenimientoSubLinea);
    $("#codigoLS").focus();
}
function LimpiarFormularioResumen() {
    webApp.clearForm(formularioResumen);
}
function rest() {
    LimpiarFormularioLinea();
    LimpiarFormularioSubLinea();
    LimpiarFormularioResumen();
    categoriaProductoLinea.length = 0;
    categoriaProductoSubLinea.length = 0;
}