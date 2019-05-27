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
            int? parentCategoryId = documentCategory.ParentCategory_Id;
            documentCategory.ParentCategory_Id = null;
            _documentCategories.Insert(documentCategory);
            await _documentCategories.Save();
            documentCategory.ParentCategory_Id = parentCategoryId;
            await UpdateDocumentCategory(documentCategory);
        }

        public async Task DeleteDocumentCategory(DocumentCategory documentCategory)
        {
            await _documentCategories.Delete(documentCategory.Id);
            await _documentCategories.Save();
        }

        public async Task<IEnumerable<DocumentCategory>> GetDocumentCategories()
        {
            return await _documentCategories.GetAll();
        }

        public async Task<IEnumerable<DocumentCategory>> GetDocumentCategoriesByParentId(int? id)
        {
            return await _documentCategories.GetAll(m => m.ParentCategory_Id == id);
        }

        public async Task<DocumentCategory> GetDocumentCategoryById(int id)
        {
            return await _documentCategories.GetById(id);
        }

        public IEnumerable<DocumentCategory> GetParentHierarchy(DocumentCategory documentCategory)
        {
            var categories = new List<DocumentCategory>();
            DocumentCategory parent = documentCategory?.ParentCategory;

            while (parent != null)
            {
                categories.Add(parent);
                parent = parent.ParentCategory;
            }

            return categories;
        }

        public async Task<IEnumerable<DocumentCategory>> GetRootDocumentCategories()
        {
            return await _documentCategories.GetAll(m => m.ParentCategory_Id == null);
        }

        public async Task UpdateDocumentCategory(DocumentCategory documentCategory)
        {
            _documentCategories.Update(documentCategory);
            await _documentCategories.Save();
        }
    }
}
