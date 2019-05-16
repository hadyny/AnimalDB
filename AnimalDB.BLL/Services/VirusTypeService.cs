using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class VirusTypeService : IVirusTypeService
    {
        private readonly IRepository<VirusType> _virusTypes;

        public VirusTypeService(IRepository<VirusType> virusTypes)
        {
            this._virusTypes = virusTypes;
        }

        public async Task CreateVirusType(VirusType virusType)
        {
            _virusTypes.Insert(virusType);
            await _virusTypes.Save();
        }

        public async Task DeleteVirusType(VirusType virusType)
        {
            await _virusTypes.Delete(virusType);
            await _virusTypes.Save();
        }

        public async Task<VirusType> GetVirusTypeById(int id)
        {
            return await _virusTypes.GetById(id);
        }

        public async Task<IEnumerable<VirusType>> GetVirusTypes()
        {
            return await _virusTypes.GetAll();
        }

        public async Task UpdateVirusType(VirusType virusType)
        {
            _virusTypes.Update(virusType);
            await _virusTypes.Save();
        }
    }
}
