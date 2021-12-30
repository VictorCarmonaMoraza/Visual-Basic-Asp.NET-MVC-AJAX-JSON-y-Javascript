
//Funcion para obtener usuario y contraseña
function login() {
    //Obtenemos nuestro usuario
    var usuario = document.getElementById("txtusu").value;
    //Obtenemos la contraseña
    var contraseña = document.getElementById("txtcontraseña").value;

    if (usuario == "" || contraseña == "") {
        alert("Necesitas completar los datos de usuario y contraseña");
        return;
    } else {
        //Llamamos a nuestro metodo del controlador
        $.get("Login/ingresarLogin/?nombreUsuario=" + usuario + "&password=" + contraseña, function (respuestaControlador) {
            //si la respuesta es cero es que no hay coincidencias
            if (respuestaControlador == 0) {
                alert("Contraseña o usuario incorrecto");
            } else {
                //Lo mandaremos a una pagina mediante location
                document.location.href = 'PaginaPrincipal';
            }
        })
    }

    
}