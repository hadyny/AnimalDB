using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class ApprovalNumberRepository : Repository<ApprovalNumber>, IApprovalNumberRepository
    {
        public ApprovalNumberRepository(AnimalDBContext context) : base(context)
        {
        }
    }
}
