var MIN_KEYWORD_LENGTH = 1;

confirm = function (title, message, buttonValue, buttonTarget) {
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
    //$("#modal-ok").click(function () { $("#" + buttonTarget).click(); });
    //$("#modal-ok").attr("onclick", "$('#" + buttonTarget + "').click();");
    $("#modal-ok").show();
    $("#modal-ok").html(buttonValue);
    $("#modal-ok").attr("href", $("#" + buttonTarget).attr('href'));
    $("#modal-ok").attr("asp-route-id", $("#" + buttonTarget).attr('asp-route-id'));
    $("#modal-ok").attr("asp-action", $("#" + buttonTarget).attr('asp-action'));
    $("#modal-close").removeClass("btn--primary");
    $("#modal").show();
}

alert = function (title, message, buttonValue = "OK") {
    $("#modal-title").html(title);
    $("#modal-message").html(message);
    $("#modal-ok").hide();
    $("#modal-close").html(buttonValue);
    $("#modal-close").addClass("btn--primary");
    $("#modal").show();
}

closeModal = function () {
    $("#modal").hide();
}

loadSearchSuggestions = function (e) {
    var keyword = e.target.value;
    var window = $("#nav-search-suggestions");
    var iframe = $("#nav-search-suggestions-iframe");
    var baseUrl = "/Games/SearchSuggestions";

    if (keyword.length >= MIN_KEYWORD_LENGTH) {
        //iframe.css("height", "0");
        iframe.attr("src", baseUrl + "?keyword=" + keyword);
        window.show();
    } else {
        window.hide();
    }
}

resizeIframe = function (obj) {
    obj.style.height = obj.contentWindow.document.documentElement.scrollHeight + 'px';
}