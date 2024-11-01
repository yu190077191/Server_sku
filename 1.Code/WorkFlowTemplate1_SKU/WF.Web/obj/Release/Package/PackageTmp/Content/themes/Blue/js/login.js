function UserLogin(NameID, PWDID, FnSuccess) {
    var user = $("#" + NameID).val();
    var pwd = $("#" + PWDID).val();
    $.ajax({
        type: "GET",
        url: "../request/loginHandler.ashx",
        cache: false,
        async: false,
        data: "username=" + user + "&pwd=" + pwd,
        success: FnSuccess
    });
}

function ShowLogin() {

    $("#loginform").show("slow", function() {
        $("#loginform input")[0].focus();
    });
}
function ShowLogin_press() {

    $("#loginform_press").show("slow", function() {
    $("#loginform_press input")[0].focus();
    });
}
function HideLogin_press() {
    $("#loginform_press").hide("slow");
    }
function CheckLogin() {
    if($("#"+$("#hidUser").val()).val()=="")
    {
        alert("请输入用户名！");
        $("#"+$("#hidUser").val()).focus();
        return false;
    }
    if($("#"+$("#hidPass").val()).val()=="")
    {
        alert("请输入密码！");
        $("#"+$("#hidPass").val()).focus();
        return false;
    }
    return true;
}

//function CheckLogin1() {
//    //debugger;
//    if ($("#" + $("#hidUser1").val()).val() == "") {
//        alert("请输入用户名！");
//        $("#" + $("#hidUser1").val()).focus();
//        return false;
//    }
//    if ($("#" + $("#hidPass1").val()).val() == "") {
//        alert("请输入密码！");
//        $("#" + $("#hidPass1").val()).focus();
//        return false;
//    }
//    return true;
//}

function CheckKeyDownLogin(event) {

    if (event && event.keyCode == 13) {//debugger;

        if (CheckLogin()) {
        __doPostBack($("#hidOK").val(), '');
            //event.keyCode = 9;
        }
    }
    return false;
}

//function CheckKeyDownLogin1(event) {//debugger;
//    if (event && event.keyCode == 13) {
//        if (CheckLogin1()) {
//            __doPostBack('ctl00$MainContent$lbtnOK', '');
////            __doPostBack($("#hidOK1").val(), '');
//            //event.keyCode = 9;
//        }
//    }
//    return false;
//}

function  tFocus()   
{   
    document.getElementById('txtUserName').focus();   
}


function TagClick(obj,num) {
    $("#divnavi li").removeClass();
    $("#divnavi li").eq(num).addClass("cur");
 }