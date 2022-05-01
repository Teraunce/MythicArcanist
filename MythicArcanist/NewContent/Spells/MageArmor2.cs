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
    static class MageArmor2
    {
        public static void AddMageArmor2()
        {
            BlueprintAbility MageArmor = BlueprintTools.GetBlueprint<BlueprintAbility>("9e1ad5d6f87d19e4d8883d63a6e35568");
            BlueprintBuff MageArmorBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("a92acdf18049d784eaa8f2004f5d2304");
            BlueprintBuff MageArmorBuffMythic = BlueprintTools.GetBlueprint<BlueprintBuff>("355be0688dabc21409f37942d637cdab");

            int spellValue = 6;
            string spellName = "MageArmor2";
            string spellDisplay = "Mage Armor II";
            string spellDesc = $"This spell functions like mage armor except you gain a +{spellValue} armor bonus. This bonus does not stack with other sources that grant an armor bonus.";
            var icon = AssetLoader.LoadInternal(MAContext, folder: "Spells", file: $"Icon_{spellName}.png");

            /*
            var buff = library.CopyAndAdd<BlueprintBuff>("a92acdf18049d784eaa8f2004f5d2304", $"{spellName}Buff", ""); //MageArmorBuff
            buff.SetNameDescriptionIcon(spellDisplay, spellDesc, icon);
            buff.ReplaceComponent<AddStatBonus>(a => a.Value = spellValue);
            */

            
            var buff = MageArmorBuff.CreateCopy(MAContext, $"{spellName}Buff", bp =>
            {
                bp.SetNameDescription(MAContext, spellDisplay, spellDesc);
                bp.m_Icon = icon;
                bp.GetComponent<AddStatBonus>().Value = spellValue;
                bp.GetComponent<ACBonusAgainstWeaponType>().ArmorClassBonus = spellValue;
                //bp.ReplaceComponents<AddStatBonus>(Helpers.Create<AddStatBonus>(a => a.Value = spellValue));
                //bp.ReplaceComponents<ACBonusAgainstWeaponType>(Helpers.Create<ACBonusAgainstWeaponType>(a => a.ArmorClassBonus = spellValue));
            });
            var buffMythic = MageArmorBuffMythic.CreateCopy(MAContext, $"{spellName}BuffMythic", bp =>
            {
                bp.GetComponent<ContextRankConfig>().m_StepLevel = spellValue;
            }
            );

            /*
            var applyBuff = Helpers.Create<ContextActionApplyBuff>(bp => {
                bp.IsFromSpell = true;
                bp.m_Buff = buff.ToReference<BlueprintBuffReference>();
                bp.DurationValue = new ContextDurationValue()
                {
                    Rate = DurationRate.Hours,
                    BonusValue = new ContextValue()
                    {
                        ValueType = ContextValueType.Rank
                    },
                    DiceCountValue = new ContextValue(),
                    DiceType = DiceType.Zero
                };
            });
            */

            /*
            var spell = library.CopyAndAdd<BlueprintAbility>("9e1ad5d6f87d19e4d8883d63a6e35568", spellName, ""); //Mage Armor
            spell.SetNameDescriptionIcon(spellDisplay, spellDesc, icon);
            spell.RemoveComponents<SpellListComponent>();
            spell.RemoveComponents<RecommendationNoFeatFromGroup>();
            spell.AddComponent(Helpers.CreateSpellDescriptor(SpellDescriptor.Force));
            spell.ReplaceComponent<AbilityEffectRunAction>(a => a.Actions = Helpers.CreateActionList(Common.createContextActionApplyBuff(buff,
                                                                                                                                         Helpers.CreateContextDuration(rate: DurationRate.Hours),
                                                                                                                                         is_from_spell: true
                                                                                                                                         )));
            */

            
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

            /*
            if (bloodrager_spelllist != null) { spell.AddToSpellList(bloodrager_spelllist, 3); }
            spell.AddToSpellList(Helpers.wizardSpellList, 4);
            if (summoner_spelllist != null) { spell.AddToSpellList(summoner_spelllist, 3); }
            if (witch_spelllist != null) { spell.AddToSpellList(witch_spelllist, 4); }
            spell.AddSpellAndScroll("e8308a74821762e49bc3211358e81016");
            mageArmor2 = spell;
            */

            if (MAContext.Homebrew.Spells.IsDisabled("MageArmor2")) {  return; }
            spell.AddToSpellList(SpellTools.SpellList.BloodragerSpellList, 3);
            spell.AddToSpellList(SpellTools.SpellList.WizardSpellList, 4);
            spell.AddToSpellList(SpellTools.SpellList.LichWizardSpelllist, 4);
            //spell.AddToSpellList(SpellTools.SpellList.SummonerSpellList, 3);
            spell.AddToSpellList(SpellTools.SpellList.WitchSpellList, 4);
            
            /*
            var buff = Helpers.CreateBuff(MAContext, $"{spellName}Buff", bp =>
            {
                bp.SetName(MAContext, spellDisplay);
                bp.SetDescription(MAContext, spellDesc);
                bp.m_Icon = icon;
                bp.IsClassFeature = false;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.Stacking = StackingType.Replace;
                bp.Ranks = 0;
                bp.AddComponent(Helpers.Create<AddStatBonus>(c =>
                {
                    c.Stat = StatType.AC;
                    c.Descriptor = ModifierDescriptor.Armor;
                    c.Value = 6;
                    c.ScaleByBasicAttackBonus = false;
                }));
                bp.AddComponent<ACBonusAgainstWeaponType>(c =>
                {
                    c.m_Type = BlueprintTools.GetBlueprint<BlueprintWeaponType>("e9e7d67d6ffb8ad4ba601fccffba4d6e").ToReference<BlueprintWeaponTypeReference>(); //IncorporealTouchType
                });
                bp.AddComponent(Helpers.Create<AddFactContextActions>(c =>
                {
                    bp.AddConditionalBuff
                }));
            });
            var applyBuff = Helpers.Create<Kingmaker.UnitLogic.Mechanics.Actions.ContextActionApplyBuff>(bp =>
            {
                bp.IsFromSpell = true;
                bp.m_Buff = buff.ToReference<BlueprintBuffReference>();
                bp.DurationValue = new ContextDurationValue()
                {
                    Rate = DurationRate.Rounds,
                    BonusValue = new ContextValue()
                    {
                        ValueType = ContextValueType.Rank
                    },
                    DiceCountValue = new ContextValue(),
                    DiceType = DiceType.One
                };
            });
            */
        }
    }
}
