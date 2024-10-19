using System;
using System.Collections.Generic;
using System.Text;

namespace FiendishFools.Effects
{
    public class SetFreeSwapMuglehEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            CombatManager.Instance.PostNotification("SteadyAimFreeSwap", caster, null);
            exitAmount++;
            return exitAmount > 0;
        }
    }
}
