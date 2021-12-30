@Code
    ViewData("Title") = "Index"
End Code
<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.11.0/css/jquery.dataTables.css">
<link href="~/Content/jquery-ui.min.css" rel="stylesheet" />

<!--Boton para agregar elementos mediante un modal-->
<button type="button" class="btn btn-primary" data-toggle="modal" data-target="#exampleModal" onclick="abrirModalMedicamento()" style="margin-bottom:10px">
    Agregar
</button>

<div id="contenidoMedicamentos">

</div>

<!-- Modal -->
<div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="tituloModalMedicamento"></h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <!--Seccion de agregado de datos-->
                <!--Id-->
                <div class="form-group">
                    <label>Id Medicamento</label>
                    <input id="txtIdMedicamento" readonly class="form-control limpiar" />
                </div>
                <!--Nombre del medicamento-->
                <div class="form-group">
                    <label>Nombre Medicamento</label>
                    <input id="txtNombreMedicamento" name="nombreMedicamento" class="form-control limpiar obligatorio" />
                </div>
                <!--Concentracion del medicamento-->
                <div class="form-group">
                    <label>Concentración</label>
                    <input id="txtConcentracion" name="concentracion" class="form-control limpiar obligatorio" />
                </div>
                <!--Precio del medicamento-->
                <div class="form-group">
                    <label>Precio</label>
                    <input id="txtPrecio" name="precioMedicamento" class="form-control limpiar obligatorio" />
                </div>
                <!--Stock del medicamento-->
                <div class="form-group">
                    <label>Stock</label>
                    <input id="txtStock" name="stockMedicamento" class="form-control limpiar obligatorio" />
                </div>
                <!--Fin seccion de agregado de datos-->
            </div>
            <div class="modal-footer">
                <button type="button" id="btncerrar" class="btn btn-secondary" data-dismiss="modal">Close</button>
                <button type="button" class="btn btn-primary" onclick="GuardarMedicamento()">Guardar</button>
            </div>

            <div id="divErrorMedicamento" style="color:red; font-weight:bold">

            </div>

        </div>
    </div>
</div>
<!--Fin Modal-->


<script src="~/Scripts/jquery-3.6.0.min.js"></script>
<script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.11.0/js/jquery.dataTables.js"></script>
<script src="~/JS/medicamento.js"></script>
