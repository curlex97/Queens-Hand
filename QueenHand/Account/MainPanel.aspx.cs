using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class MainPanel : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null) Response.Redirect("../Default.aspx");
        else
            UserLabel.Text = "Здравствуй, " + DataBase.CurrentUser(Convert.ToInt32(Session["UserID"])).Name;
        ListViewDataItem item = new ListViewDataItem(0,0);
        item.DataItem = "Hello";
        UserList.Items.Add(item);
        WebPrettyGirl.BuildScript(Convert.ToInt32(Session["UserID"]),
@"


");

    }

    protected void SignOut_Button(object sender, EventArgs e)
    {
        Session["UserID"] = null;
        Response.Redirect("~/Default.aspx");
        
    }


    protected void DownLoadForms(object sender, EventArgs e)
    {

        FileInfo file = new FileInfo(Server.MapPath(@"\Content\Samples\form_lower.png"));
        Response.Clear();
        Response.ClearHeaders();
        Response.ClearContent();
        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
        Response.AddHeader("Content-Length", file.Length.ToString());
        Response.ContentType = "text/plain";
        Response.Flush();
        Response.TransmitFile(file.FullName);
        Response.End();
    }

    protected void UploadForms(object sender, EventArgs e)
    {
        Response.Redirect("UploadFont.aspx");
    }

    protected void HandWriting(object sender, EventArgs e)
    {
        Response.Redirect("HandWrite.aspx");

    }

    protected void ManageProfile(object sender, EventArgs e)
    {
        Response.Redirect("UserSettings.aspx");
    }

    protected void FontSettings_Button(object sender, EventArgs e)
    {
        Response.Redirect("FontSettings.aspx");
    }
}