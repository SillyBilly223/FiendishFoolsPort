using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FiendishFools.Effects
{
    public class StainedDamageEffect : EffectSO
    {
        [DeathTypeEnumRef]
        public string _DeathTypeID = "Basic";

        [SerializeField]
        public bool _usePreviousExitValue;

        [SerializeField]
        public bool _ignoreShield;

        [SerializeField]
        public bool _indirect;

        [SerializeField]
        public bool _returnKillAsSuccess;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            if (_usePreviousExitValue)
            {
                entryVariable *= PreviousExitValue;
            }

            exitAmount = 0;
            bool flag = false;
            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                if (targetSlotInfo.HasUnit)
                {
                    int targetSlotOffset = areTargetSlots ? targetSlotInfo.SlotID - targetSlotInfo.Unit.SlotID : -1;
                    int amount = entryVariable;
                    DamageInfo damageInfo;
                    if (_indirect)
                    {
                        damageInfo = targetSlotInfo.Unit.Damage(amount, caster, _DeathTypeID, targetSlotOffset, true, true, false);
                    }
                    else
                    {
                        amount = caster.WillApplyDamage(amount, targetSlotInfo.Unit);
                        damageInfo = targetSlotInfo.Unit.Damage(amount, caster, _DeathTypeID, targetSlotOffset, false, true, false);
                    }
                    flag |= damageInfo.beenKilled;
                    exitAmount += damageInfo.damageAmount;
                    if (!_indirect && damageInfo.damageAmount > 0)
                    {
                        CombatManager.Instance.ProcessImmediateAction(new AddManaToManaBarAction(caster.HealthColor, LoadedDBsHandler.CombatData.EnemyPigmentAmount, targetSlotInfo.Unit.IsUnitCharacter, targetSlotInfo.Unit.ID), false);
                    }
                }
            }

            if (!_indirect && exitAmount > 0)
            {
                caster.DidApplyDamage(exitAmount);
            }

            if (!_returnKillAsSuccess)
            {
                return exitAmount > 0;
            }

            return flag;
        }
    }
}
