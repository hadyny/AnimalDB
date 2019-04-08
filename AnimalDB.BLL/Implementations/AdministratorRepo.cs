using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AnimalDB.Repo.Implementations
{
    public sealed class AdministratorRepo : IAdministrator
    {
        private readonly AnimalDBContext db;

        public AdministratorRepo()
        {
            this.db = new AnimalDBContext();
        }

        public AdministratorRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateAdministrator(Administrator administrator)
        {
            var usermanager = new UserManager<Administrator>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Administrator>(db));
            var result = await usermanager.CreateAsync(administrator, "Password not required");
            if (result.Succeeded)
            {
                usermanager.AddToRole(administrator.Id, "Administrator");
            }
        }

        public Administrator GetAdministratorByUsername(string userName)
        {
            return db.Administrators.SingleOrDefault(m => m.UserName == userName);
        }

        public async Task DeleteAdministrator(Administrator administrator)
        {
            var usermanager = new UserManager<Administrator>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Administrator>(db));
            if (usermanager.IsInRole(administrator.Id, "Investigator"))
            {
                usermanager.RemoveFromRole(administrator.Id, "Investigator");
            }
            usermanager.RemoveFromRole(administrator.Id, "Administrator");
            await usermanager.DeleteAsync(administrator);
            await db.SaveChangesAsync();
        }

        public async Task<Administrator> GetAdministratorById(string id)
        {
            return await db.Administrators.FindAsync(id);
        }

        public IEnumerable<Administrator> GetAdministrators()
        {
            return db.Administrators;
        }

        public async Task UpdateAdministrator(Administrator administrator)
        {
            db.Entry(administrator).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task SetAuthCookie(string userName)
        {
            Administrator user = GetAdministratorByUsername(userName);
            var AdminManager = new UserManager<Administrator>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Administrator>(db));
            var adminIdentity = await AdminManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, adminIdentity);
        }
    }
}
