using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IMedicationRepository : IRepository<Medication>
    {
        IEnumerable<Medication> GetByAnimalId(int animalId);
    }
}
