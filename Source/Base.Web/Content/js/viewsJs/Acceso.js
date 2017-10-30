var urlMantenimiento = baseUrl + 'Acceso/';
var dataTableRol = null;
var delRowID = 0;
var delRowPos = null;
var urlListar = baseUrl + 'Acceso/Listar';
var nombreRol = null;
var urlThemesJstree = "~/Content/js/jsTree/default/style.css";
var urlObtenerEstructuraJerarquia = baseUrl + 'Acceso/ObtenerEstructuraJerarquia';
$(document).ready(function () {
    $.extend($.fn.dataTable.defaults, {
        language: { url: baseUrl + 'Content/js/dataTables/Internationalisation/es.txt' },
        responsive: true,
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "bProcessing": true,
        "dom": 'fltip',
        "paging":   false,
        "ordering": false,
        "info":     false
    });
    checkSession(function () {
        VisualizarDataTableRol();
    });


    

            $("#divEstructuraMenu").jstree({
                'core': {
                    'data': {
                        'url': urlObtenerEstructuraJerarquia,
                        "type": "POST",
                        "data":"",
                        "dataType": "json",
                        "contentType": "application/json charset=utf-8"
                    },
                    "strings": {
                        'Loading ...': 'Cargando ...'
                    },

                    "animation": 500,
                    'check_callback': true,
                    'themes': {
                        "theme": "default",
                        "dots": true,
                        "icons": true,
                        "url": urlThemesJstree
                    }
                },
                'force_text': true,
                'plugins': ["themes", "json_data", "ui", "crrm", "contextmenu", "search", "dnd"],
                "contextmenu": {
                    items: function ($node) {
                        var tree = $("#divEstructuraMenu").jstree(true);
                        var idMenu = $node.id;

                        $('#divEstructuraMenu').jstree("select_node", '#' + idMenu, true);
                        if (idMenu != -1) {
                            return {
                                Create: {
                                    label: textoNuevaJerarquia,
                                    action: function (obj) {
                                        $node = tree.create_node($node);
                                        tree.edit($node);
                                        alert($node);
                                    }
                                },
                                Rename: {
                                    label: textoRenombrarJerarquia,
                                    action: function (obj) {
                                        tree.edit($node);
                                    }
                                },
                                Delete: {
                                    label: textoEliminarJerarquia,
                                    action: function (data) {
                                        dataNodo = {
                                            instance: $.jstree.reference(data.reference),
                                            node: $node
                                        };

                                        var mensajeEliminar = textoConfirmarEliminarJerarquia.replace("{0}", dataNodo.node.text);

                                        $('#mensajeEliminar').text(mensajeEliminar);
                                        $('#popupEliminar').modal('show');
                                    }
                                },
                                AddUser: {
                                    label: textoAsignarPersonalJerarquia,
                                    action: function (data) {
                                        asignarUsuario($node.id);
                                    }
                                }
                            };
                        } else {
                            return {
                                Create: {
                                    label: textoNuevaJerarquia,
                                    action: function (data) {
                                        $node = tree.create_node($node);
                                        tree.edit($node);
                                    }
                                }
                            };
                        }
                    }
                }
            })
            .on("create_node.jstree", function (e, data) {
                dataNodo = data;
                operacionInsertar = true;
            })
            .on("rename_node.jstree", function (e, data) {
                if (operacionInsertar) {
                    $('#popupEjecutarAccion [id=inputMasterAccion]').val("insertarOrga");
                    $('#popupEjecutarAccion .modal-body').html(textoConfirmarAgregarJerarquia);
                    $('#popupEjecutarAccion').modal('show');
                } else {
                    $.ajax({
                        url: urlRenombrarJerarquia,
                        type: 'POST',
                        data: {
                            "name": data.text,
                            "id": data.node.id,
                            "idPais": $("#IdPais").val()
                        },
                        success: function (response) {
                            if (Utils.TerminoSesion(response)) {
                                data.instance.refresh();
                                if (response.Success) {

                                    $("#Descripcion").val(data.text);
                                }

                                window.messageInfo(response.Message);
                            }
                        },
                        failure: function (msg) {
                            $.jstree.rollback(data.rlbk);
                        },
                        error: function (xhr, status, error) {
                            if (Utils.TerminoSesion(xhr)) {
                                alert(request.responseText);
                            }
                        }
                    });
                }
            })
            .on("loaded.jstree", function (e, data) {
                $(this).jstree("open_all");
                setTimeout(function () { ocultarPrimerListado("divEstructuraMenu"); }, 20);
            })
            .on("select_node.jstree", function (evt, data) {
                var idSelect = data.node.id;
                if ($("#IdJerarquia").val() != idSelect) {
                    $("#IdJerarquia").val("");
                    $("#divInformacion").hide();
                    $("#divCargarResponsables").show();
                }
                if (idSelect == -1) {
                    $("#divCargarResponsables").hide();
                }
            })
            .on("move_node.jstree", function (evt, data) {
                console.log(evt);
                console.log(data);

                $.ajax({
                    url: urlMoverJerarquia,
                    type: 'POST',
                    data: {
                        "jerarquiaId": data.node.id,
                        "nuevoParentId": data.parent
                    },
                    success: function (response) {
                        if (Utils.TerminoSesion(response)) {
                            data.instance.refresh();
                            if (response.Success) {
                                $("#Descripcion").val(data.text);
                            }

                            data.instance.refresh();
                            window.messageInfo(response.Message);
                        }
                    },
                    failure: function (msg) {
                        $.jstree.rollback(data.rlbk);
                    },
                    error: function (xhr, status, error) {
                        if (Utils.TerminoSesion(xhr)) {
                            alert(request.responseText);
                        }
                    }
                });
            });
        
    // 7 bind to events triggered on the tree
    $('#tree1').on("changed.jstree", function (e, data) {
        alert(data.selected);
    });

    $('body').on('click', 'a.verPermiso', function () {
        var aPos = dataTableRol.fnGetPosition(this.parentNode.parentNode);
        var aData = dataTableRol.fnGetData(aPos[0]);
        var rowID = aData.Id;

        nombreRol = aData.Nombre;
        delRowPos = aPos[0];
        delRowID = rowID;

        
        checkSession(function () {
            AsignarRol();
        });
    });
});

function VisualizarDataTableRol() {
    dataTableRol = $('#RolDataTable').dataTable({
        "bFilter": false,
        "bProcessing": true,
        "serverSide": true,
        //"scrollY": "350px",              
        "ajax": {
            "url": urlListar,
            "type": "POST",
            "data": function (request) {
               
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
            { "data": "Nombre" },
            
            {
                "data": function (obj) {
                    return '<div class="action-buttons">\
                       <a class="blue verPermiso" href="javascript:void(0)"><i class="ace-icon fa fa-eye bigger-130"></i></a>\
                    </div>';
                }
            }
        ],
        "aoColumnDefs": [
            { "className": "hidden-100", "aTargets": [0], "width": "20%" },
            { "className": "hidden-50", "aTargets": [1], "width": "10%" },
            { "bSortable": false, "sClass": "center", "aTargets": [2], "width": "10%" },
        ],
        "order": [[1, "desc"]],
        "initComplete": function (settings, json) {
           // AddSearchFilter();
        },
        "fnDrawCallback": function (oSettings) {

        }
    });
}

 function ocultarPrimerListado (id) {
    var r = $("#" + id + " li").length;
    if (r == 1) {
        $("#" + id + " li[id=-1] > ins[class='jstree-icon']").hide();
    }
};


function AsignarRol() {
    $("#idrol").text(nombreRol);
}

