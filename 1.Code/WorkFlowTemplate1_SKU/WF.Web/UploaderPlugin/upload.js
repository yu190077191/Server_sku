
var type = "";

var UploadFileType = ",.png,.jpg,.jpeg,.bmp,.gif,.pdf,.ppt,.pptx,.doc,.docx,.xls,.xlsx,.msg,.zip,.txt,";

function upload() {
    var filePath = $("#file1").val();
    var extentionName = filePath;
    if (extentionName.indexOf(".") > 0) {
        extentionName = extentionName.substr(extentionName.lastIndexOf("."));
        extentionName = "," + extentionName.toLowerCase() + ",";
        if (extentionName.length > 2 && UploadFileType.indexOf(extentionName) >= 0) {
            var tableId = $("#tableId").val();
            if (tableId == "") {
                alert("请选择上传类别！");
                return;
            }

            $("#uploadButton").submit();
            $("#lbl").html("uploading...");
            $("#uploadButton1").css({ "display": "none" });
            startShowProgress();
        }
        else alert("File type not allowed!");
    }
    else {
        alert("请选择要上传的文件(Excel2003格式)！");
    }
}

var elapsedSeconds = 0;
var current = 0;
var timer = null;
var progressBarCount = 0;
var progressBarId = "progressBar";
var progressBarMessageId = "";
var timeCounter = "";
function showProgress() {
    current++;
    elapsedSeconds += 0.5;
    if (current > 0 && current <= 100) {
        $("#" + progressBarId).css({"width":current.toString() + "%"});
    }
    else if (timer != null) {
        current = 0;
        $("#" + progressBarMessageId).html("文件已经上传完成，正在处理数据中，请稍候...");
    }

    $("#" + timeCounter).html("已用时：" + elapsedSeconds.toFixed(0).toString() + "秒。");
}

function startShowProgress() {
    current = 0;
    progressBarCount++;
    progressBarId = "progressBar" + progressBarCount.toString();
    progressBarMessageId = "progressBarMessageId" + progressBarCount.toString();
    timeCounter = "timeCounter" + progressBarCount.toString();
    var html = $("#progressBarDiv").html().replace("progressBarId", progressBarId).replace("progressBarMessageId", progressBarMessageId).replace("timeCounter", timeCounter);
    pop(html, 270);
    timer = setInterval(showProgress, 500);
}

$(document).ready(function () {
    $('#attachementUpload').submit(uploadAttachmentSubmit);
});

var _isUploadRunning = false;
function uploadAttachmentSubmit() {
    try {
        var json = "test";
        var tableId = $("#tableId").val();
        var param = ({ type: $("#Type").val(), tableId: tableId, json: json });
        var options = {
            url: getRootUrl() + 'UploaderPlugin/UploadHandler.ashx',
            data: param,
            success: function (msg) {
                _isUploadRunning = false;
                $("#uploadButton1").css({ "display": "" });
                $("#lbl").html("");

                current = 0;
                $("#" + progressBarMessageId).html("文件已经上传完成，正在处理数据中，请稍候...");
 
                var file = $("#file1");
                file.after(file.clone().val(""));
                file.remove();
                
                if (msg != null) {
                    msg = msg.replace("<PRE>", "");
                    msg = msg.replace("</PRE>", "");
                    msg = msg.replace("<pre>", "");
                    msg = msg.replace("</pre>", "");
                    msg = $.trim(msg);
                }

                var arr = msg.split('|');
                if (arr.length <= 1) {
                    pop(msg, 270);
                    return;
                }

                var guid = arr[0];
                var message = arr[1];
                var savedPath = arr[2]; 
                if (message.toLowerCase() == "ok") {
                    alert(getRootUrl())
                    $.post(getRootUrl() + "WF/SaveImportedData", { id: guid, tableId: tableId, requestVersionId: id, name: $("#Name").val() }, function (html) {
                        $("#searchResult").html(html);
                        closeMe();
                        elapsedSeconds = 0;
                        if (timer != null) {
                            clearInterval(timer);
                        }
                    });
                }
                else {
                    $("#searchResult").html(message);
                }
            },
            error: function (data) {
                alert(data);
            }
        };

        if (!_isUploadRunning) {
            _isUploadRunning = true;
            $(this).ajaxSubmit(options);
        }
    }
    catch (e) {
        alert(e.Message);
    }

    return false;
}