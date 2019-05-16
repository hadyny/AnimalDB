using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class EthicsDocumentService : IEthicsDocumentService
    {
        private readonly IRepository<EthicsDocument> _ethicsDocuments;

        public EthicsDocumentService(IRepository<EthicsDocument> ethicsDocuments)
        {
            this._ethicsDocuments = ethicsDocuments;
        }

        public async Task CreateEthicsDocument(EthicsDocument ethicsDocument)
        {
            _ethicsDocuments.Insert(ethicsDocument);
            await _ethicsDocuments.Save();
        }

        public async Task DeleteEthicsDocument(EthicsDocument ethicsDocument)
        {
            await _ethicsDocuments.Delete(ethicsDocument);
            await _ethicsDocuments.Save();
        }

        public async Task<EthicsDocument> GetEthicsDocumentById(int id)
        {
            return await _ethicsDocuments.GetById(id);
        }

        public async Task<IEnumerable<EthicsDocument>> GetEthicsDocumentsByInvestigatorId(string investigatorId)
        {
            return await _ethicsDocuments.GetAll(m => m.Investigator_Id == investigatorId);
        }

        public async Task<IEnumerable<EthicsDocument>> GetEthicsDocuments()
        {
            return await _ethicsDocuments.GetAll();
        }

        public async Task UpdateEthicsDocument(EthicsDocument ethicsDocument)
        {
            _ethicsDocuments.Update(ethicsDocument);
            await _ethicsDocuments.Save();
        }

        public async Task<bool> CheckIfDocumentExists(string fileName)
        {
            var docs = await _ethicsDocuments.GetAll(m => m.FileName == fileName);
            return docs.Count() != 0;
        }
    }
}
