var CONTEXTPATH = rootUrl + "UploaderPlugin/Dialog/";
var thisId = "thisPopupId";

function popup(html, divId, followCursor) {
    thisId = divId;
    var div = document.getElementById("_DialogDiv_" + thisId);
    var doc = document;
    var cw = doc.compatMode == "BackCompat" ? doc.body.clientWidth : doc.documentElement.clientWidth;
    var ch = doc.compatMode == "BackCompat" ? doc.body.clientHeight : doc.documentElement.clientHeight; //必须考虑文本框处于页面边缘处，控件显示不全的问题
    var sw = Math.max(doc.documentElement.scrollLeft, doc.body.scrollLeft);
    var sh = Math.max(doc.documentElement.scrollTop, doc.body.scrollTop); //考虑滚动的情况
     
     if (div == null) {
         var arr = [];
         arr.push("<table style='-moz-user-select:none;' border='0' cellpadding='0' cellspacing='0'>");
         arr.push("  <tr style='cursor:;'>");
         arr.push("    <td width='13' height='33' style=\"background-image:url(" + CONTEXTPATH + "dialog_lt.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + CONTEXTPATH + "dialog_lt.png', sizingMethod='crop');\"><div style='width:13px;'></div></td>");
         arr.push("    <td height='33' style=\"background-image:url(" + CONTEXTPATH + "dialog_ct.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + CONTEXTPATH + "dialog_ct.png', sizingMethod='crop');\"><div style=\"float:left;font-weight:bold; color:#FFFFFF; padding:9px 0 0 4px;\"><img src=\"" + CONTEXTPATH + "icon_dialog.gif\" align=\"absmiddle\">&nbsp;</div>");
         arr.push("      <div title='close' style=\"position: relative;cursor:pointer; float:right; margin:5px 0 0; _margin:4px 0 0;height:17px; width:28px; background-image:url(" + CONTEXTPATH + "dialog_closebtn.gif)\" onMouseOver=\"this.style.backgroundImage='url(" + CONTEXTPATH + "dialog_closebtn_over.gif)'\" onMouseOut=\"this.style.backgroundImage='url(" + CONTEXTPATH + "dialog_closebtn.gif)'\" onClick=\"closeDialogDiv();\"></div></td>");
         arr.push("    <td width='13' height='33' style=\"background-image:url(" + CONTEXTPATH + "dialog_rt.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + CONTEXTPATH + "dialog_rt.png', sizingMethod='crop');\"><div style=\"width:13px;\"></div></td>");
         arr.push("  </tr>");
         arr.push("  <tr drag='false'><td width='13' style=\"background-image:url(" + CONTEXTPATH + "dialog_mlm.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + CONTEXTPATH + "dialog_mlm.png', sizingMethod='crop');\"></td>");
         arr.push("    <td align='center' valign='top'>");
         arr.push("    <table width='100%' height='100%' border='0' cellpadding='0' cellspacing='0' bgcolor='#FFFFFF'>");
         arr.push("        <tr id='_MessageRow_" + thisId + "' style='display:none'>");
         arr.push("          <td height='50' valign='top'><table id='_MessageTable_" + thisId + "' width='100%' border='0' cellspacing='0' cellpadding='8' style=\" background:#EAECE9 url(" + CONTEXTPATH + "dialog_bg.jpg) no-repeat right top;\">");
         arr.push("              <tr><td width='25' height='50' align='right'><img id='_MessageIcon_" + thisId + "' src='" + CONTEXTPATH + "window.gif' width='32' height='32'></td>");
         arr.push("                <td align='left' style='line-height:16px;'>");
         arr.push("                <h5 class='fb' id='_MessageTitle_" + thisId + "'>&nbsp;</h5>");
         arr.push("                <div id='_Message_" + thisId + "'>&nbsp;</div></td>");
         arr.push("              </tr></table></td></tr>");


         arr.push("        <tr><td align='center' valign='top' id='myDiv" + thisId + "'>");
         arr.push(html);
         arr.push("</td></tr>");

         arr.push("      </table></td>");
         arr.push("    <td width='13' style=\"background-image:url(" + CONTEXTPATH + "dialog_mrm.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + CONTEXTPATH + "dialog_mrm.png', sizingMethod='crop');\"></td></tr>");
         arr.push("  <tr><td width='13' height='13' style=\"background-image:url(" + CONTEXTPATH + "dialog_lb.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + CONTEXTPATH + "dialog_lb.png', sizingMethod='crop');\"></td>");
         arr.push("    <td style=\"background-image:url(" + CONTEXTPATH + "dialog_cb.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + CONTEXTPATH + "dialog_cb.png', sizingMethod='crop');\"></td>");
         arr.push("    <td width='13' height='13' style=\"background-image:url(" + CONTEXTPATH + "dialog_rb.png) !important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + CONTEXTPATH + "dialog_rb.png', sizingMethod='crop');\"></td>");
         arr.push("  </tr></table>");

         div = document.createElement("div");
         div.id = "_DialogDiv_" + thisId;
         div.className = "dialogdiv";
         div.style.cssText = "display:none;";
         document.body.appendChild(div);
         div.innerHTML = arr.join('\n');
     }
     else {
         $("#myDiv" + thisId).html(html);
         $("#_DialogDiv_" + thisId).css({ "display": "" });
     }

     var top = (ch - $(div).height() - 30) / 2 + sh - 8;
     var left = (cw - $(div).width() - 12) / 2 + sw;
     if (followCursor) {
         left = event.clientX;
         top = event.clientY;
     }

     div.style.cssText = "position:absolute; display:block;z-index:992;left:" + left + "px;top:" + top + "px;background:#FFF;padding:0;";
 }

 function closeMe() {
     $("#popupDiv").css({ "display": "none" });
     $("#_DialogDiv_" + thisId).css({ "display": "none" });
 }

 function closeDialogDiv() {
     $("#_DialogDiv_" + thisId).css({ "display": "none" });
 }

 function popupDiv(divId) {
     var div = document.getElementById(divId);
     var doc = document;
     var cw = doc.compatMode == "BackCompat" ? doc.body.clientWidth : doc.documentElement.clientWidth;
     var ch = doc.compatMode == "BackCompat" ? doc.body.clientHeight : doc.documentElement.clientHeight; //必须考虑文本框处于页面边缘处，控件显示不全的问题
     var sw = Math.max(doc.documentElement.scrollLeft, doc.body.scrollLeft);
     var sh = Math.max(doc.documentElement.scrollTop, doc.body.scrollTop);
     var top = (ch - $(div).height() - 30) / 2 + sh - 8;
     var left = (cw - $(div).width() - 12) / 2 + sw;
     div.style.cssText = "position:absolute; display:block;z-index:992;left:" + left + "px;top:" + top + "px;background:#FFF;";
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