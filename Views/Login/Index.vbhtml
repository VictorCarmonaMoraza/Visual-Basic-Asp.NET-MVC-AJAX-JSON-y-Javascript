@Code
    ViewData("Title") = "Index"
End Code

@*https://codepen.io/diego_regino/pen/vmLabw*@


<!--<div action="" class="formulario">

    <p class="titulo_formulario">Iniciar Sesión</p>

    <input type="text" id="txtusu" placeholder="Usuario" class="input_textual" />

    <input type="password" id="txtcontraseña" placeholder="Contraseña" class="input_textual" />


    <button class="input_boton" onclick="login()">
        <i class="fa fa-paper-plane" aria-hidden="true"> Ingresar</i>
    </button>

</div>-->
<!--Nuesto jquery siempre debe estar-->
<!--<script src="~/Scripts/jquery-3.6.0.min.js"></script>
<script src="~/JS/login.js"></script>-->

<html>
<head>
    <Title>Login</Title>
    <meta name="viewport" content="width=device-width,initial-scale=1.0">
    @*<link rel="stylesheet" href="style.css">*@
    <link href="~/Content/login.css" rel="stylesheet" />
</head>
<body>
   
    <link href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet">
    <div class="form">
        <h2>Login</h2>
        <div class="input">
            <div class="inputBox">
                <label for="">Username</label>
                <input type="text" id="txtusu">
            </div>
            <div class="inputBox">
                <label for="">Password</label>
                <input type="password" id="txtcontraseña">
            </div>
            <div class="inputBox">
                <input type="submit" name="" value="Sign In" onclick="login()">
            </div>
        </div>
        <p class="forgot">Forgot Password? <a href="#">Click Here</a></p>

    </div>

</body>

</html>

<!--Nuesto jquery siempre debe estar-->
<script src="~/Scripts/jquery-3.6.0.min.js"></script>
<script src="~/JS/login.js"></script>
