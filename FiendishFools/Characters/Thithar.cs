using BrutalAPI;
using FiendishFools.Effects;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;
using UnityEngine;

namespace FiendishFools.Characters
{
    public class Thithar
    {
        public static void Add()
        {
            Character character = EXOP.CharacterSpriteSetterAddon(EXOP.CharacterInfoSetter("Thithar", Pigments.Red, EXOP._boyle.damageSound, EXOP._boyle.deathSound), "Thithar");

            #region ScriptableObjects

            LoadedDBsHandler.StatusFieldDB.TryGetFieldEffect(TempFieldEffectID.Shield_ID.ToString(), out FieldEffect_SO Shield);
            
            FieldEffect_Apply_Effect ApplyShield = ScriptableObject.CreateInstance<FieldEffect_Apply_Effect>();
            ApplyShield._Field = Shield;

            ChangeToRandomHealthColorEffect changecolor = ScriptableObject.CreateInstance<ChangeToRandomHealthColorEffect>();
            changecolor._healthColors = new ManaColorSO[]
            {
                Pigments.Red,
                Pigments.Blue,
                Pigments.Yellow,
                Pigments.Purple
            };

            #endregion ScriptableObjects

            Ability ability = new Ability("Weak Compression", "WeakCompression_AB");
            ability.AbilitySprite = ResourceLoader.LoadSprite("Compression");
            ability.Description = "Deal 3 damage damage to the Opposing enemy twice." + stainedDamageInfo;
            ability.Cost = new ManaColorSO[]
            {
                Pigments.Red,
                Pigments.Yellow,
                Pigments.Red,
            };
            ability.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<StainedDamageEffect>(), entryVariable = 3, targets = Targeting.Slot_Front },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<StainedDamageEffect>(), entryVariable = 3, targets = Targeting.Slot_Front },
            };
            ability.AnimationTarget = Targeting.Slot_Front;
            ability.Visuals = LoadedAssetsHandler.GetEnemy("OsmanSinnoks_BOSS").abilities[0].ability.visuals;
            ability.AddIntentsToTarget(Targeting.Slot_Front, new string[] { "Damage_3_6", "Damage_3_6" });

            ScaledAbility scaledAbility = new ScaledAbility(ability, 3);
            scaledAbility.SetName = "Compression";
            scaledAbility.AddonName = new string[] { "Constricting", "Crushing", "Mangling" };
            scaledAbility.Description = new string[]
            {
                "Deal 4 damage to the Opposing enemy twice." + stainedDamageInfo,
                "Deal 5 damage to the Opposing enemy twice." + stainedDamageInfo,
                "Deal 6 damage to the Opposing enemy twice." + stainedDamageInfo,
            };
            scaledAbility.EntryValueScale[0] = new int[3] { 4, 5, 6 };
            scaledAbility.EntryValueScale[1] = new int[3] { 4, 5, 6 };
            scaledAbility.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility.intentTypeScale[0][1] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility.Scale();

            Ability ability2 = new Ability("Oxygen Privation", "OxygenPrivation_AB");
            ability2.AbilitySprite = ResourceLoader.LoadSprite("Impoverishment");
            ability2.Description = "Deal 4 damage to the Opposing enemy, apply 6 Shield to this party member position." + stainedDamageInfo;
            ability2.Cost = new ManaColorSO[]
            {
                Pigments.Red,
                Pigments.Yellow,
                Pigments.Yellow,
            };
            ability2.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<StainedDamageEffect>(), entryVariable = 4, targets = Targeting.Slot_Front },
                new EffectInfo() { effect = ApplyShield, entryVariable = 6, targets = Targeting.Slot_SelfSlot },
            };
            ability2.AnimationTarget = Targeting.Slot_Front;
            ability2.Visuals = EXOP._boyle.rankedData[0].rankAbilities[2].ability.visuals;
            ability2.AddIntentsToTarget(Targeting.Slot_Front, new string[] { "Damage_3_6" });
            ability2.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { "Field_Shield" });

            ScaledAbility scaledAbility2 = new ScaledAbility(ability2, 3);
            scaledAbility2.SetName = "Oxygen";
            scaledAbility2.LeftName = false;
            scaledAbility2.AddonName = new string[] { "Destitution", "Deprivation", "Improverishment" };
            scaledAbility2.Description = new string[]
            {
                "Deal 6 damage to the Opposing enemy, apply 8 Shield to this party member position." + stainedDamageInfo,
                "Deal 8 damage to the Opposing enemy, apply 10 Shield to this party member position." + stainedDamageInfo,
                "Deal 10 damage to the Opposing enemy, apply 12 Shield to this party member position." + stainedDamageInfo,
            };
            scaledAbility2.EntryValueScale[0] = new int[3] { 6, 8, 10 };
            scaledAbility2.EntryValueScale[1] = new int[3] { 8, 10, 12 };
            scaledAbility2.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility2.Scale();

            Ability ability3 = new Ability("Rough Decompression", "RoughDecompression_AB");
            ability3.AbilitySprite = ResourceLoader.LoadSprite("Decompression");
            ability3.Description = "Heal this party member 1 health.\nchange this party member's health color to the Opposing enemies health color.";
            ability3.Cost = new ManaColorSO[]
            {
                Pigments.Yellow,
                Pigments.Blue,
            };
            ability3.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<HealEffect>(), entryVariable = 1, targets = Targeting.Slot_SelfSlot },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<ChangeCasterHealthColorToTargetHealthColorEffect>(), entryVariable = 0, targets = Targeting.Slot_Front },
            };
            ability3.AnimationTarget = Targeting.Slot_SelfSlot;
            ability3.Visuals = EXOP._clive.rankedData[0].rankAbilities[2].ability.visuals;
            ability3.AddIntentsToTarget(Targeting.Slot_Front, new string[] { "Mana_Generate" });
            ability3.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { "Heal_1_4", "Other_Refresh" });

            ScaledAbility scaledAbility3 = new ScaledAbility(ability3, 3);
            scaledAbility3.SetName = "Decompression";
            scaledAbility3.AddonName = new string[] { "Slow", "Relaxing", "Soothing" };
            scaledAbility3.Description = new string[]
            {
                "Heal this party member 2 health.\nchange this party member's health color to the Opposing enemy health color.",
                "Heal this party member 3 health.\nchange this party member's health color to the Opposing enemy health color.",
                "Heal this party member 4 health.\nchange this party member's health color to the Opposing enemy health color.",
            };
            scaledAbility3.EntryValueScale[0] = new int[3] { 2, 3, 4 };
            scaledAbility3.intentTypeScale[1][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Health);
            scaledAbility3.Scale();

            character.AddLevelData(12, new Ability[]
            {
                ability,
                ability2,
                ability3
            });
            character.AddLevelData(14, new Ability[]
            {
                scaledAbility.abilities[0],
                scaledAbility2.abilities[0],
                scaledAbility3.abilities[0]
            });
            character.AddLevelData(16, new Ability[]
            {
                scaledAbility.abilities[1],
                scaledAbility2.abilities[1],
                scaledAbility3.abilities[1]
            });
            character.AddLevelData(18, new Ability[]
            {
                scaledAbility.abilities[2],
                scaledAbility2.abilities[2],
                scaledAbility3.abilities[2]
            });
            character.SetMenuCharacterAsFullDPS();
            character.AddCharacter(true);
            character.StartsLocked = false;
        }
        public static readonly string stainedDamageInfo = "\nPigments generated from the damage delt are of this party member's health color.";
    }
}
