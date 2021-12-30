$('#txtFechaNacimiento').datepicker({ dateFormat: 'dd-mm-yy' })

//Comprobamos que radio buton esra chekeado
function comprobarSexo() {
    var rbMasculino = document.getElementById("rbMasculino").checked;
    //Comrpobamos el radio buton
    if (rbMasculino == true) {
        idSexo = 1;
    } else {
        idSexo = 2;
    }
    /*alert(idSexo)*/
    return idSexo;
}

//Funcion para validar los datos del formulario de registro
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

//Metodo para limpiar los campos del formulario de registro
function limpiar() {
    //Obtenemos los elementos con clase limpiar
    var elementosConClaseLimpiar = document.getElementsByClassName("limpiar");
    //Obtenemos el numero de elementos
    var nelementos = elementosConClaseLimpiar.length;
    //Recorremos la lista de lementos con clase limpiar
    for (var i = 0; i < nelementos; i++) {
        elementosConClaseLimpiar[i].value = "";
    }
}

//Metodo para guardar personas
function GuardarPersonaRegistro() {
    //Llamada al metodo de validar los datos
    var objeto = validarDatos();
    var esCorrecto = objeto.exito;
    if (esCorrecto == false) {
        document.getElementById("divErroresRegistro").innerHTML = "<ol>" + objeto.mensaje + "</ol>";
        return;
    }


    //Obtenemos todos los valores de los controles
    var nombre = document.getElementById("txtnombre").value;
    var apPaterno = document.getElementById("txtapPaterno").value;
    var apMaterno = document.getElementById("txtapMaterno").value;
    var telefono = document.getElementById("txttelefono").value;
    var email = document.getElementById("txtemail").value;
    var fechaNacimiento = document.getElementById("txtFechaNacimiento").value;
    /*var idSexo = document.getElementById("comboSexoPersonasFormulario").value;*/
    var idSexo = 1;
    //var rbMasculino = document.getElementById("rbMasculino").checked;
    ////Comrpobamos el radio buton
    //if (rbMasculino == true) {
    //    idSexo = 1;
    //} else {
    //    idSexo = 2;
    //}
    comprobarSexo();
    
   

    var nombreUsuario = document.getElementById("txtnombreUsuario").value;
    var contraseña = document.getElementById("txtcontraseña").value;

    //Enviar los datos como un form Data
    var datosAEnviar = new FormData();

    datosAEnviar.append("NOMBRE", nombre);
    datosAEnviar.append("APPATERNO", apPaterno);
    datosAEnviar.append("APMATERNO", apMaterno);
    datosAEnviar.append("TELEFONO", telefono);
    datosAEnviar.append("CORREO", email);
    datosAEnviar.append("FECHANACIMIENTO", fechaNacimiento);
    datosAEnviar.append("IIDSEXO", idSexo);
    datosAEnviar.append("BHABILITADO", 1);
    datosAEnviar.append("BTIENEUSUARIO", 1);
    //Creamos dos propiedades nuevas
    datosAEnviar.append("NOMBREUSUARIO", nombreUsuario);
    datosAEnviar.append("CONTRASEÑA", contraseña);


    //Llamada AJAX
    $.ajax({
        type: "POST",
        url: "RegistrarUsuario/GuardarUsuarioPersona",
        data: datosAEnviar,
        contentType: false,
        processData: false,
        success: function (data) {

            //Si la data es cero es que hat error
            if (data == 0) {
                llamarSweetAlertDataError();
            } else if (data == -1) {
                llamarSweetAlertError(nombreUsuario, data);
            }
            else {
                valorOperacion = 1;
                llamarSweetAlertOK(nombreUsuario, valorOperacion)
                limpiar();
                //Redirigimos a login--->"Login" nombre de controlador, si tuviera alguna accion podriamos
                // por ejemplo "Login/metodo"
                document.location.href = "Login";

            }
        }
    })

}



//Mostrar los errores de ok dependiendo de su operacion
function llamarSweetAlertOK(nombre, valorOperacion) {
    debugger;
    if (valorOperacion == 1) {
        swal({
            title: "Se registro el usuario: " + nombre + " a tu base de datos",
            icon: "success",
            button: "Aceptar",
        });
    }
    else {
        swal({
            title: "Se elimino el usuario: " + nombre + " de tu base de datos",
            icon: "success",
            button: "Aceptar",
        });
    }
}

//Mensaje cuando se produce un error 
function llamarSweetAlertError(nombre, valorOperacion) {
    //Estamos eliminando y algo salio mal
    if (valorOperacion == 1) {
        swal("Ocurrio un error!", "You clicked the button!", "error");
    }
    else {
        swal("Ya existe un usuario en base de datos con el mismo nombre de " + nombre, "You clicked the button!", "error");
    }
}

function llamarSweetAlertDataError() {
    swal("Ocurrio un error!", "You clicked the button!", "error");
}