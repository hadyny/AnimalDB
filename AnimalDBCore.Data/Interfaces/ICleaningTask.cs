using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface ICleaningTask
    {
        IEnumerable<CleaningTask> GetCleaningTasks();

        Task CreateCleaningTask(CleaningTask cleaningTask);

        Task<CleaningTask> GetCleaningTaskById(int id);

        Task UpdateCleaningTask(CleaningTask cleaningTask);

        Task DeleteCleaningTask(CleaningTask cleaningTask);
    }
}
