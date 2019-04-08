using System.ComponentModel.DataAnnotations;

namespace  AnimalDBCore.Core.Enums
{
    public enum SourceType
    {
        [Display(Name = "Breeding unit")]
        BreedingUnit,
        Commercial,
        Farm,
        [Display(Name = "Born during project")]
        BornDuringProject,
        Captured,
        [Display(Name = "Imported into NZ")]
        ImportedIntoNZ,
        [Display(Name = "Public sources")]
        PublicSources
    }
}
