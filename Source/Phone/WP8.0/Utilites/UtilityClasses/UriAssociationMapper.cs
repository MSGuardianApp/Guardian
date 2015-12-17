using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace SOS.Phone.Utilites.UtilityClasses
{
    class UriAssociationMapper : UriMapperBase
    {
        private string tempUri;

        public override Uri MapUri(Uri uri)
        {
            tempUri = System.Net.HttpUtility.UrlDecode(uri.ToString());

            // URI association launch for Guardian.
            if (tempUri.Contains("guardian:TrackMe"))
            {
                return new Uri("/MainPage.xaml?ToPage=Tracking", UriKind.Relative);
            }
            
            if (tempUri.Contains("guardian:SOS"))
            {
                return new Uri("/Pages/StartSOS.xaml?DefaultTitle=SOSTile", UriKind.Relative);
            }
            // Otherwise perform normal launch.
            return uri;
        }
    }
}
