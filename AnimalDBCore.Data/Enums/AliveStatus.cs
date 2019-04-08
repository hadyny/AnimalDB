using System.ComponentModel.DataAnnotations;

namespace  AnimalDBCore.Core.Enums
{
    public enum AliveStatus
    {
        [Display(Name = "Retained by your institution")]
        Retained_ByYourInstitution,
        [Display(Name = "Returned to owner")]
        Returned_ToOwner,
        [Display(Name = "Returned to the wild")]
        Released_ToTheWild,
        [Display(Name = "Disposed of to others")]
        DisposedOf_ToOthers
    }
}
