
var dataTableCarga = null;
var formularioMantenimiento = "CargaForm";
var formularioMantenimientoMasiva = "CargaFormMasiva";

var delRowPos = null;
var delRowID = 0;
var urlListar = baseUrl + 'EnvioEmail/Listar';
var urlMantenimiento = baseUrlApiService + 'EnvioEmail/';
var rowCarga = null;
var imgPath = null;
$(document).ready(function () {

    $("#linkUpload").click(function () {
        $("#fuCargaFileLocal").trigger("click");
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
         imgPath = $(this)[0].value;

        if (imgPath.match(/fakepath/)) {
            imgPath = imgPath.replace(/C:\\fakepath\\/i, 'E:\\');
        }
    
        var extn = imgPath.substring(imgPath.lastIndexOf('.') + 1).toLowerCase();
        var nameFileDocumento = imgPath.substring(imgPath.lastIndexOf('\\') + 1);
        var fuCargaFileLocal = "fuCargaFileLocal";
        if (/\s/.test(nameFileDocumento)) {
            webApp.showMessageDialog("Por favor el nombre del archivo no debe contener espacios en blanco.");
        }
        else {
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
});




function EliminarArchivo(fuDniFileLocal) {
    webApp.inicializarFileUpload(fuDniFileLocal);
    $("#" + fuDniFileLocal).val('');
    $("#FileCarga").val('');
}

function GuardarCargaMasiva() {

    var modelView = {
        FileCarga: $("#FileCarga").val(),
        FileName: $("#fuCargaFileLocal-queue .fileName").text(),
        UsuarioRegistro: $("#usernameLogOn strong").text()
    };

    webApp.Ajax({
        url: urlMantenimiento + 'Upload',
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
                EliminarArchivo('fuCargaFileLocal');
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


