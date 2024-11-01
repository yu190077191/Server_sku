$(document).ready(function(){
    var id=$("#hidaid").val();
    $("#"+id).addClass("styleValue");
});

function enterClick(){
    if(event.keyCode==13){
        pl1();
        return false;
    }
}

function pl1()
{
    var userName=$("#ctl00_txtUserName").val();
    var password=$("#ctl00_txtPassWord").val();
    if(userName=="" || password=="")
    {
        window.alert("用户名和密码不能为空！");
        return;
    }
    else
    {
        $.ajax({
            type:"GET",
            url:"../request/pl.ashx",
            cache:false,
            data:"userName="+userName+"&password="+password+"&type=1",
            success:function(msg){
                if (msg == "no")
                {
                    window.alert("用户名或密码错误！");return;
                }else if (msg == "noAct")
                {
                    $("#ctl00_login")[0].style.display="none";
                    $("#ctl00_welcome")[0].style.display="block";
                    window.alert("会员账号还未激活，请先激活！")
                    window.location.href='../wpress/ActUser.aspx';
                }
                else
                {
                    $("#ctl00_login")[0].style.display="none";
                    $("#ctl00_welcome")[0].style.display="block";
                    $("#ctl00_userName_lable").html(msg);
                    location.reload();
                }
            }
        });
    }
}

function pl1_register()
{
    var userName=$("#uName").val();
    var password=$("#uPassword").val();
    if(userName=="" || password=="")
    {
        window.alert("用户名和密码不能为空！");
        return;
    }
    else
    {
        $.ajax({
            type:"GET",
            url:"../request/pl.ashx",
            cache:false,
            data:"userName="+userName+"&password="+password+"&type=1",
            success:function(msg){
                if (msg == "no")
                {
                    window.alert("用户名或密码错误！");return;
                }else if (msg == "noAct")
                {
                    $("#ctl00_login")[0].style.display="none";
                    $("#ctl00_welcome")[0].style.display="block";
                    window.alert("会员账号还未激活，请先激活！")
                    window.location.href='../wpress/ActUser.aspx';
                }
                else
                {
                    $("#ctl00_login")[0].style.display="none";
                    $("#ctl00_welcome")[0].style.display="block";
                    $("#ctl00_userName_lable").html(msg);
                    window.location.href='../wpress/index.aspx';
                }
            }
        });
    }
}

function pl2()
{
    var userName=$("#ctl00_txtUserName").val();
    if(userName=="" )
    {
        window.alert("用户名不能为空！");
        return;
    }
    else
    {
        $.ajax({
            type:"GET",
            url:"../request/pl.ashx",
            cache:false,
            data:"userName="+userName+"&type=2",
            success:function(msg){
                window.alert(msg);return;
            }
        });
    }
}

function pl3()
{
    $.ajax({
        type:"GET",
        url:"../request/pl.ashx",
        cache:false,
        data:"type=3",
        success:function(msg){
            if(msg=="yes")
            {
                $("#ctl00_login")[0].style.display="block";
                $("#ctl00_welcome")[0].style.display="none";
                $("#ctl00_userName").html("");
                $("#ctl00_txtPassWord").val("");
                window.location.href='../wpress/index.aspx';
            }
        }
    });

}

function pl4()
{
    $.ajax({
        type:"GET",
        url:"../request/pl.ashx",
        cache:false,
        data:"type=4",
        success:function(msg){
            if(msg=="no")
            {
                window.alert("请先登录！");return;
            }
            else
            {
                window.location.href=msg;
            }
        }
    });
}

function pl5()
{
    $.ajax({
        type:"GET",
        url:"../request/pl.ashx",
        cache:false,
        data:"type=5",
        success:function(msg){
            if(msg=="no")
            {
                window.alert("该模块需要登录后使用，请先登录！");
                window.location.href='../wpress/RegisterPage.aspx';
            }
            else
            {
                //alert(msg);
                //window.location.href=msg;
            }
        }
    });

}
