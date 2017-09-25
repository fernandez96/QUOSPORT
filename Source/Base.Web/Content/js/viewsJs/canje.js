
var urlGetAllDocumento = baseUrlApiService + 'Persona/'
var urlGetAllListProducto = baseUrlApiService + 'TipoProducto/'
var urlGetAllListVale = baseUrlApiService + 'Vale/'
var urlAddCanje = baseUrlApiService + 'Canje/'
var urlAlertaCount = baseUrlApiService + 'Alerta/';
$(document).ready(function () {
	var $validation = true;
	$('#fuelux-wizard-container')
	.ace_wizard({
		//step: 2, //optional argument. wizard will jump to step "2" at first
		//buttons: '.wizard-actions:eq(0)'
	})
	.on('actionclicked.fu.wizard' , function(e, info){
		if(info.step == 1) {
			if(info.direction == "next"){
		        if(!$('#frmTitular').valid()){
		            e.preventDefault();
		        }else if(!$("#TitularNumeroDocumento").val().length>0){
		        	webApp.showMessageDialog("Debe buscar datos del Titular");
		        	e.preventDefault();
		        }else{
			        if($("#EsBeneficiario").prop('checked')){
						var wizard = $('#fuelux-wizard-container').data('fu.wizard')
						wizard.currentStep = 1;
						wizard.setState(); 

			            $("#frmBeneficiario .form-group").removeClass('has-error');
			            $("#frmBeneficiario .help-block").remove();						     	
			        }else{
						var wizard = $('#fuelux-wizard-container').data('fu.wizard')
						wizard.currentStep = 2;    	
			        }
		        }
			}

			if(info.direction == "previous"){
			}
		}

		if(info.step == 2) {
			if(info.direction == "next"){
		        if(!$('#frmBeneficiario').valid()){
		            e.preventDefault();
		        }
			}

			if(info.direction == "previous"){
			}
		}		

		if(info.step == 3) {
			if(info.direction == "previous"){
		        if($("#EsBeneficiario").prop('checked')){
					var wizard = $('#fuelux-wizard-container').data('fu.wizard')
					wizard.currentStep = 2;
					wizard.setState();
					e.preventDefault();
		        }else{
					var wizard = $('#fuelux-wizard-container').data('fu.wizard')
					wizard.currentStep = 1;
					wizard.setState();
					e.preventDefault();
		        }
			}
		}		
	})
	.on('finished.fu.wizard', function(e) {

		if($('#frmCanje').valid()){
		    webApp.showConfirmResumeDialog(function () {
		        checkSession(function () {
		            GuardarCanje();
		        });			   
			},
			GetResumen());				
		}else{
			webApp.showMessageDialog("Debe ingresar los datos correctamente");
		}		

	}).on('stepclick.fu.wizard', function(e){
		//e.preventDefault();//this will prevent clicking and selecting steps
	});

	$("#btnBuscarDniTitular").click(function () {
	    checkSession(function () {
	        BuscarTitular();
	    });
    	
    });	

	$("#btnBuscarDniBeneficiario").click(function () {
	    checkSession(function () {
	        BuscarBeneficiario();
	    });
    	
    });    

    $("#TipoProductoId").change(function(){
        CargarVale();
    });

    webApp.validarNumerico(['DniTitular']);
    webApp.InicializarValidacion('frmTitular', 
        {
            DniTitular: {
                required: true,
                strippedminlength: {
                    param: 8
                },
                noPasteAllowNumber: true

            },
            TitularCorreoElectronico: {
                email:true
            },
            TitularCorreoElectronicoAdicional: {
                //required: true,
                email: true
            }                            
        },
        {
            DniTitular: {
                required: "Por favor ingrese DNI",
                strippedminlength: "Por favor ingrese al menos 8 caracteres"
            },
            TitularCorreoElectronico: {
                required: "Por favor ingrese Correo",
                email: "Por favor ingrese Correo válido"
            },
            TitularCorreoElectronicoAdicional: {
                required: "Por favor ingrese Correo Adicional",
                email: "Por favor ingrese Correo Adicional válido"
            }                                
        }
        
        ); 
    webApp.validarNumerico(['DniBeneficiario']);
	webApp.validarLetrasEspacio(['BeneficiarioNombres','BeneficiarioApellidos']);
	webApp.validarNumerico(['BeneficiarioNumeroDocumento']);
    webApp.InicializarValidacion('frmBeneficiario', 
        {
            BeneficiarioNombres: {
                required: true
            },
            BeneficiarioApellidos: {
                required: true
            },
            BeneficiarioTipoDocumento: {
                required: true
            },
            BeneficiarioNumeroDocumento: {
                required: true,
             
            },
            BeneficiarioCorreoElectronico: {
                required: true,
                email:true
            }                  
        },
        {
            BeneficiarioNombres: {
                required: "Por favor ingrese Nombres"
            },
            BeneficiarioApellidos: {
                required: "Por favor ingrese Apellidos"
            },
            BeneficiarioTipoDocumento: {
                required: "Por favor seleccione Tipo Documento"
            },
            BeneficiarioNumeroDocumento: {
                required: "Por favor ingrese Número Documento"
            },
            BeneficiarioCorreoElectronico: {
                required: "Por favor ingrese Correo",
                email: "Por favor ingrese Correo válido"
            }                      
        });

    webApp.InicializarValidacion('frmCanje', 
        {
            TipoProductoId: {
                required: true
            },
            ValeId: {
                required: true
            }                
        },
        {
            TipoProductoId: {
                required: "Por favor seleccione Tipo Producto"
            },
            ValeId: {
                required: "Por favor seleccione Vale"
            }                     
        });

	CargarTipoProducto();
	$("#FechaCaducidad").val(cargarFechaActual());
});	

function buscar(e){
    tecla = (document.all) ? e.keyCode : e.which;
    if (tecla == 13) {
        checkSession(function () {
            BuscarTitular();
        });
    }
}

function buscarBeneficiario(e){
    tecla = (document.all) ? e.keyCode : e.which;
    if (tecla == 13) {
        checkSession(function () {
            BuscarBeneficiario();
        });       
    }
}

function cargarFechaActual(){
	return webApp.sumaFecha(90,null);	
}

function BuscarTitular(){
    if ($('#DniTitular').val().length >= 8) {
        webApp.Ajax({
            url: urlGetAllDocumento + 'GetDocumento',
            async: false,
            parametros:{
                NumeroDocumento:  $('#DniTitular').val()
            } 
        }, function (response) {
            if (response.Success) {
                if (response.Warning) {
                    $("#TitularId").val('');
                    $("#TitularNombres").val('');
                    $("#TitularApellidos").val('');
                    $("#TitularTipoDocumento").val('');
                    $("#TitularNumeroDocumento").val('');
                    $("#TitularCorreoElectronico").val('');
                    $("#TitularCorreoElectronicoAdicional").val('');
                    $.gritter.add({
                        title: 'Alerta',
                        text: response.Message,
                        class_name: 'gritter-warning gritter'
                    });
                } else {
                    var persona = response.Data;
                    $("#TitularId").val(persona.Id);
                    $("#TitularNombres").val(persona.Nombres);
                    $("#TitularApellidos").val(persona.Apellidos);
                    $("#TitularTipoDocumento").val(persona.TipoDocumento);
                    $("#TitularNumeroDocumento").val(persona.NumeroDocumento);
                    $("#TitularCorreoElectronico").val(persona.CorreoElectronico);

                    if (persona.CorreoElectronico != "") {
                        $("#TitularCorreoElectronicoAdicional").removeClass("ignoreForm");
                        $("#TitularCorreoElectronicoAdicional").addClass("ignoreForm");
                    } else {
                        $("#TitularCorreoElectronicoAdicional").removeClass("ignoreForm");
                    }
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
}	

function BuscarBeneficiario(){
    if ($('#DniBeneficiario').val().length >= 8) {
        webApp.Ajax({
            url: urlGetAllDocumento + 'GetDocumento',
            async: false,
            parametros: {
                NumeroDocumento: $('#DniBeneficiario').val()
            }
        }, function (response) {
            if (response.Success) {
                if (response.Warning) {
                    $("#BeneficiarioId").val('');
                    $("#BeneficiarioNombres").val('');
                    $("#BeneficiarioApellidos").val('');
                    $("#BeneficiarioTipoDocumento").val('');
                    $("#BeneficiarioNumeroDocumento").val('');
                    $("#BeneficiarioCorreoElectronico").val('');
                    $.gritter.add({
                        title: 'Alerta',
                        text: response.Message,
                        class_name: 'gritter-warning'
                    });
                }
                else {
                    var persona = response.Data;
                    $("#BeneficiarioId").val(persona.Id);
                    $("#BeneficiarioNombres").val(persona.Nombres);
                    $("#BeneficiarioApellidos").val(persona.Apellidos);
                    $("#BeneficiarioTipoDocumento").val(persona.TipoDocumento);
                    $("#BeneficiarioNumeroDocumento").val(persona.NumeroDocumento);
                    $("#BeneficiarioCorreoElectronico").val(persona.CorreoElectronico);
                }
            }
            else{
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
}

function CargarTipoProducto(){
    var WhereFilters = {
        whereFilter: 'WHERE Estado IN (1,2)'
    };
    webApp.Ajax({
        url: urlGetAllListProducto + 'GetAll',
        parametros: WhereFilters,
        async: false,
    }, function(response){
        if(response.Success){
            
            if(response.Warning){                           
                $.gritter.add({
                    title: 'Alerta',
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });                         
            }else{
                $.each(response.Data, function(index, item){
                    $("#TipoProductoId").append('<option value="'+ item.Id +'">' + item.Nombre + '</option>');
                });

                CargarVale();
            }
        }else{
            $.gritter.add({
                title: 'Error',
                text: response.message,
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

function CargarVale(){

    var modelView = {
        TipoProductoId: $("#TipoProductoId").val()
    };    
    
    webApp.Ajax({
        url: urlGetAllListVale + 'GetAllByTipoProducto',
        async: false,
        parametros: modelView,
    }, function(response){
        if(response.Success){
            $("#ValeId").html('');
            if(response.Warning){                           
                $.gritter.add({
                    title: 'Alerta',
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });                         
            }else{                
                $.each(response.Data, function(index, item){
                    $("#ValeId").append('<option value="'+ item.Id +'">' + item.Puntos + ' puntos por S/ ' + item.Monto + ' soles</option>');
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

function GuardarCanje() {
 
    var modelView = {
    	TitularId: $("#TitularId").val(),
        TitularNombres: $("#TitularNombres").val(),
        TitularApellidos: $("#TitularApellidos").val(),
        TitularTipoDocumento: $("#TitularTipoDocumento").val(),
        TitularNumeroDocumento : $("#TitularNumeroDocumento").val(),
        TitularCorreoElectronico : $("#TitularCorreoElectronico").val(),
        TitularCorreoElectronicoAdicional: $("#TitularCorreoElectronicoAdicional").val(),
        EsBeneficiario : $("#EsBeneficiario").prop('checked')?1:0,

        BeneficiarioId:  $("#BeneficiarioId").val(),
        BeneficiarioNombres : $("#BeneficiarioNombres").val(),
        BeneficiarioApellidos : $("#BeneficiarioApellidos").val(),
        BeneficiarioTipoDocumento :  $("#BeneficiarioTipoDocumento").val(),
        BeneficiarioNumeroDocumento : $("#BeneficiarioNumeroDocumento").val(),
        BeneficiarioCorreoElectronico : $("#BeneficiarioCorreoElectronico").val(),

        TipoProductoId : $("#TipoProductoId").val(),
        ValeId : $("#ValeId").val(),
        FechaCaducidad: $("#FechaCaducidad").val(),
        UsuarioRegistro: $("#usernameLogOn strong").text()
    };

    webApp.Ajax({
        url: urlAddCanje +'Add',
        parametros: modelView
        
    }, function(response){
        if(response.Success){    

            if(response.Warning){                           
                $.gritter.add({
                    title: response.Title,
                    text: response.Message,
                    class_name: 'gritter-warning gritter'
                });                         
            }else{                
                $.gritter.add({
                    title: response.Title,
                    text: response.Message,
                    class_name: 'gritter-success gritter'
                });
                LimpiarPasarella();
                NotificarAlerta();
                EnviarCorreoTope();
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

function LimpiarPasarella(){

    $("#DniTitular").val('');
    $("#TitularId").val('');
    $("#TitularNombres").val('');
    $("#TitularApellidos").val('');
    $("#TitularTipoDocumento").val('');
    $("#TitularNumeroDocumento").val('');
    $("#TitularCorreoElectronico").val('');
    $("#TitularCorreoElectronicoAdicional").val('');
    $("#EsBeneficiario").prop('checked',false);

    $("#DniBeneficiario").val('');
    $("#BeneficiarioId").val('');
    $("#BeneficiarioNombres").val('');
    $("#BeneficiarioApellidos").val('');
    $("#BeneficiarioTipoDocumento").val('');
    $("#BeneficiarioNumeroDocumento").val('');
    $("#BeneficiarioCorreoElectronico").val('');   

    $("#TipoProductoId").val(0);
    $("#ValeId").val(0);


    var wizard = $('#fuelux-wizard-container').data('fu.wizard')
    wizard.currentStep = 1;
    wizard.setState();    
}

function GetResumen(){
    var mensaje="<br/>A continuaci&oacute;n realizar&aacute; un <strong>Canje GiftCard</strong> con los siguientes datos:<br/><br/>";
    mensaje += ('<strong>Tipo Producto: </strong> ' + $("#TipoProductoId option:selected").text() + '<br/>');
    mensaje += ('<strong>Monto GiftCard: </strong> ' + $("#ValeId option:selected").text() + '<br/>');
    if($("#EsBeneficiario").prop('checked')){
        mensaje += ('<strong>Nombre: </strong>' + $("#BeneficiarioNombres").val() + ' ' + $("#BeneficiarioApellidos").val() + '<br/>');
        mensaje += ('<strong>DNI: </strong> ' + $("#BeneficiarioNumeroDocumento").val() + '<br/>');
        mensaje += ('<strong>Correo: </strong> ' + $("#BeneficiarioCorreoElectronico").val() + '<br/>');

        if($("#TitularCorreoElectronico").val()=="")
        {
            mensaje += ('<strong>Correo Titular: </strong> ' + $("#TitularCorreoElectronicoAdicional").val() + '<br/>');
        }else
        {
            mensaje += ('<strong>Correo Titular: </strong> ' + $("#TitularCorreoElectronico").val() + '<br/>');
        }
        
    }else{
        mensaje += ('<strong>Nombre: </strong>' + $("#TitularNombres").val() + ' ' + $("#TitularApellidos").val() + '<br/>');
        mensaje += ('<strong>DNI: </strong> ' + $("#TitularNumeroDocumento").val() + '<br/>');
        mensaje += ('<strong>Correo: </strong> ' + $("#TitularCorreoElectronico").val() + '<br/>');
        mensaje += ('<strong>Correo Adicional: </strong> ' + $("#TitularCorreoElectronicoAdicional").val() + '<br/>');
    }
    mensaje += ('<strong>V&aacute;lido hasta: </strong> ' + $("#FechaCaducidad").val() + '<br/><br/>');
    mensaje += '<strong>¿Está seguro que se desea continuar con el proceso?</strong>';

    return mensaje;
}