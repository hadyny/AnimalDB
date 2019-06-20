using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IDocumentRepository : IRepository<Document>
    {
        bool Exists(string fileName);
        IEnumerable<Document> GetByCategoryId(int? categoryId);
    }
}
