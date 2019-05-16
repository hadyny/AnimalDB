using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class ClinicalIncidentReportService : IClinicalIncidentReportService
    {
        private readonly IRepository<ClinicalIncidentReport> _clinicalIncidentReports;

        public ClinicalIncidentReportService(IRepository<ClinicalIncidentReport> clinicalIncidentReports)
        {
            this._clinicalIncidentReports = clinicalIncidentReports;
        }

        public async Task CreateClinicalIncidentReport(ClinicalIncidentReport clinicalIncidentReport)
        {
            _clinicalIncidentReports.Insert(clinicalIncidentReport);
            await _clinicalIncidentReports.Save();
        }

        public async Task DeleteClinicalIncidentReport(ClinicalIncidentReport clinicalIncidentReport)
        {
            await _clinicalIncidentReports.Delete(clinicalIncidentReport);
            await _clinicalIncidentReports.Save();
        }

        public async Task<ClinicalIncidentReport> GetClinicalIncidentReportById(int id)
        {
            return await _clinicalIncidentReports.GetById(id);
        }

        public async Task<IEnumerable<ClinicalIncidentReport>> GetClinicalIncidentReports()
        {
            return await _clinicalIncidentReports.GetAll();
        }

        public async Task UpdateClinicalIncidentReport(ClinicalIncidentReport clinicalIncidentReport)
        {
            _clinicalIncidentReports.Update(clinicalIncidentReport);
            await _clinicalIncidentReports.Save();
        }
    }
}
