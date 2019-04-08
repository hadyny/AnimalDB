using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface ISpecies
    {
        IEnumerable<Species> GetSpecies();

        Task CreateSpecies(Species species);

        Task<Species> GetSpeciesById(int id);

        Task UpdateSpecies(Species species);

        Task DeleteSpecies(Species species);
    }
}
