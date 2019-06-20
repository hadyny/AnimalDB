using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class DocumentCategoryRepository : Repository<DocumentCategory>, IDocumentCategoryRepository
    {

        public DocumentCategoryRepository(AnimalDBContext context) : base(context)
        {
        }

        public override void Insert(DocumentCategory documentCategory)
        {
            int? parentCategoryId = documentCategory.ParentCategory_Id;
            documentCategory.ParentCategory_Id = null;
            Context.DocumentCategories.Add(documentCategory);
            Context.SaveChanges();
            documentCategory.ParentCategory_Id = parentCategoryId;
            Context.Entry(documentCategory).State = System.Data.Entity.EntityState.Modified;
        }

        public IEnumerable<DocumentCategory> GetByParentId(int? id)
        {
            return Context.DocumentCategories.Where(m => m.ParentCategory_Id == id).ToList();
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

            return categories.ToList();
        }

        public IEnumerable<DocumentCategory> GetRootDocumentCategories()
        {
            return Context.DocumentCategories.Where(m => m.ParentCategory_Id == null).ToList();
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
