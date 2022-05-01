﻿using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using static MythicArcanist.Main;

namespace MythicArcanist.NewContent
{
    class ContentAdder
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            [HarmonyPriority(799)]
            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;
                MAContext.Logger.LogHeader("Loading New Content");

                Feats.ExtraMythicAbility.AddExtraMythicAbility();
                Feats.ExtraMythicFeat.AddExtraMythicFeat();
                MythicAbilities.AbundantPreparation.AddAbundantPreparation();
                MythicAbilities.AbundantPreparationImproved.AddAbundantPreparationImproved();
                MythicAbilities.AbundantPreparationGreater.AddAbundantPreparationGreater();
                Spells.MageArmor2.AddMageArmor2();
                Spells.MageArmor3.AddMageArmor3();
                Spells.MageArmor4.AddMageArmor4();
                Spells.MageShield2.AddMageShield2();
            }
        }
    }
}
