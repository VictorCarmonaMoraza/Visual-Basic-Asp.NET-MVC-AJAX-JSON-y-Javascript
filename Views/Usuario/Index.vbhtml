@Code
    ViewData("Title") = "Index"
End Code
<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css">

<button onclick="abrirModal()" type="button" class="btn btn-primary" data-toggle="modal" data-target="#exampleModal">
    Agregar
</button>

<div style="display:grid;grid-template-columns:250px 300px 150px">
    <label>Ingrese nombre usuario</label>
    <input type="text" id="txtNombreUsuario" class="form-control" />
    <button class="btn btn-primary" onclick="buscarUsuario()">Buscar</button>
</div>

<div id="divUsuarios">

</div>

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
                    <label>Id Usuario</label>
                    <input name="Id Usuario" readonly id="txtIdUsuario" class="form-control limpiar  obligatorioedit" />
                </div>
                <div class="form-group">
                    <label>Ingrese nombre Usuario</label>
                    <input name="Nombre del Usuario" id="txtNombreUsuarioFormulario" class="form-control limpiar obligatorio obligatorioedit" />
                </div>
                <div class="form-group" id="divContraOcultar">
                    <label>Ingrese contraseña</label>
                    <input name="la contraseña" id="txtContra" type="password" class="form-control limpiar obligatorio" />
                </div>

                <div class="form-group" id="divPersonaOcultar">
                    <label>Seleccione Persona</label>
                    <select name="o seleccionar la persona" id="cboPersona" class="form-control limpiar obligatorio"></select>
                </div>

                <div class="form-group">
                    <label>Seleccione Tipo Usuario</label>
                    <select name="o seleccionar el tipo de usuario" id="cboTipoUsuario" class="form-control limpiar obligatorio obligatorioedit"></select>
                </div>



            </div>
            <div class="modal-footer">
                <button type="button" id="btnCerrar" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                <button type="button" onclick="Guardar()" class="btn btn-primary">Guardar</button>
            </div>
            <div id="divErrores" style="color:red;font-weight:bold">

            </div>
        </div>
    </div>
</div>

@*<script src="~/Scripts/jquery-1.10.2.min.js"></script>*@
@*<script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.js"></script>*@
@*<script src="~/scripts/Usuarios.js"></script>*@



<script src="~/Scripts/jquery-3.6.0.min.js"></script>
<script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.js"></script>
<script src="~/JS/Usuarios.js"></script>

