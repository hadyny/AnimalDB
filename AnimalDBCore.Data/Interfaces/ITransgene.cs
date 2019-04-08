using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface ITransgene
    {
        IEnumerable<Transgene> GetTransgenes();

        Task CreateTransgene(Transgene transgene);

        Task<Transgene> GetTransgeneById(int id);

        Task UpdateTransgene(Transgene transgene);

        Task DeleteTransgene(Transgene transgene);
    }
}
