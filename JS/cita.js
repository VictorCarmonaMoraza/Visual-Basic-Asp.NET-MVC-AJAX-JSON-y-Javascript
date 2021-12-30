$("#txtfechaEnfermedad").datepicker({ dateFormat: 'dd-mm-yy' });
/*$("#txtfechaAtencion").datepicker({ dateFormat: 'dd-mm-yy' });*/
listar();

function listar() {

    $.get("Cita/listarEstadosCitas", function (data) {
        crearCombo(data, "cboEstadoCita");
    })

    $.get("Cita/llenarDoctor", function (data) {
        crearCombo(data, "cboDoctor");
    })

    $.get("Cita/listadoCita", function (data) {
        crearListadoDeCitas(["IdCita", "T.Mascota", "F.Enfermedad", "Nom Usuario", "Estado Cita"], data, "tablaCita", "tabla", true);
    })
    //Listamos los tipos de mascotas
    $.get("Cita/listarTipoMascota", function (data) {
        crearCombo(data, "cboTipoMascota");
    })

    //listamos las sedes
    $.get("Cita/listarSedes", function (data) {
        crearCombo(data, "cboSede");
    })
}

function crearCombo(data, comboId) {
    var contenido = "";

    contenido += "<option value=''>--Seleccione</option>"
    //Generando las opciones
    for (var i = 0; i < data.length; i++) {
        contenido += "<option value='" + data[i].Id + "'>" + data[i].Nombre + "</option>";

    }

    document.getElementById(comboId).innerHTML = contenido;

}

//funcion para mostrar las mascotas en funcion del tipo de mascota seleccionado
function mostrarMascota() {
    var idTipo = document.getElementById("cboTipoMascota").value;
    $.get("Cita/listarMascotasPorTipo/?idTipo=" + idTipo, function (data) {
        crearCombo(data, "cboMascota");
    })
}

//Funcion para validar datos
function validarDatos() {
    debugger;
    //Todos los campos estan bien validados
    var mensaje = "";
    var exito = true;
    var obligatorios = document.getElementsByClassName("obligatorio");
    var nobligatorios = obligatorios.length;
    for (var i = 0; i < nobligatorios; i++) {
        //C 
        if (obligatorios[i].value == "") {
            exito = false;
            mensaje += "<li>Debe ingresar " + obligatorios[i].name + "</li>";
        }
    }
    return { exito, mensaje };
}

//function para guardar una cita
function Guardar() {
    var objeto = validarDatos();
    var esCorrecto = objeto.exito;
    if (esCorrecto == false) {
        document.getElementById("divErrores").innerHTML = "<ol>" + objeto.mensaje + "</ol>";
        return;
    }
    //Obtenemos los valores del formulario
    var idTipo = document.getElementById("cboTipoMascota").value;
    var idMascota = document.getElementById("cboMascota").value;
    var fecha = document.getElementById("txtfechaEnfermedad").value;
    var sede = document.getElementById("cboSede").value;
    var descripcion = document.getElementById("txtdescripcion").value;

    var objetoCita = new FormData();
    objetoCita.append("IIDTIPOMASCOTA", idTipo);
    objetoCita.append("IIDMASCOTA", idMascota);
    objetoCita.append("DFECHAENFERMO", fecha);
    objetoCita.append("IIDSEDE", sede);
    objetoCita.append("VDESCRIPCION", descripcion);

    //Llamada al metodo para la creacion del ojeto cita
    $.ajax({
        type: "POST",
        url: "Cita/Guardar",
        data: objetoCita,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data == 1) {
                alert("se registro la solicitud de la cita correctamente");
                limpiar();
                listar();
            }
            else {
                alert("Ocurrio un error");
            }
        }

    });
}


function crearListadoDeCitas(cabeceras, data, divId, divTabla, verOperaciones) {
    //leemos valor del viewbag
    var iidvista = document.getElementById("txtIdVista").value;


    var color = "";
    var contenido = "";
    contenido += "<table id='" + divTabla + "' class='table'>";
    //Las cabeceras
    contenido += "<thead>";
    //Filas
    contenido += "<tr>";
    //Recorremos el array de las cebeceras
    for (var i = 0; i < cabeceras.length; i++) {
        contenido += "<td>" + cabeceras[i] + "</td>";
    }
    if (verOperaciones) {
        contenido += "<td>Operaciones</td>";
    }

    contenido += "</tr>";
    contenido += "</thead>";
    if (data.length > 0) {
        var propiedadesObjecto = Object.keys(data[0]);
        //Contenido de la tabla con los datos obtenidos de la consulta
        contenido += "<tbody>";
        //Recorremos la data
        var fila;
        for (var i = 0; i < data.length; i++) {
            fila = data[i];
            if (fila.IIDESTADOCITA == 1) {
                color = "black";
            } else if (fila.IIDESTADOCITA == 2) {
                color = "green";
            } else if (fila.IIDESTADOCITA == 5) {
                color = "red";
            }
            else if (fila.IIDESTADOCITA == 3) {
                color = "blue";
            }
            else {
                color = "black";
            }

            contenido += "<tr style='font-weight:bold;color:" + color + "'>";
            /* contenido += "<tr>";*/
            for (var j = 0; j < propiedadesObjecto.length; j++) {
                //Sacar el nombre de la propiedas
                var nombrePropiedad = propiedadesObjecto[j];
                if (nombrePropiedad != "IIDESTADOCITA") {
                    contenido += "<td>" + fila[nombrePropiedad] + "</td>";
                }
            }

            if (verOperaciones) {
                contenido += "<td with='200px'>";
                if (fila["IIDESTADOCITA"] == 1) {
                    contenido += "<i onclick='abrirModal(" + fila.IIDCITA + ")' class= 'btn btn-primary fas fa-edit' data-toggle='modal' data-target='#exampleModal'></i> ";
                }
                if (fila["IIDESTADOCITA"] != 1) {
                    contenido += "<i class='btn btn-secondary fas fa-eye' data-toggle='modal' data-target='#exampleModal' "
                    contenido += "onclick='mostrarHistorial(" + fila.IIDCITA + ")'></i> ";
                }
                if (fila["IIDESTADOCITA"] == 1) {
                    //boton enviar
                    contenido += "<i class='btn btn-primary fas fa-paper-plane' data-toggle='modal' data-target='#exampleModal' onclick='Enviar(" + fila.IIDCITA + ")'></i> ";
                }
                if (fila["IIDESTADOCITA"] == 1) {
                    //Boton Eliminar
                    contenido += "<i class='btn btn-danger fas fa-trash' data-toggle='modal' data-target='#exampleModal' onclick = 'mostrarHistorial(" + fila.IIDCITA + ")' ></i > ";
                }
                if(fila["IIDESTADOCITA"] == 2 && iidvista == 1) {
                    //Boton Revisar
                    contenido += "<i class='btn btn-primary fas fa-user-nurse' data-toggle='modal' data-target='#exampleModal' onclick='Revisar(" + fila.IIDCITA + ")' ></i > ";                        /* <i class="fas fa-window-close"></i>*/
                }
                if ((fila["IIDESTADOCITA"] == 2 || fila["IIDESTADOCITA"] == 3) && iidvista == 1) {
                    //iidvista == 1 ----> Vista total
                    //iidvista == 2 ----> Vista usuario
                    //Boton anular
                    contenido += "<i class='btn btn-danger fas fa-window-close' data-toggle='modal' data-target='#exampleModal' onclick='Anular(" + fila.IIDCITA + ")' ></i > ";
                    /* <i class="fas fa-window-close"></i>*/
                }

                contenido += "</td>";
            }
            contenido += "</tr>";
        }
        contenido += "</tbody>";
    }
    //Inyectamos en el div el contenido de la tabla
    document.getElementById(divId).innerHTML = contenido;
    //Paginamos
    $('#' + divTabla).DataTable({ searching: false });
}

function Revisar(id) {
    document.getElementById("lblTitulo").innerHTML = "Asignando Doctor";
    document.getElementById("tablaHistorial").innerHTML = "";

    //Muestra el boton por su id
    document.getElementById("btnGuardarObservacion").style.display = "none";
    document.getElementById("formObservacion").style.display = "none";
    document.getElementById("formAsignar").style.display = "block";
    document.getElementById("btnGuardarAsignacion").style.display = "inline-block";
    document.getElementById("txtCitaPopup").value = id;



}

function Anular(id) {
    debugger;
    document.getElementById("lblTitulo").innerHTML = "Anulando Cita";
    document.getElementById("tablaHistorial").innerHTML = "";
    //Oculta el boton por su id
    document.getElementById("btnGuardarObservacion").style.display = "block";
    document.getElementById("formObservacion").style.display = "inline-block";
    document.getElementById("formAsignar").style.display = "none";
    document.getElementById("btnGuardarAsignacion").style.display = "none";
    document.getElementById("txtCitaPopup").value = id;
}

function Enviar(idcita) {
    debugger;
    if (confirm("¿Desea enviar realmente la cita?") == 1) {

        $.get("Cita/cambiarEstado/?idCita=" + idcita + "&idestadoACambiar=2", function (data) {
            if (data == 0) {
                alert("Ocurrio un error");
            }
            else {
                alert("Se envio correctamente");
                listar();
            }
        })
    }

}

function mostrarHistorial(idcita) {
    debugger;
    document.getElementById("lblTitulo").innerHTML = "Historial de la Cita";
    //Oculta el boton por su id
    document.getElementById("btnGuardarObservacion").style.display = "none";
    document.getElementById("formObservacion").style.display = "none";
    document.getElementById("formAsignar").style.display = "none";
    document.getElementById("btnGuardarAsignacion").style.display = "none";
    //Llamamos a nuestro controlador
    $.get("Cita/mostrarHistorialCita/?idcita=" + idcita, function (data) {
        crearListadoDeCitas(["Nom Estado", "Persona", "Fecha", "Observación"], data, "tablaHistorial", "tablaHisto", false);
    })
}


function limpiar() {
    var elementosConClaseLimpiar = document.getElementsByClassName("limpiar");
    var nelementos = elementosConClaseLimpiar.length;
    for (var i = 0; i < nelementos; i++) {
        elementosConClaseLimpiar[i].value = "";
    }

    document.getElementById("divErrores").innerHTML = "";
}

function filtrarCitaPorEstado() {
    var idestado = document.getElementById("cboEstadoCita").value;
    $.get("Cita/filtrarCitasPorEstado/?idEstado=" + idestado, function (data) {
        crearListadoDeCitas(["IdCita", "T.Mascota", "F.Enfermedad", "Nom Usuario", "Estado Cita"], data, "tablaCita", "tabla", true);
    })
}

function GuardarObservacion() {
    var observacion = document.getElementById("txtmotivo").value;
    var idcita = document.getElementById("txtCitaPopup").value;
    if (observacion.trim() == "") {
        alert("Debe ingresar un motivo");
        return;
    }

    $.get("Cita/cambiarEstado/?idCita=" + idcita + "&idestadoACambiar=5&observacion=" + observacion, function (data) {
        if (data == 0) {
            alert("Ocurrio un error");
        }
        else {
            alert("Se anulo correctamente");
            listar();
            document.getElementById("btnCerrar").click();
        }
    })
}

function GuardarAsignacion() {
    //Obtenemos id de la cita
    var idcita = document.getElementById("txtCitaPopup").value;
    //Obtenemois el id del doctor
    var iiddoctor = document.getElementById("cboDoctor").value;
    //Obtenemos la fecha de atencion
    var fecha = document.getElementById("txtfechaAtencion").value;
    //Obtenemos el precio de la consulta
    var precio = document.getElementById("txtprecio").value;

    if (iiddoctor == "") {
        alert("Debe seleccionar un doctor");
        return;
    }

    if (fecha == "") {
        alert("Debe indicar una fecha de cita");
        return;
    }

    if (precio == "") {
        alert("Debe indicar el precio");
        return;
    }

    $.get("Cita/cambiarEstado/?idCita=" + idcita +
        "&idestadoACambiar=3"+
        "&IIDDOCTORASIGNACITAUSUARIO=" + iiddoctor +
        "&DFECHACITA" + fecha + "&PRECIOATENCION" + precio, function (data) {
        if (data == 0) {
            alert("Ocurrio un error");
        }
        else {
            alert("Se encuentra en el estado de revision correctamente");
            listar();
            document.getElementById("btnCerrar").click();
        }
    })
}
