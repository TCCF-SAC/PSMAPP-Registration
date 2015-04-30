using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.IO;

using System.Collections.Generic;
using System.Web.Caching;
using TCCF.Utilities;
using onlineRegistration_app.BusinessLayer;
//using System.Xml.XPath;
//using System.Xml.Schema;

namespace onlineRegistration_app
{
   public partial class _Default : System.Web.UI.Page
   {
      //private const string EMAIL_FORMAT_CACHE_ID = "EMAIL_FORMAT_CACHE";

      protected void Page_Load(object sender, EventArgs e)
      {
         pnlEnrollError.Visible = false;
         pnlError.Visible = false;
        //Page.ClientScript.RegisterOnSubmitStatement(this.GetType(), "val", "fnOnUpdateValidators();");

         if (!IsPostBack) return;
         litQuestionsCount.Text = (100 - txtQuestions.Text.Length).ToString();
      }

      public bool dbToBool(object dbObject)
      {
         bool boolObject = false;
         bool.TryParse(dbObject.ToString(), out boolObject);
         return boolObject;
      }

      /// <summary>
      /// Fills lstEnroll with each Checkbox's Checked value then returns true if at least one checkbox was checked
      /// </summary>
      /// <param name="lstEnroll">Boolean List to fill</param>
      /// <returns>Boolean</returns>
      private bool getEnrolledList(List<bool> lstEnroll)
      {
         bool hasEnrollCheck = false;  // Use this to determine if the user
                                       //    has checked at least one enrollment checkbox.

         foreach (ListViewItem lstItem in lstSchedule.Items)
         {
            if(lstItem.ItemType != ListViewItemType.DataItem) continue;

            ListView lstClasses = (ListView)lstItem.FindControl("lstClasses");

            if (lstClasses != null)
            {
               foreach (ListViewItem classItem in lstClasses.Items)
               {
                  if (classItem.ItemType != ListViewItemType.DataItem) continue;

                  foreach (Control objControl in classItem.Controls)
                  {
                     if (objControl is CheckBox)
                     {
                        CheckBox objCheck = (CheckBox)objControl;
                        lstEnroll.Add(objCheck.Checked);
                        if (objCheck.Checked) hasEnrollCheck = true;
                     }
                  }
               }
            }
         }

         return hasEnrollCheck;
      }

      protected void btnSubmit_Click(object sender, EventArgs e)
      {
          Page.Validate();
                
          if (!Page.IsValid) return;
          
          List<bool> lstEnroll = new List<bool>();

          if (pnlClasses.Visible)
          {
              bool hasEnrollCheck = getEnrolledList(lstEnroll);

              if (!hasEnrollCheck) { pnlEnrollError.Visible = true; return; }
          }

          // Get the email html format from the file.
          string emailFormatHTML = getEmailFormat;

          // If the email format is blank then display an error message and exit the function.
          if (string.IsNullOrEmpty(emailFormatHTML))
          {
              pnlError.Visible = true;
              return;
          }

          // Format that the phone numbers will be displayed.
          string phones = formatPhoneNumber(txtPhone.Text) + " H<br />";
          string cell = formatPhoneNumber(txtCell.Text);
          phones += (!string.IsNullOrEmpty(cell)) ? cell + " C" : "";

          DateTime orientation = DateTime.MinValue;
          DateTime livescandate = DateTime.MinValue;
          DateTime.TryParse(txtOrientation.Text, out orientation);
          DateTime.TryParse(txtLiveScanDate.Text, out livescandate);

          // Set values into the email format text.
          emailFormatHTML = emailFormatHTML.Replace("%name%", txtFName.Text.Trim() + " " + txtLName.Text.Trim())
                            .Replace("%spouse%", txtSpouseName.Text.Trim())
                            .Replace("%address%",
                               getFullAddressHTML(txtAddress1.Text.Trim(), txtCity.Text.Trim(),
                                                  txtState.Text.Trim(), txtZip.Text.Trim()))
                            .Replace("%phone%", phones)
                            .Replace("%email%", txtEmail.Text.Trim().Replace("@", "@&shy;"))
                            .Replace("%orientation_date%", (orientation == DateTime.MinValue ? "" : orientation.ToShortDateString()))
                            .Replace("%livescan_date%", (livescandate == DateTime.MinValue ? "" : livescandate.ToShortDateString()))
                            .Replace("%dcfslocation%", txtDCFS.Text.Trim().Replace("\n", "<br />"))
                            .Replace("%questions%", txtQuestions.Text.Trim().Replace("\n", "<br />"));

          string DCFS = txtDCFS.Text;
          Console.WriteLine(DCFS);

          if (pnlClasses.Visible)
          {
              for (int i = 0; i < lstEnroll.Count; i++)
                  emailFormatHTML = emailFormatHTML.Replace("%enroll_" + (i + 1).ToString() + "%", (lstEnroll[i] ? "YES" : "No"));
          }

          sendEmail(emailFormatHTML);
      }

      private void sendEmail(string emailMessage)
      {
         string userEmail = txtEmail.Text.Trim();
         string userName = txtFName.Text.Trim() + " " + txtLName.Text.Trim();
         string[] arrTCCF_Sender, arrTCCF_Recipient;

         string subject;
         try { subject = ConfigurationManager.AppSettings["Email_Subject"]; }
         catch (Exception) { subject = "Online Registration"; }

         try
         {
            EmailObj email = new EmailObj(subject, "", true);
            arrTCCF_Sender = ConfigurationManager.AppSettings["TCCF_SendingEmail"].Split(';');
            arrTCCF_Recipient = ConfigurationManager.AppSettings["TCCF_ContactEmail"].Split(';');

            // Email to a contact from TCCF.
            email.message = emailMessage.Replace("%header%", "");
            email.setRecipient(arrTCCF_Recipient[0], arrTCCF_Recipient[1]);
            email.setSender(arrTCCF_Sender[0], arrTCCF_Sender[1]);

            if (!string.IsNullOrEmpty(userEmail)) email.setReplyTo(userEmail, userName);

            email.send(null, null);
         }
         
         catch(Exception)
            {
                
                pnlError.Visible = true;
                return;
            }

         // Attempt to send the user a copy of their registration
         if (!string.IsNullOrEmpty(userEmail))
         {
            string thanksMessage;
            
            // Get the thank you message from our web config file.
            try { thanksMessage = ConfigurationManager.AppSettings["Thanks_Message"]; }
            catch (Exception) { thanksMessage = "";  }

            try
            {
               thanksMessage += (!string.IsNullOrEmpty(thanksMessage) ? "<br /><br />" : "") + 
                                 "<strong>The following is a copy of your online registration</strong><br />";

               EmailObj email = new EmailObj(subject, "", true);
               email.message = emailMessage.Replace("%header%", thanksMessage);
               // Could we do an auto response thanking them for their time and letting them know that
               //    enrollment is not confirmed until we contact them via phone or email?  Thanks.
               email.setRecipient(userEmail, userName);
               email.setSender(arrTCCF_Sender[0], arrTCCF_Sender[1]);
               email.send(null, null);
            }
            catch (Exception) {}
         }

         // If we reach this point then redirect the user to the thank you page.
         Response.Redirect("~/thanks.aspx", true);
      }

      /// <summary>
      /// Returns the HTML formatting for the email message.
      /// </summary>
      private string getEmailFormat
      {
         get {
            try {
               using (Page pageHolder = new Page())
               {
                  using (EmailFormat eFormat = (EmailFormat)pageHolder.LoadControl("EmailFormat.ascx"))
                  {
                     pageHolder.Controls.Add(eFormat);
                     pageHolder.ViewStateMode = System.Web.UI.ViewStateMode.Disabled;

                     using (StringWriter output = new StringWriter())
                     {
                        HttpContext.Current.Server.Execute(pageHolder, output, false);
                        return output.ToString();
                     }
                  }
               }
            } catch { return ""; }
         }
      }

      /// <summary>
      /// Returns full address HTML code from string parameters.
      /// </summary>
      /// <param name="addr1">Address1 field</param>
      /// <param name="addr2">Address2 field</param>
      /// <param name="city">City field</param>
      /// <param name="state">State field</param>
      /// <param name="zip">Zip field</param>
      /// <returns></returns>
      private string getFullAddressHTML(string addr1, string city, string state, string zip)
      {
         // Cannot have a full address without the first address line.
         if (string.IsNullOrEmpty(addr1)) return "";

         city += (!string.IsNullOrEmpty(city)) ? ", " : "";

         // Format for the address
         return string.Format("{0}<br />{1}{2} {3}", addr1, city, state, zip);
      }

      /// <summary>
      /// Formats a phone number from a string/database string
      /// </summary>
      /// <returns></returns>
      private string formatPhoneNumber(string pNumber)
      {
         string fPhone = pNumber.Trim();
         if (string.IsNullOrEmpty(fPhone)) return "";

         // Grab the integer's from the phone number
         fPhone = System.Text.RegularExpressions.Regex.Replace(fPhone, "[^\\d]", "");

         string tmpFormat = "###-####";   // Default format if the phone is only 7 digits.

         if (fPhone.Length == 11)
            tmpFormat = "#-###-" + tmpFormat;
         else if (fPhone.Length > 7)
            tmpFormat = "(###) " + tmpFormat;

         return string.Format("{0:" + tmpFormat + "}", long.Parse(fPhone));
      }

      public void lstSchedule_OnDataBound(object sender, EventArgs e)
      {
         if (lstSchedule.Items.Count > 0) return;

         pnlPersonalInfo.Visible = false;
         pnlFinalSteps.Visible = false;
      }

      public void lstSchedule_OnItemDataBound(object sender, ListViewItemEventArgs e)
      {
         ListViewItem lvData = e.Item;
         ListView lstClasses = (ListView)lvData.FindControl("lstClasses");

         if (lstClasses == null) return;

         if ((bool)DataBinder.Eval(lvData.DataItem, "CustomTitle") && lstClasses.Items.Count == 0)
         {
            HtmlTableRow lstScheduleTR = (HtmlTableRow)lvData.FindControl("lstScheduleTR");
            lstClasses.Visible = false;
            lstScheduleTR.Visible = false;
         }
      }
   }
}
