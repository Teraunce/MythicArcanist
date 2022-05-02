﻿using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using static MythicArcanist.Main;

namespace MythicArcanist.NewContent.Spells
{
    static class MageArmor3
    {
        public static void AddMageArmor3()
        {
            BlueprintAbility MageArmor = BlueprintTools.GetBlueprint<BlueprintAbility>("9e1ad5d6f87d19e4d8883d63a6e35568");
            BlueprintBuff MageArmorBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("a92acdf18049d784eaa8f2004f5d2304");
            BlueprintBuff MageArmorBuffMythic = BlueprintTools.GetBlueprint<BlueprintBuff>("355be0688dabc21409f37942d637cdab");

            int spellValue = 8;
            string spellName = "MageArmor3";
            string spellDisplay = "Mage Armor III";
            string spellDesc = $"This spell functions like mage armor except you gain a +{spellValue} armor bonus. This bonus does not stack with other sources that grant an armor bonus." +
                "[LONGSTART]\n\nAn invisible but tangible field of force surrounds the subject of a mage armor {g|Encyclopedia:Spell}spell{/g}, providing a +4 armor " +
                "{g|Encyclopedia:Bonus}bonus{/g} to {g|Encyclopedia:Armor_Class}AC{/g}.\nUnlike mundane armor, mage armor entails no armor {g|Encyclopedia:Check}check{/g} " +
                "{g|Encyclopedia:Penalty}penalty{/g}, {g|Encyclopedia:Spell_Fail_Chance}arcane spell failure chance{/g}. or {g|Encyclopedia:Speed}speed{/g} redution. " +
                "Since mage armor is made of force, {g|Encyclopedia:Incorporeal_Touch_Attack}incorporeal{/g} creatures can't bypass it the way they do normal amror.[LONGEND]";
            var icon = AssetLoader.LoadInternal(MAContext, folder: "Spells", file: $"Icon_{spellName}.png");

            var buff = MageArmorBuff.CreateCopy(MAContext, $"{spellName}Buff", bp =>
            {
                bp.SetNameDescription(MAContext, spellDisplay, spellDesc);
                bp.m_Icon = icon;
                bp.GetComponent<AddStatBonus>().Value = spellValue;
                bp.GetComponent<ACBonusAgainstWeaponType>().ArmorClassBonus = spellValue;
            });
            var buffMythic = MageArmorBuffMythic.CreateCopy(MAContext, $"{spellName}BuffMythic", bp =>
            {
                bp.m_Icon = icon;
                bp.GetComponent<ContextRankConfig>().m_StepLevel = spellValue;
            }
            );

            var spell = MageArmor.CreateCopy(MAContext, spellName, bp =>
            {
                bp.SetNameDescription(MAContext, spellDisplay, spellDesc);
                bp.m_Icon = icon;
                bp.RemoveComponents<SpellListComponent>();
                bp.RemoveComponents<RecommendationNoFeatFromGroup>();
                bp.AddComponent<SpellDescriptorComponent>(c =>
                {
                    c.Descriptor = SpellDescriptor.Force;
                });
                bp.GetComponent<AbilityEffectRunAction>()
                    .Actions.Actions
                    .OfType<Conditional>().FirstOrDefault()
                    .IfTrue.Actions
                    .OfType<ContextActionApplyBuff>().FirstOrDefault()
                    .m_Buff = buffMythic.ToReference<BlueprintBuffReference>();
                bp.GetComponent<AbilityEffectRunAction>()
                    .Actions.Actions
                    .OfType<Conditional>().FirstOrDefault()
                    .IfFalse.Actions
                    .OfType<ContextActionApplyBuff>().FirstOrDefault()
                    .m_Buff = buff.ToReference<BlueprintBuffReference>();
            });

            if (MAContext.Homebrew.Spells.IsDisabled("MageArmor3")) { return; }
            spell.AddToSpellList(SpellTools.SpellList.BloodragerSpellList, 4);
            spell.AddToSpellList(SpellTools.SpellList.WizardSpellList, 7);
            spell.AddToSpellList(SpellTools.SpellList.LichWizardSpelllist, 7);
            //spell.AddToSpellList(SpellTools.SpellList.SummonerSpellList, 5);
            spell.AddToSpellList(SpellTools.SpellList.WitchSpellList, 7);
        }
    }
}