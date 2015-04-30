using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace onlineRegistration_app
{
   public partial class EmailFormat : System.Web.UI.UserControl
   {
      private int counter;

      protected void Page_Load(object sender, EventArgs e)
      {
         counter = 0;
      }

      private string formatEnroll(int count) { return "%enroll_" + count.ToString() + "%"; }

      public string getEnrollClassName(int ItemIndex)
      {
         counter++;
         return (ItemIndex % 2 == 0 ? "" : "alt") + formatEnroll(counter);
      }

      public string getEnrollFormat(object Filled)
      {
         bool filled = bool.Parse(Filled.ToString());
         return filled ? "<b>FILLED</b>" : formatEnroll(counter);
      }

      public string reformatTitle(object title, object monthyear)
      {
         string Title = title.ToString();
         string MonthYear = monthyear.ToString();

         return (Title + MonthYear).ToUpper();
      }
   }
}