using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnimalDB.Repo.Enums
{
    public enum Grading
    {
        [Display(Name = "A - No Impact")]
        NoSuffering,
        [Display(Name = "B - Little Impact")]
        LittleSuffering,
        [Display(Name = "C - Moderate Impact")]
        ModerateSuffering,
        [Display(Name = "D - High Impact")]
        SevereSuffering,
        [Display(Name = "E - Very High Impact")]
        VerySevereSuffering
    }
}
