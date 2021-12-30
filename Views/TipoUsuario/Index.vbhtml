@Code
    ViewData("Title") = "Index"
End Code
<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css">


<button onclick="abrirModal()" type="button" class="btn btn-primary" data-toggle="modal" data-target="#exampleModal">
    Agregar
</button>

<div style="margin-top:10px">
    <label>Ingrese el nombre del Tipo de usuario</label>
    <input type="text" id="txtnombreTipoUsu" />
    <button class="btn btn-primary" onclick="BuscarTipoUsuario()">Buscar</button>
</div>



<div id="divTipoUsuario">

</div>

<!-- Modal -->
<div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="lblTitulo"></h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label>Id Tipo Usuario</label>
                    <input type="text" readonly id="txtIdTipousuario" class="form-control limpiar" />
                </div>
                <div class="form-group">
                    <label>Nombre Tipo Usuario</label>
                    <input type="text" name="Nombre Tipo Usuario" id="txtNombreTipoUsuario" class="form-control limpiar obligatorio" />
                </div>
                <div class="form-group">
                    <label>Descripciòn del Tipo Usuario</label>
                    <input type="text" name="Descripcion" id="txtdescripcion" class="form-control limpiar obligatorio" />
                </div>
                <div id="divPaginas">

                </div>
            </div>
            <div class="modal-footer">
                <button type="button" id="btnCerrar" class="btn btn-secondary" data-dismiss="modal">
                    Cerrar
                </button>
                <button type="button" onclick="Guardar()" class="btn btn-primary">Guardar</button>
            </div>
            <div id="divErrores" style="color:red;font-weight:bold">

            </div>

        </div>
    </div>
</div>


<script src="~/Scripts/jquery-3.6.0.min.js"></script>
<script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.js"></script>
<script src=" https://unpkg.com/sweetalert/dist/sweetalert.min.js "></script>
<script src="~/JS/TipoUsuario.js"></script>