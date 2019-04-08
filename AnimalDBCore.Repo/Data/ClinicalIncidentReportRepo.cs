using AnimalDBCore.Infrastructure.Contexts;
using  AnimalDBCore.Core.Entities;
using  AnimalDBCore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalDBCore.Infrastructure.Data
{
    public class ClinicalIncidentReportRepo : IClinicalIncidentReport, IDisposable
    {
        private AnimalDBContext db;

        public ClinicalIncidentReportRepo()
        {
            this.db = new AnimalDBContext();
        }
        public ClinicalIncidentReportRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateClinicalIncidentReport(ClinicalIncidentReport clinicalIncidentReport)
        {
            db.ClinicalIncidentReports.Add(clinicalIncidentReport);
            await db.SaveChangesAsync();
        }

        public async Task DeleteClinicalIncidentReport(ClinicalIncidentReport clinicalIncidentReport)
        {
            if (db.Entry(clinicalIncidentReport).State == EntityState.Detached)
            {
                db.ClinicalIncidentReports.Attach(clinicalIncidentReport);
            }
            db.ClinicalIncidentReports.Remove(clinicalIncidentReport);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<ClinicalIncidentReport> GetClinicalIncidentReportById(int id)
        {
            return await db.ClinicalIncidentReports.FindAsync(id);
        }

        public IEnumerable<ClinicalIncidentReport> GetClinicalIncidentReports()
        {
            return db.ClinicalIncidentReports.ToList();
        }

        public async Task UpdateClinicalIncidentReport(ClinicalIncidentReport clinicalIncidentReport)
        {
            db.Entry(clinicalIncidentReport).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
