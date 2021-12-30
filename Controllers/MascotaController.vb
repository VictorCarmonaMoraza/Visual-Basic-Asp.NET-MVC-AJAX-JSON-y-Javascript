Imports System.Web.Mvc

Namespace Controllers
    <Seguridad()>
    Public Class MascotaController
        Inherits Controller

        ' GET: Mascota
        Function Index() As ActionResult
            Return View()
        End Function

        ''' <summary>
        ''' Metodo para listar las mascotas
        ''' </summary>
        ''' <returns></returns>
        Function listarMascotas() As JsonResult

            Dim indiceMascota = Variables.controladores.IndexOf("MASCOTA")
            Dim iidvista = Variables.vistas(indiceMascota)

            Dim ousuario As Usuario = Session("Usuario")
            Dim iidusuario = ousuario.IIDUSUARIO

            'Instanciamos la base de datos
            Dim db As New BaseDeDatosDataContext
            Dim listado
            If iidvista = 1 Then
                'Hacemos la consulta
                listado = From mascota In db.Mascota
                          Join sexo In db.Sexo
                              On mascota.IIDSEXO Equals sexo.IIDSEXO
                          Join tipoMascota In db.TipoMascota
                              On mascota.IIDTIPOMASCOTA Equals tipoMascota.IIDTIPOMASCOTA
                          Where mascota.BHABILITADO = 1
                          Select New With {
                                mascota.IIDMASCOTA,
                                mascota.NOMBRE,
                                mascota.ALTURA,
                                mascota.ANCHO,
                                .SEXO = sexo.NOMBRE,
                                .NOMBREMASCOTA = tipoMascota.NOMBRE
                                }
            Else
                listado = From mascota In db.Mascota
                          Join sexo In db.Sexo
                              On mascota.IIDSEXO Equals sexo.IIDSEXO
                          Join tipoMascota In db.TipoMascota
                              On mascota.IIDTIPOMASCOTA Equals tipoMascota.IIDTIPOMASCOTA
                          Where mascota.BHABILITADO = 1 And mascota.IIDUSUARIOPROPIETARIO = iidusuario
                          Select New With {
                                mascota.IIDMASCOTA,
                                mascota.NOMBRE,
                                mascota.ALTURA,
                                mascota.ANCHO,
                                .SEXO = sexo.NOMBRE,
                                .NOMBREMASCOTA = tipoMascota.NOMBRE
                                }
            End If


            Return New JsonResult With {.Data = listado,
                 .JsonRequestBehavior = JsonRequestBehavior.AllowGet}

        End Function

        ''' <summary>
        ''' Obtenemos los datos para rellenar el combo de tipo de mascota
        ''' </summary>
        ''' <returns></returns>
        Function LoadComboTipoMascota() As JsonResult
            'instanciamos la base de datos
            Dim db As New BaseDeDatosDataContext
            'Query para traernos los datos
            Dim comboListado = From tipoMascota In db.Mascota
                               Where tipoMascota.BHABILITADO = 1
                               Select New With {
                                   .Id = tipoMascota.IIDMASCOTA,
                                   .valor = tipoMascota.NOMBRE
                               }
            Return New JsonResult With {.Data = comboListado,
             .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function

        ''' <summary>
        ''' Metodo para obtener listado segun el tipo de mascota elegido
        ''' </summary>
        ''' <param name="idTipoMascota"></param>
        ''' <returns></returns>
        Function filtrarMascotaPorTipo(idTipoMascota As Int32) As JsonResult

            Dim indiceMascota = Variables.controladores.IndexOf("MASCOTA")
            Dim iidvista = Variables.vistas(indiceMascota)

            Dim ousuario As Usuario = Session("Usuario")
            Dim iidusuario = ousuario.IIDUSUARIO
            'Instanciamos la base de datos
            Dim db As New BaseDeDatosDataContext
            Dim listadoFiltrado
            'iidvista = 1 es vista total
            If iidvista = 1 Then
                listadoFiltrado = From mascota In db.Mascota
                                  Join sexo In db.Sexo
                              On mascota.IIDSEXO Equals sexo.IIDSEXO
                                  Join tipoMascota In db.TipoMascota
                              On mascota.IIDTIPOMASCOTA Equals tipoMascota.IIDTIPOMASCOTA
                                  Where mascota.BHABILITADO = 1 And
                              mascota.IIDTIPOMASCOTA = idTipoMascota
                                  Select New With {
                                mascota.IIDTIPOMASCOTA,
                                mascota.NOMBRE,
                                mascota.ALTURA,
                                mascota.ANCHO,
                                .NOMBRESEXO = sexo.NOMBRE,
                                .NOMBREMASCOTA = tipoMascota.NOMBRE
                                }
            Else 'idvista =2 es vista usuario
                listadoFiltrado = From mascota In db.Mascota
                                  Join sexo In db.Sexo
                              On mascota.IIDSEXO Equals sexo.IIDSEXO
                                  Join tipoMascota In db.TipoMascota
                              On mascota.IIDTIPOMASCOTA Equals tipoMascota.IIDTIPOMASCOTA
                                  Where mascota.BHABILITADO = 1 And
                              mascota.IIDTIPOMASCOTA = idTipoMascota And mascota.IIDUSUARIOPROPIETARIO = iidusuario
                                  Select New With {
                                mascota.IIDTIPOMASCOTA,
                                mascota.NOMBRE,
                                mascota.ALTURA,
                                mascota.ANCHO,
                                .NOMBRESEXO = sexo.NOMBRE,
                                .NOMBREMASCOTA = tipoMascota.NOMBRE
                                }
            End If



            Return New JsonResult With {.Data = listadoFiltrado,
             .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function


        Function Eliminar(id As Int32) As Int32
            Dim rpta = ConstantesProyecto.valueRespuestaError
            Try
                Dim db As New BaseDeDatosDataContext
                Dim objeto = (From mascota In db.Mascota
                              Where mascota.IIDMASCOTA = id
                              Select mascota).First()
                objeto.BHABILITADO = ConstantesProyecto.BHABILITADO_NO_OK
                db.SubmitChanges()
                rpta = ConstantesProyecto.valueRespuestaOK
            Catch ex As Exception
                rpta = ConstantesProyecto.valueRespuestaError
            End Try
            Return rpta
        End Function

        ''' <summary>
        ''' Lista los tipos de mascotas
        ''' </summary>
        ''' <returns></returns>
        Function listarTipoMascotas() As JsonResult


            Dim db As New BaseDeDatosDataContext
            Dim listado = From tipomascota In db.TipoMascota
                          Where tipomascota.BHABILITADO = 1
                          Select New With {
                            .Id = tipomascota.IIDTIPOMASCOTA,
                            .Nombre = tipomascota.NOMBRE
                           }
            Return New JsonResult With {.Data = listado,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}


        End Function

        ''' <summary>
        ''' Obtiene el id y el valor del campo sexo
        ''' </summary>
        ''' <returns></returns>
        Function listarComboSexo() As JsonResult
            'Instanciamos la base de datos
            Dim db As New BaseDeDatosDataContext
            'Obtenemos los valores del combo
            Dim valoresCombo = From sexo In db.Sexo
                               Where sexo.BHABILITADO = ConstantesProyecto.value1
                               Select New With {
                                   .Id = sexo.IIDSEXO,
                                   .Nombre = sexo.NOMBRE
                                   }
            Return New JsonResult With {.Data = valoresCombo,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet
            }
        End Function

        ''' <summary>
        ''' Recuperamos informacion de una mascota en particular
        ''' </summary>
        ''' <param name="idMascota">id d ela mascota a recuperar</param>
        ''' <returns></returns>
        Function reuperarInformacionMascota(idMascota As Int32) As JsonResult
            'Instanciamos la base de datos 
            Dim db As New BaseDeDatosDataContext
            'Buscamos el registro con esa informacion
            Dim registro = (From mascota In db.Mascota
                            Where mascota.IIDMASCOTA = idMascota
                            Select New With {
                               mascota.IIDMASCOTA,
                               mascota.NOMBRE,
                               mascota.IIDTIPOMASCOTA,
                               .FECHANACIMIENTO = mascota.FECHANACIMIENTO.ToShortDateString(),
                               mascota.ANCHO,
                               mascota.ALTURA,
                               mascota.IIDSEXO
                           }).First()
            'Devolvemos la consulta
            Return New JsonResult With {.Data = registro,
               .JsonRequestBehavior = JsonRequestBehavior.AllowGet}

        End Function


        ''' <summary>
        ''' Metodo para guardar una mascota en base de datos
        ''' </summary>
        ''' <param name="modelMascota">modelo de mascota a guardar</param>
        ''' <returns></returns>
        Function GuardarMascota(modelMascota As Mascota)
            'Respuesta por defceto
            Dim rpta = ConstantesProyecto.valueRespuestaError

            Try
                'Obtenemos el id de la mascota
                Dim idMascota = modelMascota.IIDMASCOTA
                'instanciamos la base de datos
                Dim db As New BaseDeDatosDataContext

                'Estamos en modo creacion de persona
                If idMascota = 0 Then
                    Dim ousuario As Usuario = Session("Usuario")
                    Dim iidusuario = ousuario.IIDUSUARIO

                    'Sacamos el usuario propietario
                    modelMascota.IIDUSUARIOPROPIETARIO = iidusuario
                    'Inserto en base de datos
                    db.Mascota.InsertOnSubmit(modelMascota)
                    db.SubmitChanges()
                    rpta = ConstantesProyecto.valueRespuestaOK
                Else
                    'EDITAR
                    'Obtenemos todo el registro para editarlo
                    Dim registro = (From mascota In db.Mascota
                                    Where mascota.IIDMASCOTA = idMascota
                                    Select mascota).First()
                    'Actualizamos los registros
                    registro.NOMBRE = modelMascota.NOMBRE
                    registro.ALTURA = modelMascota.ALTURA
                    registro.ANCHO = modelMascota.ANCHO
                    registro.IIDTIPOMASCOTA = modelMascota.IIDTIPOMASCOTA
                    registro.IIDSEXO = modelMascota.IIDSEXO
                    registro.FECHANACIMIENTO = modelMascota.FECHANACIMIENTO

                    'confirmamos los cambios
                    db.SubmitChanges()
                    rpta = ConstantesProyecto.valueRespuestaOK
                End If

            Catch ex As Exception
                rpta = ConstantesProyecto.valueRespuestaError
            End Try
            Return rpta
        End Function

        ''' <summary>
        ''' Metodo para eliminar una pagina
        ''' </summary>
        ''' <param name="idMascota">id de la mascota a eleiminar</param>
        ''' <returns></returns>
        Function eliminarMascota(idMascota As Int32)
            'respuesta por defecto
            Dim rpta = 0
            Try
                Dim db As New BaseDeDatosDataContext
                Dim registro = (From mascota In db.Mascota
                                Where mascota.IIDMASCOTA = idMascota
                                Select mascota).First()
                'Ponemos el registro a BHABILITADO = 0
                registro.BHABILITADO = 0
                db.SubmitChanges()
                rpta = 1
            Catch ex As Exception
                rpta = 0
            End Try
            Return rpta
        End Function
    End Class
End Namespace