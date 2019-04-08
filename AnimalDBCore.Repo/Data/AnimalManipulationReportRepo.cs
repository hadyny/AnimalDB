using AnimalDBCore.Infrastructure.Contexts;
using  AnimalDBCore.Core.Entities;
using  AnimalDBCore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDBCore.Infrastructure.Data
{
    public class AnimalManipulationReportRepo : IAnimalManipulationReport, IDisposable
    {
        private AnimalDBContext db;

        public AnimalManipulationReportRepo()
        {
            this.db = new AnimalDBContext();
        }

        public AnimalManipulationReportRepo(AnimalDBContext db)
        {
            this.db = db;
        }
        public async Task CreateAnimalManipulationReport(AnimalManipulationReport animalManipulationReport)
        {
            db.AnimalManipulationReports.Add(animalManipulationReport);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAnimalManipulationReport(AnimalManipulationReport animalManipulationReport)
        {
            if (db.Entry(animalManipulationReport).State == EntityState.Detached)
            {
                db.AnimalManipulationReports.Attach(animalManipulationReport);
            }
            db.AnimalManipulationReports.Remove(animalManipulationReport);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<AnimalManipulationReport> GetAnimalManipulationReportById(int id)
        {
            return await db.AnimalManipulationReports.FindAsync(id);
        }

        public IEnumerable<AnimalManipulationReport> GetAnimalManipulationReports()
        {
            return db.AnimalManipulationReports.ToList();
        }

        public async Task UpdateAnimalManipulationReport(AnimalManipulationReport animalManipulationReport)
        {
            db.Entry(animalManipulationReport).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
