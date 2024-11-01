function ajustBodyHeight() {
    var windowHeight = $(window).height();
    var page_height = document.body.clientHeight;
    var container_height = document.getElementById('container').clientHeight;
    if (page_height < windowHeight) {
        var offset = 33;
        var mainBodyHeight = windowHeight - $("#footer").height() - offset;
        $("#mainBody").css({ "height": mainBodyHeight.toString() + "px" });
    }
}

$("#loadingTip")
    .bind("ajaxSend", function () {
        $(this).show();
    })
    .ajaxComplete(function () {
        $(this).hide();
    });