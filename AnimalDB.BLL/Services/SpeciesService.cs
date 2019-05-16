using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class SpeciesService : ISpeciesService
    {
        private readonly IRepository<Species> _species;

        public SpeciesService(IRepository<Species> species)
        {
            this._species = species;
        }

        public async Task CreateSpecies(Species Species)
        {
            _species.Insert(Species);
            await _species.Save();
        }

        public async Task DeleteSpecies(Species Species)
        {
            await _species.Delete(Species);
            await _species.Save();
        }

        public async Task<Species> GetSpeciesById(int id)
        {
            return await _species.GetById(id);
        }

        public async Task<IEnumerable<Species>> GetSpecies()
        {
            return await _species.GetAll();
        }

        public async Task UpdateSpecies(Species species)
        {
            _species.Update(species);
            await _species.Save();
        }

    }
}
