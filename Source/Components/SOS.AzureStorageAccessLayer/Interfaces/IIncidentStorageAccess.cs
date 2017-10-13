using SOS.AzureStorageAccessLayer.Entities;
using System.Collections.Generic;

namespace SOS.AzureStorageAccessLayer
{
    public interface IIncidentStorageAccess
    {
        void RecordIncident(Incident report);

        List<Incident> GetAllIncidents();

        List<Incident> GetAllIncidentsByProfile(string ProfileID);
        void RemoveIncident(string ProfileID);

        List<Incident> GetAllIncidentsDataByFilter(string startDate, string endDate);

        List<Incident> GetAllIncidentsDataByID(string incidentID);
    }
}
