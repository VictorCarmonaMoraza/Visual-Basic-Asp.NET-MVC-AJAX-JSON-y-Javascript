listar();

function listar() {
    $.get("TipoUsuario/listarTipoUsuario", function (data) {
        crearListado(data);
    })
}

function crearListado(data) {

    var contenido = "";
    contenido += "<table id='tabla' class='table'>";
    //Las cabeceras
    contenido += "<thead>";
    contenido += "<tr>";
    contenido += "<td>Id Tipo Usuario</td>";
    contenido += "<td>Nombre Tipo Usuario</td>";
    contenido += "<td>Descripcion</td>";
    contenido += "<td>Operaciones</td>";

    contenido += "</tr>";

    contenido += "</thead>";

    contenido += "<tbody>";

    var fila;
    for (var i = 0; i < data.length; i++) {
        fila = data[i];
        contenido += "<tr>";
        contenido += "<td>" + fila["IIDTIPOUSUARIO"] + "</td>";
        contenido += "<td>" + fila["NOMBRE"] + "</td>";
        contenido += "<td>" + fila["DESCRIPCION"] + "</td>";
        contenido += `
                     <td>
                         <i  data-toggle="modal" onclick="abrirModal(${fila.IIDTIPOUSUARIO})" data-target="#exampleModal" class= 'btn btn-primary fas fa-edit'></i>

            <i onclick = 'eliminar(${fila.IIDTIPOUSUARIO})' class="btn btn-danger fas fa-trash"></i>

            </td>

            `
        contenido += "</tr>";

    }

    contenido += "</tbody>";


    contenido += "<table>";

    document.getElementById("divTipoUsuario").innerHTML = contenido;
    $('#tabla').DataTable({ searching: false });
}

function eliminar(id) {

    if (confirm("Desea eliminar realmente") == 1) {

        $.get("TipoUsuario/eliminar/?id=" + id, function (data) {
            if (data == 0) {
                alert("Ocurrio un error");
            } else {
                alert("Se elimino correctamente");
                listar();
            }
        })
    }


}

//Variable global que utilizaremos para pasar el id
var globalid;
function abrirModal(idFront) {
    //Cada vez que se abre el modal de guardara el id  Front en la variable global
    globalid = idFront;
    document.getElementById("divErrores").innerHTML = "";
    limpiar();
    //Listar o traer las paginas
    $.get("TipoUsuario/listarPaginas", function (data) {

        listarPaginas(data);
    })

    //Siempre que sea undefined es un editar
    if (globalid != undefined) {
        document.getElementById("lblTitulo").innerHTML = "Editando Tipo Usuario";
        $.get("TipoUsuario/recuperarInformacionTipoUsuario/?id=" + globalid, function (data) {

            document.getElementById("txtIdTipousuario").value = data[0].IIDTIPOUSUARIO;
            document.getElementById("txtNombreTipoUsuario").value = data[0].NOMBRE;
            document.getElementById("txtdescripcion").value = data[0].DESCRIPCION;

        })
        //Llamariamos a nuestra funcion para recuperar los ids de los check
        $.get("TipoUsuario/recuperarCheckMarcados/?id=" + idFront, function (data) {
            //Si tenemos datos entonces hacemos el recorrido de los datos
            if (data.length > 0) {
                //Recorremos la data
                for (var i = 0; i < data.length; i++) {
                    //chk1 chk2 chk3
                    //Pintamos os check
                    document.getElementById("chk" + data[i].IIDPAGINA).checked = true;
                }
            }
        })

    } else {
        document.getElementById("lblTitulo").innerHTML = "Agregando Tipo Usuario";

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


function BuscarTipoUsuario() {
    var nombreTipoUsu = document.getElementById("txtnombreTipoUsu").value
    $.get("TipoUsuario/filtrarTipoUsuario/?nombreTipoUsuario=" + nombreTipoUsu, function (data) {
        crearListado(data);
    })
}

function Guardar() {

    var idTipo = document.getElementById("txtIdTipousuario").value;
    var nombre = document.getElementById("txtNombreTipoUsuario").value;
    var descripcion = document.getElementById("txtdescripcion").value;
    //{exito:true,mensaje:'<li></li>'}
    var objeto = validarDatos()
    var esCorrecto = objeto.exito;
    if (esCorrecto == false) {
        document.getElementById("divErrores").innerHTML = "<ol>" + objeto.mensaje + "</ol>";
        return;
    }
    /*
    if (nombre == "") {
        alert("Ingrese nombre");
        return;
    }

    if (descripcion == "") {
        alert("Ingrese descripcion");
        return;
    }*/
    //Voy a recorrer todos los elementos que tienen la clase checkbox
    var ids = "";
    var cbos = "";
    var elementos = document.getElementsByClassName("checkbox");
    var nelementos = elementos.length;
    for (var i = 0; i < nelementos; i++) {
        //De esta manera sabemos si tenemos un checkbox seleccionado
        if (elementos[i].checked == true) {
            //sacamos por alerta todos los id de los elementos seleccionados
           /* alert(elementos[i].id);*/
            //Obtenemos el id de la pagina reemplzando el chk por vacio
             var idPagina = elementos[i].id.replace("chk", "");
            //Concatenamos todos los ids obtenidos
            ids += elementos[i].id.replace("chk", "");
            //Cada id estara separado por un asterisco
            ids += "*";
            cbos += document.getElementById("cbo" + idPagina).value;
            cbos += "*";
        }
    }
    //Si hay elementos o hay ids
    if (ids.length != 0) {
        //quitamos el ultimo asterizco
        ids = ids.substring(0, ids.length - 1);
        cbos = cbos.substring(0, cbos.length - 1);

    }


//Construye el objeto
    var frmData = new FormData();
    frmData.append("IIDTIPOUSUARIO", idTipo);
    frmData.append("NOMBRE", nombre);
    frmData.append("DESCRIPCION", descripcion);
    frmData.append("BHABILITADO", 1);
    //Propiedad adicionalpara pasar la back
    frmData.append("IDS", ids);
    frmData.append("CBOS", cbos);
    $.ajax({
        type: "POST",
        url: "TipoUsuario/insertarTipoUsuario",
        data: frmData,
        contentType: false,
        processData: false,

        success: function (data) {
            if (data == 0) {
                alert("Ocurrio un error");
            }
            else if (data == -1) {
                alert("Ya existe en base de datos ese nombre");

            }
            else {
                /*alert("Se guardo correctamente");*/
                llamarSweetAlertOK(nombre,1)
                listar();
                document.getElementById("btnCerrar").click();
            }
        }
    })

}

function listarPaginas(rpta) {

    var contenido = "<table class='table'>";
    //Cabecera
    contenido += "<thead>";
    contenido += "<tr>";
    contenido += "<td>Seleccionar</td>";
    contenido += "<td>Nombre</td>";
    contenido += "<td>Tipo Vista</td>";
    contenido += "</tr>";

    contenido += "</thead>";
    //Contenido
    contenido += "<tbody>";

    contenido += "</tbody>";

    for (var i = 0; i < rpta.length; i++) {
        contenido += "<tr>";
        contenido += "<td><input class='checkbox' type='checkbox' id='chk" + rpta[i].IIDPAGINA + "' /></td>";
        contenido += "<td>" + rpta[i].MENSAJE + "</td>";
        contenido += "<td><select id='cbo" + rpta[i].IIDPAGINA + "'><option value='1'>Vista Total</option>";
        contenido += "<option value='2'>Vista Usuario</option></select></td>"
        contenido += "</tr>";

    }


    contenido += "</table>";
    document.getElementById("divPaginas").innerHTML = contenido;

    if (globalid != undefined) {
        ////Llamariamos a nuestra funcion para recuperar los ids de los check
        $.get("TipoUsuario/recuperarCheckMarcados/?id=" + globalid, function (data) {
            //recorrenmos la data
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    //chk1 chk2 chk3
                    document.getElementById("chk" + data[i].IIDPAGINA).checked = true;
                    document.getElementById("cbo" + data[i].IIDPAGINA).value = data[i].IIDVISTA
                }
            }
        })
    }
}

//Mostrar los errores de ok dependiendo de su operacion
function llamarSweetAlertOK(nombre, numeroOperacion) {
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
