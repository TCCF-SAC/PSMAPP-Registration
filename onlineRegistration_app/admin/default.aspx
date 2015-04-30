<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="onlineRegistration_app.admin._default"
   Theme="default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Manage Registration Classes</title>
   <script type="text/javascript" src="../js/jquery-1.7.min.js"></script>
   <script type="text/javascript" src="../js/jquery-ui-1.8.16.custom.min.js"></script>
   <script type="text/javascript" src="../js/jquery.placeholder.js"></script>

   <script type="text/javascript" src="../js/adminFunctions.js"></script>

   <link rel="Stylesheet" href="../css/custom-theme/jquery-ui-1.8.16.custom.css" />
</head>
<body>
   <form id="form1" runat="server" class="formWrap">

   <div class="formWrapMain large cssZebra adminEdits">
      <div style="margin: 15px 0px;">
      <asp:ValidationSummary runat="server" DisplayMode="List" CssClass="valSummary"
                             HeaderText="<strong>Please correct the following errors:</strong>"
                             ValidationGroup="insert" />

      <asp:ValidationSummary runat="server" DisplayMode="List" CssClass="valSummary"
                             HeaderText="<strong>Please correct the following errors:</strong>"
                             ValidationGroup="update" />

      <div id="dialog-custom-title" title="What is Custom Title?" style="display: none;">
         <span class="ui-icon ui-icon-info" style="float:left; margin: 10px 0 0 0;"></span>

         <div style="padding: 10px 15px 10px 25px;">
            Checking 'Custom Title' will remove the date string from the Title when displayed on
            the registration page. This allows you to enter a customized title string.
            <br />
            <br />
            Additionally, if a series does not contain a list of classes, then only the
            title will be visible on the registration page.
         </div>
      </div>

      <div id="dialog-delete-confirm" title="Confirm Delete Request" style="display: none;">
         <p>
            <span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>
            This item will be permanently deleted and cannot be recovered.
            <br /><br />Are you sure?
         </p>
      </div>

      <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="valSummary">
         <div style="font-size: 1.2em; font-weight: bold;">
            <asp:Literal ID="litError" runat="server" />
         </div>
      </asp:Panel>
      </div>

      <div class="scheduleWrapper">
         <asp:Panel ID="pnlSeries" runat="server" Visible="true">
            <asp:ListView ID="lvwSchedule" runat="server" DataSourceID="odsSchedule" InsertItemPosition="FirstItem"
                           DataKeyNames="Title,Date,CustomTitle,DisplayTitle,MonthAndYear,XPathAddress"
                           OnItemCommand="lvwSchedule_ItemCommand" OnItemDataBound="lvwSchedule_OnItemDataBound"
                           OnSelectedIndexChanged="lvwSchedule_OnSelectedIndexChanged">
               <LayoutTemplate>
                  <table cellspacing="0">
                     <thead>
                        <tr>
                           <th colspan="5" class="tbl_title">Schedule Series List</th>
                        </tr>
                        <tr>
                           <th style="width:35%">Title</th>
                           <th style="width:15%">
                              <a href="javascript:help_custom_title();" title="What is Custom Title?"
                                 class="custom-title-help ui-state-default ui-corner-all">
                                 <span class="ui-icon ui-icon-help"></span>
                              </a>
                              Custom Title
                           </th>
                           <th style="width:20%">Date</th>
                           <th style="width:10%">Visible</th>
                           <th style="width:20%">&nbsp;</th>
                        </tr>
                     </thead>
                     <tbody>
                        <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                     </tbody>
                  </table>
               </LayoutTemplate>

               <ItemTemplate>
                  <tr>
                     <td><%#Eval("Title")%></td>
                     <td><%#Eval("CustomTitle")%></td>
                     <td><%#Eval("MonthAndYear")%></td>
                     <td><%#Eval("Visible")%></td>
                     <td>
                        <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select" CommandArgument='<%#Eval("XPathAddress")%>'
                           Text="Select" />
                        |
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="Edit" />
                        |
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="XDelete" CommandArgument='<%#Eval("XPathAddress")%>'
                           Text="Delete" />
                     </td>
                  </tr>
               </ItemTemplate>

               <InsertItemTemplate>
                  <tr class="newItem">
                     <td>
                        <asp:TextBox ID="txtTitle" runat="server" placeholder="Title" Text="Tentative Schedule For" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTitle" ValidationGroup="insert"
                              ErrorMessage="Title for new entry cannot be blank." Display="None" />
                     </td>

                     <td>
                        <asp:CheckBox ID="chkCustomTitle" runat="server" Text="Custom Title" Checked="false" />
                     </td>

                     <td>
                        <asp:DropDownList ID="ddlMonth" runat="server" DataSourceID="odsMonthList" DataValueField="Value"
                           DataTextField="Text" SelectedValue="<%#DateTime.Now.Month.ToString()%>" />
                        <asp:DropDownList ID="ddlYear" runat="server" DataSourceID="odsYearList" SelectedValue="<%#DateTime.Now.Year.ToString()%>" />
                     </td>

                     <td>
                        <asp:CheckBox ID="chkVisible" runat="server" Text="Visible" Checked="true" />
                     </td>

                     <td class="nopad">
                        <asp:LinkButton ID="lnkInsert" runat="server" CommandName="XInsert" Text="Insert" ValidationGroup="insert"
                           CommandArgument='<%#Eval("XPathAddress")%>' />
                        |
                        <asp:LinkButton ID="lnkCancelInsert" runat="server" CommandName="Cancel" Text="Cancel" />
                     </td>
                  </tr>
               </InsertItemTemplate>

               <EditItemTemplate>
                  <tr>
                     <td>
                        <asp:TextBox ID="txtTitle" runat="server" Text='<%#Eval("Title")%>' placeholder="Title" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTitle" ValidationGroup="update"
                              ErrorMessage="Title for existing entry cannot be blank." Display="None" />
                     </td>

                     <td>
                        <asp:CheckBox ID="chkCustomTitle" runat="server" Text="Custom Title" Checked='<%#Eval("CustomTitle")%>' />
                     </td>

                     <td>
                        <asp:DropDownList ID="ddlMonth" runat="server" DataSourceID="odsMonthList" DataValueField="Value"
                                          DataTextField="Text" />
                        <asp:DropDownList ID="ddlYear" runat="server" DataSourceID="odsYearList" />
                     </td>

                     <td>
                        <asp:CheckBox ID="chkVisible" runat="server" Checked='<%#Eval("Visible")%>' Text="Visible" />
                     </td>

                     <td>
                        <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="XUpdate" Text="Update" ValidationGroup="update"
                           CommandArgument='<%#Eval("XPathAddress")%>' />
                        |
                        <asp:LinkButton ID="lnkCancelEdit" runat="server" CommandName="Cancel" Text="Cancel" />
                     </td>
                  </tr>
               </EditItemTemplate>
            </asp:ListView>

            <asp:ObjectDataSource ID="odsSchedule" runat="server" TypeName="onlineRegistration_app.BusinessLayer.ScheduleBLL"
               SelectMethod="SelectSeries" />
         </asp:Panel>

         <asp:Panel ID="pnlClasses" runat="server" Visible="false">
            <asp:LinkButton ID="lnkBackTop" runat="server" Text="Go Back To Schedule Series List" OnClick="lnkBack_OnClick" CssClass="goback" />

            <asp:ListView ID="lvwClasses" runat="server" DataSourceID="odsClasses" InsertItemPosition="FirstItem"
                           OnItemCommand="lvwClasses_ItemCommand" OnDataBound="lvwClasses_OnDataBound">
               <LayoutTemplate>
                  <table cellspacing="0">
                     <thead>
                        <tr>
                           <th colspan="8" class="tbl_title">
                              <asp:Literal ID="litTitle" runat="server" />
                           </th>
                        </tr>
                        <tr>
                           <th>Starts</th>
                           <th class="mandatoryDate">Mandatory Class Dates</th>
                           <th class="skipDate">Skip</th>
                           <th>Ends</th>
                           <th>Days</th>
                           <th>Hours</th>
                           <th>City</th>
                           <th>Filled</th>
                           <th>Cancelled</th>
                           <th style="width:15%">&nbsp;</th>
                        </tr>
                     </thead>
                     <tbody>
                        <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                     </tbody>
                  </table>
               </LayoutTemplate>

               <ItemTemplate>
                  <tr id="classInfo_<%#Eval("ID")%>">
                     <td><%#Eval("Start")%></td>
                     <td class="mandatoryDate"><%#dbObjectToHTMLString(Eval("MandatoryDates"))%></td>
                     <td class="skipDate"><%#dbObjectToHTMLString(Eval("SkipDates"))%></td>
                     <td><%#Eval("End")%></td>
                     <td><%#Eval("Days")%></td>
                     <td><%#Eval("Time")%></td>
                     <td><%#Eval("Area")%></td>
                     <td><%#Eval("Filled")%></td>
                     <td><%#Eval("Cancelled")%></td>
                     <td>
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="Edit" /> |
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="XDelete" CommandArgument='<%#Eval("XPathAddress")%>'
                           Text="Delete" />
                     </td>
                  </tr>
               </ItemTemplate>

               <InsertItemTemplate>
                  <tr class="newItem">
                     <td>
                        <asp:TextBox id="txtStartsDate" runat="server" SkinID="admin-datepicker" MaxLength="10" autocomplete="off" placeholder="Starts" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtStartsDate" ValidationGroup="insert"
                              ErrorMessage="Start date for new entry cannot be blank." Display="None" />
                        <asp:CompareValidator runat="server" ControlToValidate="txtStartsDate" ValidationGroup="insert"
                              ControlToCompare="txtEndsDate" Operator="LessThanEqual" Type="Date"
                              ErrorMessage="Start date for new entry cannot be greater than the End date" Display="None"  />
                     </td>

                     <td id="insertMandatoryDate" class="extraDate">
                        <asp:HiddenField ID="hidMandatoryDateArray" runat="server" />
                        <div class="addDate">
                           <a href="javascript:void(0);" title="Add another mandatory date"
                              class="icon-skip-date ui-state-default ui-corner-all addExtraDate">
                              <span class="ui-icon ui-icon-plusthick"></span>
                           </a>
                           <asp:TextBox id="txtMandatoryDate" runat="server" SkinID="admin-extradatepicker" MaxLength="10" autocomplete="off" placeholder="Date" />
                            <asp:CompareValidator ControlToValidate="txtMandatoryDate" runat="server" ValidationGroup="insert"
                               ControlToCompare="txtStartsDate" Operator="GreaterThanEqual" Type="Date"
                               ErrorMessage="Mandatory date for new entry cannot be less than the Start Date" Display="None" />
                            <asp:CompareValidator ControlToValidate="txtMandatoryDate" runat="server" ValidationGroup="insert"
                               ControlToCompare="txtEndsDate" Operator="LessThanEqual" Type="Date"
                               ErrorMessage="Mandatory date for new entry cannot be greater than the End Date" Display="None" />
                        </div>
                     </td>

                     <td id="insertSkipDate" class="extraDate">
                        <asp:HiddenField ID="hidSkipDateArray" runat="server" />
                        <div class="addDate">
                           <a href="javascript:void(0);" title="Add another skip date"
                              class="icon-skip-date ui-state-default ui-corner-all addExtraDate">
                              <span class="ui-icon ui-icon-plusthick"></span>
                           </a>
                           <asp:TextBox id="txtSkipDate" runat="server" SkinID="admin-extradatepicker" MaxLength="10" autocomplete="off" placeholder="Date" />
                            <asp:CompareValidator ControlToValidate="txtSkipDate" runat="server" ValidationGroup="insert"
                               ControlToCompare="txtStartsDate" Operator="GreaterThanEqual" Type="Date"
                               ErrorMessage="Skip date for new entry cannot be less than the Start Date" Display="None" />
                            <asp:CompareValidator ControlToValidate="txtSkipDate" runat="server" ValidationGroup="insert"
                               ControlToCompare="txtEndsDate" Operator="LessThanEqual" Type="Date"
                               ErrorMessage="Skip date for new entry cannot be greater than the End Date" Display="None" />
                        </div>
                     </td>

                     <td>
                        <asp:TextBox id="txtEndsDate" runat="server" SkinID="admin-datepicker" MaxLength="10" autocomplete="off" placeholder="Ends" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEndsDate" ValidationGroup="insert"
                              ErrorMessage="End date for new entry cannot be blank." Display="None" />
                     </td>

                     <td>
                        <asp:TextBox ID="txtDays" runat="server" SkinID="noSkin" placeholder="Days" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDays" ValidationGroup="insert"
                              ErrorMessage="Class 'Days' for new entry cannot be blank." Display="None" />
                     </td>

                     <td>
                        <asp:TextBox ID="txtTime" MaxLength="5" runat="server" SkinID="timefield" placeholder="Hours" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTime" ValidationGroup="insert"
                              ErrorMessage="Class 'Hours' for new entry cannot be blank." Display="None" />
                     </td>

                     <td>
                        <asp:TextBox ID="txtArea" runat="server" SkinID="noSkin" placeholder="Area" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtArea" ValidationGroup="insert"
                              ErrorMessage="Class 'City/Area' for new entry cannot be blank." Display="None" />
                     </td>

                     <td>
                        <asp:CheckBox ID="chkFilled" runat="server" Checked="false" Text="Filled" />
                     </td>

                     <td>
                        <asp:CheckBox ID="chkCancelled" runat="server" Checked="false" Text="Cancelled" />
                     </td>

                     <td class="nopad">
                        <asp:LinkButton ID="lnkInsert" runat="server" CommandName="XInsert" Text="Insert" ValidationGroup="insert"
                           CommandArgument='<%#odsClasses.SelectParameters["parentXPath"].DefaultValue%>' />
                        |
                        <asp:LinkButton ID="lnkCancelInsert" runat="server" CommandName="Cancel" Text="Cancel" CausesValidation="false" />
                     </td>
                  </tr>
               </InsertItemTemplate>

               <EditItemTemplate>
                  <tr>
                     <td>
                        <asp:TextBox id="txtStartsDate" runat="server" Text='<%#Eval("StartShortDate")%>' SkinID="admin-datepicker"
                           MaxLength="10" autocomplete="off" placeholder="Starts" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtStartsDate" ValidationGroup="update"
                              ErrorMessage="Start date for existing entry cannot be blank." Display="None" />
                        <asp:CompareValidator runat="server" ControlToValidate="txtStartsDate" ValidationGroup="update"
                              ControlToCompare="txtEndsDate" Operator="LessThanEqual" Type="Date"
                              ErrorMessage="Start date for existing entry cannot be greater than the End date" Display="None"  />
                     </td>

                     <td id="editMandatoryDate" class="editExtraDate extraDate">
                        <asp:HiddenField ID="hidMandatoryDateArray" runat="server" Value='<%#Eval("MandatoryDatesJS")%>' />
                        <div style="white-space: nowrap;">
                           <a href="javascript:void(0);" title="Add another mandatory date"
                              class="icon-skip-date ui-state-default ui-corner-all addExtraDate">
                              <span class="ui-icon ui-icon-plusthick"></span>
                           </a>
                           <asp:TextBox id="txtMandatoryDate" runat="server" Text='<%#Eval("MandatoryShortDate")%>' SkinID="admin-extradatepicker"
                              MaxLength="10" autocomplete="off" placeholder="Date" />
                        </div>
                        <asp:CompareValidator ControlToValidate="txtMandatoryDate" runat="server" ValidationGroup="update"
                           ControlToCompare="txtStartsDate" Operator="GreaterThanEqual" Type="Date"
                           ErrorMessage="Mandatory date for existing entry cannot be less than the Start Date" Display="None" />
                        <asp:CompareValidator ControlToValidate="txtMandatoryDate" runat="server" ValidationGroup="update"
                           ControlToCompare="txtEndsDate" Operator="LessThanEqual" Type="Date"
                           ErrorMessage="Mandatory date for existing entry cannot be greater than the End Date" Display="None" />

                        <script type="text/javascript">
                            addExtraDateArray('<%#Eval("MandatoryDateArray")%>', 'editMandatoryDate');
                        </script>
                     </td>

                     <td id="editSkipDate" class="editExtraDate extraDate">
                        <asp:HiddenField ID="hidSkipDateArray" runat="server" Value='<%#Eval("SkipDatesJS")%>' />
                        <div style="white-space: nowrap;">
                           <a href="javascript:void(0);" title="Add another skip date"
                              class="icon-skip-date ui-state-default ui-corner-all addExtraDate">
                              <span class="ui-icon ui-icon-plusthick"></span>
                           </a>
                           <asp:TextBox id="txtSkipDate" runat="server" Text='<%#Eval("SkipShortDate")%>' SkinID="admin-extradatepicker"
                              MaxLength="10" autocomplete="off" placeholder="Date" />
                        </div>
                        <asp:CompareValidator ControlToValidate="txtSkipDate" runat="server" ValidationGroup="update"
                           ControlToCompare="txtStartsDate" Operator="GreaterThanEqual" Type="Date"
                           ErrorMessage="Skip date for existing entry cannot be less than the Start Date" Display="None" />
                        <asp:CompareValidator ControlToValidate="txtSkipDate" runat="server" ValidationGroup="update"
                           ControlToCompare="txtEndsDate" Operator="LessThanEqual" Type="Date"
                           ErrorMessage="Skip date for existing entry cannot be greater than the End Date" Display="None" />

                        <script type="text/javascript">
                            addExtraDateArray('<%#Eval("SkipDateArray")%>', 'editSkipDate');
                        </script>
                     </td>

                     <td>
                        <asp:TextBox id="txtEndsDate" runat="server" Text='<%#Eval("EndShortDate")%>' SkinID="admin-datepicker"
                           MaxLength="10" autocomplete="off" placeholder="Ends" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEndsDate" ValidationGroup="update"
                              ErrorMessage="End date for existing entry cannot be blank." Display="None" />
                     </td>

                     <td>
                        <asp:TextBox ID="txtDays" runat="server" Text='<%#Eval("Days")%>' SkinID="noskin" placeholder="Days" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDays" ValidationGroup="update"
                              ErrorMessage="Class 'Days' for existing entry cannot be blank." Display="None" />
                     </td>

                     <td>
                        <asp:TextBox ID="txtTime" runat="server" Text='<%#Eval("Time")%>' SkinID="timefield" placeholder="Hours" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTime" ValidationGroup="update"
                              ErrorMessage="Class 'Hours' for existing entry cannot be blank." Display="None" />
                     </td>

                     <td>
                        <asp:TextBox ID="txtArea" runat="server" Text='<%#Eval("Area")%>' SkinID="noskin" placeholder="Area" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtArea" ValidationGroup="update"
                              ErrorMessage="Class 'City/Area' for existing entry cannot be blank." Display="None" />
                     </td>

                     <td>
                        <asp:CheckBox ID="chkFilled" runat="server" Checked='<%#Eval("Filled")%>' Text="Filled" />
                     </td>

                     <td>
                        <asp:CheckBox ID="chkCancelled" runat="server" Checked='<%#Eval("Cancelled")%>' Text="Cancelled" />
                     </td>

                     <td>
                        <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="XUpdate" CommandArgument='<%#Eval("XPathAddress")%>'
                           Text="Update" ValidationGroup="update" /> |
                        <asp:LinkButton ID="lnkCancelInsert" runat="server" CommandName="Cancel" Text="Cancel" CausesValidation="false" />
                     </td>
                  </tr>
               </EditItemTemplate>
            </asp:ListView>

            <p>
               <asp:LinkButton ID="lnkBackBottom" runat="server" Text="Go Back To Schedule Series List" OnClick="lnkBack_OnClick" CssClass="goback" />
            </p>

            <asp:ObjectDataSource ID="odsClasses" runat="server" TypeName="onlineRegistration_app.BusinessLayer.ScheduleBLL"
               SelectMethod="SelectClasses">
               <SelectParameters>
                  <asp:Parameter Name="parentXPath" Type="String" /> <%-- DefaultValue="/Series[@id='1']" --%>
               </SelectParameters>
            </asp:ObjectDataSource>
         </asp:Panel>
      </div>
   </div>

   <asp:ObjectDataSource ID="odsYearList" runat="server" TypeName="onlineRegistration_app.BusinessLayer.YearListBLL"
      SelectMethod="Select" />

   <asp:ObjectDataSource ID="odsMonthList" runat="server" TypeName="onlineRegistration_app.BusinessLayer.MonthListBLL"
      SelectMethod="Select" />

   <asp:ObjectDataSource ID="odsMonthShortList" runat="server" TypeName="onlineRegistration_app.BusinessLayer.MonthListBLL"
      SelectMethod="SelectShortMonths" />
   </form>
</body>
</html>
