using AnimalDBCore.Infrastructure.Contexts;
using AnimalDBCore.Core.Entities;
using AnimalDBCore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDBCore.Infrastructure.Data
{
    public class SurgeryTypeRepo : ISurgeryType, IDisposable
    {
        private AnimalDBContext db;

        public SurgeryTypeRepo()
        {
            this.db = new AnimalDBContext();
        }
        public SurgeryTypeRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateSurgeryType(SurgeryType surgeryType)
        {
            db.SurgeryTypes.Add(surgeryType);
            await db.SaveChangesAsync();
        }

        public async Task DeleteSurgeryType(SurgeryType surgeryType)
        {
            if (db.Entry(surgeryType).State == EntityState.Detached)
            {
                db.SurgeryTypes.Attach(surgeryType);
            }
            db.SurgeryTypes.Remove(surgeryType);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<SurgeryType> GetSurgeryTypeById(int id)
        {
            return await db.SurgeryTypes.FindAsync(id);
        }

        public IEnumerable<SurgeryType> GetSurgeryTypes()
        {
            return db.SurgeryTypes.ToList();
        }

        public async Task UpdateSurgeryType(SurgeryType surgeryType)
        {
            db.Entry(surgeryType).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
