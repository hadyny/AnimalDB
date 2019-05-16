using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IInvestigatorService
    {
        Task<IEnumerable<Investigator>> GetInvestigators();

        Task<Investigator> GetInvestigatorByUsername(string username);

        Task CreateInvestigator(Investigator investigator);

        Task<Investigator> GetInvestigatorById(string id);

        Task UpdateInvestigator(Investigator investigator);

        Task DeleteInvestigator(Investigator investigator);

        Task SetAuthCookie(string userName);
    }
}
