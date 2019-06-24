using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AnimalDB.Repositories.Concrete
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AnimalDBContext db;
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

        public virtual async Task Delete(object id)
        {
            T entityToDelete = await table.FindAsync(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            if (db.Entry(entityToDelete).State == EntityState.Detached)
            {
                table.Attach(entityToDelete);
            }
            table.Remove(entityToDelete);
        }

        public virtual async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter = null,
                                                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                string includeProperties = "")
        {
            IQueryable<T> query = table;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public virtual async Task<T> GetById(object id)
        {
            return await table.FindAsync(id);
        }

        public virtual void Insert(T obj)
        {
            table.Add(obj);
        }

        public virtual void Update(T obj)
        {
            table.AddOrUpdate(obj);
        }
    }
}
