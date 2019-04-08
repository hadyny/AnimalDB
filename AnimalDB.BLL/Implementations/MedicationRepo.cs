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
    public class MedicationRepo : IMedication
    {
        private readonly AnimalDBContext db;

        public MedicationRepo()
        {
            this.db = new AnimalDBContext();
        }
        public MedicationRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateMedication(Medication medication)
        {
            db.Medications.Add(medication);
            await db.SaveChangesAsync();
        }

        public async Task DeleteMedication(Medication medication)
        {
            if (db.Entry(medication).State == EntityState.Detached)
            {
                db.Medications.Attach(medication);
            }

            foreach (var notification in db.Notifications.Where(m => m.Medication_Id == medication.Id))
            {
                db.Notifications.Remove(notification);
            }

            db.Medications.Remove(medication);
            await db.SaveChangesAsync();
        }

        public IEnumerable<Medication> GetMedicationByAnimalId(int animalId)
        {
            return db.Medications
                .Where(m => m.Animal_Id == animalId)
                .OrderByDescending(m => m.Timestamp).ToList();
        }

        public async Task<Medication> GetMedicationById(int id)
        {
            return await db.Medications.FindAsync(id);
        }

        public IEnumerable<Medication> GetMedications()
        {
            return db.Medications.ToList();
        }

        public async Task UpdateMedication(Medication medication)
        {
            db.Entry(medication).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
