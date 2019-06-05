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
    public class TechnicianService : ITechnicianService
    {
        private readonly IUserRepository<Technician> _technicians;

        public TechnicianService(IUserRepository<Technician> technicians)
        {
            this._technicians = technicians;
        }

        public async Task<Technician> GetTechnicianByUsername(string username)
        {
            return await _technicians.GetByUsername(username);
        }

        public async Task CreateTechnician(Technician technician)
        {
            await _technicians.Insert(technician, Repo.Enums.UserType.Technician);
        }

        public async Task DeleteTechnician(Technician technician)
        {
            await _technicians.Delete(technician);
            await _technicians.Save();
        }

        public async Task<Technician> GetTechnicianById(string id)
        {
            return await _technicians.GetById(id);
        }

        public async Task<IEnumerable<Technician>> GetTechnicians()
        {
            return await _technicians.GetAll();
        }

        public async Task UpdateTechnician(Technician technician)
        {
            _technicians.Update(technician);
            await _technicians.Save();
        }

        public async Task SetAuthCookie(string userName)
        {
            using (var db = new AnimalDBContext())
            {
                Technician user = await GetTechnicianByUsername(userName);
                var AdminManager = new UserManager<Technician>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Technician>(db));
                var adminIdentity = await AdminManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, adminIdentity);
            }
        }
    }
}
