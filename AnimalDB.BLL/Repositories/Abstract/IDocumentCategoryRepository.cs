using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IDocumentCategoryRepository : IRepository<DocumentCategory>
    {
        IEnumerable<DocumentCategory> GetByParentId(int? id);
        IEnumerable<DocumentCategory> GetParentHierarchy(DocumentCategory documentCategory);
        IEnumerable<DocumentCategory> GetRootDocumentCategories();
    }
}
