//#region Common
function OnAjaxBegin(id) {
    $(id).prop("disabled", true);
    $(id).find('i').removeClass("hide");
}
function OnAjaxComplete(id) {
    $(id).prop("disabled", false);
    $(id).find('i').addClass("hide");
}

$(document).ajaxStart(function () {
    showLoading();
});

$(document).ajaxStop(function () {
    if ($.active <= 1) {
        hideLoading();
    }
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="tooltip"]').on("mouseleave", function () {
        $(this).tooltip("hide");
    });
    $('[data-toggle="popover"]').popover();
    //$('[data-toggle="popover"]').on("mouseleave", function () {
    //    $(this).popover("hide");
    //})
});

function showLoading() {
    $('#loading,#loadingbar').show();
}

function hideLoading() {
    $('#loading,#loadingbar').hide();
}

function Select2Portal(param) {
    $("#" + param + "").select2();
}

window.addEventListener("load", function (event) {
    var historyTraversal = event.persisted ||
        (typeof window.performance != "undefined" &&
            window.performance.navigation.type === 2);
    if (historyTraversal) {
        // Handle page restore.
        window.location.reload();
    }
});

function showToastPortal(ToastType, Title, Message, Timeout) {
    $.toaster({ priority: ToastType, title: Title, message: Message, timeout: Timeout });
}
//#endregion

var MessagePortal =
{
   
};