<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Backstage_AccountManage.aspx.cs" Inherits="OrderBentoProject.Backstage_AccountManage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div >
            名稱:&emsp;&emsp;<asp:TextBox ID="C_Name" runat="server"></asp:TextBox> <br />
            帳號建立:<asp:TextBox ID="C_Account" runat="server"></asp:TextBox><br />
            密碼建立:<asp:TextBox ID="C_Password" runat="server"></asp:TextBox><br />
            權限選擇:<asp:DropDownList ID="Drop_P" runat="server">
                        <asp:listitem value="使用者" text="使用者"></asp:listitem>
                        <asp:listitem value="管理者" text="管理者"></asp:listitem>
                     </asp:DropDownList><br />
            <asp:Button ID="Button1" runat="server" Text="帳號建立" OnClick="Create_Click" />
            <div style="color:red">
            <asp:Label ID="C_Message" runat="server" Text="" Visible="false"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
