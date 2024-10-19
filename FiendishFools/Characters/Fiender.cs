using BrutalAPI;
using FiendishFools.Effects;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

namespace FiendishFools.Characters
{
    public class Fiender
    {
        public static void Add()
        {
            Character character = EXOP.CharacterSpriteSetterAddon(EXOP.CharacterInfoSetter("Fiender", Pigments.Red, EXOP._splig.damageSound, EXOP._splig.deathSound), "Fiender");

            #region ScriptableObjects

            LoadedDBsHandler.StatusFieldDB.TryGetStatusEffect("Empowered_ID", out StatusEffect_SO Empowered);
            LoadedDBsHandler.StatusFieldDB.TryGetStatusEffect("Scars_ID", out StatusEffect_SO Scars);

            StatusEffect_ApplyPreviousExitValueAmount_Effect ApplyEmpoweredEffect = ScriptableObject.CreateInstance<StatusEffect_ApplyPreviousExitValueAmount_Effect>();
            ApplyEmpoweredEffect.PreviousExitValue = 0;
            ApplyEmpoweredEffect._Status = Empowered;

            StatusEffect_Apply_Effect ApplyScarsEffect = ScriptableObject.CreateInstance<StatusEffect_Apply_Effect>();
            ApplyScarsEffect._Status = Scars;

            RemoveStatusEffectEffect RemoveEmpowerdEffect = ScriptableObject.CreateInstance<RemoveStatusEffectEffect>();
            RemoveEmpowerdEffect._status = Empowered;
            RemoveStatusEffectEffect_TransferPreviousExitValue RemoveScarsEffect = ScriptableObject.CreateInstance<RemoveStatusEffectEffect_TransferPreviousExitValue>();
            RemoveScarsEffect._status = Scars;

            HealEffect HealUsingPreviousExitValueEffect = ScriptableObject.CreateInstance<HealEffect>();
            HealUsingPreviousExitValueEffect.usePreviousExitValue = true;

            #endregion ScriptableObjects

            Ability ability = new Ability("Bloodied Sacarfice", "BloodiedSacarfice_AB");
            ability.AbilitySprite = ResourceLoader.LoadSprite("SkillBloodiedSacarfice");
            ability.Description = "Deal 4 damage to the left ally, apply Empowered to the left ally equal to the amount of damage done.\nHeal the left ally and this party member 1 health.";
            ability.Cost = new ManaColorSO[] { Pigments.Red, Pigments.Blue };
            ability.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageEffect>(), entryVariable = 4, targets = Targeting.Slot_AllyLeft },
                new EffectInfo() { effect = ApplyEmpoweredEffect, entryVariable = 0, targets = Targeting.Slot_AllyLeft },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<HealEffect>(), entryVariable = 1, targets = Targeting.GenerateSlotTarget(new int[] { 0, 1 }, true) },
            };
            ability.AnimationTarget = Targeting.Slot_AllySides;
            ability.Visuals = EXOP._splig.rankedData[0].rankAbilities[1].ability.visuals;
            ability.AddIntentsToTarget(Targeting.Slot_AllyLeft, new string[] { "Damage_3_6", "ApplyEmpowered" });
            ability.AddIntentsToTarget(Targeting.GenerateSlotTarget(new int[] { 0, 1 }, true), new string[] { "Heal_1_4" });

            ScaledAbility scaledAbility = new ScaledAbility(ability, 3);
            scaledAbility.SetName = "Sacarfice";
            scaledAbility.AddonName = new string[3] { "Sliced", "Carved", "Chopped" };
            scaledAbility.Description = new string[3]
            {
                "Deal 4 damage to the left ally, apply Empowered to the left ally equal to the amount of damage done.\nHeal the left ally and this party member 2 health.",
                "Deal 5 damage to the left ally, apply Empowered to the left ally equal to the amount of damage done.\nHeal the left ally and this party member 2 health.",
                "Deal 5 damage to the left ally, apply Empowered to the left ally equal to the amount of damage done.\nHeal the left ally and this party member 3 health."
            };
            scaledAbility.EntryValueScale[0] = new int[3] { 4, 5, 5 };
            scaledAbility.EntryValueScale[2] = new int[3] { 2, 2, 3 };
            scaledAbility.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility.intentTypeScale[1][0] = new ScaledAbility.IntentTypeScalePointer(2, IntentTypeScale.Health);
            scaledAbility.Scale();

            Ability ability2 = new Ability("Bloodied strength", "Bloodiedstrength_AB");
            ability2.AbilitySprite = ResourceLoader.LoadSprite("SkillBloodiedStrength");
            ability2.Description = "Deal 3 damage to this party member, apply Empowered to the left and right allies equal to the amount of damage done. Inflict 1 scars to this party member.";
            ability2.Cost = new ManaColorSO[] { Pigments.Red, Pigments.Red };
            ability2.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageEffect>(), entryVariable = 3, targets = Targeting.Slot_SelfSlot },
                new EffectInfo() { effect = ApplyEmpoweredEffect, entryVariable = 0, targets = Targeting.GenerateSlotTarget(new int[] { 1, -1 }, true) },
                new EffectInfo() { effect = ApplyScarsEffect, entryVariable = 1, targets = Targeting.Slot_SelfSlot },
            };
            ability2.AnimationTarget = Targeting.Slot_SelfSlot;
            ability2.Visuals = EXOP._splig.rankedData[0].rankAbilities[1].ability.visuals;
            ability2.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { "Damage_3_6", "Status_Scars" });
            ability2.AddIntentsToTarget(Targeting.GenerateSlotTarget(new int[] { 1, -1 }, true), new string[] { "ApplyEmpowered" });

            ScaledAbility scaledAbility2 = new ScaledAbility(ability2, 3);
            scaledAbility2.SetName = "Bloodied";
            scaledAbility2.LeftName = false;
            scaledAbility2.AddonName = new string[3] { "Pride", "Bliss", "Rapture" };
            scaledAbility2.Description = new string[3]
            {
                "Deal 4 damage to this party member, apply Empowered to the left and right allies equal to the amount of damage done. Inflict 2 scars to this party member.",
                "Deal 5 damage to this party member, apply Empowered to the left and right allies equal to the amount of damage done. Inflict 2 scars to this party member.",
                "Deal 5 damage to this party member, apply Empowered to the left and right allies equal to the amount of damage done. Inflict 3 scars to this party member.",
            };
            scaledAbility2.EntryValueScale[0] = new int[3] { 4, 5, 5 };
            scaledAbility2.EntryValueScale[2] = new int[3] { 2, 2, 3 };
            scaledAbility2.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility2.Scale();

            Ability ability3 = new Ability("Depraved  Conversion", "DepravedConversion_AB");
            ability3.AbilitySprite = ResourceLoader.LoadSprite("SkillDepravedConversion");
            ability3.Description = "Remove all Empowered from the Right ally, remove all Scars from this party member.\nHeal 2 health plus the amount of Empowered and Scars removed to this party member and the right ally.";
            ability3.Cost = new ManaColorSO[] { Pigments.Red, Pigments.Red, Pigments.PurpleBlue };
            ability3.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = RemoveEmpowerdEffect, entryVariable = 0, targets = Targeting.Slot_AllyRight },
                new EffectInfo() { effect = RemoveScarsEffect, entryVariable = 0, targets = Targeting.Slot_SelfSlot },
                new EffectInfo() { effect = HealUsingPreviousExitValueEffect, entryVariable = 3, targets = Targeting.GenerateSlotTarget(new int[] { 1, 0 }, true) },
            };
            ability3.AnimationTarget = Targeting.Slot_SelfSlot;
            ability3.Visuals = EXOP._splig.rankedData[0].rankAbilities[1].ability.visuals;
            ability3.AddIntentsToTarget(Targeting.GenerateSlotTarget(new int[] { 1, 0 }, true), new string[] { "Heal_1_4" });
            ability3.AddIntentsToTarget(Targeting.Slot_AllyRight, new string[] { "RemoveEmpowered" });
            ability3.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { "Rem_Status_Scars" });

            ScaledAbility scaledAbility3 = new ScaledAbility(ability3, 3);
            scaledAbility3.SetName = "Conversion";
            scaledAbility3.AddonName = new string[3] { "Immoral", "Unethical", "Diabolical" };
            scaledAbility3.Description = new string[3]
            {
                "Remove all spiritual energy from the Right ally.\nHeal 4 health plus the amount of spiritual energy removed to this party member and the left ally.",
                "Remove all spiritual energy from the Right ally.\nHeal 5 health plus the amount of spiritual energy removed to this party member and the left ally.",
                "Remove all spiritual energy from the Right ally.\nHeal 6 health plus the amount of spiritual energy removed to this party member and the left ally."
            };
            scaledAbility3.EntryValueScale[2] = new int[3] { 4, 5, 6 };
            scaledAbility3.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(2, IntentTypeScale.Health);
            scaledAbility3.Scale();

            character.AddLevelData(8, new Ability[]
            {
                ability,
                ability2,
                ability3
            });
            character.AddLevelData(10, new Ability[]
            {
                scaledAbility.abilities[0],
                scaledAbility2.abilities[0],
                scaledAbility3.abilities[0]
            });
            character.AddLevelData(12, new Ability[]
            {
                scaledAbility.abilities[1],
                scaledAbility2.abilities[1],
                scaledAbility3.abilities[1]
            });
            character.AddLevelData(14, new Ability[]
            {
                scaledAbility.abilities[2],
                scaledAbility2.abilities[2],
                scaledAbility3.abilities[2]
            });
            character.SetMenuCharacterAsFullSupport();
            character.AddCharacter(true);
            character.StartsLocked = false;
        }

    }
}