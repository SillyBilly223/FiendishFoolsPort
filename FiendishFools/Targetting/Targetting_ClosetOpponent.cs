using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FiendishFools.Targetting
{
    public class Targetting_ClosetOpponent : BaseCombatTargettingSO
    {
        public bool getAllies;

        public bool IgnoreOpposing;

        public override bool AreTargetAllies => getAllies;

        public override bool AreTargetSlots => true;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            CombatStats stats = CombatManager._instance._stats;

            TargetSlotInfo SelfTarget = slots.GetCharacterTargetSlot(casterSlotID, 0);
            if (SelfTarget == null || !SelfTarget.HasUnit)
                return new TargetSlotInfo[0];

            IUnit caster = SelfTarget.Unit;
            TargetSlotInfo[] Target = new TargetSlotInfo[1];
            int Slot = -1;

            if (stats.EnemiesOnField.Count == 1)
            {
                foreach (EnemyCombat enemyCombat in stats.EnemiesOnField.Values)
                {
                    if (IgnoreOpposing && enemyCombat.SlotID == caster.SlotID) continue;
                    if (enemyCombat.SlotID == caster.SlotID && enemyCombat.IsAlive && enemyCombat.CurrentHealth > 0) { Slot = enemyCombat.SlotID; break; }
                    Slot = ((enemyCombat.SlotID > caster.SlotID) ? (enemyCombat.SlotID + (enemyCombat.Size - 1)) : enemyCombat.SlotID);
                }
            }
            else
            {
                List<int> ChosenSlots = new List<int>();
                foreach (EnemyCombat enemyCombat2 in stats.EnemiesOnField.Values)
                {
                    if (IgnoreOpposing && enemyCombat2.SlotID == caster.SlotID) continue;
                    if (!enemyCombat2.IsAlive || enemyCombat2.CurrentHealth == 0) continue;
                    if (enemyCombat2.SlotID == caster.SlotID) { ChosenSlots.Clear(); ChosenSlots.Add(enemyCombat2.SlotID); break; };
                    ChosenSlots.Add((enemyCombat2.SlotID > caster.SlotID) ? (enemyCombat2.SlotID + (enemyCombat2.Size - 1)) : enemyCombat2.SlotID);
                }
                if (ChosenSlots.Count == 0) return new TargetSlotInfo[0];
                if (IgnoreOpposing && ChosenSlots.Contains(caster.SlotID)) ChosenSlots.Remove(caster.SlotID);
                Slot = ChosenSlots.ToArray().Aggregate((int x, int y) => (Math.Abs(x - caster.SlotID) < Math.Abs(y - caster.SlotID)) ? x : y);
            }

            TargetSlotInfo ChosenTarget = slots.GetEnemyTargetSlot(Slot, 0);
            if (ChosenTarget == null) return new TargetSlotInfo[0];
            Target[0] = ChosenTarget;
            return Target;
        }
    }
}
