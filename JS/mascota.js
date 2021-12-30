$('#txtFechaNac').datepicker({ dateFormat: 'dd-mm-yy' })
listarMascotas();

function listarMascotas() {
    $.get("Mascota/listarMascotas", function (data) {

        crearListado(["Id Mascota", "Nombre", "Altura", "Ancho", "Nombre Sexo", "Tipo Mascota"],
            data, "contenidoMascotas")
    })
}

$.get("Mascota/listarTipoMascotas", function (data) {

    crearCombo(data, "comboTipoMascota");
    crearCombo(data, "cboTipoMascotaFormulario");
})

$.get("Mascota/listarComboSexo", function (data) {
    crearCombo(data, "cboSexo");
})

function crearCombo(data, comboId) {
    var contenido = "";

    contenido += "<option value=''>--Seleccione</option>"
    //Generando las opciones
    for (var i = 0; i < data.length; i++) {
        contenido += "<option value='" + data[i].Id + "'>" + data[i].Nombre + "</option>";

    }

    document.getElementById(comboId).innerHTML = contenido;

}

function validarDatos() {
    //Todos los campos estan bien validados
    var mensaje = "";
    var exito = true;
    var obligatorios = document.getElementsByClassName("obligatorio");
    var nobligatorios = obligatorios.length;
    for (var i = 0; i < nobligatorios; i++) {
        if (obligatorios[i].value == "") {
            exito = false;
            mensaje += "<li>Debe ingresar " + obligatorios[i].name + "</li>";
            //return exito;
        }
    }

    return { exito, mensaje };
}



function limpiar() {

    var elementosConClaseLimpiar = document.getElementsByClassName("limpiar");
    var nelementos = elementosConClaseLimpiar.length;
    for (var i = 0; i < nelementos; i++) {
        elementosConClaseLimpiar[i].value = "";
    }

    /*
    document.getElementById("txtIdTipousuario").value = "";
    document.getElementById("txtNombreTipoUsuario").value = "";
    document.getElementById("txtdescripcion").value = "";
    */
}

function abrirModalMascotas(id) {

    limpiar();
    if (id != undefined) {
        document
            .getElementById("tituloModalMascota").innerHTML = "Editando Mascota";
        $.get("Mascota/reuperarInformacionMascota/?idMascota=" + id, function (data) {
            document.getElementById("txtIdMascota").value = data.IIDMASCOTA;
            document.getElementById("txtNombre").value = data.NOMBRE;
            document.getElementById("cboTipoMascotaFormulario").value = data.IIDTIPOMASCOTA;
            document.getElementById("txtFechaNac").value = data.FECHANACIMIENTO;
            document.getElementById("txtAncho").value = data.ANCHO;
            document.getElementById("txtAltura").value = data.ALTURA;
            document.getElementById("cboSexo").value = data.IIDSEXO;

        })

    } else {
        document
            .getElementById("tituloModalMascota").innerHTML = "Agregando Mascota";
    }

}
var nombreVariableGlobal = "";
//Metodo que guarda una mascota en base de datos 
function Guardar() {
    var objeto = validarDatos()
    var esCorrecto = objeto.exito;
    if (esCorrecto == false) {
        document.getElementById("divErroresFormularioMascota").innerHTML = "<ol>" + objeto.mensaje + "</ol>";
        return;
    }
    //Obtenemos los datos del formulario
    var id = document.getElementById("txtIdMascota").value;
    var nombre = document.getElementById("txtNombre").value;
    var iidtipo = document.getElementById("cboTipoMascotaFormulario").value;
    var fechaNac = document.getElementById("txtFechaNac").value;
    var ancho = document.getElementById("txtAncho").value;
    var alto = document.getElementById("txtAltura").value;
    var iidsexo = document.getElementById("cboSexo").value;
    nombreVariableGlobal = nombre;

    var frm = new FormData();
    frm.append("IIDMASCOTA", id);
    frm.append("NOMBRE", nombre);
    frm.append("IIDTIPOMASCOTA", iidtipo);
    frm.append("FECHANACIMIENTO", fechaNac);
    frm.append("ANCHO", ancho);
    frm.append("ALTURA", alto);
    frm.append("IIDSEXO", iidsexo);
    frm.append("BHABILITADO", 1);

    $.ajax({
        type: "POST",
        url: "Mascota/GuardarMascota",
        data: frm,
        contentType: false,
        processData: false,
        success: function (data) {

            if (data == 0) {
                llamarSweetAlertDataError();
            } else {
                llamarSweetAlertOK(nombreVariableGlobal, 1);
                document.getElementById("btncerrar").click();
                listarMascotas();
            }
        }
    })
}


function crearListado(cabeceras, data, divId) {

    var contenido = "";
    contenido += "<table id='tabla' class='table'>";
    //Las cabeceras
    contenido += "<thead>";
    contenido += "<tr>";
    for (var i = 0; i < cabeceras.length; i++) {
        contenido += "<td>" + cabeceras[i] + "</td>"
    }
    contenido += "<td>Operaciones</td>";
    contenido += "</tr>";

    contenido += "</thead>";
    if (data.length > 0) {
        var propiedadesObjeto = Object.keys(data[0]);

        contenido += "<tbody>";

        var fila;
        for (var i = 0; i < data.length; i++) {
            fila = data[i];
            contenido += "<tr>";

            for (var j = 0; j < propiedadesObjeto.length; j++) {
                var nombrePropiedad = propiedadesObjeto[j];
                contenido += "<td>" + fila[nombrePropiedad] + "</td>";
            }

            //contenido += "<td>";
            //contenido += "<button onclick='abrirModal(" + fila.IIDMASCOTA + ")' class=' btn btn-primary";
            //contenido += " glyphicon glyphicon-edit' data-toggle='modal' data-target='#exampleModal'></button> ";
            //contenido += "<button class='btn btn-danger glyphicon glyphicon-trash' ";
            //contenido += "onclick='Eliminar(" + fila.IIDMASCOTA + ")'></button>";

            contenido += "<td>";
            contenido += "<i onclick='abrirModalMascotas(" + fila.IIDMASCOTA + ")' class= 'btn btn-primary fas fa-edit' data-toggle='modal' data-target='#exampleModal'></i> ";
            contenido += "<i class='btn btn-danger fas fa-trash' ";
            contenido += "onclick='Eliminar(" + fila.IIDMASCOTA + ")'></i>"
            contenido += "</td>";

            contenido += "</td>";

            contenido += "</tr>";

        }

        contenido += "</tbody>";
    }

    contenido += "<table>";

    document.getElementById(divId).innerHTML = contenido;
    $('#tabla').DataTable({ searching: false });
}

//Elimina una mascota por su id
function Eliminar(id) {
    nombreVariableGlobal = document.getElementById("txtNombre").value;
    $.get("Mascota/eliminarMascota/?idMascota=" + id, function (data) {
        if (data == 0) {
            llamarSweetAlertError(nombreVariableGlobal, 1);

        } else {
            listarMascotas();
            llamarSweetAlertOK(nombreVariableGlobal, 2);
        }
    })
}

function Eliminar(id) {
    if (confirm("Desea eliminar a mascota?") == 1){
        $.get("Mascota/Eliminar/?id=" + id, function (data) {
            if (data == 0) {
                alert("Ocurrio un error");
            } else {
                ("Se elimino correctamente");
                listarMascotas();
            }
        })
    }
}

//Filtrar mascota por tipo
function filtrarMascotaPorTipo() {
    var idTipo = document.getElementById("cboTipoMascota").value;
    if (idTipo == "") {
        listarMascotas();
    } else {
        $.get("Mascota/filtrarMascotaPorTipo/?iidTipoMascota=" + idTipo, function (data) {
            crearListado(["Id Mascota", "Nombre", "Altura", "Ancho", "Nombre Sexo", "Tipo Mascota"],
                data, "contenidoMascotas")
        })
    }
}

//Mostrar los errores de ok dependiendo de su operacion
function llamarSweetAlertOK(nombre, valorOperacion) {
    if (valorOperacion == 1) {
        swal({
            title: "Se agrego la mascota " + nombre + " a tu base de datos",
            icon: "success",
            button: "Aceptar",
        });
    } else {
        swal({
            title: "Se elimino la mascota " + nombre + " de tu base de datos",
            icon: "success",
            button: "Aceptar",
        });
    }
}

//Mensaje cuando se produce un error 
function llamarSweetAlertError(nombre, valorOperacion) {

    //Estamos eliminando y algo salio mal
    if (valorOperacion = 1) {
        swal("Ocurrio un error!", "You clicked the button!", "error");
    }
}

function llamarSweetAlertDataError() {
    swal("Ocurrio un error!", "You clicked the button!", "error");
}