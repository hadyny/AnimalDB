using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly IRepository<Medication> _medications;
        private readonly IRepository<Notification> _notifications;

        public MedicationService(IRepository<Medication> medications, IRepository<Notification> notifications)
        {
            this._medications = medications;
            this._notifications = notifications;
        }

        public async Task CreateMedication(Medication medication)
        {
            _medications.Insert(medication);
            await _medications.Save();
        }

        public async Task DeleteMedication(Medication medication)
        {
            foreach (var notification in await _notifications.GetAll(m => m.Medication_Id == medication.Id))
            {
                await _notifications.Delete(notification);
            }

            await _medications.Delete(medication);

            await _notifications.Save();
            await _medications.Save();
        }

        public async Task<IEnumerable<Medication>> GetMedicationByAnimalId(int animalId)
        {
            var medications = await _medications.GetAll(m => m.Animal_Id == animalId);
            return medications
                .OrderByDescending(m => m.Timestamp);
        }

        public async Task<Medication> GetMedicationById(int id)
        {
            return await _medications.GetById(id);
        }

        public async Task<IEnumerable<Medication>> GetMedications()
        {
            return await _medications.GetAll();
        }

        public async Task UpdateMedication(Medication medication)
        {
            _medications.Update(medication);
            await _medications.Save();
        }
    }
}
