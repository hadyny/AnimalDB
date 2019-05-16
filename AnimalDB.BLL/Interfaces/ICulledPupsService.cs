using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface ICulledPupsService
    {
        Task<IEnumerable<CulledPups>> GetCulledPups();

        Task CreateCulledPups(CulledPups culledPups);

        Task<CulledPups> GetCulledPupsById(int id);

        Task UpdateCulledPups(CulledPups culledPups);

        Task DeleteCulledPups(CulledPups culledPups);

        Task<IEnumerable<CulledPups>> GetCulledPupsByAnimalId(int animalId);
    }
}
