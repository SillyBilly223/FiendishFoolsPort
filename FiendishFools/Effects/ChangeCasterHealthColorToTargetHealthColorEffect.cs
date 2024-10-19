using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FiendishFools.Effects
{
    public class ChangeCasterHealthColorToTargetHealthColorEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            for (int i = 0; i < targets.Length; i++) 
            {
                if (targets[i].HasUnit && caster.ChangeHealthColor(targets[i].Unit.HealthColor))
                {
                    exitAmount++;
                }
            }
            return exitAmount > 0;
        }
    }
}
