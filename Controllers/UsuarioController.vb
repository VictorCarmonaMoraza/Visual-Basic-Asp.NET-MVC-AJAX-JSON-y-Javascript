Imports System.Security.Cryptography
Imports System.Web.Mvc
Namespace Controllers
    <Seguridad()>
    Public Class UsuarioController
        Inherits Controller

        ' GET: Usuario
        Function Index() As ActionResult
            Return View()
        End Function

        Function listarPersonasSinUsuario() As JsonResult

            Dim db As New BaseDeDatosDataContext
            Dim listado = From persona In db.Persona
                          Where persona.BHABILITADO = ConstantesProyecto.BHABILITADO_OK And persona.BTIENEUSUARIO = ConstantesProyecto.BTIENE_USUARIO_NO_OK
                          Select New With {
                         .Id = persona.IIDPERSONA,
                         .Nombre = persona.NOMBRE + " " + persona.APPATERNO + " " + persona.APMATERNO
         }
            Return New JsonResult With {.Data = listado,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}


        End Function

        Function insertarUsuario(ousuario As Usuario) As Int32
            Dim rpta = ConstantesProyecto.valueRespuestaError
            Try
                Dim db As New BaseDeDatosDataContext
                Dim ocifrado As New Cifrado

                Using transaccion As New Transactions.TransactionScope

                    If ousuario.IIDUSUARIO = 0 Then

                        Dim cantidad As Int32 = db.Usuario.Where(Function(x) x.NOMBREUSUARIO.ToUpper() = ousuario.NOMBREUSUARIO.ToUpper()).Count()
                        If cantidad >= 1 Then
                            Return -1
                        End If


                        Dim idPersona As Int32 = ousuario.IIDPERSONA

                        Dim registroPersona = (From persona In db.Persona
                                               Where persona.IIDPERSONA = idPersona
                                               Select persona).First()
                        registroPersona.BTIENEUSUARIO = 1
                        db.SubmitChanges()

                        Dim contra As String = ousuario.CONTRA
                        ousuario.CONTRA = ocifrado.cifrar(contra)
                        db.Usuario.InsertOnSubmit(ousuario)
                        db.SubmitChanges()
                        transaccion.Complete()
                        rpta = 1
                    Else
                        Dim cantidad As Int32 = db.Usuario.Where(Function(x) x.NOMBREUSUARIO.ToUpper() = ousuario.NOMBREUSUARIO.ToUpper() And x.IIDUSUARIO <> ousuario.IIDUSUARIO).Count()
                        If cantidad >= 1 Then
                            Return -1
                        End If

                        Dim usuarioListado = (From usuario In db.Usuario
                                              Where usuario.IIDUSUARIO = ousuario.IIDUSUARIO
                                              Select usuario).First()

                        usuarioListado.NOMBREUSUARIO = ousuario.NOMBREUSUARIO
                        usuarioListado.IIDTIPOUSUARIO = ousuario.IIDTIPOUSUARIO
                        db.SubmitChanges()
                        transaccion.Complete()
                        rpta = ConstantesProyecto.BHABILITADO_OK
                    End If
                End Using

            Catch ex As Exception
                rpta = ConstantesProyecto.BHABILITADO_NO_OK
            End Try
            Return rpta

        End Function

        Function eliminarUsuario(id As Int32) As Int32
            Dim rpta = ConstantesProyecto.valueRespuestaError
            Try
                Dim db As New BaseDeDatosDataContext
                Dim registro = (From usuario In db.Usuario
                                Where usuario.IIDUSUARIO = id
                                Select usuario).First()
                registro.BHABILITADO = 0
                db.SubmitChanges()
                rpta = ConstantesProyecto.valueRespuestaOK

            Catch ex As Exception
                rpta = ConstantesProyecto.valueRespuestaError
            End Try

            Return rpta

        End Function


        Function recuperarInformacion(id As Int32) As JsonResult
            Dim db As New BaseDeDatosDataContext
            Dim listadoRecuperar = (From usuario In db.Usuario
                                    Where usuario.IIDUSUARIO = id
                                    Select New With {
                                         usuario.IIDUSUARIO,
                                        usuario.NOMBREUSUARIO,
                                        usuario.IIDTIPOUSUARIO
             }).First()

            Return New JsonResult With {.Data = listadoRecuperar,
               .JsonRequestBehavior = JsonRequestBehavior.AllowGet}

        End Function



        Function listarUsuarios() As JsonResult
            Dim db As New BaseDeDatosDataContext
            Dim listadoUsuario = From usuario In db.Usuario
                                 Join tipoUsuario In db.TipoUsuario
                                 On usuario.IIDTIPOUSUARIO Equals tipoUsuario.IIDTIPOUSUARIO
                                 Join persona In db.Persona
                                  On usuario.IIDPERSONA Equals persona.IIDPERSONA
                                 Where usuario.BHABILITADO = 1
                                 Select New With {
                                     usuario.IIDUSUARIO,
                                     usuario.NOMBREUSUARIO,
                                     .NombreCompleto = persona.NOMBRE + " " + persona.APPATERNO + " " + persona.APMATERNO,
                                     tipoUsuario.NOMBRE
             }
            Return New JsonResult With {.Data = listadoUsuario,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}

        End Function



        Function BuscarUsuarios(nombreUsuario As String) As JsonResult
            Dim db As New BaseDeDatosDataContext
            Dim listadoUsuario = From usuario In db.Usuario
                                 Join tipoUsuario In db.TipoUsuario
                                 On usuario.IIDTIPOUSUARIO Equals tipoUsuario.IIDTIPOUSUARIO
                                 Join persona In db.Persona
                                  On usuario.IIDPERSONA Equals persona.IIDPERSONA
                                 Where usuario.BHABILITADO = 1 And usuario.NOMBREUSUARIO.Contains(nombreUsuario)
                                 Select New With {
                                     usuario.IIDUSUARIO,
                                     usuario.NOMBREUSUARIO,
                                     .NombreCompleto = persona.NOMBRE + " " + persona.APPATERNO + " " + persona.APMATERNO,
                                     tipoUsuario.NOMBRE
             }
            Return New JsonResult With {.Data = listadoUsuario,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}

        End Function


        Function listarTipoUsuarios() As JsonResult
            Dim db As New BaseDeDatosDataContext
            Dim listado = From tipoUsuario In db.TipoUsuario
                          Where tipoUsuario.BHABILITADO = 1
                          Select New With {
                           .Id = tipoUsuario.IIDTIPOUSUARIO,
                           .Nombre = tipoUsuario.NOMBRE
                         }
            Return New JsonResult With {.Data = listado,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}

        End Function




    End Class





End Namespace