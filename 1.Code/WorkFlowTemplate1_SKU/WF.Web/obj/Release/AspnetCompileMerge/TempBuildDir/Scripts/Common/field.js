var hasInputValues = false;
var checkboxAllChecked = true;
function getFields() {
    checkboxAllChecked = true;
    var array = new Array();
    var ids = ",";
    $("#fieldTable td").each(function () {
        $(this).children().each(function () {
            if (this.id != null && $(this).is(":visible")) {
                var value = $(this).val();
                if (value == null) value = "";
                var model = new Object();
                model.Name = this.id;
                model.Value = value;
                if (model.Name.indexOf("attachementUpload") == 0) {
                    var uploadedFile = document.getElementById("uploaded" + model.Name.replace("attachementUpload", ""));
                    if (uploadedFile == null || !$(uploadedFile).is(":visible")) {
                        if (mandatory.indexOf("," + this.id.replace("attachementUpload", "") + ",") >= 0) {
                            getValue(this.id, true, "string");
                        }
                    }
                }
                else if (model.Name.indexOf("Control_") == 0) {
                    if (mandatory.indexOf("," + this.id.replace("Control_", "") + ",") >= 0) {
                        getValue(this.id, true, "string");
                    }
                }

                if (model.Name.indexOf("Control_") == 0 && model.Value != "") {
                    model.FieldId = parseInt(this.id.replace("Control_", ""));
                    var atLeastOneIsChecked = false;
                    if (this.id.indexOf("_0") > 0) {
                        model.FieldId = parseInt(this.id.replace("Control_", "").replace("_0", ""));
                        var checkValue = "";
                        for (var x = 0; x < 50;x++){
                            var checkboxId = "Control_" + model.FieldId.toString() + "_" + x.toString();
                            var chk = document.getElementById(checkboxId);
                            if (chk == null) break;
                            if (chk.checked) {
                                checkValue += "," + $(chk).val();
                                $("#td_" + model.FieldId.toString()).css({ "background-color": "" });
                                atLeastOneIsChecked = true;
                            }
                            else {
                                if (mandatory.indexOf("," + model.FieldId.toString() + ",") < 0) {
                                    atLeastOneIsChecked = true;
                                }
                                else {
                                    $("#td_" + model.FieldId.toString()).css({ "background-color": "yellow" });
                                    checkboxAllChecked = false;
                                }
                            }
                        }

                        model.Value = checkValue;
                    }

                    if (atLeastOneIsChecked) {
                        checkboxAllChecked = true;
                    }

                    if (model.Value != "" && ids.indexOf("," + model.FieldId.toString() + ",") < 0) {
                        array.push(model);
                        ids += model.FieldId.toString() + ",";
                        hasInputValues = true;
                    }
                }
            }
        } );
    });

    var hiddens = new Array();
    $(".textboxValues").each(function () {
        var id = this.id;
        var textboxId = id.replace("Values", "");
        hiddens.push(textboxId);
    });

    for(var i=0;i<hiddens.length;i++) {
        var id = hiddens[i];
        if ($("#" + id).is(":visible")) {
            var value = $("#" + id).val();
            if (value == null) value = "";
            var model = new Object();
            model.Name = id;
            model.Value = value;
            if (mandatory.indexOf("," + id.replace("Control_", "") + ",") >= 0) {
                getValue(id, true, "string");
            }

            if (model.Name.indexOf("Control_") == 0 && model.Value != "") {
                model.FieldId = parseInt(id.replace("Control_", ""));
                array.push(model);
                hasInputValues = true;
            }
        }
    }

    var json = $.toJSON(array);
    return json;
}

function onLoadDataReady() {
    if (globalJsonModel == null) return;
    $(".textboxValues").each(function () {
        var id = this.id;
        var textboxId = id.replace("Values", "");
        $("#" + textboxId).val(jsonModel[textboxId]);
    });
}

function pushOptions(textbox) {
    var val = $(textbox).val();
    $(textbox).prev().append("<option value='" + val + "' selected='selected'>" + val + "</option>");
    $(textbox).remove();
}

function checkAllRequired() {
    var allChecked = true;
    var checkboxes = $(":checkbox");
    var arr = new Array();
    for (var i = 0; i < checkboxes.length; i++) {
        if (!checkboxes[i].checked) {
            var id = checkboxes[i].id;
            allChecked = false;
        }

        
        if (id != null) {
            arr.push(id);
        }
    }

    return allChecked;
}

function submitThis(button) {
    if ($("#Control_6").val() != undefined) {
        if ($("#Control_6").val().length > 40) {
            $("#Control_6").focus();
            alert("'Material Description EN' The length cannot exceed 40 words!");
            return;
        }
    }
    if ($("#Control_7").val() != undefined) {
        if ($("#Control_7").val().length > 40) {
            $("#Control_7").focus();
            alert("'物料中文描述' The length cannot exceed 40 words!");
            return;
        }
    }
    if ($("#Control_8").val() != undefined) {
        if ($("#Control_8").val().length > 40) {
            $("#Control_8").focus();
            alert("'物料繁体中文描述' The length cannot exceed 40 words!");
            return;
        }
    }
    if ($("#Control_17").val() == "" || $("#Control_18").val() == "" || $("#Control_19").val() == "") {
        if ($("#Control_227").val() == "CN CPW" || $("#Control_227").val() == "CN NIN"
            || $("#Control_227").val() == "HK CPW" || $("#Control_227").val() == "CN Wyeth"
            || $("#Control_227").val() == "HK Wyeth" || $("#Control_227").val() == "HK NIN") {
            $("#Control_17").focus();
            alert("Global Attribute #1 & Global Attribute #2 & Global Attribute #3 must fill in!");
            return;
        }
    }
    if ($("#Control_39").find("option:selected").text() == "Select" || $("#Control_39").val() == "") {
        $("#Control_39").focus();
        alert("请选择 Origin(Manufacturing Plant Code) 生产工厂代码!");
        return;
    }

    if ($("#File_186").val() != undefined) {
        //$("#Control_167").find("option:selected").text() != "Select" || 
        if ($("#Control_171").val() != "" || $("#Control_174").val() != "") {
            if (!$("#uploaded186").is(":visible")) {
                alert("请选择Functional Approval上传文件");
                return;
            }
        }
    }

    if ($("#File_281").val() != undefined) {
        if ($("#Control_102").val() != "" || $("#Control_103").val() != "" || $("#Control_104").val() != "") {
            if (!$("#uploaded281").is(":visible")) {
                alert("请选择Functional Approval上传文件");
                return;
            }
        }
    }

    if ($("#Control_228").find("option:selected").text() == "Dolce Gusto")
    {
        if ($("#Control_17").val() == "" || $("#Control_18").val() == "" || $("#Control_19").val() == ""||$("#Control_20").val() == "")
        {
            alert("GA1-GA4 must fill in!");
            return;
        }
    }
    ////Bu MIN GA!-GA3
    //if ($("#Control_227").find("option:selected").text() == "HK NIN" || $("#Control_227").find("option:selected").text() == "CN NIN") {
    //    if ($("#Control_17").val() == "" || $("#Control_18").val() == "" || $("#Control_19").val() == "")
    //    {
    //        alert("GA1-GA3 must fill in!");
    //        return;
    //    }
    //}

    if ($("#Control_3").val() != undefined)
    {

        if ($("#Control_3").val().indexOf("CN26") >= 0 || $("#Control_3").val().indexOf("HK15") >= 0 || $("#Control_3").val().indexOf("HK21") >= 0 || $("#Control_3").val().indexOf("TW10") >= 0 
            || $("#Control_265").val().indexOf("CN26") >= 0 || $("#Control_265").val().indexOf("HK15") >= 0 || $("#Control_265").val().indexOf("HK21") >= 0 || $("#Control_265").val().indexOf("TW10") >= 0
            || $("#Control_284").val().indexOf("CN26") >= 0 || $("#Control_284").val().indexOf("HK15") >= 0 || $("#Control_284").val().indexOf("HK21") >= 0 || $("#Control_284").val().indexOf("TW10") >= 0) {
            if ($("#Control_33 option:selected").val() == "") {
                $("#Control_33 ").focus();
                alert("Material Group 3 must fill in!");
                return;
            }
        } 
        

        if ($("#Control_3").val().indexOf("CN26") >= 0 || $("#Control_265").val().indexOf("CN26") >= 0 || $("#Control_284").val().indexOf("CN26") >= 0) {
            if ($("#Control_34 option:selected").val() == "") {
                $("#Control_34 ").focus();
                alert("Material Group 4 must fill in!");
                return;
            }
        }
    }

    if (($("#Control_2").length > 0 && $("#Control_17").length > 0 && $("#Control_18").length > 0)
        || ($("#Control_98").val() != undefined && $("#Control_98").length > 0 && $("#Control_106").length > 0 && $("#Control_107").length > 0)) {
        if (($("#Control_2").val() != undefined && ($("#Control_2").val().toLowerCase() == "tw nhs"
            || $("#Control_2").val().toLowerCase() == "tw infant nutrition"
            || $("#Control_2").val().toLowerCase() == "tw wyeth"
            || $("#Control_2").val().toLowerCase() == "tw coffee dolce gusto"
            || $("#Control_2").val().toLowerCase() == "hk nhs"
            || $("#Control_2").val().toLowerCase() == "cn nhs"))
            && ($("#Control_17").val() == "" || $("#Control_18").val() == "")) {
            alert("GA1、GA2 must fill in!");
            return;
        }
        if (($("#Control_98").val() != undefined && ($("#Control_98").val().toLowerCase() == "tw nhs"
            || $("#Control_98").val().toLowerCase() == "tw infant nutrition"
            || $("#Control_98").val().toLowerCase() == "tw wyeth"
            || $("#Control_98").val().toLowerCase() == "tw coffee dolce gusto"
            || $("#Control_98").val().toLowerCase() == "hk nhs"
            || $("#Control_98").val().toLowerCase() == "cn nhs"))
            && ($("#Control_106").val() == "" || $("#Control_107").val() == "")) {
            alert("GA1、GA2 must fill in!");
            return;
        }
    }

    if (($("#Control_2").length > 0 && $("#Control_19").length > 0 && $("#Control_20").length > 0)
        || ($("#Control_98").val() != undefined && $("#Control_98").length > 0 && $("#Control_108").length > 0 && $("#Control_109").length > 0)) {
        if ($("#Control_2").val() != undefined && $("#Control_2").val().toLowerCase() == "tw dolce gusto"
            && ($("#Control_19").val() == "" || $("#Control_20").val() == "")) {
            alert("GA3、GA4 must fill in!");
            return;
        }
        if ($("#Control_98").val() != undefined && $("#Control_98").val().toLowerCase() == "tw dolce gusto"
            && ($("#Control_108").val() == "" || $("#Control_109").val() == "")) {
            alert("GA3、GA4 must fill in!");
            return;
        }
    }
       
    if ($("input[name='otherinput']").length >=1)
    {
        $("input[name='otherinput']").focus();
        alert("GParent Product Code must fill in!");
        return;
    }
    if ($("#IsRequester").length >= 1 && $("#hidOversize").val() == "Yes" && $("#Control_26").val()=="")
    {
        $("#Control_26").focus();
        alert("Bonus Weight/Volume must fill in!");
        return;
    }

    if ($("#Control_223").val() == "") {
        $("#Control_223").focus();
        alert("Salvg. days bef. exp (Days) must fill in!");
        return;
    }

    var json = getFields();
    var id = getModelId();
    var _requiredIsEmpty = requiredIsEmpty();
    if (_requiredIsEmpty || !checkboxAllChecked) {
        alert("The fields highlighted in yellow are mandatory!\n页面上标为黄色的是必填项，请填写完整再提交！");
        return;
    }
    $.post(getRootUrl() + "WF/Save", { id: id, json: json, requiredIsEmpty: _requiredIsEmpty }, function (id) {
        location.href = getRootUrl() + "WF/Preview?id=" + id.toString();
    });
}

function myShowModalDialog(url, width, height, callback) {
    if (window.showModalDialog) {
        if (callback) {
            //alert(1);
            var rlt = showModalDialog(url, '', 'resizable:no;scroll:no;status:no;center:yes;help:no;dialogWidth:' + width + ' px;dialogHeight:' + height + ' px');
            if (rlt)
                return callback(rlt);
            else {
                callback(window.returnValue);
            }
        }
        else
            //alert(2);
        showModalDialog(url, '', 'resizable:no;scroll:no;status:no;center:yes;help:no;dialogWidth:' + width + ' px;dialogHeight:' + height + ' px');
    }
    else {
        if (callback)
            window.showModalDialogCallback = callback;
        var top = (window.screen.availHeight - 30 - height) / 2; //获得窗口的垂直位置;
        var left = (window.screen.availWidth - 10 - width) / 2; //获得窗口的水平位置;
        var winOption = "top=" + top + ",left=" + left + ",height=" + height + ",width=" + width + ",resizable=no,scrollbars=no,status=no,toolbar=no,location=no,directories=no,menubar=no,help=no";

        //alert(3);
        window.open(url, window, winOption);
    }
}

function showEmptyAlert() {
    var msg = "Required fields cannot be empty!";
    if (confirm(msg)) {
        location.href = getRootUrl() + "WF/Edit?id=" + id.toString();
    }
}

var GUID = "";

var type = "";

var UploadFileType = ",.png,.jpg,.jpeg,.bmp,.gif,.pdf,.ppt,.pptx,.doc,.docx,.xls,.xlsx,.msg,.zip,.txt,";
var UploadFileExcelType = ",.xls,.xlsx,";

$(document).ready(function () {
    $('.attachmentForm').submit(uploadAttachmentSubmit);
});

var FILEID = "";
var FILENAME = "";
function uploadFile(_fileId) {
    FILEID = _fileId;
    var filePath = $("#File_" + FILEID).val();
    var extentionName = filePath;
    FILENAME = filePath.substr(filePath.lastIndexOf("\\") + 1);
    var name = "";//getValue("Name", true, "string");
    
    if (extentionName.indexOf(".") > 0) {
        extentionName = extentionName.substr(extentionName.lastIndexOf("."));
        extentionName = "," + extentionName.toLowerCase() + ",";
        if (FILEID == "152" || FILEID =="187") {
            if (extentionName.length > 2 && UploadFileExcelType.indexOf(extentionName) >= 0) {
                $("#uploadButton" + FILEID).submit();
                $("#lbl" + FILEID).html("uploading...");
                $("#fileButton" + FILEID).css({ "display": "none" });
            } else alert("File type not allowed(Excel type)!");
        }else if (extentionName.length > 2 && UploadFileType.indexOf(extentionName) >= 0) {
            $("#uploadButton" + FILEID).submit();
            $("#lbl" + FILEID).html("uploading...");
            $("#fileButton" + FILEID).css({ "display": "none" });
        }
        else alert("File type not allowed!");
    }
    else {
        alert("请选择要上传的文件！");
    }
}

function uploadAttachmentSubmit() {
    try {
        var id = getModelId();
        var name = $.trim($("#Name" + FILEID).html());
        var param = ({ type: "CommonAttachment", id: id, typeCode: "common", subCode: FILEID, Description: FILENAME });
        var options = {
            url: getRootUrl() + 'Upload',
            data: param,
            success: function (msg) {
                $("#fileButton" + FILEID).css({ "display": "" });
                $("#lbl" + FILEID).html("");

                var file = $("#File_" + FILEID);
                file.after(file.clone().val(""));
                file.remove();
                var arr = msg.split('|');
                var guid = arr[1];
                var html = "<div id='trFile" + guid + "'>";
                html += "<a id='uploaded" + FILEID + "' href='" + getRootUrl() + "Request/Download?id=" + guid + "' target='_blank' style='text-decoration:underline;color:blue;'>" + FILENAME + " [download]<a>";
                html += " <a style='cursor:pointer;color:red;' onclick='deleteAttachment(\"" + guid + "\");'>[delete]</a>";
                html += "</div>";
                $("#lbl" + FILEID).html(html);
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

function deleteAttachment(guid) {
    if (confirm(getConfirmDeleteMessage())) {
        $.post(getRootUrl() + "Request/Delete", { id: guid }, function (json) {
            if (json.Success) {
                $("#trFile" + guid).css({ "display": "none" });
                closeDialogDiv();
            }
        });
    }
}

function hasEmptyFields() {
    var json = getFields();
    var id = getModelId();
    var _requiredIsEmpty = requiredIsEmpty();
    return _requiredIsEmpty || !checkboxAllChecked;
}

function CheckFields() {
    if ($("#td_52").text() != undefined && $("#Control_213").val() != undefined) {
        var selectDay = $("#Control_213Selected").val();
        var selectNumber = selectDay.split(' ')[0];
        var selectUnit = selectDay.split(' ')[1];
        if (selectUnit == "Month" || selectUnit == "Months") {
            selectNumber = parseInt(selectNumber) * 30;
        }
        if (parseFloat($("#td_52 span").text()) > selectNumber) {
            $("Control_213").focus();
            alert("Total Shelf Life 必须大于等于Minimum Remaining Shelf Life");
            return false;
        }
    }
    return true;
}
function saveField() {
    if (!hasInputValues) return;
    var json = getFields();
    var id = getModelId();
    var _requiredIsEmpty = requiredIsEmpty();

    $.post(getRootUrl() + "WF/Save", { id: id, json: json, requiredIsEmpty: _requiredIsEmpty }, function (id) {
        
    });
}

(function ($) {
    $.fn.extend({
        MultDropList: function (options) {
            var op = $.extend({ wraperClass: "wraper", width: "200px", height: "200px", data: "", selected: "" }, options);
            return this.each(function () {
                var $this = $(this); //指向TextBox
                var $hf = $(this).next(); //指向隐藏控件存
                var conSelector = "#" + $this.attr("id") + ",#" + $hf.attr("id");
                var $wraper = $(conSelector).wrapAll("<div><div></div></div>").parent().parent().addClass(op.wraperClass);

                var $list = $('<div class="list"></div>').appendTo($wraper);
                $list.css({ "width": op.width, "height": op.height });
                //控制弹出页面的显示与隐藏
                $this.click(function (e) {
                    $(".list").hide();
                    $list.toggle();
                    e.stopPropagation();
                });

                $(document).click(function () {
                    $list.hide();
                });

                $list.filter("*").click(function (e) {
                    e.stopPropagation();
                });
                //加载默认数据
                $list.append('<ul><li><input type="checkbox" class="selectAll" value="" /><span>Select All</span></li></ul>');
                var $ul = $list.find("ul");

                //加载json数据
                var listArr = op.data.split("|");
                var jsonData;
                for (var i = 0; i < listArr.length; i++) {
                    jsonData = eval("(" + listArr[i] + ")");
                    $ul.append('<li><input type="checkbox" value="' + jsonData.k + '" /><span>' + jsonData.v + '</span></li>');
                }

                //加载勾选项
                var seledArr;
                if (op.selected.length > 0) {
                    seledArr = selected.split(",");
                }
                else {
                    seledArr = $hf.val().split(",");
                }

                $.each(seledArr, function (index) {
                    $("li input[value='" + seledArr[index] + "']", $ul).attr("checked", "checked");

                    var vArr = new Array();
                    $("input[class!='selectAll']:checked", $ul).each(function (index) {
                        vArr[index] = $(this).next().text();
                    });
                    $this.val(vArr.join(","));
                });
                //全部选择或全不选
                $("li:first input", $ul).click(function () {
                    if ($(this).attr("checked")) {
                        $("li input", $ul).attr("checked", "checked");
                    }
                    else {
                        $("li input", $ul).removeAttr("checked");
                    }
                });
                //点击其它复选框时，更新隐藏控件值,文本框的值
                $("input", $ul).click(function () {
                    var kArr = new Array();
                    var vArr = new Array();
                    $("input[class!='selectAll']:checked", $ul).each(function (index) {
                        kArr[index] = $(this).val();
                        vArr[index] = $(this).next().text();
                    });
                    $hf.val(kArr.join(","));
                    $this.val(vArr.join(","));
                });
            });
        }
    });
})(jQuery);


$(document).ready(function () {
    setTimeout(function () {
        $(".selectAll").each(function () {
            $(this).removeAttr("checked");
        });
    }, 1000);
});