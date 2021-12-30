@Code
    ViewData("Title") = "Index"
End Code
<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css">
<link href="~/Content/jquery-ui.min.css" rel="stylesheet" />
<input value="@ViewBag.iidvista" type="hidden" id="txtIdVista" />

<!--div Padre-->
<div style="display:grid; grid-template-columns:2fr 3fr">
    <!--Grilla 1-->
    <div>
        <div>
            <h1>Crear Cita</h1>
            <div class="form-group">
                <label>Tipo Mascota</label>
                <select name="tipo mascota" class="form-control obligatorio limpiar" id="cboTipoMascota" onchange="mostrarMascota()"></select>
            </div>

            <div class="form-group">
                <label>Mascota</label>
                <select name="nombre de la mascota" class="form-control obligatorio limpiar" id="cboMascota"></select>
            </div>

            <div class="form-group">
                <label>Fecha enfermedad</label>
                <input name="fecha en que se enfermo su mascota" type="text" id="txtfechaEnfermedad"
                       class="form-control obligatorio limpiar" />
            </div>

            <div class="form-group">
                <label>Sede</label>
                <select name="sede donde se quiere atender" class="form-control obligatorio limpiar"
                        id="cboSede"></select>
            </div>

            <div class="form-group">
                <label>Descripcion de la enfermedad</label>
                <textarea id="txtdescripcion" name="Descripcion"
                          class="form-control obligatorio limpiar" rows="10" cols="50"></textarea>
            </div>
        </div>
    </div>

    <!--Grilla 2-->
    <div>
        <h1>Citas</h1>

        <select id="cboEstadoCita" onchange="filtrarCitaPorEstado()"
                class="form-control" style="margin-bottom:1em"></select>

        <div id="tablaCita">

        </div>
    </div>
</div>



<button class="btn btn-primary" onclick="Guardar()">Crear</button>
<button class="btn btn-danger" onclick="limpiar()">Limpiar</button>

<!--Div para mostrar los errores-->
<div id="divErrores" style="font-weight:bold;color:red">

</div>


<!-- Modal -->
<div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog  modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="lblTitulo"></h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <input id="txtCitaPopup" type="hidden" value="" />
                <div id="tablaHistorial">

                </div>
                <!--Cuadro de texto para introducir observacion-->
                <div id="formObservacion">
                    <label>Ingrese el motivo de la anulación</label>
                    <textarea id="txtmotivo" name="Descripcion"
                              class="form-control limpiar" rows="10" cols="50"></textarea>
                </div>
                <!--Cuadro de texto para introducir -->
                <div id="formAsignar">
                    <div class="form-group">
                        <label>Ingrese Doctor a Atender</label>
                        <select id="cboDoctor" class="form-control"></select>
                    </div>
                    <div class="form-group">
                        <label>Ingrese Fecha a Atender</label>
                        <input type="datetime-local" id="txtfechaAtencion" class="form-control" />
                    </div>
                    <!--Precio-->
                    <div class="form-group">
                        <label>Precio</label>
                        <input type="text" id="txtprecio" class="form-control" />
                    </div>
                </div>

            </div>
            <div class="modal-footer">
                <button type="button" id="btnCerrar" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                <button id="btnGuardarObservacion" onclick="GuardarObservacion()" type="button" class="btn btn-primary">Guardar Observacion</button>
                <button id="btnGuardarAsignacion" onclick="GuardarAsignacion()" type="button" class="btn btn-primary">Guardar Asignacion</button>
            </div>
        </div>
    </div>
</div>
<!--Fin modal-->


<script src="~/Scripts/jquery-3.6.0.min.js"></script>
<script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.11.0/js/jquery.dataTables.js"></script>
<script src="~/scripts/jquery-ui.min.js"></script>
<script src="~/JS/cita.js"></script>
