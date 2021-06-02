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
    public partial class CreateTeam : System.Web.UI.Page
    {
        DBTool dbTool = new DBTool();
        protected void Page_Load(object sender, EventArgs e)
        {
            LogInfo Info = (LogInfo)Session["IsLogined"];
            Image1.ImageUrl = TeamPicture_DropDownList.SelectedValue;
            string[] colname = { "StoreName", "StoreID" }; //DataTable的欄位名稱
            DataTable StoreName = dbTool.readTable("Stores", colname, "WHERE DeleteDate IS NULL AND WhoDelete IS NULL", null, null);  //第一個null為DB內參數欄位名稱，第二個為傳入比對之變數
            List<string> Storenum = new List<string>();
            foreach (DataRow item in StoreName.Rows) //Rows表一列
            {
                Store_DropDownList.Items.Add(new ListItem()
                {
                    Text = item["StoreName"].ToString(),
                    Value = item["StoreID"].ToString()
                });
            }
        }

        protected void OK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Team_Name.Text))
            {
                LogInfo Info = (LogInfo)Session["IsLogined"];
                string TeamName = Team_Name.Text;
                string StoreName = Store_DropDownList.SelectedValue;
                string TeamPicture = TeamPicture_DropDownList.SelectedValue;

                string[] colname = { "TeamName", "Status", "StoreID", "UserID", "PictureFile", "CreateDate", "WhoCreate" };
                string[] colnamep = { "@TeamName", "@Status", "@StoreID", "@UserID", "@PictureFile", "@CreateDate", "@WhoCreate" };
                List<string> p = new List<string>();
                p.Add(TeamName);
                p.Add("未結團");
                p.Add(Store_DropDownList.SelectedValue);
                p.Add(Info.UserID.ToString());
                p.Add(TeamPicture_DropDownList.SelectedValue);
                p.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                p.Add("Manager");
                dbTool.InsertTable("Teams", colname, colnamep, p);
                Response.Redirect("Index.aspx");
            }
            else 
            {
                Team_ErrorMessage.Visible = true;
                Team_ErrorMessage.Text = "請輸入團名";
            }
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            Team_Name.Text = string.Empty;
            Store_DropDownList.SelectedIndex = 0;
            TeamPicture_DropDownList.SelectedIndex = 0;
            Image1.ImageUrl = TeamPicture_DropDownList.SelectedValue;
        }

        protected void TeamPicture_DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Image1.ImageUrl = TeamPicture_DropDownList.SelectedValue;
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("Index.aspx");
        }
    }
}