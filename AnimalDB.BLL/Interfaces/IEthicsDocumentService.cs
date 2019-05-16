using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IEthicsDocumentService
    {
        Task<IEnumerable<EthicsDocument>> GetEthicsDocuments();

        Task CreateEthicsDocument(EthicsDocument ethicsDocument);

        Task<EthicsDocument> GetEthicsDocumentById(int id);

        Task UpdateEthicsDocument(EthicsDocument ethicsDocument);

        Task DeleteEthicsDocument(EthicsDocument ethicsDocument);

        Task<IEnumerable<EthicsDocument>> GetEthicsDocumentsByInvestigatorId(string investigatorId);

        Task<bool> CheckIfDocumentExists(string fileName);
    }
}
