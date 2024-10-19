
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FiendishFools.StatusEffects
{

    public class EmpoweredSE_SO : StatusEffect_SO
    {

        public override bool IsPositive => true;

        public static void SetUp()
        {
            EmpoweredSE_SO spiritualEnergySE_SO = ScriptableObject.CreateInstance<EmpoweredSE_SO>();
            spiritualEnergySE_SO._StatusID = "Empowered_ID";

            LoadedDBsHandler.StatusFieldDB.TryGetStatusEffect(TempStatusEffectID.Scars_ID.ToString(), out StatusEffect_SO Scars);
            LoadedDBsHandler.StatusFieldDB.TryGetFieldEffect(TempFieldEffectID.Shield_ID.ToString(), out FieldEffect_SO Shield);


            StatusEffectInfoSO statusEffectInfoSO = ScriptableObject.CreateInstance<StatusEffectInfoSO>();
            statusEffectInfoSO._statusName = "Empowered";
            statusEffectInfoSO.icon = ResourceLoader.LoadSprite("EmpoweredIcon");
            statusEffectInfoSO._description = "Increase damage dealt by this character by 1 for each stack of Empowered.\n1 Empowered is lost upon doing damage or taking damage.";
            statusEffectInfoSO._applied_SE_Event = Shield._EffectInfo.AppliedSoundEvent;
            statusEffectInfoSO._removed_SE_Event = Scars._EffectInfo.RemovedSoundEvent;
            statusEffectInfoSO._updated_SE_Event = Shield._EffectInfo.UpdatedSoundEvent;

            spiritualEnergySE_SO._EffectInfo = statusEffectInfoSO;

            LoadedDBsHandler.StatusFieldDB.AddNewStatusEffect(spiritualEnergySE_SO);

            IntentInfoBasic ApplyDeathTouchIntent = new IntentInfoBasic();
            ApplyDeathTouchIntent.id = "ApplyEmpowered";
            ApplyDeathTouchIntent._sprite = ResourceLoader.LoadSprite("EmpoweredIcon");

            IntentInfoBasic RemoveDeathTouchIntent = new IntentInfoBasic();
            RemoveDeathTouchIntent.id = "RemoveEmpowered";
            RemoveDeathTouchIntent._sprite = ResourceLoader.LoadSprite("EmpoweredIcon");
            RemoveDeathTouchIntent._color = Color.black;

            LoadedDBsHandler.IntentDB.AddNewBasicIntent("ApplyEmpowered", ApplyDeathTouchIntent);
            LoadedDBsHandler.IntentDB.AddNewBasicIntent("RemoveEmpowered", RemoveDeathTouchIntent);
        }

        public override void OnTriggerAttached(StatusEffect_Holder holder, IStatusEffector caller)
        {
            CombatManager.Instance.AddObserver(new Action<object, object>(holder.OnEventTriggered_01), "OnWillApplyDamage", caller);
            CombatManager.Instance.AddObserver(new Action<object, object>(holder.OnEventTriggered_02), "OnDamaged", caller);
        }

        public override void OnTriggerDettached(StatusEffect_Holder holder, IStatusEffector caller)
        {
            CombatManager.Instance.RemoveObserver(new Action<object, object>(holder.OnEventTriggered_01), "OnWillApplyDamage", caller);
            CombatManager.Instance.RemoveObserver(new Action<object, object>(holder.OnEventTriggered_02), "OnDamaged", caller);
        }

        public override void OnEventCall_01(StatusEffect_Holder holder, object sender, object args)
        {
            DamageDealtValueChangeException ex = args as DamageDealtValueChangeException;
            ex.AddModifier(new AdditionValueModifier(false, holder.m_ContentMain));
            ReduceDuration(holder, sender as IStatusEffector);
        }

        public override void OnEventCall_02(StatusEffect_Holder holder, object sender, object args)
        {
            ReduceDuration(holder, sender as IStatusEffector);
        }

    }
}

