using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using onlineRegistration_app.BusinessLayer;

namespace onlineRegistration_app.admin
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pnlError.Visible = false;
        }

        public void lnkBack_OnClick(object sender, EventArgs e)
        {
            lvwSchedule.SelectedIndex = -1;
            pnlSeries.Visible = true;
            pnlClasses.Visible = false;
        }

        #region Helper Methods
        public string dbObjectToHTMLString(object value) { return dbObjectToHTMLString(value, "&nbsp;"); }
        public string dbObjectToHTMLString(object value, string nullValueReplace)
        {
            if (string.IsNullOrEmpty(nullValueReplace)) nullValueReplace = "&nbsp;";
            string strCheck = value.ToString();
            if (string.IsNullOrEmpty(strCheck)) return nullValueReplace;
            return strCheck;
        }

        private void displayDbError(bool dbModified, string errorMessage, string commandName)
        {
            if (!dbModified)
            {
                if (string.IsNullOrEmpty(errorMessage))
                {
                    litError.Text = "There was an unexpected error issuing the ";
                    switch (commandName)
                    {
                        case "xdelete":
                            litError.Text += "Delete";
                            break;
                        case "xupdate":
                            litError.Text += "Update";
                            break;
                        case "xinsert":
                            litError.Text += "Insert";
                            break;
                    }
                    litError.Text += " command. Please try again.";
                }
                else
                    litError.Text = errorMessage;

                pnlError.Visible = true;
            }
        }

        private void setListViewDropDownValue(ListViewItem lvItem, string dropDownName, string dbItem)
        {
            DropDownList ddl = (DropDownList)lvItem.FindControl(dropDownName);
            if (ddl == null) return;

            string value = DataBinder.Eval(lvItem.DataItem, dbItem).ToString();

            ListItem selValue = ddl.Items.FindByValue(value);
            if (selValue == null) return;

            ddl.ClearSelection();
            selValue.Selected = true;
        }

        private void setFormScripts()
        {
            ScheduleBLL scheduleDB = new ScheduleBLL();

            string xPath = odsClasses.SelectParameters["parentXPath"].DefaultValue;
            if (string.IsNullOrEmpty(xPath)) return;

            XElement root = scheduleDB.getXElement.XPathSelectElement(xPath);

            DateTime dateRange = (DateTime)DateTime.Parse(root.Attribute("Date").Value);

            // Set the script to restrict the date picker date range
            string script = "setDateRangeByMonth('" + dateRange.ToShortDateString() + "');";
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "setDateRange", script, true);
        }
        #endregion

        #region lvwSchedule Events
        public void lvwSchedule_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string commandName = e.CommandName.ToLower();
            if (!commandName.StartsWith("x")) return;

            ScheduleBLL scheduleDB = new ScheduleBLL();

            bool dbModified = false;

            string xPath = (string)e.CommandArgument;

            if (commandName.Equals("xdelete"))
            {
                if (scheduleDB.deleteItem(xPath))
                {
                    lvwSchedule.DataBind();
                    dbModified = true;
                }
            }
            else
            {
                SeriesInfo seriesInfo = new SeriesInfo();

                DropDownList ddlMonth = (DropDownList)e.Item.FindControl("ddlMonth");
                DropDownList ddlYear = (DropDownList)e.Item.FindControl("ddlYear");
                TextBox txtTitle = (TextBox)e.Item.FindControl("txtTitle");
                CheckBox chkCustomTitle = (CheckBox)e.Item.FindControl("chkCustomTitle");
                CheckBox chkVisible = (CheckBox)e.Item.FindControl("chkVisible");

                // Set the new series information data
                seriesInfo.Date = new DateTime(int.Parse(ddlYear.SelectedValue), int.Parse(ddlMonth.SelectedValue), 1);
                seriesInfo.Title = txtTitle.Text;
                seriesInfo.CustomTitle = chkCustomTitle.Checked;
                seriesInfo.Visible = chkVisible.Checked;

                // Update or save the series info based on the command name
                if (commandName.Equals("xupdate"))
                {
                    if (scheduleDB.saveChanges(xPath, seriesInfo))
                    {
                        lvwSchedule.EditIndex = -1;
                        dbModified = true;
                    }
                }
                else   // xinsert
                {
                    if (scheduleDB.saveNewItem(seriesInfo))
                    {
                        lvwSchedule.DataBind();
                        dbModified = true;
                    }
                }

                if (dbModified)
                {
                    string strScript = "highLightSeries('" + seriesInfo.Date.ToShortDateString() + "');";
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "hlSeries", strScript, true);
                }
            }

            displayDbError(dbModified, scheduleDB.Error, commandName);
        }

        public void lvwSchedule_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwSchedule.SelectedIndex == -1) return;
            DataKey dk = lvwSchedule.SelectedDataKey;
            odsClasses.SelectParameters["parentXPath"].DefaultValue = dk.Values["XPathAddress"].ToString();
            lvwClasses.DataBind();

            Literal litTitle = (Literal)lvwClasses.FindControl("litTitle");
            if (litTitle != null)
            {
                bool customTitle = (bool)dk.Values["CustomTitle"];
                string MonthAndYear = (customTitle) ? "(" + dk.Values["MonthAndYear"].ToString() + ")" : "";
                litTitle.Text = dk.Values["DisplayTitle"].ToString() + " " + MonthAndYear;
            }

            pnlSeries.Visible = false;
            pnlClasses.Visible = true;
        }

        public void lvwSchedule_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType != ListViewItemType.DataItem ||
                  lvwSchedule.EditIndex == -1 ||
                  lvwSchedule.EditIndex != e.Item.DataItemIndex) return;

            setListViewDropDownValue(e.Item, "ddlMonth", "Month");

            setListViewDropDownValue(e.Item, "ddlYear", "Year");
        }
        #endregion

        #region lvwClasses Events
        public void lvwClasses_OnDataBound(object sender, EventArgs e)
        {
            setFormScripts();
            lvwClasses_SetReadOnlyInputs();
        }

        /// <summary>
        /// Make sure users cannot enter manual data into specific fields by making them readonly.
        /// </summary>
        private void lvwClasses_SetReadOnlyInputs()
        {
            ListViewItem[] items = { lvwClasses.InsertItem, lvwClasses.EditItem };

            foreach (ListViewItem lvi in items)
            {
                if (lvi == null) continue;
                TextBox txtStartsDate = (TextBox)lvi.FindControl("txtStartsDate");
                TextBox txtEndsDate = (TextBox)lvi.FindControl("txtEndsDate");
                TextBox txtSkipDate = (TextBox)lvi.FindControl("txtSkipDate");
                TextBox txtMandatoryDate = (TextBox)lvi.FindControl("txtMandatoryDate");

                if (txtStartsDate != null) txtStartsDate.Attributes.Add("readonly", "readonly");
                if (txtEndsDate != null) txtEndsDate.Attributes.Add("readonly", "readonly");
                if (txtSkipDate != null) txtSkipDate.Attributes.Add("readonly", "readonly");
                if (txtMandatoryDate != null) txtMandatoryDate.Attributes.Add("readonly", "readonly");
            }
        }

        public void lvwClasses_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string commandName = e.CommandName.ToLower();
            if (!commandName.StartsWith("x")) return;

            ScheduleBLL scheduleDB = new ScheduleBLL();

            bool dbModified = false;

            string xPath = (string)e.CommandArgument;

            if (commandName.Equals("xdelete"))
            {
                if (scheduleDB.deleteItem(xPath))
                {
                    lvwClasses.DataBind();
                    dbModified = true;
                }
            }
            else
            {
                ClassInfo classInfo = new ClassInfo();

                TextBox txtStartsDate = (TextBox)e.Item.FindControl("txtStartsDate");
                HiddenField hidMandatoryDateArray = (HiddenField)e.Item.FindControl("hidMandatoryDateArray");
                TextBox txtMandatoryDate = (TextBox)e.Item.FindControl("txtMandatoryDate");
                HiddenField hidSkipDateArray = (HiddenField)e.Item.FindControl("hidSkipDateArray");
                TextBox txtSkipDate = (TextBox)e.Item.FindControl("txtSkipDate");
                TextBox txtEndsDate = (TextBox)e.Item.FindControl("txtEndsDate");
                TextBox txtDays = (TextBox)e.Item.FindControl("txtDays");
                TextBox txtTime = (TextBox)e.Item.FindControl("txtTime");
                TextBox txtArea = (TextBox)e.Item.FindControl("txtArea");
                CheckBox chkFilled = (CheckBox)e.Item.FindControl("chkFilled");
                CheckBox chkCancelled = (CheckBox)e.Item.FindControl("chkCancelled");

                classInfo.StartDate = DateTime.Parse(txtStartsDate.Text);
                classInfo.EndDate = DateTime.Parse(txtEndsDate.Text);
                classInfo.Days = txtDays.Text;
                classInfo.Time = txtTime.Text;
                classInfo.Area = txtArea.Text;
                classInfo.Filled = chkFilled.Checked;
                classInfo.Cancelled = chkCancelled.Checked;

                // If hidMandatoryDateArray is blank, then only set the MandatoryDate,
                // otherwise set the MandatoryDateArray, which uses the first Date from the array as MandatoryDate
                if (string.IsNullOrEmpty(hidMandatoryDateArray.Value))
                    classInfo.MandatoryDate = (string.IsNullOrEmpty(txtMandatoryDate.Text) ? DateTime.MinValue : DateTime.Parse(txtMandatoryDate.Text));
                else
                    classInfo.MandatoryDateArray = hidMandatoryDateArray.Value;

                // If hidSkipDateArray is blank, then only set the SkipDate,
                // otherwise set the SkipDateArray, which uses the first Date from the array as SkipDate
                if (string.IsNullOrEmpty(hidSkipDateArray.Value))
                    classInfo.SkipDate = (string.IsNullOrEmpty(txtSkipDate.Text) ? DateTime.MinValue : DateTime.Parse(txtSkipDate.Text));
                else
                    classInfo.SkipDateArray = hidSkipDateArray.Value;

                // Update or save the series info based on the command name
                if (commandName.Equals("xupdate"))
                {
                    if (scheduleDB.saveChanges(xPath, classInfo))
                    {
                        lvwClasses.EditIndex = -1;
                        dbModified = true;
                    }
                }
                else   // xinsert
                {
                    if (scheduleDB.saveNewItem(xPath, classInfo))
                    {
                        lvwClasses.DataBind();
                        dbModified = true;
                    }
                }

                if (dbModified)
                {
                    string strScript = "highLightClass('" + classInfo.ID.ToString() + "');";
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "hlClass", strScript, true);
                }
            }

            displayDbError(dbModified, scheduleDB.Error, commandName);
        }

        #endregion
    }
}