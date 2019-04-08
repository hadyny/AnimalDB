using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDBCore.Core.Interfaces
{
    public interface IInvestigator<T>
    {
        IEnumerable<T> GetInvestigators();

        T GetInvestigatorByUsername(string username);

        Task CreateInvestigator(T investigator);

        Task<T> GetInvestigatorById(string id);

        Task UpdateInvestigator(T investigator);

        Task DeleteInvestigator(T investigator);

        Task SetAuthCookie(string userName);
    }
}
