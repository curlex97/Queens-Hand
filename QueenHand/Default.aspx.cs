using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _Default : Page
{

    public static string DefaultServerPath;
    
       
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] != null) Response.Redirect("/Account/MainPanel.aspx");
        if (DataBase.ServerPath.Length == 0) DataBase.ServerPath = Server.MapPath("");

        QueenHandScript script = new QueenHandScript();
        script.BuildScript(@"

set #a0, 1, 1, 1, 1;

");

    }

  

    protected void Login_Button(object sender, EventArgs e)
    {
        Response.Redirect("/Account/Login.aspx");
        
    }

    protected void SignUp_Button(object sender, EventArgs e)
    {
        Response.Redirect("/Account/Register.aspx");

    }
}