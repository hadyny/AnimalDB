using System.ComponentModel.DataAnnotations;

namespace  AnimalDBCore.Core.Enums
{
    public enum CauseOfDeathEnum
    {
        Culled,
        [Display(Name = "Natural causes")]
        Natural_Causes,
        [Display(Name = "Died during surgery")]
        Died_During_Surgery,
        [Display(Name = "Died after surgery")]
        Died_After_Surgery,
        [Display(Name = "Died post op")]
        Died_Post_Op,
        [Display(Name = "Euthanased for slice research")]
        Euthanased_for_Slice_Research,
        Histology,
    }
}
