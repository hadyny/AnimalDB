using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Enums;
using System.DirectoryServices;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IUserManagementService
    {
        SearchResult GetADUser(string username, string password);
        Task<bool> SignInADUserAsync(string userName, string password, UserType userType);
        Task<UserType?> GetUserType(string username);
        Task<string> Register(AnimalUser user, UserType newUserType);
    }
}