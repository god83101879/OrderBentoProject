<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="OrderBentoProject.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <h1 style="color:blue">訂便當系統</h1>
    <form id="form1" runat="server">
        <div>
            帳號建立:<asp:TextBox ID="C_Account" runat="server"></asp:TextBox><br />
            密碼建立:<asp:TextBox ID="C_Password" runat="server"></asp:TextBox><br />
            <asp:Button ID="Button1" runat="server" Text="登入" OnClick="Login_Click" style="height: 27px" />
            <div style="color:red">
            <asp:Label ID="Login_Message" runat="server" Text="Label" Visible="false"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
