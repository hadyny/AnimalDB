using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        public DocumentRepository(AnimalDBContext context) : base(context)
        {
        }

        public bool Exists(string fileName)
        {
            var docs = Context.Documents.Where(m => m.FileName == fileName);
            return docs.Count() != 0;
        }

        public IEnumerable<Document> GetByCategoryId(int? categoryId)
        {
            return Context.Documents.Where(m => m.Category_Id == categoryId).ToList();
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
