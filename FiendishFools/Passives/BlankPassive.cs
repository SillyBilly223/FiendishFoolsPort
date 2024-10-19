using FiendishFools.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiendishFools.Passives
{
    public class BlankPassive : BasePassiveAbilitySO
    {
        public override bool IsPassiveImmediate => false;

        public override bool DoesPassiveTrigger => false;

        public override void TriggerPassive(object sender, object args)
        {
        }

        public override void OnPassiveConnected(IUnit unit)
        {
        }

        public override void OnPassiveDisconnected(IUnit unit)
        {
            CombatManager._instance.AddRootAction(new ObsessionAction(unit.ID, false));
        }
    }
}
