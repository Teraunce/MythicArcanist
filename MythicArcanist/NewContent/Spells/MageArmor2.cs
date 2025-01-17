﻿using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Equipment;
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
using MythicArcanist.Utilities;

namespace MythicArcanist.NewContent.Spells
{
    static class MageArmor2
    {
        public static void Add()
        {
            BlueprintAbility SpellCopy = BlueprintTools.GetBlueprint<BlueprintAbility>("9e1ad5d6f87d19e4d8883d63a6e35568"); //MageArmor
            BlueprintBuff SpellCopyBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("a92acdf18049d784eaa8f2004f5d2304"); //MageArmorBuff
            BlueprintBuff SpellCopyBuffMythic = BlueprintTools.GetBlueprint<BlueprintBuff>("355be0688dabc21409f37942d637cdab"); //MageArmorBuffMythic

            int SpellValue = 6;
            string SpellName = "MageArmor2";
            string SpellDisplay = "Mage Armor II";
            string SpellDesc = $"This spell functions like mage armor except you gain a +{SpellValue} armor bonus. This bonus does not stack with other sources that grant an armor bonus." +
                "[LONGSTART]\n\nAn invisible but tangible field of force surrounds the subject of a mage armor {g|Encyclopedia:Spell}spell{/g}, providing a +4 armor " +
                "{g|Encyclopedia:Bonus}bonus{/g} to {g|Encyclopedia:Armor_Class}AC{/g}.\nUnlike mundane armor, mage armor entails no armor {g|Encyclopedia:Check}check{/g} " +
                "{g|Encyclopedia:Penalty}penalty{/g}, {g|Encyclopedia:Spell_Fail_Chance}arcane spell failure chance{/g}. or {g|Encyclopedia:Speed}speed{/g} redution. " +
                "Since mage armor is made of force, {g|Encyclopedia:Incorporeal_Touch_Attack}incorporeal{/g} creatures can't bypass it the way they do normal amror.[LONGEND]";
            var Icon = AssetLoader.LoadInternal(ThisModContext, folder: "Spells", file: $"Icon_{SpellName}.png");
            var ScrollIcon = BlueprintTools.GetBlueprint<BlueprintItemEquipmentUsable>("e8308a74821762e49bc3211358e81016").Icon; //ScollOfMageArmor.Icon

            var Buff = SpellCopyBuff.CreateCopy(ThisModContext, $"{SpellName}Buff", bp =>
            {
                bp.SetNameDescription(ThisModContext, SpellDisplay, SpellDesc);
                bp.m_Icon = Icon;
                bp.GetComponent<AddStatBonus>().Value = SpellValue;
                bp.GetComponent<ACBonusAgainstWeaponType>().ArmorClassBonus = SpellValue;
            });
            var BuffMythic = SpellCopyBuffMythic.CreateCopy(ThisModContext, $"{SpellName}BuffMythic", bp =>
            {
                bp.m_Icon = Icon; 
                bp.GetComponent<ContextRankConfig>().m_StepLevel = SpellValue;
            }
            );

            var Spell = SpellCopy.CreateCopy(ThisModContext, SpellName, bp =>
            {
                bp.SetNameDescription(ThisModContext, SpellDisplay, SpellDesc);
                bp.m_Icon = Icon;
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
                    .m_Buff = BuffMythic.ToReference<BlueprintBuffReference>();
                bp.GetComponent<AbilityEffectRunAction>()
                    .Actions.Actions
                    .OfType<Conditional>().FirstOrDefault()
                    .IfFalse.Actions
                    .OfType<ContextActionApplyBuff>().FirstOrDefault()
                    .m_Buff = Buff.ToReference<BlueprintBuffReference>();
            });


            if (ThisModContext.ThirdParty.Spells.IsDisabled("MageArmor2")) {  return; }
            Spell.AddToSpellList(SpellTools.SpellList.BloodragerSpellList, 3);
            Spell.AddToSpellList(SpellTools.SpellList.WizardSpellList, 4);
            Spell.AddToSpellList(SpellTools.SpellList.LichWizardSpelllist, 4);
            //spell.AddToSpellList(SpellTools.SpellList.SummonerSpellList, 3);
            Spell.AddToSpellList(SpellTools.SpellList.WitchSpellList, 4);
            var Scroll = Utilities.ItemTools.CreateScroll(ThisModContext, Spell, ScrollIcon);
            VenderTools.AddScrollToLeveledVenders(Scroll);
            //Utilities.ItemTools.AddToVendor(ThisModContext, Scroll, 1, BlueprintSharedVendorTables.WarCamp_ScrollVendorClericTable);
            //Utilities.ItemTools.AddToVendor(ThisModContext, Scroll, 2, BlueprintSharedVendorTables.WarCamp_REVendorTableMagic);
            //Utilities.ItemTools.AddToVendor(ThisModContext, Scroll, 3, BlueprintSharedVendorTables.Scroll_Chapter3VendorTable);
            //Utilities.ItemTools.AddToVendor(ThisModContext, Scroll, 3, BlueprintSharedVendorTables.Scroll_Chapter5VendorTable);
        }
    }
}
