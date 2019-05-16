using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class StrainService : IStrainService
    {
        private readonly IRepository<Strain> _strains;

        public StrainService(IRepository<Strain> strains)
        {
            this._strains = strains;
        }

        public async Task CreateStrain(Strain Strain)
        {
            _strains.Insert(Strain);
            await _strains.Save();
        }

        public async Task DeleteStrain(Strain Strain)
        {
            await _strains.Delete(Strain);
            await _strains.Save();
        }

        public async Task<Strain> GetStrainById(int id)
        {
            return await _strains.GetById(id);
        }

        public async Task<IEnumerable<Strain>> GetStrains()
        {
            return await _strains.GetAll();
        }

        public async Task UpdateStrain(Strain strain)
        {
            _strains.Update(strain);
            await _strains.Save();
        }
    }
}
