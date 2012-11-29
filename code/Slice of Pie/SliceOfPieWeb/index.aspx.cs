using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SliceOfPieWeb
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["online"] == null) { Session["online"] = false; Session.Abandon(); }
            LabelTitle.Text = "Slice of pie";
            
        }

        public Boolean Online
        {
            get;
            private set;
        }
    }
}