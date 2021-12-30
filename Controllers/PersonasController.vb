Imports System.Web.Mvc

Namespace Controllers
    <Seguridad()>
    Public Class PersonasController
        Inherits Controller

        ' GET: Personas
        Function Index() As ActionResult
            Return View()
        End Function

        ''' <summary>
        ''' Metodo que nos devuelve un listado de personas
        ''' </summary>
        ''' <returns>listado de personas</returns>
        Function listaPersonas() As JsonResult
            'Instanciamos nuestra base de datos
            Dim db As New BaseDeDatosDataContext
            'Formamos la sentencia que traera los datos 
            Dim listadoPersonas = From personas In db.Persona
                                  Where personas.BHABILITADO = ConstantesProyecto.value1
                                  Select New With {
                                      personas.IIDPERSONA,
                                      .NOMBRECOMPLETO = personas.NOMBRE + " " + personas.APPATERNO + " " + personas.APMATERNO,
                                      personas.TELEFONO,
                                      personas.CORREO,
                                      .FECHANACIMIENTO = personas.FECHANACIMIENTO.Value.ToLongDateString()
                                  }
            Return New JsonResult With {.Data = listadoPersonas,
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
                                   .valor = sexo.NOMBRE
                                   }
            Return New JsonResult With {.Data = valoresCombo,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet
            }
        End Function

        ''' <summary>
        ''' Metodo para filtrar por genero
        ''' </summary>
        ''' <param name="iidsexo">Id del sexo</param>
        ''' <returns></returns>
        Function filtrarPorGenero(iidsexo As Int32) As JsonResult
            'instanciamos la base de datos
            Dim db As New BaseDeDatosDataContext
            Dim listadoPorGenero

            listadoPorGenero = From personas In db.Persona
                               Where personas.BHABILITADO = ConstantesProyecto.value1 And
                                     personas.IIDSEXO = iidsexo
                               Select New With {
                                      personas.IIDPERSONA,
                                      .NOMBRECOMPLETO = personas.NOMBRE + " " + personas.APPATERNO + " " + personas.APMATERNO,
                                      personas.TELEFONO,
                                      personas.CORREO,
                                      .FECHANACIMIENTO = personas.FECHANACIMIENTO.Value.ToShortDateString()
                                      }


            Return New JsonResult With {.Data = listadoPorGenero,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function

        ''' <summary>
        ''' Metodo que nos devuleve la informacion dela persona buscada por su id
        ''' </summary>
        ''' <param name="idPersona">Id de la persona a buscar</param>
        ''' <returns></returns>
        Function recuperarInformacionPersona(idPersona As Int32) As JsonResult

            Dim db As New BaseDeDatosDataContext
            'Creamos la consulta 
            Dim personaBuscada = From persona In db.Persona
                                 Where persona.IIDPERSONA = idPersona
                                 Select New With {
                                   persona.IIDPERSONA,
                                   persona.NOMBRE,
                                   persona.APMATERNO,
                                   persona.APPATERNO,
                                   persona.CORREO,
                                   persona.TELEFONO,
                                   .FECHANACIMIENTO = persona.FECHANACIMIENTO.Value.ToShortDateString(),
                                   persona.IIDSEXO
                                 }
            Return New JsonResult With {.Data = personaBuscada,
               .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function

        ''' <summary>
        ''' Metod para eliminar personas por su id, borrado logico
        ''' </summary>
        ''' <param name="idPersona">id de la persona a buscar</param>
        ''' <returns></returns>
        Function eliminarPersona(idPersona As Int32)
            'respuesta por defecto
            Dim rpta = ConstantesProyecto.valueRespuestaError
            Try
                'instanciamos a la base de datos
                Dim db As New BaseDeDatosDataContext
                'Registro para obtener la persona para despues poder borrar
                Dim registroAEliminar = (From persona In db.Persona
                                         Where persona.IIDPERSONA = idPersona
                                         Select persona).First()
                'Borrado logico
                registroAEliminar.BHABILITADO = ConstantesProyecto.value0
                db.SubmitChanges()
                rpta = ConstantesProyecto.valueRespuestaOK

            Catch ex As Exception
                rpta = ConstantesProyecto.valueRespuestaError
            End Try
            Return rpta
        End Function

        ''' <summary>
        ''' Metodo para guardar el objeto persona en la base de datos
        ''' </summary>
        ''' <param name="modelPersona">modelo de persona</param>
        ''' <returns></returns>
        Function GuardarPersonas(modelPersona As Persona)
            'Respuesta por feceto
            Dim rpta = ConstantesProyecto.valueRespuestaError
            'Obtenemos el id de la persona
            Dim idPersona = modelPersona.IIDPERSONA
            'instanciamos la base de datos
            Dim db As New BaseDeDatosDataContext
            Try
                'Estamos en modo creacion de persona
                If idPersona = 0 Then
                    'Consulta lambda
                    Dim cantidad As Int32 = db.Persona.Where(Function(x) x.NOMBRE.ToUpper() = modelPersona.NOMBRE.ToUpper()).Count()
                    If cantidad >= 1 Then
                        Return -1
                    End If


                    db.Persona.InsertOnSubmit(modelPersona)
                    db.SubmitChanges()
                    rpta = ConstantesProyecto.valueRespuestaOK
                Else
                    'EDITAR
                    'Consulta lambda
                    Dim cantidad As Int32 = db.Persona.Where(Function(x) x.NOMBRE.ToUpper() = modelPersona.NOMBRE.ToUpper() And x.IIDPERSONA <> modelPersona.IIDPERSONA).Count()
                    If cantidad >= 1 Then
                        Return -1
                    End If
                    Dim registro = (From persona In db.Persona
                                    Where persona.IIDPERSONA = idPersona
                                    Select persona).First()
                    'Actualizamos los registros
                    registro.NOMBRE = modelPersona.NOMBRE
                    registro.APPATERNO = modelPersona.APPATERNO
                    registro.APMATERNO = modelPersona.APMATERNO
                    registro.TELEFONO = modelPersona.TELEFONO
                    registro.CORREO = modelPersona.CORREO
                    registro.IIDSEXO = modelPersona.IIDSEXO
                    registro.FECHANACIMIENTO = modelPersona.FECHANACIMIENTO

                    'confirmamos los cambios
                    db.SubmitChanges()
                    rpta = ConstantesProyecto.valueRespuestaOK
                End If

            Catch ex As Exception
                rpta = ConstantesProyecto.valueRespuestaError
            End Try
            Return rpta
        End Function

        '''' <summary>
        '''' Obtiene la informacion d euna persona por su id
        '''' </summary>
        '''' <param name="id"></param>
        '''' <returns></returns>
        'Function reuperarInformacionPersona(id As Int32) As JsonResult
        '    'Instanciamos la base de datos 
        '    Dim db As New BaseDeDatosDataContext
        '    'Buscamos el registro con esa informacion
        '    Dim registro = From persona In db.Persona
        '                   Where persona.IIDPERSONA = id
        '                   Select New With {
        '                       persona.IIDPERSONA,
        '                       persona.NOMBRE,
        '                       persona.APMATERNO,
        '                       persona.APPATERNO,
        '                       .FECHABASEDEDATOS = persona.FECHANACIMIENTO.Value.ToShortDateString(),
        '                       persona.CORREO,
        '                       persona.TELEFONO,
        '                       persona.IIDSEXO
        '                   }
        '    'Devolvemos la consulta
        '    Return New JsonResult With {.Data = registro,
        '       .JsonRequestBehavior = JsonRequestBehavior.AllowGet}

        'End Function

    End Class
End Namespace