var CONTEXTPATH = '';//图片路径配置
try
{
    var jspath = document.getElementById("DialogJS");
    if(jspath!=null)
    {
        CONTEXTPATH = jspath.src.replace("Dialog.js","");
    }
}
catch(Error)
{
    alert(Error);
}

function getOs() {
    var OsObject = "";
    if (navigator.userAgent.indexOf("MSIE") > 0) {
        return "MSIE";
    }
    if (isFirefox = navigator.userAgent.indexOf("Firefox") > 0) {
        return "Firefox";
    }
    if (isSafari = navigator.userAgent.indexOf("Safari") > 0) {
        return "Safari";
    }
    if (isCamino = navigator.userAgent.indexOf("Camino") > 0) {
        return "Camino";
    }
    if (isMozilla = navigator.userAgent.indexOf("Gecko/") > 0) {
        return "Gecko";
    }

}

var isIE = navigator.userAgent.toLowerCase().indexOf("msie") != -1;
var isGecko = navigator.userAgent.toLowerCase().indexOf("gecko") != -1;
function $(ele) {
  if (typeof(ele) == 'string'){
    ele = document.getElementById(ele)
    if(!ele){
  		return null;
    }
  }
  if(ele){
  	Core.attachMethod(ele);
	}
  return ele;
}
function $T(tagName,ele){
	ele = $(ele);
	ele = ele || document;
	var ts = ele.getElementsByTagName(tagName);//此处返回的不是数组
	var arr = [];
	var len = ts.length;
	for(var i=0;i<len;i++){
		arr.push($(ts[i]));
	}
	return arr;
}
Array.prototype.remove = function(s){
  for(var i=0;i<this.length;i++){
    if(s == this[i]){
    	this.splice(i, 1);
    }
  }
}
if (!isIE && window.HTMLElement) {//给FF添加IE专有的属性和方法
    HTMLElement.prototype.__defineGetter__("parentElement",function(){
        if(this.parentNode==this.ownerDocument)return null;
        return this.parentNode;
        });
	HTMLElement.prototype.__defineSetter__("outerHTML",function(sHTML){
        var r=this.ownerDocument.createRange();
        r.setStartBefore(this);
        var df=r.createContextualFragment(sHTML);
        this.parentNode.replaceChild(df,this);
        return sHTML;
        });
    HTMLElement.prototype.__defineGetter__("outerHTML",function(){
        var attr;
        var attrs=this.attributes;
        var str="<"+this.tagName;
        for(var i=0;i<attrs.length;i++){
            attr=attrs[i];
            if(attr.specified)
                str+=" "+attr.name+'="'+attr.value+'"';
            }
        if(!this.canHaveChildren)
            return str+">";
        return str+">"+this.innerHTML+"</"+this.tagName+">";
        });
    HTMLElement.prototype.__defineSetter__("innerText",function(sText){
        var parsedText=document.createTextNode(sText);
        this.innerHTML=parsedText;
        return parsedText;
        });
    HTMLElement.prototype.__defineGetter__("innerText",function(){
        var r=this.ownerDocument.createRange();
        r.selectNodeContents(this);
        return r.toString();
        });
}
var $E = {};
$E.getTopLevelWindow = function(){
	var pw = window;
	while(pw!=pw.parent){
		pw = pw.parent;
	}
	return pw;
}
$E.hide = function(ele) {
	ele = ele || this;
	ele = $(ele);
  ele.style.display = 'none';
}
$E.show = function(ele) {
	ele = ele || this;
	ele = $(ele);
  ele.style.display = '';
}
var Core = {};
Core.attachMethod = function(ele){
	if(!ele||ele["$A"]){
		return;
	}
	if(ele.nodeType==9){
		return;
	}
	var win;
	try{
		if(isGecko){		
			win = ele.ownerDocument.defaultView;
		}else{
			win = ele.ownerDocument.parentWindow;
		}
		for(var prop in $E){
			ele[prop] = win.$E[prop];
		}
	}catch(ex){
		//alert("Core.attachMethod:"+ele)//有些对象不能附加属性，如flash
	}
}
function Dialog(strID){
		if(!strID){
			alert("错误的Dialog ID！");
			return;
		}
		this.ID = strID;
		this.isModal = true;
		this.Width = 400;
		this.Height = 300;
		this.Top = 0;
		this.Left = 0;
		this.ParentWindow = null;
		this.onLoad = null;
		this.Window = null;
		this.Title = "";
		this.URL = null;
		this.DialogArguments = {};
		this.WindowFlag = false;
		this.Message = null;
		this.MessageTitle = null;
		this.ShowMessageRow = false;
		this.ShowButtonRow = true;
		this.Icon = null;
		this.IsDiv = false;
		this.DivHtml = null;
}
Dialog._Array = [];
Dialog.prototype.showWindow = function(){
	if(isIE){
		this.ParentWindow.showModalessDialog( this.URL, this.DialogArguments, "dialogWidth:" + this.Width + ";dialogHeight:" + this.Height + ";help:no;scroll:no;status:no") ;
	}
	if(isGecko){
		var sOption  = "location=no,menubar=no,status=no;toolbar=no,dependent=yes,dialog=yes,minimizable=no,modal=yes,alwaysRaised=yes,resizable=no";
		this.Window = this.ParentWindow.open( '', this.URL, sOption, true ) ;
		var w = this.Window;
		if ( !w ){
			alert( "发现弹出窗口被阻止，请更改浏览器设置，以便正常使用本功能！" ) ;
			return ;
		}
		w.moveTo( this.Left, this.Top ) ;
		w.resizeTo( this.Width, this.Height+30 ) ;
		w.focus() ;
		w.location.href = this.URL ;
		w.Parent = this.ParentWindow;
		w.dialogArguments = this.DialogArguments;
	}
}
Dialog.prototype.show = function(){
	var pw = $E.getTopLevelWindow();
	var doc = pw.document;
	var cw = doc.compatMode == "BackCompat"?doc.body.clientWidth:doc.documentElement.clientWidth;
	var ch = doc.compatMode == "BackCompat"?doc.body.clientHeight:doc.documentElement.clientHeight;//必须考虑文本框处于页面边缘处，控件显示不全的问题
	var sw = Math.max(doc.documentElement.scrollLeft, doc.body.scrollLeft);
	var sh = Math.max(doc.documentElement.scrollTop, doc.body.scrollTop);//考虑滚动的情况
//	alert("\n"+cw+"\n"+ch+"\n"+sw+"\n"+sh)
if ( !this.ParentWindow ){
	 	this.ParentWindow = window ;
	}	
	this.DialogArguments._DialogInstance = this;
	this.DialogArguments.ID = this.ID;
	
	if(!this.Height){
		this.Height = this.Width/2;
	}
	if(this.Top==0){
		this.Top = (ch - this.Height - 30) / 2 + sh - 8;
	}
	if(this.Left==0){
		this.Left = (cw - this.Width - 12) / 2 +sw;
	}
	if(this.ShowButtonRow){//按钮行高36
		this.Top -= 18;
	}
	if(this.WindowFlag){
		this.showWindow();
		return;
	}
	var arr = [];
	arr.push("<table style='-moz-user-select:none;' oncontextmenu='stopEvent(event);' onselectstart='stopEvent(event);' border='0' cellpadding='0' cellspacing='0' width='"+(this.Width+26)+"'>");
	arr.push("  <tr style='cursor:;'>");
	arr.push("    <td width='13' height='33' style=\"background-image:url("+CONTEXTPATH+"dialog_lt.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='"+CONTEXTPATH+"dialog_lt.png', sizingMethod='crop');\"><div style='width:13px;'></div></td>");
	arr.push("    <td height='33' style=\"background-image:url("+CONTEXTPATH+"dialog_ct.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='"+CONTEXTPATH+"dialog_ct.png', sizingMethod='crop');\"><div style=\"float:left;font-weight:bold; color:#FFFFFF; padding:9px 0 0 4px;\"><img src=\""+CONTEXTPATH+"icon_dialog.gif\" align=\"absmiddle\">&nbsp;"+this.Title+"</div>");
	arr.push("      <div style=\"position: relative;cursor:pointer; float:right; margin:5px 0 0; _margin:4px 0 0;height:17px; width:28px; background-image:url("+CONTEXTPATH+"dialog_closebtn.gif)\" onMouseOver=\"this.style.backgroundImage='url("+CONTEXTPATH+"dialog_closebtn_over.gif)'\" onMouseOut=\"this.style.backgroundImage='url("+CONTEXTPATH+"dialog_closebtn.gif)'\" drag='false' onClick=\"Dialog.getInstance('"+this.ID+"').CancelButton.onclick.apply(Dialog.getInstance('"+this.ID+"').CancelButton,[]);\"></div></td>");
	arr.push("    <td width='13' height='33' style=\"background-image:url("+CONTEXTPATH+"dialog_rt.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='"+CONTEXTPATH+"dialog_rt.png', sizingMethod='crop');\"><div style=\"width:13px;\"></div></td>");
	arr.push("  </tr>");
	arr.push("  <tr drag='false'><td width='13' style=\"background-image:url("+CONTEXTPATH+"dialog_mlm.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='"+CONTEXTPATH+"dialog_mlm.png', sizingMethod='crop');\"></td>");
	arr.push("    <td align='center' valign='top'>");
	arr.push("    <table width='100%' height='100%' border='0' cellpadding='0' cellspacing='0' bgcolor='#FFFFFF'>");
	arr.push("        <tr id='_MessageRow_"+this.ID+"' style='display:none'>");
	arr.push("          <td height='50' valign='top'><table id='_MessageTable_"+this.ID+"' width='100%' border='0' cellspacing='0' cellpadding='8' style=\" background:#EAECE9 url("+CONTEXTPATH+"dialog_bg.jpg) no-repeat right top;\">");
	arr.push("              <tr><td width='25' height='50' align='right'><img id='_MessageIcon_"+this.ID+"' src='"+CONTEXTPATH+"window.gif' width='32' height='32'></td>");
	arr.push("                <td align='left' style='line-height:16px;'>");
	arr.push("                <h5 class='fb' id='_MessageTitle_"+this.ID+"'>&nbsp;</h5>");
	arr.push("                <div id='_Message_"+this.ID+"'>&nbsp;</div></td>");
	arr.push("              </tr></table></td></tr>");
	
	if(!this.IsDiv)
	{
	    arr.push("        <tr><td align='center' valign='top'>");
	    arr.push("          <iframe src='");
	    if(this.URL.indexOf(":")==-1){
		    arr.push(CONTEXTPATH+this.URL);
	    }else{
		    arr.push(this.URL);
	    }
	    arr.push("' id='_DialogFrame_"+this.ID+"' allowTransparency='true'  width='"+this.Width+"' height='"+this.Height+"' frameborder='0' style=\"background-color: #transparent; border:none;\"></iframe></td></tr>");
	}
	else(this.IsDiv)
	{
	    arr.push("        <tr><td align='center' valign='top'>");
	    arr.push(this.DivHtml);
	    arr.push("</td></tr>");
	}
	
	arr.push("        <tr drag='false' id='_ButtonRow_"+this.ID+"'><td height='36'>");
	arr.push("            <div id='_DialogButtons_"+this.ID+"' style='text-align:right; border-top:#dadee5 1px solid; padding:8px 20px; background-color:#f6f6f6;'>");
	arr.push("           	<input id='_ButtonOK_"+this.ID+"'  type='button' value='确 定'>");
	arr.push("           	<input id='_ButtonCancel_"+this.ID+"'  type='button' onclick=\"Dialog.getInstance('"+this.ID+"').close();\" value='取 消'>");
	arr.push("            </div></td></tr>");
	arr.push("      </table></td>");
	arr.push("    <td width='13' style=\"background-image:url("+CONTEXTPATH+"dialog_mrm.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='"+CONTEXTPATH+"dialog_mrm.png', sizingMethod='crop');\"></td></tr>");
	arr.push("  <tr><td width='13' height='13' style=\"background-image:url("+CONTEXTPATH+"dialog_lb.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='"+CONTEXTPATH+"dialog_lb.png', sizingMethod='crop');\"></td>");
	arr.push("    <td style=\"background-image:url("+CONTEXTPATH+"dialog_cb.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='"+CONTEXTPATH+"dialog_cb.png', sizingMethod='crop');\"></td>");
	arr.push("    <td width='13' height='13' style=\"background-image:url("+CONTEXTPATH+"dialog_rb.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='"+CONTEXTPATH+"dialog_rb.png', sizingMethod='crop');\"></td>");
	arr.push("  </tr></table>");
	this.TopWindow = pw;
	var bgdiv = pw.$("_DialogBGDiv");
	if(!bgdiv){
		bgdiv = pw.document.createElement("div");	
		bgdiv.id = "_DialogBGDiv";
		$E.hide(bgdiv);
	 	pw.$T("body")[0].appendChild(bgdiv);
	}
	var div = pw.$("_DialogDiv_"+this.ID);
	if(!div){
		div = pw.document.createElement("div");	
		$E.hide(div);
		div.id = "_DialogDiv_"+this.ID;
		div.className = "dialogdiv";
		div.setAttribute("dragStart","Dialog.dragStart");				
	 	pw.$T("body")[0].appendChild(div);
	}

	this.DialogDiv = div;
	div.innerHTML = arr.join('\n');	
	pw.$("_DialogFrame_"+this.ID).DialogInstance = this;
	this.OKButton = pw.$("_ButtonOK_"+this.ID);
	this.CancelButton = pw.$("_ButtonCancel_"+this.ID);
	//显示标题图片
	if(this.ShowMessageRow){
		$E.show(pw.$("_MessageRow_"+this.ID));
		if(this.MessageTitle){
			pw.$("_MessageTitle_"+this.ID).innerHTML = this.MessageTitle;
		}
		if(this.Message){
			pw.$("_Message_"+this.ID).innerHTML = this.Message;
		}
	}
	//显示按钮栏
	if(!this.ShowButtonRow){
		pw.$("_ButtonRow_"+this.ID).hide();
	}
	if(this.OKEvent){
		this.OKButton.onclick = this.OKEvent;
	}
	if(this.CancelEvent){
		this.CancelButton.onclick = this.CancelEvent;
	}
	if(!this.AlertFlag){
		$E.show(bgdiv);
	}else{
		bgdiv = pw.$("_AlertBGDiv");
		if(!bgdiv){
			bgdiv = pw.document.createElement("div");	
			bgdiv.id = "_AlertBGDiv";
			$E.hide(bgdiv);
		 	pw.$T("body")[0].appendChild(bgdiv);
			bgdiv.style.cssText = "background-color:#333;position:absolute;left:0px;top:0px;opacity:0.4;filter:alpha(opacity=40);width:" + cw + "px;height:" + document.body.clientHeight + "px;z-index:991";
		}
	}
	this.DialogDiv.style.cssText = "position:absolute; display:block;z-index:"+(this.AlertFlag?992:990)+";left:"+this.Left+"px;top:"+this.Top+"px";
	//判断当前窗口是否是对话框，如果是，则将其置在bgdiv之后
	if(!this.AlertFlag){
		var win = window;
		var flag = false;
		while(win!=win.parent){//需要考虑父窗口是弹出窗口中的一个iframe的情况
			if(win._DialogInstance){
				win._DialogInstance.DialogDiv.style.zIndex = 959;
				flag = true;
				break;
			}
			win = win.parent;
		}
		if(!flag){
			bgdiv.style.cssText = "background-color:#333;position:absolute;left:0px;top:0px;opacity:0.4;filter:alpha(opacity=40);width:" + cw + "px;height:" + document.body.clientHeight + "px;z-index:960";
		}
	}
	//放入队列中，以便于ESC时正确关闭
	pw.Dialog._Array.push(this.ID);
}
Dialog.hideAllFlash = function(win){//非IE浏览器在对话框弹出时必须手工隐藏flash
	if(!win||!win.$T){//有可能是Dialog.alert()
		return;
	}
	var swfs = win.$T("embed");
	for(var i=0;i<swfs.length;i++){
		try{
			swfs[i].OldStyle = swfs[i].style.display;
			swfs[i].style.display = 'none';
		}catch(ex){}
	}
	var fs = win.$T("iframe");
	for(var i=0;fs&&i<fs.length;i++){
		Dialog.hideAllFlash(fs[i].contentWindow);
	}
}
Dialog.showAllFlash = function(win){
	if(!win||!win.$T){
		return;
	}
	var swfs = win.$T("embed");
	for(var i=0;i<swfs.length;i++){
		try{
			swfs[i].style.display = swfs[i].OldStyle;
		}catch(ex){}
	}
	var fs = win.$T("iframe");
	for(var i=0;fs&&i<fs.length;i++){
		Dialog.hideAllFlash(fs[i].contentWindow);
	}
}
Dialog.prototype.addParam = function(paramName,paramValue){
		this.DialogArguments[paramName] = paramValue;
}
Dialog.prototype.close = function(){
	if(this.WindowFlag){
		this.ParentWindow.$D = null;
		this.ParentWindow.$DW = null;
		this.Window.opener = null;
		this.Window.close();
		this.Window = null;
	}else{
		//如果上级窗口是对话框，则将其置于bgdiv前		
		var pw = $E.getTopLevelWindow();
		var win = window;
		var flag = false;
		while(win!=win.parent){
			if(win._DialogInstance){
				flag = true;
				win._DialogInstance.DialogDiv.style.zIndex = 960;
				break;
			}
			win = win.parent;
		}
		if(this.AlertFlag){
			$E.hide(pw.$("_AlertBGDiv"));
		}
		if(!flag&&!this.AlertFlag){//此处是为处理弹出窗口被关闭后iframe立即被重定向时背景层不消失的问题
			pw.eval('window._OpacityFunc = function(){var w = $E.getTopLevelWindow();$E.hide(w.$("_DialogBGDiv"));}');
			pw._OpacityFunc();
		}
		this.DialogDiv.outerHTML = "";
		pw.Dialog._Array.remove(this.ID);
	}
}
Dialog.prototype.addButton = function(id,txt,func){
	var html = "<input id='_Button_"+this.ID+"_"+id+"' type='button' value='"+txt+"'> ";
	var pw = $E.getTopLevelWindow();
	pw.$("_DialogButtons_"+this.ID).$T("input")[0].getParent("a").insertAdjacentHTML("beforeBegin",html);
	pw.$("_Button_"+this.ID+"_"+id).onclick = func;
}
Dialog.close = function(evt){
	window.Args._DialogInstance.close();
}
Dialog.getInstance = function(id){
	var pw = $E.getTopLevelWindow()
	var f = pw.$("_DialogFrame_"+id);
	if(!f){
		return null;
	}
	return f.DialogInstance;
}
Dialog.AlertNo = 0;
Dialog.alert = function(msg,func,w,h){
	var pw = $E.getTopLevelWindow()
	var diag = new Dialog("_DialogAlert"+Dialog.AlertNo++);
	diag.ParentWindow = pw;
	diag.Width = w?w:300;
	diag.Height = h?h:120;
	diag.Title = "系统提示";
	diag.URL = "javascript:void(0);";
	diag.AlertFlag = true;
	diag.CancelEvent = function(){
		diag.close();
		if(func){
			func();
		}
	};
	diag.show();
	pw.$("_AlertBGDiv").show();
	$E.hide(pw.$("_ButtonOK_"+diag.ID));
	var win = pw.$("_DialogFrame_"+diag.ID).contentWindow;
	var doc = win.document;
	doc.open();
	doc.write("<body oncontextmenu='return false;'></body>") ;
	var arr = [];
	arr.push("<table height='100%' border='0' align='center' cellpadding='10' cellspacing='0'>");
	arr.push("<tr><td align='right'><img id='Icon' src='"+CONTEXTPATH+"icon_alert.gif' width='34' height='34' align='absmiddle'></td>");
	arr.push("<td align='left' id='Message' style='font-size:9pt'>"+msg+"</td></tr></table>");
	var div = doc.createElement("div");
	div.innerHTML = arr.join('');
	doc.body.appendChild(div);
	doc.close();
	var h = Math.max(doc.documentElement.scrollHeight, doc.body.scrollHeight);
	var w = Math.max(doc.documentElement.scrollWidth, doc.body.scrollWidth);
	if(w>300){
		win.frameElement.width = w;
	}
	if(h>120){
		win.frameElement.height = h;
	}
	
	diag.CancelButton.value = "确 定";
	pw.$("_DialogButtons_"+diag.ID).style.textAlign = "center";
}

Dialog.confirm = function(msg,func1,func2,w,h){
	var pw = $E.getTopLevelWindow()
	var diag = new Dialog("_DialogAlert"+Dialog.AlertNo++);
	diag.Width = w?w:300;
	diag.Height = h?h:120;
	diag.Title = "信息确认";
	diag.URL = "javascript:void(0);";
	diag.AlertFlag = true;
	diag.CancelEvent = function(){
		if(func2){
			func2();
		}
		diag.close();
	};
	diag.OKEvent = function(){
		if(func1){
			func1();
		}
		diag.close();
	};
	diag.show();
	pw.$("_AlertBGDiv").show();
	var win = pw.$("_DialogFrame_"+diag.ID).contentWindow;
	var doc = win.document;
	doc.open();
	doc.write("<body oncontextmenu='return false;'></body>") ;
	var arr = [];
	arr.push("<table height='100%' border='0' align='center' cellpadding='10' cellspacing='0'>");
	arr.push("<tr><td align='right'><img id='Icon' src='"+CONTEXTPATH+"icon_query.gif' width='34' height='34' align='absmiddle'></td>");
	arr.push("<td align='left' id='Message' style='font-size:9pt'>"+msg+"</td></tr></table>");
	var div = doc.createElement("div");
	div.innerHTML = arr.join('');
	doc.body.appendChild(div);
	doc.close();
	pw.$("_DialogButtons_"+diag.ID).style.textAlign = "center";
}

Dialog.confirm2 = function (msg, func1, func2, okText,noText, w, h) {
    var pw = $E.getTopLevelWindow()
    var diag = new Dialog("_DialogAlert" + Dialog.AlertNo++);
    diag.Width = w ? w : 300;
    diag.Height = h ? h : 120;
    diag.Title = "选择操作";
    diag.URL = "javascript:void(0);";
    diag.AlertFlag = true;
    diag.CancelEvent = function () {
        if (func2) {
            func2();
        }
        diag.close();
    };
    diag.OKEvent = function () {
        if (func1) {
            func1();
        }
        diag.close();
    };
    diag.show();
    pw.$("_AlertBGDiv").show();
    var win = pw.$("_DialogFrame_" + diag.ID).contentWindow;
    var doc = win.document;
    doc.open();
    doc.write("<body oncontextmenu='return false;'></body>");
    var arr = [];
    arr.push("<table height='100%' border='0' align='center' cellpadding='10' cellspacing='0'>");
    arr.push("<tr><td align='right'><img id='Icon' src='" + CONTEXTPATH + "icon_query.gif' width='34' height='34' align='absmiddle'></td>");
    arr.push("<td align='left' id='Message' style='font-size:9pt'>" + msg + "</td></tr></table>");
    var div = doc.createElement("div");
    div.innerHTML = arr.join('');
    doc.body.appendChild(div);
    doc.close();
    diag.OKButton.value = okText;
    diag.CancelButton.value = noText;
    pw.$("_DialogButtons_" + diag.ID).style.textAlign = "center";
}

var _DialogInstance = window.frameElement?window.frameElement.DialogInstance:null;
var Page={};
Page.onDialogLoad = function(){
	if(_DialogInstance){
		if(_DialogInstance.Title){
			document.title = _DialogInstance.Title;
		}
		window.Args = _DialogInstance.DialogArguments;
		_DialogInstance.Window = window;
		window.Parent = _DialogInstance.ParentWindow;
	}
}
Page.onDialogLoad();
PageOnLoad=function (){
	var d = _DialogInstance;
	if(d){
		try{
			d.ParentWindow.$D = d;
			d.ParentWindow.$DW = d.Window;
			var flag = false;
			if(!this.AlertFlag){
				var win = d.ParentWindow;
				while(win!=win.parent){
					if(win._DialogInstance){
						flag = true;
						break;
					}
					win = win.parent;
				}
				if(!flag){
					$E.getTopLevelWindow().$("_DialogBGDiv").style.opacity="0";
					$E.getTopLevelWindow().$("_DialogBGDiv").style.filter="alpha(opacity=0)";
				}
			}
			if(d.AlertFlag){
				$E.show($E.getTopLevelWindow().$("_AlertBGDiv"));
			}
			if(d.ShowButtonRow&&$E.visible(d.CancelButton)){
				d.CancelButton.focus();
			}
			if(d.onLoad){
				d.onLoad();
			}
		}catch(ex){alert("DialogOnLoad:"+ex.message+"\t("+ex.fileName+" "+ex.lineNumber+")");}
	}
}
Dialog.onKeyUp = function(event){
	if(event.keyCode==27){
		var pw = $E.getTopLevelWindow();
		if(pw.Dialog._Array.length>0){
			//Page.mousedown();
			//Page.click();
			var diag = pw.Dialog.getInstance(pw.Dialog._Array[pw.Dialog._Array.length-1]);
			diag.CancelButton.onclick.apply(diag.CancelButton,[]);
		}
	}	
};

//按ESC关闭对话框
if(isIE){
	document.attachEvent("onkeyup",Dialog.onKeyUp);
	window.attachEvent("onload",PageOnLoad);
	
}else{
	document.addEventListener("keyup",Dialog.onKeyUp,false);
	window.addEventListener("load",PageOnLoad,false);
}

function Show(width,height,url,Title,IsImage)
{
    var w = width;
    var h = height;
    
    var diag = new Dialog("Dialog1");
    diag.Width = width;
    diag.Height = height;
    diag.Title = Title;
    diag.URL = url;
    diag.ShowButtonRow = false;
    if(IsImage==1)
    {
        diag.IsDiv = true;
        if(url.toLowerCase().indexOf(".swf")>0)diag.DivHtml = GetFlash(url,width,height,"Dialog1");
        else diag.DivHtml = GetPic(url,width,height,"Dialog1");
    }
    diag.show()
}

function Show2(width, height, url, Title, IsImage) {
    var w = width;
    var h = height;
    if (w > 900 || h > 480) {
        if (h > 480) {
            w = w * (480 / h);
            h = 480;
        }
        if (w > 900) {
            h = h * (900 / w);
            w = 900;
        }
        width = w;
        height = h;
        width = parseInt(width);
        height = parseInt(height);
    }

    var diag = new Dialog("Dialog1");
    diag.Width = width;
    diag.Height = height;
    diag.Title = Title;
    diag.URL = url;
    diag.ShowButtonRow = false;
    if (IsImage == 1) {
        diag.IsDiv = true;
        if (url.toLowerCase().indexOf(".swf") > 0) diag.DivHtml = GetFlash(url, width, height, "Dialog1");
        else diag.DivHtml = GetPic(url, width, height, "Dialog1");
    }
    diag.show()
}

function GetPic(url, width, height,DivID)
{
    url = url.toLowerCase();
    var html = "<div id='_DialogFrame_"+DivID+"' allowTransparency='true'   style=\"background-color: #transparent; border:none;\">";
    if (url.indexOf(".") >= 0)
    {
        if (url.indexOf(".swf") < 0)
        {
            html += "<a href='" + url + "' target='_blank'><img src='" + url + "' width='"+width+"' height='"+height+"' border='0'/></a>";
        }
        else html = GetFlash(url, width, height);
    }
    html += "</div>";
    return html;
}
function GetFlash(Url, Width, Height,DivID)
{
    if (Width <= 0 || Width > 900) Width = 900;
    if (Height <= 0) Height = 100;
    var result = "<div id='_DialogFrame_"+DivID+"' allowTransparency='true' style=\"width:"+Width+"px;height:"+Height+"px;background-color: #transparent; border:none;\">";
    result += "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0\"  width=\"" + Width + "\" height=\"" + Height + "\">\r\n";
    result += "        <param name=\"movie\" value=\"" + Url + "\">\r\n";
    result += "        <param name=\"quality\" value=\"high\">\r\n";
    result += "        <embed src=\"" + Url + "\" quality=\"high\" pluginspage=\"http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash\" type=\"application/x-shockwave-flash\"  width=\"" + Width + "\" height=\"" + Height + "\"></embed>\r\n";
    result += "</object>\r\n";
    result += "</div>";
    return result;
}
