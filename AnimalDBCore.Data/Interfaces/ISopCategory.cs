using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface ISopCategory
    {
        IEnumerable<SopCategory> GetSopCategories();

        Task CreateSopCategory(SopCategory sopCategory);

        Task<SopCategory> GetSopCategoryById(int id);

        Task UpdateSopCategory(SopCategory sopCategory);

        Task DeleteSopCategory(SopCategory sopCategory);
    }
}
