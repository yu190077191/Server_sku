function changeMenuActive(oid,option){
	if(option===undefined){
		
	 if (oid.indexOf("p")>=0) addClass(document.getElementById(oid),"hoverdown");
	 else addClass(document.getElementById(oid),"active");
	 


	}else{
		addClass(document.getElementById(oid),document.getElementById(oid).getAttribute("activeClass"))
	}
	Event.addEvent(window,"load",function(){changeMenuActive(oid)})
}
function listerenChannel(_id,_option){
		Event.addEvent(window,"load",function(){changeMenuActive(_id,_option)})
}
// JavaScript Document

function MM_jumpMenu(targ,selObj,restore){ //v3.0
  eval(targ+".location='"+selObj.options[selObj.selectedIndex].value+"'");
  if (restore) selObj.selectedIndex=0;
}
//login trun input's BG
function clearInputBg(it){

	it.className="";
}
function verInputBg(it){
	
		if(it.value==""){
				it.className="default";	
		}
}
function getEventObj(evt){
	evt = evt || window.event;
	return evt.target || evt.srcElement;
}
//hack ff childnodes
if(!document.all){    
        
        HTMLElement.prototype.__defineGetter__("children",     
             function () {     
                 var returnValue = new Object();     
                 var number = 0;     
                 for (var i=0; i<this.childNodes.length; i++) {     
                     if (this.childNodes[i].nodeType == 1) {     
                         returnValue[number] = this.childNodes[i];     
                         number++;     
                     }     
                 }     
                 returnValue.length = number;     
                 return returnValue;     
             }     
         );    
        
    }


//返回标添对象,参数className必须唯一
function c$(cn,tag){
		if(tag){
			var _o = document.getElementsByTagName(tag);
		}else{
			var _o = document.getElementsByTagName("*");
		}
		for(i=0;i<_o.length;i++){
			if(_o[i].className==cn){
				return _o[i]
			}
			
		}
		return null;	
}


//追加样式
function addClass(element,value) { 
    if (element == null) return;
          if (!element.className) {
            element.className = value;
          } else {
			var _cls = element.className.split(" ")
			for(var i=0;i<_cls.length;i++){
				if(_cls[i]==value){//已经存在
				
					return false;
				}
			}
            element.className+= " " + value;
         //   element.className+= value;
          }
 }
 //移出样式
function removeClass(element, value) {
    if (element == null) return;
          if (!element.className) {
            return false
          } else {
			var _n;
            var _cls =element.className.split(" ")
			if(_cls.length==1){
				if(element.className == value){
					element.className = ""
					return true;
				}
			}
			for(var i=0;i<_cls.length;i++){
				if(_cls[i]==value){
					_n =i;
					break;
				}
			}
			_cls.splice(_n,1)

			element.className = _cls.join(" ")
            
          }
 }
//创建Event对象,addEvent ,removeEvent
var Event = {
	addEvent: function(obj, evType, fn){
		if (obj.addEventListener) {
			obj.addEventListener(evType, fn, false);
			return true;
		}
		else {
			if (obj.attachEvent) {
				var r = obj.attachEvent("on" + evType, fn);
				EventCache.add(obj, evType, fn);
				return r;
			}
			else {
				return false;
			}
		}
	},
	removeEvent: function(obj, evType, fn){
		if (obj.removeEventListener) {
			obj.removeEventListener(evType, fn, false);
			return true;
		}
		else {
			if (obj.detachEvent) {
				var r = obj.detachEvent("on" + evType, fn);
				return r;
			}
			else {
				return false;
			}
		}
	},
	getEvent: function(e){
		e = window.event || e;
		e.leftButton = false;
		if (e.srcElement == null && e.target != null) {
			e.srcElement = e.target;
			e.leftButton = (e.button == 1);
		}
		else {
			if (e.target == null && e.srcElement != null) {
				e.target = e.srcElement;
				e.leftButton = (e.button == 0);
			}
			else {
				if (e.srcElement != null && e.target != null) {
				}
				else {
					return null
				}
			}
		}
		if (document.body && document.documentElement) {
			e.mouseX = e.pageX || (e.clientX + Math.max(document.body.scrollLeft, document.documentElement.scrollLeft));
			e.mouseY = e.pageY || (e.clientY + Math.max(document.body.scrollTop, document.documentElement.scrollTop));
		}
		else {
			e.mouseX = -1;
			e.mouseY = -1;
		}
		return e;
	},
	stopEvent: function(e){
		if (e && e.cancelBubble != null) {
			e.cancelBubble = true;
			e.returnValue = false;
		}
		if (e && e.stopPropagation && e.preventDefault) {
			e.stopPropagation();
			e.preventDefault();
		}
		return false;
	}
};
var EventCache=function(){
	var listEvents = [];
	return {
		listEvents: listEvents,
		add: function(node, sEventName, fHandler, bCapture){
			listEvents[listEvents.length] = arguments;
		},
		flush: function(){
			var i;
			var item;
			for (i = listEvents.length - 1; i >= 0; i = i - 1) {
				item = listEvents[i];
				if (item[0].removeEventListener) {
					item[0].removeEventListener(item[1], item[2], item[3]);
				};
				if (item[1].substring(0, 2) != "on") {
					item[1] = "on" + item[1];
				};
				if (item[0].detachEvent) {
					item[0].detachEvent(item[1], item[2]);
				};
				item[0][item[1]] = null;
			};
		}
	};
}();

Event.addEvent(window,"unload",EventCache.flush);

function error_handler(a, b, c){
	window.status = (c + "\n" + b + "\n\n" + a + "\n\n" + error_handler.caller);
	return true;
}

//target:[targetObject,targetClassName]
//other:[otherObject,otherClassName]
function tabChannel(it){
	var _layers;
	function returnClassName(_prev){//反转oldClassName,class

				var _temcn = _prev.className
				_prev.className = _prev.getAttribute("oldClassName")
				_prev.setAttribute("oldClassName",_temcn)
	}
	var _l = it.parentNode.getAttribute("targetLayer")
	var _ls = _l.split(",")
	if(!document.getElementById(_ls[0])){alert("layer未找到");return false}
	if(it.getAttribute("open")=='true'){
		return;
	}
	else{
		var _menus = it.parentNode.children;

		for(var i=0;i<_menus.length;i++){
			if(_menus[i].getAttribute("open")=='true'){
				returnClassName(_menus[i])
			}
			_menus[i].setAttribute("open","false")
			if(it == _menus[i]){
				returnClassName(_menus[i])
				_menus[i].setAttribute("open","true")
				//
				for(var ii=0;ii<_ls.length;ii++){
					_layers = document.getElementById(_ls[ii]).children;
				//

					for(var i2=0;i2<_layers.length;i2++){
						if(i==i2){
							removeClass(_layers[i2],"hide");
						}
						else{
							addClass(_layers[i2],"hide");
						}
					}
				}
			}

		}
	}
}


//cn:className
//it:event.srcELement
function parentChannel(cn,it){
	var _l = it.parentNode.children;
	for(var i=0;i<_l.length;i++){
		var _d = _l[i].getAttribute("targetLayer");

		if(document.getElementById(_d)){
			document.getElementById(_d).style.display="none";
		}else{
			alert(_l[i].getAttribute("targetLayer")+"没找到"); return false
		}
	}
	
	document.getElementById(it.getAttribute("targetLayer")).style.display="block"
	it.parentNode.className = cn;
	
	//it.getAttribute("targetLayer")
}
//change fontSize
// c为大小级别参数
function changeFontSize(objId,c){
	if(!document.getElementById(objId))return false;
	var _fontSize,_lineHeight;
	with(document.getElementById(objId)){
		if(c==0){
			_fontSize="14px"
			_lineHeight="24px"
		}else{
	
			_fontSize=c*2+parseInt(currentStyle.fontSize)+"px"
			_lineHeight=c*2+parseInt(currentStyle.lineHeight)+"px"
		}
		style.fontSize=_fontSize;
		style.lineHeight=_lineHeight;
	}
		Cookies.set("zggXmContentFontSize",_fontSize)
		Cookies.set("zggXmContentLineHeight",_lineHeight)
	
}
//初始化cookies中的明细字体大小
function initXmContentFontSizeFromCookies(){

	if(!document.getElementById("xmContent"))return false;
	var _cookies
	var _fontSize = Cookies.get("zggXmContentFontSize");
	var _lineHeight = Cookies.get("zggXmContentLineHeight")

		with(document.getElementById("xmContent")){
			style.fontSize=_fontSize;
			style.lineHeight=_lineHeight;			
		}

}
Event.addEvent(window,"load",initXmContentFontSizeFromCookies);

//write,read cookies
var Cookies = {};
/**//**
 * 设置Cookies
 */
Cookies.set = function(name, value){
     var argv = arguments;
     var argc = arguments.length;
     var expires = (argc > 2) ? argv[2] : null;
     var path = (argc > 3) ? argv[3] : '/';
     var domain = (argc > 4) ? argv[4] : null;
     var secure = (argc > 5) ? argv[5] : false;
     document.cookie = name + "=" + escape (value) +
       ((expires == null) ? "" : ("; expires=" + expires.toGMTString())) +
       ((path == null) ? "" : ("; path=" + path)) +
       ((domain == null) ? "" : ("; domain=" + domain)) +
       ((secure == true) ? "; secure" : "");
};
/**//**
 * 读取Cookies
 */
Cookies.get = function(name){
    var arg = name + "=";
    var alen = arg.length;
    var clen = document.cookie.length;
    var i = 0;
    var j = 0;
    while(i < clen){
        j = i + alen;
        if (document.cookie.substring(i, j) == arg)
            return Cookies.getCookieVal(j);
        i = document.cookie.indexOf(" ", i) + 1;
        if(i == 0)
            break;
    }
    return null;
};
/**//**
 * 清除Cookies
 */
Cookies.clear = function(name) {
  if(Cookies.get(name)){
    var expdate = new Date(); 
    expdate.setTime(expdate.getTime() - (86400 * 1000 * 1)); 
    Cookies.set(name, "", expdate); 
  }
};

Cookies.getCookieVal = function(offset){
   var endstr = document.cookie.indexOf(";", offset);
   if(endstr == -1){
       endstr = document.cookie.length;
   }
   return unescape(document.cookie.substring(offset, endstr));
};