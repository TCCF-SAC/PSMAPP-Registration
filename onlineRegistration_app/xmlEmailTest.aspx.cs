using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace onlineRegistration_app
{
    public partial class xmlEmailTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (Page pageHolder = new Page())
            {
                using (EmailFormat eFormat = (EmailFormat)pageHolder.LoadControl("EmailFormat.ascx"))
                {
                    pageHolder.Controls.Add(eFormat);
                    pageHolder.ViewStateMode = System.Web.UI.ViewStateMode.Disabled;

                    using (StringWriter output = new StringWriter())
                    {
                        HttpContext.Current.Server.Execute(pageHolder, output, true);
                        litTest.Text = output.ToString();
                    }
                }
            }
        }
    }
}