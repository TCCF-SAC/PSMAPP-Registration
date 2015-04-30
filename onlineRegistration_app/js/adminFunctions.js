$(document).ready(function () {
   pageInit();
});

var dlgCustomTitle = null;

function pageInit()
{
   $("[placeholder]").placeholder();

   $("div[id$=pnlError]").effect("highlight", {color: '#cc0000'}, 1000);

   // Hover event for Omit Date help link
   $(".custom-title-help").hover(
		function() { $(this).addClass('ui-state-hover'); }, 
		function() { $(this).removeClass('ui-state-hover'); }
	);

   // Setup Omit Date help dialog
   dlgCustomTitle = $("#dialog-custom-title");
   dlgCustomTitle.dialog({
         autoOpen: false,
         draggable: true,
         modal: true,
         position: ['center', 100],
         resizable: false,
         width: 450,
         buttons:
            {
               OK: function() { $(this).dialog("close"); }
            }
   });

   $("a[id$=lnkDelete]").click(function(event) {
      event.preventDefault();

      var lnkDelete = $(this);
      var dlgDelete = "#dialog-delete-confirm";

      var position = lnkDelete.position();

      $(dlgDelete).dialog({
         draggable: true,
         modal: true,
         position: ['center', 100], //[position.left,position.top],
         resizable: false,
         buttons:
            {
               "Confirm Delete": function() {
                     $(this).dialog("close");
                     __doPostBack($(lnkDelete).attr("id").replace(/_/gi, "$"), '');

                     return true;
                  },
               Cancel: function() { $(this).dialog("close"); return false; }
            },
         open: function(event, ui) { $(this).parents().find('.ui-dialog-buttonset button:eq(1)').focus(); },
         close: function(event, ui) { return false; }
      });

   });

   // Set onchange event for any date inputs or minus icon clicks. This is to update the skip date array field.
   var addFunc = function(){ addExtraDate(null, $(this).closest('td')); }
   var changeFunc = function(){ setExtraDateArray($(this).closest('td')); };
   var removeFunc = function(){
      if(!confirm("Remove this date?")) return;
      var td = $(this).closest('td');
      $(this).parent().remove();
      setExtraDateArray(td);
   };
   $("a.addExtraDate").live("click", addFunc);
   $("input.ExtraDate").live("change", changeFunc);
   $("a.deleteExtraDate").live("click", removeFunc);
}

function addExtraDateArray(strArray, containerID)
{
   if(strArray == '') return;
   var arrDates = strArray.split(';');

   for(x in arrDates)
       addExtraDate(arrDates[x], $('#' + containerID));
}

function addExtraDate(strDate, container)
{
   var newDiv = $("<div></div>");
   var newLink = $("<a href='javascript:void(0);' title='Remove date'" +
                   "class='icon-skip-date ui-state-default ui-corner-all deleteExtraDate'></a>");
   // Add span icon to the new link.
   newLink.append("<span class='ui-icon ui-icon-minusthick'></span>");
   newDiv.append(newLink);
   // Add date input field
   var newInput = $("<input type=\"text\" placeholder=\"Date\" class=\"defaultInput admin date-picker ExtraDate\" " +
                        "readonly=\"readonly\" maxlength=\"10\">");
   // Add input value if it's not blank
   if(strDate != null) newInput.val(strDate);
   newDiv.append(newInput);

   container.append(newDiv);

   setDateRangeByMonth(globalMonthDate);
}

function setExtraDateArray(container)
{
   var hidField = $("input:hidden", container);
   var extraDates = $("input:text", container);

   var arrDates = "";
   // Only process if the user is entering more than one skip date
   if(extraDates.length > 1)
   {
      extraDates.each(function(i) {
         var value = $(this).val();
         if(value == '') return;

         var hasDates = (arrDates != '');
         arrDates += (hasDates ? ";" : "") + value;
      });
   }

   hidField.val(arrDates);
}

function help_custom_title()
{
   if(dlgCustomTitle == null) return;

   dlgCustomTitle.dialog("open");
}

function highLightSeries(strDate)
{
   var hlDate = new Date(strDate);
   var arrMonthNames = new Array("January", "February", "March", "April", "May", "June", "July",
                             "August", "September", "October", "November", "December")
   var findString = arrMonthNames[hlDate.getMonth()] + " " + hlDate.getFullYear();
   $("td:contains('" + findString + "')").parent().find("td").effect("highlight", { color: '#FCD209' }, 2500);
}

function highLightClass(strID)
{
   $("[id='classInfo_" + strID + "']").find("td").effect("highlight", { color: '#FCD209' }, 2500);
}

function addDatePickerClearButton()
{
   // Change the datepicker's generate html call to add a new 'Clear' button
   $(function () {
      //wrap up the redraw function with our new button
      var dpFunc = $.datepicker._generateHTML; //record the original
      $.datepicker._generateHTML = function (inst) {
         var thishtml = $(dpFunc.call($.datepicker, inst)); //call the original

         thishtml = $('<div />').append(thishtml); //add a wrapper div for jQuery context

         var buttonPane = $('.ui-datepicker-buttonpane', thishtml);
         var btnClass = "ui-datepicker-current ui-state-default ui-priority-primary ui-corner-all clear";

         // Hide the "Today" button
         buttonPane.find(".ui-datepicker-current").css("display", "none");

         //locate the button panel and add our button - with a custom css class.
         buttonPane.append(
			      $('<button type="button" class="' + btnClass + '" \>Clear</button>'
			      ).click(function () {
			         $.datepicker._clearDate(inst.input);
			         //			         inst.input.datepicker('hide');
			      })
		      );

         thishtml = thishtml.children(); //remove the wrapper div

         return thishtml; //assume okay to return a jQuery
      };
   });
}

var globalMonthDate = null;
function setDateRangeByMonth(MonthDate)
{
   if (globalMonthDate  == null) globalMonthDate  = MonthDate;
   $(function () {
      var min = new Date(MonthDate);
      var max = new Date(MonthDate);

      // Set the max date to 3 months ahead of the min date
      max.setMonth(max.getMonth() + 3);
      // Subtract one day from max date to push it to only 2 months ahead
      max.setDate(max.getDate() - 1);

      addDatePickerClearButton();

      $(".date-picker").datepicker({
         show: 'fade',
         dateFormat: 'mm/dd/yy',
         showButtonPanel: true,
         minDate: min,
         maxDate: max,
         defaultDate: min
      });
   });
}
