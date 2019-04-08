using System.ComponentModel.DataAnnotations;

namespace  AnimalDBCore.Core.Enums
{
    public enum TreatmentPerformedByEnum
    {
        Technician,
        [Display(Name = "Research Personnel")]
        Research_Personnel
    }
}
