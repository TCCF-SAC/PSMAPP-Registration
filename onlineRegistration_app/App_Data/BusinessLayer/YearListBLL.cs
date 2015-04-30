using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace onlineRegistration_app.BusinessLayer
{
   public class YearListBLL
   {
      private int yearPadding = 2;

      public YearListBLL() { }

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