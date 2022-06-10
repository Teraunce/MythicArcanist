﻿using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static MythicArcanist.Main;

namespace MythicArcanist.NewContent.LoremasterSecrets
{
    static class LoremasterPaladinSpellSecret
    {
        public static void Add()
        {
            var LoremasterSecretSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("beeb25d7a7732e14f9986cdb79acecfc");

            var LoremasterPaladinSpellSecret = Helpers.CreateBlueprint<BlueprintFeatureSelection>(ThisModContext, "LoremasterPaladinSpellSecret", bp =>
            {

            });
        }
    }
}


namespace MythicArcanist.NewContent.ArcanistExploits
{
    static class MartialTraining
    {
        public static void Add()
        {
            var icon = AssetLoader.LoadInternal(ThisModContext, folder: "ArcanistExploits", file: "Icon_MartialTraining.png");


            var ArcanistExploitMartialTraining = Helpers.CreateBlueprint<BlueprintFeature>(ThisModContext, "ArcanistExploitMartialTraining", bp =>
            {
                bp.SetName(ThisModContext, "Martial Training");
                bp.SetDescription(ThisModContext, "The arcanist's base attack bonus increases by 2, to a maximum of her level.");
                bp.m_Icon = icon;
                bp.Groups = new FeatureGroup[] { FeatureGroup.ArcanistExploit };
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.AddComponent<AEMatrtialTraining>(c => c.Value = 2);
            });
            if (ThisModContext.ThirdParty.ArcanistExploits.IsDisabled("MartialTraining")) { return; }
            FeatTools.AddAsArcanistExploit(ArcanistExploitMartialTraining);
        }
    }
    static class MartialTrainingGreater
    {
        public static void Add()
        {
            var ArcanistGreaterExploitsFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("c7536b93f17c70d4fa3a8cf9aa76bfb7");
            var icon = AssetLoader.LoadInternal(ThisModContext, folder: "ArcanistExploits", file: "Icon_MartialTraining.png");


            var ArcanistExploitMartialTrainingGreater = Helpers.CreateBlueprint<BlueprintFeature>(ThisModContext, "ArcanistExploitMartialTrainingGreater", bp =>
            {
                bp.SetName(ThisModContext, "Greater Martial Training");
                bp.SetDescription(ThisModContext, "The arcanist's base attack bonus increases by 3, to a maximum of her level. The arcanist must already have the martial training exploit to select this exploit.");
                bp.m_Icon = icon;
                bp.Groups = new FeatureGroup[] { FeatureGroup.ArcanistExploit };
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.AddComponent<AEMatrtialTraining>(c => c.Value = 3);
                bp.AddPrerequisiteFeature(ArcanistGreaterExploitsFeature);
                bp.AddPrerequisiteFeature(BlueprintTools.GetModBlueprint<BlueprintFeature>(ThisModContext, "ArcanistExploitMartialTraining"));
            });
            if (ThisModContext.ThirdParty.ArcanistExploits.IsDisabled("MartialTraining")) { return; }
            FeatTools.AddAsArcanistExploit(ArcanistExploitMartialTrainingGreater);
        }
    }
}
