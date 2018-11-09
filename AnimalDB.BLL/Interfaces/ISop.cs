using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
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
