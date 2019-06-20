using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IRackEntryRepository : IRepository<RackEntry>
    {
        IEnumerable<RackEntry> GetByAnimalId(int animalId);
    }
}
