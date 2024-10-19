using UnityEngine;

namespace FiendishFools.Passives
{
    
    public class ColdPigmentedPassive : BasePassiveAbilitySO
    {
        
        public override bool IsPassiveImmediate => true;

        public override bool DoesPassiveTrigger => true;


        public override void TriggerPassive(object sender, object args)
        {
            CharacterCombat Unit = sender as CharacterCombat;
            DamageDealtValueChangeException DamageExpectation = (DamageDealtValueChangeException)args;
            DamageExpectation.AddModifier(new ScarsValueModifier(4 * Unit.LastCalculatedWrongMana));
        }

        public override void OnPassiveConnected(IUnit unit)
        {
            CombatManager._instance.AddObserver(TryTriggerPassive, "OnWillApplyDamage", unit);
        }

        public override void OnPassiveDisconnected(IUnit unit)
        {
            CombatManager._instance.RemoveObserver(TryTriggerPassive, "OnWillApplyDamage", unit);
        }
    }
}
