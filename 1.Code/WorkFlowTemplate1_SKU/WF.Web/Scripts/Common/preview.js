function checkEmail(textbox) {
    var txt = $.trim($(textbox).val());
    if (txt == "") return;
    if (txt.indexOf("<") >= 0) {
        var email = "";
        for (var i = 0; i < txt.length; i++) {
            var a = txt.indexOf("<", i);
            if (a < 0) break;
            var b = txt.indexOf(">", a);
            if (b < a) break;
            email += txt.substr(a + 1, b - a - 1) + ";";
            i = b + 1;
        }

        $(textbox).val(email);
    }
}

function send(button) {
    var o = new Object();
    o.Id = getModelId();
    var tableId = "#POP_Table_" + popId.toString();
    var array = $(tableId).find(".inputfl");
    var empty = false;
    $(tableId + " .inputfl").each(function () {
        var value = $.trim($(this).val());
        if (value == "") {
            $(this).css("background", "yellow");
            empty = true;
        }
        else {
            $(this).css( "background", "");
        }
    });

    o.Step = $(array[0]).val();
    o.To = $(array[1]).val();
    o.Cc = $(array[2]).val();
    o.Subject = $(array[3]).val();
    o.Body = $(array[4]).val();

    if (empty) {
        alert("The fields highlighted in yellow cannot be empty!");
        return;
    }

    var json = $.toJSON(o);
    $.post(getRootUrl() + "Home/SendEmail", { json: json }, function (html) {
        pop(html);
        if (html.toLowerCase().indexOf("success") >= 0) {
            setTimeout(function () { location.href = location.href; }, 2000);
        }
    });
}