using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface ICulledPupsRepository : IRepository<CulledPups>
    {
        IEnumerable<CulledPups> GetByAnimalId(int animalId);
    }
}
