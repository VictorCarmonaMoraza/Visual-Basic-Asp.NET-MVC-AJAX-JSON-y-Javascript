Imports System.Web.Mvc

Namespace Controllers
    <Seguridad()>
    Public Class TipoUsuarioController
        Inherits Controller

        ' GET: TipoUsuario
        Function Index() As ActionResult
            Return View()
        End Function

        ''' <summary>
        ''' Devuelve un listado con todos los tipos de usuario que esten habilitados
        ''' </summary>
        ''' <returns>listad de usuarios</returns>
        Function listarTipoUsuario() As JsonResult
            '1-Instanciamos la llamada a la base de datos
            Dim db As New BaseDeDatosDataContext
            '2-Creamos la query para traernos los datos
            Dim listarTipoUsu = From tipoUsuario In db.TipoUsuario
                                Where tipoUsuario.BHABILITADO = 1 'Nos traemos solo los que esten habilitados
                                Select New With { 'los datos que nos traemos 
                                    tipoUsuario.IIDTIPOUSUARIO,
                                    tipoUsuario.NOMBRE,
                                    tipoUsuario.DESCRIPCION
                                }

            Return New JsonResult With {.Data = listarTipoUsu,
             .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function

        ''' <summary>
        ''' Metodo para filtrar por nombre 
        ''' </summary>
        ''' <param name="nombreTipoUsuario"></param>
        ''' <returns></returns>
        Function filtrarTipoUsuario(nombreTipoUsuario As String) As JsonResult
            'instanciamos la base de datos
            Dim db As New BaseDeDatosDataContext
            Dim listarTipoUsuario
            If nombreTipoUsuario = "" Then
                listarTipoUsuario = From tipoUsuario In db.TipoUsuario
                                    Where tipoUsuario.BHABILITADO = 1
                                    Select New With {
                                        tipoUsuario.IIDTIPOUSUARIO,
                                        tipoUsuario.NOMBRE,
                                        tipoUsuario.DESCRIPCION
                                    }
            Else
                listarTipoUsuario = From tipoUsuario In db.TipoUsuario
                                    Where tipoUsuario.BHABILITADO = 1 And
                                    tipoUsuario.NOMBRE.Contains(nombreTipoUsuario)
                                    Select New With {
                                        tipoUsuario.IIDTIPOUSUARIO,
                                        tipoUsuario.NOMBRE,
                                        tipoUsuario.DESCRIPCION
                                    }
            End If

            Return New JsonResult With {.Data = listarTipoUsuario,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function

        ''' <summary>
        ''' Metodo para la insercion de un tipo de usuario
        ''' </summary>
        ''' <param name="oTipoUsuario">modelo de tipo de usaurio</param>
        ''' <returns></returns>
        Function insertarTipoUsuario(oTipoUsuario As TipoUsuario, IDS As String, CBOS As String)
            Dim rpta = ConstantesProyecto.valueRespuestaError
            Try
                Dim db As New BaseDeDatosDataContext

                Using transaccion As New Transactions.TransactionScope

                    Dim idTipoUsuario = oTipoUsuario.IIDTIPOUSUARIO
                    If idTipoUsuario = 0 Then
                        Dim cantidad As Int32 = db.TipoUsuario.Where(Function(p) p.NOMBRE.ToUpper() = oTipoUsuario.NOMBRE.ToUpper()).Count()
                        If cantidad >= 1 Then
                            'Significa que existe en la base de datos
                            Return -1
                        End If
                        'Validaremos que no exista el nombre del tipo de usuario en la base de datos
                        'Utilizaremos una expresion lambda


                        'Dim cantidad As Int32 = db.TipoUsuario.Where(Function(p) p.NOMBRE.ToUpper() = oTipoUsuario.NOMBRE.ToUpper()).Count()
                        'If cantidad >= 1 Then
                        '    Return -1
                        'End If
                        ''Voy a verificar que no exista el nombre del tipo de usuario en la BD administrador    ADMINISTRADOR

                        db.TipoUsuario.InsertOnSubmit(oTipoUsuario)
                        db.SubmitChanges()
                        'Luego del SubmitChanges
                        Dim iidTipoUsuario = oTipoUsuario.IIDTIPOUSUARIO
                        If IDS <> "" Then
                            '6*7*9  SPLIT('*')
                            '[6,7,9]
                            Dim arrayIds As String() = IDS.Split("*")
                            Dim arrayCbos As String() = CBOS.Split("*")

                            For i As Integer = 0 To arrayIds.Length - 1
                                Dim oPaginaTipoUsuario As New PaginaTipoUsuario
                                oPaginaTipoUsuario.IIDPAGINA = Integer.Parse(arrayIds(i))
                                oPaginaTipoUsuario.IIDTIPOUSUARIO = iidTipoUsuario
                                oPaginaTipoUsuario.BHABILITADO = ConstantesProyecto.BHABILITADO_OK

                                db.PaginaTipoUsuario.InsertOnSubmit(oPaginaTipoUsuario)
                                db.SubmitChanges()

                            Next
                            transaccion.Complete()
                            rpta = ConstantesProyecto.valueRespuestaOK
                        End If
                    Else
                        'Comprobarmos que no exista ese nombre de tipo usuario cuando estamos editando
                        Dim cantidad As Int32 = db.TipoUsuario.Where(Function(p) p.NOMBRE.ToUpper() = oTipoUsuario.NOMBRE.ToUpper() And p.IIDTIPOUSUARIO <> oTipoUsuario.IIDTIPOUSUARIO).Count()

                        If cantidad >= 1 Then
                            Return -1
                        End If

                        Dim registro = (From tipoUsu In db.TipoUsuario
                                        Where tipoUsu.IIDTIPOUSUARIO = idTipoUsuario
                                        Select tipoUsu).First()

                        registro.NOMBRE = oTipoUsuario.NOMBRE
                        registro.DESCRIPCION = oTipoUsuario.DESCRIPCION
                        db.SubmitChanges()

                        'Vamos a deshabilitar todas las paginas asociadas al tipo de usuario que queremos editar
                        Dim listaTotal = (From tipoUsupagina In db.PaginaTipoUsuario
                                          Where tipoUsupagina.IIDTIPOUSUARIO = idTipoUsuario
                                          Select tipoUsupagina).ToList()

                        For Each opaginaTipo As PaginaTipoUsuario In listaTotal
                            'Deshabilitamos todos registros de pagina
                            opaginaTipo.BHABILITADO = ConstantesProyecto.BHABILITADO_NO_OK
                            'Guardamos los cambios
                            db.SubmitChanges()
                        Next

                        Dim arrayIds As String() = IDS.Split("*")
                        Dim arrayCbos As String() = CBOS.Split("*")
                        For i As Integer = 0 To arrayIds.Length - 1
                            'consulta para obtener los registros a habilitar
                            Dim registroAHabilitar = (From oPaginaTipoUsu In db.PaginaTipoUsuario
                                                      Where oPaginaTipoUsu.IIDTIPOUSUARIO = idTipoUsuario And
                                                      oPaginaTipoUsu.IIDPAGINA = Integer.Parse(arrayIds(i))
                                                      Select oPaginaTipoUsu).ToList()
                            'Si es mayor que cero es que esta en la base de datos
                            If registroAHabilitar.Count > 0 Then
                                'Habilitamos el primer elemento de cada pasada
                                registroAHabilitar.First().BHABILITADO = ConstantesProyecto.BHABILITADO_OK
                                registroAHabilitar.First().IIDVISTA = arrayCbos(i)
                                db.SubmitChanges()
                                'sino existe debemos crear una nueva paginaTipoUsuario
                            Else
                                Dim oPaginaTipoUsuario As New PaginaTipoUsuario
                                'creamos el nuevo oPaginaTipoUsuario seleccionado
                                oPaginaTipoUsuario.IIDTIPOUSUARIO = idTipoUsuario
                                oPaginaTipoUsuario.IIDPAGINA = Integer.Parse(arrayIds(i))
                                oPaginaTipoUsuario.BHABILITADO = ConstantesProyecto.BHABILITADO_OK
                                oPaginaTipoUsuario.IIDVISTA = arrayCbos(i)
                                db.PaginaTipoUsuario.InsertOnSubmit(oPaginaTipoUsuario)
                                db.SubmitChanges()
                            End If
                        Next

                        transaccion.Complete()
                        rpta = ConstantesProyecto.valueRespuestaOK

                    End If

                End Using

            Catch ex As Exception
                rpta = ConstantesProyecto.valueRespuestaError
            End Try

            Return rpta

        End Function

        ''' <summary>
        ''' recupera informacion de un tipo usuario por su id
        ''' </summary>
        ''' <param name="id">id a buscar</param>
        ''' <returns></returns>
        Function recuperarInformacionTipoUsuario(id As Int32) As JsonResult
            'instanciamos la base de datos
            Dim db As New BaseDeDatosDataContext
            'buscamos la informacion del usuario por su id
            Dim registro = From tipoUsu In db.TipoUsuario
                           Where tipoUsu.IIDTIPOUSUARIO = id
                           Select New With {
                               tipoUsu.IIDTIPOUSUARIO,
                               tipoUsu.NOMBRE,
                               tipoUsu.DESCRIPCION
            }
            'Retornamos el registro encontrado
            Return New JsonResult With {.Data = registro,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function


        Function listarPaginas() As JsonResult
            'Instanciamos la base de datos
            Dim db As New BaseDeDatosDataContext

            Dim listado = From pagina In db.Pagina
                          Where pagina.BHABILITADO = ConstantesProyecto.BHABILITADO_OK
                          Select New With {
                              pagina.IIDPAGINA,
                              pagina.MENSAJE
                           }
            Return New JsonResult With {.Data = listado,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet
            }
        End Function


        ''' <summary>
        ''' Recuperamos todos los check marcados en un usuario
        ''' </summary>
        ''' <param name="id">id del tipo de usuario del cual vamos a recuperar los datos</param>
        ''' <returns></returns>
        Function recuperarCheckMarcados(id As Int32) As JsonResult
            'Conectamos con nuestra base de datos
            Dim db As New BaseDeDatosDataContext
            'Preparamos consulta
            Dim registros = From paginaTipoUsu In db.PaginaTipoUsuario
                            Where paginaTipoUsu.IIDTIPOUSUARIO = id And
                                paginaTipoUsu.BHABILITADO = ConstantesProyecto.BHABILITADO_OK
                            Select New With {'nos traemos los datos que queremos
                                 paginaTipoUsu.IIDPAGINA,
                                 paginaTipoUsu.IIDVISTA
                             }
            'rRetornamos los registros
            Return New JsonResult With {.Data = registros,
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function


        ''' <summary>
        ''' Metodo para elminar por su id
        ''' </summary>
        ''' <param name="id">id del tipo usuario a elminar</param>
        ''' <returns></returns>
        Function eliminar(id As Int32)

            Dim rpta = ConstantesProyecto.valueRespuestaError
            Try
                Dim db As New BaseDeDatosDataContext
                Dim registro = (From tipoUsu In db.TipoUsuario
                                Where tipoUsu.IIDTIPOUSUARIO = id
                                Select tipoUsu).First()
                registro.BHABILITADO = ConstantesProyecto.BHABILITADO_NO_OK
                db.SubmitChanges()
                rpta = ConstantesProyecto.valueRespuestaOK

            Catch ex As Exception
                rpta = ConstantesProyecto.valueRespuestaError
            End Try

            Return rpta



        End Function

    End Class
End Namespace