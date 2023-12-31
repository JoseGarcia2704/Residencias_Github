﻿var dataTable;

$(document).ready(function () {
    cargarDatatable();
})

function cargarDatatable() {
    dataTable = $("#tblProveedores").DataTable({
        "ajax": {
            "url": '/admin/proveedores/GetAll',
            "type": "GET",
            "datatype": "json"
        },

        "columns": [
            { "data": "ordenCompra", "width": "5%" },
            { "data": "fechaRegistro", "width": "5%" },
            { "data": "solicitante", "width": "5%" },
            { "data": "moneda", "width": "5%" },
            { "data": "monto", "width": "5%" },
            { "data": "folio", "width": "5%" },
            { "data": "estatus", "width": "5%" },
            { "data": "fechaPago", "width": "5%" },
            { "data": "nombreProveedor", "width": "5%" },
            { "data": "notas", "width": "5%" },
            { "data": "comentariosSeguimiento", "width": "5%" },
            { "data": "complemento", "width": "5%" },


            {
                "data": "ordenCompra",
                "render": function (data) {
                    return `<div class="text-center">
                         <a href="/Admin/Proveedores/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer;width:80px;font-size: 15px; text-align: left;">
                         <i class="far fa-edit"></i>Editar
                         </a>
                         &nbsp;
                          <a onclick=Delete("/Admin/Proveedores/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer; width:80px; font-size: 15px; text-align: left;">
                                <i class="far fa-trash-alt"></i>Borrar
                                </a>

                    </div>
                    `;
                },"width":"25%"
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
        "width": "100%",
        "initComplete": function () {
            console.log('Data loaded:', dataTable.ajax.json());
        }
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