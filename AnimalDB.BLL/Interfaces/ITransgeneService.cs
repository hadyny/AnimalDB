using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface ITransgeneService
    {
        Task<IEnumerable<Transgene>> GetTransgenes();

        Task CreateTransgene(Transgene transgene);

        Task<Transgene> GetTransgeneById(int id);

        Task UpdateTransgene(Transgene transgene);

        Task DeleteTransgene(Transgene transgene);
    }
}
