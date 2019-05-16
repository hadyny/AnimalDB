using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AnimalDB.Repositories.Concrete
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AnimalDBContext db;
        private readonly DbSet<T> table = null;

        public Repository()
        {
            db = new AnimalDBContext();
            table = db.Set<T>();
        }

        public Repository(AnimalDBContext _db)
        {
            db = _db;
            table = db.Set<T>();
        }

        public async Task Delete(object id)
        {
            T existing = await table.FindAsync(id);

            if (db.Entry(existing).State == EntityState.Detached)
            {
                table.Attach(existing);
            }

            
            table.Remove(existing);
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T,bool>> filter = null)
        {
            IQueryable<T> query = table;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            
            return await query.ToListAsync();
        }

        public async Task<T> GetById(object id)
        {
            return await table.FindAsync(id);
        }

        public void Insert(T obj)
        {
            table.Add(obj);
        }

        public async Task Save()
        {
            await db.SaveChangesAsync();
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            db.Entry(obj).State = EntityState.Modified;
        }
    }
}
