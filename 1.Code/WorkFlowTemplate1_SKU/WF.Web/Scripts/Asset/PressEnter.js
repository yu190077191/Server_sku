document.onkeydown = function (event) {
    event = window.event || event;
    var keyvalue = event.keyCode || event.which;
    if (keyvalue == 13) {
        if (event.srcElement == null) return;
        var type = event.srcElement.type;
        if(type == null){
            type = "";
        }

        var type2 = "," + type.toLowerCase() + ",";
        var ignoredTypes = ",image,textarea,";//submit,
        if (type == "button") {
            event.srcElement.click();
        }
        else if (ignoredTypes.indexOf(type2) < 0) {
            event.keyCode = 9; 
        }
    }
}

function focusFirst() {
    var a = document.body.innerHTML.indexOf("tdRight2");
    if (a > 0) {
        a = document.body.innerHTML.toLowerCase().indexOf("id=\"", a);
        if (a > 0) {
            a += 4;
            var b = document.body.innerHTML.indexOf("\"", a);
            if (b > a) {
                var elementId = document.body.innerHTML.substr(a, b - a);
                $("#" + elementId).focus();
            }
        }
    }
}

$(function () {
    focusFirst();
});