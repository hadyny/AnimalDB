using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IRackRepository : IRepository<Rack>
    {
        IEnumerable<Rack> GetByRoomId(int roomId);
    }
}
