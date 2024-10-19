using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static UnityEngine.Graphics;
using UnityEngine;
using Random = UnityEngine.Random;
using FiendishFools.StatusEffects;

namespace FiendishFools
{
    public class EnemyDeathTouchAction : CombatAction
    {
        public override IEnumerator Execute(CombatStats stats)
        {
            if (stats.Enemies.Values.Count == 0) yield break;

            foreach (EnemyCombat enemyCombat in stats.EnemiesOnField.Values)
                if (enemyCombat.IsAlive && enemyCombat.ContainsStatusEffect("DeathTouch_ID")) yield break;

            LoadedDBsHandler.StatusFieldDB.TryGetStatusEffect("DeathTouch_ID", out StatusEffect_SO DeathTouch);
            stats.Enemies[Random.Range(0, stats.EnemiesOnField.Values.Count)].ApplyStatusEffect(DeathTouch, 0, 0);
            yield break;
        }
    }
}