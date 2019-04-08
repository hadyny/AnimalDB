using System.ComponentModel.DataAnnotations;

namespace AnimalDB.Repo.Enums
{
    public enum TreatmentRequiredEnum
    {
        No,
        Yes,
        [Display(Name = "Monitor by technician")]
        Monitor_by_Technician,
        [Display(Name = "Monitor by research personnel")]
        Monitor_by_Research_Personnel
    }
}
