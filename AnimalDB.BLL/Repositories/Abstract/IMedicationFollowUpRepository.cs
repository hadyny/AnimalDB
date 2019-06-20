using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IMedicationFollowUpRepository : IRepository<MedicationFollowUp>
    {
        IEnumerable<MedicationFollowUp> GetByAnimalId(int animalId);
    }
}
