﻿using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Implementations
{
    public class ClinicalIncidentReportRepo : IClinicalIncidentReport
    {
        private readonly AnimalDBContext db;

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
