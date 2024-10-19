using FiendishFools.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiendishFools.Passives
{
    public class ZelithPassive : BasePassiveAbilitySO
    {
        public override bool IsPassiveImmediate => false;

        public override bool DoesPassiveTrigger => false;

        public override void TriggerPassive(object sender, object args)
        {
        }

        public override void OnPassiveConnected(IUnit unit)
        {
            ExtraUtils.AddStalker(unit.ID); 
            CombatManager._instance.AddRootAction(new ObsessionAction(unit.ID, true));
        }

        public override void OnPassiveDisconnected(IUnit unit)
        {
            ExtraUtils.RemoveStalker(unit.ID);
        }

        public bool IsObsession;
    }
}
