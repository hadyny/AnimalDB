using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IMedicationType
    {
        IEnumerable<MedicationType> GetMedicationTypes();

        Task CreateMedicationType(MedicationType medicationType);

        Task<MedicationType> GetMedicationTypeById(int id);

        Task UpdateMedicationType(MedicationType medicationType);

        Task DeleteMedicationType(MedicationType medicationType);
    }
}
