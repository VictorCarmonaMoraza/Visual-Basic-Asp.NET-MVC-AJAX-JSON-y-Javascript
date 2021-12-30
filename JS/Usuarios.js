listar();
llenarTipoUsuario();
llenarPersonas();

function llenarTipoUsuario() {

    $.get("Usuario/listarTipoUsuarios", function (data) {

        crearCombo(data, "cboTipoUsuario");
    })

}

function llenarPersonas() {
    $.get("Usuario/listarPersonasSinUsuario", function (data) {
        crearCombo(data, "cboPersona");
    })

}

function listar() {

    $.get("Usuario/listarUsuarios", function (data) {

        crearListado(["Id Usuario", "Nombre Usuario", "Nombre Completo", "Tipo Usuario"], data, "divUsuarios")
    })

}

function crearCombo(data, comboId) {

    var contenido = "";


    contenido += "<option value=''>--Seleccione</option>"
    //Generando las opciones
    for (var i = 0; i < data.length; i++) {
        contenido += "<option value='" + data[i].Id + "'>" + data[i].Nombre + "</option>"

    }

    document.getElementById(comboId).innerHTML = contenido;



}



function validarDatos(claseAValidar) {
    //Todos los campos estan bien validados
    var mensaje = "";
    var exito = true;
    var obligatorios = document.getElementsByClassName(claseAValidar);
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


function abrirModal(id) {

    limpiar();
    if (id != undefined) {
        document.getElementById("lblTitulo").innerHTML = "Editar Usuario";

        //Ocultar

        document.getElementById("divPersonaOcultar").style.display = "none";
        document.getElementById("divContraOcultar").style.display = "none";

        $.get("Usuario/recuperarInformacion/?id=" + id, function (data) {

            document.getElementById("txtIdUsuario").value = data.IIDUSUARIO;
            document.getElementById("txtNombreUsuarioFormulario").value = data.NOMBREUSUARIO;
            document.getElementById("cboTipoUsuario").value = data.IIDTIPOUSUARIO;


        })



    } else {
        document.getElementById("lblTitulo").innerHTML = "Agregar Usuario";
        document.getElementById("divPersonaOcultar").style.display = "block";
        document.getElementById("divContraOcultar").style.display = "block";
    }

}

function buscarUsuario() {
    var nombre = document.getElementById("txtNombreUsuario").value;
    if (nombre == "") {
        listar();
    } else {
        $.get("Usuario/BuscarUsuarios/?nombreUsuario=" + nombre, function (data) {
            crearListado(["Id Usuario", "Nombre Usuario", "Nombre Completo", "Tipo Usuario"], data, "divUsuarios")

        })
    }
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

            contenido += "<td>";
            //contenido += "<button onclick='abrirModal(" + fila.IIDUSUARIO + ")' class=' btn btn-primary";
            //contenido += " glyphicon glyphicon-edit' data-toggle='modal' data-target='#exampleModal'></button> ";
            //contenido += "<button class='btn btn-danger glyphicon glyphicon-trash' ";
            //contenido += "onclick='Eliminar(" + fila.IIDUSUARIO + ")'></button>";

            //contenido += "</td>";
            contenido += `
                     <td>
                         <i  data-toggle="modal" onclick="abrirModal(${fila.IIDUSUARIO})" data-target="#exampleModal" class= 'btn btn-primary fas fa-edit'></i>

            <i onclick = 'Eliminar(${fila.IIDUSUARIO})' class="btn btn-danger fas fa-trash"></i>

            </td>

            `
            contenido += "</tr>";

        }

        contenido += "</tbody>";
    }

    contenido += "<table>";

    document.getElementById(divId).innerHTML = contenido;
    $('#tabla').DataTable({ searching: false });
}



function Eliminar(id) {
    //Ocurre cuando das click en Aceptar
    if (confirm("Desea eliminar realmente?") == 1) {

        $.get("Usuario/eliminarUsuario/?id=" + id, function (data) {
            if (data == "") {
                alert("Ocurrio un error");
            } else {
                alert("Se elimino correctamente");
                listar();
            }

        })
    }
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

//Metodo para guardar un usuario
function Guardar() {
    //Capturar los valores

    var tituloPopup = document.getElementById("lblTitulo").innerHTML;

    if (tituloPopup == "Agregar Usuario") {
        var objeto = validarDatos("obligatorio")
        var esCorrecto = objeto.exito;
        if (esCorrecto == false) {
            document.getElementById("divErrores").innerHTML = "<ol>" + objeto.mensaje + "</ol>";
            return;
        }
    } else {
        var objeto = validarDatos("obligatorioedit")
        var esCorrecto = objeto.exito;
        if (esCorrecto == false) {
            document.getElementById("divErrores").innerHTML = "<ol>" + objeto.mensaje + "</ol>";
            return;
        }
    }


    var idUsuario = document.getElementById("txtIdUsuario").value;
    var nombreusuario = document.getElementById("txtNombreUsuarioFormulario").value;
    var contra = document.getElementById("txtContra").value;
    var idpersona = document.getElementById("cboPersona").value;
    var idTipoUsuario = document.getElementById("cboTipoUsuario").value;


    var frm = new FormData();
    frm.append("IIDUSUARIO", idUsuario);
    frm.append("NOMBREUSUARIO", nombreusuario);
    frm.append("CONTRA", contra);
    frm.append("IIDPERSONA", idpersona);
    frm.append("IIDTIPOUSUARIO", idTipoUsuario);
    frm.append("BHABILITADO", 1);


    $.ajax({
        type: "POST",
        url: "Usuario/insertarUsuario",
        data: frm,
        contentType: false,
        processData: false,
        success: function (data) {

            if (data == 0) {
                llamarSweetAlertError(nombreVariableGlobal, 1);
            } else if (data == -1) {
                llamarSweetAlertError(nombreVariableGlobal, data);

            } else {
                alert("Se guardo correctamente");
                document.getElementById("btnCerrar").click();
                listar();
                llenarPersonas();
            }
        }
    })
}

//Mensaje cuando se produce un error 
function llamarSweetAlertError(nombre, valorOperacion) {

    //Estamos eliminando y algo salio mal
    if (valorOperacion = 1) {
        swal("Ocurrio un error!", "You clicked the button!", "error");
    }
    else {
        swal("Ya existe un usuario en base de datos con el mismo nombre!", "You clicked the button!", "error");
    }
}

function llamarSweetAlertDataError() {
    swal("Ocurrio un error!", "You clicked the button!", "error");
}

//Mostrar los errores de ok dependiendo de su operacion
function llamarSweetAlertOK(nombre, valorOperacion) {
    if (valorOperacion == 1) {
        swal({
            title: "Se agrego la mascota " + nombre + " a tu base de datos",
            icon: "success",
            button: "Aceptar",
        });
    }
    else {
        swal({
            title: "Se elimino la mascota " + nombre + " de tu base de datos",
            icon: "success",
            button: "Aceptar",
        });
    }
}