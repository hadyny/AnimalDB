using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class MedicationFollowUpService : IMedicationFollowUpService
    {
        private readonly IRepository<MedicationFollowUp> _medicationFollowUps;

        public MedicationFollowUpService(IRepository<MedicationFollowUp> medicationFollowUps)
        {
            this._medicationFollowUps = medicationFollowUps;
        }

        public async Task CreateMedicationFollowUp(MedicationFollowUp medicationFollowUp)
        {
            _medicationFollowUps.Insert(medicationFollowUp);
            await _medicationFollowUps.Save();
        }

        public async Task DeleteMedicationFollowUp(MedicationFollowUp medicationFollowUp)
        {
            await _medicationFollowUps.Delete(medicationFollowUp.Id);
            await _medicationFollowUps.Save();
        }

        public async Task<IEnumerable<MedicationFollowUp>> GetMedicationFollowsUpByAnimalId(int animalId)
        {
            return await _medicationFollowUps.GetAll(m => m.Medication.Animal_Id == animalId);
        }

        public async Task<MedicationFollowUp> GetMedicationFollowUpById(int id)
        {
            return await _medicationFollowUps.GetById(id);
        }

        public async Task<IEnumerable<MedicationFollowUp>> GetMedicationFollowUps()
        {
            return await _medicationFollowUps.GetAll();
        }

        public async Task UpdateMedicationFollowUp(MedicationFollowUp medicationFollowUp)
        {
            _medicationFollowUps.Update(medicationFollowUp);
            await _medicationFollowUps.Save();
        }
    }
}
