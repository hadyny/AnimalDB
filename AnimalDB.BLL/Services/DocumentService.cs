using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IRepository<Document> _documents;

        public DocumentService(IRepository<Document> documents)
        {
            this._documents = documents;
        }

        public async Task CreateDocument(Document document)
        {
            _documents.Insert(document);
            await _documents.Save();
        }

        public async Task DeleteDocument(Document document)
        {
            await _documents.Delete(document);
            await _documents.Save();
        }

        public async Task<bool> DoesDocumentFileNameExist(string fileName)
        {
            var docs = await _documents.GetAll(m => m.FileName == fileName);
            return docs.Count() != 0;
        }

        public async Task<Document> GetDocumentById(int id)
        {
            return await _documents.GetById(id);
        }

        public async Task<IEnumerable<Document>> GetDocuments()
        {
            return await _documents.GetAll();
        }

        public async Task<IEnumerable<Document>> GetDocumentsByCategoryId(int categoryId)
        {
            var docs = await GetDocuments();
            return docs.Where(m => m.Category_Id == categoryId);
        }

        public async Task UpdateDocument(Document document)
        {
            _documents.Update(document);
            await _documents.Save();
        }        
    }
}
