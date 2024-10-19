using System.Reflection;
using BrutalAPI;
using FiendishFools.Condition;
using FiendishFools.Effects;
using FiendishFools.Passives;
using FiendishFools.Targetting;
using MonoMod.RuntimeDetour;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace FiendishFools.Characters
{
    internal class Daymon
    {
        public static void Add()
        {
            #region Passive

            ColdPigmentedPassive coldPigmented = ScriptableObject.CreateInstance<ColdPigmentedPassive>();
            coldPigmented._passiveName = "Cold Pigmented";
            coldPigmented.passiveIcon = ResourceLoader.LoadSprite("ColdPigmentedPassive");
            coldPigmented.m_PassiveID = "Cold Pigmented";
            coldPigmented._enemyDescription = "hhiiyaa!! sigma here!.";
            coldPigmented._characterDescription = "Increase all damage by 4 for each wrong pigment used in an ability.";

            Glossary.CreateAndAddCustom_PassiveToGlossary("Cold Pigmented", "Double all damage for each wrong pigment used in an ability.", ResourceLoader.LoadSprite("ColdPigmentedPassive"));

            #endregion Passive

            Character character = EXOP.CharacterSpriteSetterAddon(EXOP.CharacterInfoSetter("Daymon", Pigments.Purple, EXOP._jumbleGutsWaning.damageSound, EXOP._jumbleGutsWaning.deathSound), "Daymon");
            character.AddPassive(coldPigmented);

            #region ScriptableObjects

            IndexEffectConditon IfFirstEffectTrue = ScriptableObject.CreateInstance<IndexEffectConditon>();
            IfFirstEffectTrue.EffectIndex = 0;

            DamageEffect ReturnIfKill = ScriptableObject.CreateInstance<DamageEffect>();
            ReturnIfKill._returnKillAsSuccess = true;

            DamageEffect IndirectDamage = ScriptableObject.CreateInstance<DamageEffect>();
            IndirectDamage._indirect = true;

            Targetting_ClosetOpponent ClosetOpponentIgnoreOpposing = ScriptableObject.CreateInstance<Targetting_ClosetOpponent>();
            ClosetOpponentIgnoreOpposing.IgnoreOpposing = true;

            #endregion ScriptableObjects

            Ability ability = new Ability("Finger Shots", "FingerShots_AB");
            ability.AbilitySprite = ResourceLoader.LoadSprite("SkillFingerShots");
            ability.Description = "Deal 3 damage to the Opposing enemy, consume 2 non red pigments. if successful, deal 2 indirect damage to the Left and Right enemy.";
            ability.Cost = new ManaColorSO[]
            {
                Pigments.Red,
                Pigments.Red,
                Pigments.Yellow
            };
            ability.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageEffect>(), entryVariable = 3, targets = Targeting.Slot_Front},
                new EffectInfo() { effect = ScriptableObject.CreateInstance<ConsumeColorManaButColorManaEffect>(), entryVariable = 2, targets = Targeting.Slot_SelfSlot},
                new EffectInfo() { effect = IndirectDamage, entryVariable = 3, targets = Targeting.GenerateSlotTarget(new int[] { -1, 1 }), condition = ScriptableObject.CreateInstance<PreviousEffectCondition>()},
            };
            ability.AnimationTarget = Targeting.Slot_Front;
            ability.Visuals = EXOP._clive.rankedData[0].rankAbilities[0].ability.visuals;
            ability.AddIntentsToTarget(Targeting.Slot_Front, new string[] { "Damage_3_6" });
            ability.AddIntentsToTarget(Targeting.GenerateSlotTarget(new int[] { 1, -1 }), new string[] { "Damage_3_6" });
            ability.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { IntentType_GameIDs.Mana_Consume.ToString() });

            ScaledAbility scaledAbility = new ScaledAbility(ability, 3);
            scaledAbility.SetName = "Finger";
            scaledAbility.LeftName = false;
            scaledAbility.AddonName = new string[] { "Guns", "Rockets", "Barrage" };
            scaledAbility.Description = new string[]
            {
                "Deal 4 damage to the Opposing enemy, consume 2 non red pigments. if successful, deal 2 indirect damage to the Left and Right enemy.",
                "Deal 5 damage to the Opposing enemy, consume 2 non red pigments. if successful, deal 3 indirect damage to the Left and Right enemy.",
                "Deal 6 damage to the Opposing enemy, consume 2 non red pigments. if successful, deal 4 indirect damage to the Left and Right enemy.",
            };
            scaledAbility.EntryValueScale[0] = new int[3] { 4, 5, 6 };
            scaledAbility.EntryValueScale[2] = new int[3] { 2, 3, 4 };
            scaledAbility.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility.intentTypeScale[1][0] = new ScaledAbility.IntentTypeScalePointer(2, IntentTypeScale.Damage);
            scaledAbility.Scale();

            Ability ability2 = new Ability("Mean Gesture", "MeanGesture_AB");
            ability2.AbilitySprite = ResourceLoader.LoadSprite("SkillMeanGesture");
            ability2.Description = "Deal 4 damage to the Opposing enemy.\nIf this attack kills, move to the closest enemy and deal 2 damage to the Opposing enemy. Damage is increased by 2 for each time moved.";
            ability2.Cost = new ManaColorSO[]
            {
                Pigments.Red,
                Pigments.Red,
                Pigments.Red,
            };
            ability2.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ReturnIfKill, entryVariable = 4, targets = Targeting.Slot_Front},
                new EffectInfo() { effect = ScriptableObject.CreateInstance<MoveToClosetEnemyEffect>(), entryVariable = 2, targets = Targeting.Slot_Front, condition = IfFirstEffectTrue},
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageIncreaseEffect>(), entryVariable = 2, targets = Targeting.Slot_Front, condition = IfFirstEffectTrue},
            };
            ability2.AnimationTarget = Targeting.Slot_Front;
            ability2.Visuals = EXOP._splig.rankedData[0].rankAbilities[0].ability.visuals;
            ability2.AddIntentsToTarget(Targeting.Slot_Front, new string[] { "Damage_3_6" });
            ability2.AddIntentsToTarget(ClosetOpponentIgnoreOpposing, new string[] { "Damage_3_6" });
            ability2.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { IntentType_GameIDs.Swap_Sides.ToString() });          

            ScaledAbility scaledAbility2 = new ScaledAbility(ability2, 3);
            scaledAbility2.SetName = "Gesture";
            scaledAbility2.AddonName = new string[] { "Rude", "Crule", "Abominable" };
            scaledAbility2.Description = new string[]
            {
                "Deal 6 damage to the Opposing enemy. If this attack kills, move to the closest enemy and deal 3 damage to the Opposing enemy. Damage is increased by 2 for each time moved.",
                "Deal 8 damage to the Opposing enemy. If this attack kills, move to the closest enemy and deal 3 damage to the Opposing enemy. Damage is increased by 2 for each time moved.",
                "Deal 9 damage to the Opposing enemy. If this attack kills, move to the closest enemy and deal 4 damage to the Opposing enemy. Damage is increased by 2 for each time moved.",
            };
            scaledAbility2.EntryValueScale[0] = new int[3] { 6, 8, 9 };
            scaledAbility2.EntryValueScale[2] = new int[3] { 3, 3, 4 };
            scaledAbility2.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility2.intentTypeScale[1][0] = new ScaledAbility.IntentTypeScalePointer(2, IntentTypeScale.Damage);
            scaledAbility2.Scale();

            Ability ability3 = new Ability("Spill Over", "SpillOver_AB");
            ability3.AbilitySprite = ResourceLoader.LoadSprite("SkillBoilOver");
            ability3.Description = "Produce 1 pigments of this party member health color. Heal this party member 3 health.";
            ability3.Cost = new ManaColorSO[]
            {
                Pigments.Blue,
                Pigments.Red,
            };
            ability3.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<GenerateCasterHealthManaEffect>(), entryVariable = 1, targets = Targeting.Slot_SelfSlot},
                new EffectInfo() { effect = ScriptableObject.CreateInstance<HealEffect>(), entryVariable = 3, targets = Targeting.Slot_SelfSlot},
            };
            ability3.AnimationTarget = Targeting.Slot_Front;
            ability3.Visuals = EXOP._rags.rankedData[0].rankAbilities[2].ability.visuals;
            ability3.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { IntentType_GameIDs.Mana_Generate.ToString(), IntentType_GameIDs.Heal_1_4.ToString() });

            ScaledAbility scaledAbility3 = new ScaledAbility(ability3, 3, true);
            scaledAbility3.SetName = "Over";
            scaledAbility3.AddonName = new string[] { "Bubble", "Gush", "Boil" };
            scaledAbility3.Description = new string[]
            {
                "Produce 1 pigments of this party member health color. Heal this party member 3 health.",
                "Produce 2 pigments of this party member health color. Heal this party member 4 health.",
                "Produce 2 pigments of this party member health color. Heal this party member 4 health.",
            };
            scaledAbility3.EntryValueScale[0] = new int[3] { 1, 2, 2 };
            scaledAbility3.EntryValueScale[1] = new int[3] { 3, 4, 4 };
            scaledAbility3.Scale();

            character.AddLevelData(18, new Ability[]
            {
                ability,
                ability2,
                ability3
            });
            character.AddLevelData(22, new Ability[]
            {
                scaledAbility.abilities[0],
                scaledAbility2.abilities[0],
                scaledAbility3.abilities[0]
            });
            character.AddLevelData(24, new Ability[]
            {
                scaledAbility.abilities[1],
                scaledAbility2.abilities[1],
                scaledAbility3.abilities[1]
            });
            character.AddLevelData(28, new Ability[]
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





