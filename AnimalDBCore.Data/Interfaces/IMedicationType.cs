using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
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
