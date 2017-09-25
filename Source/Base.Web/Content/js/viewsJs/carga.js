var dataTableCarga = null;
var formularioMantenimiento = "CargaForm";
var formularioMantenimientoMasiva = "CargaFormMasiva";

var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'Carga/Listar';
var urlMantenimiento = baseUrlApiService + 'Carga/';
var urlListaTipoProducto = baseUrlApiService + 'TipoProducto/';
var rowCarga = null;

$(document).ready(function () {
    $.extend($.fn.dataTable.defaults, {
        language: { url: baseUrl + 'Content/js/dataTables/Internationalisation/es.txt' },
        responsive: true,
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "bProcessing": true,
        "dom": 'fltip'
    });
    checkSession(function () {
        VisualizarDataTableCarga();
    });



    $('body').on('click', 'a.agregarCarga', function () {
        LimpiarFormulario();
        desHabilitarControles(false);
        $("#accionTitle").text('Nueva');

        $("#NuevoCarga").modal("show");
    });

    $('body').on('click', 'a.uploadCarga', function () {
        $("#TipoProductoIdMasiva").val(1);
        EliminarArchivo('fuCargaFileLocal');
        $("#accionTitleMasiva").text('Nueva');

        $("#NuevoCargaUpload").modal("show");
    });

    $("#linkUpload").click(function () {
        $("#fuCargaFileLocal").trigger("click");
    });

    $('body').on('click', 'a.editarCarga', function () {
        rowCarga = this;
        checkSession(function () {
            GetCargaById();
        });
    });

    $('body').on('click', 'a.eliminarCarga', function () {
        var aPos = dataTableCarga.fnGetPosition(this.parentNode.parentNode);
        var aData = dataTableCarga.fnGetData(aPos[0]);
        var rowID = aData.Id;

        delRowPos = aPos[0];
        delRowID = rowID;

        webApp.showDeleteConfirmDialog(function () {
            checkSession(function () {
                Eliminar();
            });
        }, 'Se eliminará el registro.  ¿Está seguro que desea continuar?');
    });

    $("#btnGuardarCarga").on("click", function (e) {

        if ($('#' + formularioMantenimiento).valid()) {
            webApp.showReConfirmDialog(function () {
                checkSession(function () {
                    GuardarCarga();
                });
            });
        }
        e.preventDefault();
    });

    $("#btnGuardarCargaMasiva").on("click", function (e) {
        if ($('#' + formularioMantenimientoMasiva).valid()) {
            webApp.showReConfirmDialog(function () {
                checkSession(function () {
                    GuardarCargaMasiva();
                });
            });
        }
        e.preventDefault();
    });

    $("body").on('change', '.fuCargaFileLocal', function () {
        var countFiles = $(this)[0].files.length;
        var imgPath = $(this)[0].value;
    
       
        var extn = imgPath.substring(imgPath.lastIndexOf('.') + 1).toLowerCase();
        var nameFileDocumento = imgPath.substring(imgPath.lastIndexOf('\\') + 1);
        var fuCargaFileLocal = "fuCargaFileLocal";
    
        if (/\s/.test(nameFileDocumento)) {
            webApp.showMessageDialog("Por favor el nombre del archivo no debe contener espacios en blanco.");
        }
        else
         { 
            if (extn == "xls" || extn == "xlsx") {
                if (typeof (FileReader) != "undefined") {
                    for (var i = 0; i < countFiles; i++) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $("#CargaFormMasiva #FileCarga").val(e.target.result);

                            var fileSize = Math.round(e.total / 1024);
                            var suffix = 'KB';
                            if (fileSize > 1000) {
                                fileSize = Math.round(fileSize / 1000);
                                suffix = 'MB';
                            }
                            var fileSizeParts = fileSize.toString().split('.');
                            fileSize = fileSizeParts[0];
                            if (fileSizeParts.length > 1) {
                                fileSize += '.' + fileSizeParts[1].substr(0, 2);
                            }
                            fileSize += suffix;

                            // Truncate the filename if it's too long
                            var fileName = nameFileDocumento;
                            if (fileName.length > 25) {
                                fileName = fileName.substr(0, 25) + '...';
                            }


                            // Create the file data object
                            var itemData = {
                                'fileID': 1,
                                'instanceID': fuCargaFileLocal,
                                'fileName': fileName,
                                'fileSize': fileSize
                            }

                            var itemTemplate = "";
                            itemTemplate = '<div id="${fileID}" class="uploadify-queue-item">\
                                                <div class="cancel">\
                                                    <a href="javascript:EliminarArchivo(\'${instanceID}\')">X</a>\
                                                </div>\
                                                <span class="fileName">${fileName}</span><span class="fileSize"> (${fileSize})</span><span class="data"></span>\
                                                <div class="uploadify-progress">\
                                                    <div class="uploadify-progress-bar"><!--Progress Bar--></div>\
                                                </div>\
                                            </div>';


                            // Replace the item data in the template
                            var itemHTML = itemTemplate;
                            for (var d in itemData) {
                                itemHTML = itemHTML.replace(new RegExp('\\$\\{' + d + '\\}', 'g'), itemData[d]);
                            }

                            // Add the file item to the queue
                            $('#' + fuCargaFileLocal + '-queue').html('');
                            $('#' + fuCargaFileLocal + '-queue').append(itemHTML);

                        }
                        reader.readAsDataURL($(this)[0].files[i]);
                    }
                } else {
                    webApp.showMessageDialog("Su explorador no soporta FileReader.");
                }
            } else {
                webApp.showMessageDialog("Por favor seleccione solo archivos excel");
            }
        }
    });

    webApp.validarLetrasEspacio(['Cliente']);
    webApp.validarAlfanumerico(['GC', 'CodigoGC']);

    webApp.InicializarValidacion(formularioMantenimiento,
        {
            Cliente: {
                required: true,
                noPasteAllowLetterAndSpaceAndNumberGuion:true
            },
            GC: {
                required: true
            },
            CodigoGC: {
                required: true
            },
            Denominacion: {
                required: true
            },
            TipoProductoId: {
                required: true
            }
        },
        {
            Cliente: {
                required: "Por favor ingrese Cliente"
            },
            GC: {
                required: "Por favor ingrese GC"
            },
            CodigoGC: {
                required: "Por favor ingrese CodigoGC"
            },
            Denominacion: {
                required: "Por favor ingrese Denominacion"
            },
            TipoProductoId: {
                required: "Por favor seleccione Tipo Producto"
            }
        });

    webApp.InicializarValidacion(formularioMantenimientoMasiva,
        {
            TipoProductoIdMasiva: {
                required: true
            },
            fuCargaFileLocal: {
                required: true
            }
        },
        {
            TipoProductoIdMasiva: {
                required: "Por favor ingrese Cliente"
            },
            fuCargaFileLocal: {
                required: "Por favor seleccione archivo"
            }
        });

    CargarTipoProducto();
});

function VisualizarDataTableCarga() {
    dataTableCarga = $('#CargaDataTable').dataTable({
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
                    <a class="green editarCarga" href="javascript:void(0)"><i class="ace-icon fa fa-pencil bigger-130"></i></a>\
                    <a class="red eliminarCarga" href="javascript:void(0)"><i class="ace-icon fa fa-trash-o bigger-130"></i></a>\
                    </div>';
                }
            },
            { "data": "Id" },
            { "data": "Cliente" },
            { "data": "GC" },
            { "data": "CodigoGC" },
            { "data": "Denominacion" },
            { "data": "TipoProductoNombre" },
            {
                "data": function (obj) {
                    if (obj.Quemado == "1")
                        return '<span class="label label-info label-sm arrowed-in arrowed-in-right">SI</span>';
                    else
                        return '<span class="label label-sm arrowed-in arrowed-in-right">NO</span>';
                }
            },
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
            { "aTargets": [2], "width": "15%" },
            { "className": "hidden-480", "aTargets": [3], "width": "10%" },
            { "className": "hidden-480", "aTargets": [4], "width": "15%" },
            { "className": "hidden-480", "aTargets": [5], "width": "15%" },
            { "className": "hidden-480", "aTargets": [6], "width": "15%" },
            { "bSortable": false, "aTargets": [7], "width": "10%" },
            { "bSortable": false, "aTargets": [8], "width": "10%" }
        ],
        "order": [[1, "desc"]]
    });
}

function LimpiarFormulario() {
    webApp.clearForm(formularioMantenimiento);
    $("#TipoProductoId").val(1);
    $("#Estado").val(1);
    $("#Estado").focus();
}

function Eliminar() {
    var modelView = {
        Id: delRowID,
        UsuarioRegistro: $("#usernameLogOn strong").text()
    };

    webApp.Ajax({
        url: urlMantenimiento + 'Delete',
        parametros: modelView

    }, function (response) {
        $("#NuevoCarga").modal("hide");
        if (response.Success) {
            dataTableCarga.fnDeleteRow(delRowPos);
            if (response.Warning) {
                $.gritter.add({
                    title: 'Alerta',
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });
            } else {
                NotificarAlerta();
                EnviarCorreoTope();
                $.gritter.add({
                    title: 'Eliminación Satisfactoria',
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

function GuardarCarga() {

    var modelView = {
        Id: $("#CargaId").val(),
        Numero: 0,
        Tipo: 2,
        Cliente: $("#Cliente").val(),
        GC: $("#GC").val(),
        CodigoGC: $("#CodigoGC").val(),
        Denominacion: $("#Denominacion").val(),
        TipoProductoId: $("#TipoProductoId").val(),
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
        $("#NuevoCarga").modal("hide");
        if (response.Success) {
            dataTableCarga.fnReloadAjax();
            if (response.Warning) {
                $.gritter.add({
                    title: response.Title,
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });
            } else {
                NotificarAlerta();
                EnviarCorreoTope();
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

function GetCargaById() {
    var aPos = dataTableCarga.fnGetPosition(rowCarga.parentNode.parentNode);
    var aData = dataTableCarga.fnGetData(aPos[0]);
    var rowID = aData.Id;

    var modelView = {
        Id: aData.Id
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
                desHabilitarControles(true);
                var carga = response.Data;
                $("#Cliente").val(carga.Cliente);
                $("#GC").val(carga.GC);
                $("#CodigoGC").val(carga.CodigoGC);
                $("#Denominacion").val(carga.Denominacion);
                $("#TipoProductoId").val(carga.TipoProductoId);
                $("#Estado").val(carga.Estado);
                $("#CargaId").val(carga.Id);

                $(".masivo").hide();
                $(".individual").show();

                $("#accionTitle").text('Editar');
                $("#NuevoCarga").modal("show");
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

function GuardarCargaMasiva() {

    var modelView = {
        TipoProductoId: $("#TipoProductoIdMasiva").val(),
        FileCarga: $("#FileCarga").val(),
        FileName: $("#fuCargaFileLocal-queue .fileName").text(),
        UsuarioRegistro: $("#usernameLogOn strong").text()
    };

    webApp.Ajax({
        url: urlMantenimiento + 'Upload',
        parametros: modelView
    }, function (response) {
        $("#NuevoCargaUpload").modal("hide");
        if (response.Success) {
            dataTableCarga.fnReloadAjax();
            if (response.Warning) {
                $.gritter.add({
                    title: response.Title,
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });
            } else {
                NotificarAlerta();
                EnviarCorreoTope();
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

function CargarTipoProducto() {
    var WhereFilters = {
        whereFilter: 'WHERE Estado IN (1,2)'
    };
    webApp.Ajax({
        url: urlListaTipoProducto + 'GetAll',
        parametros: WhereFilters,
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
                    $("#TipoProductoId").append('<option value="' + item.Id + '">' + item.Nombre + '</option>');
                    $("#TipoProductoIdMasiva").append('<option value="' + item.Id + '">' + item.Nombre + '</option>');
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

function desHabilitarControles(desHabilitar) {
    $("#Cliente").prop("disabled", desHabilitar);
    $("#GC").prop("disabled", desHabilitar);
    $("#CodigoGC").prop("disabled", desHabilitar);
    $("#Denominacion").prop("disabled", desHabilitar);
    $("#TipoProductoId").prop("disabled", desHabilitar);
}

function EliminarArchivo(fuDniFileLocal) {
    webApp.inicializarFileUpload(fuDniFileLocal);
    $("#" + fuDniFileLocal).val('');
    $("#FileCarga").val('');
}