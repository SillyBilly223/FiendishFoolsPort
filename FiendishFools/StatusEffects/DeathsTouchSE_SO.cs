using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FiendishFools.StatusEffects
{
    public class DeathsTouchSE_SO : StatusEffect_SO
    {

        public static void SetUp()
        {
            DeathsTouchSE_SO deathsTouchSE_SO = ScriptableObject.CreateInstance<DeathsTouchSE_SO>();
            deathsTouchSE_SO._StatusID = "DeathTouch_ID";

            LoadedDBsHandler.StatusFieldDB.TryGetStatusEffect(TempStatusEffectID.Cursed_ID.ToString(), out StatusEffect_SO Curse);
            LoadedDBsHandler.StatusFieldDB.TryGetStatusEffect(TempStatusEffectID.Focused_ID.ToString(), out StatusEffect_SO Focused);


            StatusEffectInfoSO statusEffectInfoSO = ScriptableObject.CreateInstance<StatusEffectInfoSO>();
            statusEffectInfoSO._statusName = "Deaths Touch";
            statusEffectInfoSO.icon = ResourceLoader.LoadSprite("DeathsTouchIcon");
            statusEffectInfoSO._description = "Enhances Thanatos's abilties.";
            statusEffectInfoSO._applied_SE_Event = Curse._EffectInfo.AppliedSoundEvent;
            statusEffectInfoSO._removed_SE_Event = Focused._EffectInfo.RemovedSoundEvent;
            statusEffectInfoSO._updated_SE_Event = Curse._EffectInfo.UpdatedSoundEvent;

            deathsTouchSE_SO._EffectInfo = statusEffectInfoSO;

            LoadedDBsHandler.StatusFieldDB.AddNewStatusEffect(deathsTouchSE_SO);

            IntentInfoBasic ApplyDeathTouchIntent = new IntentInfoBasic();
            ApplyDeathTouchIntent.id = "ApplyDeathTouch";
            ApplyDeathTouchIntent._sprite = ResourceLoader.LoadSprite("DeathsTouchIcon");

            IntentInfoBasic RemoveDeathTouchIntent = new IntentInfoBasic();
            RemoveDeathTouchIntent.id = "RemoveDeathTouch";
            RemoveDeathTouchIntent._sprite = ResourceLoader.LoadSprite("RemDeathsTouchIcon");

            LoadedDBsHandler.IntentDB.AddNewBasicIntent("ApplyDeathTouch", ApplyDeathTouchIntent);
            LoadedDBsHandler.IntentDB.AddNewBasicIntent("RemoveDeathTouch", RemoveDeathTouchIntent);
        }

        public override void OnTriggerAttached(StatusEffect_Holder holder, IStatusEffector caller)
        {

        }

        public override void OnTriggerDettached(StatusEffect_Holder holder, IStatusEffector caller)
        {
            if (FiendishMain.ThanatosInCombat > 0)
                CombatManager._instance.AddPriorityRootAction(new EnemyDeathTouchAction());
        }
    }
}
