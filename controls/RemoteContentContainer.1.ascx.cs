using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RemoteContentContainer : System.Web.UI.UserControl
{
   public string Src { get; set; }

   protected void Page_Load(object sender, EventArgs e)
   {

   }

   public void RenderUserControl()
   {
      if (!string.IsNullOrWhiteSpace(Src))
         ifrRemoteContentContainer.Attributes["src"] = Src.ToLower();
   }
}