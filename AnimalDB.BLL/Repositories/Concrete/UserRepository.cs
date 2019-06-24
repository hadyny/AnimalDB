using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Enums;
using AnimalDB.Repo.Repositories.Abstract;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repositories.Concrete
{
    public class UserRepository<T> : IUserRepository<T> where T : AnimalUser
    {
        protected readonly AnimalDBContext db;
        private readonly DbSet<T> table = null;

        public UserRepository()
        {
            db = new AnimalDBContext();
            table = db.Set<T>();
        }

        public UserRepository(AnimalDBContext _db)
        {
            db = _db;
            table = db.Set<T>();
        }

        public virtual async Task Delete(T obj)
        {
            var usermanager = new UserManager<T>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<T>(db));
            var roles = await usermanager.GetRolesAsync(obj.Id);
            await usermanager.RemoveFromRolesAsync(obj.Id, roles.ToArray());
            await usermanager.DeleteAsync(obj);
        }

        public async Task<IEnumerable<T>> Get()
        {
            return await table.ToListAsync();
        }

        public async Task<T> GetById(object id)
        {
            return await table.FindAsync(id);
        }

        public async Task<T> GetByUsername(string name)
        {
            return await table.SingleOrDefaultAsync(m => m.UserName.Equals(name, System.StringComparison.OrdinalIgnoreCase));
        }

        public async Task Insert(T obj, UserType userType)
        {
            var usermanager = new UserManager<T>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<T>(db));
            var result = await usermanager.CreateAsync(obj, "Password not required");
            if (result.Succeeded)
            {
                usermanager.AddToRole(obj.Id, userType.ToString());
            }
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            db.Entry(obj).State = EntityState.Modified;
        }
    }
}
