using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Enums;
using AnimalDB.Repo.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.DirectoryServices;
using System.Threading.Tasks;
using System.Web;

namespace AnimalDB.Repo.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IAdministratorService _administrators;
        private readonly ITechnicianService _technicians;
        private readonly IVeterinarianService _veterinarians;
        private readonly IInvestigatorService _investigators;
        private readonly IStudentService _students;

        public UserManagementService(IAdministratorService administrators,
                              ITechnicianService technicians,
                              IVeterinarianService veterinarians,
                              IInvestigatorService investogators,
                              IStudentService students)
        {
            _administrators = administrators;
            _technicians = technicians;
            _veterinarians = veterinarians;
            _investigators = investogators;
            _students = students;
        }

        public async Task<string> CreateAnimalUser(AnimalUser user, UserType newUserType)
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

            switch (newUserType)
            {
                case UserType.Administrator:
                    await _administrators.CreateAdministrator(user as Administrator);
                    break;
                case UserType.Investigator:
                    await _investigators.CreateInvestigator(user as Investigator);
                    break;
                case UserType.Student:
                    await _students.CreateStudent(user as Student);
                    break;
                case UserType.Technician:
                    await _technicians.CreateTechnician(user as Technician);
                    break;
                case UserType.Veterinarian:
                    await _veterinarians.CreateVeterinarian(user as Veterinarian);
                    break;
             
            }

            return null;
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

            using (var db = new AnimalDBContext())
            {
                switch (userType)
                {
                    case UserType.Administrator:
                        await _administrators.SetAuthCookie(userName);
                        break;
                    case UserType.Technician:
                        await _technicians.SetAuthCookie(userName);
                        break;
                    case UserType.Veterinarian:
                        await _veterinarians.SetAuthCookie(userName);
                        break;
                    case UserType.Investigator:
                        await _investigators.SetAuthCookie(userName);
                        break;
                    case UserType.Student:
                        await _students.SetAuthCookie(userName);
                        break;
                }
            }
            return true;
        }

        public async Task<UserType?> GetUserType(string username)
        {
            if (await _administrators.GetAdministratorByUsername(username) != null)
            {
                return UserType.Administrator;
            }
            else if (await _technicians.GetTechnicianByUsername(username) != null)
            {
                return UserType.Technician;
            }
            else if (await _veterinarians.GetVeterinarianByUsername(username) != null)
            {
                return UserType.Veterinarian;
            }
            else if (await _investigators.GetInvestigatorByUsername(username) != null)
            {
                return UserType.Investigator;
            }
            else if (await _students.GetStudentByUsername(username) != null)
            {
                return UserType.Student;
            }

            return null;
        }
    }
}