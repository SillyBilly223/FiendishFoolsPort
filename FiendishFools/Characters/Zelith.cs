using BrutalAPI;
using FiendishFools.Effects;
using FiendishFools.Passives;
using FiendishFools.Targetting;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FiendishFools.Characters
{
    public class Zelith
    {
        public static void Add()
        {
            #region Passives

            ZelithPassive zelithPassive = ScriptableObject.CreateInstance<ZelithPassive>();
            zelithPassive._passiveName = "Stalker";
            zelithPassive.passiveIcon = ResourceLoader.LoadSprite("StalkerIcon");
            zelithPassive._enemyDescription = "skibidi";
            zelithPassive._characterDescription = "a random party member will become this party member's Obsession.";
            zelithPassive.m_PassiveID = "Stalker_ID";
            zelithPassive.IsObsession = false;

            BlankPassive zelithPassive2 = ScriptableObject.CreateInstance<BlankPassive>();
            zelithPassive2._passiveName = "Obsession";
            zelithPassive2.passiveIcon = ResourceLoader.LoadSprite("StalkedIcon");
            zelithPassive2._enemyDescription = "skibidi";
            zelithPassive2._characterDescription = "This party member feels like they are being watched";
            zelithPassive2.m_PassiveID = "Obsession_ID";

            LoadedDBsHandler.PassiveDB.AddNewPassive(zelithPassive.m_PassiveID, zelithPassive);
            LoadedDBsHandler.PassiveDB.AddNewPassive(zelithPassive2.m_PassiveID, zelithPassive2);

            #endregion Passives

            Character character = EXOP.CharacterSpriteSetterAddon(EXOP.CharacterInfoSetter("Zalith", Pigments.Purple, EXOP._keko.damageSound, EXOP._keko.deathSound), "Stalker");
            character.AddPassive(zelithPassive);

            #region ScriptableObjects

            Targetting_ObsessionReflectIndex TargetOpposingObsession = ScriptableObject.CreateInstance<Targetting_ObsessionReflectIndex>();
            TargetOpposingObsession.getAllies = false;
            TargetOpposingObsession.allSelfSlots = false;
            TargetOpposingObsession.slotPointerDirections = new int[1] { 0 };

            Targetting_ObsessionReflectIndex TargetLeftRightObsession = ScriptableObject.CreateInstance<Targetting_ObsessionReflectIndex>();
            TargetLeftRightObsession.getAllies = false;
            TargetLeftRightObsession.allSelfSlots = false;
            TargetLeftRightObsession.slotPointerDirections = new int[2] { -1, 1 };

            Targetting_ObsessionReflectIndex TargetSelfObsession = ScriptableObject.CreateInstance<Targetting_ObsessionReflectIndex>();
            TargetSelfObsession.getAllies = true;
            TargetSelfObsession.allSelfSlots = true;
            TargetSelfObsession.slotPointerDirections = new int[1] { 0 };

            DamageFromObsessionEffect IfkillDamageEffect = ScriptableObject.CreateInstance<DamageFromObsessionEffect>();
            IfkillDamageEffect._returnKillAsSuccess = true;

            DamageEffect DamageUsePreviousEffect = ScriptableObject.CreateInstance<DamageEffect>();
            DamageUsePreviousEffect._usePreviousExitValue = true;

            #endregion ScriptableObjetcs

            Ability ability = new Ability("Erratic Influince", "ErraticInfluince_AB");
            ability.AbilitySprite = ResourceLoader.LoadSprite("SkillInfluince");
            ability.Description = "The stalked party member deals 8 damage to their Opposing enemy.";
            ability.Cost = new ManaColorSO[]
            {
                Pigments.Red,
                Pigments.Blue,
                Pigments.Blue,
            };
            ability.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageFromObsessionEffect>(), entryVariable = 8, targets = TargetOpposingObsession},
            };
            ability.AnimationTarget = TargetOpposingObsession;
            ability.Visuals = EXOP._splig.rankedData[0].rankAbilities[0].ability.visuals;
            ability.AddIntentsToTarget(TargetOpposingObsession, new string[] { IntentType_GameIDs.Damage_7_10.ToString() });

            ScaledAbility scaledAbility = new ScaledAbility(ability, 3);
            scaledAbility.SetName = "Influince";
            scaledAbility.AddonName = new string[] { "Agressive", "Violent", "Murderous " };
            scaledAbility.Description = new string[]
            {
                "The stalked party member deals 10 damage to their Opposing enemy.",
                "The stalked party member deals 12 damage to their Opposing enemy.",
                "The stalked party member deals 14 damage to their Opposing enemy.",
            };
            scaledAbility.EntryValueScale[0] = new int[3] { 10, 12, 14 };
            scaledAbility.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility.Scale();

            Ability ability2 = new Ability("Malicious Whispers ", "MaliciousWhispers_AB");
            ability2.AbilitySprite = ResourceLoader.LoadSprite("SkillWhispers");
            ability2.Description = "The stalked party member deals 4 damage to their Left and Right enemies. If this Kills, refreash the stalked party members.";
            ability2.Cost = new ManaColorSO[]
            {
                Pigments.Red,
                Pigments.Blue,
                Pigments.Yellow,
            };
            ability2.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = IfkillDamageEffect, entryVariable = 4, targets = TargetLeftRightObsession},
                new EffectInfo() { effect = ScriptableObject.CreateInstance<RefreshAbilityUseEffect>(), entryVariable = 0, targets = TargetSelfObsession, condition = ScriptableObject.CreateInstance<PreviousEffectCondition>()},
            };
            ability2.AnimationTarget = TargetLeftRightObsession;
            ability2.Visuals = EXOP._boyle.rankedData[0].rankAbilities[1].ability.visuals;
            ability2.AddIntentsToTarget(TargetLeftRightObsession, new string[] { IntentType_GameIDs.Damage_7_10.ToString() });
            ability2.AddIntentsToTarget(TargetSelfObsession, new string[] { IntentType_GameIDs.Other_Refresh.ToString() });

            ScaledAbility scaledAbility2 = new ScaledAbility(ability2, 3);
            scaledAbility2.SetName = "Whispers";
            scaledAbility2.AddonName = new string[] { "Malevolent", "Malignant", "Maleficent " };
            scaledAbility2.Description = new string[]
            {
                "The stalked party member deals 7 damage to their Left and Right enemies. If this Kills, refreash the stalked party members abilities.",
                "The stalked party member deals 9 damage to their Left and Right enemies. If this Kills, refreash the stalked party members abilities.",
                "The stalked party member deals 11 damage to their Left and Right enemies. If this Kills, refreash the stalked party members abilities.",
            };
            scaledAbility2.EntryValueScale[0] = new int[3] { 7, 9, 11 };
            scaledAbility2.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility2.Scale();

            Ability ability3 = new Ability("Pitiful Imitation", "PitifulImitation_AB");
            ability3.AbilitySprite = ResourceLoader.LoadSprite("SkillImitation");
            ability3.Description = "The stalked party member deals 6 damage to their Opposing enemy. Deal damage equal to the amount of damage done to this party member's Opposing enemy.";
            ability3.Cost = new ManaColorSO[]
            {
                Pigments.Blue,
                Pigments.Blue
            };
            ability3.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageFromObsessionEffect>(), entryVariable = 6, targets = TargetOpposingObsession},
                new EffectInfo() { effect = DamageUsePreviousEffect, entryVariable = 1, targets = Targeting.Slot_Front },
            };
            ability3.AnimationTarget = TargetOpposingObsession;
            ability3.Visuals = EXOP._boyle.rankedData[0].rankAbilities[0].ability.visuals;
            ability3.AddIntentsToTarget(TargetOpposingObsession, new string[] { IntentType_GameIDs.Damage_3_6.ToString() });
            ability3.AddIntentsToTarget(Targeting.Slot_Front, new string[] { IntentType_GameIDs.Damage_3_6.ToString() });

            ScaledAbility scaledAbility3 = new ScaledAbility(ability3, 3);
            scaledAbility3.SetName = "Imitation";
            scaledAbility3.AddonName = new string[] { "Sore", "Aching", "Atinging" };
            scaledAbility3.Description = new string[]
            {
                "The stalked party member deals 8 damage to their Opposing enemy. Deal damage equal to the amount of damage done to this party member's Opposing enemy.",
                "The stalked party member deals 10 damage to their Opposing enemy. Deal damage equal to the amount of damage done to this party member's Opposing enemy.",
                "The stalked party member deals 12 damage to their Opposing enemy. Deal damage equal to the amount of damage done to this party member's Opposing enemy.",
            };
            scaledAbility3.EntryValueScale[0] = new int[3] { 8, 10, 12 };
            scaledAbility3.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility3.Scale();

            character.AddLevelData(6, new Ability[]
            {
                ability,
                ability2,
                ability3
            });
            character.AddLevelData(8, new Ability[]
            {
                scaledAbility.abilities[0],
                scaledAbility2.abilities[0],
                scaledAbility3.abilities[0]
            });

            character.AddLevelData(10, new Ability[]
            {
                scaledAbility.abilities[1],
                scaledAbility2.abilities[1],
                scaledAbility3.abilities[1]
            });

            character.AddLevelData(12, new Ability[]
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
