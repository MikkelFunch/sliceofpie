<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginControl.ascx.cs" Inherits="SliceOfPieWeb.WebUserControl1" %>
<asp:Label ID="LabelUser" runat="server" Text="Username"></asp:Label>
<asp:TextBox ID="TextBoxUser" runat="server"></asp:TextBox>
<br />
<asp:Label ID="LabelPass" runat="server" Text="Password"></asp:Label>
<asp:TextBox ID="TextBoxPass" runat="server"></asp:TextBox>

<br />

<asp:Button ID="ButtonSubmit" runat="server" Text="Submit" 
    onclick="ButtonSubmit_Click" />
<asp:Button ID="ButtonRegister" runat="server" Text="Register" 
    onclick="ButtonRegister_Click" />



