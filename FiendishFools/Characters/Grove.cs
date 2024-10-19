using BrutalAPI;
using FiendishFools.Effects;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace FiendishFools.Characters
{
    public class Grove
    {
        public static void Add()
        {
            Character character = EXOP.CharacterSpriteSetterAddon(EXOP.CharacterInfoSetter("Grove", Pigments.Red, EXOP._arnold.damageSound, EXOP._arnold.deathSound), "Grove");

            #region ScriptableObjects

            HealEffect healEffect = ScriptableObject.CreateInstance<HealEffect>();
            healEffect.usePreviousExitValue = true;
            GetMissingHealthEffect getMissingHealthEffect = ScriptableObject.CreateInstance<GetMissingHealthEffect>();
            getMissingHealthEffect._DecreaseByPercentage = true;

            #endregion ScriptableObjects

            Ability ability = new Ability("Show the Pain", "Show the Pain_AB");
            ability.AbilitySprite = ResourceLoader.LoadSprite("SkillShowThePain");
            ability.Description = "Deal 6 damage to the Opposing enemy, plus the amount of missing health from the Left ally.";
            ability.Cost = EXOP.MultiPigments("Red", 3);
            ability.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<GetMissingHealthEffect>(), entryVariable = 0, targets = Targeting.Slot_AllyLeft},
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageIncreaseEffect>(), entryVariable = 6, targets = Targeting.Slot_Front},
            };
            ability.AnimationTarget = Targeting.Slot_Front;
            ability.Visuals = EXOP._kleiver.rankedData[0].rankAbilities[1].ability.visuals;
            ability.AddIntentsToTarget(Targeting.Slot_Front, new string[] { "Damage_3_6" });
            ability.AddIntentsToTarget(Targeting.Slot_AllyLeft, new string[] { "Other_MaxHealth" });

            ScaledAbility scaledAbility = new ScaledAbility(ability, 3);
            scaledAbility.SetName = "Pain";
            scaledAbility.AddonName = new string[3] { "Make the", "Feel the", "Bring the" };
            scaledAbility.Description = new string[3]
            {
                "Deal 8 damage to the Opposing enemy, plus the amount of missing health from the Left ally.",
                "Deal 10 damage to the Opposing enemy, plus the amount of missing health from the Left ally.",
                "Deal 12 damage to the Opposing enemy, plus the amount of missing health from the Left ally."
            };
            scaledAbility.EntryValueScale[0] = new int[3] { 8, 10, 12 };
            scaledAbility.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility.Scale();

            Ability ability2 = new Ability("Spiteful Masochism", "Spiteful Masochism_AB");
            ability2.AbilitySprite = ResourceLoader.LoadSprite("SkillMasochism");
            ability2.Description = "Deal 5 damage to the left and right enemies, plus the amount of missing health from this party member.";
            ability2.Cost = new ManaColorSO[]
            {
                Pigments.Red,
                Pigments.Blue
            };
            ability2.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<GetMissingHealthEffect>(), entryVariable = 0, targets = Targeting.Slot_SelfSlot},
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageIncreaseEffect>(), entryVariable = 5, targets = Targeting.Slot_OpponentSides},
            };
            ability2.AnimationTarget = Targeting.Slot_OpponentSides;
            ability2.Visuals = EXOP._kleiver.rankedData[0].rankAbilities[2].ability.visuals;
            ability2.AddIntentsToTarget(Targeting.Slot_OpponentSides, new string[] { "Damage_3_6" });
            ability2.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { "Other_MaxHealth" });

            ScaledAbility scaledAbility2 = new ScaledAbility(ability2, 3);
            scaledAbility2.SetName = "Masochism";
            scaledAbility2.AddonName = new string[3] { "Raging", "Intolerable", "Uncontrollable" };
            scaledAbility2.Description = new string[3]
            {
                "Deal 7 damage to the left and right enemies, plus the amount of missing health from this party member.",
                "Deal 8 damage to the left and right enemies, plus the amount of missing health from this party member.",
                "Deal 10 damage to the left and right enemies, plus the amount of missing health from this party member."
            };
            scaledAbility2.EntryValueScale[0] = new int[3] { 7, 8, 10 };
            scaledAbility2.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility2.Scale();

            Ability ability3 = new Ability("Blood Bind", "Blood Bind_AB");
            ability3.AbilitySprite = ResourceLoader.LoadSprite("SkillBloodBind");
            ability3.Description = "Heal an amount of health equal to this party members missing health to the Right ally.\nHeal an amount of health equal to half this party members missing health to this party member.";
            ability3.Cost = EXOP.MultiPigments("Blue", 2);
            ability3.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<GetMissingHealthEffect>(), entryVariable = 0, targets = Targeting.Slot_SelfSlot},
                new EffectInfo() { effect = healEffect, entryVariable = 1, targets = Targeting.Slot_AllyRight},
                new EffectInfo() { effect = getMissingHealthEffect, entryVariable = 0, targets = Targeting.Slot_SelfSlot},
                new EffectInfo() { effect = healEffect, entryVariable = 1, targets = Targeting.Slot_SelfSlot},
            };
            ability3.AnimationTarget = Targeting.Slot_AllyRight;
            ability3.Visuals = EXOP._rags.rankedData[0].rankAbilities[1].ability.visuals;
            ability3.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { "Heal_1_4", "Other_MaxHealth" });
            ability3.AddIntentsToTarget(Targeting.Slot_AllyRight, new string[] { "Heal_1_4" });

            ScaledAbility scaledAbility3 = new ScaledAbility(ability3, 3);
            scaledAbility3.SetName = "Blood";
            scaledAbility3.LeftName = false;
            scaledAbility3.AddonName = new string[3] { "Link", "Connect", "Forge" };
            scaledAbility3.Description = new string[3]
            {
                "Heal an amount of health equal to this party members missing health to the Right ally.\nHeal an amount of health equal to half this party members missing health to this party member.",
                "Heal an amount of health equal to this party members missing health to the Right ally.\nHeal an amount of health equal to half this party members missing health to this party member.",
                "Heal an amount of health equal to this party members missing health to the Right ally.\nHeal an amount of health equal to half this party members missing health to this party member."
            };
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

            character.SetMenuCharacterAsFullDPS();
            character.AddCharacter(true);
            character.StartsLocked = false;
        }
    }
}
