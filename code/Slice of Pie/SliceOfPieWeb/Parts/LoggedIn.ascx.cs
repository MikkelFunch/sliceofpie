using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SliceOfPieWeb
{
    public partial class LoggedIn : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LabelWelcome.Text = "Welcome " + (String)Session["email"];
        }

        protected void ButtonLogOut_Click(object sender, EventArgs e)
        {
            //log out
        }
    }
}