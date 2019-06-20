using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface ICageLocationHistoryRepository : IRepository<CageLocationHistory>
    {
        IEnumerable<CageLocationHistory> GetByAnimalId(int animalId);
    }
}
