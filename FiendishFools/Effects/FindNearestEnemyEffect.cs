using BrutalAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.UI.CanvasScaler;

namespace FiendishFools.Effects
{
    public class FindNearestEnemyEffect : EffectSO
    {
        public bool IncreaseExitAmountByEntryVariable = true;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            int MoveTo = 0;
            if (stats.EnemiesOnField.Count == 0) return false;
            if (stats.EnemiesOnField.Count == 1)
            {
                foreach (EnemyCombat Enemy in stats.EnemiesOnField.Values)
                {
                    if (Enemy.SlotID == caster.SlotID || !Enemy.IsAlive) return false;
                    MoveTo = Enemy.SlotID > caster.SlotID ? Enemy.SlotID + (Enemy.Size - 1) : Enemy.SlotID;
                }
            }
            else
            {
                List<int> distances = new List<int>();
                foreach (EnemyCombat Enemy in stats.EnemiesOnField.Values)
                {
                    if (Enemy.SlotID == caster.SlotID) return false;
                    if (!Enemy.IsAlive) continue;
                    distances.Add(Enemy.SlotID > caster.SlotID ? Enemy.SlotID + (Enemy.Size - 1) : Enemy.SlotID);
                }
                MoveTo = distances.ToArray().Aggregate((x, y) => Math.Abs(x - caster.SlotID) < Math.Abs(y - caster.SlotID) ? x : y);
            }
            int moveAmount = Math.Max(caster.SlotID - MoveTo, 0);
            Debug.Log(moveAmount);
            for (int i = 0; i < moveAmount; i++)
            {
                if (caster.IsUnitCharacter)
                {
                    int num = MoveTo > caster.SlotID ? 1 : -1;
                    if (caster.SlotID + num >= 0 && caster.SlotID + num < stats.combatSlots.CharacterSlots.Length && stats.combatSlots.SwapCharacters(caster.SlotID, caster.SlotID + num, isMandatory: true))
                    {
                        exitAmount++;
                    }
                }
                else
                {
                    int num2 = MoveTo > caster.SlotID ? caster.Size : -1;
                    if (stats.combatSlots.CanEnemiesSwap(caster.SlotID, caster.SlotID + num2, out var firstSlotSwap, out var secondSlotSwap) && stats.combatSlots.SwapEnemies(caster.SlotID, firstSlotSwap, caster.SlotID + num2, secondSlotSwap))
                    {
                        exitAmount++;
                    }
                }
            }
            if (IncreaseExitAmountByEntryVariable)
                exitAmount = exitAmount * entryVariable;
            return exitAmount > 0;
        }
    }
}
