using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class About : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null) Response.Redirect("~/Default.aspx");
        else
        {
            int id = Convert.ToInt32(Session["UserID"]);
            if(UserName.Text.Length == 0) UserName.Text = DataBase.CurrentUser(id).Name;
        }
        
    }

    protected void ChangeUser(object sender, EventArgs e)
    {
        if(DataBase.UpdateUser(UserName.Text, Password.Text, ConfirmPassword.Text, Convert.ToInt32(Session["UserID"]))) 
            Response.Redirect("~/Default.aspx");
    }
}