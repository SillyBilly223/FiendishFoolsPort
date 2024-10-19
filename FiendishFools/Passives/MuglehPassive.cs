using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace FiendishFools.Passives
{
    public class MuglehPassive : BasePassiveAbilitySO
    {
        public override bool IsPassiveImmediate => true;

        public override bool DoesPassiveTrigger => true;

        public override void TriggerPassive(object sender, object args)
        {
            if (!HasMoved)
            {
                IPassiveEffector effector = sender as IPassiveEffector;
                CombatManager.Instance.AddUIAction(new ShowPassiveInformationUIAction(effector.ID, effector.IsUnitCharacter, _passiveName, passiveIcon));
                DamageDealtValueChangeException ex = args as DamageDealtValueChangeException;
                ex.AddModifier(new MultiplyIntValueModifier(true, 2));
            }
        }

        public void TrackMovment(object sender, object args)
        {
            if (!FreeSwap)
            {
                HasMoved = true;
            }
            FreeSwap = false;
        }

        public void ResetMovmentTracker(object sender, object args)
        {
            if (!TurnStartCancle)
            {
                HasMoved = false;
            }
            TurnStartCancle = false;
            FreeSwap = false;
        }

        public void TurnStartTracker(object sender, object args)
        {
            TurnStartCancle = true;
            FreeSwap = false;
            HasMoved = true;
        }

        public void ActivateFreeSwap(object sender, object args)
        {
            FreeSwap = true;
        }

        public override void OnPassiveConnected(IUnit unit)
        {
        }

        public override void OnPassiveDisconnected(IUnit unit)
        {
        }

        public override void CustomOnTriggerAttached(IPassiveEffector caller)
        {
            CombatManager.Instance.AddObserver(new Action<object, object>(TrackMovment), TriggerCalls.OnSwappedTo.ToString(), caller);
            CombatManager.Instance.AddObserver(new Action<object, object>(TrackMovment), TriggerCalls.OnSwapTo.ToString(), caller);
            ///
            CombatManager.Instance.AddObserver(new Action<object, object>(TurnStartTracker), TriggerCalls.OnFirstTurnStart.ToString(), caller);
            CombatManager.Instance.AddObserver(new Action<object, object>(ResetMovmentTracker), TriggerCalls.OnTurnStart.ToString(), caller);
            CombatManager.Instance.AddObserver(new Action<object, object>(ActivateFreeSwap), "SteadyAimFreeSwap", caller);
        }

        public override void CustomOnTriggerDettached(IPassiveEffector caller)
        {
            CombatManager.Instance.RemoveObserver(new Action<object, object>(TrackMovment), TriggerCalls.OnSwappedTo.ToString(), caller);
            CombatManager.Instance.RemoveObserver(new Action<object, object>(TrackMovment), TriggerCalls.OnSwapTo.ToString(), caller);
            ///
            CombatManager.Instance.RemoveObserver(new Action<object, object>(TurnStartTracker), TriggerCalls.OnFirstTurnStart.ToString(), caller);
            CombatManager.Instance.RemoveObserver(new Action<object, object>(ResetMovmentTracker), TriggerCalls.OnTurnStart.ToString(), caller);
            CombatManager.Instance.RemoveObserver(new Action<object, object>(ActivateFreeSwap), "SteadyAimFreeSwap", caller);
        }

        public bool FreeSwap = false;
        public bool TurnStartCancle = false;
        public bool HasMoved = false;
    }
}
