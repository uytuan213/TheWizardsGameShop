prompt = function (title, message, buttonValue, buttonTarget) {
    $("#modal-title").html(title);
    $("#modal-message").html(message);
    //var buttons = "";
    //for (i = 0; i < buttonsArr.length; i++) {
    //    buttonsArr.forEach(b => {
    //        buttons += "<a onclick=\"$('#" + b['target'] + "').click();\" class='btn'>" + b['value'] + "</a>";
    //    })
    //}
    
    //buttons += "<a onclick='closeModal();' class='btn'>" + Cancel + "</a>";
    //$("#modal-buttons").html(buttons);
    $("#modal-ok").html(buttonValue);
    //$("#modal-ok").click(function () { $("#" + buttonTarget).click(); });
    //$("#modal-ok").attr("onclick", "$('#" + buttonTarget + "').click();");
    $("#modal-ok").attr("href", $("#" + buttonTarget).attr('href'));
    $("#modal-ok").attr("asp-route-id", $("#" + buttonTarget).attr('asp-route-id'));
    $("#modal-ok").attr("asp-action", $("#" + buttonTarget).attr('asp-action'));
    $("#modal").show();
}

closeModal = function () {
    $("#modal").hide();
}