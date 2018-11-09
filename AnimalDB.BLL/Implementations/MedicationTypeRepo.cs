using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Implementations
{
    public class MedicationTypeRepo : IMedicationType, IDisposable
    {
        private AnimalDBContext db;

        public MedicationTypeRepo()
        {
            this.db = new AnimalDBContext();
        }
        public MedicationTypeRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateMedicationType(MedicationType MedicationType)
        {
            db.MedicationTypes.Add(MedicationType);
            await db.SaveChangesAsync();
        }

        public async Task DeleteMedicationType(MedicationType MedicationType)
        {
            if (db.Entry(MedicationType).State == EntityState.Detached)
            {
                db.MedicationTypes.Attach(MedicationType);
            }
            db.MedicationTypes.Remove(MedicationType);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<MedicationType> GetMedicationTypeById(int id)
        {
            return await db.MedicationTypes.FindAsync(id);
        }

        public IEnumerable<MedicationType> GetMedicationTypes()
        {
            return db.MedicationTypes.ToList();
        }

        public async Task UpdateMedicationType(MedicationType MedicationType)
        {
            db.Entry(MedicationType).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
