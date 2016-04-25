using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class About : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null) Response.Redirect("~/Default.aspx");
        else if(TextBox0.Text.Length == 0) Set();
        
        
    }

    protected void ApplySettings_Button(object sender, EventArgs e)
    {

        int max = Hand.RMax;
        int min = Hand.RMin;

        try
        {
            if (Session["UserID"] != null)
            {
                int id = Convert.ToInt32(Session["UserID"]);
            Hand.RMin = Convert.ToInt32(TextBox8.Text);
            Hand.RMax = Convert.ToInt32(TextBox9.Text);
            bool fl = HandRandom.SetConfiguration(Convert.ToInt32(TextBox1.Text),
                Convert.ToInt32(TextBox2.Text),
                Convert.ToInt32(TextBox3.Text),
                Convert.ToInt32(TextBox5.Text),
                Convert.ToInt32(TextBox7.Text),
                Convert.ToInt32(TextBox15.Text),
                Convert.ToInt32(TextBox16.Text),
                Convert.ToInt32(TextBox18.Text),
                Convert.ToInt32(TextBox19.Text),
                Convert.ToInt32(TextBox10.Text),
                Convert.ToInt32(TextBox4.Text),
                Convert.ToInt32(TextBox0.Text),
                Convert.ToInt32(TextBox12.Text),
                Convert.ToInt32(TextBox14.Text),
                Convert.ToInt32(TextBox17.Text),
                Convert.ToInt32(TextBox11.Text));
            Set();
            if (new FileInfo(DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(id).Name + @"\Data\config.ini").Exists) new FileInfo(DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(id).Name + @"\Data\config.ini").Delete();
            using (StreamWriter fs = new StreamWriter(DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(id).Name + @"\Data\config.ini", false))
            {
                fs.WriteLine(
                    TextBox8.Text + ";" +
                    TextBox9.Text + ";" +
                    TextBox1.Text + ";" +
                    TextBox2.Text + ";" +
                    TextBox3.Text + ";" +
                    TextBox5.Text + ";" +
                    TextBox7.Text + ";" +
                    TextBox15.Text + ";" +
                    TextBox16.Text + ";" +
                    TextBox18.Text + ";" +
                    TextBox19.Text + ";" +
                    TextBox10.Text + ";" +
                    TextBox4.Text + ";" +
                    TextBox0.Text + ";" +
                    TextBox12.Text + ";" +
                    TextBox14.Text + ";" +
                    TextBox17.Text + ";" +
                    TextBox11.Text);
            }
            
              
                    DataBase.CurrentUser(id).HandWrite.ReadConfig(id);
                    Response.Redirect("~/Default.aspx");
            }
            
           

        }
        catch
        {
           // Hand.RMax = max;
           // Hand.RMin = min;
          
        }

            



    }

    void Set()
    {
        TextBox0.Text = HandRandom.DefaultSpace.ToString();
        TextBox1.Text = HandRandom.MinGetSpaceLength.ToString();
        TextBox2.Text = HandRandom.MaxGetSpaceLength.ToString();
        TextBox3.Text = HandRandom.MultiGetSpaceLength.ToString();
        TextBox4.Text = HandRandom.DefaultLine.ToString();
        TextBox5.Text = HandRandom.MinGetLineSpace.ToString();

        TextBox7.Text = HandRandom.MaxGetLineSpace.ToString();
        TextBox8.Text = Hand.RMin.ToString();
        TextBox9.Text = Hand.RMax.ToString();
        TextBox10.Text = HandRandom.BeginY.ToString();
        TextBox11.Text = HandRandom.RightEdge.ToString();
        TextBox12.Text = HandRandom.MinBeginX.ToString();

        TextBox14.Text = HandRandom.MaxBeginX.ToString();
        TextBox15.Text = HandRandom.DivGetLetterScaleX.ToString();
        TextBox16.Text = HandRandom.DivGetLetterScaleY.ToString();
        TextBox17.Text = HandRandom.ScaleMultipler.ToString();
        TextBox18.Text = HandRandom.MinGetDeltaMoveY.ToString();
        TextBox19.Text = HandRandom.MaxGetDeltaMoveY.ToString();
    }
}