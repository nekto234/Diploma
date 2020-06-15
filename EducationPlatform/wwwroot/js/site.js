// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.datatable-table').DataTable({
        dom: 'Bfrtip',
        //buttons: [
        //    'csv', 'excel', 'pdf', 'print'
        //]
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Ukrainian.json"
        },
        buttons: {
            buttons: [
                {
                    extend: 'pdf',
                    className: 'btn btn-inverse-primary btn-sm',
                    exportOptions: {
                        columns: ['.visible']
                    }
                },
                {
                    extend: 'excel',
                    className: 'btn btn-inverse-success btn-sm',
                    exportOptions: {
                        columns: ['.visible']
                    }
                },
                {
                    extend: 'print',
                    className: 'btn btn-inverse-secondary btn-sm',
                    exportOptions: {
                        columns: ['.visible']
                    }
                },
            ]
        }
    });
});