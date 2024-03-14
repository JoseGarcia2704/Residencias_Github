var dataTable;

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
            { "data": "estatus", "width": "3%" },
            { "data": "ordenCompra", "width": "3%" },
            { "data": "fechaRegistro", "width": "3%" },
            { "data": "solicitante", "width": "3%" },
            { "data": "moneda", "width": "3%" },
            { "data": "monto", "width": "3%" },
            { "data": "folio", "width": "3%" },
            { "data": "fechaPago", "width": "3%" },
            { "data": "nombreProveedor", "width": "3%" },
            { "data": "notas", "width": "3%" },
            { "data": "comentariosSeguimiento", "width": "3%" },
            { "data": "statusComplemento", "width": "3%" },
            


            {
                "data": "idProveedor",
                "render": function (data) {
                    if (isAdmin) {
                        return `<div class="text-center">
    <a href="/Admin/Proveedores/Edit/${data}" class="w-100 btn btn-lg btn-outline-primary" style="cursor:pointer;width:60px;font-size: 12px; text-align: left;">
    <i class="far fa-edit"></i>Editar
    </a>
    &nbsp;
    <a onclick=Delete("/Admin/Proveedores/Delete/${data}") class="w-100 btn btn-lg btn-outline-primary" style="cursor:pointer; width:60px; font-size: 12px; text-align: left;">
        <i class="far fa-trash-alt"></i>Borrar
    </a>
</div>`;
                    } else {
                        return `<div class="text-center">
    <a onclick=Delete("/Admin/Proveedores/Delete/${data}") class="w-100 btn btn-lg btn-outline-primary" style="cursor:pointer; width:60px; font-size: 12px; text-align: left;">
        <i class="far fa-trash-alt"></i>Borrar
    </a>
</div>`;
                    }
                },
                "width": "5%"
            }






        ],

        "createdRow": function (row, data, dataIndex) {
            var estatus = data.estatus; // Obtener el valor de la columna "estatus"

            // Aplicar formato condicional basado en los valores de "estatus"
            switch (estatus) {
                case "Revision":
                    $(row).find('td:eq(0)').css('background-color', '#FFF600').css('font-weight', 'bold'); // Cambiar el color de fondo y poner en negritas el texto
                    break;
                case "Rechazada":
                    $(row).find('td:eq(0)').css('background-color', '#FF0000').css('font-weight', 'bold');
                    break;
                case "Validada":
                    $(row).find('td:eq(0)').css('background-color', '#ffc107').css('font-weight', 'bold');
                    break;
                case "Pagada":
                    $(row).find('td:eq(0)').css('background-color', '#00FF19').css('font-weight', 'bold');
                    break;
                case "Complemento":
                    $(row).find('td:eq(0)').css('background-color', '#A800FF').css('font-weight', 'bold').css('color', 'white');
                    break;
                // Puedes agregar más casos según tus necesidades
                default:
                    $(row).find('td:eq(0)').css('background-color', '#FF00CD').css('font-weight', 'bold');

                    break;
            }
        },
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


