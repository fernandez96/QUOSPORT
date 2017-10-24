var urlMantenimientoStock = baseUrl + 'StockXAlmacen/';
//var stockxalmacen = new Array();

//jQuery(document).ready(function () {


//});


function ListarStockXAlmacen() {
    pageBlocked = true;
    webApp.Ajax({
        url: urlMantenimientoStock + 'ListaStockXAlmacen'

    }, function (response) {
    }, function (response) {
    }, function (XMLHttpRequest, textStatus, errorThrown) {
    });
}