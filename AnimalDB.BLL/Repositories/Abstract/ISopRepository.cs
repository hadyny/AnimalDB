using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface ISopRepository : IRepository<Sop>
    {
        IEnumerable<Sop> GetByCategoryId(int categoryId);
        bool FileNameExists(string fileName);
    }
}
