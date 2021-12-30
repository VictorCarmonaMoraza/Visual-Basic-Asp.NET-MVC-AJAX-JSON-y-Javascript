<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title - My ASP.NET Application</title>
    <link href="~/Content/Site.css" rel="stylesheet" type="text/css" />
    <!-- <link href="~/Content/bootstrap.min.css" rel="stylesheet" type="text/css" />-->
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <!--<script src="~/Scripts/modernizr-2.8.3.js"></script>-->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.js"></script>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.8.1/css/all.css" integrity="sha384-50oBUHEmvpQ+1lW4y57PTFmhCaXp0ML5d60M1M7uH2+nqUivzIebhndOJK28anvf" crossorigin="anonymous">

</head>
<body>
    <!--  < <div class="navbar navbar-inverse navbar-fixed-top">
           <div class="container">
               <div class="navbar-header">
                   <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                       <span class="icon-bar"></span>
                       <span class="icon-bar"></span>
                       <span class="icon-bar"></span>
                   </button>

               </div>
               <div class="navbar-collapse collapse">
                   <ul class="nav navbar-nav">
                   </ul>
               </div>
           </div>
       </div>-->
    @code
        Dim ousuario As Usuario = Session("Usuario")
    End Code

   
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
        <a style="color:white">
            @code
                Dim nelementos As Int32 = Variables.controladores.Count()
                For i As Integer = 0 To nelementos - 1
                    @Html.ActionLink(Variables.mensajes(i), Variables.acciones(i), Variables.controladores(i), New With {.area = ""}, New With {.Class = "navbar-brand"})
                Next
            End Code
            @*@Html.ActionLink("Tipo Usuario", "Index", "TipoUsuario", New With {.area = ""}, New With {.Class = "navbar-brand"})
        @Html.ActionLink("Personas", "Index", "Personas", New With {.area = ""}, New With {.Class = "navbar-brand"})
        @Html.ActionLink("Medicamentos", "Index", "Medicamentos", New With {.area = ""}, New With {.Class = "navbar-brand"})
        @Html.ActionLink("Mascotas", "Index", "Mascota", New With {.area = ""}, New With {.Class = "navbar-brand"})
        @Html.ActionLink("Paginas", "Index", "Pagina", New With {.area = ""}, New With {.Class = "navbar-brand"})
        @Html.ActionLink("Usuarios", "Index", "Usuario", New With {.area = ""}, New With {.Class = "navbar-brand"})*@
            @*@Html.ActionLink("Login", "Index", "Login", New With {.area = ""}, New With {.Class = "navbar-brand"})*@

            <!--Si la variable de sesion no es nula-->
            @If ousuario IsNot Nothing Then
                @Html.ActionLink("Cerrar Sesion", "CerrarSesion", "Login", New With {.area = ""}, New With {.Class = "navbar-brand"})
            Else
                @Html.ActionLink("Login", "Index", "Login", New With {.area = ""}, New With {.Class = "navbar-brand"})
                @Html.ActionLink("Registrar", "Index", "RegistrarUsuario", New With {.area = ""}, New With {.Class = "navbar-brand"})
            End If

        </a>

        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarNavAltMarkup">
            <ul class="navbar-nav mr-auto mt-2 mt-lg-0">
                <li class="nav-item active">
                    <a class="nav-link" href="#"> <span class="sr-only">(current)</span></a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="#"></a>
                </li>
                <li class="nav-item">
                    <a class="nav-link disabled" href="#"></a>
                </li>
            </ul>
        </div>
    </nav>

    <div class="container body-content">
        <hr />
        @RenderBody()

        <footer>
            <p>&copy; @DateTime.Now.Year - My ASP.NET Application</p>
        </footer>
    </div>

    <!-- <script src="~/Scripts/jquery-3.4.1.min.js"></script>-->
    <!-- <script src="~/Scripts/bootstrap.min.js"></script>-->
    <script src="~/Scripts/bootstrap.min.js"></script>
</body>
</html>
