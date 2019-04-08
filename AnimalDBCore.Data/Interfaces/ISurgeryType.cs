using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface ISurgeryType
    {
        IEnumerable<SurgeryType> GetSurgeryTypes();

        Task CreateSurgeryType(SurgeryType surgeryType);

        Task<SurgeryType> GetSurgeryTypeById(int id);

        Task UpdateSurgeryType(SurgeryType surgeryType);

        Task DeleteSurgeryType(SurgeryType surgeryType);
    }
}
