using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FiendishFools.Effects
{
    public class FieldEffect_ApplyByID_Effect : EffectSO
    {
        [Header("Field")]
        public string _FieldID;

        [Header("Previous Random Option Data")]
        public bool _UseRandomBetweenPrevious;

        [Header("Previous Multiplier Option Data")]
        public bool _UsePreviousExitValueAsMultiplier;

        public int _PreviousExtraAddition;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (!LoadedDBsHandler._StatusFieldDB.TryGetFieldEffect(_FieldID, out FieldEffect_SO _Field)) return false;

            if (_Field == null)
            {
                return false;
            }

            if (_UsePreviousExitValueAsMultiplier)
            {
                entryVariable = _PreviousExtraAddition + entryVariable * base.PreviousExitValue;
            }

            for (int i = 0; i < targets.Length; i++)
            {
                exitAmount += ApplyFieldEffect(stats, targets[i], entryVariable, _Field);
            }

            return exitAmount > 0;
        }

        public int ApplyFieldEffect(CombatStats stats, TargetSlotInfo target, int entryVariable, FieldEffect_SO _Field)
        {
            int num = (_UseRandomBetweenPrevious ? Random.Range(base.PreviousExitValue, entryVariable + 1) : entryVariable);
            if (num < _Field.MinimumRequiredToApply)
            {
                return 0;
            }

            if (!stats.combatSlots.ApplyFieldEffect(target.SlotID, target.IsTargetCharacterSlot, _Field, num))
            {
                return 0;
            }

            return num;
        }
    }
}
