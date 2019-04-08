using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using   AnimalDBCore.Core.Entities;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface IDocumentCategory
    {
        IEnumerable<DocumentCategory> GetDocumentCategories();

        Task CreateDocumentCategory(DocumentCategory documentCategory);

        Task<DocumentCategory> GetDocumentCategoryById(int id);

        Task UpdateDocumentCategory(DocumentCategory documentCategory);

        Task DeleteDocumentCategory(DocumentCategory documentCategory);
    }
}
