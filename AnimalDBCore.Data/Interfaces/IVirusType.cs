using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface IVirusType
    {
        IEnumerable<VirusType> GetVirusTypes();

        Task CreateVirusType(VirusType virusType);

        Task<VirusType> GetVirusTypeById(int id);

        Task UpdateVirusType(VirusType virusType);

        Task DeleteVirusType(VirusType virusType);
    }
}
