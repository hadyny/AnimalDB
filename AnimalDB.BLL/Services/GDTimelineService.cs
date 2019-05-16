using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class GDTimelineService : IGDTimelineService
    {
        private readonly IRepository<GDTimeline> _gDTimelines;
        private readonly IRepository<SurgicalNote> _surgicalNotes;

        public GDTimelineService(IRepository<GDTimeline> gDTimelines, IRepository<SurgicalNote> surgicalNotes)
        {
            this._gDTimelines = gDTimelines;
            this._surgicalNotes = surgicalNotes;
        }

        public async Task CreateGDTimeline(GDTimeline gDTimeline)
        {
            _gDTimelines.Insert(gDTimeline);
            await _gDTimelines.Save();
        }

        public async Task<List<string>> GetUnusedTimelineList()
        {
            var notes = await _surgicalNotes.GetAll();
            var timelines = await _gDTimelines.GetAll();
            return timelines
                        .Where(m => notes.Count(n => n.GDTimeline_Id == m.Id) == 0)
                        .Select(m => m.Description)
                        .ToList();
        }

        public async Task DeleteGDTimeline(GDTimeline gDTimeline)
        {
            await _gDTimelines.Delete(gDTimeline);
            await _gDTimelines.Save();
        }

        public async Task<GDTimeline> GetGDTimelineById(int id)
        {
            return await _gDTimelines.GetById(id);
        }

        public async Task<IEnumerable<GDTimeline>> GetGDTimelines()
        {
            return await _gDTimelines.GetAll();
        }

        public async Task UpdateGDTimeline(GDTimeline gDTimeline)
        {
            _gDTimelines.Update(gDTimeline);
            await _gDTimelines.Save();
        }

        public async Task<bool> CheckIfTimeLineExists(GDTimeline gDTimeline)
        {
            var timelines = await _gDTimelines.GetAll(m => m.Description == gDTimeline.Description);
            return timelines.Count() != 0;
        }
    }
}
