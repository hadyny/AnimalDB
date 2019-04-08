using System.ComponentModel.DataAnnotations;

namespace AnimalDB.Repo.Enums
{
    public enum EuthanasiaRequiredEnum
    {
        No,
        Yes,
        [Display(Name = "Awaits discussion")]
        Awaits_Discussion
    }
}
