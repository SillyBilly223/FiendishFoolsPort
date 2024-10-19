using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using static FiendishFools.ExtraUtils;

namespace FiendishFools
{
    public static class ExtraUtils
    {
        #region Stalker

        public static Dictionary<int, StalkerData> StalkedChars = new Dictionary<int, StalkerData>();

        public static void AddObession(int StalkerID, int ObessionID)
        {
            if (!StalkedChars.TryGetValue(StalkerID, out StalkerData Data))
            { AddStalker(StalkerID, ObessionID); return; }

            Data.ObessionID = ObessionID;
        }

        public static void AddStalker(int StalkerID, int ObessionID = -1)
        {
            if (StalkedChars.ContainsKey(StalkerID)) return;
            StalkedChars.Add(StalkerID, new StalkerData(StalkerID, ObessionID));
        }

        public static void RemoveStalker(int StalkerID)
        {
            StalkedChars.Remove(StalkerID);
        }

        public static bool TryRemoveObsession(int ObessionID, out int StalkerID)
        {
            StalkerID = -1;
            foreach (StalkerData Data in StalkedChars.Values)
            {
                if (Data.ObessionID == ObessionID)
                {
                    Data.ResetObessionID();
                    StalkerID = Data.StalkerID;
                    return true;
                }
            }
            return false;
        }

        public static bool ContainsObsession(int StalkerID)
        {
            if (StalkedChars.TryGetValue(StalkerID, out StalkerData Data))
                return Data.HasObession;
            return false;
        }

        public static bool UnitIsObsession(int UnitID)
        {
            foreach (StalkerData Data in StalkedChars.Values)
                if (Data.ObessionID == UnitID) return true;
            return false;
        }


        public static bool TryGetCurrentObsession(int StalkerID, out int ObessionID)
        {
            ObessionID = -1;
            if (StalkedChars.TryGetValue(StalkerID, out StalkerData Data))
            { ObessionID = Data.ObessionID; return Data.HasObession; }
            return false;
        }

        public static bool TryGetCharacterFromField(int ID, out CharacterCombat character)
        {
            character = CombatManager._instance._stats.TryGetCharacterOnField(ID);
            return character != null;
        }

        #endregion Stalker

        #region hooks

        public static void InitializeCombat(Action<CombatManager> orig, CombatManager self)
        {
            StalkedChars.Clear();
            orig(self);
        }

        #endregion hooks

        #region misc

        public static EffectSO AutoSetFieldEffectEffects(EffectSO effect, string fieldEffectID)
        {
            LoadedDBsHandler.StatusFieldDB.TryGetFieldEffect(fieldEffectID, out FieldEffect_SO fieldEffect);
            foreach (FieldInfo field in effect.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
                if (field.FieldType == typeof(FieldEffect_SO))
                    field.SetValue(effect, fieldEffect);
            return effect;
        }

        public static EffectSO AutoSetStatusEffectEffects(EffectSO effect, string statusEffectID)
        {
            LoadedDBsHandler.StatusFieldDB.TryGetStatusEffect(statusEffectID, out StatusEffect_SO statusEffect);
            foreach (FieldInfo field in effect.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
                if (field.FieldType == typeof(StatusEffect_SO))
                    field.SetValue(effect, statusEffect);
            return effect;
        }

        public static EffectSO AutoSetEffects(EffectSO effect, Type type, object obj)
        {
            foreach (FieldInfo field in effect.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
                if (field.FieldType == type)
                    field.SetValue(effect, obj);
            return effect;
        }

        public static T AutoSetObject<T>(T instobj, Type type, object obj)
        {
            foreach (FieldInfo field in instobj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
                if (field.FieldType == type)
                    field.SetValue(instobj, obj);
            return instobj;
        }

        #endregion misc

        public static void SetUp()
        {
            IDetour detour = new Hook(typeof(CombatManager).GetMethod("InitializeCombat", (BindingFlags)(-1)), typeof(ExtraUtils).GetMethod("InitializeCombat", (BindingFlags)(-1)));
        }
    }
}
