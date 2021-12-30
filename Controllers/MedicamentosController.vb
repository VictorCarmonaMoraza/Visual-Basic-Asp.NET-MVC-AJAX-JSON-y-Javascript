Imports System.Web.Mvc

Namespace Controllers
    <Seguridad()>
    Public Class MedicamentosController
        Inherits Controller

        ' GET: Medicamentos
        Function Index() As ActionResult
            Return View()
        End Function

        ''' <summary>
        ''' Metodo para obtener todo el listado de medicamentos que estan habilitados
        ''' </summary>
        ''' <returns></returns>
        Function listarMedicamentos() As JsonResult
            'Instanciamos la base de datos
            Dim db As New BaseDeDatosDataContext
            'Preparamos la query 
            Dim listadoMedicamentos = From medicamento In db.Medicamento
                                      Where medicamento.BHABILITADO = 1
                                      Select New With {
                                          medicamento.IIDMEDICAMENTO,
                                          medicamento.NOMBRE,
                                          medicamento.CONCENTRACION,
                                          medicamento.PRECIO,
                                          medicamento.STOCK
                                      }
            Return New JsonResult With {.Data = listadoMedicamentos,
                                  .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function

        ''' <summary>
        ''' Elimina un medicamento por su id(Borrado lógico)
        ''' </summary>
        ''' <param name="id">id del medicamento</param>
        ''' <returns></returns>
        Function eliminarMedicamento(id As Int32) As Int32
            Dim rpta = ConstantesProyecto.valueRespuestaError
            Try
                'Instanciamos la base de datos
                Dim db As New BaseDeDatosDataContext
                Dim registroAeliminar = (From medicamento In db.Medicamento
                                         Where medicamento.IIDMEDICAMENTO = id
                                         Select medicamento).First()

                'Hacemos borrado logico, lo que es poner el registro a cero
                registroAeliminar.BHABILITADO = ConstantesProyecto.BHABILITADO_NO_OK
                'Guardamos los cambios
                db.SubmitChanges()
                'MAndamos la respuesta de todo correcto
                rpta = ConstantesProyecto.valueRespuestaOK
            Catch ex As Exception
            End Try
            Return rpta
        End Function

        ''' <summary>
        ''' Guarda o edita un medicamento en funcion de su id
        ''' </summary>
        ''' <param name="modelMedicamento">modelo de medicamento</param>
        ''' <returns></returns>
        Function guardarMedicamento(modelMedicamento As Medicamento) As Int32
            Dim rpta = ConstantesProyecto.valueRespuestaError
            Try
                'Instanciamos la base de datos
                Dim db As New BaseDeDatosDataContext
                Dim idMedicamento = modelMedicamento.IIDMEDICAMENTO

                'Creacion 
                If idMedicamento = 0 Then
                    'Insertamos el medicamento
                    db.Medicamento.InsertOnSubmit(modelMedicamento)
                    'Devolvemos respuesta 
                    rpta = ConstantesProyecto.valueRespuestaOK
                    'Guardamos los cambios
                    db.SubmitChanges()
                Else
                    Dim registroAEditar = (From medicamento In db.Medicamento
                                           Where medicamento.IIDMEDICAMENTO = modelMedicamento.IIDMEDICAMENTO
                                           Select medicamento).First()

                    registroAEditar.NOMBRE = modelMedicamento.NOMBRE
                    registroAEditar.PRECIO = modelMedicamento.PRECIO
                    registroAEditar.STOCK = modelMedicamento.STOCK
                    registroAEditar.CONCENTRACION = modelMedicamento.CONCENTRACION

                    'Actualizamos los cambios
                    db.SubmitChanges()
                    'Devolvemos respuesta
                    rpta = ConstantesProyecto.valueRespuestaOK
                End If
            Catch ex As Exception
            End Try
            Return rpta
        End Function


        ''' <summary>
        ''' Recupera la informacion de un medicamento
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Function recuperarInformacionMedicamento(id As Int32) As JsonResult
            'Instanciamos la base de datos
            Dim db As New BaseDeDatosDataContext

            Dim registro = (From medicamento In db.Medicamento
                            Where medicamento.IIDMEDICAMENTO = id
                            Select New With {
                               medicamento.IIDMEDICAMENTO,
                               medicamento.NOMBRE,
                               medicamento.PRECIO,
                               medicamento.STOCK,
                               medicamento.CONCENTRACION
                           }).First()
            Return New JsonResult With {.Data = registro,
            .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function
    End Class
End Namespace