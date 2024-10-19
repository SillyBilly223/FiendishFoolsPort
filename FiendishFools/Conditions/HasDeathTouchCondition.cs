using BrutalAPI;
using UnityEngine;

namespace FiendishFools.Conditions
{
    public class HasDeathTouchCondition : EffectConditionSO
    {

        public BaseCombatTargettingSO targeting = Targeting.Slot_Front;

        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            TargetSlotInfo[] Targets = targeting.GetTargets(CombatManager._instance._stats.combatSlots, caster.SlotID, caster.IsUnitCharacter);
            Debug.Log("length" + Targets.Length);
            for (int i = 0; i < Targets.Length; i++)
            {
                Debug.Log("Slot" + Targets[i].SlotID + " " + Targets[i].HasUnit);
                if (Targets[i].HasUnit && Targets[i].Unit.ContainsStatusEffect("DeathTouch_ID")) return true;
            }
            return false;
        }

    }
}
