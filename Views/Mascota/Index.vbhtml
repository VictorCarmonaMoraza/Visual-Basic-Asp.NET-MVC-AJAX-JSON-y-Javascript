@Code
    ViewData("Title") = "Index"
End Code
<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.11.0/css/jquery.dataTables.css">
<link href="~/Content/jquery-ui.min.css" rel="stylesheet" />
@*<link rel="stylesheet" href="sweetalert2.min.css">*@

<div style="display:grid; grid-template-columns: 180px 200px; margin-top: 1em">
    <label>Seleccione el tipo de mascota</label>
    <select id="comboTipoMascota" onchange="filtrarMascotaPorTipo()" class="form-control"></select>
</div>

<!--Boton para agregar elementos mediante un modal-->
<button type="button" class="btn btn-primary" data-toggle="modal" data-target="#exampleModal" onclick="abrirModalMascotas()" style="margin-bottom:10px">
    Agregar
</button>

<div id="contenidoMascotas">

</div>

<!-- Modal -->
<div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="tituloModalMascota"></h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <!--Seccion de agregado de datos-->
                <!--Id de mascota-->
                <div class="form-group">
                    <label>Id Mascota</label>
                    <input id="txtIdMascota" name="Id mascota" readonly class="form-control limpiar" />
                </div>
                <!--Nombre maacota-->
                <div class="form-group">
                    <label>Nombre Mascota</label>
                    <input id="txtNombre" name="Nombre Mascota" class="form-control limpiar obligatorio" />
                </div>
                <!--Tipo Mascota-->
                <div class="form-group">
                    <label>Selecciona Tipo Mascota</label>
                    <select id="cboTipoMascotaFormulario" name="Tipo MAscota" class="form-control limpiar obligatorio"></select>
                </div>
                <!--Fecha de Nacimiento-->
                <div class="form-group">
                    <label>Fecha de nacimiento</label>
                    <input id="txtFechaNac" name="Fecha Nacimiento" class="form-control limpiar obligatorio" />
                </div>
                <!--Ancho-->
                <div class="form-group">
                    <label>Ancho</label>
                    <input id="txtAncho" name="Ancho de la mascota" class="form-control limpiar obligatorio" />
                </div>
                <!--Altura-->
                <div class="form-group">
                    <label>Altura</label>
                    <input id="txtAltura" name="Alto de la mascota" class="form-control limpiar obligatorio" />
                </div>
                <!--sexo mascota-->
                <div class="form-group">
                    <label>Sexo</label>
                    <select id="cboSexo" name="Sexo de la mascota" class="form-control limpiar obligatorio"></select>
                </div>
                <!--Fin seccion de agregado de datos-->
            </div>
            <div class="modal-footer">
                <button type="button" id="btncerrar" class="btn btn-secondary" data-dismiss="modal">Close</button>
                <button type="button" class="btn btn-primary" onclick="Guardar()">Save changes</button>
            </div>

            <div id="divErroresFormularioMascota" style="color:red; font-weight:bold">

            </div>

        </div>
    </div>
</div>
<!--Fin Modal-->

<script src="~/Scripts/jquery-3.6.0.min.js"></script>
<script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.11.0/js/jquery.dataTables.js"></script>
<script src="~/Scripts/jquery-ui.min.js"></script>
<script src = " https://unpkg.com/sweetalert/dist/sweetalert.min.js " > </script >
<script src="~/JS/mascota.js"></script>
@*<script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>*@
@*< script src = " https://cdn.bootcss.com/sweetalert/1.1.3/sweetalert.min.js " > < / script >*@


