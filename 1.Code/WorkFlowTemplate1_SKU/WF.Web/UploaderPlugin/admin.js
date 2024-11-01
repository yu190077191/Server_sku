$.ajaxSetup({ cache: false });

function ToMoney(moneyInKRMB) {
    moneyInKRMB = moneyInKRMB.toString();
    moneyInKRMB = $.trim(moneyInKRMB.replace(/\,/g, ""));
    var ratio = getRatio();
    if (ratio > 1) {
        var times = 0;
        for (var i = ratio; i > 1; i = i / 10) {
            moneyInKRMB += "0";
            times++;
        }

        var pos = moneyInKRMB.indexOf(".");
        if (pos > 0) {
            moneyInKRMB = moneyInKRMB.substr(0, pos + times + 1).replace(".", "") + "." +  moneyInKRMB.substr(pos + times + 1);
        }
    }

    return moneyInKRMB;
}

function ToString(money) {
     var ratio = getRatio();
     if (ratio > 1) {
         var m = parseFloat(money) / parseFloat(ratio);
         money = m;
         if (isNaN(money)) {
             money = "";
         }
     }

     return toMoneyString(money.toString());
}

function toggleMenu(obj) {
    var menuId = obj.currentTarget.id.replace("title", "menu");
    $("#" + menuId).toggle();
}

function initLeftMenu() {
    $("#menu0").show();

    $(".adminLeftTop .title").click(function (obj) {
        toggleMenu(obj);
    });

    $(".adminLeftMiddle .title").click(function (obj) {
        toggleMenu(obj);
    });
}

$(document).ready(function () {
    initLeftMenu();
});

function getConfirmDeleteMessage() {
    var language = getLanguage();
    if (language == "Chinese")
        return "您确定要删除吗？";
    else return "Are you sure to delte it?";
}

function showOK() {
    var language = getLanguage();
    if (language == "Chinese")
        alert("保存成功！");
    else return alert("Saved successfully!");
}

function getOperationFailureMessage() {
    var language = getLanguage();
    if (language == "Chinese")
        return "对不起，操作失败！";
    else return "Sorry, operation is invalid!";
}

function getCannotBeEmptyMessage() {
    var language = getLanguage();
    if (language == "Chinese")
        return "请填写完整再提交！";
    else return "Reuired fields cannot be empty!";
}

function getSearchDivParameters() {
    if (document.getElementById("searchDiv") != null) {
        var arr = getIdArrayInDiv("searchDiv", "filter");
        if (arr != null) {
            var paraArray = new Array();
            for (var i = 0; i < arr.length; i++) {
                var value = getValue(arr[i], false, "string");
                if (value != null && value != "") {
                    paraArray.push(createParameter(arr[i].replace("filter", ""), value));
                }
            }

            return paraArray;
        }
    }

    return null;
}

function search(pageIndex) {
    var query = new Object();
    query.PageIndex = pageIndex;
    var pageSize = $("#PageSize").val();
    if (pageSize != null) {
        query.PageSize = pageSize;
    }

    query.Keyword = $.trim($("#keyword").val());
    if ($("#RecordStatus").val() == "2") {
        query.RecordStatus = $("#RecordStatus").val();
    }

    var arr = getSearchDivParameters();
    if (arr != null && arr.length > 0) {
        query.Array = arr;
    }

    $.get(location.href, { json: $.toJSON(query) }, function (html) {
        $("#searchResult").html(html);
    });
}

function resetSearch() {
    $("#keyword").val("");
    var arr = getIdArrayInDiv("searchDiv", "filter");
    if (arr != null) {
        var paraArray = new Array();
        for (var i = 0; i < arr.length; i++) {
            var value = getValue(arr[i], false, "string");
            if (value != null && value != "") {
                $("#" + arr[i]).val("");
            }
        }
    }

    search(1);
}

function searchEmail(pageIndex) {
    var query = new Object();
    query.PageIndex = pageIndex;
    query.Keyword = $("#keyword").val();
    if ($("#RecordStatus").val() == "2") {
        query.RecordStatus = $("#RecordStatus").val();
    }

    if ($("#SendStatus").val() != "") {
        var nameValuePair = new Object();
        nameValuePair.Name = "Status";
        nameValuePair.Value = $("#SendStatus").val();

        query.Array = new Array();
        query.Array.push(nameValuePair);
    }

    $.get(location.href, { json: $.toJSON(query) }, function (html) {
        $("#searchResult").html(html);
    });
}

function reset() {
    $("#keyword").val("");
    search(1)
}

function goToPage(url, updateTargetId) {
    $("#" + updateTargetId).load(url);
}

function deleteItem(tableName, id, rowId, callback) {
    if (confirm(getConfirmDeleteMessage())) {
        var url = "/SKURegistration/Common/Delete";
        if (id != "" && isNaN(id) && id.length > 20) {
            url += "Guid";
        }

        $.post(url, { tableName: tableName, id: id }, function (json) {
            if (json.Success) {
                if (rowId == null) {
                    $("#tr" + id).css({ "display": "none" });
                }
                else {
                    $("#" + rowId).css({ "display": "none" });
                }

                if (callback) {
                    callback();
                }
            }
            else {
                alert(getOperationFailureMessage());
            }
        });
    }
}

function deleteGroupedItems(tableName, id) {
    if (confirm(getConfirmDeleteMessage())) {
        $.post("/SKURegistration/Common/Delete", { tableName: tableName, id: id }, function (json) {
            if (json.Success) {
                $("#tr" + id).css({ "display": "none" });
                for (var rowNumber = 1; rowNumber < 20; rowNumber++) {
                    var groupedRowId = "#tr" + id + "_" + rowNumber;
                    if ($(groupedRowId).html() == null) break;
                    else $(groupedRowId).css({ "display": "none" });
                }
            }
            else {
                alert(getOperationFailureMessage());
            }
        });
    }
}

function restoreItem(tableName, id, rowId) {
    //if (confirm(getConfirmDeleteMessage())) {
        $.post("/SKURegistration/Common/Restore", { tableName: tableName, id: id }, function (json) {
            if (json.Success) {
                if (rowId == null) {
                    $("#tr" + id).css({ "display": "none" });
                }
                else {
                    $("#" + rowId).css({ "display": "none" });
                }
            }
            else {
                alert(getOperationFailureMessage());
            }
        });
    //}
}

function createParameter(name, value) {
    var item = new Object();
    item.Name = name;
    item.Value = value;
    return item;
}

function newObject(name, value) {
    return createParameter(name, value);
}

function getJsonValue(json, name) {
    var arr = new Array();
    arr = json.Array;
    for (var i = 0; i < arr.length; i++) {
        if (arr[i].Name == name) {
            return arr[i].Value;
            break;
        }
    }
}

function parseXML(xml, nodeName) {
    var startIndex = xml.toLowerCase().indexOf("<" + nodeName.toLowerCase() + ">");
    if (startIndex >= 0) {
        var endIndex = xml.toLowerCase().lastIndexOf("</" + nodeName.toLowerCase() + ">");
        if (endIndex > startIndex) {
            return xml.substr(startIndex + nodeName.length + 2, endIndex - startIndex - nodeName.length - 2);
        }
    }
}

function replaceHTML(html) {
    if (html.indexOf(">") >= 0 || html.indexOf("<") >= 0) {
        html = html.replace(/\>/g, "＞");
        html = html.replace(/\</g, "＜");
        return html;
    }
    else return html;
}

var selectControlCallback;

function setControlSelect() {
    var data;
    selectControlCallback(data);
}

function loadPartialViewCallBack(targetdiv, url, params, callback) {
    var para = params;
    $.ajax({
        url: url,
        data: para,
        type: "POST",
        cache: false,
        datatype: "json",
        timeout: 100000,
        error: function (response) {
            alert(response);
        },
        success: function (response) {
            $("#" + targetdiv).html(response);
            callback(response);
        }
    });
}

function loadDateSelect(controlId, defaultDate) {
    var para = { defaultDate: defaultDate };
    var callback = function (data) {
        $("#ctrid").val(controlId);
    }

    popup("<div id='dateSelect' style='display:none;'></div>", "datepicker", true);
    loadPartialViewCallBack("dateSelect", "/SKURegistration/Common/DateSelect", para, callback);
}

function callbackSele() {
    $("#dateSele").datepicker({
        changeMonth: true,
        changeYear: true,
        numberOfMonths: 1,
        showButtonPanel: true,
        gotoCurrent: true,
        changeMonth: true,
        changeYear: true,
        duration: "fast",
        showCurrentAtPos: 1,
        showOtherMonths: true,
        //onSelect: onSelectDate
        onSelect: function (date, ins) {
            
        }
    });
}
function onSelectDate(datetext, ins) {
    var ctrl = $("#ctrid").val();
    $("#" + ctrl).val(datetext);
}

function selectDate(obj) {
    var today = new Date();
    var defaultDate = today.getFullYear().toString() + "-" + (today.getMonth() + 1).toString() + "-" + today.getDate().toString();
    if (obj.value != "") {
        var str = obj.value;
        if (str.indexOf("-") > 0) {
            if (str.indexOf(" ") > 0) {
                str = str.substr(0, str.indexOf(" "));
            }

            defaultDate = str;
        }
    }

    loadDateSelect(obj.id, defaultDate);
}

function convertArrayToJson(array) {
    var json = "{\"Array\":[";
    for (var i = 0; i < array.length; i++) {
        if (i > 0) json += ",";
        json += "{\"Name\":\"" + array[i].Name + "\",\"Value\":\"" + array[i].Value + "\"}";
    }

    json += "]}";
    return json;
}

function getIdArrayInDiv(divId, idPrefix) {
    var str = ("//" + idPrefix + "(\\w+)//g").replace(/\/\//g, "\/");
    var regex = eval(str);
    var html = $("#" + divId).html();
    var m = html.match(regex);
    var arr = new Array();
    if (m == null) return null;
    for (var i = 0; i < m.length; i++) {
        arr.push(m[i]);
    }

    return arr;
}

function getIdArrayInDivBAK2(divId, idPrefix) {
    var arr = new Array();
    var elements = $("#" + divId).children();
    if (elements != null && elements.length > 0) {
        for (var i = 0; i < elements.length; i++) {
            if (elements[i].id != null && elements[i].id.indexOf(idPrefix) == 0) {
                arr.push(elements[i].id);
            }
        }
    }

    return arr;
}

function getIdArrayInDivBAK(divId, idPrefix) {
    var html = "";
    if (divId != null && document.getElementById(divId) != null) {
        html = $("#" + divId).html();
    }
    else {
        html = document.body.innerHTML;
    }

    var str = "id\\=([\\S]+)";
    var pattern = new RegExp(str, "g");
    var m = html.match(pattern);
    var arr = new Array();
    if (m != null && m.length > 0) {
        for (var i = 0; i < m.length; i++) {
            var id = m[i].replace("id=", "");
            id = id.replace(/\"/g, "");
            id = id.replace(/\'/g, "");
            if (idPrefix != null && id.indexOf(idPrefix) != 0) {
                continue;
            }
            
            arr.push(id);
        }
    }

    return arr;
}

function getIdArray(idPrefix) {
    var str = "id\\=\\\"" + idPrefix + "([0-9]+)";
    var pattern = new RegExp(str, "g");
    var html = document.body.innerHTML;
    var m = html.match(pattern);
    var arr = new Array();
    if (m != null && m.length > 0) {
        for (var i = 0; i < m.length; i++) {
            var id = m[i].replace("id=\"" + idPrefix, "");
            arr.push(id);
        }
    }

    return arr;
}

function getIdStr(idPrefix) {
    var arr = getIdArray(idPrefix);
    if (arr != null && arr.length > 0) {
        var str = ",";
        for (var i = 0; i < arr.length; i++) {
            str += arr[i] + ",";
        }

        return str;
    }

    return "";
}

//function getCheckboxList() {
//    return getIdArray("checkbox");
//}

function getCheckboxList() {
    var checkboxes = $(":checkbox");
    var arr = new Array();
    for (var i = 0; i < checkboxes.length; i++) {
        var id = checkboxes[i].id;
        if (id != null && id.toLowerCase() != "selectall") {
            arr.push(id.replace("checkbox", ""));
        }
    }

    return arr;
}

function selectAll(obj) {
    var selectStatus = true;
    if (!obj.checked) {
        selectStatus = false;
    }

    var checkboxArray = getCheckboxList();
    if (checkboxArray != null && checkboxArray.length > 0) {
        for (var i = 0; i < checkboxArray.length; i++) {
            document.getElementById("checkbox" + checkboxArray[i]).checked = selectStatus;
        }
    }
}

function getSelectedIds() {
    var ids = ",";
    var checkboxArray = getCheckboxList();
    if (checkboxArray != null && checkboxArray.length > 0) {
        for (var i = 0; i < checkboxArray.length; i++) {
            if (document.getElementById("checkbox" + checkboxArray[i]).checked) {
                ids += checkboxArray[i] + ",";
            }
        }
    }

    if (ids == ",") return "";
    else return ids;
}

function deleteAll(tableName) {
    var ids = getSelectedIds();
    if (ids == "") alert("未选择任何项！");
    else if(confirm("是否要删除选中项？")){
        $.post("/SKURegistration/Finance/DeleteAll", { tableName: tableName, ids: ids }, function (json) {
            if (json.Success) {
                var array = new Array();
                array = ids.split(',');
                for (var i = 0; i < array.length; i++) {
                    if (array[i] != "" && !isNaN(array[i])) {
                        $("#tr" + array[i]).css({ "display": "none" });
                    }
                }
            }
        });
    }
}

function getDateStr() {
    var date = new Date();
    var now = "";
    now = date.getFullYear() + "-";
    now = now + (date.getMonth() + 1) + "-";
    now = now + date.getDate() + " ";
    now = now + date.getHours() + ":";
    now = now + date.getMinutes() + ":";
    now = now + date.getSeconds() + "";
    return now;
}

function unicode(str) {
    return str.replace(/[^\u0000-\u00FF]/g, function ($0) { return escape($0).replace(/(%u)(\w{4})/gi, "/u$2") }); //\\u$2
}

;(function($) { 
$.extend({ 
format : function(str, step, splitor) { 
str = str.toString(); 
var len = str.length; 

if(len > step) { 
var l1 = len%step, 
l2 = parseInt(len/step), 
arr = [], 
first = str.substr(0, l1); 
if(first != '') { 
arr.push(first); 
}; 
for(var i=0; i<l2 ; i++) { 
arr.push(str.substr(l1 + i*step, step)); 
}; 
str = arr.join(splitor); 
}; 
return str; 
} 
}); 
})(jQuery);

var lastAmount = 0;
function toMoneyString(str, runOneTimeOnly) {
    str = str.toString();
    str = $.trim(str.replace(/\,/g, ""));
    var m = parseFloat(str);
    if (!isNaN(m) && m > 0) {
//        if (m >= 1000) {
//            if (m != lastAmount && runOneTimeOnly != true) {
//                lastAmount = m;
//                var realValue = m * getRatio();
//                var msg = "Warning: you have input a number that is equivalent to <b style='color:Red;'>" + toMoneyString(realValue, true) + "</b>! Please notice that \"1\" means \"1000\" in this system!<br />(请注意：该系统以千元为单元，您输入的金额已达到一百万元以上！)";
//                show(msg);
//            }
//        }

        var decimalPart = "";
        if (str.indexOf(".") >= 0) {
            decimalPart = str.substr(str.indexOf("."));
            str = str.substr(0, str.indexOf("."));
        }

        return $.format(str, 3, ",") + decimalPart;
    }

    return str;
}

function formatMoney(obj) {
    obj.value = toMoneyString(obj.value);
}

    function parseMoney(str) {
        str = str.replace(/\,/g, "");
        if (str != "" && !isNaN(str)) {
            return parseInt(str);
        }
        
        return -1;
    }

    var requiredMissingCount = 0;
    function resetRequiredMissingCount() {
        if (hasReset == false) {
            hasReset = true;
            requiredMissingCount = 0;
        }
    }

    function requiredIsEmpty() {
        var result = requiredMissingCount > 0;
        requiredMissingCount = 0;
        return result;
    }

    function getValue(elementId, isRequired, dataType) {
        if(elementId.indexOf("#")<0){
            elementId = "#" + elementId;
        }

        var str = $(elementId).val();
        if (str == null) return "";

        if (str != null && str.toLowerCase().indexOf("</option>") > 0) {
            str = $(elementId).find("option:selected").val();
        }

        str = $.trim(str);

        if ($(elementId)[0].type.toLowerCase() == "textarea" && $(elementId).attr("title") != null) {
            if ($(elementId).val().indexOf($(elementId).attr("title")) >= 0) {
                str = "";
            }
        }

        if (str != "") {
            switch (dataType) {
                case "int":
                    str = str.replace(/\,/g, "");
                    var number = parseInt(str);
                    var test = str.replace(/([0-9]+)/g, "");
                    if (isNaN(number) || test != "") {
                        str = "";
                    }

                    break;
                case "decimal":
                    str = str.replace(/\,/g, "");
                    var number = parseFloat(str);
                    var test = str.replace(/([0-9]+)/g, "");
                    test = test.replace(".", "");
                    if (isNaN(number) || test != "") {
                        str = "";
                    }

                    break;
                case "double":
                    str = str.replace(/\,/g, "");
                    var number = parseFloat(str);
                    var test = str.replace(/([0-9]+)/g, "");
                    test = test.replace(".", "");
                    if (isNaN(number) || test != "") {
                        str = "";
                    }

                    break;
                case "bool":
                    return str == "1" || str == "true"
                    break;
                default:
                    break;
            }
        }

        if (str == "") {
            if (isRequired) {
                requiredMissingCount++;
                hightLight(elementId);
            }
        }
        else if (isRequired) {
            removeHightLight(elementId);
        }

        if ($(elementId)[0].className.indexOf("money")>=0 && str != "") {
            str = ToMoney(str);
        }

        return str;
    }

    var hightLightColor = "Yellow";
    function hightLight(elementId) {
        $(elementId).css({ "background-color": hightLightColor });
    }

    function removeHightLight(elementId) {
        $(elementId).css({ "background-color": "" });
    }

    function checkElements(ids) {
        var result = true;
        var missingIds = "";
        var arr = ids.split(',');
        for (var i = 0; i < arr.length;i++ ) {
            if (arr[i] != "" && document.getElementById(arr[i]) == null) {
                result = false;
                missingIds += arr[i] + ",";
            }
        }

        if (missingIds != "") {
            alert("Cannot find: " + missingIds);
        }

        return result;
    }

    var globalJsonModel = null;
    $(document).ready(function () {
        if (document.getElementById("jsonModelJs") != null) {
            globalJsonModel = jsonModel;
            var ManualFillIds = $("#ManualFillIds").val();
            if (ManualFillIds == null) ManualFillIds = "";
            ManualFillIds = "," + ManualFillIds + ",";

            for (var elementId in jsonModel) {
                if (ManualFillIds.indexOf("," + elementId + ",") >= 0) continue;

                if (jsonModel[elementId] == true) {
                    var radioButton = document.getElementById(elementId);
                    if (radioButton != null) {
                        radioButton.checked = true;
                    }
                }
                else if (jsonModel[elementId] == false) {
                    var radioButton = document.getElementById("Not" + elementId);
                    if (radioButton != null) {
                        radioButton.checked = true;
                    }
                }

                var obj = document.getElementById(elementId);
                if (obj != null) {
                    if (obj.className.indexOf("money") >= 0) {
                        $(obj).val(ToString(jsonModel[elementId]));
                    }
                    else {
                        if (jsonModel[elementId] != null && jsonModel[elementId].toString().indexOf("/Date(") >= 0) {
                            jsonModel[elementId] = parseDateStr(jsonModel[elementId].toString());
                        }

                        if ("td,span".indexOf(obj.tagName.toLowerCase()) >= 0) {
                            $(obj).html(jsonModel[elementId]);
                            continue;
                        }

                        $(obj).val(jsonModel[elementId]);
                    }
                }
            }
        }

        waterMarkTextareas();
        if (document.getElementById("loadData") != null) {
            loadData();
        }
    });

    function parseDateStr(time) {

        if (time != null) {

            if (time.indexOf("/Date(") < 0) return time;

            var date = new Date(parseInt(time.replace("/Date(", "").replace(")/", ""), 10));

            var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;

            var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();

            var dateStr = date.getFullYear() + "-" + month + "-" + currentDate;
            if (dateStr == "1-01-01") dateStr = "";

            return dateStr;

        }

        return "";
    }

    function getModelId() {
        if (globalJsonModel != null) {
            return globalJsonModel.Id;
        }

        return 0;
    }

    function showCurrentHeader() {
        var url = location.href;
        if (url.indexOf("?") > 0) {
            url = url.substr(0, url.indexOf("?"));
        }

        var links = $("#navigateDiv").find("a");
        if (links != null && links.length > 0) {
            for (var i = 0; i < links.length; i++) {
                var pos = url.toLowerCase().indexOf(links[i].href.toLowerCase());
                if (pos >= 0) {
                    var rightPart = url.substr(pos + links[i].href.length);

                    if (rightPart.replace(/\//g, "") == "") {
                        links[i].className = "isCurrent";
                        break;
                    }
                }
            }
        }
    }

    $(document).ready(showCurrentHeader);

    function signOut(){
        $.post(getRootUrl() + "Admin/StopDebugUser", null, function (json) {
            if (json.Success) {
                location.href = getRootUrl();
            }
        });
    }

    function watermarkTextarea(obj) {
        if (obj != null && obj.title != null) {
            $(obj).focus(function () {
                var html = $(obj).html().replace(/\&amp\;/g, "&");
                if (html.indexOf(obj.title) >= 0) {
                    $(obj).html("");
                    $(obj).css({ "color": "" });
                }
            });

            $(obj).blur(function () {
                if ($.trim($(obj).html()) == "") {
                    $(obj).html(obj.title);
                    $(obj).css({ "color": "#999" });
                }
            });
        }
    }

    function waterMarkTextareas() {
        var textareas = document.getElementsByTagName("TEXTAREA");
        if (textareas != null && textareas.length > 0) {
            for (var i = 0; i < textareas.length; i++) {
                if (textareas[i].title != null && textareas[i].title != "") {
                    watermarkTextarea(textareas[i]);
                    if($.trim($(textareas[i]).html()) == ""){
                        $(textareas[i]).html(textareas[i].title);
                        $(textareas[i]).css({ "color": "#999" });
                    }
                }
            }
        }
    }

    function show(msg) {
        popup("<div style='padding:5px 15px 15px;text-align:left;line-height:30px;font-size:14px;'>" + msg + "</div>");
    }

    function confirmAction(msg, confirmMessage, url, id, actionId, callback, remarkCanbeEmpty) {
        var ok = "OK";
        var cancel = "Cancel";
        if (getLanguage() == "Chinese") {
            ok = "确定";
            cancel = "取消";
        }

        var html = "<div style='padding:5px 15px 15px;text-align:left;line-height:30px;font-size:14px;'>";
        html += "    " + msg + "<br />\r\n";
        html += "    <textarea id=\"confirmRemark\" style=\"width:350px;height:100px;\"></textarea>\r\n";
        html += "    <div style='margin-top:10px;'>\r\n";
        html += "    <button type=\"button\" class=\"button\" onmousedown=\"this.className='buttonPressed'\" onmouseout=\"this.className='button'\" onclick=\"confirmFunction(";
        html += "'" + msg.replace(/\'/g, '') + "', '" + confirmMessage + "', '" + url + "', " + id + ", " + actionId + ", " + callback + ", " + remarkCanbeEmpty;
        html += ");\">" + ok + "</button>\r\n";
        html += "    <button type=\"button\" class=\"button\" onmousedown=\"this.className='buttonPressed'\" onmouseout=\"this.className='button'\" onclick=\"closeDialogDiv();\">" + cancel + "</button>\r\n";
        html += "    </div>\r\n";
        html += "</div>\r\n";

        popup(html);
    }

    function confirmFunction(msg, confirmMessage, url, id, actionId, callback, remarkCanbeEmpty) {
        var remark = $.trim($("#confirmRemark").val());
        if (remarkCanbeEmpty) {
            if (confirm(confirmMessage)) {
                closeDialogDiv();
                show("Please wait...");
                $.post(url, { id: id, remark: remark, actionId: actionId }, function (json) {
                    callback(json);
                });
            }
        }
        else {
            if (remark == "") alert(msg);
            else if (remark.length < 2) {
                var msg2 = "Your reason is too short!";
                if (getLanguage() == "Chinese") {
                    msg2 = "您输入的原因太短!";
                }

                alert(msg2);
            }
            else if (confirm(confirmMessage)) {
                show("Please wait...");
                $.post(url, { id: id, remark: remark, actionId: actionId }, function (json) {
                    callback(json);
                });
            }
        }
    }

    function setLanguage(languageEnglishName) {
        $.post("/SKURegistration/Home/SetLanguage", { languageEnglishName: languageEnglishName }, function (response) {
            if (response.toLowerCase() == "true") {
                window.location.href = window.location.href;
            }
            else {
                alert("Set language failed!");
            }
        });
    }

    var popDivCount = 0;
    function pop(msg, width, height) {
        if (msg != null) {
            msg = msg.toString();
        }
        else return;

        popDivCount++;
        var divId = "pop" + popDivCount.toString();
        closeDialogDiv();
        var emailReg = /[a-zA-Z0-9\.]+@\S+\.com/;
        var email = msg.match(emailReg);
        if (email != null) {
            msg = msg.replace(email, "<a href='mailto:" + email + "'>" + email + "</a>");
        }

        if (width == null) {
            width = "500";
        }

        var html = "<div style='font-size:14px;padding:15px;line-height:24px;width:" + width + "px;text-align:left;";
        if (height != null && height > 0) {
            html += "height:" + height + "px;overflow:scroll;";
        }

        html += "'>";

        popup(html + msg + "</div>", divId);
    }

    function closeWindow() {
        window.opener = null;
        window.open("", "_self");
        window.close();
    }

    function popFailure() {
        var message = getOperationFailureMessage();
        var html = "<div style='font-size:14px;padding:15px;line-height:24px;text-align:center;'>" + message + "</div>";
        popDivCount++;
        var divId = "pop" + popDivCount.toString();
        closeDialogDiv();
        popup(html, divId);
    }

    function emptyAlert() {
        if (getLanguage() == "Chinese") {
            alert("请填写完整再提交！");
        }
        else alert("Required fields cannot be empty!");
    }

    function getOptionHtml(selectId, selectedValue) {
        var html = "";
        $("#" + selectId + " option").each(function (index, item) {
            html += "<option value=\"" + item.value + "\"";
            if (item.value == selectedValue) {
                html += " selected=\"selected\"";
            }

            html += ">" + item.text + "</option>";
        });

        return html;
    }

    function lang(key, elementId) {
        if (elementId == null) {
            var result = key;
            $.ajax({
                data: "get",
                url: getRootUrl() + "Common/Lang",
                data: "key=" + key,
                cache: false,
                async: false,
                success: function (data) {
                    result = data;
                }
            })

            return result;
        }
        else {
            $.get(getRootUrl() + "Common/Lang", { key: key }, function (value) {
                if (elementId != null) {
                    $("#" + elementId).html(value);
                    $("#" + elementId).val(value);
                    $("#" + elementId).text(value);
                }
            });
        }
    }

    function translate(elementId) {
        var key = $.trim($("#" + elementId).html());
        if (key != "") {
            lang(key, elementId);
        }
    }

    function popOK(callback) {
        var language = getLanguage();
        var message = "Saved successfully!";
        if (language == "Chinese") {
            message = "保存成功！";
        }

        var html = "<div style='font-size:14px;padding:15px;line-height:24px;text-align:center;'>" + message + "</div>";
        popDivCount++;
        var divId = "pop" + popDivCount.toString();
        closeDialogDiv();
        popup(html, divId);
        if (callback == null) {
            callback = closeDialogDiv;
        }

        setTimeout(callback, 2000);
    }