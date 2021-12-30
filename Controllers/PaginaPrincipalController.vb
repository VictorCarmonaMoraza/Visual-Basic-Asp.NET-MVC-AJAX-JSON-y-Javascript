Imports System.Web.Mvc

Namespace Controllers
    <Seguridad()>
    Public Class PaginaPrincipalController
        Inherits Controller

        ' GET: PaginaPrincipal
        Function Index() As ActionResult

            Dim usu As Usuario = Session("Usuario")
            Dim per As Persona = Session("Persona")
            ViewData("idusuario") = usu.IIDUSUARIO
            ViewData("nombrecompleto") = per.NOMBRE + " " + per.APPATERNO + " " + per.APMATERNO

            Return View()
        End Function
    End Class
End Namespace