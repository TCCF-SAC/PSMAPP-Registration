<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailFormat.ascx.cs" Inherits="onlineRegistration_app.EmailFormat" ViewStateMode="Disabled" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
   <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
   <title>Online Registration - Tentative Schedule List</title>
   <style type="text/css">
      body
      {
         font-family: Verdana;
         font-size: 9pt;
         line-height: 120%;
         color: #222;
      }
      .tableStyle, .notice { width: 95%; margin: 0px auto; padding: 0px; }
      .tableStyle tr td, .tableStyle thead tr th { text-align: center; padding: 6px 4px; border: solid 1px #000; }
      .tableStyle thead tr th
      {
         font-size: 10pt;
         font-weight: bold;
         background: #ddd;
      }
      .tableStyle thead .tbl_title { font-size: 125%; background: #fff; border: none; }
      .tableStyle thead .tbl_title small { font-weight: normal; }
      .tableStyle .altNo td { background: #eee; }
      .tableStyle tr td.align_left { text-align: left; }
      .tableStyle tr td.sp { color: #c00; font-weight: bold; }
      .tbl_title.alt { text-transform: none; font-weight: normal; color: #f00; }

      .tableStyle .altYES td { background: #B7CFAF }
      .tableStyle .YES td { background: #A9C69F }

      .tableStyle .YES td, .tableStyle .altYES td { font-weight: bold; }
      .tableStyle .YES .answer, .tableStyle .altYES .answer { font-size: 11pt; }
      .notice, .scheduleNotice { font-size: 12pt; font-weight: bold; line-height: normal; }
      .scheduleNotice { border: dotted 2px #ccc; margin: 10px auto; padding: 10px; text-align: center; width: 95%; }
   </style>
</head>
<body>

   %header%
   <table class="tableStyle" cellspacing="0">
      <thead>
         <tr>
            <th colspan="6" class="tbl_title">PERSONAL INFORMATION</th>
         </tr>

         <tr>
            <th>NAME</th>
            <th>SPOUSE/PARTNER</th>
            <th>ADDRESS</th>
            <th>PHONE NUMBER</th>
            <th>EMAIL ADDRESS</th>
            <th>ORIENTATION DATE</th>
            <th>LIVE SCAN DATE</th>
            <th>DCFS LOCATION</th>
            <th>QUESTIONS</th>
         </tr>
      </thead>
      <tbody>
         <tr>
            <td nowrap="nowrap">%name%</td>
            <td nowrap="nowrap">%spouse%<br /></td>
            <td nowrap="nowrap" class="align_left">%address%</td>
            <td nowrap="nowrap">%phone%</td>
            <td nowrap="nowrap">%email%<br /></td>
            <td nowrap="nowrap">%orientation_date%<br /></td>
            <td nowrap="nowrap">%livescan_date%</td>
            <td nowrap="nowrap">%dcfslocation%</td>
            <td>%questions%<br /></td>
         </tr>
      </tbody>
   </table>

   <div class="notice">
      <p>
         The first step in becoming a Resource (foster/adoptive) Parent is to attend orientation.
         If you have not completed this step, please call 888-811-1121 to register.
         Once this is complete, the next step is to attend a PS-MAPP Training consisting
         of 33 hours during six (6) consecutive weeks.
         We offer evening and Saturday series throughout Los Angeles County.
         They are in Spanish or English.
      </p>
   </div>

   <asp:ListView ID="lstSchedule" runat="server" DataSourceID="odsSchedule">
      <ItemTemplate>
         <table class="tableStyle" cellspacing="0">
            <thead>
               <tr>
                  <th colspan="7" class="tbl_title">
                     <asp:HiddenField ID="hidXPath" runat="server" Visible="false" Value='<%#Eval("XPathAddress")%>' />
                     <%#reformatTitle(Eval("Title"), Eval("MonthAndYear"))%>
                  </th>
               </tr>

               <tr>
                  <th>STARTS</th>
                  <th>MANDATORY CLASS DATES</th>
                  <th>SKIP</th>
                  <th>ENDS</th>
                  <th>DAYS</th>
                  <th>hours</th>
                  <th>CITY</th>
                  <th>ENROLL FOR THIS CLASS</th>
               </tr>
            </thead>

            <tbody>
               <asp:ListView ID="lstClasses" runat="server" DataSourceID="odsClasses">
                  <ItemTemplate>
                     <tr id="trClasses" runat="server" class="<%#getEnrollClassName(Container.DataItemIndex)%>">
                        <td><%#Eval("Start")%></td>
                        <td><%#Eval("MandatoryDates")%></td>
                        <td><%#Eval("SkipDates")%></td>
                        <td><%#Eval("End")%></td>
                        <td><%#Eval("Days")%></td>
                        <td><%#Eval("Time")%></td>
                        <td><%#Eval("Area")%></td>
                        <td><%#getEnrollFormat(Eval("Filled"))%></td>
                     </tr>
                  </ItemTemplate>

                  <EmptyDataTemplate>
                     <tr class="altNo">
                        <td colspan="7" class="notice">
                           There are no classes within this series.
                        </td>
                     </tr>
                  </EmptyDataTemplate>
               </asp:ListView>
            </tbody>
         </table>
         <br />

         <asp:ObjectDataSource ID="odsClasses" runat="server" TypeName="onlineRegistration_app.BusinessLayer.ScheduleBLL"
            SelectMethod="SelectClasses">
            <SelectParameters>
               <asp:ControlParameter Name="parentXPath" ControlID="hidXPath" PropertyName="Value" Type="String" />
            </SelectParameters>
         </asp:ObjectDataSource>
      </ItemTemplate>
   </asp:ListView>

   <asp:ObjectDataSource ID="odsSchedule" runat="server" TypeName="onlineRegistration_app.BusinessLayer.ScheduleBLL"
      SelectMethod="SelectSeriesVisible" />

</body>
</html>
