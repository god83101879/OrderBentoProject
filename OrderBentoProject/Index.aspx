<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="OrderBentoProject.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="assets/css/bootstrap.css" rel="stylesheet" />
    <script src="assets/css/bootstrap.min.js"></script>
    <script src="assets/js/jquery-3.6.0.min.js"></script>
    <style>
        .Line {
            border: 1px solid;
        }

        .Area {
            border: 0.1px solid;
        }

        .innertitle {
            background-color: #808080;
            color: #ffffff;
        }

        .centertitle {
            text-align: center;
        }
    </style>
</head>
<body>
    <div style="color: blue">
        <asp:Label ID="User_Message" runat="server" Text="" Visible="false"></asp:Label>
    </div>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="SearchBox" runat="server"></asp:TextBox>
            <asp:DropDownList ID="DropDownList1" runat="server">
                <asp:ListItem Value="團名" Text="團名"></asp:ListItem>
                <asp:ListItem Value="店名" Text="店名"></asp:ListItem>
                <asp:ListItem Value="菜色" Text="菜色"></asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="Button3" runat="server" Text="Search" OnClick="Search_Click" />
            <asp:Button ID="Button1" runat="server" Text="登入" OnClick="ToLogin_Click" />
            <asp:Button ID="Button4" runat="server" Text="登出" OnClick="Logout_Click"></asp:Button>
            <asp:Button ID="Button2" runat="server" Text="開團" OnClick="CreateTeam_Click" />
            <div style="color: red">
                <asp:Label ID="Error_Team" runat="server" Text="" Visible="false"></asp:Label>
            </div>
        </div>
        <br />
        <br />
        <div id="Nothingdiv" class="col-12" runat="server" style="font-size: 120px">
            無       
        </div>
        <div style="height: 50px"></div>
        <div class="row">
            <asp:Repeater ID="Team_Rep" runat="server" OnItemDataBound="TeamRep_ItemDataBound">
                <ItemTemplate>
                    <div class="col-12 col-md-6" style="border: solid black 2px;">
                        <a href="./TeamDetail.aspx?id=<%#Eval("TeamID") %>">
                            <div class="row col-12">
                                <div class="col-3">
                                    <img src="<%#Eval("PictureFIle") %>" width="100" height="100" />
                                </div>
                                <div class="col-9 row">
                                    <div class="col-6">
                                        團名：<%#Eval("TeamName") %>
                                    </div>
                                    <div class="col-6">
                                        主揪：<%#Eval("Name") %>
                                    </div>
                                    <div class="col-6">
                                        店名：<%#Eval("StoreName") %>
                                    </div>
                                    <div class="col-6">
                                        目前人數：<%#Eval("peoplenum") %>
                                    </div>
                                    <div class="col-12">
                                        <%#Eval("Status") %>
                                    </div>
                                </div>
                            </div>
                            <div style="height: 50px"></div>
                            <asp:HiddenField ID="HFDsihStore" runat="server" Value='<%#Eval("StoreID") %>' />
                            <div class="row col-12">
                                <asp:Repeater ID="Rep_Indexdish" runat="server">
                                    <ItemTemplate>
                                        <div class="col-4"><%#Eval("DishName") %></div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <div style="height: 50px"></div>
                        </a>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div class="row">
            <asp:HyperLink ID="HLfirst" runat="server" ToolTip="前往第一頁">First</asp:HyperLink>｜
           
            <asp:Repeater runat="server" ID="repPaging">
                <ItemTemplate>
                    <a href="<%# Eval("Link") %>" title="<%# Eval("Title") %>"><%# Eval("Name") %></a>｜
               
                </ItemTemplate>
            </asp:Repeater>
            <asp:HyperLink ID="HLlast" runat="server" ToolTip="前往尾頁">Last</asp:HyperLink>
        </div>
    </form>
</body>
</html>
