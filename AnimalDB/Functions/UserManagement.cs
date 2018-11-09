using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Enums;
using AnimalDB.Repo.Implementations;
using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.DirectoryServices;
using System.Threading.Tasks;
using System.Web;

namespace AnimalDB.Functions
{
    public static class UserManagement
    {
        public static async Task<string> CreateAnimalUser(AnimalUser user, UserType newUserType)
        {
            var userType = HelperFunctions.GetUserType(user.UserName);
            
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
                    await new AdministratorRepo().CreateAdministrator(user as Administrator);
                    break;
                case UserType.Investigator:
                    await new InvestigatorRepo().CreateInvestigator(user as Investigator);
                    break;
                case UserType.Student:
                    await new StudentRepo().CreateStudent(user as Student);
                    break;
                case UserType.Technician:
                    await new TechnicianRepo().CreateTechnician(user as Technician);
                    break;
                case UserType.Veterinarian:
                    await new VeterinarianRepo().CreateVeterinarian(user as Veterinarian);
                    break;
             
            }

            return null;
        }

        private static SearchResult GetADUser(string username, string password)
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
            catch (Exception ex)
            {
                var n = ex;
            }
            return null;
        }

        public static async Task<bool> SignInADUserAsync(string userName, string password, UserType userType)
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
                        await new AdministratorRepo().SetAuthCookie(userName);
                        break;
                    case UserType.Technician:
                        await new TechnicianRepo().SetAuthCookie(userName);
                        break;
                    case UserType.Veterinarian:
                        await new VeterinarianRepo().SetAuthCookie(userName);
                        break;
                    case UserType.Investigator:
                        await new InvestigatorRepo().SetAuthCookie(userName);
                        break;
                    case UserType.Student:
                        await new StudentRepo().SetAuthCookie(userName);
                        break;
                }
            }
            return true;
        }
    }
}