using System.ComponentModel.DataAnnotations;

namespace AnimalDB.Repo.Enums
{
    public enum TreatmentPerformedByEnum
    {
        Technician,
        [Display(Name = "Research Personnel")]
        Research_Personnel
    }
}
