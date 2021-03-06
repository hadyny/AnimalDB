﻿using System.ComponentModel.DataAnnotations;

namespace AnimalDB.Repo.Enums
{
    public enum CauseOfDeathEnum
    {
        Culled,
        [Display(Name = "Natural causes")]
        Natural_Causes,
        [Display(Name = "Died during surgery")]
        Died_During_Surgery,
        [Display(Name = "Died after surgery")]
        Died_After_Surgery,
        [Display(Name = "Died post op")]
        Died_Post_Op,
        [Display(Name = "Euthanased for slice research")]
        Euthanased_for_Slice_Research,
        Histology,
        [Display(Name = "Killed to use body or tissues")]
        Killed_to_use_body_or_tissues
    }
}
