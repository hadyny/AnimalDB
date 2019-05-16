using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class MedicationTypeService : IMedicationTypeService
    {
        private readonly IRepository<MedicationType> _medicationTypes;

        public MedicationTypeService(IRepository<MedicationType> medicationTypes)
        {
            this._medicationTypes = medicationTypes;
        }

        public async Task CreateMedicationType(MedicationType MedicationType)
        {
            _medicationTypes.Insert(MedicationType);
            await _medicationTypes.Save();
        }

        public async Task DeleteMedicationType(MedicationType MedicationType)
        {
            await _medicationTypes.Delete(MedicationType);
            await _medicationTypes.Save();
        }

        public async Task<MedicationType> GetMedicationTypeById(int id)
        {
            return await _medicationTypes.GetById(id);
        }

        public async Task<IEnumerable<MedicationType>> GetMedicationTypes()
        {
            return await _medicationTypes.GetAll();
        }

        public async Task UpdateMedicationType(MedicationType medicationType)
        {
            _medicationTypes.Update(medicationType);
            await _medicationTypes.Save();
        }
    }
}
