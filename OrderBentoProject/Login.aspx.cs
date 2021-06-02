using OrderBentoProject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OrderBentoProject
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //_goToUrl = Request.RawUrl; //轉回本頁
            //如不用static 需用new建立此實體物件 命名helper變數 使用LoginHelper方法儲存Session資料
            LoginHelper helper = new LoginHelper();
            // 檢查使用者資訊，如果已經有session 且符合資料庫內使用者資訊則直接轉至首頁
            if (helper.HasLogIned())
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            Login_Message.Visible = false;
            string Account = C_Account.Text;
            string Password = C_Password.Text;

            if (!string.IsNullOrWhiteSpace(Account) && !string.IsNullOrWhiteSpace(Password))
            {
                LoginHelper helper = new LoginHelper();

                bool isSuccess = helper.TryLogIn(Account, Password);
                if (isSuccess)
                {
                    //跳轉到首頁
                    Response.Redirect("Index.aspx");
                }
                else
                {
                    Login_Message.Visible = true;
                    Login_Message.Text = "帳號密碼輸入錯誤";
                }
            }
            else
            {
                Login_Message.Visible = true;
                Login_Message.Text = "帳號密碼欄位不可為空";
            }
        }
    }
}