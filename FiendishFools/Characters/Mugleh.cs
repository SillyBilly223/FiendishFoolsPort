using BrutalAPI;
using FiendishFools;
using FiendishFools.Effects;
using FiendishFools.Passives;
using UnityEngine;

namespace FiendishFools.Characters
{
    public class Mugleh
    {
        public static void Add()
        {
            #region Passive

            MuglehPassive muglehPassive = ScriptableObject.CreateInstance<MuglehPassive>();
            muglehPassive._passiveName = "Steady Aim";
            muglehPassive.passiveIcon = ResourceLoader.LoadSprite("SteadyAimPassive");
            muglehPassive.m_PassiveID = "Steady Aim";
            muglehPassive._enemyDescription = "This enemy wants to kill themself and so should you";
            muglehPassive._characterDescription = "If this party member has not moved this turn, deal double damage.\nThis will not activate on the first turn.";
            muglehPassive.doesPassiveTriggerInformationPanel = false;
            muglehPassive._triggerOn = new TriggerCalls[]
            {
                TriggerCalls.OnWillApplyDamage
            };

            GlossaryPassives glossaryPassives = new GlossaryPassives(
                "Steady Aim", 
                "If this party member has not moved this turn, deal double damage.\nThis will not activate on the first turn.",
                ResourceLoader.LoadSprite("SteadyAimPassive")
                );
            glossaryPassives.glossaryID = (GlossaryLocID)121;

            LoadedDBsHandler.PassiveDB.AddNewPassive("SteadyAim_ID", muglehPassive);
            LoadedDBsHandler.GlossaryDB.AddNewPassive(glossaryPassives);

            #endregion Passive

            #region ScriptableObjects

            DamageEffect damageEffect = ScriptableObject.CreateInstance<DamageEffect>();
            damageEffect._returnKillAsSuccess = true;
            PreviousEffectCondition previousEffectCondition = ScriptableObject.CreateInstance<PreviousEffectCondition>();
            previousEffectCondition.wasSuccessful = true;
            FieldEffect_ApplyByID_Effect fieldEffect_Apply = ScriptableObject.CreateInstance<FieldEffect_ApplyByID_Effect>();
            fieldEffect_Apply._FieldID = TempFieldEffectID.Shield_ID.ToString();

            #endregion ScriptableObjects

            Character character = EXOP.CharacterSpriteSetterAddon(EXOP.CharacterInfoSetter("Mugleh", Pigments.Red, EXOP._mungCH.damageSound, EXOP._mungCH.deathSound), "Mugleh");
            character.AddPassive(muglehPassive);

            Ability ability = new Ability("Shoddy Ricochet", "Shoddy Ricochet_AB");
            ability.AbilitySprite = ResourceLoader.LoadSprite("SkillRicochete");
            ability.Description = "Deal 5 damage to the far Left and far Right enemies.";
            ability.Cost = EXOP.MultiPigments("Red", 3);
            ability.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageEffect>(), entryVariable = 5, targets = Targeting.Slot_AllyFarSides}
            };
            ability.AnimationTarget = Targeting.Slot_OpponentFarSides;
            ability.Visuals = EXOP._clive.rankedData[0].rankAbilities[2].ability.visuals;
            ability.AddIntentsToTarget(Targeting.Slot_OpponentFarSides, new string[] { "Damage_3_6" });

            ScaledAbility scaledAbility = new ScaledAbility(ability, 3);
            scaledAbility.SetName = "Ricochet";
            scaledAbility.AddonName = new string[3] { "Twisted", "Bloody", "Deadly" };
            scaledAbility.Description = new string[3]
            {
                "Deal 7 damage to the far Left and far Right enemies.",
                "Deal 9 damage to the far Left and far Right enemies.",
                "Deal 11 damage to the far Left and far Right enemies."
            };
            scaledAbility.EntryValueScale[0] = new int[3] { 7, 9, 11 };
            scaledAbility.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility.Scale();

            Ability ability2 = new Ability("Quick Draw", "Quick Draw_AB");
            ability2.AbilitySprite = ResourceLoader.LoadSprite("SkillQuickDraw"); ;
            ability2.Description = "Deal 6 damage to the opposing enemy. if this kills, move to the left or right and refresh this party member";
            ability2.Cost = new ManaColorSO[]
            {
                Pigments.Red,
                Pigments.Red,
                Pigments.Blue,
            };
            ability2.Effects = new EffectInfo[]
            {
                new EffectInfo() {effect = damageEffect, entryVariable = 6, targets = Targeting.Slot_Front},
                new EffectInfo() {effect = ScriptableObject.CreateInstance<SwapToSidesEffect>(), entryVariable = 1, targets = Targeting.Slot_SelfSlot, condition = previousEffectCondition},
                new EffectInfo() {effect = ScriptableObject.CreateInstance<RefreshAbilityUseEffect>(), entryVariable = 1, targets = Targeting.Slot_SelfSlot, condition = previousEffectCondition},
            };
            ability2.AnimationTarget = Targeting.Slot_Front;
            ability2.Visuals = EXOP._anton.rankedData[0].rankAbilities[0].ability.visuals;
            ability2.AddIntentsToTarget(Targeting.Slot_Front, new string[] { "Damage_3_6" });
            ability2.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { "Swap_Sides", "Other_Refresh" });

            ScaledAbility scaledAbility2 = new ScaledAbility(ability2, 3);
            scaledAbility2.SetName = "Draw";
            scaledAbility2.AddonName = new string[] { "Fast", "Speedy", "Lightning" };
            scaledAbility2.Description[0] = "Deal 8 damage to the opposing enemy. if this kills, move to the left or right and refresh this party member.";
            scaledAbility2.Description[1] = "Deal 10 damage to the opposing enemy. if this kills, move to the left or right and refresh this party member.";
            scaledAbility2.Description[2] = "Deal 12 damage to the opposing enemy. if this kills, move to the left or right and refresh this party member.";
            scaledAbility2.EntryValueScale[0] = new int[3] { 8, 10, 12 };
            scaledAbility.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility2.Scale();

            Ability ability3 = new Ability("Silent StandOff", "Silent StandOff_AB");
            ability3.AbilitySprite = ResourceLoader.LoadSprite("SkillStandOff");
            ability3.Description = "Move Left or Right, this does not disable Steady Aim. apply 6 Shield to this party member's new position. refresh this party member.";
            ability3.Cost = new ManaColorSO[]
            {
                Pigments.Red,
                Pigments.Yellow,
            };
            ability3.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance <SetFreeSwapMuglehEffect>(), entryVariable = 0, targets = Targeting.Slot_SelfSlot },
                new EffectInfo() { effect = ScriptableObject.CreateInstance < SwapToSidesEffect >(), entryVariable = 1, targets = Targeting.Slot_SelfSlot },
                new EffectInfo () { effect = fieldEffect_Apply, entryVariable = 6, targets = Targeting.Slot_SelfSlot },
                new EffectInfo () { effect = ScriptableObject.CreateInstance < RefreshAbilityUseEffect >(), entryVariable = 1, targets = Targeting.Slot_SelfSlot },
            };
            ability3.AnimationTarget = Targeting.Slot_SelfSlot;
            ability3.Visuals = EXOP._boyle.rankedData[0].rankAbilities[2].ability.visuals;
            
            ScaledAbility scaledAbility3 = new ScaledAbility(ability3, 3);
            scaledAbility3.SetName = "StandOff";
            scaledAbility3.AddonName = new string[] { "Scary", "Unnerving", "Intense" };
            scaledAbility3.Description[0] = "Move Left or Right, this does not disable Steady Aim. apply 8 Shield to this party member's new position. refresh this party member.";
            scaledAbility3.Description[1] = "Move Left or Right, this does not disable Steady Aim. apply 10 Shield to this party member's new position. refresh this party member.";
            scaledAbility3.Description[2] = "Move Left or Right, this does not disable Steady Aim. apply 12 Shield to this party member's new position. refresh this party member.";
            scaledAbility3.EntryValueScale[1] = new int[3] { 8, 10, 12 };
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
