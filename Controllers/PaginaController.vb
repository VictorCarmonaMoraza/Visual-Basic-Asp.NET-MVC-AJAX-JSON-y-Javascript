Imports System.Web.Mvc

Namespace Controllers
    <Seguridad()>
    Public Class PaginaController
        Inherits Controller

        ' GET: Pagina
        Function Index() As ActionResult
            Return View()
        End Function

        ''' <summary>
        ''' Metodo para obtener el listado de paginas
        ''' </summary>
        ''' <returns></returns>
        Function listarPaginas() As JsonResult
            'instanciamos la base de datos
            Dim db As New BaseDeDatosDataContext
            'query para obtener la infromacion
            Dim listadoPaginas = From pagina In db.Pagina
                                 Where pagina.BHABILITADO = ConstantesProyecto.BHABILITADO_OK
                                 Select New With {
                                    pagina.IIDPAGINA, 'campos que nos traemos de la base de datos
                                     pagina.MENSAJE,
                                     pagina.ACCION,
                                     pagina.CONTROLADOR
                                 }
            Return New JsonResult With {.Data = listadoPaginas,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function

        ''' <summary>
        ''' Metodo para guardar una pagina
        ''' </summary>
        ''' <param name="modelPagina">modelo con todos los datos de una pagina</param>
        ''' <returns></returns>
        Function guardarPaginas(modelPagina As Pagina)
            'Respuesta por defecto que sera 0 y la no buena
            Dim rpta = ConstantesProyecto.valueRespuestaError
            Dim idPagina = modelPagina.IIDPAGINA
            'Instanciamos la base de datos
            Dim db As New BaseDeDatosDataContext
            Try
                If idPagina = 0 Then
                    'Consulta lambda
                    Dim cantidad As Int32 = db.Pagina.Where(Function(x) x.MENSAJE.ToUpper() = modelPagina.MENSAJE.ToUpper()).Count()
                    If cantidad >= 1 Then
                        Return -1
                    End If

                    'nsertamos los datos
                    db.Pagina.InsertOnSubmit(modelPagina)
                    'confirmamos los cambios
                    db.SubmitChanges()
                    rpta = ConstantesProyecto.valueRespuestaOK
                Else
                    Dim cantidad As Int32 = db.Pagina.Where(Function(x) x.MENSAJE.ToUpper() = modelPagina.MENSAJE.ToUpper() And x.IIDPAGINA <> modelPagina.IIDPAGINA).Count()
                    If cantidad >= 1 Then
                        Return -1
                    End If
                    'Obtenemos el registro
                    Dim registro = (From pagina In db.Pagina
                                    Where pagina.IIDPAGINA = idPagina
                                    Select pagina).First()
                    registro.MENSAJE = modelPagina.MENSAJE
                    registro.ACCION = modelPagina.ACCION
                    registro.CONTROLADOR = modelPagina.CONTROLADOR

                    'Guardo los cambios
                    db.SubmitChanges()
                    'Devuelvo la respuesta
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
        ''' <param name="id"></param>
        ''' <returns></returns>
        Function eliminarPagina(id As Int32)
            'respuesta por defecto
            Dim rpta = 0
            Try
                Dim db As New BaseDeDatosDataContext
                Dim registro = (From pagina In db.Pagina
                                Where pagina.IIDPAGINA = id
                                Select pagina).First()
                'Ponemos el registro a BHABILITADO = 0
                registro.BHABILITADO = 0
                db.SubmitChanges()
                rpta = 1
            Catch ex As Exception
                rpta = 0
            End Try
            Return rpta
        End Function

        ''' <summary>
        ''' Obtiene la informacion de una pagina por su id
        ''' </summary>
        ''' <param name="id">id de la pagina a buscar</param>
        ''' <returns></returns>
        Function recuperarInformacionPagina(id As Int32) As JsonResult
            'Instanciamos la base de datos
            Dim db As New BaseDeDatosDataContext
            'Obtenemos la pagina por su id
            Dim registro = From pagina In db.Pagina
                           Where pagina.IIDPAGINA = id
                           Select New With {
                               pagina.IIDPAGINA,
                               pagina.MENSAJE,
                               pagina.CONTROLADOR,
                               pagina.ACCION
                           }
            'Devolvemos el objeto pagina
            Return New JsonResult With {.Data = registro,
              .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function

        ''' <summary>
        ''' Metodo para buscar paginas por contenido
        ''' </summary>
        ''' <param name="mensaje">mensaje el cual buscar</param>
        ''' <returns></returns>
        Function buscarPaginas(mensaje As String) As JsonResult
            'Instanciamos la base de datos
            Dim db As New BaseDeDatosDataContext
            'Hacemos la consulta
            Dim listado = From pagina In db.Pagina
                          Where pagina.BHABILITADO = 1 And
                          pagina.MENSAJE.Contains(mensaje)
                          Select New With {
                              pagina.IIDPAGINA,
                              pagina.MENSAJE,
                              pagina.ACCION,
                              pagina.CONTROLADOR
            }
            'Devolvemos el objeto pagina
            Return New JsonResult With {.Data = listado,
              .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function


    End Class
End Namespace