using AnimalDBCore.Infrastructure.Contexts;
using  AnimalDBCore.Core.Entities;
using  AnimalDBCore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDBCore.Infrastructure.Data
{
    public class GroupRepo : IGroup, IDisposable
    {
        private AnimalDBContext db;

        public GroupRepo()
        {
            this.db = new AnimalDBContext();
        }
        public GroupRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateGroup(Group group)
        {
            db.Groups.Add(group);
            await db.SaveChangesAsync();
        }

        public async Task DeleteGroup(Group group)
        {
            if (db.Entry(group).State == EntityState.Detached)
            {
                db.Groups.Attach(group);
            }
            foreach (var animal in db.Animals.Where(m => m.Group_Id == group.Id))
            {
                animal.Group_Id = null;
            }
            db.Groups.Remove(group);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<Group> GetGroupById(int id)
        {
            return await db.Groups.FindAsync(id);
        }

        public IEnumerable<Group> GetGroups()
        {
            return db.Groups.ToList();
        }

        public async Task UpdateGroup(Group group)
        {
            db.Entry(group).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public IEnumerable<Group> GetGroupsByFeedingGroupId(int feedingGroupId)
        {

            return db.Groups
                .Where(m => m.FeedingGroup_Id == feedingGroupId).ToList();
        }

        public async Task RemoveAnimalFromGroup(int animalId, int groupId)
        {
            var animal = await db.Animals.FindAsync(animalId);
            var group = await db.Groups.FindAsync(groupId);
            group.Animals.Remove(animal);
            db.Entry(group).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
