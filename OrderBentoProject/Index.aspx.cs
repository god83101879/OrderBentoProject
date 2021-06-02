using OrderBentoProject.Models;
using OrderBentoProject.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OrderBentoProject
{
    public partial class Index : System.Web.UI.Page
    {
        LoginHelper helper = new LoginHelper();
        DBTool dbTool = new DBTool();
        protected void Page_Load(object sender, EventArgs e)
        {
            //_goToUrl = Request.RawUrl; //轉回本頁
            //如不用static 需用new建立此實體物件 命名helper變數 使用LoginHelper方法儲存Session資料

            LogInfo Info = (LogInfo)Session["IsLogined"];
            // 檢查使用者資訊，如果已經有session 且符合資料庫內使用者資訊則直接轉至首頁
            if (helper.HasLogIned())
            {
                User_Message.Visible = true;
                User_Message.Text = $"歡迎{Info.Name}，請開始你的表演";
            }

            BuildDataTableCommit();
        }

        protected void ToLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void CreateTeam_Click(object sender, EventArgs e)
        {
            if (helper.HasLogIned())
            {
                Response.Redirect("CreateTeam.aspx");
            }
            else
            {
                Error_Team.Visible = true;
                Error_Team.Text = $"請先登入帳號再開團";
            }

        }
        protected void Search_Click(object sender, EventArgs e)
        {
            string name = this.SearchBox.Text;

            string template = $"?Page=1&Type={DropDownList1.SelectedValue}";

            if (!string.IsNullOrEmpty(name))
            {
                template += "&Name=" + name;
            }


            Response.Redirect("Index.aspx" + template);

        }
        protected void Logout_Click(object sender, EventArgs e)
        {
            LoginHelper helper = new LoginHelper();
            helper.Logout();
            Response.Redirect("~/Index.aspx");
        }

        public void BuildDataTableCommit()
        {
            string page = Request.QueryString["Page"];
            int pIndex;
            if (string.IsNullOrEmpty(page))
                pIndex = 1;
            else
            {
                int.TryParse(page, out pIndex);

                if (pIndex <= 0)
                    pIndex = 1;
            }
            string name = Request.QueryString["Name"];


            int totalSize;
            int _pageSize = 10;

            DataTable Comdata = readTableForPage(name, out totalSize, pIndex, _pageSize);
            List<IndexModel> lastresult = new List<IndexModel>();
            if (Comdata.Rows.Count > 0)
            {
                Nothingdiv.Visible = false;
                foreach (DataRow item in Comdata.Rows)
                {
                    IndexModel indexModel = new IndexModel();
                    indexModel.TeamID = Convert.ToInt32(item["TeamID"]);
                    indexModel.TeamName = item["TeamName"].ToString();
                    indexModel.PictureFile = item["PictureFile"].ToString();
                    indexModel.Status = item["Status"].ToString();
                    indexModel.Name = item["Name"].ToString();
                    indexModel.StoreName = item["StoreName"].ToString();
                    indexModel.StoreID = Convert.ToInt32(item["StoreID"]);

                    string[] uccolname = { "COUNT(UserID) as peoplenum" };
                    string[] uccolnamep = { "@TeamID" };
                    string[] ucp = { indexModel.TeamID.ToString() };

                    string uclogic = @"
                           (SELECT Teams.UserID
                            FROM Orders
                            JOIN Teams ON Orders.TeamID=Teams.TeamID
                            WHERE Orders.DeleteDate IS NULL AND Orders.TeamID=@TeamID
							GROUP BY Teams.UserID) as temp
                            ";
                    DataTable ucdata = dbTool.readTable(uclogic, uccolname, "", uccolnamep, ucp);
                    indexModel.peoplenum = Convert.ToInt32(ucdata.Rows[0]["peoplenum"]);

                    string[] dcolname = { "Dishes.DishName" };
                    string[] dcolnamep = { "@StoreID" };
                    string[] dp = { indexModel.StoreID.ToString() };

                    string dlogic = @"
                            JOIN Stores ON Dishes.StoreID=Stores.StoreID
                            WHERE Dishes.DeleteDate IS NULL AND Dishes.StoreID=@StoreID
                            ";
                    DataTable ddata = dbTool.readTable("Dishes", dcolname, dlogic, dcolnamep, dp);
                    List<IndexDish> indexDishes = new List<IndexDish>();
                    if (ddata.Rows.Count > 0)
                    {

                        foreach (DataRow ditem in ddata.Rows)
                        {
                            IndexDish indexDish = new IndexDish();
                            indexDish.DishName = ditem["DishName"].ToString();
                            indexDishes.Add(indexDish);
                        }

                    }
                    indexModel.Dishes = indexDishes;

                    lastresult.Add(indexModel);
                }
            }
            else
            {
                Nothingdiv.Visible = true;
            }

            int pages = CalculatePages(totalSize, _pageSize);
            List<PagingLink> pagingList = new List<PagingLink>();
            HLfirst.NavigateUrl = $"Index.aspx{this.GetQueryString(false, 1)}";
            HLlast.NavigateUrl = $"Index.aspx{this.GetQueryString(false, pages)}";
            for (var i = 1; i <= pages; i++)
            {
                pagingList.Add(new PagingLink()
                {
                    Link = $"Index.aspx{this.GetQueryString(false, i)}",
                    Name = $"{i}",
                    Title = $"前往第 {i} 頁"
                });
            }

            this.repPaging.DataSource = pagingList;
            this.repPaging.DataBind();

            Team_Rep.DataSource = lastresult;
            Team_Rep.DataBind();

        }

        private string GetQueryString(bool includePage, int? pageIndex)
        {
            //----- Get Query string parameters -----
            string page = Request.QueryString["Page"];
            string name = Request.QueryString["Name"];
            //----- Get Query string parameters -----


            List<string> conditions = new List<string>();

            if (!string.IsNullOrEmpty(page) && includePage)
                conditions.Add("Page=" + page);

            conditions.Add("Type=" + DropDownList1.SelectedValue);

            if (!string.IsNullOrEmpty(name))
                conditions.Add("Name=" + name);


            if (pageIndex.HasValue)
                conditions.Add("Page=" + pageIndex.Value);

            string retText =
                (conditions.Count > 0)
                    ? "?" + string.Join("&", conditions)
                    : string.Empty;

            return retText;
        }

        private string connectionString = "Data Source=localhost\\SQLExpress;Initial Catalog=BentoSystem; Integrated Security=true";
        public DataTable readTableForPage(string name, out int totalSize, int currentPage = 1, int pageSize = 9)
        {
            string searchcol;
            switch (DropDownList1.SelectedIndex)
            {
                case 0:
                    searchcol = "Teams.TeamName";
                    break;
                case 1:
                    searchcol = "Stores.StoreName";
                    break;
                default:
                    searchcol = "team_name";
                    break;
            }
            string queryString;
            if (DropDownList1.SelectedIndex != 2)
            {
                queryString =
                             $@" 
                                  SELECT TOP {pageSize} * FROM
                                  (
                                      SELECT 
                                          ROW_NUMBER() OVER(ORDER BY TeamID DESC) AS RowNumber,
                                           Teams.TeamID,
                                           Teams.TeamName, 
                                           Teams.PictureFile,
                                           Teams.Status,
                                           Users.Name,
				                 	      Stores.StoreName,
				                 		  Stores.StoreID
                                      FROM Teams
                                      JOIN Users ON Teams.UserID=Users.UserID
                                      JOIN Stores ON Teams.StoreID=Stores.StoreID
                                      WHERE Teams.DeleteDate IS NULL AND {searchcol} LIKE '%' + @name + '%'
                                  ) AS TempT
                                  WHERE RowNumber > {pageSize * (currentPage - 1)}
                                  ORDER BY TeamID DESC
                              ";
            }
            else
            {
                queryString =
                             $@" 
                                  SELECT TOP {pageSize} * FROM
                                  (
                                      SELECT 
                                         ROW_NUMBER() OVER(ORDER BY TeamID DESC) AS RowNumber,
                                           Teams.TeamID,
                                           Teams.TeamName, 
                                           Teams.PictureFile,
                                           Teams.Status,
                                           Users.Name,
				                 	      Stores.StoreName,
				                 		  Stores.StoreID
						                 Dishes.DishName
                                      FROM Groups
                                      JOIN Users ON Teams.UserID=Users.UserID
                                      JOIN Stores ON Teams.StoreID=Stores.StoreID
                                      JOIN Dishes ON Stores.StoreID=Dishes.StoreID
                                      WHERE DeleteDate IS NULL AND DishName=@name
                                   ) AS TempT
                                      WHERE RowNumber > {pageSize * (currentPage - 1)}
                                      ORDER BY TeamID DESC
                              ";
            }


            //資料庫開啟並執行SQL
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                if (!string.IsNullOrEmpty(name))
                    command.Parameters.AddWithValue("@name", name);
                else
                    command.Parameters.AddWithValue("@name", "");
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader(); //執行指令串
                    DataTable dt = new DataTable();
                    dt.Load(reader); // 將reader放入dt表
                    reader.Close();
                    connection.Close();
                    DataTable totalSize1 = readTablePageNum(name);
                    int? totalSize2 = totalSize1.Rows[0]["COUNT"] as int?;
                    totalSize = (totalSize2.HasValue) ? totalSize2.Value : 0;
                    return dt;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public DataTable readTablePageNum(string name)
        {
            string searchcol;
            switch (DropDownList1.SelectedIndex)
            {
                case 0:
                    searchcol = "Teams.TeamName";
                    break;
                case 1:
                    searchcol = "Stores.StoreName";
                    break;
                default:
                    searchcol = "team_name";
                    break;
            }
            string countQuery;
            if (DropDownList1.SelectedIndex != 2)
            {
                countQuery =
                     $@" SELECT 
                             COUNT(TeamID) AS COUNT
                         FROM Teams
                         JOIN Users ON Teams.UserID=Users.UserID
                         JOIN Stores ON Teams.StoreID=Stores.StoreID
                         WHERE Teams.DeleteDate IS NULL AND {searchcol} LIKE '%' + @name + '%'
                     ";
            }
            else
            {
                countQuery =
                     $@" SELECT 
                             COUNT(TeamID) AS COUNT
                         FROM Teams
                         JOIN Users ON Teams.UserID=Users.UserID
                         JOIN Stores ON Teams.StoreID=Stores.StoreID
                         JOIN Dishes ON Stores.StoreID=Dishes.StoreID
                         WHERE DeleteDate IS NULL AND DishName=@name
                     ";

            }
            //資料庫開啟並執行SQL
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(countQuery, connection);
                if (!string.IsNullOrEmpty(name))
                    command.Parameters.AddWithValue("@name", name);
                else
                    command.Parameters.AddWithValue("@name", "");
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader(); //執行指令串
                    DataTable dt = new DataTable();
                    dt.Load(reader); // 將reader放入dt表
                    reader.Close();
                    connection.Close();
                    return dt;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        protected void TeamRep_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rep = e.Item.FindControl("Rep_Indexdish") as Repeater;
                HiddenField hf = e.Item.FindControl("HFDsihStore") as HiddenField;

                //////原讀表顯示方式
                string[] colname = { "Dishes.DishName" };
                string[] colnamep = { "@StoreID" };
                string[] p = { hf.Value };

                string logic = @"
                            JOIN Stores ON Dishes.StoreID=Stores.StoreID
                            WHERE Dishes.DeleteDate IS NULL AND Dishes.StoreID=@StoreID
                            ";

                DataTable data = dbTool.readTable("Dishes", colname, logic, colnamep, p);

                rep.DataSource = data;
                rep.DataBind();

            }
        }

        public int CalculatePages(int totalSize, int pageSize)
        {
            int pages = totalSize / pageSize;

            if (totalSize % pageSize != 0)
                pages += 1;

            return pages;
        }


        internal class PagingLink
        {
            public string Name { get; set; }
            public string Link { get; set; }
            public string Title { get; set; }
        }


        private class IndexModel
        {
            public int TeamID { get; set; }
            public string TeamName { get; set; }
            public string PictureFile { get; set; }
            public string Status { get; set; }
            public string Name { get; set; }
            public string StoreName { get; set; }
            public int StoreID { get; set; }
            public int peoplenum { get; set; }
            public List<IndexDish> Dishes { get; set; }
        }

        private class IndexDish
        {
            public string DishName { get; set; }
        }
        

    }
}