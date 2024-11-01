$.ajaxSetup({ cache: false });

var editPage = "WF/Edit";
var workflowAPI = "WorkFlowAPI";
var buttonDiv = $("#buttonDiv");

var workFlowRequestVersionId = 0;
var workFlowAction = null;
var _workFlowIsRunning = false;
function api(action, eventId, workFlowId) {
    action = action.toLowerCase();
    workFlowAction = action;
    workFlowRequestVersionId = getModelId();
    if(_workFlowIsRunning){
        alert("The system is busy, please try it later!");
        return;
    }

    if (workFlowAction == null || workFlowAction == "") {
        alert("Invalid action name!")
        return;
    }

    if (isNaN(workFlowRequestVersionId) || workFlowRequestVersionId <= 0) {
        alert("Invalid id!")
        return;
    }
    if ("Withdraw".indexOf(action) >= 0 && MDC_Completed == "True" && (MDC_Input_Completed).toLowerCase() == "false" && type=="Creation") {
        alert("Request is not allowed to change after MDC step approved");
        return;
    } else if ("Withdraw".indexOf(action) >= 0 && MDC_Completed == "True" && (Trigger_inSAP_Completed).toLowerCase() == "false" && type.indexOf("Extend")>0) {
        alert("Request is not allowed to change after MDC step approved");
        return;
    }
    if ("Withdraw".indexOf(action) >= 0 && RequestVersionState == 2) {
        alert("审批状态为Approved，用户不可以再重启");
        return;
    }
    if (hasEmptyFields() == true && "approve,save,ProcessInSAP".indexOf(action) >= 0) {
        alert("The fields highlighted in yellow are mandatory!\n页面上标为黄色的是必填项，请填写完整再提交！");
        return;
    }
    else if ("approve,save,ProcessInSAP".indexOf(action) >= 0) {
        if (CheckFields() == false) {
            return;
        }
        saveField();
    }


    switch (action) {
        case "approve":
            if ($("#FVstepName").val() == "final validation") {
                if ($("#FV").val() == "true") {
                    approve(eventId, workFlowId);
                }
                else {
                    alert("Kindly check the value of Bonus Weight/Volume is mandatory due to the value of Oversize maintained in Finance data with Y.");
                    showCustomerNumber(workFlowRequestVersionId);
                }
            }
            else
            {
                approve(eventId, workFlowId);
            }
            break;
        case "reject":
            reject(eventId, workFlowId);
            break;
        case "return":
            returnThis(eventId, workFlowId);
            break;
        case "edit":
            edit();
            break;
        case "submit":
            submit(eventId, workFlowId);
            break;
        case "withdraw":
            withdraw(eventId, workFlowId);
            break;
        case "save":
            saveComment(eventId, workFlowId);
            break;
        case "LGO BR/NPDI update".toLowerCase():
            submitNPDI(eventId, workFlowId);
            break;
        case "ProcessInSAP".toLowerCase():
            ProcessInSAP(eventId, workFlowId);
            break;
        case "SapStep".toLowerCase():
            completeSAPStep(eventId, workFlowId);
            break;
            
        default:
            throwNotImplementedException();
            break;
    }
}

function throwNotImplementedException() {
    alert(workFlowAction + " is not emplemented!");
}

function editSameVersion() {
    if (MDC_Input_Completed == "True" && type == "Creation") {
        alert("Material Code已经生成，有任何主数据变更请提交新的申请");
        return;
    } else if (Trigger_inSAP_Completed == "True" && type.indexOf("Extend") > 0) {
        alert("Material Code已经生成，有任何主数据变更请提交新的申请");
        return;
    } else if (MDC_Completed == "True" && type.indexOf("Update") > 0) {
        alert("Material Code已经生成，有任何主数据变更请提交新的申请");
        return;
    }
    workFlowRequestVersionId = getModelId();
    var href = getRootUrl() + editPage + "?id=" + workFlowRequestVersionId + "&listType=" + $("#xbdType").attr("data-Type");
    location.href = href;
}

function showCustomerNumber(RequestVersionId) {
    var html = '';
    html += '<div>';
    html += '    Bonus Weight/Volume:     ';
    html += '    <input type="text" id="CustomerNumber" style="width:90px;"/><br>';
    html += '  UoM for Bonus Weight/Volume:';
    html += '<select id="Control_27" class="selectControl" onchange="selectedIndexChanged(this);">';
    html += '<option value="">Select</option>';
    html += '<option value="EA each">EA each</option>';
    html += '<option value="G Gram">G Gram</option>';
    html += '<option value="KG Kilogram">KG Kilogram</option>';
    html += '<option value="L litre">L litre</option>';
    html += '<option value="CS Case">CS Case</option>';
    html += '<option value="M metre">M metre</option>';
    html += '<option value="M2 square metre">M2 square metre</option>';
    html += '<option value="ML millilitre">ML millilitre</option>';
    html += ' <option value="CT carton">CT carton</option>';
    html += '<option value="DM decimetre">DM decimetre</option>';
    html += '<option value="DR Drum">DR Drum</option>';
    html += '<option value="ROL Roll">ROL Roll</option>';
    html += '<option value="TON ton US">TON ton US</option>';
    html += '<option value="CM3 cubic centimetre">CM3 cubic centimetre</option>';
    html += ' <option value="DM3 cubic decimetre">DM3 cubic decimetre</option>';
    html += '</select>';
    html += '    <br><button onclick="saveCustomerNumber(this,+'+RequestVersionId+');">';
    html += '        保存';
    html += '    </button>';
    html += '</div>';
    pop(html, 420);
}

function saveCustomerNumber(btn, RequestVersionId) {
    var f26= $(btn).parent().find("input")[0].value;
    var f27 = $(btn).parent().find("[id='Control_27']").val();
    var model = new Object();
    model.RequestId = RequestVersionId;
    model.f26 = f26;
    model.f27 = f27;
   // model.seltest = 
    if (f26 == "" || f27 == "") {
        alert("Kindly check the value of Bonus Weight/Volume is mandatory .");

    }
    else {
        var json = $.toJSON(model);
        $.post(getRootUrl() + "WF/UpdateField", { json: json }, function (json) {
            if (json.Success) {
                popOK();
                setTimeout(function () {
                    location.href = location.href
                }, 1000);
            }
        });

    }
    
    
}



function submit(eventId, workFlowId) {
    if (mandatoryIsEmpty) {
        alert("带*的是必填项！");
        return;
    }

    var atctionName = "submit";
    var actionUrl = getRootUrl() + workflowAPI + "/ReviewRequest";
    workFlowRequestVersionId = getModelId();
    apiConfirmAction(
        "Remarks(optional):",
        "Are you sure to submit it? After you submit it, an email will be sent to the first approver to remind him/her to review your request.",
        actionUrl,
        workFlowRequestVersionId,
        eventId,
        "submitFeedback",
        true,
        workFlowId
    );
}

function approve(eventId, workFlowId) {
    var atctionName = "approve";
    var actionUrl = getRootUrl() + workflowAPI + "/ReviewRequest";
    workFlowRequestVersionId = getModelId();
    apiConfirmAction(
        "Remarks(optional):",
        "Are you sure to " + atctionName + " it? ",
        actionUrl,
        workFlowRequestVersionId,
        eventId,
        "submitFeedback",
        true,
        workFlowId
    );
}

function reject(eventId, workFlowId) {
    var atctionName = "reject";
    var actionUrl = getRootUrl() + workflowAPI + "/ReviewRequest";
    workFlowRequestVersionId = getModelId();
    apiConfirmAction(
        "Please provide the reason for why you reject it:",
        "Are you sure to " + atctionName + " it? ",
        actionUrl,
        workFlowRequestVersionId,
        eventId,
        "submitFeedback",
        false,
        workFlowId
    );
}

function returnThis(eventId, workFlowId) {
    var atctionName = "return";
    var actionUrl = getRootUrl() + workflowAPI + "/ReviewRequest";
    workFlowRequestVersionId = getModelId();
    apiConfirmAction(
        "Please provide the reason for why you " + atctionName + " it:",
        "Are you sure to " + atctionName + " it? ",
        actionUrl,
        workFlowRequestVersionId,
        eventId,
        "submitFeedback",
        false,
        workFlowId
    );
    if (StepName == "MDC" || StepName == "MDC Create SKU Code in SAP") {
        var $list = document.querySelector(".list");
        $list.addEventListener("click", DisReturnStep, false);
        var $selectpicker = document.querySelector(".ul");
        $selectpicker.addEventListener("click", ChooseReturnStep, false);
        var $sel_pyCoordinator = document.querySelector("#sel_ReturnStep");
        $sel_pyCoordinator.addEventListener("click", ShowReturnStep, false);
    }
}

function submitFeedback(json) {
    closeMe();
    if (json == null) {
        alert("Feedback is NULL!");
        return;
    }

    if (json.Success) {
        popOK();
        setTimeout(function () { location.href = location.href; }, 2000);
    }
    else {
        alert(json.Message);
    }
}

function withdraw(eventId, workFlowId) {
    var atctionName = "withdraw";
    var actionUrl = getRootUrl() + workflowAPI + "/ReviewRequest";
    workFlowRequestVersionId = getModelId();
    apiConfirmAction(
        "Please provide the reason for why you " + atctionName + " it:",
        "Are you sure to " + atctionName + " it? ",
        actionUrl,
        workFlowRequestVersionId,
        eventId,
        "submitFeedback",
        false,
        workFlowId
    );
}

function saveComment(eventId, workFlowId) {
    var atctionName = "save";
    var actionUrl = getRootUrl() + workflowAPI + "/ReviewRequest";
    workFlowRequestVersionId = getModelId();
    apiConfirmAction(
        "Please provide your comment:",
        "Are you sure to submit it now? ",
        actionUrl,
        workFlowRequestVersionId,
        eventId,
        "submitFeedback",
        false,
        workFlowId
    );
}

function completeSAPStep(eventId, workFlowId) {
    var atctionName = "save";
    var actionUrl = getRootUrl() + workflowAPI + "/ReviewRequest";
    workFlowRequestVersionId = getModelId();
    apiConfirmAction(
        "Please provide your comment (optional):",
        "Are you sure that you have already completed the step in SAP? ",
        actionUrl,
        workFlowRequestVersionId,
        eventId,
        "submitFeedback",
        true,
        workFlowId
    );
}

var wfPopCounter = 0;
var actionData = new Object();
function apiConfirmAction(msg, confirmMessage, url, id, actionId, callback, remarkCanbeEmpty, workFlowId) {
    wfPopCounter++;
    var ok = "OK";
    var cancel = "Cancel";
    if (getLanguage() == "Chinese") {
        ok = "确定";
        cancel = "取消";
    }

    actionData = new Object();
    actionData.RequestId = requestId;
    actionData.RequestVersionId = id;
    actionData.DepartmentId = 0;
    actionData.EventId = actionId;
    actionData.WorkFlowId = workFlowId;
    actionData.TypeCode = "default";
    actionData.ReturnStep = "";
    actionData.Comment = "";

    var specialInput = $("#specialInput").html().replace(/\PopId/g, wfPopCounter.toString());

    var html = "<div style='padding:5px 15px 15px;text-align:left;line-height:30px;font-size:14px;'>";
    html += "    " + msg + "<br />\r\n";
    if (confirmMessage.indexOf("return") > 0 && (StepName == "MDC" || StepName == "MDC Create SKU Code in SAP")) {
        html += '<div class="wraper"><div><input type="text" id="sel_ReturnStep" value=""><input type="hidden" id="hid_ReturnStep" value=""> '
        html += '</div><div class="list" style="width: 200px; height: 200px;"><ul class="ul">';
        if (ReturnStepList != undefined) {
            $.each(ReturnStepList.split('|'), function (i, item) {
                var itemArray = item.split('&amp;');
                html += '<li><input type="checkbox" value="' + itemArray[0] + '" /><span>' + itemArray[1] + '</span></li>';
            });
        }
        html += '</ul>';
        html += "</div></div>"
    }
    html += "    <textarea id=\"confirmRemark\" style=\"width:350px;height:100px;\"></textarea>\r\n";
    if (specialInput.indexOf(">") > 0) {
        html += specialInput;
    }
    
    html += "    <div style='margin-top:10px;'>\r\n";
    html += "    <button type=\"button\" class=\"button\" onmousedown=\"this.className='buttonPressed'\" onmouseout=\"this.className='button'\" onclick=\"apiConfirmFunction(";
    html += "'" + msg.replace(/\'/g, '') + "', '" + confirmMessage + "', '" + url + "', " + callback + ", " + remarkCanbeEmpty;
    html += ");\">" + ok + "</button>\r\n";
    html += "    <button type=\"button\" class=\"button\" onmousedown=\"this.className='buttonPressed'\" onmouseout=\"this.className='button'\" onclick=\"closeDialogDiv();\">" + cancel + "</button>\r\n";
    html += "    </div>\r\n";
    html += "</div>\r\n";

    popup(html);
}

$(document).click(function () {
    $(".list").hide();
});
function ShowReturnStep(e) {
    $(".list").hide();
    $(".list").toggle();
    e.stopPropagation();
};
function DisReturnStep(e) {
    e.stopPropagation();
};
function ChooseReturnStep(e) {
    var kArr = new Array();
    var kArrName = new Array();
    $("input:checked", $(".list").find("ul")).each(function (index) {
        kArr[index] = $(this).val();
        kArrName[index] = $(this).next().text();
    });
    $("#sel_ReturnStep").val(kArrName.join(","));
    $("#hid_ReturnStep").val(kArr.join(","));
    e.stopPropagation();
};

var isWorkFlowAPIBusy = false;
function apiConfirmFunction(msg, confirmMessage, url, callback, remarkCanbeEmpty) {
    buttonDiv = $("#buttonDiv");
    var remark = $.trim($("#confirmRemark").val());
    if ($("#hid_ReturnStep").val() != undefined) {
        actionData.ReturnStep = $("#hid_ReturnStep").val();
    }
    actionData.Comment = remark;

    if (isWorkFlowAPIBusy) {
        show("The system is busy...");
        return;
    }
    if (confirmMessage.indexOf("return") > 0) {
        if (StepName == "MDC" || StepName == "MDC Create SKU Code in SAP") {
            actionData.ReturnStep = $("#hid_ReturnStep").val();
            if (actionData.ReturnStep == "") {
                show("请选择ReturnStep");
                return;
            }
        }
    }

    if (remarkCanbeEmpty) {
        if (confirm(confirmMessage)) {
            closeDialogDiv();
            show("Please wait...");
            buttonDiv.css({ "display": "none" });
            isWorkFlowAPIBusy = true;
            $.post(url, { json: $.toJSON(actionData) }, function (json) {
                isWorkFlowAPIBusy = false;
                callback(json);
                //buttonDiv.css({ "display": "" });
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
            buttonDiv.css({ "display": "none" });
            isWorkFlowAPIBusy = true;
            $.post(url, { json: $.toJSON(actionData) }, function (json) {
                isWorkFlowAPIBusy = false;
                callback(json);
                buttonDiv.css({ "display": "" });
            });
        }
    }
}

//function getStartSequence() {
//    var trs = $("#approversTable").find("tr");
//    for (var i = 0; i < trs.length; i++) {
//        if (trs[i].id != null && trs[i].id.indexOf("tr") == 0) {
//            var tr = $(trs[i]);
//            if (tr.css("display") == "none") {
//                continue;
//            }

//            var oldSequenceInfo = $("#" + trs[i].id + " td")[0].id;
//            if (oldSequenceInfo != null && oldSequenceInfo.indexOf("Sequence") == 0) {
//                var arr = oldSequenceInfo.replace("Sequence", "").split('_');
//                var startSequence = parseInt(arr[0]) - 1;
//                return startSequence;
//            }
//        }
//    }
//}

function saveApprovers(callback) {
    var array = new Array();
    var trs = $("#approversTable").find("tr");
    var lastSequence = 0;
    var isLastManuallyAdded = false;
    var isManuallyAdded = false;
    var rowNumber = 0;
    for (var i = 0; i < trs.length; i++) {
        if (trs[i].id != null && trs[i].id.indexOf("tr") == 0) {
            var tr = $(trs[i]);
            if (tr.css("display") == "none") {
                continue;
            }

            var model = new Object();

            rowNumber++;
            var employeeId = trs[i].id.substr(2);
            if (employeeId.length > 8) {
                isManuallyAdded = true;
            }
            else {
                isManuallyAdded = false;
            }

            model.ApproveType = 0;
            var ApproveType = $("#approverType" + employeeId).val();
            if (ApproveType != null && ApproveType != "" && parseInt(ApproveType) >= 0) {
                model.ApproveType = parseInt(ApproveType);
            }

            var oldSequenceInfo = $("#tr" + employeeId + " td")[0].id;
            if (oldSequenceInfo != null && oldSequenceInfo.indexOf("Sequence") == 0) {
                var arr = oldSequenceInfo.replace("Sequence", "").split('_');
                model.WorkFlowPatternEventContainerId = arr[0];
                model.Id = arr[1];
                model.EmployeeId = arr[2];
            }

            model.RequestVersionId = getModelId();
            model.MinSequence = rowOffSet + rowNumber;

            if (isManuallyAdded) {
                model.EmployeeId = employeeId;

                var message = "";
                var a = tr.html().toLowerCase().indexOf("<span>");
                if (a > 0) {
                    a += "<span>".length;
                    var b = tr.html().toLowerCase().indexOf("</span>", a);
                    if (b > a) {
                        message = tr.html().substr(a, b - a);
                    }
                }

                model.Comment = message;
                array.push(model);
            }
            else {
                model.Comment = "{auto}";
                array.push(model);;
            }

            if (employeeId.length > 8) {
                isLastManuallyAdded = true;
                lastSequence = 0;
            }
            else {
                isLastManuallyAdded = false;
                lastSequence = parseInt(employeeId);
            }
        }
    }

    $.post(getRootUrl() + workflowAPI + "/SaveApprovers", { json: $.toJSON(array) }, function (json) {
        //if (json.Success) {
        if (callback) {
            callback();
        }
        //}
    });
}

var approverChanged = true;
function selectUser() {
    var str = window.showModalDialog(getRootUrl() + "Admin/?IsSelectUser=1", null, "dialogWidth=1010px;dialogHeight=670px;scroll=no;");
    if (str != null && str.length > 0) {
        var arr = str.split('|');
        var employeeId = $("#position").find("option:selected").val();
        var position = $("#position").find("option:selected").text();
        var msg = "Are you sure to add the approver " + arr[2] + " " + position + "?";
        if (confirm(msg)) {
            var positionTr = $("#tr" + employeeId);
            var justification = getValue("Justification", false, "string");
            if (justification != "") {
                justification = "(Message: <span>" + justification + "</span>) ";
            }

            var newTr = $("<tr id=\"tr" + arr[0] + "\"><td>&nbsp;</td><td>&nbsp;</td><td>" + arr[1] + "</td><td style='padding:3px;'>" + arr[2] + justification + "<button style='margin-left:5px;' type=\"button\" class=\"button\" onmousedown=\"this.className='buttonPressed'\" onmouseout=\"this.className='button'\" onclick=\"deleteApprover('" + arr[0] + "');\">Delete</button></td></tr>");
            if (position.toLowerCase().indexOf("before ") >= 0) {
                positionTr.before(newTr);
            }
            else if (position.toLowerCase().indexOf("after ") >= 0) {
                positionTr.after(newTr);
            }

            approverChanged = true;
        }
    }
}

function deleteApprover(employeeId) {
    $("#tr" + employeeId).hide();
    approverChanged = true;
}

function deleteDepartmentRole(id, employeeName) {
    var msg = "Are you sure to delete " + employeeName + "'s role?";
    if (confirm(msg)) {
        $.post(getRootUrl() + workflowAPI + "/DeleteDepartmentRole", { id: id }, function (json) {
            if (json.Success) {
                $("#roleTR" + id).css({ "display": "none" });
                popOK();
            }
            else {
                closeDialogDiv();
                alert("Sorry, failed!");
            }
        });
    }
}

function showSelect(employeeId, departmentId, titleId, buId) {
    var departmentHtml = getOptionHtml("departmentSelect", departmentId);
    var titleHtml = getOptionHtml("RoleSelect", titleId);
    var buHtml = getOptionHtml("BUDepartmentId", buId);
    closeDialogDiv();
    var html = "<div style='padding:10px;'> <span>" + $("#DisplayName" + employeeId).val() + "</span><br /> <span class=\"inputfl\">BU: </span> <select id=\"buChanged" + employeeId + "\" class=\"inputfl\">" + buHtml + "</select>";
    html += "<span class=\"inputfl\">Department: </span> <select id=\"departmentChanged" + employeeId + "\" class=\"inputfl\">" + departmentHtml + "</select>";
    html += "<span class=\"inputfl\">Title: </span> <select id=\"titleChanged" + employeeId + "\" class=\"inputfl\">" + titleHtml + "</select>";
    html += "<button style=\"float:left;\" type=\"button\" class=\"button\" onmousedown=\"this.className='buttonPressed'\" onmouseout=\"this.className='button'\" onclick=\"saveRole('" + employeeId + "');\">Save</button><div style='clear:both;'></div></div>";
    popup(html, "selectDiv" + employeeId, false);
}

function saveRole(employeeId) {
    var displayName = $("#DisplayName" + employeeId).val();
    displayName = displayName.replace(/\'/g, " ’ ");
    var buId = $("#buChanged" + employeeId).val();
    var bu = $("#buChanged" + employeeId).find("option:selected").text();
    var departmentId = $("#departmentChanged" + employeeId).val();
    var department = $("#departmentChanged" + employeeId).find("option:selected").text();
    var titleId = $("#titleChanged" + employeeId).val();
    var title = $("#titleChanged" + employeeId).find("option:selected").text();
    var msg = "Are you sure to update " + displayName + "'s department to '" + department + "' and title to '" + title + "'?";
    if (titleId == "" || departmentId == "") {
        msg = "";
        if (titleId == "") {
            titleId = "0";
            title = "(empty)";
            msg += "Are you sure to remove " + displayName + "'s title?";
        }

        if (departmentId == "") {
            departmentId = "0";
            department = "(empty)";
            msg += "Are you sure to remove " + displayName + "'s department?";
        }
    }

    if (titleId != "") {
        title = "<b style='color:Green;'>" + title + "</b>";
    }

    if (buId != "0") {
        bu = "<b style='color:Green;'>" + bu + "</b>";
    }
    else {
        bu = "";
    }

    if (departmentId != "") {
        department = "<b style='color:Green;'>" + department + "</b>";
    }

    if (confirm(msg)) {
        $.post(getRootUrl() + workflowAPI + "/AddDepartmentRole", { employeeId: employeeId, departmentId: departmentId, roleId: titleId, buId: buId }, function (json) {
            if (json.Success) {
                $("#tdBU" + employeeId).html(bu);
                $("#tdDepartment" + employeeId).html(department);
                $("#tdTitle" + employeeId).html(title);
                popOK();
            }
            else {
                closeDialogDiv();
                alert("Sorry, failed!");
            }
        });
    }
    else {
        closeDialogDiv();
    }
}

function forward(id) {
    location.href = getRootUrl() + "WF/Approvers?id=" + id;
}

function addApprover() {
    var selectedIds = null;
    var selectedNames = null;
    var str = window.showModalDialog(getRootUrl() + "Common/Search"
        , {
            url: "Employee/EmployeeList",
            viewName: "PartialListSelect",
            AutoLoad: false,
            selectedIds: selectedIds,
            selectedNames: selectedNames
        }
        , "dialogWidth=1010px;dialogHeight=670px;scroll=auto;");
    if (str == null) return;
    var arr = str.split('|');

    var employeeId = $("#position").find("option:selected").val();
    var position = $("#position").find("option:selected").text();
    var msg = "Are you sure to add the approver " + arr[1] + " " + position + "?";
    if (confirm(msg)) {
        var positionTr = $("#tr" + employeeId);
        var justification = getValue("Justification", false, "string");
        if (justification != "") {
            justification = "(Message: <span>" + justification + "</span>) ";
        }

        var approverTypeHtml = "<input type='hidden' id=\"approverType" + arr[0] + "\" value='" + $("#AllowActions").val() + "'/>" + $("#AllowActions").find("option:selected").text();
        var newTr = $("<tr id=\"tr" + arr[0] + "\"><td>&nbsp;</td><td>" + approverTypeHtml + "</td><td>" + arr[2] + "</td><td style='padding:3px;'>" + arr[1] + justification + "<button style='margin-left:5px;' type=\"button\" class=\"button\" onmousedown=\"this.className='buttonPressed'\" onmouseout=\"this.className='button'\" onclick=\"deleteApprover('" + arr[0] + "');\">Delete</button></td></tr>");
        if (position.toLowerCase().indexOf("before ") >= 0) {
            positionTr.before(newTr);
        }
        else if (position.toLowerCase().indexOf("after ") >= 0) {
            positionTr.after(newTr);
        }

        approverChanged = true;
    }
}

function cancelThisRequest(eventId, workFlowId) {
    var atctionName = "cancel";
    var actionUrl = getRootUrl() + workflowAPI + "/ReviewRequest";
    workFlowRequestVersionId = getModelId();
    apiConfirmAction(
        "Please provide the reason for why you cancel it:",
        "Are you sure to " + atctionName + " it? ",
        actionUrl,
        workFlowRequestVersionId,
        eventId,
        "submitFeedback",
        true,
        workFlowId
    );
}

function addNote(eventId, workFlowId) {
    var actionUrl = getRootUrl() + workflowAPI + "/ReviewRequest";
    workFlowRequestVersionId = getModelId();
    apiConfirmAction(
        "Please input your comments here:",
        "Are you sure to save it? ",
        actionUrl,
        workFlowRequestVersionId,
        eventId,
        "submitFeedback",
        true,
        workFlowId
    );
}

//function CopyToHK(eventId, workFlowId) {
//    var actionUrl = getRootUrl() + workflowAPI + "/CopyRequestForHK";
//    workFlowRequestVersionId = getModelId();
//    apiConfirmAction(
//        "Remarks(optional):",
//        "Are you sure to Copy it For HK Approve? ",
//        actionUrl,
//        workFlowRequestVersionId,
//        eventId,
//        "submitFeedback",
//        true,
//        workFlowId
//    );
//}

function ProcessInSAP(eventId, workFlowId) {
    var actionUrl = getRootUrl() + workflowAPI + "/ReviewRequest";
    workFlowRequestVersionId = getModelId();
    apiConfirmAction(
        "Please input your comments here:",
        "Are you sure that you have processed it in SAP and informed the requestor?",
        actionUrl,
        workFlowRequestVersionId,
        eventId,
        "submitFeedback",
        true,
        workFlowId
    );
}

function rollback(WorkFlowApproverId) {
    var actionUrl = getRootUrl() + workflowAPI + "/Rollback";
    if (confirm("Are you sure to rollback the work flow to this approver again?")) {
        $.post(actionUrl, { WorkFlowApproverId: WorkFlowApproverId }, function (json) {
            if (json.Success) {
                popOK();
                setTimeout(function () {
                    location.href = location.href
                }, 1000);
            }
            else {
                popFailure();
            }
        });
    }
}