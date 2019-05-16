using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class DocumentCategoryService : IDocumentCategoryService
    {
        private readonly IRepository<DocumentCategory> _documentCategories;

        public DocumentCategoryService(IRepository<DocumentCategory> documentCategories)
        {
            this._documentCategories = documentCategories;
        }

        public async Task CreateDocumentCategory(DocumentCategory documentCategory)
        {
            _documentCategories.Insert(documentCategory);
            await _documentCategories.Save();
        }

        public async Task DeleteDocumentCategory(DocumentCategory documentCategory)
        {
            await _documentCategories.Delete(documentCategory);
            await _documentCategories.Save();
        }

        public async Task<IEnumerable<DocumentCategory>> GetDocumentCategories()
        {
            return await _documentCategories.GetAll();
        }

        public async Task<DocumentCategory> GetDocumentCategoryById(int id)
        {
            return await _documentCategories.GetById(id);
        }

        public async Task UpdateDocumentCategory(DocumentCategory documentCategory)
        {
            _documentCategories.Update(documentCategory);
            await _documentCategories.Save();
        }
    }
}
