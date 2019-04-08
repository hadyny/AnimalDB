using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
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
