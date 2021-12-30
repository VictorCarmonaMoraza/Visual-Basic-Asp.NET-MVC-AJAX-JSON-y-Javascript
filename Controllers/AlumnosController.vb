Imports System.Web.Mvc

Namespace Controllers
    Public Class AlumnosController
        Inherits Controller

        ' GET: Alumnos
        Function Index() As ActionResult
            Return View()
        End Function
    End Class
End Namespace