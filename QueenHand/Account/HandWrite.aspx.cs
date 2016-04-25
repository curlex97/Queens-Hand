using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class About : Page
{

    private string Text = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null) Response.Redirect("../Default.aspx");
      if(WidthBlock.Text.Length==0)  WidthBlock.Text = Hand.Width.ToString();
      if (HeightBlock.Text.Length == 0) HeightBlock.Text = Hand.Height.ToString();
       // Request.Form["TextInput"] = Text;
    }

    protected void Button1_OnClick(object sender, EventArgs e)
    {
        if (WebPrettyGirl.IsInitialize)
        {
            try
            {
                Hand.Width = Convert.ToInt32(WidthBlock.Text);
                Hand.Height = Convert.ToInt32(HeightBlock.Text);
            }
            catch
            {

            }
            finally
            {
                Text = Request.Form["TextInput"];
                string path = WebPrettyGirl.Build(Text, Convert.ToInt32(Session["UserID"]));
                path = path.Substring(path.IndexOf(@"\Content", StringComparison.Ordinal));
                path = "../" + path;
                Image1.ImageUrl = path;

            }
           
        }

    }

    protected void SaveFont_OnClick(object sender, EventArgs e)
    {

        try
        {
            FileInfo file = new FileInfo(Server.MapPath(@"\Content\Users\" + DataBase.CurrentUser(Convert.ToInt32(Session["UserID"])).Name + @"\Compile\LastCompile.png"));
            if (file.Exists)
            {
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
        }
        catch 
        {
            
            
        }
    }

    protected void FontSettings_Button(object sender, EventArgs e)
    {
        Response.Redirect("FontSettings.aspx");
    }
}