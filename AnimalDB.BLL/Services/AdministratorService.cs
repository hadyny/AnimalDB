using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace AnimalDB.Repo.Services
{
    public sealed class AdministratorService : IAdministratorService
    {
        private readonly IUserRepository<Administrator> _administrators;

        public AdministratorService(IUserRepository<Administrator> administrators)
        {
            _administrators = administrators;
        }

        public async Task CreateAdministrator(Administrator administrator)
        {
            await _administrators.Insert(administrator, Repo.Enums.UserType.Administrator);
        }

        public async Task<Administrator> GetAdministratorByUsername(string userName)
        {
            return await _administrators.GetByUsername(userName);
        }

        public async Task DeleteAdministrator(Administrator administrator)
        {
            await _administrators.Delete(administrator);
            await _administrators.Save();
        }

        public async Task<Administrator> GetAdministratorById(string id)
        {
            return await _administrators.GetById(id);
        }

        public async Task<IEnumerable<Administrator>> GetAdministrators()
        {
            return await _administrators.GetAll();
        }

        public async Task UpdateAdministrator(Administrator administrator)
        {
            _administrators.Update(administrator);
            await _administrators.Save();
        }

        public async Task SetAuthCookie(string userName)
        {
            using (var db = new AnimalDBContext())
            {
                Administrator user = await GetAdministratorByUsername(userName);
                var AdminManager = new UserManager<Administrator>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Administrator>(db));
                var adminIdentity = await AdminManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, adminIdentity);
            }
        }
    }
}