using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class MedicationRepository : Repository<Medication>, IMedicationRepository
    {
        public MedicationRepository(AnimalDBContext context) : base(context)
        {
        }

        public override void Delete(Medication medication)
        {
            foreach (var notification in Context.Notifications.Where(m => m.Medication_Id == medication.Id))
            {
                Context.Notifications.Remove(notification);
            }

            Context.Medications.Remove(medication);
        }

        public IEnumerable<Medication> GetByAnimalId(int animalId)
        {
            return Context
                    .Medications
                    .Where(m => 
                            m.Animal_Id == animalId)
                    .OrderByDescending(m => m.Timestamp)
                    .ToList();
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
