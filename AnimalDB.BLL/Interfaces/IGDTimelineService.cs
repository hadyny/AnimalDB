using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IGDTimelineService
    {
        Task<IEnumerable<GDTimeline>> GetGDTimelines();

        Task CreateGDTimeline(GDTimeline gDTimeline);

        Task<GDTimeline> GetGDTimelineById(int id);

        Task UpdateGDTimeline(GDTimeline gDTimeline);

        Task DeleteGDTimeline(GDTimeline gDTimeline);

        Task<List<string>> GetUnusedTimelineList();

        Task<bool> CheckIfTimeLineExists(GDTimeline gDTimeline);
    }
}
