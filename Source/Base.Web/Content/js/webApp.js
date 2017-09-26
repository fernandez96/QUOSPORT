/*
 *  Document   : webApp.js
 *  Description: Custom scripts and plugin initializations
 */

var webApp = function () {
	
    var _popupEspera = "popupEspera";
    var _popupMensaje = "popupMensaje";
    var _popupConfirmacion = "popupConfirmacion";
    var _popupReConfirmacion = "popupReConfirmacion";
    var _popupEliminacionMensaje = "popupEliminar";
    var _mensajeEspera = "";
    var _tituloPopupMensaje = "";
    var _tituloPopupComfirmacion = "";
    var _tituloEliminacionPopupMensaje = "";
    var _mensajePopupConfirmacion = "";
    var _mensajePopupEliminacionConfirmacion = "";
    var _btnCancelar = "";
    var _btnAceptar = "";
    var _formatoFecha = "";  
    
    var createMessageDialog = function () {
        var dialogMessage = "<div id='" + _popupMensaje + "' tabindex='-1' role='dialog' aria-hidden='true' class='modal fade' data-backdrop='static' style='z-index:100000;'>";
        dialogMessage += "<div class='modal-dialog'>";
        dialogMessage += "<div class='modal-content'>";
        dialogMessage += "<div class='modal-header'>";
        dialogMessage += '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>';
        dialogMessage += "<h4 class='modal-title'>" + _tituloPopupMensaje + "</h4>";
        dialogMessage += "</div>";
        dialogMessage += "<div class='modal-body paddingTop15'></div>";
        dialogMessage += "<div class='modal-footer' style='margin-top: 0px; margin-bottom: 0px;'>";
        dialogMessage += "<button class='btn btn-primary btn-aceptar btn-sm' data-dismiss='modal'><i class='fa fa-thumbs-o-up'></i> " + _btnAceptar + "</button>";
        dialogMessage += "</div>";
        dialogMessage += "</div>";
        dialogMessage += "</div>";

        $("body").append(dialogMessage);
    };
    
    var createConfirmDialog = function () {
        var dialogConfirm = "<div id='" + _popupConfirmacion + "' tabindex='-1' role='dialog' aria-hidden='true' class='modal fade' data-backdrop='static' style='z-index:100000;'>";
        dialogConfirm += "<div class='modal-dialog'>";
        dialogConfirm += "<div class='modal-content'>";
        dialogConfirm += "<div class='modal-header'><h4 class='modal-title'>" + _tituloPopupComfirmacion + "</h4></div>";
        dialogConfirm += "<div class='modal-body paddingTop15'><p></p></div>";
        dialogConfirm += "<div class='modal-footer' style='margin-top: 0px; margin-bottom: 0px;'>";
        dialogConfirm += "<button class='btn btn-danger btn-sm' data-dismiss='modal'><i class='fa fa-remove'></i> " + _btnCancelar + "</button> ";
        dialogConfirm += "<button class='btn btn-primary btn-sm' data-dismiss='modal'><i class='fa fa-thumbs-o-up'></i> " + _btnAceptar + "</button>";
        dialogConfirm += "</div>";
        dialogConfirm += "</div>";
        dialogConfirm += "</div>";

        $("body").append(dialogConfirm);
    };

    var createReConfirmDialog = function () {
        var dialogConfirm = "<div id='" + _popupReConfirmacion + "' tabindex='-1' role='dialog' aria-hidden='true' class='modal fade' data-backdrop='static' style='z-index:100000;'>";
        dialogConfirm += "<div class='modal-dialog'>";
        dialogConfirm += "<div class='modal-content'>";
        dialogConfirm += "<div class='modal-header'><h4 class='modal-title'>" + _tituloPopupComfirmacion + "</h4></div>";
        dialogConfirm += "<div class='modal-body paddingTop15'><p></p></div>";
        dialogConfirm += "<div class='modal-footer' style='margin-top: 0px; margin-bottom: 0px;'>";
        dialogConfirm += "<button class='btn btn-danger btn-sm' data-dismiss='modal'><i class='fa fa-remove'></i> " + _btnCancelar + "</button> ";
        dialogConfirm += "<button class='btn btn-primary btn-sm' data-dismiss='modal'><i class='fa fa-thumbs-o-up'></i> " + _btnAceptar + "</button>";
        dialogConfirm += "</div>";
        dialogConfirm += "</div>";
        dialogConfirm += "</div>";

        $("body").append(dialogConfirm);
    };

    var createMessageDeleteDialog = function () {

        var dialogMessage = "<div id='" + _popupEliminacionMensaje + "' class='modal fade' tabindex='-1' role='dialog' aria-labelledby='lblTituloConfirmarEliminar' aria-hidden='true' class='modal fade' data-backdrop='static' style='z-index:100000;'>";
        dialogMessage += "<div class='modal-dialog'>";
        dialogMessage += "<div class='modal-content'>";
        dialogMessage += "<div class='modal-header'>";
        dialogMessage += '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>';
        dialogMessage += "<h4 id='lblTituloConfirmarEliminar' class='modal-title'>" + _tituloEliminacionPopupMensaje + "</h4>";
        dialogMessage += "</div>";
        dialogMessage += "<div class='modal-body paddingTop15'><p></p></div>";
        dialogMessage += "<div class='modal-footer' style='margin-top: 0px; margin-bottom: 0px;'>";
        dialogMessage += "<button class='btn btn-danger btn-sm' data-dismiss='modal'><i class='fa fa-remove'></i> " + _btnCancelar + "</button> ";
        dialogMessage += "<button class='btn btn-primary btn-sm' data-dismiss='modal'><i class='fa fa-thumbs-o-up'></i> " + _btnAceptar + "</button>";
        dialogMessage += "</div>";
        dialogMessage += "</div>";
        dialogMessage += "</div>";

        $("body").append(dialogMessage);
    };         
    
    var getDataForm = function (form) {
        var that = $(form);
        var url = that.attr('action');
        var type = that.attr('method');
        var data = {};

        var namex = "";
        that.find('[name]').each(function (index, value) {
            var that = $(this);
            var name = that.attr('name');
            var value = that.val();

            if (that.attr('type') === 'radio') {
                if (that.is(':checked')) {
                    data[name] = value;
                }
            } else if (that.attr('type') === 'checkbox') {
                if (that.is(':checked') && namex != name) {
                    data[name] = value;
                    namex = name;
                } else if (namex == name) {
                    namex = "";
                } 
            }
            else if (namex == name && that.attr('type') === 'hidden') {
                namex = "";
            }
            else {
                data[name] = value;
            }
        });

        var obj = {
            url: url,
            type: type,
            data: data
        };

        return obj;
    };    
    
    var formValidBootstrap = function () {
        //$('form').validateBootstrap(true);
    }; 
    
    var createLoading = function () {
        var urlImgLoading = location.protocol + "//" + location.host + "EmailingSite/Content/images/loading.gif";
        $("body").append('<div id="' + _popupEspera + '" tabindex="-1" role="dialog" aria-hidden="true" class="modal fade" data-backdrop="static" style="z-index:100000;"><div class="modal-dialog"><div class="modal-content"><div class="modal-body"><h4 class="text-center"> ' + _mensajeEspera + '</h4></div></div></div></div>');
    };    
	
	var createGrid = function(opciones){
        var grid = jQuery(opciones.grid_selector);
        var estadoSubGrid = false;
		
        if (opciones.caption == null)
            opciones.caption = "";		
		
        if (opciones.sortname == null)
            opciones.sortname = 'Id';
      
        if (opciones.sortorder == null)
            opciones.sortorder = 'desc';

        if (opciones.subGrid != null)
            estadoSubGrid = true;

        if (opciones.rules == null)
            opciones.rules = false;

        if (opciones.viewrecords == null)
            opciones.viewrecords = true;

        if (opciones.rownumbers == null)
            opciones.rownumbers = true;
    
        if (opciones.rowNum == null)
            opciones.rowNum = 15;

        if (opciones.rowList == null)
            opciones.rowList = [opciones.rowNum, 20, 50, 100, 150];

        if (opciones.altRows == null)
            opciones.altRows = true;

        if (opciones.height == null)
            opciones.height = 'auto';

        if (opciones.width == null)
            opciones.width = 'auto';

        if (opciones.noregistro == null)
            opciones.noregistro = false;
        
        if (opciones.rules == null)
            opciones.rules = false;  

        if (opciones.search == null)
            opciones.search = false;

        if (opciones.footerrow == null)
            opciones.footerrow = false;

        if (opciones.multiselect == null)
            opciones.multiselect = false;
        
        if (opciones.agregarBotones == null)
            opciones.agregarBotones = false; 

        if (opciones.autowidth == null)
            opciones.autowidth = false;                 

        if (opciones.GridLocal == null) {
            opciones.GridLocal = false;

            if (opciones.CellEdit == null)
                opciones.CellEdit = false;
        }                  
        
        if (opciones.AlertTitle == null)
            opciones.AlertTitle = "Alerta";
        
        if (opciones.SinRegistro == null)
            opciones.SinRegistro = "Sin registros"; 

        if (opciones.NuevoCaption == null)
            opciones.NuevoCaption = "";
        
        if (opciones.NuevoTitle == null)
            opciones.NuevoTitle = "Nuevo";        

        if (opciones.EditarCaption == null)            
            opciones.EditarCaption = "";
        
        if (opciones.EditarTitle == null)            
            opciones.EditarTitle = "Editar";        

        if (opciones.EliminarCaption == null)
            opciones.EliminarCaption = "";
        
        if (opciones.EliminarTitle == null)
            opciones.EliminarTitle = "Eliminar";        

        if (opciones.Lenguaje == null)
            opciones.Lenguaje = "es-PE";        

        if (opciones.dialogDelete == null)
            opciones.dialogDelete = 'dialog-delete';

        if (opciones.dialogAlert == null)
            opciones.dialogAlert = 'dialog-alert';

        if (opciones.seleccioneRegistro == null)
            opciones.seleccioneRegistro = 'Por favor seleccione un registro';        

        if (opciones.CustomButtons == null)
            opciones.CustomButtons = {};

        if (opciones.canEventSameRow == null)
            opciones.canEventSameRow = true;

        if (opciones.pgbuttons == null)
            opciones.pgbuttons = true;

        if (opciones.pginput == null)
            opciones.pginput == true;        

        if (opciones.refresh == null)
            opciones.refresh == false;  

        if (opciones.treeGrid == null)
            opciones.treeGrid = false;

        if (opciones.grouping == null)
            opciones.grouping = false;

        if (opciones.async == null)
            opciones.async = true;

        if (opciones.groupingViewOptions == null)
            opciones.groupingViewOptions = new Object();        

        var rowKey;
        var lastRowKey;

        if (!opciones.GridLocal) {
            
            var settingsGrid = {
                prmNames: {
                    search: 'isSearch',
                    nd: null,
                    rows: 'rows',
                    page: 'page',
                    sort: 'sortField',
                    order: 'sortOrder',
                    filters: 'filters'
                },

                postData: { searchString: '', searchField: '', searchOper: '', filters: '' },
                jsonReader: {
                    root: 'rows',
                    page: 'page',
                    total: 'total',
                    records: 'records',
                    cell: 'cell',
                    id: 'id', //index of the column with the PK in it
                    userdata: 'userdata',
                    repeatitems: true
                },
                pgbuttons: opciones.pgbuttons,
                pginput: opciones.pginput,
                rowNum: opciones.rowNum,
                rowList: opciones.rowList,
                pager: opciones.pager_selector,
                sortname: opciones.sortname,
                viewrecords: opciones.viewrecords,
                shrinkToFit: opciones.shrinkToFit != null ? opciones.shrinkToFit : true,
                multiselect: opciones.multiselect,
                rownumbers: opciones.rownumbers,
                sortorder: opciones.sortorder,
                height: opciones.height,
                footerrow: opciones.footerrow,
                width: opciones.width,
                autowidth: opciones.autowidth,
                colNames: opciones.colNames,
                colModel: opciones.colModel,
                caption: opciones.caption,
                subGrid: estadoSubGrid,
                grouping: opciones.grouping,
                groupingView: {
                    groupField: opciones.groupingViewOptions.groupField == null ? [] : opciones.groupingViewOptions.groupField,
                    groupColumnShow: opciones.groupingViewOptions.groupColumnShow == null ? [] : opciones.groupingViewOptions.groupColumnShow,
                    groupText: opciones.groupingViewOptions.groupText == null ? [] : opciones.groupingViewOptions.groupText,
                    groupCollapse: opciones.groupingViewOptions.groupCollapse == null ? false : opciones.groupingViewOptions.groupCollapse,
                    groupOrder: opciones.groupingViewOptions.groupOrder == null ? [] : opciones.groupingViewOptions.groupOrder,
                    groupSummary: opciones.groupingViewOptions.groupSummary == null ? [] : opciones.groupingViewOptions.groupSummary,
                    hideFirstGroupCol: opciones.groupingViewOptions.hideFirstGroupCol == null ? true : opciones.groupingViewOptions.hideFirstGroupCol,
                    groupSummaryPos: opciones.groupingViewOptions.groupSummaryPos == null ? [] : opciones.groupingViewOptions.groupSummaryPos,
                },
                editurl: opciones.EditingOptions ? opciones.EditingOptions.editUrl : '',
                ajaxSelectOptions: { type: "POST", contentType: 'application/json; charset=utf-8', dataType: 'json', },
                subGridRowColapsed: function (subgrid_id, row_id) {
                    var subgrid_table_id, pager_id;
                    subgrid_table_id = subgrid_id + "_t";
                    pager_id = "p_" + subgrid_table_id;
                    jQuery("#" + subgrid_table_id).remove();
                    jQuery("#" + pager_id).remove();
                },
                subGridRowExpanded: function (subgrid_id, row_id) {
                    var subGrid = opciones.subGrid;

                    var subgrid_table_id, pager_id;
                    subgrid_table_id = subgrid_id + "_t";
                    pager_id = "p_" + subgrid_table_id;

                    $("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table><div id='" + pager_id + "' class='scroll'></div>");

                    var parameters = { Id: row_id };
                    $.ajax({
                        type: "POST",
                        url: subGrid.Url,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(parameters),
                        success: function (rsp) {
                            var data = (typeof rsp.d) == 'string' ? eval('(' + rsp.d + ')') : rsp.d;
                            $("#" + subgrid_table_id).jqGrid({
                                datatype: "local",
                                colNames: subGrid.ColNames,
                                colModel: subGrid.ColModels,
                                rowNum: 10,
                                rowList: [10, 20, 50, 100],
                                sortorder: "desc",
                                viewrecords: true,
                                rownumbers: true,
                                pager: "#" + pager_id,
                                loadonce: true,
                                sortable: true,
                                height: subGrid.Height,
                                width: subGrid.Width
                            });

                            for (var i = 0; i <= data.length; i++)
                                jQuery("#" + subgrid_table_id).jqGrid('addRowData', i + 1, data[i]);

                            $("#" + subgrid_table_id).trigger("reloadGrid");
                        },
                        failure: function (msg) {
                            $('#mensajeFalla').show().fadeOut(8000);
                        }
                    });
                },

                ondblClickRow: function (rowid) {
                    if (opciones.search) {
                        var ret = grid.getRowData(rowid);
                        SelectRow(ret);
                    }

                    if (opciones.EditingOptions != null && opciones.EditingOptions.canEditRowInline) {
                        if (opciones.BeforeEditHandler != null && typeof (opciones.BeforeEditHandler) == "function")
                            opciones.BeforeEditHandler(grid, rowKey);

                        var editparameters = {
                            "keys": true,
                            "oneditfunc": null,
                            "successfunc": opciones.SaveRowInline ? opciones.SaveRowInline : null,
                            "url": opciones.EditingOptions ? opciones.EditingOptions.editUrl : null,
                            "extraparam": {},
                            "aftersavefunc": null,
                            "errorfunc": null,
                            "afterrestorefunc": opciones.AfterSaveRowInline ? opciones.AfterSaveRowInline : null,
                            "restoreAfterError": true,
                            "mtype": "POST"
                        }

                        grid.jqGrid("editRow", rowKey, editparameters);

                        lastRowKey = rowKey;
                    }

                    if (opciones.DblClickHandler != null && typeof (opciones.DblClickHandler) == 'function') {
                        opciones.DblClickHandler(rowid);
                    }
                },
                onSelectRow: function (id) {
                    rowKey = grid.getGridParam('selrow');

                    if (rowKey != null) {
                        $("#" + opciones.identificador).val(rowKey);
                    }

                    if (opciones.EditingOptions != null && opciones.EditingOptions.canEditRowInline) {
                        if (lastRowKey !== rowKey) {
                            if (lastRowKey != "undefined") {
                                if ($("tr#" + lastRowKey).attr("editable") === "1") {
                                    grid.jqGrid('restoreRow', lastRowKey);
                                }
                            }
                        }
                    }

                    if (opciones.OnSelectRow != null && typeof (opciones.OnSelectRow) == 'function') {
                        if (opciones.canEventSameRow || (lastRowKey !== rowKey))
                            if (opciones.multiselect == false)
                                opciones.OnSelectRow(id);
                            else
                                opciones.OnSelectRow(id, opciones.nameArraySelected);
                    }
                    lastRowKey = rowKey;
                },
                onSelectAll: opciones.OnSelectAll,
                gridComplete: function () {
                    if (grid.getGridParam('records') == 0) {
                        if (opciones.noregistro == true) {
                        	webApp.showMessageDialog(opciones.SinRegistro);
                        }
                    }

                    rowKey = null;
                    if (opciones.agregarBotones == true) {
                        AgregarBotones();
                    }

                    if (opciones.GridCompleteHandler != null && typeof (opciones.GridCompleteHandler) == "function")
                        opciones.GridCompleteHandler(grid);
                },
                loadComplete: function () {
                    if (opciones.LoadCompleteHandler != null && typeof (opciones.LoadCompleteHandler) == "function")
                        opciones.LoadCompleteHandler(this);
                },                
                datatype : function (postdata) {
                    
                    var migrilla = new Object();
                    migrilla.page = postdata.page;
                    migrilla.rows = postdata.rows;
                    migrilla.sidx = postdata.sortField;
                    migrilla.sord = postdata.sortOrder;
                    migrilla._search = postdata.isSearch;
                    migrilla.filters = postdata.filters;
                    
                    if (opciones.rules != false) {

                        if (opciones.GetRulesMethod == null)
                            opciones.GetRulesMethod = GetRules;
                        var parametroExtra = null;
                        var customRules = opciones.GetRulesMethod();

                        if (migrilla.filters != "") {

                            parametroExtra = JSON.parse(migrilla.filters);

                            if (customRules != null) {
                                parametroExtra.groups = new Array();

                                if (opciones.AdvancedSearch == true) {
                                    parametroExtra.groups.push(customRules);
                                }
                                else {
                                    var nuevoSubGrupo = { groupOp: 'AND', rules: {} };
                                    nuevoSubGrupo.rules = customRules;

                                    parametroExtra.groups.push(nuevoSubGrupo);
                                }
                            }
                        }
                        else {
                            if (opciones.AdvancedSearch == true && customRules != null)
                                parametroExtra = customRules;
                            else if (customRules != null && customRules.length > 0) {
                                parametroExtra = { groupOp: 'AND', rules: {} };
                                parametroExtra.rules = customRules;
                            }
                        }

                        migrilla.filters = parametroExtra == null ? "" : parametroExtra;
                    }

                    if (migrilla._search == true) {
                        migrilla.searchField = postdata.searchField;
                        migrilla.searchOper = postdata.searchOper;
                        migrilla.searchString = postdata.searchString;
                    }
                    
                    $.ajax({
                        url: opciones.urlListar,
                        type: 'post',
                        data: { grid: migrilla },
                        success: function (data, st) {
                            if (st == 'success') {
                                if (opciones.pivotGridOptions == null) {
                                    
                                    var jq = grid[0];
                                    jq.addJSONData(data);
                                } else {
                                    
                                    if (opciones.pivotGridOptions.colTotals)
                                        settingsGrid.footerrow = true;

                                    grid.jqGrid('jqPivot', data, {
                                    //grid.jqGrid('jqPivot', opciones.pivotGridOptions.urlData+"2?grid=" + gridPivot, {
                                        xDimension: opciones.pivotGridOptions.xDimensionColumns,
                                        yDimension: opciones.pivotGridOptions.yDimensionColumns,
                                        aggregates: opciones.pivotGridOptions.aggregateColumns,
                                        groupSummaryPos: opciones.pivotGridOptions.groupSummaryPos == null ? 'header' : opciones.pivotGridOptions.groupSummaryPos,
                                        colTotals: opciones.pivotGridOptions.colTotals == null ? false : opciones.pivotGridOptions.colTotals,
                                        frozenStaticCols: opciones.pivotGridOptions.frozenStaticCols == null ? false : opciones.pivotGridOptions.frozenStaticCols,
                                        groupSummary: opciones.pivotGridOptions.groupSummary == null ? true : opciones.pivotGridOptions.groupSummary,
                                        rowTotals: opciones.pivotGridOptions.rowTotals == null ? false : opciones.pivotGridOptions.rowTotals,
                                        rowTotalsText: opciones.pivotGridOptions.rowTotalsText == null ? "Total" : opciones.pivotGridOptions.rowTotalsText
                                    }, settingsGrid, opciones.pivotGridOptions.ajaxOptions);
                                }
                            }

                        },
                        error: function (a, b, c) {
                            alert('Error with AJAX callback:' + a.responseText);
                        }
                    });
                }
            };

            grid.jqGrid(settingsGrid);
        }
        else if (opciones.GridLocal) {

            var settingsGrid = {
                colNames: opciones.colNames,
                colModel: opciones.colModel,
                sortorder: opciones.sortorder,
                pgbuttons: opciones.pgbuttons,
                pginput: opciones.pginput,
                rowNum: opciones.rowNum,
                rowList: opciones.rowList,
                rownumbers: opciones.rownumbers,
                cellEdit: opciones.CellEdit,
                cellsubmit: "clientArray",
                pager: opciones.pager_selector,
                sortname: opciones.sortname,
                viewrecords: opciones.viewrecords,
                multiselect: opciones.multiselect,
                sortorder: opciones.sortorder,
                footerrow: opciones.footerrow,
                height: height,
                width: width,
                gridview: true,
                autowidth: false,
                caption: opciones.caption,
                subGrid: estadoSubGrid,
                editurl: opciones.editurl,
                grouping: opciones.grouping,
                groupingView: {
                    groupField: opciones.groupingViewOptions.groupField == null ? [] : opciones.groupingViewOptions.groupField,
                    groupColumnShow: opciones.groupingViewOptions.groupColumnShow == null ? [] : opciones.groupingViewOptions.groupColumnShow,
                    groupText: opciones.groupingViewOptions.groupText == null ? [] : opciones.groupingViewOptions.groupText,
                    groupCollapse: opciones.groupingViewOptions.groupCollapse == null ? false : opciones.groupingViewOptions.groupCollapse,
                    groupOrder: opciones.groupingViewOptions.groupOrder == null ? [] : opciones.groupingViewOptions.groupOrder,
                    groupSummary: opciones.groupingViewOptions.groupSummary == null ? [] : opciones.groupingViewOptions.groupSummary,
                    hideFirstGroupCol: opciones.groupingViewOptions.hideFirstGroupCol == null ? true : opciones.groupingViewOptions.hideFirstGroupCol,
                    groupSummaryPos: opciones.groupingViewOptions.groupSummaryPos == null ? [] : opciones.groupingViewOptions.groupSummaryPos,
                },
                treeGrid: opciones.treeGrid,
                gridComplete: function () {
                    if (opciones.GridCompleteHandler != null && typeof (opciones.GridCompleteHandler) == "function")
                        opciones.GridCompleteHandler();
                },
                loadComplete: function () {
                    if (opciones.LoadCompleteHandler != null && typeof (opciones.LoadCompleteHandler) == "function")
                        opciones.LoadCompleteHandler(this);
                },
                afterSaveCell: function (rowid, cellname, value, iRow, iCol) {
                    if (opciones.AfterSaveCellHandler != null)
                        opciones.AfterSaveCellHandler(rowid, cellname, value, iRow, iCol);
                },
                onSelectRow: function () {
                    rowKey = grid.getGridParam('selrow');

                    if (rowKey != null) {
                        $("#" + opciones.identificador).val(rowKey);
                    }
                    if (opciones.selectRowFunc != null) {
                        if (typeof (opciones.selectRowFunc) == 'function') {
                            opciones.selectRowFunc(rowKey)
                        }
                    }
                },
                ondblClickRow: function (rowid) {
                    if (opciones.search) {
                        var ret = grid.getRowData(rowid);
                        SelectRow(ret);
                    }

                    if (opciones.DblClickHandler != null && typeof (opciones.DblClickHandler) == 'function') {
                        opciones.DblClickHandler(rowid);
                    }
                },
                beforeEditCell: function (rowid, cellname, value, iRow, iCol) {
                    if (opciones.BeforeEditCellHandler != null)
                        opciones.BeforeEditCellHandler(rowid, cellname, value, iRow, iCol);
                },
                afterEditCell: function (rowid, cellname, value, iRow, iCol) {
                    if (opciones.AfterEditCellHandler != null)
                        opciones.AfterEditCellHandler(rowid, cellname, value, iRow, iCol);
                },
                rowattr: function (rowId) {
                    if (opciones.RowAttrHandler != null)
                        opciones.RowAttrHandler(rowId);
                }
            };

            if (opciones.pivotGridOptions == null) {
                settingsGrid.datatype = "local";

                grid.jqGrid(settingsGrid);
            } else {
                if (opciones.pivotGridOptions.colTotals)
                    settingsGrid.footerrow = true;

                grid.jqGrid('jqPivot', opciones.pivotGridOptions.urlData, {
                    xDimension: opciones.pivotGridOptions.xDimensionColumns,
                    yDimension: opciones.pivotGridOptions.yDimensionColumns,
                    aggregates: opciones.pivotGridOptions.aggregateColumns,
                    groupSummaryPos: opciones.pivotGridOptions.groupSummaryPos == null ? 'header' : opciones.pivotGridOptions.groupSummaryPos,
                    colTotals: opciones.pivotGridOptions.colTotals == null ? false : opciones.pivotGridOptions.colTotals,
                    frozenStaticCols: opciones.pivotGridOptions.frozenStaticCols == null ? false : opciones.pivotGridOptions.frozenStaticCols,
                    groupSummary: opciones.pivotGridOptions.groupSummary == null ? true : opciones.pivotGridOptions.groupSummary,
                    rowTotals: opciones.pivotGridOptions.rowTotals == null ? false : opciones.pivotGridOptions.rowTotals,
                    rowTotalsText: opciones.pivotGridOptions.rowTotalsText == null ? "Total" : opciones.pivotGridOptions.rowTotalsText
                }, settingsGrid, opciones.pivotGridOptions.ajaxOptions);
            }
        }        

		grid.jqGrid('navGrid', opciones.pager_selector, {
            edit: false,
            add: false,
            del: false,
            search: false,
            refresh: false,
        },
        {}, // use default settings for edit
        {}, // use default settings for add
        {}, // delete instead that del:false we need this
        {
            multipleSearch: true,
            beforeShowSearch: function () {
                return true;
            }
        });

        if (opciones.nuevo) {
            $(opciones.grid_selector).navButtonAdd(opciones.pager_selector, {
                caption: opciones.NuevoCaption,
                title: opciones.NuevoTitle,
                buttonicon: 'ace-icon fa fa-plus-circle purple',
                position: 'first',
                onClickButton: function () {
                    if (opciones.NuevoCommand != null && typeof (opciones.NuevoCommand) == "function")
                        opciones.NuevoCommand();
                    else
                        Nuevo();
                }
            });
        }

        if (opciones.editar) {
            $(opciones.grid_selector).navButtonAdd(opciones.pager_selector, {
                caption: opciones.EditarCaption,
                title: opciones.EditarTitle,
                buttonicon: 'ace-icon fa fa-pencil blue',
                position: 'second',
                onClickButton: function () {
                    if (rowKey != null) {
                        if (opciones.EditarCommand != null && typeof (opciones.EditarCommand) == "function")
                            opciones.EditarCommand(rowKey);
                        else
                            Editar(rowKey);
                    } else {
                    	webApp.showMessageDialog(opciones.seleccioneRegistro);
                    }
                }
            });
        }

        if (opciones.eliminar) {
            $(opciones.grid_selector).navButtonAdd(opciones.pager_selector, {
                caption: opciones.EliminarCaption,
                title: opciones.EliminarTitle,
                buttonicon: 'ace-icon fa fa-trash-o red',
                position: 'thrid',
                onClickButton: function () {
                    if (rowKey != null) {
                        if (opciones.EliminarCommand != null && typeof (opciones.EliminarCommand) == "function")
                            opciones.EliminarCommand(rowKey);
                        else
                            Eliminar(rowKey);

                        //$("#" + opciones.dialogDelete).dialog("open");
                    } else {
                    	webApp.showMessageDialog(opciones.seleccioneRegistro);
                    }
                }
            });
        }

        if (opciones.CustomButtons) {
            $.each(opciones.CustomButtons, function (index, botonNuevo) {
                $(opciones.grid_selector).navButtonAdd(opciones.pager_selector, {
                    caption: botonNuevo.Caption,
                    title: botonNuevo.Title,
                    buttonicon: botonNuevo.ButtonIcon ? botonNuevo.ButtonIcon : 'ace-icon fa fa-search-plus grey',
                    position: botonNuevo.Position ? botonNuevo.Position : 'fourth',
                    onClickButton: function () {
                        if (botonNuevo.ClickFunction != null && typeof (botonNuevo.ClickFunction) == "function")
                            botonNuevo.ClickFunction();
                    }
                });
            });
        }        

	};
	
    var exportarExcel = function (urlExportar) {
        webApp.showLoading();

        var iframe_ = document.createElement("iframe");
        iframe_.style.display = "none";
        iframe_.setAttribute("src", urlExportar);
        iframe_.frameBorder = 0;

        if (navigator.userAgent.indexOf("MSIE") > -1 && !window.opera) {
            // Si es Internet Explorer
            iframe_.onreadystatechange = function () {
                switch (this.readyState) {
                    case "loading":
                        webApp.showLoading();
                        break;
                    case "complete":
                    case "interactive":
                    case "uninitialized":
                        webApp.hideLoading();
                        getCookie("DescargaCompleta");
                        break;
                    default:
                        webApp.hideLoading();
                        delCookie("DescargaCompleta");
                        break;
                }
            };
        } else {
            // Si es Firefox o Chrome
            document.body.appendChild(iframe_);

            var _timer = setInterval(function () {
                var success = getCookie("DescargaCompleta");
                if (success === "1") {
                    clearInterval(_timer);
                    webApp.hideLoading();
                    delCookie("DescargaCompleta");
                }
            }, 1000);
            return;
        }
        document.body.appendChild(iframe_);
    };

    var disableAllFormElements = function (formId) {
        $('#' + formId).find('input, textarea, button, select').attr('disabled', 'disabled');
    };

    var mayuscula = function (e, elemento) {
        elemento.value = elemento.value.toUpperCase();
    };

    var inicializarFileUpload = function(id) {
        var itemTemplate = '<div id="'+id+'_SWFUpload_0_0" class="uploadify-queue-item">\
            <span class="ace-file-name" ><i class=" ace-icon fa fa-upload"></i> Sin Archivo ...</span>\
            <div class="uploadify-progress">\
                <div class="uploadify-progress-bar"><!--Progress Bar--></div>\
            </div>\
        </div>';
        $('#'+id+'-queue').html('');
        $('#'+id+'-queue').append(itemTemplate);         
    }

    var sumaFecha = function (d, fecha) {
        var Fecha = new Date();
        var sFecha = fecha || (Fecha.getDate() + "-" + (Fecha.getMonth() + 1) + "-" + Fecha.getFullYear());
        var sep = sFecha.indexOf('/') != -1 ? '/' : '-';
        var aFecha = sFecha.split(sep);
        var fecha = aFecha[2] + '/' + aFecha[1] + '/' + aFecha[0];
        fecha = new Date(fecha);
        fecha.setDate(fecha.getDate() + parseInt(d));
        var anno = fecha.getFullYear();
        var mes = fecha.getMonth() + 1;
        var dia = fecha.getDate();
        mes = (mes < 10) ? ("0" + mes) : mes;
        dia = (dia < 10) ? ("0" + dia) : dia;
        var fechaFinal = dia + sep + mes + sep + anno;
        return (fechaFinal);
    };

    var number_format = function (number, decimals, dec_point, thousands_sep) {
        // *     example 1: number_format(1234.56);
        // *     returns 1: '1,235'
        // *     example 2: number_format(1234.56, 2, ',', ' ');
        // *     returns 2: '1 234,56'
        // *     example 3: number_format(1234.5678, 2, '.', '');
        // *     returns 3: '1234.57'
        // *     example 4: number_format(67, 2, ',', '.');
        // *     returns 4: '67,00'
        // *     example 5: number_format(1000);
        // *     returns 5: '1,000'
        // *     example 6: number_format(67.311, 2);
        // *     returns 6: '67.31'
        // *     example 7: number_format(1000.55, 1);
        // *     returns 7: '1,000.6'
        // *     example 8: number_format(67000, 5, ',', '.');
        // *     returns 8: '67.000,00000'
        // *     example 9: number_format(0.9, 0);
        // *     returns 9: '1'
        // *    example 10: number_format('1.20', 2);
        // *    returns 10: '1.20'
        // *    example 11: number_format('1.20', 4);
        // *    returns 11: '1.2000'
        // *    example 12: number_format('1.2000', 3);
        // *    returns 12: '1.200'
        var n = !isFinite(+number) ? 0 : +number,
            prec = !isFinite(+decimals) ? 0 : Math.abs(decimals),
            sep = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep,
            dec = (typeof dec_point === 'undefined') ? '.' : dec_point,
            toFixedFix = function (n, prec) {
                // Fix for IE parseFloat(0.55).toFixed(0) = 0;
                var k = Math.pow(10, prec);
                return Math.round(n * k) / k;
            },
            s = (prec ? toFixedFix(n, prec) : Math.round(n)).toString().split('.');
        if (s[0].length > 3) {
            s[0] = s[0].replace(/\B(?=(?:\d{3})+(?!\d))/g, sep);
        }
        if ((s[1] || '').length < prec) {
            s[1] = s[1] || '';
            s[1] += new Array(prec - s[1].length + 1).join('0');
        }
        return s.join(dec);
    };	

    var validarDecimal = function(evt) {
        var keyPressed = (evt.which) ? evt.which : event.keyCode
        return !((keyPressed !=13) && (keyPressed != 46) && (keyPressed < 48 || keyPressed > 57));
    };

    var validarNumero = function(evt) {
        var keyPressed = (evt.which) ? evt.which : event.keyCode
        return !((keyPressed !=13) && (keyPressed < 48 || keyPressed > 57));
    };

    var validarLetrasEspacio = function(identificadores) {
        $.each(identificadores, function (index, item) {
            $('#'+item).validCampoFranz(' abcdefghijklmnñopqrstuvwxyzáéíóú_');
        });        
    };

    var validarAlfanumerico = function(identificadores) {
        $.each(identificadores, function (index, item) {
            $('#'+item).validCampoFranz(' abcdefghijklmnñopqrstuvwxyzáéíóú1234567890_');
        });        
    };

    var validarAlfanumericoGuion = function (identificadores) {
        $.each(identificadores, function (index, item) {
            $('#' + item).validCampoFranz(' abcdefghijklmnñopqrstuvwxyzáéíóú1234567890_-');
        });
    };

    var validarNumerico = function(identificadores) {
        $.each(identificadores, function (index, item) {
            $('#'+item).validCampoFranz('1234567890');
        });        
    };
    var validarCantidadAmarillaMax = function (identificadores,cantidad) {
        return (identificadores > cantidad) ? true : false;
    };
    var validarBisiesto = function (anio) {
        return ((anio % 4 == 0 && anio % 100 != 0) || anio % 400 == 0) ? true : false;
    };

    return {
        init: function (parametros) {
            if (parametros) {
                _mensajeEspera = parametros.mensajeEspera;
                _tituloPopupMensaje = parametros.tituloPopupMensaje;
                _tituloPopupComfirmacion = parametros.tituloPopupComfirmacion;
                _tituloEliminacionPopupMensaje = parametros.tituloEliminacionPopupMensaje;
                _mensajePopupConfirmacion = parametros.mensajePopupConfirmacion;
                _mensajePopupEliminacionConfirmacion = parametros.mensajePopupEliminacionConfirmacion
                _btnCancelar = parametros.btnCancelar;
                _btnAceptar = parametros.btnAceptar;
                _formatoFecha = parametros.formatoFecha;
            }
            formValidBootstrap();
            createLoading();
            createMessageDialog();
            createConfirmDialog();
            createReConfirmDialog();
            createMessageDeleteDialog();
        },
        getDataForm: getDataForm,
        showLoading: function () {
            $('#' + _popupEspera).modal("show");
        },
        hideLoading: function () {
            $('#' + _popupEspera).modal("hide");
        },
        showMessageDialog: function (message, fnAceptar) {
            $('#' + _popupMensaje + ' .modal-body').html(message);
            $('#' + _popupMensaje).modal('show');

            if ($.isFunction(fnAceptar)) {
                $('#' + _popupMensaje + ' .btn-aceptar').off('click');
                $('#' + _popupMensaje + ' .close').off('click');
                $('#' + _popupMensaje + ' .btn-aceptar').on('click', fnAceptar);
                $('#' + _popupMensaje + ' .close').on('click', fnAceptar);
            }
        },
        showConfirmDialog: function (fnSuccess, message, fnCancel) {
        	
            var popup = $('#' + _popupConfirmacion);
            popup.modal('show');
            var btnSuccess = $(popup).find('.btn-primary');
            var btnCancel = $(popup).find('.btn-danger');

            btnSuccess.off('click');
            if ($.isFunction(fnSuccess)) {
                btnSuccess.on('click', function () { fnSuccess(); })
            }

            btnCancel.off('click');
            if ($.isFunction(fnCancel)) {
                btnCancel.on('click', function () { fnCancel(); })
            }

            if (message != null && message != '') {
                $('#' + _popupConfirmacion + ' .modal-body p').html(message);
            } else {
                $('#' + _popupConfirmacion + ' .modal-body p').html(_mensajePopupConfirmacion);
            }

            
        },
        showReConfirmDialog: function (fnSuccess, message, fnCancel) {
            var popup = $('#' + _popupReConfirmacion);
            var btnSuccess = $(popup).find('.btn-primary');
            var btnCancel = $(popup).find('.btn-danger');

            btnSuccess.off('click');
            if ($.isFunction(fnSuccess)) {
                btnSuccess.on('click', function () { fnSuccess(); })
            }

            btnCancel.off('click');
            if ($.isFunction(fnCancel)) {
                btnCancel.on('click', function () { fnCancel(); })
            }

            if (message != null && message != '') {
                $('#' + _popupReConfirmacion + ' .modal-body p').html(message);
            } else {
                $('#' + _popupReConfirmacion + ' .modal-body p').html(_mensajePopupConfirmacion);
            }

            popup.modal('show');
        },
        showReDeleteConfirmDialog: function (fnSuccess, message, fnCancel) {
            var popup = $('#' + _popupReConfirmacion);
            var btnSuccess = $(popup).find('.btn-primary');
            var btnCancel = $(popup).find('.btn-danger');

            btnSuccess.off('click');
            if ($.isFunction(fnSuccess)) {
                btnSuccess.on('click', function () { fnSuccess(); })
            }

            btnCancel.off('click');
            if ($.isFunction(fnCancel)) {
                btnCancel.on('click', function () { fnCancel(); })
            }

            if (message != null && message != '') {
                $('#' + _popupReConfirmacion + ' .modal-body p').html(message);
            } else {
                $('#' + _popupReConfirmacion + ' .modal-body p').html(_mensajePopupEliminacionConfirmacion);
            }

            popup.modal('show');
        },
        showConfirmResumeDialog: function (fnSuccess, message, fnCancel) {
            
            var popup = $('#' + _popupConfirmacion);
            popup.modal('show');
            var btnSuccess = $(popup).find('.btn-primary');
            var btnCancel = $(popup).find('.btn-danger');

            btnSuccess.off('click');
            if ($.isFunction(fnSuccess)) {
                btnSuccess.on('click', function () { fnSuccess(); })
            }

            btnCancel.off('click');
            if ($.isFunction(fnCancel)) {
                btnCancel.on('click', function () { fnCancel(); })
            }

            if (message != null && message != '') {
                $('#' + _popupConfirmacion + ' .modal-body p').html(message);
            } else {
                $('#' + _popupConfirmacion + ' .modal-body p').text(_mensajePopupConfirmacion);
            }
            
        },          
        showDeleteConfirmDialog: function (fnSuccess, message, fnCancel) {
            var popup = $('#' + _popupEliminacionMensaje);
            var btnSuccess = $(popup).find('.btn-primary');
            var btnCancel = $(popup).find('.btn-danger');

            btnSuccess.off('click');
            if ($.isFunction(fnSuccess)) {
                btnSuccess.on('click', function () { fnSuccess(); })
            }

            btnCancel.off('click');
            if ($.isFunction(fnCancel)) {
                btnCancel.on('click', function () { fnCancel(); })
            }

            if (message != null && message != '') {
                $('#' + _popupEliminacionMensaje + ' .modal-body p').text(message);
            } else {
                $('#' + _popupEliminacionMensaje + ' .modal-body p').text(_mensajePopupEliminacionConfirmacion);
            }

            popup.modal('show');
        },             
        formatResponse: function (respuesta, contenedor) {
            var estado = "";
            if (respuesta.Success) {
                if (!respuesta.Warning) {
                    estado = "alert-success";
                }
            } else {
                estado = "alert-danger";
            }
            $("#" + contenedor).append("<div class='alert " + estado + "'>" + respuesta.Message + "</div>");
        },
        clearResponse: function (contenedor) {
            $("#" + contenedor).html('');
        },
        clearForm: function (form) {

            $(".form-group").removeClass('has-error');
            $(".help-block").remove(); 
            $('#' + form).find('[name]').each(function (index, value) {
                var that = $(this);
                var name = that.attr('name');
                var value = that.val();               

                if (that.attr('type') === 'radio') {
                    if (that.is(':checked')) {
                        that.attr('checked', false)
                    }
                } else if (that.attr('type') === 'checkbox') {
                    if (that.is(':checked')) {
                        that.attr('checked', false)
                    }
                } else {
                    that.val('');
                }
            });
        },
        Ajax: function (opciones, successCallback, failureCallback, errorCallback) {

            if (opciones.url == null)
                opciones.url = "";

            if (opciones.cache == null)
                opciones.cache = false;            

            if (opciones.parametros == null)
                opciones.parametros = {};

            if (opciones.async == null)
                opciones.async = true;

            if (opciones.datatype == null)
                opciones.datatype = "json";

            if (opciones.contentType == null)
                opciones.contentType = "application/json; charset=utf-8";

            if (opciones.type == null)
                opciones.type = "POST";

            $.ajax({
                type: opciones.type,
                url: opciones.url,
                cache: opciones.cache,
                async: opciones.async,
                data: opciones.parametros,
                success: function (response) {
                    if (successCallback != null && typeof (successCallback) == "function")
                        successCallback(response);
                },
                failure: function (msg) {
                    if (failureCallback != null && typeof (failureCallback) == "function")
                        failureCallback(msg);
                },
                error: function (xhr, status, error) {
                    if (errorCallback != null && typeof (errorCallback) == "function")
                        errorCallback(xhr);
                }
            });
        },
        ExportarExcel: function (url) {
            
            exportarExcel(url);
        },
        InicializarValidacion: function (formName, rules, messages, ignore) {
            if (ignore == null) ignore = ".ignore";

            $('#'+formName).validate({
                errorElement: 'div',
                errorClass: 'help-block',
                focusInvalid: false,
                ignore: ignore,
                rules: rules,
                messages: messages,

                highlight: function (e) {
                    $(e).closest('.form-group').removeClass('has-info').addClass('has-error');
                },

                success: function (e) {
                    $(e).closest('.form-group').removeClass('has-error');//.addClass('has-info');
                    $(e).remove();
                },

                errorPlacement: function (error, element) {
                    if (element.is('input[type=checkbox]') || element.is('input[type=radio]')) {
                        var controls = element.closest('div[class*="col-"]');
                        if (controls.find(':checkbox,:radio').length > 1) controls.append(error);
                        else error.insertAfter(element.nextAll('.lbl:eq(0)').eq(0));
                    }
                    else if (element.is('.select2')) {
                        error.insertAfter(element.siblings('[class*="select2-container"]:eq(0)'));
                    }
                    else if (element.is('.chosen-select')) {
                        error.insertAfter(element.siblings('[class*="chosen-container"]:eq(0)'));
                    }
                    else error.insertAfter(element.parent());
                },

                submitHandler: function (form) {
                },
                invalidHandler: function (form) {
                }
            });
        },
        guid: function() {
            return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
              s4() + '-' + s4() + s4() + s4();
        },
        disableAllFormElements: disableAllFormElements,
        mayuscula: mayuscula,
        sumaFecha: sumaFecha,
        inicializarFileUpload: inicializarFileUpload,
        number_format: number_format,
        createGrid: createGrid,
        validarDecimal: validarDecimal,
        validarNumero: validarNumero,
        validarLetrasEspacio: validarLetrasEspacio,
        validarAlfanumerico: validarAlfanumerico,
        validarAlfanumericoGuion: validarAlfanumericoGuion,
        validarNumerico: validarNumerico,
        validarBisiesto: validarBisiesto
    }
}();

function getCookie(name) {
    var pairs = document.cookie.split("; "),
        count = pairs.length,
        parts;
    while (count--) {
        parts = pairs[count].split("=");
        if (parts[0] === name)
            return parts[1];
    }
    return false;
}

function delCookie(name) {
    var date = new Date();
    date.setDate(date.getDate() - 1);
    document.cookie = name + "=" + '=;expires=' + date + "; path=/";
}

function s4() {
    return Math.floor((1 + Math.random()) * 0x10000)
      .toString(16)
      .substring(1);
}