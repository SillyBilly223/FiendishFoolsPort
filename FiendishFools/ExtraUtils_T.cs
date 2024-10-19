using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine.Windows.Speech;

namespace FiendishFools
{
    public static class ExtraUtils_T
    {
        #region HiddenHealth

        public static List<IUnit> HiddenHealthUnit = new List<IUnit>();

        public static bool SetUnitHealthHidden(this IUnit unit)
        {
            if (!HiddenHealthUnit.Contains(unit))
            { HiddenHealthUnit.Add(unit); return true; }
            return false;
        }

        public static bool RemoveUnitHiddenHealth (this IUnit unit)
        {
            if (!HiddenHealthUnit.Contains(unit)) return false;
            HiddenHealthUnit.Remove(unit);
            return true;
        }

        public static bool IsUnitHealthHidden(this IUnit unit)
        {
            return HiddenHealthUnit.Contains(unit);
        }

        public static bool IsUnitIDInHealthHiddenPool(int ID, bool IsCharacter)
        {
            for (int i = 0; i < HiddenHealthUnit.Count; i++)
                if (HiddenHealthUnit[i].IsUnitCharacter == IsCharacter && HiddenHealthUnit[i].ID == ID)
                    return true;
            return false;
        }

        #endregion HiddenHealth

        #region Hooks

        public static void UpdateHealthLayout(Action<EnemyInfoLayout, ManaColorSO, int, int, HealthBarType> orig, EnemyInfoLayout layout, ManaColorSO health, int current, int max, HealthBarType barType)
        {
            if (IsUnitIDInHealthHiddenPool(layout.FieldEntity.EnemyID, false)) layout._health.SetInformationHidden(health, max, barType);
            else layout._health.SetInformation(health, current, max, barType);
        }

        public static void UpdateHealthAndAbilityLayout(Action<CharacterInfoLayout, ManaColorSO, int, int, HealthBarType> orig, CharacterInfoLayout layout, ManaColorSO health, int current, int max, HealthBarType barType)
        {
            if (IsUnitIDInHealthHiddenPool(layout.FieldEntity.CharacterID, true)) layout._health.SetInformationHidden(health, max, barType);
            else layout._health.SetInformation(health, current, max, barType);
        }

        public static void SetCharacterInformation(Action<InformationZoneLayout, CharacterCombatUIInfo, SlotCombatUIInfo> orig, InformationZoneLayout self, CharacterCombatUIInfo character, SlotCombatUIInfo characterSlot)
        {
            self.UpdateUnitName(character.Name);
            self.UpdateUnitPortrait(character.Portrait);

            if (IsUnitIDInHealthHiddenPool(character.ID, true)) self._unitHealthBar.SetInformationHidden(character.HealthColor, character.MaxHealth, character.HealthBarSpriteType);
            else self.UpdateUnitHealth(character.HealthColor, character.CurrentHealth, character.MaxHealth, character.HealthBarSpriteType);

            self.UpdateUnitItem(usesItem: true, character.Item, character.ItemConsumed);
            self.UpdatePassives(character.ID, character.Passives.ToArray());
            self.UpdateStatusEffects(character.ID, character.StatusEffectsSpriteArray, character.StatusEffectTextArray);
            self.UpdateFieldEffects(characterSlot.SlotID, characterSlot.FieldEffectsSpriteArray, characterSlot.FieldEffectTextArray);
        }

        public static void SetEnemyInformation(Action<InformationZoneLayout, EnemyCombatUIInfo, SlotCombatUIInfo[]> orig, InformationZoneLayout self, EnemyCombatUIInfo enemy, SlotCombatUIInfo[] enemySlots)
        {
            self.UpdateUnitName(enemy.Name);
            self.UpdateUnitPortrait(enemy.Portrait);

            if (IsUnitIDInHealthHiddenPool(enemy.ID, false)) self._unitHealthBar.SetInformationHidden(enemy.HealthColor, enemy.MaxHealth, enemy.HealthBarSpriteType);
            else self.UpdateUnitHealth(enemy.HealthColor, enemy.CurrentHealth, enemy.MaxHealth, enemy.HealthBarSpriteType);

            self.UpdateUnitItem(usesItem: false, null, isConsumed: false);
            self.UpdatePassives(enemy.ID, enemy.Passives.ToArray());
            self.UpdateStatusEffects(enemy.ID, enemy.StatusEffectsSpriteArray, enemy.StatusEffectTextArray);
            self.UpdateFieldEffects(enemySlots);
        }

        public static void SetInformationHidden(this CombatHealthBarLayout bar, ManaColorSO health, int max, HealthBarType barType)
        {
            bar._unitHealth.sprite = health.healthSprite; bar._unitHealth.fillAmount = max;
            bar._unitCurrentHealthText.text = "??"; bar._unitMaxHealthText.text = "??";
            switch (barType)
            {
                case HealthBarType.Basic:
                    bar._unitHealthBorder_L.sprite = bar._basicBar_L;
                    bar._unitHealthBorder_R.sprite = bar._basicBar_R;
                    break;
                case HealthBarType.CombatCharacterWithAbility:
                    bar._unitHealthBorder_L.sprite = bar._characterWithAbilityBar_L;
                    bar._unitHealthBorder_R.sprite = bar._characterWithAbilityBar_R;
                    break;
                case HealthBarType.CombatCharacterNoAbility:
                    bar._unitHealthBorder_L.sprite = bar._characterNoAbilityBar_L;
                    bar._unitHealthBorder_R.sprite = bar._characterNoAbilityBar_R;
                    break;
            }
        }

        public static void InitializeCombat(Action<CombatManager> orig, CombatManager self)
        {
            HiddenHealthUnit.Clear();
            orig(self);
        }

        #endregion Hooks

        public static void SetUp()
        {
            new Hook(typeof(CombatManager).GetMethod("InitializeCombat", (BindingFlags)(-1)), ExtraUtils_T.InitializeCombat);

            new Hook(typeof(InformationZoneLayout).GetMethod("SetEnemyInformation", (BindingFlags)(-1)), ExtraUtils_T.SetEnemyInformation);
            new Hook(typeof(InformationZoneLayout).GetMethod("SetCharacterInformation", (BindingFlags)(-1)), ExtraUtils_T.SetCharacterInformation);

            new Hook(typeof(EnemyInfoLayout).GetMethod("UpdateHealthLayout", (BindingFlags)(-1)), ExtraUtils_T.UpdateHealthLayout);
            new Hook(typeof(CharacterInfoLayout).GetMethod("UpdateHealthAndAbilityLayout", (BindingFlags)(-1)), ExtraUtils_T.UpdateHealthAndAbilityLayout);

            new Hook(typeof(CombatHealthBarLayout).GetMethod("SetInformationHidden", (BindingFlags)(-1)), ExtraUtils_T.SetInformationHidden);
        }
    }
}
