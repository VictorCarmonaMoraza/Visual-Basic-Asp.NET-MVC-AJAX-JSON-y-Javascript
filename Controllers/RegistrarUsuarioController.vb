Imports System.Web.Mvc

Namespace Controllers
    Public Class RegistrarUsuarioController
        Inherits Controller

        ' GET: RegistrarUsuario
        Function Index() As ActionResult
            Return View()
        End Function


        ''' <summary>
        ''' Metodo para el registro de una persona
        ''' </summary>
        ''' <param name="modelPersona">Objeto persona</param>
        ''' <returns></returns>
        Function GuardarUsuarioPersona(modelPersona As Persona, NOMBREUSUARIO As String, CONTRASEÑA As String)
            'Respuesta por deceto
            Dim rpta = ConstantesProyecto.valueRespuestaError
            Try
                'Obtenemos el id de la persona
                Dim idPersona = modelPersona.IIDPERSONA
                'instanciamos la base de datos
                Dim db As New BaseDeDatosDataContext
                'Tenemos que cifrar nuestra contraseña para cifrarla y asi poder compararla con otra cifrada
                Dim cifrado As New Cifrado
                'Estamos en modo creacion de persona
                Using transaccion As New Transactions.TransactionScope
                    If idPersona = 0 Then
                        'Validacmos que el usuario no se repita
                        Dim cantidad = db.Usuario.Where(Function(x) x.NOMBREUSUARIO.ToUpper() = NOMBREUSUARIO.ToUpper()).Count()
                        If cantidad >= 1 Then
                            Return -1
                        End If

                        'Fin validacion repeticion de usuartheio
                        db.Persona.InsertOnSubmit(modelPersona)
                            db.SubmitChanges()
                            rpta = ConstantesProyecto.valueRespuestaOK

                            'Definimos un objeto usuario
                            Dim ousuario As New Usuario
                            'llenamos el objeto
                            ousuario.NOMBREUSUARIO = NOMBREUSUARIO
                            ousuario.CONTRA = cifrado.cifrar(CONTRASEÑA)
                            ousuario.BHABILITADO = ConstantesProyecto.BHABILITADO_OK
                            ousuario.IIDPERSONA = modelPersona.IIDPERSONA
                            ousuario.IIDTIPOUSUARIO = 5

                            db.Usuario.InsertOnSubmit(ousuario)
                            db.SubmitChanges()
                            transaccion.Complete()
                            rpta = ConstantesProyecto.valueRespuestaOK
                        End If
                End Using

            Catch ex As Exception
                rpta = ConstantesProyecto.valueRespuestaError
            End Try
            Return rpta
        End Function
    End Class
End Namespace