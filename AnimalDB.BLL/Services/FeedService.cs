using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class FeedService : IFeedService
    {
        private readonly IRepository<Feed> _feeds;

        public FeedService(IRepository<Feed> feeds)
        {
            this._feeds = feeds;
        }

        public async Task CreateFeed(Feed feed)
        {
            _feeds.Insert(feed);
            await _feeds.Save();
        }

        public async Task DeleteFeed(Feed feed)
        {
            _feeds.Update(feed);
            await _feeds.Save();
        }

        public async Task<Feed> GetFeedById(int id)
        {
            return await _feeds.GetById(id);
        }

        public async Task<IEnumerable<Feed>> GetFeeds()
        {
            return await _feeds.GetAll();
        }

        public async Task UpdateFeed(Feed feed)
        {
            _feeds.Update(feed);
            await _feeds.Save();
        }
    }
}
