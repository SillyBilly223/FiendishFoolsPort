using UnityEngine;

namespace FiendishFools.Effects
{
    public class StatusEffect_ApplyRandomAmount_Effect : EffectSO
    {
        [Header("Status")]
        public StatusEffect_SO _Status;

        [Header("Data")]
        public Vector2 ApplyRange;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (_Status == null)
            {
                return false;
            }

            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].HasUnit)
                {
                    int ApplyAmount = Mathf.RoundToInt(Random.Range(ApplyRange.x, ApplyRange.y + 1));
                    if (targets[i].Unit.ApplyStatusEffect(_Status, ApplyAmount))
                    {
                        exitAmount += ApplyAmount;
                    }
                }
            }

            return exitAmount > 0;
        }
    }
}
