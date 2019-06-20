using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class EthicsDocumentRepository : Repository<EthicsDocument>, IEthicsDocumentRepository
    {
        public EthicsDocumentRepository(AnimalDBContext context) : base(context)
        {
        }

        public IEnumerable<EthicsDocument> GetByInvestigatorId(string investigatorId)
        {
            return Context.EthicsDocuments.Where(m => m.Investigator_Id == investigatorId).ToList();
        }

        public bool Exists(string fileName)
        {
            return Context.EthicsDocuments.Count(m => m.FileName == fileName) != 0;
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
