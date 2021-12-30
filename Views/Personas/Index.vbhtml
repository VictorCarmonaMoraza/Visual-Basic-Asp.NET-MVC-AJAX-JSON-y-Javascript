@Code
    ViewData("Title") = "Index"
End Code
<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.11.0/css/jquery.dataTables.css">
<link href="~/Content/jquery-ui.min.css" rel="stylesheet" />
<!--Boton del Modal-->
<button onclick="abrirModalPersonas()" type="button" class="btn btn-primary" data-toggle="modal" data-target="#exampleModal" style="margin-top:10px">
    Agregar
</button>

<!--div para pintar el combo-->
<div style="display:grid; grid-template-columns: 150px 300px 150px; margin-top: 10px">
    <label>Seleccione Sexo</label>
    <select id="comboSexo" class="form-control"></select>
    <button onclick="buscarSexoPorNombre()" class="btn btn-primary">Buscar</button>
</div>

<!--div para pintar la tabla-->
<div id="divPersonas">

</div>

<!-- Modal -->
<div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="tituloModalPersona"></h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <!--Id Persona-->
                <div class="form-group">
                    <label>Ingrese Id Persona</label>
                    <input readonly id="txtIdPersona" class="form-control limpiar obligatorio" />
                </div>
                <!--NOTA: sino pones type="text" por defecto es text-->
                <!--Nombre-->
                <div class="form-group">
                    <label>Nombre</label>
                    <input name="Nombre" type="text" id="txtNombre" class="form-control limpiar obligatorio" />
                </div>
                <!--Apellido Paterno-->
                <div class="form-group">
                    <label>Apellido Paterno</label>
                    <input name="Apellido Paterno" type="text" id="txtApPaterno" class="form-control limpiar obligatorio" />
                </div>

                <!--Apellido Materno-->
                <div class="form-group">
                    <label>Apellido Materno</label>
                    <input name="Apellido Materno" type="text" id="txtApMaterno" class="form-control limpiar obligatorio" />
                </div>

                <!--Telefono-->
                <div class="form-group">
                    <label>Telefono</label>
                    <input name="Telefono" type="number" id="txtTelefono" class="form-control limpiar obligatorio" />
                </div>

                <!--Email-->
                <div class="form-group">
                    <label>Email</label>
                    <input name="Correo" type="email" id="txtEmail" class="form-control limpiar obligatorio" />
                </div>

                <!--Fecha de Nacimiento-->
                <div class="form-group">
                    <label>Fecha de Nacimiento</label>
                    <input name="Fecha de nacimiento" type="text" id="txtFechaNacimiento" class="form-control limpiar obligatorio" />
                </div>

                <!--Fecha de Nacimiento-->
                <div class="form-groupl">
                    <label>Sexo</label>
                    <select name="sexo" id="comboSexoPersonasFormulario" class="form-control limpiar obligatorio"></select>
                </div>

            </div>
            <div class="modal-footer">
                <button type="button" id="btnCerrar" class="btn btn-secondary" data-dismiss="modal">Close</button>
                <button type="button" onclick="GuardarPersona()" class="btn btn-primary">Save changes</button>
            </div>

            <div id="divErroresPersona" style="font-weight: bold; color: red">

            </div>
        </div>
    </div>
</div>
<!--Fin Modal-->



<script src="~/Scripts/jquery-3.6.0.min.js"></script>
<script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.11.0/js/jquery.dataTables.js"></script>
<script src="~/Scripts/jquery-ui.min.js"></script>
<script src="~/JS/persona.js"></script>
