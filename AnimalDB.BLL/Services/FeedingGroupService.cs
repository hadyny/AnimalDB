using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class FeedingGroupService : IFeedingGroupService
    {
        private readonly IRepository<FeedingGroup> _feedingGroups;
        private readonly IRepository<Animal> _animals;
        private readonly IRepository<Feed> _feeds;

        public FeedingGroupService(IRepository<FeedingGroup> feedingGroups, 
                                   IRepository<Animal> animals, 
                                   IRepository<Feed> feeds)
        {
            this._feedingGroups = feedingGroups;
            this._animals = animals;
            this._feeds = feeds;
        }

        public async Task CreateFeedingGroup(FeedingGroup feedingGroup)
        {
            _feedingGroups.Insert(feedingGroup);
            await _feedingGroups.Save();
        }

        public async Task DeleteFeedingGroup(FeedingGroup feedingGroup)
        {
            foreach (var animal in feedingGroup.Animals.ToList())
            {
                animal.FeedingGroup_Id = null;
                animal.Group_Id = null;
                _animals.Update(animal);
            }
            await _animals.Save();
            await _feedingGroups.Delete(feedingGroup);
            await _feedingGroups.Save();
        }

        public async Task<FeedingGroup> GetFeedingGroupById(int id)
        {
            return await _feedingGroups.GetById(id);
        }

        public async Task<IEnumerable<FeedingGroup>> GetFeedingGroups()
        {
            return await _feedingGroups.GetAll();
        }

        public async Task UpdateFeedingGroup(FeedingGroup feedingGroup)
        {
            _feedingGroups.Update(feedingGroup);
            await _feedingGroups.Save();
        }

        public async Task<IEnumerable<FeedingGroup>> GetFeedingGroupsByRoomId(int roomId)
        {
            return await _feedingGroups.GetAll(m => m.Room_Id == roomId);
        }

        public async Task UpdateFeedingGroupPage(FeedingGroup model)
        {
            foreach (var animal in model.Animals)
            {
                var allFeeds = await _feeds.GetAll(m => m.Animal_Id == animal.Id);

                foreach (var feed in animal.Feeds.OrderByDescending(m => m.Date).Distinct())
                {
                    //if (feed.Id == 0)
                    if (allFeeds.Count(m => m.Animal_Id == feed.Animal_Id && m.Date == feed.Date) == 0)
                    {
                        if (feed.FeedAmount != null || feed.Weight != null)
                        {
                            _feeds.Insert(feed);
                        }
                    }
                    else
                    {
                        //var oldFeed = db.Feeds.Find(feed.Id);
                        var oldFeed = allFeeds.First(m => m.Animal_Id == feed.Animal_Id && m.Date == feed.Date);

                        if (feed.FeedAmount == null && feed.Weight == null)
                        {
                            await _feeds.Delete(oldFeed);
                        }
                        else
                        {
                            oldFeed.CombinedFeed = feed.CombinedFeed;
                            oldFeed.FeedAmount = feed.FeedAmount;
                            oldFeed.Weight = feed.Weight;
                        }
                    }
                }

                var a = await _animals.GetById(animal.Id);
                a.LastChecked = DateTime.Now.Date;
            }

            await _animals.Save();
            await _feeds.Save();
            await _feedingGroups.Save();
        }

        public async Task<IEnumerable<Animal>> GetAnimalsInFeedingGroup(int feedingGroupId)
        {
            return await _animals.GetAll(m => m.FeedingGroup_Id == feedingGroupId);
        }

        public async Task RemoveAnimalFromFeedingGroup(int animalId)
        {
            var animal = await _animals.GetById(animalId);
            animal.FeedingGroup_Id = null;
            animal.Group_Id = null;
            _animals.Update(animal);
            await _animals.Save();
        }

        public async Task<int?> GetFeedingGroupByAnimalId(int animalId)
        {
            var animal = await _animals.GetById(animalId);
            return animal?.FeedingGroup_Id;
        }
    }
}
