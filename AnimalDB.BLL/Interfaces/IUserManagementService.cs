using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Enums;
using System.DirectoryServices;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IUserManagementService
    {
        Task<string> CreateAnimalUser(AnimalUser user, UserType newUserType);

        SearchResult GetADUser(string username, string password);

        Task<bool> SignInADUserAsync(string userName, string password, UserType userType);

        Task<UserType?> GetUserType(string username);
    }
}