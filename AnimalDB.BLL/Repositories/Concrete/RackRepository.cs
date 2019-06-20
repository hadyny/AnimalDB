using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class RackRepository : Repository<Rack>, IRackRepository
    {
        public RackRepository(AnimalDBContext context) : base(context)
        {
        }

        public IEnumerable<Rack> GetByRoomId(int roomId)
        {
            return Context.Racks.Where(m => m.Room_Id == roomId).ToList();
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
