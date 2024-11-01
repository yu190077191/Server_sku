var id = 0;

var type = "";

var UploadFileType = ",.png,.jpg,.jpeg,.bmp,.gif,.pdf,.ppt,.pptx,.doc,.docx,.xls,.xlsx,.msg,.zip,.txt,";

$(document).ready(function () {
    $('#attachementUpload').submit(uploadAttachmentSubmit);
});

function upload() {
    var filePath = $("#file1").val();
    var extentionName = filePath;
    var bu = getValue("BU", true, "string");
    var goodsType = $("input[name='goodsType']:checked").val();
    if (requiredIsEmpty() || goodsType == null || goodsType == "") {
        alert("Required fields cannot be empty!");
        return "";
    }

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
        }
        else alert("File type not allowed!");
    }
    else {
        alert("请选择要上传的文件(Excel格式)！");
    }
}

function uploadAttachmentSubmit() {
    try {
        var tableId = $("#tableId").val();
        var param = ({ type: $("#Type").val(), tableId: tableId });
        var options = {
            url: getRootUrl() + 'Upload',
            data: param,
            success: function (msg) {
                $("#uploadButton1").css({ "display": "" });
                $("#lbl").html("");

                var file = $("#file1");
                file.after(file.clone().val(""));
                file.remove();

                var arr = msg.split('|');
                var guid = arr[0];
                var message = arr[1];
                id = getModelId();
                if (id == 0 && document.getElementById("Id") != null && $("#Id").val() != "") {
                    id = parseInt($("#Id").val());
                }

                var goodsType = $("input[name='goodsType']:checked").val();
                if (message.toLowerCase() == "ok") {
                    $.post(getRootUrl() + "WF/SaveImportedData", { id: guid, tableId: tableId, requestVersionId: id, name: $("#Name").val(), goodsType: goodsType, bu: $("#BU").val() }, function (html) {
                        $("#searchResult").html(html);
                        if (html.indexOf("color:red;") < 0 || html.indexOf("Please ") > 0) {
                            updateFileTable(tableId, guid, "");
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

        $(this).ajaxSubmit(options);
    }
    catch (e) {
        alert(e.Message);
    }

    return false;
}