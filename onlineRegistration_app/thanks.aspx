<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="thanks.aspx.cs" Inherits="onlineRegistration_app.thanks" Theme="default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   <title>Thank you!</title>
</head>
<body>
   <form id="form1" runat="server" class="formWrap">
   <div class="formWrapMain" style="text-align: center;">
      <h1>Your submission has been sent.</h1>

      <p><asp:Literal runat="server" Text="<%$ appSettings:Thanks_Message %>" /></p>

      <p align="left">
         <a href="http://www.communitycollege.org/ps-mapp/about-ps-mapp/"
            title="PS-MAPP">Back to PS-MAPP homepage</a>
         <br />
         <a href="http://www.communitycollege.org/"
            title="The Community College Foundation">Back to The Community College Foundation homepage</a>
      </p>
   </div>
   </form>
</body>
</html>
