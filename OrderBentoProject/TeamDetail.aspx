<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeamDetail.aspx.cs" Inherits="OrderBentoProject.TeamDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="assets/css/bootstrap.css" rel="stylesheet" />
    <script src="assets/css/bootstrap.js"></script>
</head>
<body>
    <form runat="server">
    <asp:HiddenField ID="TotalPrice" runat="server" />
    <div class="container">
        <div class="col-12 row">
            <div class="col-4">
                <div class="col-12">
                    <asp:Image ID="Team_Image" runat="server" Height="250" Width="250" />
                </div>
                <div class="col-12">
                    <div style="text-align: center; font-size: 50px">
                        <asp:Literal ID="Store_ltMessage" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
            <div class="col-8 row">
                <div class="col-12">
                    狀態:<div runat="server" id="IsUser">
                        <asp:Literal ID="TeamStatus_ltMessage" runat="server"></asp:Literal>
                    </div>
                    <div runat="server" id="IsConvener">
                        <asp:DropDownList ID="Status_DropDownList" runat="server">
                            <asp:ListItem>未結團</asp:ListItem>
                            <asp:ListItem>結團</asp:ListItem>
                            <asp:ListItem>已到</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Button ID="ChangeStatus_btn" runat="server" Text="更換狀態" OnClick="ChangeStatusbtn_Click" OnClientClick="return confirm('確定更改？');" />
                    </div>
                </div>
                <div class="col-12">
                    主揪:<asp:Literal ID="MainName_lt" runat="server"></asp:Literal>
                    <div class="row" style="border: solid black 2px">
                        <asp:Repeater ID="OrderCount_Rep" runat="server" OnItemDataBound="OrderCount_Rep_ItemDataBound">
                            <HeaderTemplate>
                                小記:NT$<asp:Literal ID="LitTotalPrice" runat="server"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="col-12 row">
                                    <div class="col-6">
                                        名稱: <%#Eval("DishName") %>
                                    </div>
                                    <div class="col-6">
                                        數量: <%#Eval("DishNum") %>個
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
        <div style="height: 50px"></div>
        <h4 style="text-align: center">菜單</h4>
        <div class="row">
            <asp:Repeater ID="Dish_Rep" runat="server" OnItemDataBound="Dish_Rep_ItemDataBound">
                <ItemTemplate>
                    <div class="col-12 col-md-4" style="border: solid black 2px;">
                        <div class="row col-12">
                            <div class="col-6">
                                <img src='<%#Eval("DishPic") %>' width="100" height="100" />
                            </div>
                            <div class="col-6">
                                <div class="col-12">名稱: <%#Eval("DishName") %></div>
                                <div class="col-12">價格: NT<%#Eval("DishPrice") %></div>
                                <div class="col-12">
                                    <asp:PlaceHolder ID="DishNum_PlaceHolder" runat="server">數量:<asp:DropDownList ID="DishNum_DropDownList" runat="server"
                                        ToolTip='<%#Eval("DishName")+","+Eval("DishID")  %>'
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="DishNum_DDL_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Value="0">請選擇數量</asp:ListItem>
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                    </asp:DropDownList>
                                    </asp:PlaceHolder>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div style="height: 50px"></div>
        <h4 style="text-align: center">訂單</h4>
        <div class="row">
            <asp:Repeater ID="Order_Rep" runat="server" OnItemDataBound="Order_Rep_ItemDataBound">
                <ItemTemplate>
                    <div class="col-12 col-md-4">
                        <div class="col-2">
                            <asp:Button ID="Kick_btn" runat="server" Text="X" CommandName="KickPeople"
                                CommandArgument='<%#Eval("UserID") %>' OnClick="Kickbtn_Click" OnClientClick="return confirm('確定剔除？');" />
                        </div>
                        <div class="col-4">
                            <img src='<%#Eval("UserPic") %>' width="50" height="50" />
                            <%#Eval("Name") %>
                        </div>
                        <div class="col-6">
                            <asp:Repeater ID="OrderDish_Rep" runat="server">
                                <ItemTemplate>
                                    <div class="col-12"><%#Eval("DishName") %> X <%#Eval("DishNum") %></div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div style="height: 250px"></div>
        <div id="LogIndiv" runat="server" style="border: solid black 2px;">
            <div class="col-12 row">
                <div class="col-12 row justify-content-center">
                    <div class="col-3">
                        <asp:Image ID="ImageUser" runat="server" Width="100" Height="100" />
                    </div>
                    <div class="col-9">
                        <asp:Repeater ID="Ordering_Rep" runat="server">
                            <ItemTemplate>
                                <div class="col-12 row">
                                    <div class="col-4">
                                        <%#Eval("DishName") %>
                                    </div>
                                    <div class="col-1" style="border: solid black 1px;">
                                        <%#Eval("DishNum") %>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div style="height: 50px"></div>
                <div class="col-8 row justify-center-end">
                    <asp:Button ID="OrderOK_btn" runat="server" Text="OK" OnClick="OrderOKbtn_Click" OnClientClick="return confirm('確定送出？');" />
                    <asp:Button ID="OrderReset_btn" runat="server" Text="Reser" OnClick="OrderResetbtn_Click" />
                    <span class="errorMessage">
                        <asp:Literal ID="Error_ltMessage" runat="server"></asp:Literal>
                    </span>
                </div>
            </div>
        </div>
    </div>
    <div style="height: 50px"></div>
    <div class="container">
        <div class="row justify-content-end">
            <asp:Button ID="BackList_btn" runat="server" Text="返回列表" OnClick="BackListbtn_Click"/>
        </div>
    </div>
    <div style="height: 50px"></div>
    </form>
</body>
</html>
