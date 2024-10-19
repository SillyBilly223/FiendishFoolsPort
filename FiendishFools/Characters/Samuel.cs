using BrutalAPI;
using FiendishFools.Effects;
using FiendishFools.Targetting;
using UnityEngine;

namespace FiendishFools.Characters
{
    public class Samuel
    {
        public static void Add()
        {
            Character character = EXOP.CharacterSpriteSetterAddon(EXOP.CharacterInfoSetter("Samuel", Pigments.Purple, EXOP._clive.deathSound, EXOP._clive.deathSound), "Samuel");

            #region ScriptableObjects

            LoadedDBsHandler._StatusFieldDB.TryGetStatusEffect(TempStatusEffectID.OilSlicked_ID.ToString(), out StatusEffect_SO OilSilcked);

            StatusEffect_Apply_Effect ApplyOilSilcked = ScriptableObject.CreateInstance<StatusEffect_Apply_Effect>();
            ApplyOilSilcked._Status = OilSilcked;

            #endregion ScriptableObjects

            Ability ability = new Ability("Iron Pierce", "Iron Pierce_AB");
            ability.AbilitySprite = ResourceLoader.LoadSprite("SkillPeirce");
            ability.Description = "Move to the nearest enemy, deal 4 damage to the Opposing enemy. Damage is increased by 4 for the amount of times moved.";
            ability.Cost = new ManaColorSO[] { Pigments.Red, Pigments.Red, Pigments.Yellow };
            ability.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<MoveToClosetEnemyEffect>(), entryVariable = 4, targets = Targeting.Slot_Front },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<AnimationVisualReturnPreviousEffect>(), entryVariable = 0, targets = Targeting.Slot_Front },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageIncreaseEffect>(), entryVariable = 4, targets = Targeting.Slot_Front },
            };
            ability.AnimationTarget = Targeting.Slot_SelfSlot;
            ability.Visuals = EXOP._clive.rankedData[0].rankAbilities[2].ability.visuals;
            ability.AddIntentsToTarget(ScriptableObject.CreateInstance<Targetting_ClosetOpponent>(), new string[] { IntentType_GameIDs.Damage_3_6.ToString() });

            ScaledAbility scaledAbility = new ScaledAbility(ability, 3);
            scaledAbility.SetName = "Pierce";
            scaledAbility.AddonName = new string[] { "Titanium", "Chromium", "Tungusten" };
            scaledAbility.Description = new string[] 
            {
                "Move to the nearest enemy, deal 5 damage to the Opposing enemy. Damage is increased by 4 for the amount of times moved.",
                "Move to the nearest enemy, deal 7 damage to the Opposing enemy. Damage is increased by 5 for the amount of times moved.",
                "Move to the nearest enemy, deal 8 damage to the Opposing enemy. Damage is increased by 5 for the amount of times moved.",
            };
            scaledAbility.EntryValueScale[0] = new int[3] { 4, 5, 5 };
            scaledAbility.EntryValueScale[2] = new int[3] { 5, 7, 8 };
            scaledAbility.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(2, IntentTypeScale.Damage);
            scaledAbility.Scale();

            Ability ability2 = new Ability("Painful Skewer", "Painful Skewer_AB");
            ability2.AbilitySprite = ResourceLoader.LoadSprite("SkillShishkebab");
            ability2.Description = "Move to the centeral position, deal 8 damage to the Opposing enemy. Damage is increased by 3 for the amount of times moved.";
            ability2.Cost = new ManaColorSO[] { Pigments.Red, Pigments.Red, Pigments.Purple };
            ability2.Effects = new  EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<MoveToSlotEffect>(), entryVariable = 3, targets = EXOP.SlotGeneric(new int[] { 2 }, false) },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<AnimationVisualReturnPreviousEffect>(), entryVariable = 0, targets = Targeting.Slot_Front },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageIncreaseEffect>(), entryVariable = 8, targets = Targeting.Slot_Front },
            };
            ability2.AnimationTarget = Targeting.Slot_SelfSlot;
            ability2.Visuals = EXOP._fennec.rankedData[0].rankAbilities[0].ability.visuals;
            ability2.AddIntentsToTarget(EXOP.SlotGeneric(new int[] { 2 }, false), new string[] { IntentType_GameIDs.Damage_3_6.ToString() });
            ability.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { IntentType_GameIDs.Swap_Sides.ToString() });

            ScaledAbility scaledAbility2 = new ScaledAbility(ability2, 3);
            scaledAbility2.SetName = "Skewer";
            scaledAbility2.AddonName = new string[] { "vicious", "Savage", "Brutal" };
            scaledAbility2.Description = new string[] 
            { 
                "Move to the centeral position, deal 10 damage to the Opposing enemy. Damage is increased by 3 for the amount of times moved.",
                "Move to the centeral position, deal 12 damage to the Opposing enemy. Damage is increased by 4 for the amount of times moved.",
                "Move to the centeral position, deal 14 damage to the Opposing enemy. Damage is increased by 4 for the amount of times moved."
            };
            scaledAbility2.EntryValueScale[0] = new int[3] { 3, 4, 4 };
            scaledAbility2.EntryValueScale[2] = new int[3] { 10, 12, 14 };
            scaledAbility2.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(2, IntentTypeScale.Damage);
            scaledAbility2.Scale();

            Ability ability3 = new Ability("Toxic SideProduct", "Toxic SideProduct_AB");
            ability3.AbilitySprite = ResourceLoader.LoadSprite("SkillImpalement");
            ability3.Description = "Inflict 2 OilSilked to the Opposing enemy. Deal 4 damage to the Opposing enemy.";
            ability3.Cost = new ManaColorSO[] { Pigments.Red, Pigments.Red };
            ability3.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ApplyOilSilcked, entryVariable = 2, targets = Targeting.Slot_Front  },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageEffect>(), entryVariable = 4, targets = Targeting.Slot_Front },
            };
            ability3.AnimationTarget = Targeting.Slot_SelfSlot;
            ability3.Visuals = EXOP._jumbleGutsFlummoxing.abilities[0].ability.visuals;
            ability3.AddIntentsToTarget(Targeting.Slot_Front, new string[] { IntentType_GameIDs.Damage_3_6.ToString(), IntentType_GameIDs.Status_OilSlicked.ToString() });

            ScaledAbility scaledAbility3 = new ScaledAbility(ability3, 3);
            scaledAbility3.SetName = "SideProduct";
            scaledAbility3.AddonName = new string[] { "Noxious", "Deadly", "Fatal" };
            scaledAbility3.Description = new string[]
            {
                "Inflict 2 OilSilked to the Opposing enemy. Deal 6 damage to the Opposing enemy.",
                "Inflict 3 OilSilked to the Opposing enemy. Deal 7 damage to the Opposing enemy.",
                "Inflict 3 OilSilked to the Opposing enemy. Deal 8 damage to the Opposing enemy.",
            };
            scaledAbility2.EntryValueScale[0] = new int[3] { 2, 3, 3 };
            scaledAbility3.EntryValueScale[1] = new int[3] { 6, 7, 8 };
            scaledAbility3.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(1, IntentTypeScale.Damage);
            scaledAbility3.Scale();

            character.AddLevelData(15, new Ability[]
            {
                ability,
                ability2,
                ability3
            });
            character.AddLevelData(18, new Ability[]
            {
                scaledAbility.abilities[0],
                scaledAbility2.abilities[0],
                scaledAbility3.abilities[0],
            });
            character.AddLevelData(20, new Ability[]
            {
                scaledAbility.abilities[1],
                scaledAbility2.abilities[1],
                scaledAbility3.abilities[1],
            });
            character.AddLevelData(22, new Ability[]
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
