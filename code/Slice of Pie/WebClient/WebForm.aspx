<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm.aspx.cs" Inherits="WebClient.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="Button1" runat="server" Text="Register User" OnClick="RegisterNewUser"/>
    
        <asp:TreeView ID="TreeView" runat="server" OnLoad="PopulateTreeView">
        </asp:TreeView>
        <asp:TextBox ID="TextBox" runat="server" Height="253px" Width="449px"></asp:TextBox>
    
    </div>
    </form>
</body>
</html>
