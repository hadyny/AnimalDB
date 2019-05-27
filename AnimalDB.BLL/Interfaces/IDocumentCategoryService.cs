using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IDocumentCategoryService
    {
        Task<IEnumerable<DocumentCategory>> GetDocumentCategories();

        Task<IEnumerable<DocumentCategory>> GetRootDocumentCategories();

        Task<IEnumerable<DocumentCategory>> GetDocumentCategoriesByParentId(int? id);

        Task CreateDocumentCategory(DocumentCategory documentCategory);

        Task<DocumentCategory> GetDocumentCategoryById(int id);

        Task UpdateDocumentCategory(DocumentCategory documentCategory);

        Task DeleteDocumentCategory(DocumentCategory documentCategory);

        IEnumerable<DocumentCategory> GetParentHierarchy(DocumentCategory documentCategory);
    }
}
