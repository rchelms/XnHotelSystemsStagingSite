var flagIsCheckinDateSelected = false;
var selectedCheckin = new Date();
var selectedCheckout = new Date();

var const_class_day = "mm_avail_cal_day";
var const_available = "mm_available";
var const_passed = "mm_passed_day";
var const_not_available = "mm_not_available";
var const_class_checkin = "mama_avail_cal_day_checkin";
var const_class_checkout = "mama_avail_cal_day_checkout";
var const_class_na_checkout = "mm_not_avail_cal_day_checkout";
var const_class_selecting = "mama_avail_cal_day_selecting";
var const_class_hover_selecting = "mama_avail_cal_day_selecting-over";

var controlId_dayDate = "hdfAvailCalDayDate";
var controlId_selectedCheckinDate = "hdfSelectedCheckinDate";
var constrolId_selectedCheckoutDate = "hdfSelectedCheckoutDate";

var isTouch = false;

InitGlobalVariables = function() {
   flagIsCheckinDateSelected = false;
   selectedCheckin = null;
   selectedCheckout = null;
   isTouch = false;
};

InitCalendar = function () {
   if (selectedCheckin) {
      $("." + const_available).each(function () {
         var curDate = new Date(parseInt($(this).children("input[id$='hdfAvailCalDayDate']").val().substr(8)));

         if (curDate.format("MM/dd/yyyy") == selectedCheckin.format("MM/dd/yyyy")) {
            availcal_selected(this);
            return false;
         }
      });
   }
};

availcal_touch = function(availcalday) {
   isTouch = true;
   availcal_selected(availcalday);
};

availcal_click = function (availcalday) {
   if (isTouch != true)
      availcal_selected(availcalday);
},

availcal_selected = function (availcalday) {
   // Reset checkin date if user click on checkin date again.
   if (flagIsCheckinDateSelected && $(availcalday).hasClass(const_class_checkin)) {
      changeMode(false);
      $(availcalday).removeClass(const_class_checkin);
      return;
   }


   //   else if (flagIsCheckinDateSelected && (new Date(parseInt($(availcalday).children("input[id$='hdfAvailCalDayDate']").val().substr(8)))) < selectedCheckin)
   //      return;

   // Process selected date for postback to server for touch
   if (flagIsCheckinDateSelected && isTouch) {
      var endDate = new Date(parseInt($(availcalday).children("input[id$='hdfAvailCalDayDate']").val().substr(8)));
      var flagIsNABetweenCheckinAndEOM = false;
      $("." + const_class_day).each(function () {
         if ($(this).hasClass(const_not_available)) {
            var currentNotVailableDate = new Date(parseInt($(this).children("input[id$='" + controlId_dayDate + "']").val().substr(8)));
            if (currentNotVailableDate > selectedCheckin && currentNotVailableDate < endDate) {
               flagIsNABetweenCheckinAndEOM = true;
               return false;
            }
         }
      });


      if (!flagIsNABetweenCheckinAndEOM && endDate > selectedCheckin) {
         $(availcalday).parents("div[id$='panAvailCal']").children("input[id$='hdfSelectedCheckinDate']").val(toInternalDateTimeString(selectedCheckin));
         $(availcalday).parents("div[id$='panAvailCal']").children("input[id$='hdfSelectedCheckoutDate']").val(toInternalDateTimeString(endDate));
         $(".mm_avail_cal_submit_button").click();
         return;
      } else {
         return;
      }
   }

   // Process selected date for postback to server for click
   else if (flagIsCheckinDateSelected && ($(availcalday).hasClass(const_class_checkout) || $(availcalday).hasClass(const_class_na_checkout))) {
      selectedCheckout = new Date(parseInt($(availcalday).children("input[id$='hdfAvailCalDayDate']").val().substr(8)));
      $(availcalday).parents("div[id$='panAvailCal']").children("input[id$='hdfSelectedCheckinDate']").val(toInternalDateTimeString(selectedCheckin));
      $(availcalday).parents("div[id$='panAvailCal']").children("input[id$='hdfSelectedCheckoutDate']").val(toInternalDateTimeString(selectedCheckout));
      $(".mm_avail_cal_submit_button").click();
      return;
   }

   // Do nothing when user click a no_available day
   if ($(availcalday).hasClass(const_not_available) || $(availcalday).hasClass(const_passed))
      return;

   // Set Checkin date and switch to selecting mode
   // Remove any current checkin/selecting/checkout
   $("." + const_class_checkin).removeClass(const_class_checkin);
   $("." + const_class_selecting).removeClass(const_class_selecting);
   $("." + const_class_checkout).removeClass(const_class_checkout);

   $(availcalday).addClass(const_class_checkin);
   selectedCheckin = new Date(parseInt($(availcalday).children("input[id$='hdfAvailCalDayDate']").val().substr(8))); // convert serialized date object to date object

   changeMode(true);
};
changeMode = function (isSelectingMode) {
   flagIsCheckinDateSelected = isSelectingMode;
   checkin_touch = isSelectingMode;
   if (isSelectingMode) {
      $("span[id$='lblAvailCalInstructionMessage']").text(__MamaShelterResources.CheckOutstructionMessage);
   }
   else {
      $("span[id$='lblAvailCalInstructionMessage']").text(__MamaShelterResources.CheckInInstructionMessage);
   }
};
availcal_mouseover = function (availcalday) {
    // Mouse over a day that is not in selecting mode
    if (!flagIsCheckinDateSelected) {

        if ($(availcalday).hasClass(const_available)) {

            $(availcalday).addClass(const_class_hover_selecting);
            $(availcalday).mouseout(function () {
                $(this).removeClass(const_class_hover_selecting);
                $(this).unbind("mouseout");
            });
        }
        else if ($(availcalday).hasClass(const_not_available)) {
            $(availcalday).addClass("mm_not_available_hover");
            $(availcalday).mouseout(function () {
                $(this).removeClass("mm_not_available_hover");
                $(this).unbind("mouseout");
            });
        }
    }
    else {
        var endDate = new Date(parseInt($(availcalday).children("input[id$='hdfAvailCalDayDate']").val().substr(8)));
        var startDate = selectedCheckin;

        // flag if there is any not available day between selected checkin date and end of month
        var flagIsNABetweenCheckinAndEOM = false;
        var NAFromCheckin;
        flagIsNABetweenCheckinAndEOM = false;
        $("." + const_class_day).each(function () {

           if ($(this).hasClass(const_not_available) || $(this).hasClass(const_passed)) {
                var currentNotVailableDate = new Date(parseInt($(this).children("input[id$='" + controlId_dayDate + "']").val().substr(8)));
                if (currentNotVailableDate > selectedCheckin) {
                    flagIsNABetweenCheckinAndEOM = true;
                    NAFromCheckin = currentNotVailableDate;
                    return false;
                }
            }
        });


        $("." + const_class_day).each(function () {
            var curDate = new Date(parseInt($(this).children("input[id$='hdfAvailCalDayDate']").val().substr(8)));

            if (curDate > startDate && curDate < endDate && (!flagIsNABetweenCheckinAndEOM || curDate < NAFromCheckin)) {
                $(this).addClass(const_class_selecting);
                $(this).removeClass(const_class_checkout);
            }
            else if ((curDate > startDate && curDate <= endDate && (!flagIsNABetweenCheckinAndEOM || curDate < NAFromCheckin))
                || (curDate > startDate && curDate <= endDate && (!flagIsNABetweenCheckinAndEOM || curDate <= NAFromCheckin))) {
                if ($(this).hasClass(const_not_available))
                    $(this).addClass(const_class_na_checkout);
                else
                    $(this).addClass(const_class_checkout);

                $(this).removeClass(const_class_selecting);
            }
            else {
                $(this).removeClass(const_class_selecting);
                $(this).removeClass(const_class_checkout);
                $(this).removeClass(const_class_na_checkout);
            }
        });


    }
};
reassignInstructionTextResource = function () {
    if (flagIsCheckinDateSelected) {
        $("span[id$='lblAvailCalInstructionMessage']").text(__MamaShelterResources.CheckOutstructionMessage);
    }
    else {
        $("span[id$='lblAvailCalInstructionMessage']").text(__MamaShelterResources.CheckInInstructionMessage);
    }
};
toInternalDateTimeString = function (date) {
    var internalShortDateString = "";
    internalShortDateString += date.getFullYear();
    internalShortDateString += "-";
    internalShortDateString += date.getMonth() + 1;
    internalShortDateString += "-";
    internalShortDateString += date.getDate();

    return internalShortDateString;
};


