@Code
    ViewData("Title") = "Index"
End Code
<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.11.0/css/jquery.dataTables.css">
<link href="~/Content/jquery-ui.min.css" rel="stylesheet" />

<h2>Registro Usuario</h2>

<!--<div class="modal-body">-->
<!--Id Persona-->
<!--<div class="form-group">
    <label>Ingrese Id Persona</label>
    <input readonly id="txtIdPersona" class="form-control limpiar" />
</div>-->
<!--NOTA: sino pones type="text" por defecto es text-->
<!--Nombre-->
<!--<div class="form-group">
    <label>Nombre</label>
    <input type="text" id="txtNombre" class="form-control limpiar" />
</div>-->
<!--Apellido Paterno-->
<!--<div class="form-group">
    <label>Apellido Paterno</label>
    <input type="text" id="txtApPaterno" class="form-control limpiar" />
</div>-->
<!--Apellido Materno-->
<!--<div class="form-group">
    <label>Apellido Materno</label>
    <input type="text" id="txtApMaterno" class="form-control limpiar" />
</div>-->
<!--Telefono-->
<!--<div class="form-group">
    <label>Telefono</label>
    <input type="number" id="txtTelefono" class="form-control limpiar" />
</div>-->
<!--Email-->
<!--<div class="form-group">
    <label>Email</label>
    <input type="email" id="txtEmail" class="form-control limpiar" />
</div>-->
<!--Fecha de Nacimiento-->
<!--<div class="form-group">
    <label>Fecha de Nacimiento</label>
    <input type="text" id="txtFechaNacimiento" class="form-control limpiar" />
</div>-->
<!--Fecha de Nacimiento-->
<!--<div class="form-group">
    <label>Sexo</label>
    <select id="comboSexoPersonasFormulario" class="form-control limpiar"></select>
</div>-->
<!--Nombre de usuario-->
<!--<div class="form-group">
    <label>Nombre de Usuario</label>
    <input type="text" id="txtnombreusuario" class="form-control limpiar" />
</div>-->
<!--Contraseña de usuario-->
<!--<div class="form-group">
        <label>Contraseña</label>
        <input type="password" id="txtcontra" class="form-control limpiar" />
    </div>

    <div class="form-group">
        <button type="button" class="btn btn-secondary">Volver Login</button>
        <button type="button" onclick="GuardarPersona()" class="btn btn-primary">Guardar</button>
    </div>
</div>-->

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <title>Document</title>
    <link href="~/Content/formularioregistro.css" rel="stylesheet" />
</head>
<body>
    <div class="container">
        <h1 class="title">Formulario Registro</h1>
        <br />
        <br />
        <div class="card">
            <!--Id de persona-->
            @*<div class="form-group">
                <label>Ingrese Id Persona:</label>
                <input readonly id="txtIdPersona" name="idPersona" class="form-control" value="" />
            </div>*@
            <!--Nombre persona-->
            <div class="form-group">
                <label>Nombre:</label>
                <input id="txtnombre" name="Nombre" type="text" class="form-control limpiar obligatorio" value="" />
            </div>
            <!--Apellido paterno-->
            <div class="form-group">
                <label>Apellido Paterno:</label>
                <input id="txtapPaterno" name="Apellido paterno de la persona" type="text" class="form-control limpiar obligatorio" value="" />
            </div>
            <!--Apellido materno-->
            <div class="form-group">
                <label>Apellido Materno:</label>
                <input id="txtapMaterno" name="Apellido materno de la persona" type="text" class="form-control limpiar obligatorio" value="" />
            </div>
            <!--Telefono-->
            <div class="form-group">
                <label>Telefono:</label>
                <input id="txttelefono" name="Telefono" type="number" class="form-control limpiar obligatorio" value="" />
            </div>
            <!--Email-->
            <div class="form-group">
                <label>Email:</label>
                <input id="txtemail" name="Email" type="email" class="form-control limpiar obligatorio" value="" />
            </div>
            <!--Fecha Nacimiento-->
            <div class="form-group">
                <label>Fecha de Nacimiento:</label>
                <input id="txtFechaNacimiento" name="Fecha Nacimiento" type="text" class="form-control limpiar obligatorio" value="" />
            </div>
            <!--Sexo-->
            <div class="form-group">
                <label>Sexo:</label>
                @*<select id="comboSexoPersonasFormulario" name="sexo" class="form-control"></select>*@
                <div>
                    <span>Masculino</span><input type="radio" class="form-control" id="rbMasculino" name="sexo" checked />
                    <span>Femenino</span><input type="radio" class="form-control" id="rbFemenino" name="sexo" />
                </div>
            </div>
            <!--Nombre de usuario-->
            <div class="form-group">
                <label>Nombre Usuario</label>
                <input id="txtnombreUsuario" name="Nombre Usuario" type="text" class="form-control limpiar obligatorio" value="" />
            </div>
            <!--Contraseña-->
            <div class="form-group">
                <label>Contraseña</label>
                <input id="txtcontraseña" name="Contraseña" type="password" class="form-control limpiar obligatorio" value="" />
            </div>

            <div id="divErroresRegistro" style="font-weight:bold; color:red">

            </div>
            <!--Botones del formulario-->
            <div class="form-group">
                @Html.ActionLink(" Volver Login", "Index", "Login", New With {.area = ""}, New With {.Class = "btn btn-danger"})
                <input type="button" onclick="GuardarPersonaRegistro()" class="btn btn-primary" value="Guardar" style="margin-bottom:10px">
            </div>
        </div>

    </div>

</body>
</html>

<script src="~/Scripts/jquery-3.6.0.min.js"></script>
<script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.11.0/js/jquery.dataTables.js"></script>
<script src="~/Scripts/jquery-ui.min.js"></script>
<script src=" https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
<script src="~/JS/registrousuario.js"></script>