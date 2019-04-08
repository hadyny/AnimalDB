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
    public class MedicationFollowUpRepo : IMedicationFollowUp, IDisposable
    {
        private AnimalDBContext db;

        public MedicationFollowUpRepo()
        {
            this.db = new AnimalDBContext();
        }

        public MedicationFollowUpRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateMedicationFollowUp(MedicationFollowUp medicationFollowUp)
        {
            db.MedicationFollowUps.Add(medicationFollowUp);
            await db.SaveChangesAsync();
        }

        public async Task DeleteMedicationFollowUp(MedicationFollowUp medicationFollowUp)
        {
            if (db.Entry(medicationFollowUp).State == EntityState.Detached)
            {
                db.MedicationFollowUps.Attach(medicationFollowUp);
            }
            db.MedicationFollowUps.Remove(medicationFollowUp);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public IEnumerable<MedicationFollowUp> GetMedicationFollowUpByAnimalId(int animalId)
        {
            return db.MedicationFollowUps.Where(m => m.Medication.Animal_Id == animalId).ToList();
        }

        public async Task<MedicationFollowUp> GetMedicationFollowUpById(int id)
        {
            return await db.MedicationFollowUps.FindAsync(id);
        }

        public IEnumerable<MedicationFollowUp> GetMedicationFollowUps()
        {
            return db.MedicationFollowUps.ToList();
        }

        public async Task UpdateMedicationFollowUp(MedicationFollowUp medicationFollowUp)
        {
            db.Entry(medicationFollowUp).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
