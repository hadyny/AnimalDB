using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class EthicsNumberRepository : Repository<EthicsNumber>, IEthicsNumberRepository
    {
        public EthicsNumberRepository(AnimalDBContext context) : base(context)
        {
        }

        public IEnumerable<EthicsNumberHistory> GetByEthicsId(int ethicsId)
        {
           return Context
                .EthicsNumberHistories
                .Where(m =>
                            m.Ethics_Id == ethicsId &&
                            m.Animal
                                .EthicsNumbers
                                .OrderByDescending(n => n.Timestamp)
                                .FirstOrDefault().Id == m.Id)
                .ToList();
        }

        public void Archive(EthicsNumber ethicsNumber)
        {
            ethicsNumber.Archived = true;
            Context.Entry(ethicsNumber).State = System.Data.Entity.EntityState.Modified;
        }

        public EthicsNumber GetByName(string name)
        {
            return Context.EthicsNumbers.SingleOrDefault(m => m.Text == name);
        }

        public IEnumerable<EthicsNumber> GetArchived()
        {
            return Context.EthicsNumbers.Where(m => m.Archived).ToList();
        }

        public IEnumerable<EthicsNumberHistory> GetHistoryByEthicsId(int ethicsId)
        {
            return Context.EthicsNumberHistories.Where(m =>
                            m.Ethics_Id == ethicsId &&
                            m.Animal
                                .EthicsNumbers
                                .OrderByDescending(n => n.Timestamp)
                                .FirstOrDefault().Id == m.Id)
                .ToList();
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
