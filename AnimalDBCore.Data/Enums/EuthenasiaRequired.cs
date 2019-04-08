using System.ComponentModel.DataAnnotations;

namespace  AnimalDBCore.Core.Enums
{
    public enum EuthanasiaRequiredEnum
    {
        No,
        Yes,
        [Display(Name = "Awaits discussion")]
        Awaits_Discussion
    }
}
