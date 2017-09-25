	var date = new Date();
	var d = date.getDate();
	var m = date.getMonth();
	var y = date.getFullYear();
	var listaRequerimientos = new Array();

jQuery(function($) {

	$("#usuario").change(function () {
      GetAllCalendario();
    });	

	CargarFiltros();
});

function CargarFiltros(){
	$.ajax({
	    data : { action: 'FindAllActive' },
	    url:   'Controllers/ItemTablaController.php',
	    type:  'post',
	    beforeSend: function () {
	            
	    },
	    success:  function (response) {
        	if(response.success){

        		if(response.warning){                			
        			$.gritter.add({
        				title: 'Alerta',
        				text: response.message,
        				class_name: 'gritter-warning gritter-center'
        			});                			
        		}        			
					
				$.each(response.data, function( key, value ) {
					if(value.tablaId==1){
          				$("#areaIdV").append("<option value='"+value.valor+"'>"+value.nombre+"</option>");
					}

					if(value.tablaId==2){
          				$("#encargadoIdV").append("<option value='"+value.valor+"'>"+value.nombre+"</option>");
					}

					if(value.tablaId==3){
			          $("#entregableIdV").append("<option value='"+value.valor+"'>"+value.nombre+"</option>"); 
			        }

			        if(value.tablaId==4){
		              $("#usuario").append("<option value='"+value.valor+"'>"+value.nombre+"</option>");	
		            }
        			
				});

				$('#usuario').chosen({allow_single_deselect:true});
				SeleccionarUsuario();	
				GetAllCalendario();		

        	}else{
    			$.gritter.add({
    				title: 'Error',
    				text: response.message,
    				class_name: 'gritter-error gritter-center'
    			});                		
        	}	    	

	    },
	    error: function(XMLHttpRequest, textStatus, errorThrown) { 
			$.gritter.add({
				title: 'Error',
				text: "Status: " + textStatus + "<br/>Error: " + errorThrown,
				class_name: 'gritter-error gritter-center'
			});	        
	    }                
	});
}

function GetAllCalendario(){
	
	var usuarioId = $("#usuario").val();

	$.ajax({
	    data : { action: 'GetAllCalendario', usuarioId : usuarioId},
	    url:   'Controllers/RequerimientoController.php',
	    type:  'post',
	    beforeSend: function () {
	     	pageBlocked = false;
	    },
	    success:  function (response) {
	    	
	    	var requerimientos = response.data;
	    	listaRequerimientos = [];
			$.each(requerimientos, function( key, value ) {
				var requerimiento = Object();
				requerimiento.title = value.codigo;
				
				if(value.estadoRequerimiento == 2)
					requerimiento.className = 'label-yellow';
				if(value.estadoRequerimiento == 3)
					requerimiento.className = 'label-info';
				if(value.estadoRequerimiento == 4)
					requerimiento.className = 'label-grey';

				var fechas = value.fechaReanudacion.split('-');
				fechas[1] = parseInt(fechas[1])-1;
				requerimiento.start = new Date(fechas[0], fechas[1], fechas[2].substring(0, 2));
				
				requerimiento.allDay = true,

				listaRequerimientos.push(requerimiento);
			});

	    	InicializarCalendario(listaRequerimientos);

        	if(response.success){

        		if(response.warning){                			
        			$.gritter.add({
        				title: 'Alerta',
        				text: response.message,
        				class_name: 'gritter-warning gritter-center'
        			});                			
        		}

        	}else{
    			$.gritter.add({
    				title: 'Error',
    				text: response.message,
    				class_name: 'gritter-error gritter-center'
    			});                		
        	}	    	

	    },
	    error: function(XMLHttpRequest, textStatus, errorThrown) { 
			$.gritter.add({
				title: 'Error',
				text: "Status: " + textStatus + "<br/>Error: " + errorThrown,
				class_name: 'gritter-error gritter-center'
			});	        
	    }                
	});
}

function InicializarCalendario(listaRequerimientos){
	
	/* initialize the calendar
	-----------------------------------------------------------------*/
	$('#calendar').fullCalendar( 'destroy' );
	var calendar = $('#calendar').fullCalendar({
		//isRTL: true,
		 buttonHtml: {
			prev: '<i class="ace-icon fa fa-chevron-left"></i>',
			next: '<i class="ace-icon fa fa-chevron-right"></i>'
		},
	
		header: {
			left: 'prev,next today',
			center: 'title',
			right: 'month,agendaWeek,agendaDay'
		},
		events: listaRequerimientos
		,
		editable: true,
		droppable: true, // this allows things to be dropped onto the calendar !!!
		drop: function(date, allDay) { // this function is called when something is dropped
		
			// retrieve the dropped element's stored Event Object
			var originalEventObject = $(this).data('eventObject');
			var $extraEventClass = $(this).attr('data-class');
			
			
			// we need to copy it, so that multiple events don't have a reference to the same object
			var copiedEventObject = $.extend({}, originalEventObject);
			
			// assign it the date that was reported
			copiedEventObject.start = date;
			copiedEventObject.allDay = allDay;
			if($extraEventClass) copiedEventObject['className'] = [$extraEventClass];
			
			// render the event on the calendar
			// the last `true` argument determines if the event "sticks" (http://arshaw.com/fullcalendar/docs/event_rendering/renderEvent/)
			$('#calendar').fullCalendar('renderEvent', copiedEventObject, true);
			
			// is the "remove after drop" checkbox checked?
			if ($('#drop-remove').is(':checked')) {
				// if so, remove the element from the "Draggable Events" list
				$(this).remove();
			}
			
		}
		,
		selectable: true,
		selectHelper: true,
		select: function(start, end, allDay) {
			
			bootbox.prompt("New Event Title:", function(title) {
				if (title !== null) {
					calendar.fullCalendar('renderEvent',
						{
							title: title,
							start: start,
							end: end,
							allDay: allDay,
							className: 'label-info'
						},
						true // make the event "stick"
					);
				}
			});
			

			calendar.fullCalendar('unselect');
		}
		,
		eventClick: function(calEvent, jsEvent, view) {

			var requerimientoId = parseInt(calEvent.title.substring(2,10));


			$.ajax({
              data : { action: 'GetById', id: requerimientoId },
              url:   'Controllers/RequerimientoController.php',
              type:  'post',
              beforeSend: function () {
                      
              },
              success:  function (response) {
                
                  if(response.success){

                    if(response.warning){                     
                      $.gritter.add({
                        title: 'Alerta',
                        text: response.message,
                        class_name: 'gritter-warning gritter-center'
                      });                     
                    }
                    var requerimiento = response.data;

                    LimpiarVisualizador();                 
                    $("#requerimientoId").val(requerimiento.id);
                    $("#codigoV").val(requerimiento.codigo);
                    $("#areaIdV").val(requerimiento.areaId);
                    $("#encargadoIdV").val(requerimiento.encargadoId);
                    $("#descripcionV").val(requerimiento.descripcion);
                    $("#entregableIdV").val(requerimiento.entregableId);
                    $("#entregableIdV").val(requerimiento.entregableId);
                    $("#entregableIdV").val(requerimiento.entregableId);
                    $("#fechaInicioV").val(requerimiento.fechaInicio);
                    $("#fechaFinV").val(requerimiento.fechaReanudacion);
                    $("#avanceV").val(requerimiento.avance);
                    $("#observacionV").val(requerimiento.observacionV);

                    $("#visualizadorTitle").text(requerimiento.codigo);
                    $("#VerRequerimiento").modal("show");                            

                  }else{
                  $.gritter.add({
                    title: 'Error',
                    text: response.message,
                    class_name: 'gritter-error gritter-center'
                  });                   
                  }       

              },
              error: function(XMLHttpRequest, textStatus, errorThrown) { 
                
	              $.gritter.add({
	                title: 'Error',
	                text: "Status: " + textStatus + "<br/>Error: " + errorThrown,
	                class_name: 'gritter-error gritter-center'
	              });         
              }                
          	});	
		}
		
	});
}

function LimpiarVisualizador(){
	$("#codigoV, #descripcionV, observacionV").val('');
	$("#areaIdV, #encargadoIdV, #entregableIdV, #entregableIdV, #avanceV").val(0);
}