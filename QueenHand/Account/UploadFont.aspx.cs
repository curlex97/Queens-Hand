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
    }

    protected void UploadFont_Button(object sender, EventArgs e)
    {
        string lowerName = FileUpload1.FileName;
        string upperName = FileUpload2.FileName;
        string specialName = FileUpload3.FileName;

        if (lowerName.Length > 0)
        {
            if (System.IO.Path.GetExtension(lowerName) == ".png")
            {
                string path = DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(Convert.ToInt32(Session["UserID"])).Name + @"\Font\lower.png";
                FileUpload1.SaveAs(path);
                HandWriteBuilder.BuildHandWrite(Convert.ToInt32(Session["UserID"]), path, 0, 5);
            }
        }

        if (upperName.Length > 0)
        {
            if (System.IO.Path.GetExtension(upperName) == ".png")
            {
                string path = DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(Convert.ToInt32(Session["UserID"])).Name + @"\Font\upper.png";
                FileUpload2.SaveAs(path);
                HandWriteBuilder.BuildHandWrite(Convert.ToInt32(Session["UserID"]), path, 0, 5);
            }
        }

        if (specialName.Length > 0)
        {
            if (System.IO.Path.GetExtension(specialName) == ".png")
            {
                string path = DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(Convert.ToInt32(Session["UserID"])).Name + @"\Font\special.png";
                FileUpload3.SaveAs(path);
                HandWriteBuilder.BuildHandWrite(Convert.ToInt32(Session["UserID"]), path, 0, 5);
            }
        }

        Response.Redirect("~/Default.aspx");

    }
}