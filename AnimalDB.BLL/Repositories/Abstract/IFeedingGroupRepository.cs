using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IFeedingGroupRepository : IRepository<FeedingGroup>
    {
        IEnumerable<FeedingGroup> GetByRoomId(int roomId);
        Task UpdateFeedingGroupPage(FeedingGroup model);
        IEnumerable<Animal> GetAnimalsInFeedingGroup(int feedingGroupId);
        Task RemoveAnimalFromFeedingGroup(int animalId);
        Task<int?> GetByAnimalId(int animalId);
    }
}
