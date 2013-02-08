var __MamaShelterResources;

createDelegate = function (obj, func) {
   return function () { func.apply(obj, arguments); };
};
showWaitingPage = function () {
   $("#waitingPage").dialog('open');
};

closeWaitingPage = function () {
   $("#waitingPage").dialog('close');
};

recalculateButtonSize = function (numberOfButton, buttonClassName) {
   var totalWidth = $(".mm_content_section").outerWidth();
   var widthWithoutBorder = totalWidth - (2 * numberOfButton);
   var newSize = parseInt(widthWithoutBorder / numberOfButton); // 2 = border left + border right
   if ($(buttonClassName).length > 0) {
      $(buttonClassName).css("width", newSize + "px");
      $($(buttonClassName)[$(buttonClassName).length - 1]).css("width", newSize + (widthWithoutBorder % numberOfButton) + "px");
   }
};

applyStyleForTouchDevice = function (cn) {
   if (navigator.userAgent.match(/iPhone/i) || navigator.userAgent.match(/iPad/i)) {
      var className = (cn.charAt(0) == "." ? cn : ("." + cn));
      $(className).css("display", "block");
   }

};

toggleDetail = function (controlId, obj) {

   var isControlHidden = $("#" + controlId).hasClass("mm_hidden");
   $(".isopening").addClass("mm_hidden");
   $(".isopening").removeClass("isopening");

   $(".isUnderlined").removeClass("mm_underline");
   $(".isUnderlined").removeClass("isUnderlined");

   if (isControlHidden) {
      $("#" + controlId).removeClass("mm_hidden");
      $("#" + controlId).addClass("isopening");
      if (obj != null) {
         $(obj).addClass("isUnderlined");
         $(obj).addClass("mm_underline");
      }
   } else {
      $("#" + controlId).addClass("mm_hidden");
      if (obj != null) {
         $(obj).removeClass("mm_underline");
      }
   }

   return false;
};

showphoto = function (controlId) {
   $("#" + controlId).click();
};

hideButtonEdit = function (controlId) {
   $("#" + controlId).css("display", "none");
};

btnEdit_click = function () {

   if (confirm(__MamaShelterResources.StepBackwardWarningMessage)) {
      showWaitingPage();
      return true;
   } else
      return false;

};

item_changed = function (obj, refObj, changeObj) {
   var price = parseInt($("#" + refObj).contents().last().text());
   var quantity = parseInt($(obj).val());
   var newPrice = price * quantity;
   $("#" + changeObj).contents().last().text(newPrice);
};

startOverPrompt = function (msg) {
   var ans = confirm(msg);
   if (ans == true) {
      location.replace("Default.aspx?fres=1");
   } else {
      location.replace("Default.aspx");
   }
};

