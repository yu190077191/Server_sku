
function checkExist(tableName, obj, oldValue, emptyTheTextboxIfAlreadyExist) {
    var elementId = obj.id;
    if(elementId == null) return;
    var str = $.trim($(obj).val());
    if (oldValue != null) {
        oldValue = $.trim(oldValue.toLowerCase());
        if (oldValue == str.toLowerCase()) {
            return;
        }
    }

    var _id = getModelId();
    if (_id == null) _id = 0;
    var label = $("#" + elementId + "Label");
    label.html("正在查询系统中是否有重复记录，请稍候...");
    $.post("/SKURegistration/Common/CheckExist", { tableName: tableName, columnName: elementId, id: _id, value: str }, function(json){
        if(json.IsDuplicated){
            var url = location.href;
            if(url.indexOf("?")> 0){
                url = url.substr(0, url.indexOf("?"));
            }

            url += "?id=" + json.Data[0].Id;
            var html = "<span style=\"color:red;\">系统中存在重复记录！</span><a href=\"" + url + "\" target='_blank'>点击这里查看</a>";
            label.html(html);
            if (emptyTheTextboxIfAlreadyExist == true) {
                $(obj).val("");
            }
        }
        else {
            label.html("<span style=\"color:green;\">可以添加，没有找到重复记录。</span>");
        }
    });
}

function autoComplete(elementId) {
    $("#" + elementId).autocomplete(getRootUrl() + "Common/AutoComplete?type=" + elementId, {
        extraParams: { time: new Date(), subType: function () { return 0; } },
        minChars: 1,
        max: 50,
        scrollHeight: 400,
        width: 200,
        mustMatch: true,
        dataType: 'json',
        parse: function (data) {
            return $.map(data, function (row) {
                return {
                    data: row,
                    value: row.Value,
                    result: row.Value
                }
            });
        },
        formatItem: function (row, i, max) {
            var html = "<table style='width:200px;'>";
            html += "<tr><td align='left'>" + row.Name + "</td></tr>";
            html += "</table>";
            return html;
        },
        formatMatch: function (item) {
            return row.Value;
        },
        formatResult: function (row) {
            return row.Value;
        }
    }).result(function (event, row) {
        if (row == null) return;
        $("#" + elementId).val(row.Value);
        //if (elementId.indexOf("Parent") > 0) {
        //    $("#ParentId").val(row.Id);
        //}
        //else if (document.getElementById("ForeignId") != null) {
        //    $("#ForeignId").val(row.Id);
        //    $("#TableName").val(elementId);
        //}
        //else {
        //    $("#" + elementId + "Id").val(row.Id);
        //}
        
        $("#" + elementId).flushCache();
    });
}