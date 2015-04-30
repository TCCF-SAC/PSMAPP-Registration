using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlineRegistration_app.BusinessLayer
{
   public class SeriesInfo
   {
      public static string Name { get { return "Series"; } }

      private string title;
      private bool customTitle;
      private DateTime date;
      private bool visible;
      private string xpathaddress;

      public SeriesInfo()
      {
         title = xpathaddress = "";
         date = DateTime.MinValue;
         visible = true;
      }

      public DateTime Date
      {
         get { return this.date; }
         set { this.date = value; }
      }

      public int Month
      { get { return this.date.Month; } }

      public string MonthName
      { get { return this.date.ToString("MMMM"); } }

      public string MonthAndYear
      { get { return MonthName + " " + Year.ToString(); } }

      public string DisplayTitle
      {
         get
         {
            string rTitle = Title;

            if (!CustomTitle) rTitle += " " + MonthAndYear;

            return rTitle;
         }
      }

      public bool CustomTitle
      {
         get { return this.customTitle; }
         set { this.customTitle = value; }
      }

      public string Title
      {
         get { return this.title; }
         set { this.title = value; }
      }

      public bool Visible
      {
         get { return this.visible; }
         set { this.visible = value; }
      }

      public int Year
      { get { return this.date.Year; } }

      public string XPathAddress
      {
         get { return this.xpathaddress; }
         set { this.xpathaddress = value; }
      }
   }
}