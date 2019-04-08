using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
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
