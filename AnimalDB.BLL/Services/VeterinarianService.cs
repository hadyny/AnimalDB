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
    public class VeterinarianService : IVeterinarianService
    {
        private readonly IUserRepository<Veterinarian> _veterinarians;

        public VeterinarianService(IUserRepository<Veterinarian> veterinarians)
        {
            this._veterinarians = veterinarians;
        }

        public async Task CreateVeterinarian(Veterinarian veterinarian)
        {
            await _veterinarians.Insert(veterinarian);
        }

        public async Task<Veterinarian> GetVeterinarianByUsername(string username)
        {
            return await _veterinarians.GetByUsername(username);
        }

        public async Task DeleteVeterinarian(Veterinarian veterinarian)
        {
            await _veterinarians.Delete(veterinarian);
            await _veterinarians.Save();
        }

        public async Task<Veterinarian> GetVeterinarianById(string id)
        {
            return await _veterinarians.GetById(id);
        }

        public async Task<IEnumerable<Veterinarian>> GetVeterinarians()
        {
            return await _veterinarians.GetAll();
        }

        public async Task UpdateVeterinarian(Veterinarian veterinarian)
        {
            _veterinarians.Update(veterinarian);
            await _veterinarians.Save();
        }

        public async Task SetAuthCookie(string userName)
        {
            using (var db = new AnimalDBContext())
            {
                Veterinarian user = await GetVeterinarianByUsername(userName);
                var AdminManager = new UserManager<Veterinarian>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Veterinarian>(db));
                var adminIdentity = await AdminManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, adminIdentity);
            }
        }
    }
}
