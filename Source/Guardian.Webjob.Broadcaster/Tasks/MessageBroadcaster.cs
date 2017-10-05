using Guardian.Common.Configuration;
using Microsoft.WindowsAzure.ServiceRuntime;
using SOS.AzureSQLAccessLayer;
using SOS.Mappers;
using SOS.Model;
using SOS.Model.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Guardian.Webjob.Broadcaster
{
    public class MessageBroadcaster
    {
        string RoleInstanceId = string.Empty;
        readonly Settings settings;
        const int minute = 60 * 1000;

        public MessageBroadcaster(IConfigManager configManager)
        {
            SOS.Mappers.Mapper.InitializeMappers();
            this.settings = configManager.Settings;
            RoleInstanceId = RoleEnvironment.IsAvailable ? RoleEnvironment.CurrentRoleInstance.Id : string.Empty;
        }

        public async Task Run()
        {
            //Processing Cycle time 30 secs 
            //1. Process messages from LiveSession - Send Messages. Use intance id as identifier for the batch
            //Parallelly send messages for multiple profiles
            try
            {
                Guid processKey = Guid.NewGuid();

                List<LiveSession> sosSessions =
                    await new LiveSessionRepository()
                    .GetSessionsForNotifications(RoleInstanceId, processKey, settings.SendSms, settings.SMSPostGap, settings.EmailPostGap, 9999);

                if (sosSessions != null && sosSessions.Count > 0)
                {
                    var processedSessions = PostMessages.SendSOSNotifications(sosSessions, settings);

                    var liteSessions = processedSessions.ConvertToLiveSessionLite();

                    string liteSessionsXML = Serialize<List<LiveSessionLite>>(liteSessions, true, true);

                    new LiveSessionRepository().UpdateNotificationComplete(RoleInstanceId, processKey, liteSessionsXML).Wait();
                }
                else
                {
                    Trace.TraceInformation($"Broadcasting messages has completed. Sleeping for {settings.BroadcastRunIntervalInSeconds.ToString()} seconds...", "Information");
                    await Task.Delay(settings.BroadcastRunIntervalInSeconds * 1000);
                }
            }

            catch (Exception ex)
            {
                Trace.TraceError("WebJob Error: Broadcasting messages failed! " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace, "Error");

                await Task.Delay(minute);
            }
        }

        public static string Serialize<X>(X obj, bool omitXmlDeclaration = false, bool omitNameSpace = false)
        {
            var xmlSer = new XmlSerializer(typeof(X));

            var settings = new XmlWriterSettings() { OmitXmlDeclaration = omitXmlDeclaration, Indent = true, Encoding = new UTF8Encoding(false) };
            using (var ms = new MemoryStream())
            using (var writer = XmlWriter.Create(ms, settings))
            {
                if (!omitNameSpace)
                {
                    xmlSer.Serialize(writer, obj);
                }
                else
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    xmlSer.Serialize(writer, obj, ns);
                }
                ms.Seek(0, 0);
                return new UTF8Encoding(false).GetString(ms.GetBuffer(), 0, (int)ms.Length);
            }
        }
    }
}
