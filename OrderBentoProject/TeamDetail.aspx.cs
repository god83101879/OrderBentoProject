using OrderBentoProject.Models;
using OrderBentoProject.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OrderBentoProject
{
    public partial class TeamDetail : System.Web.UI.Page
    {
        static List<OrderingDish> dishdiction = new List<OrderingDish>();
        DBTool dbTool = new DBTool();
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable teamdata = new DataTable();
            if (!IsPostBack)
            {
                LogInfo info = Session["IsLogined"] as LogInfo;

                if (Status_DropDownList.SelectedValue == "已到")
                {
                    Response.Redirect("~/Index.aspx");
                }

                string teamID = Request.QueryString["id"];
                if (!string.IsNullOrWhiteSpace(teamID))
                {
                    string[] teamcolname = { "StoreID", "PictureFile", "Status", "UserID", "TeamName" };
                    string[] teamcolnamep = { "@TeamID" };
                    string[] teamp = { teamID };

                    string teamlogic = @"WHERE WhoDelete IS NULL AND TeamID=@TeamID";
                    teamdata = dbTool.readTable("Teams", teamcolname, teamlogic, teamcolnamep, teamp);
                    if (teamdata.Rows.Count > 0)
                    {
                        Team_Image.ImageUrl = teamdata.Rows[0]["PictureFile"].ToString();
                        if(info != null)
                        {
                            if (info.UserID == Convert.ToInt32(teamdata.Rows[0]["UserID"]))
                            {
                                Status_DropDownList.SelectedValue = teamdata.Rows[0]["Status"].ToString();
                                IsUser.Visible = false;
                                IsConvener.Visible = true;
                                LogIndiv.Visible = true;
                                OrderCount_Rep.Visible = true;
                                ImageUser.ImageUrl = info.UserPic;
                            }
                            else
                            {
                                TeamStatus_ltMessage.Text = teamdata.Rows[0]["Status"].ToString();
                                IsUser.Visible = true;
                                IsConvener.Visible = false;
                                LogIndiv.Visible = true;
                                OrderCount_Rep.Visible = false;
                                ImageUser.ImageUrl = info.UserPic;
                            }
                        }
                        else
                        {
                            TeamStatus_ltMessage.Text = teamdata.Rows[0]["Status"].ToString();
                            IsUser.Visible = true;
                            IsConvener.Visible = false;
                            LogIndiv.Visible = true;
                            OrderCount_Rep.Visible = false;
                        }

                        string[] Concolname = { "Name" };
                        string[] Concolnamep = { "@UserID" };
                        string[] Conp = { teamdata.Rows[0]["UserID"].ToString() };

                        string Conlogic = @"
                            WHERE DeleteDate IS NULL AND UserID=@UserID
                            ";
                        DataTable Condata = dbTool.readTable("Users", Concolname, Conlogic, Concolnamep, Conp);
                        if (Condata.Rows.Count > 0)
                        {
                            MainName_lt.Text = Condata.Rows[0]["Name"].ToString();
                        }


                        string[] storecolname = { "StoreName" };
                        string[] storecolnamep = { "@StoreID" };
                        string[] storep = { teamdata.Rows[0]["StoreID"].ToString() };

                        string storelogic = @"
                            WHERE DeleteDate IS NULL AND StoreID=@StoreID
                            ";
                        DataTable storedata = dbTool.readTable("Stores", storecolname, storelogic, storecolnamep, storep);
                        if (storedata.Rows.Count > 0)
                        {
                            Store_ltMessage.Text = storedata.Rows[0]["StoreName"].ToString();
                        }

                        string[] dishcolname = { "DishID", "DishName", "DishPrice", "DishPic" };
                        string[] dishcolnamep = { "@StoreID" };
                        string[] dishp = { teamdata.Rows[0]["StoreID"].ToString() };

                        string dishlogic = @"
                            WHERE StoreID=@StoreID
                            ";
                        DataTable dishdata = dbTool.readTable("Dishes", dishcolname, dishlogic, dishcolnamep, dishp);
                        Dish_Rep.DataSource = dishdata;
                        Dish_Rep.DataBind();




                        string[] orderusercolname = { "Orders.UserID", "Users.Name", "Users.UserPic" };
                        string[] orderusercolnamep = { "@TeamID" };
                        string[] orderuserp = { teamID };

                        string orderuserlogic = @"
                            JOIN Users ON Orders.UserID=Users.UserID
                            WHERE Orders.DeleteDate IS NULL AND Orders.TeamID=@TeamID
                            GROUP BY Orders.UserID, Users.Name, Users.UserPic
                            ";
                        DataTable orderuserdata = dbTool.readTable("Orders", orderusercolname, orderuserlogic, orderusercolnamep, orderuserp);
                        Order_Rep.DataSource = orderuserdata;
                        Order_Rep.DataBind();




                        string[] ordercolname = { "Orders.DishID", "Dishes.DishName", "Orders.OrderNum", "Dishes.DishPrice" };
                        string[] ordercolnamep = { "@TeamID" };
                        string[] orderp = { teamID };

                        string orderlogic = @"
                            JOIN Dishes ON Orders.DishID=Dishes.DishID
                            WHERE Orders.DeleteDate IS NULL AND Orders.TeamID=@TeamID
                            ORDER BY Orders.DishID
                            ";
                        DataTable orderdata = dbTool.readTable("Orders", ordercolname, orderlogic, ordercolnamep, orderp);
                        List<OrderingDish> allorder = new List<OrderingDish>();
                        decimal totalprice = 0;
                        foreach (DataRow item in orderdata.Rows)
                        {
                            if (allorder.FindIndex(obj => obj.DishID == Convert.ToInt32(item["DishID"])) == -1)
                            {
                                OrderingDish order = new OrderingDish();
                                order.DishID = Convert.ToInt32(item["DishID"]);
                                order.DishName = item["DishName"].ToString();
                                order.DishNum = Convert.ToInt32(item["OrderNum"]);
                                allorder.Add(order);
                                totalprice += Convert.ToInt32(item["DishPrice"]) * Convert.ToInt32(item["OrderNum"]);
                            }
                            else
                            {
                                allorder[allorder.FindIndex(obj => obj.DishID == Convert.ToInt32(item["DishID"]))].DishNum += Convert.ToInt32(item["OrderNum"]);
                                totalprice += Convert.ToInt32(item["DishPrice"]) * Convert.ToInt32(item["OrderNum"]);
                            }
                        }

                        TotalPrice.Value = totalprice.ToString();
                        OrderCount_Rep.DataSource = allorder;
                        OrderCount_Rep.DataBind();

                    }
                }
            }
            if (Status_DropDownList.SelectedValue == "結團")
            {
                Status_DropDownList.Items[0].Enabled = false;
                LogIndiv.Visible = false;
            }
            else if (Status_DropDownList.SelectedValue == "已到")
            {
                Status_DropDownList.Enabled = false;
            }
            Error_ltMessage.Text = "";
        }

        protected void Order_Rep_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LogInfo info = Session["IsLogined"] as LogInfo;
                Repeater rel = e.Item.FindControl("OrderDish_Rep") as Repeater;
                Button btn = e.Item.FindControl("Kick_btn") as Button;

                string teamid = Request.QueryString["id"];
                string[] teamcolname = { "StoreID", "PictureFile", "Status", "UserID", "TeamName" };
                string[] teamcolnamep = { "@TeamID" };
                string[] teamp = { teamid };

                string teamlogic = @"
                            WHERE DeleteDate IS NULL AND TeamID=@TeamID
                            ";
                DataTable teamdata = dbTool.readTable("Teams", teamcolname, teamlogic, teamcolnamep, teamp);
                if (info != null)
                {
                    if (info.UserID == Convert.ToInt32(teamdata.Rows[0]["UserID"]) && Status_DropDownList.SelectedValue == "未結團")
                    {
                        btn.Visible = true;
                    }
                    else
                    {
                        btn.Visible = false;
                    }
                }
                else
                {
                    btn.Visible = false;
                }

                string userid = btn.CommandArgument.ToString();

                if (!string.IsNullOrWhiteSpace(userid))
                {
                    string[] orderusercolname = { "Orders.DishID", "Dishes.DishName", "Orders.OrderNum" };
                    string[] orderusercolnamep = { "@UserID", "@TeamID" };
                    string[] orderuserp = { userid, teamid };

                    string orderuserlogic = @"
                            JOIN Dishes ON Orders.DishID=Dishes.DishID
                            WHERE Orders.DeleteDate IS NULL AND Orders.UserID=@UserID AND Orders.TeamID=@TeamID
                            ";
                    DataTable orderuserdata = dbTool.readTable("Orders", orderusercolname, orderuserlogic, orderusercolnamep, orderuserp);

                    List<OrderingDish> allorder = new List<OrderingDish>();

                    foreach (DataRow item in orderuserdata.Rows)
                    {
                        if (allorder.FindIndex(obj => obj.DishID == Convert.ToInt32(item["DishID"])) == -1)
                        {
                            OrderingDish order = new OrderingDish();
                            order.DishID = Convert.ToInt32(item["DishID"]);
                            order.DishName = item["DishName"].ToString();
                            order.DishNum = Convert.ToInt32(item["OrderNum"]);
                            allorder.Add(order);
                        }
                        else
                        {
                            allorder[allorder.FindIndex(obj => obj.DishID == Convert.ToInt32(item["DishID"]))].DishNum += Convert.ToInt32(item["OrderNum"]);
                        }
                    }

                    rel.DataSource = allorder;
                    rel.DataBind();
                }

            }
        }

        protected void BackListbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Index.aspx");
        }

        protected void DishNum_DDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dishchoose = sender as DropDownList;
            string[] dishidandname = dishchoose.ToolTip.Split(',');

            if (dishdiction.FindIndex(obj => obj.DishName == dishidandname[0]) == -1)
            {
                OrderingDish orderingDish = new OrderingDish();
                orderingDish.DishID = Convert.ToInt32(dishidandname[1]);
                orderingDish.DishName = dishidandname[0];
                orderingDish.DishNum = Convert.ToInt32(dishchoose.SelectedValue);
                dishdiction.Add(orderingDish);
            }
            else
            {
                dishdiction[dishdiction.FindIndex(obj => obj.DishName == dishidandname[0])].DishNum += Convert.ToInt32(dishchoose.SelectedValue);
            }
            Ordering_Rep.DataSource = dishdiction;
            Ordering_Rep.DataBind();
            dishchoose.SelectedIndex = 0;
        }

        protected void Dish_Rep_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PlaceHolder dishchoose = e.Item.FindControl("DishNum_PlaceHolder") as PlaceHolder;
                LogInfo info = Session["IsLogined"] as LogInfo;
                if (info != null && Status_DropDownList.SelectedValue == "未結團")
                {
                    dishchoose.Visible = true;

                }
                else
                {
                    dishchoose.Visible = false;
                }

            }
        }

        protected void OrderResetbtn_Click(object sender, EventArgs e)
        {
            string teamid = Request.QueryString["id"];

            Error_ltMessage.Text = "";
            dishdiction.Clear();
            Response.Redirect($"~/TeamDetail.aspx?id={teamid}");
        }

        protected void OrderOKbtn_Click(object sender, EventArgs e)
        {
            if (dishdiction.Count == 0)
            {
                Error_ltMessage.Text = "請選擇至少一項";
            }
            else
            {
                LogInfo info = Session["IsLogined"] as LogInfo;
                string teamid = Request.QueryString["id"];

                string[] colname = { "TeamID", "UserID", "DishID", "OrderNum", "WhoCreate", "CreateDate" };
                string[] colnamep = { "@TeamID", "@UserID", "@DishID", "@OrderNum", "@WhoCreate", "@CreateDate" };
                List<string> p = new List<string>();
                foreach (OrderingDish item in dishdiction)
                {
                    p.Add(teamid);
                    p.Add(info.UserID.ToString());
                    p.Add(item.DishID.ToString());
                    p.Add(item.DishNum.ToString());
                    p.Add(info.UserID.ToString());
                    p.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                dbTool.InsertTable("Orders", colname, colnamep, p);
                dishdiction.Clear();

                Response.Redirect($"~/TeamDetail.aspx?id={teamid}");
            }
        }

        protected void Kickbtn_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            LogInfo info = Session["IsLogined"] as LogInfo;
            string teamid = Request.QueryString["id"];


            string[] updatecol_Logic = { "WhoDelete=@WhoDelete", "DeleteDate=@DeleteDate" }; 
            string Where_Logic = "UserID=@UserID AND TeamID=@TeamID";
            string[] updatecolname_P = { "@WhoDelete", "@DeleteDate", "@UserID", "@TeamID" }; 
            List<string> update_P = new List<string>();
            update_P.Add(info.UserID.ToString());
            update_P.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            update_P.Add(button.CommandArgument);
            update_P.Add(teamid);
            dbTool.UpdateTable("Orders", updatecol_Logic, Where_Logic, updatecolname_P, update_P);

            Response.Redirect($"~/TeamDetail.aspx?id={teamid}");
        }

        protected void OrderCount_Rep_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                Literal dishchoose = e.Item.FindControl("LitTotalPrice") as Literal;
                if (dishchoose != null)
                {
                    dishchoose.Text = TotalPrice.Value;
                }

            }
        }

        protected void ChangeStatusbtn_Click(object sender, EventArgs e)
        {
            LogInfo info = Session["IsLogined"] as LogInfo;
            string teamid = Request.QueryString["id"];

            if (Status_DropDownList.SelectedValue == "已到")
            {
                string[] updatecol_Logic = { "Status=@Status", "WhoDelete=@WhoDelete", "DeleteDate=@DeleteDate" }; /*  要更新的欄位*/
                string Where_Logic = "TeamID=@TeamID AND DeleteDate IS NULL";
                string[] updatecolname_P = { "@Status", "@WhoDelete", "@DeleteDate", "@TeamID" }; /*要帶入的參數格子*/
                List<string> update_P = new List<string>();
                update_P.Add(Status_DropDownList.SelectedValue);
                update_P.Add(info.UserID.ToString());
                update_P.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                update_P.Add(teamid);
                dbTool.UpdateTable("Teams", updatecol_Logic, Where_Logic, updatecolname_P, update_P);
                Response.Redirect($"~/Index.aspx");

            }
            else
            {
                string[] updatecol_Logic = { "Status=@Status", "WhoCreate=@WhoCreate", "CreateDate=@CreateDate" }; /*  要更新的欄位*/
                string Where_Logic = "TeamID=@TeamID AND DeleteDate IS NULL";
                string[] updatecolname_P = { "@Status", "@WhoCreate", "@CreateDate", "@TeamID" }; /*要帶入的參數格子*/
                List<string> update_P = new List<string>();
                update_P.Add(Status_DropDownList.SelectedValue);
                update_P.Add(info.UserID.ToString());
                update_P.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                update_P.Add(teamid);
                dbTool.UpdateTable("Teams", updatecol_Logic, Where_Logic, updatecolname_P, update_P);
                Response.Redirect($"~/TeamDetail.aspx?id={teamid}");
            }
        }
        private class OrderingDish
        {
            public int DishID { get; set; }
            public string DishName { get; set; }
            public int DishNum { get; set; }
        }
    }
}