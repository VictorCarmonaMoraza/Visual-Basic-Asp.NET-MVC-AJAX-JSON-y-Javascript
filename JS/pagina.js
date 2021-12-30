
listarPaginas();

function listarPaginas() {
    //Llamamos al metodo del controlador Medicamentos para que nos devuelva la data que contendra el listado de medicamentos
    $.get("Pagina/listarPaginas", function (respuestaServidor) {
        //cabeceras para la tabla
        crearListadoDePaginas(["Id Pagina", "Mensaje", "Accion", "Controlador"], respuestaServidor, "contenidoPagina");
    })
}

//Metodo para pintar la tabla del listado de personas
function crearListadoDePaginas(cabeceras, data, divId) {

    var contenido = "";
    contenido += "<table id='tablaPaginas' class='table'>";
    //Las cabeceras
    contenido += "<thead>";
    //Filas
    contenido += "<tr>";
    //Recorremos el array de las cebeceras
    for (var i = 0; i < cabeceras.length; i++) {
        contenido += "<td>" + cabeceras[i] + "</td>";
    }

    contenido += "<td>Operaciones</td>"
    contenido += "</tr>";
    contenido += "</thead>";
    var propiedadesObjecto = Object.keys(data[0]);
    //Contenido de la tabla con los datos obtenidos de la consulta
    contenido += "<tbody>";
    //Recorremos la data
    var fila;
    for (var i = 0; i < data.length; i++) {
        fila = data[i];
        contenido += "<tr>";
        for (var j = 0; j < propiedadesObjecto.length; j++) {
            //Sacar el nombre de la propiedas
            var nombrePropiedad = propiedadesObjecto[j];
            contenido += "<td>" + fila[nombrePropiedad] + "</td>";
        }
        contenido += "<td>";
        contenido += "<i onclick='abrirModalPagina(" + fila.IIDPAGINA + ")' class= 'btn btn-primary fas fa-edit' data-toggle='modal' data-target='#exampleModal'></i> ";
        contenido += "<i class='btn btn-danger fas fa-trash' ";
        contenido += "onclick='eliminarPagina(" + fila.IIDPAGINA + ")'></i>"
        contenido += "</td>";

        contenido += "</tr>";
    }
    contenido += "</tbody>";
    //Inyectamos en el div el contenido de la tabla
    document.getElementById(divId).innerHTML = contenido;
    //Paginamos
    $('#tablaPaginas').DataTable({ searching: false });
}

//Funcion para guardar una pagina
function Guardar() {
    //Nos traemos los datos por su id
    var idPagina = document.getElementById("txtIdPagina").value;
    var mensaje = document.getElementById("txtmensaje").value;
    var accion = document.getElementById("txtaccion").value;
    var controlador = document.getElementById("txtcontrolador").value;

    //La respuesta que traeria el validar datos seria Ej={exito:true, mensaje:'<li></li>'}
    var objeto = validarDatosPagina();
    var validacionesDeCampos = objeto.exito;
    //si la validacion es false es que las validaciones no estan siendo correctas
    if (validacionesDeCampos == false) {
        //alert("Ocurrio un error")
        document.getElementById("divErroresPaginas").innerHTML = "<ol>" + objeto.mensaje + "</ol>";
        //return para que no se ejecute nada de abajo
        return;
    }

    //Creamos la pgian con los valores obtenidos de la modal
    var objetoPagina = new FormData();
    objetoPagina.append("IIDPAGINA", idPagina);
    objetoPagina.append("MENSAJE", mensaje);
    objetoPagina.append("ACCION", accion);
    objetoPagina.append("CONTROLADOR", controlador);
    objetoPagina.append("BHABILITADO", 1);

    //Llamada ajax para la creacion de la pagina
    $.ajax({
        type: "POST",
        url: "Pagina/guardarPaginas",
        contentType: false,
        processData: false,
        data: objetoPagina,
        success: function (respuestaControlador) {
            //Si la informacion es vacio
            if (respuestaControlador == 0) {
                alert("Ocurrio un error");
            } else if (respuestaControlador==-1) {
                alert("Ya existe ese nombre de mensaje en base de datos");
            }
            else
                alert("Se registro correctamente");
            listarPaginas();
            //Cerramos la modal
            document.getElementById("btncerrar").click();
        }
    })
}

//Funcion para eliminar una pagina
function eliminarPagina(idFront) {
    //Llamamos al metodo del controlador para elimianr un elemento por su id
    $.get("Pagina/eliminarPagina/?id=" + idFront, function (respuestaDelControlador) {
        if (respuestaDelControlador == 0) {
            alert("Ocurrio un error");
        } else {
            alert("Se elimino correctamente");
            listarPaginas();
        }
    })
}

//Funcion para limpiar los campos de un modal
function limpiarModalPagina() {
    //Nos traemos los elementos con la clase limpiar
    var elementosConClaseLimpiar = document.getElementsByClassName("limpiar");
    //Obtenemos el numero de elementos con la clase limpiar
    var numeroElementosConClaseLimpiar = elementosConClaseLimpiar.length;
    //Recorremos los elementos con la clase limpiar para ponerlos a vacio
    for (var i = 0; i < numeroElementosConClaseLimpiar; i++) {
        elementosConClaseLimpiar[i].value = "";
    }
}

//Funcion para abrir modal de la pagina
function abrirModalPagina(idFront) {

    //Cada vez que se abra el modal se limpiaran los campos
    limpiarModalPagina();
    document.getElementById("divErroresPaginas").innerHTML = ""
    //si el idFront tiene valor es que quiero editarlo
    if (idFront != undefined) {
        //Asignamos titulo a la ventana modal
        document.getElementById("tituloModalPagina").innerHTML = "Editando Pagina";
        $.get("Pagina/recuperarInformacionPagina/?id=" + idFront, function (respuestaControlador) {
            //Asigno los datos de la consulta a la base de datos a sus campos en el front
            document.getElementById("txtIdPagina").value = respuestaControlador[0].IIDPAGINA;
            document.getElementById("txtmensaje").value = respuestaControlador[0].MENSAJE;
            document.getElementById("txtaccion").value = respuestaControlador[0].ACCION;
            document.getElementById("txtcontrolador").value = respuestaControlador[0].CONTROLADOR;

        })
    }
    else {
        document.getElementById("tituloModalPagina").innerHTML = "Creando una Pagina";
    }
}

//Funcion para filtrar las paginas por contenido
function filtrarPagina() {
    //Obtenemos el valor del input
    var mensajeBusqueda = document.getElementById("txtMensajeBusqueda").value;

    //Si el mensaje de busqueda esta vacio devolvemos toda la lista
    if (mensajeBusqueda == "") {
        listarPaginas();
    }
    else {
        $.get("Pagina/buscarPaginas/?mensaje=" + mensajeBusqueda, function (respuestaControlador) {
            //cabeceras para la tabla
            crearListadoDePaginas(["Id Pagina","Mensaje","Accion","Controlador"], respuestaControlador, "contenidoPagina");
        })
    }
}

function validarDatosPagina() {
    var exito = true;
    var mensaje = "";
    //Obtenemos todos los campos que tienen la clase obligatoria
    var obligatorios = document.getElementsByClassName("obligatorio");
    //Obtenemos el numero de campos con esa clase
    var nObligatorios = obligatorios.length;
    //recorremos todos los campos 
    for (var i = 0; i < nObligatorios; i++) {
        if (obligatorios[i].value == "") {
            exito = false;
            mensaje += "<li>Debe ingresar " + obligatorios[i].name + "</li>";
        }
    }
    return { exito, mensaje }
}