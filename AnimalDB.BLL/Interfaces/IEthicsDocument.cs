using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IEthicsDocument
    {
        IEnumerable<EthicsDocument> GetEthicsDocuments();

        Task CreateEthicsDocument(EthicsDocument ethicsDocument);

        Task<EthicsDocument> GetEthicsDocumentById(int id);

        Task UpdateEthicsDocument(EthicsDocument ethicsDocument);

        Task DeleteEthicsDocument(EthicsDocument ethicsDocument);

        IEnumerable<EthicsDocument> GetEthicsDocumentsByInvestigatorId(string investigatorId);

        bool CheckIfDocumentExists(string fileName);
    }
}
