prompt = function (title, message, buttonsArr) {
    $("#modal-title").html(title);
    $("#modal-message").html(message);
    var buttons = "";
    buttonsArr.forEach(b => {
        buttons += "<a onclick=\"$('#" + b.target + "').click();\" class='btn'>" + b.value + "</a>";
    })
    buttons += "<a onclick='closeModal();' class='btn'>" + Cancel + "</a>";
    $("#modal-message").html(buttons);
    $("#modal").show();
}

closeModal = function () {
    $("#modal").hide();
}