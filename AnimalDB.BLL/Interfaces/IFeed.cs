using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IFeed
    {
        IEnumerable<Feed> GetFeeds();

        Task CreateFeed(Feed feed);

        Task<Feed> GetFeedById(int id);

        Task UpdateFeed(Feed feed);

        Task DeleteFeed(Feed feed);
    }
}
