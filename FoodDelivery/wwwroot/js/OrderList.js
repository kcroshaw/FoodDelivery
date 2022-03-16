var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("cancelled")) {
        loadList("cancelled");
    }
    else {
        if (url.includes("delivered")) {
            loadList("delivered");
        }
        else {
            loadList("ready");
        }
    }
});

function loadList(param) {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/order?status=" + param,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "orderHeader.id", "width": "20%" },
            { "data": "orderHeader.deliveryName", "width": "20%" },
            { "data": "orderHeader.applicationUser.email", "width": "20%" },
            { "data": "orderHeader.orderTotal", render: $.fn.dataTable.render.number(',', '.', 2, '$'), "width": "20%" },
            {
                "data": "orderHeader.id",
                "render": function (data) {
                    return ` <div class="text-center">
                                <a href="/Admin/Orders/OrderDetails?id=${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                    <i class="far fa-edit"></i> Details
                                </a>
                               
                    </div>`;
                }, "width": "20%"
            }
        ],
        "language": {
            "emptyTable": "no data found."
        },
        "width": "100%"
    });
}