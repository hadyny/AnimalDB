using AnimalDB.Repo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface ICulledPups
    {
        IEnumerable<CulledPups> GetCulledPups();

        Task CreateCulledPups(CulledPups culledPups);

        Task<CulledPups> GetCulledPupsById(int id);

        Task UpdateCulledPups(CulledPups culledPups);

        Task DeleteCulledPups(CulledPups culledPups);

        IEnumerable<CulledPups> GetCulledPupsByAnimalId(int animalId);
    }
}
