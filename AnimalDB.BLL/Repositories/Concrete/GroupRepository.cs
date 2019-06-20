using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(AnimalDBContext context) : base(context)
        {
        }

       
        public override void Delete(Group group)
        {
            foreach (var animal in Context.Animals.Where(m => m.Group_Id == group.Id))
            {
                animal.Group_Id = null;
            }

            Context.Groups.Remove(group);
        }

        public IEnumerable<Group> GetByFeedingGroupId(int feedingGroupId)
        {
            return Context.Groups.Where(m => m.FeedingGroup_Id == feedingGroupId).ToList();
        }

        public async Task RemoveAnimalFromGroup(int animalId, int groupId)
        {
            var animal = await Context.Animals.FindAsync(animalId);
            var group = await Context.Groups.FindAsync(groupId);
            group.Animals.Remove(animal);
            Context.Entry(group).State = System.Data.Entity.EntityState.Modified;
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
