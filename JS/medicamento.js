listarMedicamentos();

function listarMedicamentos(){
    //Llamamos al metodo del controlador Medicamentos para que nos devuelva la data que contendra el listado de medicamentos
    $.get("Medicamentos/listarMedicamentos", function (respuestaServidor) {
        //cabeceras para la tabla
        cabeceras = ["Id Medicamento", "Nombre Medicamento", "Concentracion", "Precio", "Fecha de Nacimiento"];
        crearListadoDeMedicamentos(cabeceras, respuestaServidor, "contenidoMedicamentos");
    })
}

//Metodo para pintar la tabla del listado de personas
//function crearListadoDeMedicamentos(cabeceras,data, divId) {

//    var contenido = "";
//    contenido += "<table id='tablaMedicamentos' class='table'>";
//    //Las cabeceras
//    contenido += "<thead>";
//    //Filas
//    contenido += "<tr>";
//    //Recorremos el array de las cebeceras
//    for (var i = 0; i < cabeceras.length; i++) {
//        contenido += "<td>" + cabeceras[i] + "</td>";

//    }
//    contenido += "</tr>";
//    contenido += "</thead>";
//    debugger;
//    var propiedadesObjecto = Object.keys(data[0]);
//    //Contenido de la tabla con los datos obtenidos de la consulta
//    contenido += "<tbody>";
//    //Recorremos la data
//    var fila;
//    for (var i = 0; i < data.length; i++) {
//        fila = data[i];
//        contenido += "<tr>";
//        for (var j = 0; j < propiedadesObjecto.length; j++) {
//            //Sacar el nombre de la propiedas
//            var nombrePropiedad = propiedadesObjecto[j];
//            contenido += "<td>" + fila[nombrePropiedad] + "</td>";
//        }
//        //contenido += "<td>" + fila.IIDMEDICAMENTO + "</td>";
//        //contenido += "<td>" + fila.NOMBRE + "</td>";
//        //contenido += "<td>" + fila.CONCENTRACION + "</td>";
//        //contenido += "<td>" + fila.PRECIO + "</td>";
//        //contenido += "<td>" + fila.STOCK + "</td>";
//        contenido += "</tr>";
//    }
//    contenido += "</tbody>";
//    //Inyectamos en el div el contenido de la tabla
//    document.getElementById(divId).innerHTML = contenido;
//    //Paginamos
//    $('#tablaMedicamentos').DataTable({ searching: false });

//}

//Metodo para pintar la tabla del listado de personas

function crearListadoDeMedicamentos(cabeceras, data, divId) {

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
        contenido += "<i onclick='abrirModalMedicamento(" + fila.IIDMEDICAMENTO + ")' class= 'btn btn-primary fas fa-edit' data-toggle='modal' data-target='#exampleModal'></i> ";
        contenido += "<i class='btn btn-danger fas fa-trash' ";
        contenido += "onclick='eliminarPagina(" + fila.IIDMEDICAMENTO + ")'></i>"
        contenido += "</td>";

        contenido += "</tr>";
    }
    contenido += "</tbody>";
    //Inyectamos en el div el contenido de la tabla
    document.getElementById(divId).innerHTML = contenido;
    //Paginamos
    $('#tablaPaginas').DataTable({ searching: false });
}

function eliminar(idFront) {
    if (confirm("Desea eliminar realmente?") == 1) {
        $.get("Medicamentos/eliminarMedicamento/?id=" + idFront, function (respuestaControlador) {
            if (respuestaControlador == 0) {
                alert("Ha habido un error")
            }
            else {
                alert("Se elimino correctamente");
                listarMedicamentos();
            }
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

//Funcion para guardar una pagina
function GuardarMedicamento() {
    //Nos traemos los datos por su id
    var idMedicamento = document.getElementById("txtIdMedicamento").value;
    var nombreMedicamento = document.getElementById("txtNombreMedicamento").value;
    var concentracionMedicamento = document.getElementById("txtConcentracion").value;
    var precioMedicamento = document.getElementById("txtPrecio").value;
    var stockMedicamento = document.getElementById("txtStock").value;

    //La respuesta que traeria el validar datos seria Ej={exito:true, mensaje:'<li></li>'}
    var objeto = validarDatosPagina();
    var validacionesDeCampos = objeto.exito;
    //si la validacion es false es que las validaciones no estan siendo correctas
    if (validacionesDeCampos == false) {
        //alert("Ocurrio un error")
        document.getElementById("divErrorMedicamento").innerHTML = "<ol>" + objeto.mensaje + "</ol>";
        //return para que no se ejecute nada de abajo
        return;
    }

    //Creamos la pgian con los valores obtenidos de la modal
    var objetoMedicamento = new FormData();
    objetoMedicamento.append("IIDMEDICAMENTO", idMedicamento);
    objetoMedicamento.append("NOMBRE", nombreMedicamento);
    objetoMedicamento.append("CONCENTRACION", concentracionMedicamento);
    objetoMedicamento.append("PRECIO", precioMedicamento);
    objetoMedicamento.append("STOCK", stockMedicamento);
    objetoMedicamento.append("BHABILITADO", 1);

    //Llamada ajax para la creacion de la pagina
    $.ajax({
        type: "POST",
        url: "Medicamentos/guardarMedicamento",
        contentType: false,
        processData: false,
        data: objetoMedicamento,
        success: function (respuestaControlador) {
            //Si la informacion es vacio
            if (respuestaControlador == 0) {
                alert("Ocurrio un error");
            } else
                alert("Se registro correctamente");
            listarMedicamentos();
            //Cerramos la modal
            document.getElementById("btncerrar").click();
        }
    })
}

//Funcion para abrir modal de la pagina
function abrirModalMedicamento(idFront) {
    //Cada vez que se abra el modal se limpiaran los campos
    limpiarModalMedicamento();
    document.getElementById("divErrorMedicamento").innerHTML = "";
    //si el idFront tiene valor es que quiero editarlo
    if (idFront != undefined) {
        //Asignamos titulo a la ventana modal
        document.getElementById("tituloModalMedicamento").innerHTML = "Editando Medicamento";
        $.get("Pagina/recuperarInformacionMedicamento/?id=" + idFront, function (respuestaControlador) {
            //Asigno los datos de la consulta a la base de datos a sus campos en el front
            document.getElementById("txtIdMedicamento").value = respuestaControlador[0].IIDMEDICAMENTO;
            document.getElementById("txtConcentracion").value = respuestaControlador[0].CONCENTRACION;
            document.getElementById("txtNombreMedicamento").value = respuestaControlador[0].NOMBRE;
            document.getElementById("txtPrecio").value = respuestaControlador[0].PRECIO;
            document.getElementById("txtStock").value = respuestaControlador[0].STOCK;
        })
    }
    else {
        document.getElementById("tituloModalMedicamento").innerHTML = "Creando un medicamento";
    }
}

//Funcion para limpiar los campos de un modal
function limpiarModalMedicamento() {
    //Nos traemos los elementos con la clase limpiar
    var elementosConClaseLimpiar = document.getElementsByClassName("limpiar");
    //Obtenemos el numero de elementos con la clase limpiar
    var numeroElementosConClaseLimpiar = elementosConClaseLimpiar.length;
    //Recorremos los elementos con la clase limpiar para ponerlos a vacio
    for (var i = 0; i < numeroElementosConClaseLimpiar; i++) {
        elementosConClaseLimpiar[i].value = "";
    }
}
