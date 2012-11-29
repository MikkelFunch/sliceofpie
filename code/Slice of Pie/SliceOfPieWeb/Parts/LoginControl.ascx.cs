using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SliceOfPieWeb
{
    public partial class WebUserControl1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            Session["online"] = (Object)true;
            Session["email"] = "test@email.com";
        }

        protected void ButtonRegister_Click(object sender, EventArgs e)
        {
            //popupbox med register
        }
    }
}