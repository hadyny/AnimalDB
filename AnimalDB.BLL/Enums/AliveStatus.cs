using System.ComponentModel.DataAnnotations;

namespace AnimalDB.Repo.Enums
{
    public enum AliveStatus
    {
        [Display(Name = "Retained [by your institution]")]
        Retained_ByYourInstitution,
        [Display(Name = "Returned [to owner]")]
        Returned_ToOwner,
        [Display(Name = "Released [to the wild]")]
        Released_ToTheWild,
        [Display(Name = "Disposed of [to others e.g. to works]")]
        DisposedOf_ToOthers,
        [Display(Name = "Rehomed [to others]")]
        RehomedToOthers
    }
}
