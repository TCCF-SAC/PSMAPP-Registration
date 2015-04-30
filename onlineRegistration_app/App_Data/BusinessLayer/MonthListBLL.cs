using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace onlineRegistration_app.BusinessLayer
{
   public class MonthListBLL
   {
      public MonthListBLL() { }

      public ListItemCollection Select()
      {
         string[] arrMonths = {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};
         return ArrayToItemCollection(arrMonths);
      }

      public ListItemCollection SelectShortMonths()
      {
         string[] arrMonths = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
         return ArrayToItemCollection(arrMonths);
      }

      private ListItemCollection ArrayToItemCollection(string[] arr)
      {
         ListItemCollection licReturn = new ListItemCollection();

         for (int i = 0; i < arr.Length; i++)
            licReturn.Add(new ListItem(arr[i], (i + 1).ToString()));

         return licReturn;
      }
   }
}