using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class SurgeryTypeService : ISurgeryTypeService
    {
        private readonly IRepository<SurgeryType> _surgeryTypes;

        public SurgeryTypeService(IRepository<SurgeryType> surgeryTypes)
        {
            this._surgeryTypes = surgeryTypes;
        }

        public async Task CreateSurgeryType(SurgeryType surgeryType)
        {
            _surgeryTypes.Insert(surgeryType);
            await _surgeryTypes.Save();
        }

        public async Task DeleteSurgeryType(SurgeryType surgeryType)
        {
            await _surgeryTypes.Delete(surgeryType);
            await _surgeryTypes.Save();
        }

        public async Task<SurgeryType> GetSurgeryTypeById(int id)
        {
            return await _surgeryTypes.GetById(id);
        }

        public async Task<IEnumerable<SurgeryType>> GetSurgeryTypes()
        {
            return await _surgeryTypes.GetAll();
        }

        public async Task UpdateSurgeryType(SurgeryType surgeryType)
        {
            _surgeryTypes.Update(surgeryType);
            await _surgeryTypes.Save();
        }
    }
}
