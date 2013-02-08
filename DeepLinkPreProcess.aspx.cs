using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DeepLinkPreProcess : XnGR_WBS_Page
{
   protected override void Page_Init(object sender, EventArgs e)
   {
      this.IsNewSessionOverride = true;
      Session.Abandon();
      base.Page_Init(sender, e);
   }
   protected void Page_Load(object sender, EventArgs e)
   {

   }
}