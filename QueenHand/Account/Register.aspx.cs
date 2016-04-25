using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.UI;

public partial class Account_Register : Page
{
    protected void CreateUser_Click(object sender, EventArgs e)
    {
        int result = DataBase.SignUp(UserName.Text, Password.Text, ConfirmPassword.Text);
        if (result >= 0)
        {
            Session["UserID"] = result;
            Response.Redirect("../Default.aspx");   
        }
    }
}