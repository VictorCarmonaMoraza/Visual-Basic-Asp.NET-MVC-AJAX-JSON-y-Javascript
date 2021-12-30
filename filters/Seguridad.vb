Imports System.Web.Mvc

'clase para deteenr que no se pueda acceder sin autorizacion
Public Class Seguridad
    Inherits ActionFilterAttribute

    'sobreescribimso un metodo. Siempre se va a ejecutar que la pagina que se muestre
    Public Overrides Sub OnActionExecuting(filterContext As ActionExecutingContext)

        'obtenemos el usuario de la session
        Dim usuario = HttpContext.Current.Session("Usuario")
        Dim controladores As List(Of String) = Variables.controladores.Select(Function(x) x.ToUpper()).ToList()
        'Sacamos el nombre del controlador
        Dim nombreController = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName
        'Condicion por la sesion es nula.Si el usuario es nulo redirecionamos a login
        If usuario Is Nothing Or Not controladores.Contains(nombreController.ToUpper()) Then
            'Redireccionamos a login
            filterContext.Result = New RedirectResult("~/Login")
        End If

        MyBase.OnActionExecuting(filterContext)
    End Sub

End Class
