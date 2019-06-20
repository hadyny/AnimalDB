using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IEthicsDocumentRepository : IRepository<EthicsDocument>
    {
        IEnumerable<EthicsDocument> GetByInvestigatorId(string investigatorId);
        bool Exists(string fileName);
    }
}
