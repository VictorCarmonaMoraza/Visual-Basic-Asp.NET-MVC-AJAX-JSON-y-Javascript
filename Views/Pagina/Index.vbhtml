@Code
    ViewData("Title") = "Index"
End Code
<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.11.0/css/jquery.dataTables.css">


<!--filtro-->
<div style="display:grid; grid-template-columns: 150px 300px 150px; margin-top:2em">
    <label>Ingrese mensaje</label>
    <input type="text" class="form-control" id="txtMensajeBusqueda" onkeypress="filtrarPagina()" />
</div>

    <!--Boton para agregar elementos mediante un modal-->
    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#exampleModal" onclick="abrirModalPagina()" style="margin-bottom:10px">
        Agregar
    </button>

    <!--Aqui ira el contenido de la pagina-->
    <div id="contenidoPagina">

    </div>

    <!-- Modal -->
    <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="tituloModalPagina"></h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <!--Seccion de agregado de datos-->
                    <!--Id-->
                    <div class="form-group">
                        <label>Id Pagina</label>
                        <input id="txtIdPagina" readonly class="form-control limpiar" />
                    </div>
                    <!--Mensaje-->
                    <div class="form-group">
                        <label>Mensaje</label>
                        <input id="txtmensaje" name="mensaje" class="form-control limpiar obligatorio" />
                    </div>
                    <!--Accion-->
                    <div class="form-group">
                        <label>Accion</label>
                        <input id="txtaccion" name="accion" class="form-control limpiar obligatorio" />
                    </div>
                    <!--Controlador-->
                    <div class="form-group">
                        <label>Controlador</label>
                        <input id="txtcontrolador" name="controlador" class="form-control limpiar obligatorio" />
                    </div>
                    <!--Fin seccion de agregado de datos-->
                </div>
                <div class="modal-footer">
                    <button type="button" id="btncerrar" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary" onclick="Guardar()">Save changes</button>
                </div>

                <div id="divErroresPaginas" style="color:red; font-weight:bold">

                </div>

            </div>
        </div>
    </div>
    <!--Fin Modal-->

    <script src="~/Scripts/jquery-3.6.0.min.js"></script>
    <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.11.0/js/jquery.dataTables.js"></script>
    <script src="~/JS/pagina.js"></script>
