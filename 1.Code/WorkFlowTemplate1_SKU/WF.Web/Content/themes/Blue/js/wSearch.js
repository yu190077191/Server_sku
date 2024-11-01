function jump(url) {   
    window.parent.document.getElementById("divShow").display = "none";
    window.parent.location.href = url.toString();
}

function PopAdvancedSearch() {
    
    if (window.parent.document.getElementById("indexIframe"))
    {
        window.parent.document.getElementById("indexIframe").style.display = "none";
    }
    if (window.parent.document.getElementById("shown"))
    {
        window.parent.document.getElementById("shown").style.display = "none";
    }
    var divObj = $(window.parent.document).find("#divShow");
    //divObj.html("<div id='divbg' class='bg' ></div>" +
    //    "<iframe id='frmBook' frameborder=\"0\" scrolling=\"no\" marginheight=\"0\" allowtransparency='ture' name='frmBook' class='hideiframe' src='" + document.getElementById("hidUrl").value.toString() + "wpress/Search.aspx' " +
    //    "frameborder='0' scrolling='auto'style='height:300px;width:600px;margin-left:-338px; z-index:10000;'></iframe>");
        
    divObj.html("<div id='divbg' class='bg' ></div>" +
        "<iframe id='frmBook' frameborder=\"0\" scrolling=\"no\" marginheight=\"0\" allowtransparency='ture' name='frmBook' class='hideiframe' src='../wpress/Search.aspx' " +
        "frameborder='0' scrolling='auto'style='height:300px;width:600px;margin-left:-338px; z-index:10000;'></iframe>");
    divObj.show("fast");
}

function search() {
    var searchKey = "condition=资源名称=?" + document.getElementById("searchName").value.toString();
    window.location.href = document.getElementById("hidUrl").value.toString() + "wpress/ResourcesList.aspx?" + searchKey + "&key=" + document.getElementById("searchName").value.toString();
}

function searchClose()
{
    if (window.parent.document.getElementById("divShow"))
    {
        window.parent.document.getElementById("divShow").style.display = "none";
    }
    if (window.parent.document.getElementById("indexIframe"))
    {
        window.parent.document.getElementById("indexIframe").style.display = "";
    }
    if (window.parent.document.getElementById("shown"))
    {
        window.parent.document.getElementById("shown").style.display = "";
    }
}

function focusCheck(buttonId)
{
    if (document.getElementById(buttonId))
    {
        var focusButton = document.getElementById(buttonId);
        focusButton.focus();
    }
}

function FocusCheckKeyDownLogin(event) {
    if (event && event.keyCode == 13) {
        if (document.getElementById("fastSearch"))
        {
            var focusButton = document.getElementById("fastSearch");
            focusButton.focus();
        }
    }
    return false;
}

