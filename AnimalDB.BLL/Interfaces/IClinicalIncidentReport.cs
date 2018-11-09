using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IClinicalIncidentReport
    {
        IEnumerable<ClinicalIncidentReport> GetClinicalIncidentReports();

        Task CreateClinicalIncidentReport(ClinicalIncidentReport clinicalIncidentReport);

        Task<ClinicalIncidentReport> GetClinicalIncidentReportById(int id);

        Task UpdateClinicalIncidentReport(ClinicalIncidentReport clinicalIncidentReport);

        Task DeleteClinicalIncidentReport(ClinicalIncidentReport clinicalIncidentReport);
    }
}
