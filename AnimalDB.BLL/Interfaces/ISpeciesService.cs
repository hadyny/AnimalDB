using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface ISpeciesService
    {
        Task<IEnumerable<Species>> GetSpecies();

        Task CreateSpecies(Species species);

        Task<Species> GetSpeciesById(int id);

        Task UpdateSpecies(Species species);

        Task DeleteSpecies(Species species);
    }
}
