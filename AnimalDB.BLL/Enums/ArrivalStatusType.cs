using System.ComponentModel.DataAnnotations;

namespace AnimalDB.Repo.Enums
{
    public enum ArrivalStatusType
    {
        [Display(Name = "Normal/Conventional")]
        Normal_Conventional,
        [Display(Name = "SPF/Germ free")]
        SPF_GermFree,
        Diseased,
        [Display(Name = "Transgenic/Chimera")]
        Transgenic_Chimera,
        [Display(Name = "Protected species")]
        ProtectedSpecies,
        [Display(Name = "Unborn/prehatched")]
        Unborn_Prehatched,
        Other
    }
}
