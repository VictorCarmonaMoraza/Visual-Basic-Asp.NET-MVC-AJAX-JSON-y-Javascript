Imports System.Transactions
Imports System.Web.Mvc

Namespace Controllers
    Public Class CitaController
        Inherits Controller

        ' GET: Cita
        Function Index() As ActionResult
            Dim indiceMascota = Variables.controladores.IndexOf("CITA")
            Dim iidvista = Variables.vistas(indiceMascota)
            'El viewbag nos permite pasar informacion del controlador a la vista
            ViewBag.iidvista = iidvista
            Return View()
        End Function

        Function listarTipoMascota() As JsonResult
            Dim db As New BaseDeDatosDataContext
            Dim listado = From tipoMascota In db.TipoMascota
                          Where tipoMascota.BHABILITADO = 1
                          Select New With {
                           .Id = tipoMascota.IIDTIPOMASCOTA,
                           .Nombre = tipoMascota.NOMBRE
                          }
            Return New JsonResult With {.Data = listado,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}

        End Function

        Function listarSedes() As JsonResult
            Dim db As New BaseDeDatosDataContext
            Dim listado = From sede In db.Sede
                          Where sede.BHABILITADO = 1
                          Select New With {
                              .Id = sede.IIDSEDE,
                              .Nombre = sede.VNOMBRE
                          }
            Return New JsonResult With {.Data = listado,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function


        ''' <summary>
        ''' Devuelve el nombre de la mascota(Combo dependiente)
        ''' </summary>
        ''' <param name="idTipo">id de la mascota seleccionada</param>
        ''' <returns></returns>
        Function listarMascotasPorTipo(idTipo As Int32) As JsonResult

            Dim indiceMascota = Variables.controladores.IndexOf("CITA")
            Dim iidvista = Variables.vistas(indiceMascota)

            'Obtenemos el objeto usuario
            Dim ousuario As Usuario = Session("Usuario")
            'obtenemos el id del usaurio del objeto usuario(id usuario logueado)
            Dim iidusuario = ousuario.IIDUSUARIO


            'Instanciamos la base de datos
            Dim db As New BaseDeDatosDataContext
            Dim listado
            If iidvista = 1 Then
                'Consulta
                listado = From mascota In db.Mascota
                          Where mascota.BHABILITADO = ConstantesProyecto.BHABILITADO_OK And
                                  mascota.IIDTIPOMASCOTA = idTipo
                          Select New With {
                                 .Id = mascota.IIDMASCOTA,
                                 .Nombre = mascota.NOMBRE
                                  }

            Else
                'Consulta
                listado = From mascota In db.Mascota
                          Where mascota.BHABILITADO = ConstantesProyecto.BHABILITADO_OK And
                                  mascota.IIDTIPOMASCOTA = idTipo And
                              mascota.IIDUSUARIOPROPIETARIO = iidusuario
                          Select New With {
                                 .Id = mascota.IIDMASCOTA,
                                 .Nombre = mascota.NOMBRE
                                  }
            End If

            Return New JsonResult With {.Data = listado,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function


        ''' <summary>
        ''' Metodo para guardar una cita
        ''' </summary>
        ''' <param name="modelCita">modelo con los datos de una cita</param>
        ''' <returns></returns>
        Function Guardar(modelCita As Cita) As Int32
            ''Respuesta por defecto
            Dim rpta = ConstantesProyecto.valueRespuestaError
            Try
                Dim db As New BaseDeDatosDataContext
                'rellenamos el usuario id de usuario que va a registrar una cita
                Using transaccion As New TransactionScope
                    Dim ousuario As Usuario = Session("Usuario")
                    Dim iidusuario = ousuario.IIDUSUARIO
                    modelCita.IIDUSUARIO = iidusuario
                    'Insercion de la fecha por defecto en la cual se guarda
                    modelCita.DFECHAINICIO = DateTime.Now
                    'habilitamos la cita
                    modelCita.BHABILITADO = ConstantesProyecto.BHABILITADO_OK
                    'Guardamos el estado de la cita
                    modelCita.IIDESTADOCITA = EstadosCita.registrado
                    'insertamos en la base de datos
                    db.Cita.InsertOnSubmit(modelCita)
                    'Guardamos los cambios
                    db.SubmitChanges()
                    'Tambien tenemos que insertar en la tabla Historial Cita el estado de la cita
                    Dim modelHistorialCita As New HistorialCita
                    modelHistorialCita.IIDCITA = modelCita.IIDCITA
                    modelHistorialCita.IIDESTADO = EstadosCita.registrado
                    modelHistorialCita.IIDUSUARIO = iidusuario
                    modelHistorialCita.DFECHA = DateTime.Now
                    'registramos los cambios
                    db.HistorialCita.InsertOnSubmit(modelHistorialCita)
                    'Guardamos los cambios
                    db.SubmitChanges()
                    transaccion.Complete()
                    'Devolvemos la respuesta si todo esta correcto
                    rpta = ConstantesProyecto.valueRespuestaOK
                End Using

            Catch ex As Exception
                rpta = ConstantesProyecto.valueRespuestaError
            End Try
            Return rpta
        End Function

        ''' <summary>
        ''' metod para listar todas las citas por vista total o vista usuario
        ''' </summary>
        ''' <returns></returns>
        Function listadoCita() As JsonResult
            Dim indiceMascota = Variables.controladores.IndexOf("CITA")
            Dim iidvista = Variables.vistas(indiceMascota)

            'Obtenemos el objeto usuario
            Dim ousuario As Usuario = Session("Usuario")
            'obtenemos el id del usaurio del objeto usuario(id usuario logueado)
            Dim iidusuario = ousuario.IIDUSUARIO

            Dim db As New BaseDeDatosDataContext
            Dim listadoCitas
            If iidvista = ConstantesProyecto.value1 Then
                'Consulta
                listadoCitas = From cita In db.Cita
                               Join tipoMascota In db.TipoMascota
                                  On cita.IIDTIPOMASCOTA Equals tipoMascota.IIDTIPOMASCOTA
                               Join usuario In db.Usuario
                                  On cita.IIDUSUARIO Equals usuario.IIDUSUARIO
                               Join persona In db.Persona
                                  On usuario.IIDPERSONA Equals persona.IIDPERSONA
                               Join estadocita In db.EstadoCita
                                   On cita.IIDESTADOCITA Equals estadocita.IIDESTADO
                               Where cita.BHABILITADO = ConstantesProyecto.BHABILITADO_OK
                               Order By cita.DFECHAINICIO
                               Select New With {
                                  cita.IIDCITA,
                                  tipoMascota.NOMBRE,
                                  cita.DFECHAENFERMO.Value.ToShortDateString(),
                                  .NombreCompleto = persona.NOMBRE + " " + persona.APPATERNO + " " + persona.APMATERNO,
                                   estadocita.VNOMBRE,
                                   cita.IIDESTADOCITA
                                   }
            Else 'Vista usuario
                'Consulta
                listadoCitas = From cita In db.Cita
                               Join tipoMascota In db.TipoMascota
                                  On cita.IIDTIPOMASCOTA Equals tipoMascota.IIDTIPOMASCOTA
                               Join usuario In db.Usuario
                                  On cita.IIDUSUARIO Equals usuario.IIDUSUARIO
                               Join persona In db.Persona
                                  On usuario.IIDPERSONA Equals persona.IIDPERSONA
                               Join estadocita In db.EstadoCita
                                   On cita.IIDESTADOCITA Equals estadocita.IIDESTADO
                               Where cita.BHABILITADO = ConstantesProyecto.BHABILITADO_OK And cita.IIDUSUARIO = iidusuario
                               Order By cita.DFECHAINICIO
                               Select New With {
                                  cita.IIDCITA,
                                  tipoMascota.NOMBRE,
                                  cita.DFECHAENFERMO.Value.ToShortDateString(),
                                  .NombreCompleto = persona.NOMBRE + " " + persona.APPATERNO + " " + persona.APMATERNO,
                                   estadocita.VNOMBRE,
                                    cita.IIDESTADOCITA
                                   }
            End If

            Return New JsonResult With {.Data = listadoCitas,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function

        ''' <summary>
        ''' Obtenemos los doctores 
        ''' </summary>
        ''' <returns></returns>
        Function llenarDoctor() As JsonResult
            'instanciamos nuestra base de datos
            Dim db As New BaseDeDatosDataContext
            'Creamos query
            Dim listadoDoctores = From usuario In db.Usuario
                                  Join persona In db.Persona
                              On usuario.IIDPERSONA Equals persona.IIDPERSONA
                                  Where usuario.IIDTIPOUSUARIO = 20
                                  Select New With {
                            .Id = usuario.IIDUSUARIO,
                            .Nombre = persona.NOMBRE + " " + persona.APPATERNO + " " + persona.APMATERNO
            }
            Return New JsonResult With {.Data = listadoDoctores,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function

        ''' <summary>
        ''' Muestra el historial de las citas
        ''' </summary>
        ''' <param name="idCita">id de la cita</param>
        ''' <returns></returns>
        Function mostrarHistorialCita(idCita As Int32) As JsonResult
            'Instanciamos nuestra base de datos
            Dim db As New BaseDeDatosDataContext
            Dim listadoHistorialCitas = From historialcita In db.HistorialCita
                                        Join estadocita In db.EstadoCita
                              On historialcita.IIDESTADO Equals estadocita.IIDESTADO
                                        Join usuario In db.Usuario
                              On historialcita.IIDUSUARIO Equals usuario.IIDUSUARIO
                                        Join persona In db.Persona
                              On usuario.IIDPERSONA Equals persona.IIDPERSONA
                                        Where historialcita.IIDCITA = idCita
                                        Select New With {
                              estadocita.VNOMBRE,
                              .NombreCompleto = persona.NOMBRE + " " + persona.APPATERNO + " " + persona.APMATERNO,
                              .Fecha = historialcita.DFECHA.Value.ToShortDateString(),
                              .Observacion = If(historialcita.VOBSERVACION Is Nothing, "", historialcita.VOBSERVACION)
            }
            Return New JsonResult With {.Data = listadoHistorialCitas,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function


        Function listarEstadosCitas() As JsonResult
            'Instanciamos nuestra base de datos
            Dim db As New BaseDeDatosDataContext
            Dim listadoCitas = From estadocita In db.EstadoCita
                               Where estadocita.BHABILITADO = ConstantesProyecto.BHABILITADO_OK
                               Select New With {
                               .Id = estadocita.IIDESTADO,
                              .Nombre = estadocita.VNOMBRE
            }
            Return New JsonResult With {.Data = listadoCitas,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}

        End Function


        Function filtrarCitasPorEstado(idEstado As String) As JsonResult
            'Instanciamos la base de datos
            Dim db As New BaseDeDatosDataContext

            Dim indiceMascota = Variables.controladores.IndexOf("CITA")
            Dim iidvista = Variables.vistas(indiceMascota)

            'Obtenemos el objeto usuario
            Dim ousuario As Usuario = Session("Usuario")
            'obtenemos el id del usaurio del objeto usuario(id usuario logueado)
            Dim iidusuario = ousuario.IIDUSUARIO
            Dim listadoCitas
            'Vista total
            If iidvista = 1 Then

                If idEstado = "" Then
                    listadoCitas = From cita In db.Cita
                                   Join tipoMascota In db.TipoMascota
                                      On cita.IIDTIPOMASCOTA Equals tipoMascota.IIDTIPOMASCOTA
                                   Join usuario In db.Usuario
                                      On cita.IIDUSUARIO Equals usuario.IIDUSUARIO
                                   Join persona In db.Persona
                                      On usuario.IIDPERSONA Equals persona.IIDPERSONA
                                   Join estadocita In db.EstadoCita
                                       On cita.IIDESTADOCITA Equals estadocita.IIDESTADO
                                   Where cita.BHABILITADO = ConstantesProyecto.BHABILITADO_OK
                                   Order By cita.DFECHAINICIO
                                   Select New With {
                                      cita.IIDCITA,
                                      tipoMascota.NOMBRE,
                                      cita.DFECHAENFERMO.Value.ToShortDateString(),
                                      .NombreCompleto = persona.NOMBRE + " " + persona.APPATERNO + " " + persona.APMATERNO,
                                       estadocita.VNOMBRE,
                                       cita.IIDESTADOCITA
                                       }
                Else
                    listadoCitas = From cita In db.Cita
                                   Join tipoMascota In db.TipoMascota
                                      On cita.IIDTIPOMASCOTA Equals tipoMascota.IIDTIPOMASCOTA
                                   Join usuario In db.Usuario
                                      On cita.IIDUSUARIO Equals usuario.IIDUSUARIO
                                   Join persona In db.Persona
                                      On usuario.IIDPERSONA Equals persona.IIDPERSONA
                                   Join estadocita In db.EstadoCita
                                       On cita.IIDESTADOCITA Equals estadocita.IIDESTADO
                                   Where cita.BHABILITADO = ConstantesProyecto.BHABILITADO_OK And
                                       cita.IIDESTADOCITA = idEstado
                                   Order By cita.DFECHAINICIO
                                   Select New With {
                                      cita.IIDCITA,
                                      tipoMascota.NOMBRE,
                                      cita.DFECHAENFERMO.Value.ToShortDateString(),
                                      .NombreCompleto = persona.NOMBRE + " " + persona.APPATERNO + " " + persona.APMATERNO,
                                       estadocita.VNOMBRE,
                                       cita.IIDESTADOCITA
                                       }
                End If

                'Vista Usuario
            Else

                If idEstado = "" Then
                    listadoCitas = From cita In db.Cita
                                   Join tipoMascota In db.TipoMascota
                                      On cita.IIDTIPOMASCOTA Equals tipoMascota.IIDTIPOMASCOTA
                                   Join usuario In db.Usuario
                                      On cita.IIDUSUARIO Equals usuario.IIDUSUARIO
                                   Join persona In db.Persona
                                      On usuario.IIDPERSONA Equals persona.IIDPERSONA
                                   Join estadocita In db.EstadoCita
                                       On cita.IIDESTADOCITA Equals estadocita.IIDESTADO
                                   Where cita.BHABILITADO = ConstantesProyecto.BHABILITADO_OK And
                                       cita.IIDUSUARIO = iidusuario
                                   Order By cita.DFECHAINICIO
                                   Select New With {
                                      cita.IIDCITA,
                                      tipoMascota.NOMBRE,
                                      cita.DFECHAENFERMO.Value.ToShortDateString(),
                                      .NombreCompleto = persona.NOMBRE + " " + persona.APPATERNO + " " + persona.APMATERNO,
                                       estadocita.VNOMBRE,
                                       cita.IIDESTADOCITA
                                       }
                Else
                    listadoCitas = From cita In db.Cita
                                   Join tipoMascota In db.TipoMascota
                                      On cita.IIDTIPOMASCOTA Equals tipoMascota.IIDTIPOMASCOTA
                                   Join usuario In db.Usuario
                                      On cita.IIDUSUARIO Equals usuario.IIDUSUARIO
                                   Join persona In db.Persona
                                      On usuario.IIDPERSONA Equals persona.IIDPERSONA
                                   Join estadocita In db.EstadoCita
                                       On cita.IIDESTADOCITA Equals estadocita.IIDESTADO
                                   Where cita.BHABILITADO = ConstantesProyecto.BHABILITADO_OK And
                                       cita.IIDESTADOCITA = idEstado And
                                       cita.IIDUSUARIO = iidusuario
                                   Order By cita.DFECHAINICIO
                                   Select New With {
                                      cita.IIDCITA,
                                      tipoMascota.NOMBRE,
                                      cita.DFECHAENFERMO.Value.ToShortDateString(),
                                      .NombreCompleto = persona.NOMBRE + " " + persona.APPATERNO + " " + persona.APMATERNO,
                                       estadocita.VNOMBRE,
                                       cita.IIDESTADOCITA
                                       }
                End If
            End If

            Return New JsonResult With {.Data = listadoCitas,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}

        End Function


        Function cambiarEstado(idCita As Int32, idestadoACambiar As Int32, observacion As String, modelCita As Cita) As Int32
            'Respuesta por defecto 
            Dim rpta = ConstantesProyecto.valueRespuestaError

            Try
                Dim ousuario As Usuario = Session("Usuario")
                Dim iidusuario = ousuario.IIDUSUARIO
                Using transaccion As New TransactionScope
                    Dim db As New BaseDeDatosDataContext

                    'Buscamo la cita para cambiarle el estado
                    Dim ocita = (From cita In db.Cita
                                 Where cita.IIDCITA = idCita
                                 Select cita).First()
                    ocita.IIDESTADOCITA = idestadoACambiar
                    db.SubmitChanges()

                    Dim oHistorialCita As New HistorialCita
                    oHistorialCita.IIDCITA = idCita
                    oHistorialCita.DFECHA = DateTime.Now
                    oHistorialCita.IIDESTADO = idestadoACambiar
                    oHistorialCita.IIDUSUARIO = iidusuario
                    If observacion Is Nothing Then
                        oHistorialCita.VOBSERVACION = ""
                    Else
                        oHistorialCita.VOBSERVACION = observacion
                    End If
                    db.HistorialCita.InsertOnSubmit(oHistorialCita)
                    db.SubmitChanges()

                    If idestadoACambiar = 3 Then
                        ocita.PRECIOATENCION = modelCita.PRECIOATENCION
                        ocita.IIDDOCTORASIGNACITAUSUARIO = modelCita.IIDDOCTORASIGNACITAUSUARIO
                        ocita.DFECHACITA = modelCita.DFECHACITA
                        db.SubmitChanges()
                    End If

                    transaccion.Complete()
                    rpta = ConstantesProyecto.valueRespuestaOK


                End Using
            Catch ex As Exception
                rpta = ConstantesProyecto.valueRespuestaError
            End Try
            Return rpta

        End Function

    End Class
End Namespace