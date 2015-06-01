using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace onlineRegistration_app.BusinessLayer
{
    /// <summary>
    /// Simple function to generate a list of years based on the current year.
    /// </summary>
   public class YearListBLL
   {
       /// <summary>
       /// The padding for the year list.
       /// Padding by 2 will start the list with the current year minus 2 and
       /// end the list with the current year plus 2.
       /// </summary>
      private int yearPadding = 2;

      public YearListBLL() { }

       /// <summary>
       /// Generates a List Item Collection of years starting with the current year minus yearPadding
       /// and ending with the current year plus yearPadding.
       /// For example, if the current year is 2015, the list will start with 2013 and end with 2017.
       /// </summary>
       /// <returns></returns>
      public ListItemCollection Select()
      {
         int curYear = DateTime.Now.Year;
         ListItemCollection yearsList = new ListItemCollection();

         for (int i = 0; i < yearPadding * 2 + 1; i++)
         {
            string yearItem = (curYear + (-yearPadding + yearsList.Count)).ToString();
            yearsList.Add(new ListItem(yearItem, yearItem));
         }

         return yearsList;
      }
   }
}