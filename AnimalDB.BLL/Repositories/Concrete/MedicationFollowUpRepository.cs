using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class MedicationFollowUpRepository : Repository<MedicationFollowUp>, IMedicationFollowUpRepository
    {
        public MedicationFollowUpRepository(AnimalDBContext context) : base(context)
        {
        }

        public IEnumerable<MedicationFollowUp> GetByAnimalId(int animalId)
        {
            return Context.MedicationFollowUps.Where(m => m.Medication.Animal_Id == animalId).ToList();
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
