<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="onlineRegistration_app._Default" Theme="default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Online Registration</title>
   <script type="text/javascript" src="js/jquery-1.7.min.js"></script>
   <script type="text/javascript" src="js/jquery-ui-1.8.16.custom.min.js"></script>
   <script type="text/javascript" src="js/jquery.placeholder.js"></script>
   <script type="text/javascript">
      $(function() {
         $("[placeholder]").placeholder();

         $(".date-picker").datepicker({
            buttonImage: "images/calendar_small.gif",
            buttonImageOnly: true,
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            showOn: "both",
            showOtherMonths: true
         });
      });

      $(function () {
          $('input:checkbox').change(function () {
              if ($('input:checked').size() > 1) {
                  $('input:checkbox').not(':checked').prop('disabled', true);
              }
              else {
                  $('input:checkbox').not(':checked').prop('disabled', false);
              }
          })
      });

    </script>

   <link rel="Stylesheet" href="css/custom-theme/jquery-ui-1.8.16.custom.css" />
</head>
<body>
   <form id="form1" runat="server" class="formWrap">

   <div class="formWrapMain">
      <asp:ValidationSummary runat="server" DisplayMode="List" CssClass="valSummary"
                             HeaderText="<strong>Please review the following errors:</strong>" />
      <asp:Panel ID="pnlEnrollError" runat="server" CssClass="valSummary" Visible="false">
         <strong>Please review the following errors:</strong>
         <br />
         You must check at least one class to enroll into.
      </asp:Panel>

      <asp:Panel ID="pnlError" runat="server" CssClass="valSummary" Visible="false">
         <strong>An unexpected error has occurred, please try again later.</strong>
      </asp:Panel>

      <p style="line-height: 1.3em;">
         <a href="http://hdys.communitycollege.org/programs/psmapp.html"
            title="PS-MAPP">Click to go to the PS-MAPP home page</a>
         <br />
         <a href="http://www.communitycollege.org/"
            title="The Community College Foundation">Click to go to the home page</a>
      </p>

      <asp:Panel ID="pnlPersonalInfo" runat="server">
         <div class="notice">
            <p>
               The first step in becoming a Resource (foster/adoptive) Parent is to attend orientation.
               If you have not completed this step, please call 888-811-1121 to register. Once this is
               complete, it will be necessary that you Live-Scan for DCFS.  Contact Cheryl Rogers at 
               213-351-0173 or Edgar Benitez at 213-351-0276 at DCFS to arrange a visit to a local DCFS
               office during regular business hours. They will enter your information into the Live-Scan
               system so when you visit one of the offices they will be ready.

               A new Live-Scan is required--Foster family agency, Kinship-Care, Day-Care, Employment related
               live-scans are not valid due to privacy and legal concerns. PS-MAPP is the process to become
               a Resource (foster/ adoptive) Parent for a non-relative child.  If you have an ICPC case, the
               Live-Scan requirement is waived.

               <br /><br />

               The next step when this is completed is to register to attend a PS-MAPP Training consisting of
               33 hours during six (6) consecutive weeks. We offer weekday evening, weekday morning, Saturday
               and (tentatively) day-time Monday or Friday series throughout Los Angeles County.
               They are in Spanish or English.
            </p>
         </div>

         <div class="example">All fields marked with a red asterisk <em>*</em> are required.</div>
      
         <table class="plain" cellspacing="0" cellpadding="0" align="left">
             <thead>
             <tr>
                  <th colspan="2" class="tbl_title">Personal Information</th>
               </tr>
            </thead> 
            <tbody>
               <tr>
                  <td class="w1"><asp:Label AssociatedControlID="txtFName" runat="server">First Name <em>*</em></asp:Label></td>
                  <td class="w2" style="white-space:nowrap;">
                     <asp:TextBox id="txtFName" runat="server" AutoCompleteType="FirstName" MaxLength="40" placeholder="First Name"/>
                     <asp:RequiredFieldValidator ControlToValidate="txtFName" runat="server" Display="Dynamic"
                                     ErrorMessage="First name is required." Text="This field is required." />
                  </td>
               </tr>

               <tr>
                  <td class="w1"><asp:Label AssociatedControlID="txtLName" runat="server">Last Name <em>*</em></asp:Label></td>
                  <td class="w2" style="white-space:nowrap;">
                     <asp:TextBox id="txtLName" runat="server" AutoCompleteType="LastName" MaxLength="40" placeholder="Last Name"/>
                     
                      <asp:RequiredFieldValidator ControlToValidate="txtLName" runat="server" Display="Dynamic"
                                                 ErrorMessage="Last name is required." Text="This field is required."  />
                  </td>
               </tr>

               <tr>
                  <td class="w1"><asp:Label AssociatedControlID="txtSpouseName" runat="server" Text="Spouse/Partner Name"/></td>
                  <td class="w2"><asp:TextBox id="txtSpouseName" runat="server" placeholder="Spouse/Partner Name" /></td>
               </tr>

               <tr>
                  <td class="w1"><asp:Label AssociatedControlID="txtPhone" runat="server">Phone Number <em>*</em></asp:Label></td>
                  <td class="w2" style="white-space:nowrap;">
                     <asp:TextBox id="txtPhone" runat="server" AutoCompleteType="HomePhone" MaxLength="14" placeholder="Phone Number"/>
                     <asp:RequiredFieldValidator ControlToValidate="txtPhone" runat="server" Display="Dynamic"
                                                 ErrorMessage="Phone number is required." Text="This field is required."/>
                     <asp:RegularExpressionValidator ControlToValidate="txtPhone" runat="server" Display="Dynamic"
                                 ValidationExpression="<%$ appSettings:regex_Phone %>"
                                 ErrorMessage="Phone number is invalid." Text="This field is invalid." />
                     <div class="example">
                        Format: 10 digits, including area code. ###-###-####
                     </div>
                  </td>
               </tr>

               <tr>
                  <td class="w1">
                      <asp:Label AssociatedControlID="txtCell" runat="server" Text="Cell Number" />

                  </td>
                  <td class="w2" style="white-space:nowrap;">
                     <asp:TextBox id="txtCell" runat="server" MaxLength="14" placeholder="Cell Number" />
                     <asp:RegularExpressionValidator ControlToValidate="txtCell" runat="server" Display="Dynamic"
                                 ValidationExpression="<%$ appSettings:regex_Phone %>"
                                 ErrorMessage="Cell number is invalid." Text="This field is invalid." />
                     <div class="example">
                        Format: 10 digits, including area code. ###-###-####
                     </div>
                  </td>
               </tr>

               <tr>
                  <td class="w1"><asp:Label AssociatedControlID="txtEmail" runat="server" Text="Email Address" /></td>
                  <td class="w2" style="white-space:nowrap;">
                     <asp:TextBox id="txtEmail" runat="server" MaxLength="65" placeholder="Email Address"/>
                     <asp:RegularExpressionValidator ControlToValidate="txtEmail" runat="server" Display="Dynamic"
                                 ValidationExpression="<%$ appSettings:regex_Email %>"
                                 ErrorMessage="Email address is invalid." Text="This field is invalid." />
                  </td>
               </tr>

               <tr>
                  <td class="w1"><asp:Label AssociatedControlID="txtOrientation" runat="server" Text="Orientation Date <em>*</em>" /></td>
                  <td class="w2 date-picker-container" style="white-space:nowrap;">
                     <asp:TextBox id="txtOrientation" runat="server" MaxLength="10" Width="170" SkinID="datepicker" placeholder="Orientation Date" />
                     <asp:RequiredFieldValidator ControlToValidate="txtOrientation" runat="server" Display="Dynamic"
                                                 ErrorMessage="Orientation Date is required." Text="This field is required." />
                     <asp:CompareValidator ID="cvOrientation" runat="server" Type="Date" ControlToValidate="txtOrientation"
                        Operator="DataTypeCheck" Text="This date is invalid."
                        ErrorMessage="Orientation date is invalid." />
                  </td>
               </tr>

                <tr>
                    <td class="w1"><asp:Label AssociatedControlID="txtLiveScanDate" runat="server" Text="Live Scan Date <em>*</em>" /></td>
                    
                    <td class="w2 date-picker-container" style="white-space:nowrap;">
                        <asp:TextBox id="txtLiveScanDate" runat="server" MaxLength="10" Width="170" SkinID="datepicker" placeholder="Live Scan Date" />
                        
                        
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtLiveScanDate" runat="server" Display="Dynamic"
                                                 ErrorMessage="Live Scan Date is required." Text="This field is required." />
                        
                        
                        
                        <asp:CompareValidator ID="cvLiveScanDate" runat="server" Type="Date" ControlToValidate="txtLiveScanDate"
                        Operator="DataTypeCheck" Text="This date is invalid."
                        ErrorMessage="Live Scan date is invalid." />
                    </td>
                </tr>
                 
                 <tr>
                  <td class="w1" style="white-space:nowrap;">
                      <asp:Label ID="Label1" AssociatedControlID="txtDCFS" runat="server" Text="County/DCFS Live Scan Location <em>*</em>" />
                  </td>
                  <td class="w2" style="white-space:nowrap;">
                    <asp:TextBox id="txtDCFS" runat="server" MaxLength="65" placeholder="DCFS Live Scan Location"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtDCFS" runat="server" Display="Dynamic"
                    ErrorMessage="Live Scan Location field is required." Text="This field is required." />
                  </td>
               </tr>

            </tbody>
         </table>

         <table class="plain" cellspacing="0">
            <thead>
               <tr>
                  <th colspan="4" class="tbl_title">Address information</th>
               </tr>
            </thead>

            <tbody>
               <tr>
                  <td class="w1"><asp:Label AssociatedControlID="txtAddress1" runat="server">Address1 <em>*</em></asp:Label></td>
                  <td class="w2" colspan="3">
                     <asp:TextBox id="txtAddress1" runat="server" AutoCompleteType="HomeStreetAddress" MaxLength="65" placeholder="Address 1"/>
                     <asp:RequiredFieldValidator ControlToValidate="txtAddress1" runat="server" Display="Dynamic"
                                                 ErrorMessage="Address1 field is required." Text="This field is required." />
                  </td>
               </tr>

               <tr>
                  <td class="w1"><asp:Label AssociatedControlID="txtCity" runat="server">City <em>*</em></asp:Label></td>
                  <td class="w2" colspan="3">
                     <asp:TextBox id="txtCity" runat="server" AutoCompleteType="HomeCity" MaxLength="65" placeholder="Address 2"/>
                     <asp:RequiredFieldValidator ControlToValidate="txtCity" runat="server" Display="Dynamic"
                                                 ErrorMessage="City field is required." Text="This field is required." />
                  </td>
               </tr>

               <tr>
                  <td class="w1"><asp:Label AssociatedControlID="txtState" runat="server">State <em>*</em></asp:Label></td>
                  <td class="w3"><asp:TextBox id="txtState" runat="server" AutoCompleteType="HomeState" MaxLength="2" SkinID="short" placeholder="State"/></td>

                  <td class="w3 align_right"><asp:Label AssociatedControlID="txtZip" runat="server">Zip Code <em>*</em></asp:Label></td>
                  <td class="w4"><asp:TextBox id="txtZip" runat="server" AutoCompleteType="HomeZipCode" MaxLength="5" SkinID="short" placeholder="Zip"/></td>
               </tr>
            
               <tr class="nopad">
                  <td></td>
                  <td class="align_left">
                     <asp:RequiredFieldValidator ControlToValidate="txtState" runat="server" Display="Dynamic"
                                                 ErrorMessage="State field is required." Text="This field is required." />
                  </td>

                  <td class="align_left" colspan="2">
                     <asp:RequiredFieldValidator ControlToValidate="txtZip" runat="server" Display="Dynamic"
                                                 ErrorMessage="Zip code is required." Text="This field is required." />
                     
                      <asp:RegularExpressionValidator ControlToValidate="txtZip" runat="server" Display="Dynamic"
                                 ValidationExpression="<%$ appSettings:regex_NumberOnly %>"
                                 ErrorMessage="Zip code is invalid." Text="This field is invalid." />

                   </td>
               </tr>
            </tbody>
         </table>
         <div class="notice">
            <p>
               Registration is not complete until we contact you via phone to confirm enrollment.
               We'll try to contact you within 24 hours during regular business hours as we need to
               speak with you to ask a few follow-up questions.
               <%--Our office is closed for Winter Break and will re-open on Wednesday, January 2, 2013.
               Please continue to submit your online registration form because we enroll people in the
               order they were received.
               We will verify your enrollment via email the week of January 7th.
               Thank you in advance for your patience and understanding and happy holidays!--%>
            </p>
         </div>
      </asp:Panel>

      <%--
      <div class="scheduleNotice">
         Thank you for enrolling in PS-MAPP. All classes for this series are filled. We will begin Registration in July for the next series.
      </div>
      --%>

      <asp:Panel ID="pnlClasses" runat="server">
         <asp:ListView ID="lstSchedule" runat="server" DataSourceID="odsSchedule"
                        OnDataBound="lstSchedule_OnDataBound" OnItemDataBound="lstSchedule_OnItemDataBound">
            <ItemTemplate>
               <table cellspacing="0">
                  <thead>
                     <tr>
                        <th colspan="8" class="tbl_title<%# ((bool)Eval("CustomTitle") ? " plain" : "") %>">
                           <asp:HiddenField ID="hidXPath" runat="server" Visible="false" Value='<%#Eval("XPathAddress")%>' />
                           <%#Eval("DisplayTitle")%>
                        </th>
                     </tr>

                     <tr id="lstScheduleTR" runat="server">
                        <th>Starts</th>
                        <th>Mandatory Class Dates</th>
                        <th>Skip</th>
                        <th>Ends</th>
                        <th>Days</th>
                        <th>Hours</th>
                        <th>City</th>
                        <th>&nbsp;</th>
                     </tr>
                  </thead>

                  <tbody>
                     <asp:ListView ID="lstClasses" runat="server" DataSourceID="odsClasses">
                        <ItemTemplate>
                           <tr <%# Container.DataItemIndex % 2 == 0 ? "" : "class=\"alt\"" %>>
                              <td><%#Eval("Start")%></td>
                              <td style="max-width: 250px;"><%#Eval("MandatoryDates")%></td>
                              <td style="max-width: 75px;"><%#Eval("SkipDates")%></td>
                              <td><%#Eval("End")%></td>
                              <td><%#Eval("Days")%></td>
                              <td><%#Eval("Time")%></td>
                              <td><%#Eval("Area")%></td>
                              <td>
                                 <asp:CheckBox ID="chkEnroll" runat="server" Text="Enroll for this class" Visible='<%#!dbToBool(Eval("Filled")) && !dbToBool(Eval("Cancelled"))%>' />
                                 <asp:Literal ID="litFilled" runat="server" Visible='<%#dbToBool(Eval("Filled"))%>'><b>FILLED</b></asp:Literal>
                                 <asp:Literal ID="litCancelled" runat="server" Visible='<%#dbToBool(Eval("Cancelled"))%>'><b>CLASS CANCELLED</b></asp:Literal>
                              </td>
                           </tr>
                        </ItemTemplate>

                        <EmptyDataTemplate>
                           <tr class="alt">
                              <td colspan="8" class="notice">
                                 There are no classes within this series.
                              </td>
                           </tr>
                        </EmptyDataTemplate>
                     </asp:ListView>
                  </tbody>
               </table>

               <asp:ObjectDataSource ID="odsClasses" runat="server" TypeName="onlineRegistration_app.BusinessLayer.ScheduleBLL"
                  SelectMethod="SelectClasses">
                  <SelectParameters>
                     <asp:ControlParameter Name="parentXPath" ControlID="hidXPath" PropertyName="Value" Type="String" />
                  </SelectParameters>
               </asp:ObjectDataSource>
            </ItemTemplate>

            <EmptyDataTemplate>
               <div class="valSummary scheduleNotice" style="text-align: left;">
                  <p>
                     There are currently no classes to display.
                     Enrollment will be suspended until at least one series of classes is added.
                  </p>
                  <p>
                     Please check back later or contact us using the links below for more information.
                  </p>
               </div>
            </EmptyDataTemplate>
         </asp:ListView>

         <asp:ObjectDataSource ID="odsSchedule" runat="server" TypeName="onlineRegistration_app.BusinessLayer.ScheduleBLL"
            SelectMethod="SelectSeriesVisible" />
      </asp:Panel>

      <asp:Panel ID="pnlFinalSteps" runat="server">
         <div class="tbl_title"><asp:Label AssociatedControlID="txtQuestions" runat="server" Text="Questions:" /></div>
         <asp:TextBox ID="txtQuestions" runat="server" TextMode="MultiLine" Columns="120" Rows="6" SkinID="noSkin"
                      taMaxLength="100" onKeyPress="return taLimit(this);" onKeyUp="return taCount(this, 'questionsCount');"
                      placeholder="Questions or Comments" />
         <div>
            You have
            <span id="questionsCount" class="bold"><asp:Literal ID="litQuestionsCount" runat="server" Text="100" /></span>
            characters remaining...
         </div>

         <div class="buttons">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" UseSubmitBehavior="true" />
         </div>
      </asp:Panel>

      <div class="notice_footer">
         <p>
            For more information please call or
            <a href="http://www.communitycollege.org/about-us/contact-us/"
               target="_blank">e-mail Alex Wallace</a>, Program Assistant - phone
            <strong>213-640-3079</strong> or toll-free <strong>1.866.266.2655 x241</strong>.
         </p>

         <p>
            Or call or
            <a href="http://www.communitycollege.org/about-us/contact-us/"
               target="_blank">e-mail Gomanna Choi</a>, Program Coordinator -
               phone <strong>213-640-3082</strong> or toll-free <strong>1.866.266.2655 x209</strong>.
         </p>

         <p>
            For classes in Spanish please contact Mina Mata, Program Manager - phone
            <strong>213-640-3083</strong> or toll-free <strong>1.866.266.2655 x247</strong>.
            <a href="http://www.communitycollege.org/about-us/contact-us/"
               target="_blank">e-mail Mina Mata</a>.
         </p>
      </div>
   </div>
   </form>
</body>
</html>
