using System.IO;
using System.ServiceModel;

namespace SOS.Service.Interfaces
{
    [ServiceContract(Namespace = "http://www.microsoft.com/sos/2013/02", Name = "MediaService")]
    public interface IMediaService
    {

        //[OperationContract] //Not implemented/ Not in use
        //[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST", BodyStyle = WebMessageBodyStyle.Bare)]
        void SaveTeaseImage(Stream imgStream);

    }
}
