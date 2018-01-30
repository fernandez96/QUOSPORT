var dataTableProveedor = null;
var formularioMantenimiento = "proveedorForm";
var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'Proveedor/Listar';
var urlMantenimiento = baseUrl + 'Proveedor/';
var urlListaCargo = baseUrl + 'Proveedor/';
var rowCliente = null;


$(document).ready(function () {
    $.extend($.fn.dataTable.defaults, {
        language: { url: baseUrl + 'Content/js/dataTables/Internationalisation/es.txt' },
        responsive: true,
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "bProcessing": true,
        "dom": 'fltip'
    });

    checkSession(function () {
         VisualizarDataTableProveedor();
    });

    $('#proveedorDataTable  tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            dataTableProveedor.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $('#btnAgregarCliente').on('click', function () {
     
        if ($("#natural").is(":checked")) {
            $("#juridico").prop("checked", false);
            var valor = $('input:radio[id=natural]:checked').val();
            FormularioJuridico(true);
            FormularioNatural(false);
        }
  
        LimpiarFormulario();

        $("#UsuarioId").val(0);
        $("#accionTitle").text('Nuevo');
        $("#NuevoCliente").modal("show");
        $("#Username").prop("disabled", false);
    });

    $('#editarUsuario').on('click', function () {
        rowUsuario = dataTableProveedor.row('.selected').data();
        if (typeof rowUsuario === "undefined") {
            webApp.showMessageDialog("Por favor seleccione un registro.");
        }
        else {
            checkSession(function () {
                GetUsuarioById();
            });
        }

    });

    $('#eliminarUsuario').on('click', function () {
        rowUsuario = dataTableProveedor.row('.selected').data();
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

    $("#natural").on('click', function () {
        $("#juridico").prop("checked", false);
        var valor = $('input:radio[id=natural]:checked').val();
        FormularioJuridico(true);
        FormularioNatural(false);
    });


    $("#juridico").on('click', function () {
        $("#natural").prop("checked", false);
        var valor = $('input:radio[id=juridico]:checked').val();
        FormularioNatural(true);
        FormularioJuridico(false);
    });


    $("#btnSearchUsuario").on("click", function (e) {
        if ($('#UsuarioSearchForm').valid()) {
            checkSession(function () {
                dataTableUsuario.ajax.reload();
            });
        }
        e.preventDefault();
    });

    $("#btnGuardarProveedor").on("click", function (e) {
   
        if ($("#natural").prop("checked")) {
            ValidateNatural();
            if ($('#' + formularioMantenimiento).valid()) {
                checkSession(function () {
                    GuardarUsuario();
                });
            }
        }
        else if ($("#juridico").prop("checked")) {

        }

    

        e.preventDefault();
    });

    //evento de verificacion de seleccion de radio(tipo persona )
    //click persona natural
    //if ($("#natural").prop("checked")) {
    //    alert("1");
    //}
    //if ($("#juridico").prop("checked")) {
    //    alert("2");
    //}
    //$("#natural").on("click", function () {
     
    //});


    webApp.validarLetrasEspacio(['Nombre', 'Paterno', 'Materno']);
    webApp.validarNumerico(['Telefono', 'Celular', 'RUC']);
    $('#Correo').validCampoFranz(' @abcdefghijklmnÃ±opqrstuvwxyz_1234567890.');

   
    //CargarCargo();
    //CargarRol();
    //CargarEstado();
    $('[data-toggle="tooltip"]').tooltip();
    $('#fecha').datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        todayHighlight: true,
        language: 'es',
        //startView: "months",
        //minViewMode: "months"
    });
});

//validad persona natural
function ValidateNatural() {
    webApp.InicializarValidacion(formularioMantenimiento,
          {
              Codigo: {
                  required: true

              },
              TDpcumento: {
                  required: true
              },
              Documento: {
                  required: true
              },
              Nombre: {
                  required: true,
                  noPasteAllowLetterAndSpace: true,
                  firstCharacterBeLetter: true
              },
              Paterno: {
                  required: true,
                  noPasteAllowLetterAndSpace: true,
                  firstCharacterBeLetter: true
              },
              Materno: {
                  required: true
              },
              FAX: {
                  required: true
              },
              inpDireccion: {
                  required: true
              },
              Telefono: {
                  strippedminlength: {
                      param: 6
                  },
              },
              Celular: {
                  strippedminlength: {
                      param:9
                  }
              },
              fecha: {
                  required: true
              },
              Correo: {
                  email:true
              },
              Representante: {
                  required: true
              },
              Estado: {
                  required:true
              }
          },
          {
              Codigo: {
                  required: "Por favor ingrese Codigo.",

              },
              TDpcumento: {
                  required: "Por favor seleccione un tipo documento.",

              },
              Documento: {
                  required: "Por favor ingrese Documento.",

              },
              Nombre: {
                  required: "Por favor ingrese Nombre."
              },
              Paterno: {
                  required: "Por favor ingrese Apellido Paterno"
              },

              Materno: {
                  required: "Por favor Ingrese Apellido Materno."
              },
              FAX: {
                  required: "Por favor Ingrese FAX."
              },
              inpDireccion: {
                  required: "Por favor ingrese Direccion."
              },
              Telefono: {
                  strippedminlength: "Por favor ingrese al menos 6 caracteres."
              },
              Celular:{
                  strippedminlength:"Por favor ingrese 9 digitos."
              },
              fecha: {
                  required:"Por favor ingrese Fecha."
              },
              Correo: {
                  email:"Por favor ingrese correo valido."
              },
              Representante: {
                  required:"Por favor ingrese Representante."
              },
              Estado: {
                  required:"Por favor seleccione estado."
              }
             
          });
}


function VisualizarDataTableProveedor() {
    dataTableProveedor = $('#proveedorDataTable').DataTable({
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
                    CodigoSearch: $("#CodigoSearch").val(),
                    DescripcionSearch: $("#descripcionSearch").val()
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
            { "data": "proc_vcod_proveedor" },
            {
                "data": function (obj) {
                    if (obj.proc_tipo_persona == "1")
                        return "Persona Natural";
                    else
                        return "Persona Juridica";
                }
            },
      
            { "data": "proc_vnombrecompleto" },
            { "data": "proc_vruc" },
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

            { "bVisible": false, "aTargets": [0] },
            { "className": "hidden-120", "aTargets": [1], "width": "10%" },
            { "className": "hidden-120", "aTargets": [2], "width": "18%" },
            { "className": "hidden-992", "aTargets": [3], "width": "18%" },
            { "className": "hidden-768", "aTargets": [4], "width": "20%" },
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

function CargarRol() {
    var WhereFilter = {
        idtabla: 3
    };
    webApp.Ajax({
        url: urlMantenimiento + 'GetAll',
        parametros: WhereFilter,
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
                $("#RolIdSearch").append('<option value=""> - TODOS - </option>');
                $.each(response.Data, function (index, item) {
                    $("#RolId,#RolIdSearch").append('<option value="' + item.Id + '">' + item.tbpd_vdescripcion_detalle + '</option>');
                });
                console.log(response.Data);
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
    $("#natural").prop("checked", true);
}
function FormularioNatural(valor) {
    $("#Nombre").prop("disabled", valor);
    $("#Paterno").prop("disabled", valor);
    $("#Materno").prop("disabled", valor);
    $("#Nombre").val('');
    $("#Paterno").val('');
    $("#Materno").val('');
}
function FormularioJuridico(valor) {
    $("#RUC").prop("disabled", valor);
    $("#RSocial").prop("disabled", valor);
    $("#NombreComercial").prop("disabled", valor);
    $("#RUC").val('');
    $("#RSocial").val('');
    $("#NombreComercial").val('');
}