function displayBU() {
    var department = $.trim($("#DepartmentId").find("option:selected").text()).toUpperCase();
    var p = department.indexOf(" N");
    if(p > 0){
        department = department.substr(p + 1).trim();
    }
    else if(department.indexOf("GRFL")>=0)department = "GRFL";
    else{
        p = department.lastIndexOf(" ");
        if(p > 0){
            department = department.substr(p + 1).trim();
        }
    }

    var mapping = new Array();
    mapping.push({"BU":"Dairy","Factory":"NSL, NQL Dairy, NHL, NSL DFI"});
    mapping.push({"BU":"Industrial","Factory":"NSL, NQL Dairy, NHL,NDL"});
    mapping.push({"BU":"Infant Nutrition","Factory":"NSL"});
    mapping.push({"BU":"Coffee & Beverage","Factory":"NDL,NQL Coffee,NSHL"});
    mapping.push({"BU":"Confectionery","Factory":"NTL"});
    mapping.push({"BU":"NP","Factory":"NSL,NTL,NQL Dairy, NSHL,MDL,NDL"});
    mapping.push({"BU":"NPP","Factory":"NPPTL"});
    mapping.push({"BU":"CPW","Factory":"CPWTJ"});
    mapping.push({"BU":"NHS","Factory":"NHS Taizhou"});
    mapping.push({"BU":"Food","Factory":"MDL"});
    mapping.push({"BU":"Ice Cream","Factory":"NTL, GRFL"});

    var options = "";
    var count = 0;
    $(mapping).each(function(){
        if(this.Factory.toUpperCase().indexOf(department) >= 0){
            count++;
            options += "<option value=\""+ this.BU +"\">"+ this.BU +"</option>\r\n";
        }
    });

    if(options != ""){
        if(count > 1){
            options = "<option value=\"\">Select</option>\r\n" + options;
        }

        $("#BU").html(options);
        if (document.getElementById("jsonModelJs") != null) {
            globalJsonModel = jsonModel;
            $("#BU").val(jsonModel["BU"]);
        }
    }
}

$(function () {
    displayBU();
});

function departmentChanged(){
    displayBU();
    var department = $.trim($("#DepartmentId").find("option:selected").text()).toUpperCase();
    var timeStamp = new Date().toLocaleTimeString();
    var href = getRootUrl() + (department.indexOf("DC")<0?"Factory/":"") + "Stock Write-off Request Form Template.xlsx?v" + timeStamp;
    document.getElementById("templateHref").href=href;
}