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
    public class InvestigatorService : IInvestigatorService
    {
        private readonly IUserRepository<Investigator> _investigators;

        public InvestigatorService(IUserRepository<Investigator> investigators)
        {
            this._investigators = investigators;
        }

        public async Task CreateInvestigator(Investigator investigator)
        {
            await _investigators.Insert(investigator, Repo.Enums.UserType.Investigator);
        }

        public async Task<Investigator> GetInvestigatorByUsername(string username)
        {
            return await _investigators.GetByUsername(username);
        }

        public async Task DeleteInvestigator(Investigator investigator)
        {
            await _investigators.Delete(investigator);
            await _investigators.Save();
        }

        public async Task<Investigator> GetInvestigatorById(string id)
        {
            return await _investigators.GetById(id);
        }

        public async Task<IEnumerable<Investigator>> GetInvestigators()
        {
            return await _investigators.GetAll();
        }

        public async Task UpdateInvestigator(Investigator investigator)
        {
            _investigators.Update(investigator);
            await _investigators.Save();
        }

        public async Task SetAuthCookie(string userName)
        {
            using (var db = new AnimalDBContext())
            {
                Investigator user = await GetInvestigatorByUsername(userName);
                var AdminManager = new UserManager<Investigator>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Investigator>(db));
                var adminIdentity = await AdminManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, adminIdentity);
            }
        }
    }
}
