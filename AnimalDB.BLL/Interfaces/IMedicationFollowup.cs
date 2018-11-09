using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IMedicationFollowUp
    {
        IEnumerable<MedicationFollowUp> GetMedicationFollowUps();

        Task CreateMedicationFollowUp(MedicationFollowUp MedicationFollowUp);

        Task<MedicationFollowUp> GetMedicationFollowUpById(int id);

        IEnumerable<MedicationFollowUp> GetMedicationFollowUpByAnimalId(int animalId);

        Task UpdateMedicationFollowUp(MedicationFollowUp MedicationFollowUp);

        Task DeleteMedicationFollowUp(MedicationFollowUp MedicationFollowUp);
    }
}
