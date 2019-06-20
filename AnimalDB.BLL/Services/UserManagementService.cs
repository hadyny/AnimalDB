using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Enums;
using AnimalDB.Repo.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Configuration;
using System.DirectoryServices;
using System.Threading.Tasks;
using System.Web;

namespace AnimalDB.Repo.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public SearchResult GetADUser(string username, string password)
        {
            DirectoryEntry entry = new DirectoryEntry("LDAP://psy.local", username, password);

            try
            {
                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry)
                {
                    Filter = "(SAMAccountName=" + username.ToLower() + ")"
                };
                return search.FindOne();
            }
            catch (Exception)
            {
            }
            return null;
        }

        public async Task<bool> SignInADUserAsync(string userName, string password, UserType userType)
        {
            var AuthenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            var result = GetADUser(userName, password);

            if (result == null) return false;

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            await SetAuthCookie(userName, userType);

            return true;
        }

        public async Task SetAuthCookie(string userName, UserType userType)
        {
            //using (var db = new AnimalDBContext())
            //{
            //    Administrator user = await GetAdministratorByUsername(userName);
            //    var AdminManager = new UserManager<Administrator>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Administrator>(db));
            //    var adminIdentity = await AdminManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            //    HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, adminIdentity);
            //}

            AnimalUser user = null;

            switch (userType)
            {
                case UserType.Administrator:
                    user = await _unitOfWork.Administrators.GetByUsername(userName);
                    break;
                case UserType.Investigator:
                    user = await _unitOfWork.Investigators.GetByUsername(userName);
                    break;
                case UserType.Student:
                    user = await _unitOfWork.Students.GetByUsername(userName);
                    break;
                case UserType.Technician:
                    user = await _unitOfWork.Technicians.GetByUsername(userName);
                    break;
                case UserType.Veterinarian:
                    user = await _unitOfWork.Veterinarians.GetByUsername(userName);
                    break;
            }

            var manager = new UserManager<AnimalUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<AnimalUser>(_unitOfWork.Context));
            var identity = await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
        }

        public async Task<string> Register(AnimalUser user, UserType newUserType)
        {
            var userType = await GetUserType(user.UserName);

            if (userType == UserType.Administrator && newUserType == UserType.Investigator)
            {
                user.UserName += "_Investigator";
            }
            else if (userType != null) // or is admin and investigator
            {
                return "User " + user.UserName + " is already a " + Enum.GetName(typeof(UserType), userType.Value);
            }

            DirectoryEntry entry = new DirectoryEntry("LDAP://psy.local", ConfigurationManager.AppSettings["SystemUsername"], ConfigurationManager.AppSettings["SystemPassword"]);
            SearchResult _result = null;
            try
            {
                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry)
                {
                    Filter = "(SAMAccountName=" + user.UserName.Replace("_Investigator", "") + ")"
                };
                search.PropertiesToLoad.Add("givenname");
                search.PropertiesToLoad.Add("sn");
                search.PropertiesToLoad.Add("mail");
                _result = search.FindOne();
            }
            catch (Exception) { }

            if (_result == null)
            {
                return "User " + user.UserName + " does not have an Active Directory account.";
            }
            if (!_result.Properties.Contains("mail"))
            {
                return "User " + user.UserName + " does not have an email address in their Active Directory account.";
            }
            else
            {
                user.Email = _result.Properties["mail"][0].ToString();
                user.FirstName = _result.Properties["givenname"][0].ToString();
                user.LastName = _result.Properties["sn"][0].ToString();
            }


            var usermanager = new UserManager<AnimalUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<AnimalUser>(_unitOfWork.Context));
            var result = await usermanager.CreateAsync(user, "Password not required");
            if (result.Succeeded)
            {
                usermanager.AddToRole(user.Id, newUserType.ToString());
            }

            return null;
        }

        public async Task <UserType?> GetUserType(string username)
        {
            if (await _unitOfWork.Administrators.GetByUsername(username) != null)
            {
                return UserType.Administrator;
            }
            else if (await _unitOfWork.Technicians.GetByUsername(username) != null)
            {
                return UserType.Technician;
            }
            else if (await _unitOfWork.Veterinarians.GetByUsername(username) != null)
            {
                return UserType.Veterinarian;
            }
            else if (await _unitOfWork.Investigators.GetByUsername(username) != null)
            {
                return UserType.Investigator;
            }
            else if (await _unitOfWork.Students.GetByUsername(username) != null)
            {
                return UserType.Student;
            }

            return null;
        }
    }
}