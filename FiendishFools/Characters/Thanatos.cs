using BepInEx;
using BrutalAPI;
using FiendishFools.Conditions;
using FiendishFools.Effects;
using FiendishFools.Passives;
using FiendishFools.Targetting;
using UnityEngine;

namespace FiendishFools.Characters
{
    public class Thanatos
    {
        public static void Add()
        {
            #region Passive

            ThanatosPassive thanatosPassive = ScriptableObject.CreateInstance<ThanatosPassive>();
            thanatosPassive._passiveName = "Deaths List";
            thanatosPassive.passiveIcon = ResourceLoader.LoadSprite("PassivesIconDeathsList");
            thanatosPassive.m_PassiveID = "Deaths List";
            thanatosPassive._enemyDescription = "Smelelr.";
            thanatosPassive._characterDescription = "If there no are no enemies inflicted with deaths touch, inflict deaths touch to a random enemy.";

            GlossaryPassives glossaryPassives = new GlossaryPassives(
                "Deaths List",
                "If there no are no enemies inflicted with deaths touch, inflict deaths touch to a random enemy.",
                ResourceLoader.LoadSprite("PassivesIconDeathsList")
            );
            glossaryPassives.glossaryID = (GlossaryLocID)120;

            LoadedDBsHandler.PassiveDB.AddNewPassive("DeathsList_ID", thanatosPassive);
            LoadedDBsHandler.GlossaryDB.AddNewPassive(glossaryPassives);

            #endregion Passive

            Character character = EXOP.CharacterSpriteSetterAddon(EXOP.CharacterInfoSetter("Thanatos", Pigments.Red, EXOP._griffin.damageSound, EXOP._griffin.deathSound), "Thanatos");
            character.AddPassive(thanatosPassive);

            #region ScriptableObjects

            LoadedDBsHandler._StatusFieldDB.TryGetStatusEffect("Scars_ID", out StatusEffect_SO Scars);
            LoadedDBsHandler.StatusFieldDB.TryGetStatusEffect("DeathTouch_ID", out StatusEffect_SO DeathsTouch);
            LoadedDBsHandler.StatusFieldDB.TryGetFieldEffect("Shield_ID", out FieldEffect_SO Shield);

            PreviousEffectCondition previousEffectCondition = ScriptableObject.CreateInstance<PreviousEffectCondition>();
            previousEffectCondition.wasSuccessful = true;

            RemoveStatusEffectEffect removeStatusEffectEffect = ScriptableObject.CreateInstance<RemoveStatusEffectEffect>();
            removeStatusEffectEffect._status = DeathsTouch;

            DealDoubleDamageIfHasStatusEffectEffect DealDoubleDamageIfHasStatus = ScriptableObject.CreateInstance<DealDoubleDamageIfHasStatusEffectEffect>();
            DealDoubleDamageIfHasStatus._Status = DeathsTouch;

            StatusEffect_ApplyRandomAmount_Effect ApplyScarsEffect_1t4 = ScriptableObject.CreateInstance<StatusEffect_ApplyRandomAmount_Effect>();
            ApplyScarsEffect_1t4._Status = Scars;
            ApplyScarsEffect_1t4.ApplyRange = new Vector2(1, 4);

            StatusEffect_ApplyRandomAmount_Effect ApplyScarsEffect_2t4 = ScriptableObject.CreateInstance<StatusEffect_ApplyRandomAmount_Effect>();
            ApplyScarsEffect_2t4._Status = Scars;
            ApplyScarsEffect_2t4.ApplyRange = new Vector2(2, 4);

            FieldEffect_Apply_Effect ApplyShieldEffect = ScriptableObject.CreateInstance<FieldEffect_Apply_Effect>();
            ApplyShieldEffect._Field = Shield;

            HasDeathTouchCondition RightHasDeathTouch = ScriptableObject.CreateInstance<HasDeathTouchCondition>();
            RightHasDeathTouch.targeting = Targeting.GenerateSlotTarget(new int[] { 1 });

            HasDeathTouchCondition LeftHasDeathTouch = ScriptableObject.CreateInstance<HasDeathTouchCondition>();
            LeftHasDeathTouch.targeting = Targeting.GenerateSlotTarget(new int[] { -1 });

            HasDeathTouchCondition OpposingHasDeathTouch = ScriptableObject.CreateInstance<HasDeathTouchCondition>();
            OpposingHasDeathTouch.targeting = Targeting.GenerateSlotTarget(new int[] { 0 });

            DamageEffect IndirectDamage = ScriptableObject.CreateInstance<DamageEffect>();
            IndirectDamage._indirect = true;

            Targetting_BySlot_Index_Conditional ThanatosTargetingLeft = ScriptableObject.CreateInstance<Targetting_BySlot_Index_Conditional>();
            ThanatosTargetingLeft.EffectCondition = LeftHasDeathTouch;
            ThanatosTargetingLeft.slotPointerDirections = new int[] { -2, -3, -4 };

            Targetting_BySlot_Index_Conditional ThanatosTargetingRight = ScriptableObject.CreateInstance<Targetting_BySlot_Index_Conditional>();
            ThanatosTargetingRight.EffectCondition = RightHasDeathTouch;
            ThanatosTargetingRight.slotPointerDirections = new int[] { 2, 3, 4 };

            #endregion ScriptableObjects

            Ability ability = new Ability("Soul Suck", "Soul Suck_AB");
            ability.AbilitySprite = ResourceLoader.LoadSprite("SkillSoulSuck");
            ability.Description = "Deal 4 damage to the Opposing enemy, Apply 5 Shield to this party member's current position.\nIf the Opposing enemy is inflicted with Deaths Touch inflict 1-4 scars to the Opposing enemy.\nRemove Deaths touch from the Opposing enemy.";
            ability.Cost = new ManaColorSO[]
            {
                Pigments.Grey,
                Pigments.Red,
                Pigments.Blue
            };
            ability.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageEffect>(), entryVariable = 4, targets = Targeting.Slot_Front},
                new EffectInfo() { effect = ApplyShieldEffect, entryVariable = 5, targets = Targeting.Slot_SelfSlot},
                new EffectInfo() { effect = ApplyScarsEffect_1t4, entryVariable = 0, targets = Targeting.Slot_Front, condition = OpposingHasDeathTouch },
                new EffectInfo() { effect = removeStatusEffectEffect, entryVariable = 0, targets = Targeting.Slot_Front, condition = OpposingHasDeathTouch },
            };
            ability.AnimationTarget = Targeting.Slot_Front;
            ability.Visuals = EXOP._splig.rankedData[0].rankAbilities[0].ability.visuals;
            ability.AddIntentsToTarget(Targeting.Slot_Front, new string[] { "Damage_3_6", "RemoveDeathTouch", "Status_Scars" });
            ability.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { "Field_Shield" });

            ScaledAbility scaledAbility = new ScaledAbility(ability, 3);
            scaledAbility.SetName = "Soul";
            scaledAbility.LeftName = false;
            scaledAbility.AddonName = new string[] { "Reap", "Devour", "Buffet" };
            scaledAbility.Description = new string[]
            {
                "Deal 5 damage to the Opposing enemy, Apply 6 Shield to this party member's current position.\nIf the Opposing enemy is inflicted with Deaths Touch inflict 1-4 scars to the Opposing enemy.\nRemove Deaths touch from the Opposing enemy.",
                "Deal 6 damage to the Opposing enemy, Apply 8 Shield to this party member's current position.\nIf the Opposing enemy is inflicted with Deaths Touch inflict 1-4 scars to the Opposing enemy.\nRemove Deaths touch from the Opposing enemy.",
                "Deal 7 damage to the Opposing enemy, Apply 10 Shield to this party member's current position.\nIf the Opposing enemy is inflicted with Deaths Touch inflict 1-4 scars to the Opposing enemy.\nRemove Deaths touch from the Opposing enemy.",
            };
            scaledAbility.EntryValueScale[0] = new int[3] { 5, 6, 7 };
            scaledAbility.EntryValueScale[1] = new int[3] { 6, 8, 10 };
            scaledAbility.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility.Scale();
            scaledAbility.abilities[1].ability.effects[1].effect = ApplyScarsEffect_2t4;
            scaledAbility.abilities[2].ability.effects[1].effect = ApplyScarsEffect_2t4;

            Ability ability2 = new Ability("Reaper's Bargain", "Reaper's Bargain_AB");
            ability2.AbilitySprite = ResourceLoader.LoadSprite("SkillReapersBargin");
            ability2.Description = "Deal 7 damage to the Opposing enemy, If the Opposing enemy is inflicted with Deaths Touch deal double damage.\nRemove Deaths Touch from the Opposing enemy.";
            ability2.Cost = new ManaColorSO[]
            {
                Pigments.Purple,
                Pigments.Red,
                Pigments.Red
            };
            ability2.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = DealDoubleDamageIfHasStatus, entryVariable = 7, targets = Targeting.Slot_Front},
                new EffectInfo() { effect = removeStatusEffectEffect, entryVariable = 0, targets = Targeting.Slot_Front },
            };
            ability2.AnimationTarget = Targeting.Slot_Front;
            ability2.Visuals = EXOP._bimini.rankedData[0].rankAbilities[0].ability.visuals;
            ability2.AddIntentsToTarget(Targeting.Slot_Front, new string[] { "Damage_7_10", "RemoveDeathTouch" });

            ScaledAbility scaledAbility2 = new ScaledAbility(ability2, 3);
            scaledAbility2.SetName = "Reaper's";
            scaledAbility2.LeftName = false;
            scaledAbility2.AddonName = new string[] { "Bet", "Debt", "Tax" };
            scaledAbility2.Description = new string[]
            {
                "Deal 9 damage to the Opposing enemy, If the Opposing enemy is inflicted with Deaths with Touch deal double damage.\nRemove Deaths Touch from the Opposing enemy.",
                "Deal 11 damage to the Opposing enemy, If the Opposing enemy is inflicted with Deaths with Touch deal double damage.\nRemove Deaths Touch from the Opposing enemy.",
                "Deal 14 damage to the Opposing enemy, If the Opposing enemy is inflicted with Deaths with Touch deal double damage.\nRemove Deaths Touch from the Opposing enemy.",
            };
            scaledAbility2.EntryValueScale[0] = new int[3] { 9, 11, 14 };
            scaledAbility2.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility2.Scale();

            Ability ability3 = new Ability("Soul Slit", "Soul Slit_AB");
            ability3.AbilitySprite = ResourceLoader.LoadSprite("SkillSoulSweep");
            ability3.Description = "Deal 4 damage to the Left and Right enemies.\nIf the left or right enemy is inflicted with deaths touch, deal 4 damage to all enemies in that direction.";
            ability3.Cost = new ManaColorSO[]
            {
                Pigments.Red,
                Pigments.Red,
                Pigments.Grey
            };
            ability3.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageEffect>(), entryVariable = 4, targets = Targeting.GenerateSlotTarget(new int[] { 1, -1})},
                new EffectInfo() { effect = IndirectDamage, entryVariable = 4, targets = Targeting.GenerateSlotTarget(new int[] { -2, -3, -4 }), condition = LeftHasDeathTouch },
                new EffectInfo() { effect = IndirectDamage, entryVariable = 4, targets = Targeting.GenerateSlotTarget(new int[] { 2, 3, 4 }), condition = RightHasDeathTouch }
            };
            ability3.AnimationTarget = Targeting.GenerateSlotTarget(new int[] { 1, -1});
            ability3.Visuals = EXOP._bimini.rankedData[0].rankAbilities[1].ability.visuals;
            ability3.AddIntentsToTarget(Targeting.GenerateSlotTarget(new int[] { 1, -1 }), new string[] { "Damage_3_6" });
            ability3.AddIntentsToTarget(ThanatosTargetingLeft, new string[] { "Damage_3_6" });
            ability3.AddIntentsToTarget(ThanatosTargetingRight, new string[] { "Damage_3_6" });

            ScaledAbility scaledAbility3 = new ScaledAbility(ability3, 3);
            scaledAbility3.SetName = "Soul";
            scaledAbility3.LeftName = false;
            scaledAbility3.AddonName = new string[] { "Sweep", "Puncture", "Pierce" };
            scaledAbility3.Description = new string[]
            {
                "Deal 6 damage to the Left and Right enemies.\nIf the left or right enemy is inflicted with deaths touch, deal 6 damage to all enemies in that direction.",
                "Deal 8 damage to the Left and Right enemies.\nIf the left or right enemy is inflicted with deaths touch, deal 8 damage to all enemies in that direction.",
                "Deal 10 damage to the Left and Right enemies.\nIf the left or right enemy is inflicted with deaths touch, deal 8 damage to all enemies in that direction.",
            };
            scaledAbility3.EntryValueScale[0] = new int[3] { 6, 8, 10 };
            scaledAbility3.EntryValueScale[1] = new int[3] { 6, 8, 8 };
            scaledAbility3.EntryValueScale[2] = new int[3] { 6, 8, 8 };
            scaledAbility3.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility3.intentTypeScale[1][0] = new ScaledAbility.IntentTypeScalePointer(1, IntentTypeScale.Damage);
            scaledAbility3.intentTypeScale[2][0] = new ScaledAbility.IntentTypeScalePointer(2, IntentTypeScale.Damage);
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
                scaledAbility3.abilities[0],
            });
            character.AddLevelData(12, new Ability[]
            {
                scaledAbility.abilities[1],
                scaledAbility2.abilities[1],
                scaledAbility3.abilities[1],
            });
            character.AddLevelData(14, new Ability[]
            {
                scaledAbility.abilities[2],
                scaledAbility2.abilities[2],
                scaledAbility3.abilities[2],
            });
            character.SetMenuCharacterAsFullDPS();
            character.AddCharacter(true);
            character.StartsLocked = false;
        }
    }
}