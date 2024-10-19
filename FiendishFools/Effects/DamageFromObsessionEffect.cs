using System;
using System.Collections.Generic;
using System.Text;

namespace FiendishFools.Effects
{
    public class DamageFromObsessionEffect : EffectSO
    {
        [DeathTypeEnumRef]
        public string _DeathTypeID = "Basic";

        public bool _usePreviousExitValue;

        public bool _ignoreShield;

        public bool _returnKillAsSuccess;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (!ExtraUtils.TryGetCurrentObsession(caster.ID, out int ObsessionID) || !ExtraUtils.TryGetCharacterFromField(ObsessionID, out CharacterCombat Obsession)) return false;

            if (_usePreviousExitValue)
                entryVariable *= base.PreviousExitValue;

            bool flag = false;
            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                if (targetSlotInfo.HasUnit)
                {
                    int targetSlotOffset = (areTargetSlots ? (targetSlotInfo.SlotID - targetSlotInfo.Unit.SlotID) : (-1));
                    int amount = entryVariable;

                    amount = Obsession.WillApplyDamage(amount, targetSlotInfo.Unit);
                    DamageInfo damageInfo = targetSlotInfo.Unit.Damage(amount, Obsession, _DeathTypeID, targetSlotOffset, true, true, _ignoreShield);

                    flag |= damageInfo.beenKilled;
                    exitAmount += damageInfo.damageAmount;
                }
            }

            if (exitAmount > 0)
                Obsession.DidApplyDamage(exitAmount);

            if (!_returnKillAsSuccess)
                return exitAmount > 0;

            return flag;
        }
    }
}
