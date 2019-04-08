using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface ISop
    {
        IEnumerable<Sop> GetSops();

        Task CreateSop(Sop sop);

        Task<Sop> GetSopById(int id);

        Task UpdateSop(Sop sop);

        Task DeleteSop(Sop sop);

        IEnumerable<Sop> GetSopsByCategoryId(int categoryId);

        bool DoesSopFileNameExist(string fileName);
    }
}
