
$(document).ready(function () {
    debugger
    GetProductCategoryList();
    $('#ddlIsActive').select2();
});

$(document).on('click', '#ProductCategorySearch', function () {
    $("#ProductCategoryList").dataTable().fnDestroy();
    GetProductCategoryList();
});

function GetProductCategoryList() {
    $('#ProductCategoryList').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'ProductCategory/GetProductCategoryList',
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "Name", "value": $("#txtName").val() },
               { "name": "IsAvailable", "value": $("#ddlIsAvailable").val() });
        },
        "processing": true,
        "bLengthChange": true,
        "bInfo": true,
        "paging": true,
        "searching": false,
        "columnDefs": [],
        "order": [[0, "desc"]],
        "lengthMenu": [10, 25, 50, 75, 100],
        "aoColumns": [
            { "sName": "ProductCategoryId", "bSearchable": false, "bSortable": true },
            { "sName": "ProductCategoryName", "bSearchable": false, "bSortable": true },
            { "sName": "IsAvailable", "bSearchable": false, "bSortable": true },
            {
                "sName": "Action",
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, aoData) {
                    var href = "/manager/add-edit-productcategory" + "?ProductCategoryId=" + aoData[0];
                    return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a> <i style="cursor:pointer;" onclick="javascript:ConfirmDeleteProductCategory(' + aoData[0] + ')" class="fas fa-trash-alt"></i>';

                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}

function ConfirmDeleteProductCategory(ProductCategoryId) {
    $(".modal-header #ProductCategoryId").val(ProductCategoryId);
    $("#confirmDeleteProductCategoryModal").modal('show');;
}

function DeleteProductCategory() {
    $("#confirmDeleteProductCategoryModal").modal('hide');
    var ProductCategoryId = $("#ProductCategoryId").val();
    $.ajax({
        url: siteURLPortal + "ProductCategory/DeleteProductCategory",
        type: 'POST',
        data: { "ProductCategoryId": ProductCategoryId },
        dataType: "json",
    }).done(function (data, textStatus, jqXHR) {
        if (data.result.AllowToDelete == true) {
            showToastPortal('success', '', MessagePortal.ProductCategoryDeleted);
            var oTable = $('#ProductCategoryList').dataTable();
            oTable.fnClearTable(0);
            oTable.fnDraw();
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        showToastPortal('danger', '', MessagePortal.ProductCategoryDeletedFailed);
    }).always(function (data, textStatus, errorThrown) {
        //write content here
    });
}