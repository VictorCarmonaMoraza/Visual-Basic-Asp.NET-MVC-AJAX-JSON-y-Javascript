$("#txtFechaNacimiento").datepicker();
//Llamada ala metodo para que me devuelva todo el listado
listar();

function listar() {
    //Llamamos al metodo de listarPersonas
    $.get("Personas/listaPersonas", function (data) {
        var x = data;
        //Metodo para la funcion de crear listado de personas
        crearListadoPersonas(data);
    })
}


//Lamada al metodo de listarComboSexo
$.get("Personas/listarComboSexo", function (data) {
    //Metodo para la funcion de crear listado de personas
    crearCombo(data, "comboSexo");
    //Rellenamos combo del formualrio
    crearCombo(data, "comboSexoPersonasFormulario");
})



function crearCombo(data, comboId) {

    var contenido = "";

    contenido += "<option value=''>--Seleccione una opcion--</option>";
    //Recorremos la data para generrar las opciones
    for (var i = 0; i < data.length; i++) {
        contenido += "<option value='" + data[i].Id + "'>" + data[i].valor + "</option>";
    }

    //Agregamos los datos al combo de sexo
    document.getElementById(comboId).innerHTML = contenido;
}

//Funcion para validar los datos del formulario
function validarDatos() {
    debugger;
    //Todos los campos estan bien validados
    var mensaje = "";
    var exito = true;
    var obligatorios = document.getElementsByClassName("obligatorio");
    var nobligatorios = obligatorios.length;
    for (var i = 0; i < nobligatorios; i++) {
        exito = false;
        mensaje += "<li>Debe ingresar " + obligatorios[i].name + "</i>";
    }
}

function buscarSexoPorNombre() {
    //Me traigo el valor del combo
    var idSexo = document.getElementById("comboSexo").value;
    if (idSexo == "" || idSexo == undefined) {
        listar();
    } else {
        //Llamamos al metodo de listarPersonas
        $.get("Personas/filtrarPersonaPorSexo/?iidsexo=" + idSexo, function (data) {

            //Metodo para la funcion de crear listado de personas
            crearListadoPersonas(data);
        })
    }
}

function abrirModalPersonas(idFront) {
    //alert(idFront);
    limpiarModalPersona();
    //Si el id viene vacio es que estamos agregando un elemento a la base de datos
    //Si el id viene vacio(o undefined) con datos es que vamos a hacer una edicion
    if (idFront != undefined) {
        document.getElementById("tituloModalPersona").innerHTML = "Editando Persona"
        $.get("Personas/recuperarInformacionPersona/?idPersona=" + idFront, function (respuestaServidor) {
            document.getElementById("txtIdPersona").value = respuestaServidor[0].IIDPERSONA;
            document.getElementById("txtNombre").value = respuestaServidor[0].NOMBRE;
            document.getElementById("txtApPaterno").value = respuestaServidor[0].APPATERNO;
            document.getElementById("txtApMaterno").value = respuestaServidor[0].APMATERNO;
            document.getElementById("txtTelefono").value = respuestaServidor[0].TELEFONO;
            document.getElementById("txtEmail").value = respuestaServidor[0].CORREO;
            document.getElementById("txtFechaNacimiento").value = respuestaServidor[0].FECHANACIMIENTO;
            document.getElementById("comboSexoPersonasFormulario").value = respuestaServidor[0].IIDSEXO;
        })
    } else {
        document.getElementById("tituloModalPersona").innerHTML = "Agregando  Persona"
    }
}

function limpiarModalPersona() {
    //Obtengo todos los elementos por su clase
    var elementosConClaseLimpiar = document.getElementsByClassName("limpiar");
    //Obtengo el nuemro de elementos con la clase limpiar
    var numeroElementos = elementosConClaseLimpiar.length;
    //Recorro todos los elementos y los pongo con valores a vacios
    for (var i = 0; i < numeroElementos; i++) {
        elementosConClaseLimpiar[i].value = "";
    }
}


function crearListadoPersonas(data) {

    var contenido = "";
    //Abro la etiqueta de table
    contenido += "<table id='tablaPersonas' class='table-striped'>";
    //Las cabaceras
    contenido += "<thead>";
    //Definimos la fila de las cabeceras
    contenido += "<tr>";
    //Contenido de las celdas
    contenido += "<td>Id Persona</td>";
    contenido += "<td>Nombre Completo</td>";
    contenido += "<td>Telefono</td>";
    contenido += "<td>Correo Electronico</td>";
    contenido += "<td>Fecha de Nacimiento</td>";
    contenido += "<td>Operaciones</td>";
    //Fin de la fila
    contenido += "</tr>";
    //Fin de las cebeceras
    contenido += "</thead>";

    //Pintamos el contenido
    contenido += "<body>";

    //Recorremos los datos 
    var fila;
    for (var i = 0; i < data.length; i++) {
        //Recojera los datos de cada fila
        fila = data[i];
        //Por cada objeto se forma una fila
        contenido += "<tr>";
        //Por cada dato,es una celda diferente
        contenido += "<td>" + fila.IIDPERSONA + "</td>";
        contenido += "<td>" + fila.NOMBRECOMPLETO + "</td>";
        contenido += "<td>" + fila.TELEFONO + "</td>";
        contenido += "<td>" + fila.CORREO + "</td>";
        contenido += "<td>" + fila.FECHANACIMIENTO + "</td>";
        contenido += `<td>
            
            <i onclick="abrirModalPersonas(${fila.IIDPERSONA})"  data-toggle="modal" data-target="#exampleModal" class= 'btn btn-primary fas fa-edit'></i>

            <i onclick = 'eliminarPersona(${fila.IIDPERSONA})' class="btn btn-danger fas fa-trash"></i>
            
            </td>
        `
        contenido += "</tr>";
    }

    //Fin del contenido
    contenido += "</body>";

    //Cierro la etiqueta de table
    contenido += "</table>";


    //Pintar todo lo que estamos generando en la variable contenido
    document.getElementById("divPersonas").innerHTML = contenido;
    //{ searching: false }-->elimina la caja de etxto del buscador
    $('#tablaPersonas').DataTable({ searching: false });
}

//Funcion que elimina personas por su id
function eliminarPersona(idPersonaFront) {
    if (confirm("Desea eliminar realmente esta persona") == 1) {
        alert(idPersonaFront);
        //$.get("Nombre_Controlador/Funcion_del_Controlador/?_Parametro="+vlor_de_ la_caja_de_texto);
        $.get("Personas/eliminarPersona/?id=" + idPersonaFront, function (data) {
            if (data == 0) {
                alert("Ocurrio un error");
            }
            else {
                alert("Se elimino la persona correctamente");
                listar();
                document.getElementById("btnCerrar").click();
            }
        });
    }

}


//Metodo para guardar personas
function GuardarPersona() {
    //Llamada al metodo de Guardar
    var objeto = validarDatos()
    var esCorrecto = objeto.exito;
    if (esCorrecto == false) {
        document.getElementById("divErroresPersona").innerHTML = "<ol>" + objeto.mensaje + "</ol>";
        return;
    }
    //Obtenemos todos los valores de los controles
    var idPersona = document.getElementById("txtIdPersona").value;
    var nombre = document.getElementById("txtNombre").value;
    var apPaterno = document.getElementById("txtApPaterno").value;
    var apMaterno = document.getElementById("txtApMaterno").value;
    var telefono = document.getElementById("txtTelefono").value;
    var email = document.getElementById("txtEmail").value;
    var fechaNacimiento = document.getElementById("txtFechaNacimiento").value;
    var idSexo = document.getElementById("comboSexoPersonasFormulario").value;

    //Enviar los datos como un form Data
    var datosAEnviar = new FormData();

    datosAEnviar.append("IIDPERSONA", idPersona);
    datosAEnviar.append("NOMBRE", nombre);
    datosAEnviar.append("APPATERNO", apPaterno);
    datosAEnviar.append("APMATERNO", apMaterno);
    datosAEnviar.append("TELEFONO", telefono);
    datosAEnviar.append("CORREO", email);
    datosAEnviar.append("FECHANACIMIENTO", fechaNacimiento);
    datosAEnviar.append("IIDSEXO", idSexo);
    datosAEnviar.append("BHABILITADO", 1);
    datosAEnviar.append("BTIENEUSUARIO", 0);

    //Llamada AJAX
    $.ajax({
        type: "POST",
        url: "Personas/GuardarPersonas",
        data: datosAEnviar,
        contentType: false,
        processData: false,
        success: function (data) {

            //Si la data es cero es que hat error
            if (data == 0) {
                alert("Ha habido un error")
            }
            else {
                alert("Se guardo correctamente");
                listar();
                document.getElementById("btnCerrar").click();

            }
        }
    })

}