using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FiendishFools.Effects
{
    public class MoveToClosetEnemyEffect : EffectSO
    {

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            int Slot = 0;
            if (stats.EnemiesOnField.Count == 1)
            {
                foreach (EnemyCombat enemyCombat in stats.EnemiesOnField.Values)
                {
                    if (enemyCombat.SlotID == caster.SlotID || !enemyCombat.IsAlive || enemyCombat.CurrentHealth == 0) return false;
                    Slot = ((enemyCombat.SlotID > caster.SlotID) ? (enemyCombat.SlotID + (enemyCombat.Size - 1)) : enemyCombat.SlotID);
                }
            }
            else
            {
                List<int> ChosenSlots = new List<int>();
                foreach (EnemyCombat enemyCombat2 in stats.EnemiesOnField.Values)
                {
                    if (enemyCombat2.SlotID == caster.SlotID && enemyCombat2.IsAlive && enemyCombat2.CurrentHealth > 0) return false;
                    if (!enemyCombat2.IsAlive || enemyCombat2.CurrentHealth == 0) continue;
                    ChosenSlots.Add((enemyCombat2.SlotID > caster.SlotID) ? (enemyCombat2.SlotID + (enemyCombat2.Size - 1)) : enemyCombat2.SlotID);
                }
                if (ChosenSlots.Count == 0) return false;
                Slot = ChosenSlots.ToArray().Aggregate((int x, int y) => (Math.Abs(x - caster.SlotID) < Math.Abs(y - caster.SlotID)) ? x : y);
            }
            int Distance = Math.Abs(caster.SlotID - Slot);
            for (int i = 0; i < Distance; i++)
            {
                bool isUnitCharacter = caster.IsUnitCharacter;
                if (isUnitCharacter)
                {
                    int Direction = (Slot > caster.SlotID) ? 1 : -1;
                    if (caster.SlotID + Direction >= 0 && caster.SlotID + Direction < stats.combatSlots.CharacterSlots.Length)
                    {
                        if (stats.combatSlots.SwapCharacters(caster.SlotID, caster.SlotID + Direction, isMandatory: true))
                        {
                            exitAmount++;
                        }

                        continue;
                    }

                    Direction *= -1;
                    if (caster.SlotID + Direction >= 0 && caster.SlotID + Direction < stats.combatSlots.CharacterSlots.Length && stats.combatSlots.SwapCharacters(caster.SlotID, caster.SlotID + Direction, isMandatory: true))
                    {
                        exitAmount++;
                    }
                }
                else
                {
                    int Direction = (Slot > caster.SlotID) ? caster.Size : -1;

                    if (stats.combatSlots.CanEnemiesSwap(caster.SlotID, caster.SlotID + Direction, out var firstSlotSwap, out var secondSlotSwap))
                    {
                        if (stats.combatSlots.SwapEnemies(caster.SlotID, firstSlotSwap, caster.SlotID + Direction, secondSlotSwap, isMandatory: true))
                        {
                            exitAmount++;
                        }

                        continue;
                    }

                    Direction = ((Direction < 0) ? caster.Size : (-1));
                    if (stats.combatSlots.CanEnemiesSwap(caster.SlotID, caster.SlotID + Direction, out firstSlotSwap, out secondSlotSwap) && stats.combatSlots.SwapEnemies(caster.SlotID, firstSlotSwap, caster.SlotID + Direction, secondSlotSwap, isMandatory: true))
                    {
                        exitAmount++;
                    }
                }
            }
            exitAmount *= entryVariable;
            return exitAmount > 0;
        }
    }
}
