var urlMantenimiento = baseUrl + 'Acceso/';
var dataTableRol = null;
var delRowID = 0;
var delRowPos = null;
var urlListar = baseUrl + 'Acceso/Listar';
var nombreRol = null;
var urlListarModulo = baseUrl + 'Acceso/GetTreeData';
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

        $('#mainTree').jstree({
            "json_data": {
                "ajax": {
                    "url": urlListarModulo,
                    "type": "POST",
                    "dataType": "json",
                    "contentType": "application/json charset=utf-8",
                    "data": function (n) {
                        return { id: n.attr ? n.attr("id") : 0 };
                    }
                }
            },
            "themes": {
                "theme": "default",
                "dots": false,
                "icons": true
                //"url": '@Url.Content("~/Content/treeView/default/style.css")'
            },
            "contextmenu": {
                "items": {
                    "create": false,
                    "rename": false,
                    "remove": false,
                    "ccp": false,
                }
            },
            "ui": { "initially_select": ["0"] },
            "plugins": ["themes", "json_data", "ui", "crrm"]
        });

    
    //$('#tree1').jstree({

    //    'core': {
    //        'data': {
    //            "url": urlListarModulo,
    //            "type":'POST',
    //            "data": function (node) {
    //                return { "id": node.id };
    //            },
    //            "success": function (response) {
    //                data = [];
    //                var _this = this;
    //                for (opnum in response) {
    //                    var op = response[opnum]
    //                    console.log(op);
    //                    //node = {
    //                    //    "data": op.info,
    //                    //    "metadata": op,
    //                    //    "state": "closed"
    //                    //}
    //                    //data.push(node);
    //                }
    //                return data;;

    //            }
             
    //        }
    //    }
    //});
   
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

function AsignarRol() {
    
    $("#idrol").text(nombreRol);
}

