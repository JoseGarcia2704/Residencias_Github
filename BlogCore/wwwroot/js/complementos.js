var DataTable;

$(document).ready(function () {
    cargarDatatable();
})

function cargarDatatable() { // Corregido el nombre de la función
    DataTable = $("#tblComplementos").DataTable({ 
        "ajax": {
            "url": "/admin/complementos/GetAll",
            "type": "GET",
            "datatype": "json",

        },
        "columns": [
            { "data": "idComplemento", "width": "5%" },
            { "data": "uuidc", "width": "20%" },
            { "data": "monto", "width": "15%" },
            { "data": "saldoInsoluto", "width": "20%" },
            {
                "data": "idComplemento",
                "render": function (data) {
                    return `<div class="text-center">
                         <a href="/Admin/Complementos/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer;width:100px;">
                         <i class="far fa-edit"></i>&nbsp;Editar
                         </a>
                         &nbsp;
                          <a onclick=Delete("/Admin/Complementos/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                <i class="far fa-trash-alt"></i>&nbsp;Borrar
                                </a>
                                 &nbsp;
                          <a href="/Admin/Complementos/VerPdf/${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                               <i class="far fa-file-pdf"></i> &nbsp;Ver Pdf
                                </a>

                    </div>
                    `;
                }, "width": "45%"
            }

        ],
        // "order": [[2, 'asc']], // Ordenar por la tercera columna (orden) de manera ascendente
        "language": {
            "decimal": "",
            "emptyTable": "No hay registros",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "Sin resultados encontrados",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "width": "100%"

    });
}
function Delete(url) {
    swal({
        title: "Esta seguro de borrar?",
        text: "Este contenido no se puede recuperar!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, borrar!",
        closeOnconfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(data.message);
                }
            }
        });
    });
}