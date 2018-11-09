using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IMedication
    {
        IEnumerable<Medication> GetMedications();

        Task CreateMedication(Medication medication);

        Task<Medication> GetMedicationById(int id);

        IEnumerable<Medication> GetMedicationByAnimalId(int animalId);

        Task UpdateMedication(Medication medication);

        Task DeleteMedication(Medication medication);
    }
}
