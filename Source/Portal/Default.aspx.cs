using System;
using System.Web.UI.HtmlControls;

namespace SOS.Web
{
    public partial class Map : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string culture = "en-IN";
            if (Request.UserLanguages.Length > 0)
            {
                culture = Request.UserLanguages[0];
            }

            var js = new HtmlGenericControl("script");
            js.Attributes["type"] = @"text/javascript";
            js.Attributes["src"] = @"https://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0&s=1&mkt=" + culture;
            Page.Header.Controls.Add(js);
        }

    }
}