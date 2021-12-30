Imports System.Web.Mvc

Namespace Controllers
    Public Class LoginController
        Inherits Controller

        ' GET: Login
        Function Index() As ActionResult
            Return View()
        End Function

        Function CerrarSesion() As ActionResult
            'Ponemos las variables de sesion a nulas
            Session("Usuario") = Nothing
            Session("Persona") = Nothing
            'Vaciamos las variables
            'Siempre se elemina de la ultima poscion hasta la primera
            For x = Variables.acciones.Count - 1 To 0 Step -1
                Variables.acciones.RemoveAt(x)
                Variables.controladores.RemoveAt(x)
                Variables.mensajes.RemoveAt(x)
                Variables.vistas.RemoveAt(x)
            Next

            'Retornamos a la vista index de Login
            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="nombreUsuario">nombre del usuario</param>
        ''' <param name="password">contraseña del usuario</param>
        ''' <returns></returns>
        Function ingresarLogin(nombreUsuario As String, password As String) As Int32
            'Por defecto mi respuesta es cero
            Dim rpta = ConstantesProyecto.valueRespuestaError
            Try
                'instanciamos la base de datos
                Dim db As New BaseDeDatosDataContext
                'Tenemos que cifrar nuestra contraseña para cifrarla y asi poder compararla con otra cifrada
                Dim cifrado As New Cifrado
                'Contraseña cifrada
                Dim contraseñaCifrada = cifrado.cifrar(password)


                'Consulta a la base de datos
                Dim registro = (From usuario In db.Usuario
                                Where usuario.NOMBREUSUARIO = nombreUsuario And
                                   usuario.CONTRA = contraseñaCifrada
                                Select usuario).Count()

                If registro = ConstantesProyecto.value1 Then
                    'obtenemos usuario logueado
                    Dim usuarioLogueado = (From usuario In db.Usuario
                                           Where usuario.NOMBREUSUARIO = nombreUsuario And
                                            usuario.CONTRA = contraseñaCifrada
                                           Select usuario).First()

                    Dim personalogueada = (From persona In db.Persona
                                           Where persona.IIDPERSONA = usuarioLogueado.IIDPERSONA
                                           Select persona).First()


                    'Guardamos en session el usuarioLogueado para poderlo utilizar en los distintos controladores
                    Session("Usuario") = usuarioLogueado
                    'Creamos otro session para guardar persona
                    Session("Persona") = personalogueada
                    Dim idUsuarioSesion As Int32 = usuarioLogueado.IIDUSUARIO


                    Dim roles = From usuario In db.Usuario
                                Join tipoUsuario In db.TipoUsuario
                                    On usuario.IIDTIPOUSUARIO Equals tipoUsuario.IIDTIPOUSUARIO
                                Join paginatipousu In db.PaginaTipoUsuario
                                    On tipoUsuario.IIDTIPOUSUARIO Equals paginatipousu.IIDTIPOUSUARIO
                                Join pagina In db.Pagina
                                    On paginatipousu.IIDPAGINA Equals pagina.IIDPAGINA
                                Where usuario.BHABILITADO = ConstantesProyecto.BHABILITADO_OK And
                                paginatipousu.BHABILITADO = ConstantesProyecto.BHABILITADO_OK And
                                    usuario.IIDUSUARIO = idUsuarioSesion
                                Select New With {
                                    .accion = pagina.ACCION.ToUpper(),
                                    .controlador = pagina.CONTROLADOR.ToUpper(),
                                    .mensaje = pagina.MENSAJE.ToUpper(),
                                    .idvista = paginatipousu.IIDVISTA
                    }
                    'Recorremos la lista de roles
                    For Each oroles In roles
                        Variables.acciones.Add(oroles.accion)
                        Variables.controladores.Add(oroles.controlador)
                        Variables.mensajes.Add(oroles.mensaje)
                        Variables.vistas.Add(oroles.idvista)

                    Next

                End If
                Return registro

            Catch ex As Exception
                rpta = ConstantesProyecto.valueRespuestaError
            End Try
            Return rpta
        End Function
    End Class
End Namespace