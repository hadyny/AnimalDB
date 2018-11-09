using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface ICageLocationHistory
    {
        IEnumerable<CageLocationHistory> GetCageLocationHistories();

        Task CreateCageLocationHistory(CageLocationHistory cageLocationHistory);

        Task<CageLocationHistory> GetCageLocationHistoryById(int id);

        IEnumerable<CageLocationHistory> GetCageLocationHistoryByAnimalId(int animalId);

        Task UpdateCageLocationHistory(CageLocationHistory cageLocationHistory);

        Task DeleteCageLocationHistory(CageLocationHistory cageLocationHistory);
    }
}
