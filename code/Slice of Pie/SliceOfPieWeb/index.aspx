<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="SliceOfPieWeb.index" %>


<%@ Register src="Parts/LoginControl.ascx" tagname="LoginControl" tagprefix="uc2" %>

<%@ Register src="Parts/LoggedIn.ascx" tagname="LoggedIn" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="Styles/Style.css" rel="stylesheet" type="text/css" />

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="page">
        <div class="header">
            <div class="title">
                <h1>
                    <asp:Label ID="LabelTitle" runat="server" Text="LabelTitle"></asp:Label>
                </h1>
            </div>
            <div class="login">
            <%if (Session["online"].Equals((Object)true))
              { %>
                    <uc1:LoggedIn ID="LoggedIn1" runat="server" />
              <%}
              else
              {%>
                    <uc2:LoginControl ID="LoginControl" runat="server" />
              <%}%>
            </div>
        </div>
        <div class="menubar">
            <asp:Label ID="LabelMenu" runat="server" Text="LabelMenu"></asp:Label>
        </div>
        <div class="main">
            <div class="filesview">
                <asp:Label ID="LabelFileView" runat="server" Text="LabelFileView"></asp:Label>
            </div>
            <div class="editview">
                <asp:Label ID="LabelEdit" runat="server" Text="LabelEdit"></asp:Label>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
