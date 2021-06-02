<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateTeam.aspx.cs" Inherits="OrderBentoProject.CreateTeam" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            團名<asp:TextBox ID="Team_Name" runat="server"></asp:TextBox> <br />
            店名<asp:DropDownList ID="Store_DropDownList" runat="server">
              </asp:DropDownList><br /><br /><br />
            <%--<asp:Image ID="Image1" runat="server" />&emsp;&emsp;<asp:FileUpload ID="FileUpload1" runat="server" />--%>
            <asp:Image ID="Image1" runat="server" Height="100" Width="100"/>
            團圖<asp:DropDownList ID="TeamPicture_DropDownList" AutoPostBack="true" runat="server" OnSelectedIndexChanged="TeamPicture_DropDownList_SelectedIndexChanged">
                        <asp:listitem value="PictureFIle/21K.jpg" text="團圖1"></asp:listitem>
                        <asp:listitem value="PictureFIle/BafangYunji.jpg" text="團圖2"></asp:listitem>
                        <asp:listitem value="PictureFIle/BraisedDishes.jpg" text="團圖3"></asp:listitem>
                        <asp:listitem value="PictureFIle/Grandpa.jpg" text="團圖4"></asp:listitem>
                        <asp:listitem value="PictureFIle/TAISHRFU.png" text="團圖5"></asp:listitem>
              </asp:DropDownList><br /><br /><br />
            <br /><br /><br />
            <asp:Button ID="Button1" runat="server" Text="OK" onclick="OK_Click"/>&emsp;&emsp;
            <asp:Button ID="Button2" runat="server" Text="Reset" onclick="Reset_Click"/>&emsp;&emsp;
            <asp:Button ID="Button3" runat="server" Text="Back" OnClick="Back_Click" />
            <asp:Label ID="Team_ErrorMessage" runat="server" Text="" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>
