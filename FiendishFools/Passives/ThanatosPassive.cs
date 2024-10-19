using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FiendishFools.Passives
{
    public class ThanatosPassive : BasePassiveAbilitySO
    {
        public override bool IsPassiveImmediate => false;

        public override bool DoesPassiveTrigger => false;

        public override void TriggerPassive(object sender, object args)
        {
        }

        public override void OnPassiveConnected(IUnit unit)
        {
            CombatManager.Instance.AddPriorityRootAction (new EnemyDeathTouchAction());
            FiendishMain.ThanatosInCombat++;
        }

        public override void OnPassiveDisconnected(IUnit unit)
        {
            FiendishMain.ThanatosInCombat--;
        }
    }
}
