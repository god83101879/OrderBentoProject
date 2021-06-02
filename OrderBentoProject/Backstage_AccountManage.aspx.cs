using OrderBentoProject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OrderBentoProject
{
    public partial class Backstage_AccountManage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Create_Click(object sender, EventArgs e)
        {
            C_Message.Visible = false;
            string Name = C_Name.Text;
            string Account = C_Account.Text;
            string PassWord = C_Password.Text;
            bool Privilege = false;
            if (Drop_P.SelectedValue == "使用者")
            {
                Privilege = false;
            }
            else if (Drop_P.SelectedValue == "管理者")
            {
                Privilege = true;
            }
            else
            {
                C_Message.Visible = true;
                C_Message.Text = "欄位不可為空";
                return;
            }
            //string Privilege = ;
            if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Account) && !string.IsNullOrWhiteSpace(PassWord))
            {
                DBTool Create = new DBTool();
                string[] colname = { "Name", "Account", "PassWord", "Privilege", "WhoCreate", "CreateDate" };
                string[] colnamep = { "@Name", "@Account", "@PassWord", "@Privilege", "@WhoCreate", "@CreateDate" };
                List<string> p = new List<string>();
                p.Add(Name);
                p.Add(Account);
                p.Add(PassWord);
                p.Add(Privilege.ToString());
                p.Add("Manager");
                p.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Create.InsertTable("Users", colname, colnamep, p);
                C_Message.Visible = true;
                C_Message.Text = "新增成功";
            }
            else
            {
                C_Message.Visible = true;
                C_Message.Text = "欄位不可為空";
            }
        }
    }
}