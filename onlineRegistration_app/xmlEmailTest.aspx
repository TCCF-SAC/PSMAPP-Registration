<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xmlEmailTest.aspx.cs" Inherits="onlineRegistration_app.xmlEmailTest" %>

<%@ Register TagPrefix="mn" TagName="EmailFormat" Src="~/EmailFormat.ascx" %>

<asp:Literal ID="litTest" runat="server" Visible="false" />
<mn:EmailFormat runat="server" />
