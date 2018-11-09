using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
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
