var ms = 30000;
var url = "/SKURegistration/home/heartBeat";
var _heartBeatRunning = false;
var _heartBeathandler = null;
var heartBeatTimes = 0;
function heartBeat() {
    if (_heartBeatRunning) {
        return;
    }
    else {
        _heartBeatRunning = true;
        heartBeatTimes++;
        $.post(url, { heartBeatTimes: heartBeatTimes }, function (result) {
            _heartBeatRunning = false;
            if (document.getElementById("heartBeatDiv") != null) {
                $("#heartBeatDiv").html(result);
            }
        });
    }
}

$(function () {
    var url = location.href.toLowerCase();
    if (url.indexOf("nestlechinese.com") > 0) {
        _heartBeathandler = setInterval(heartBeat, ms);
    }
});