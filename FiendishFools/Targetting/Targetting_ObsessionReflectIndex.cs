using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FiendishFools.Targetting
{
    public class Targetting_ObsessionReflectIndex : BaseCombatTargettingSO
    {

        public bool getAllies;

        public int[] slotPointerDirections;

        public bool allSelfSlots;

        public override bool AreTargetAllies => getAllies;

        public override bool AreTargetSlots => true;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {

            TargetSlotInfo StalkerChar = slots.GetCharacterTargetSlot(casterSlotID, 0);
            if (!StalkerChar.HasUnit || !ExtraUtils.TryGetCurrentObsession(StalkerChar.Unit.ID, out int ObsessionID))
                return new TargetSlotInfo[0];

            CharacterCombat ObsessionChar = CombatManager._instance._stats.TryGetCharacterOnField(ObsessionID);

            if (ObsessionChar == null)
                return new TargetSlotInfo[0];

            return GetTargetsIndex(slots, ObsessionChar.SlotID, true);
        }

        public TargetSlotInfo[] GetTargetsIndex(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            List<TargetSlotInfo> list = new List<TargetSlotInfo>();
            for (int i = 0; i < slotPointerDirections.Length; i++)
            {
                if (!getAllies && slotPointerDirections[i] == 0)
                {
                    list.AddRange(slots.GetFrontOpponentSlotTargets(casterSlotID, isCasterCharacter));
                }
                else if (allSelfSlots && getAllies && slotPointerDirections[i] == 0)
                {
                    list.AddRange(slots.GetAllSelfSlots(casterSlotID, isCasterCharacter));
                }
                else if (getAllies)
                {
                    TargetSlotInfo allySlotTarget = slots.GetAllySlotTarget(casterSlotID, slotPointerDirections[i], isCasterCharacter);
                    if (allySlotTarget != null)
                    {
                        list.Add(allySlotTarget);
                    }
                }
                else
                {
                    TargetSlotInfo allySlotTarget = slots.GetOpponentSlotTarget(casterSlotID, slotPointerDirections[i], isCasterCharacter);
                    if (allySlotTarget != null)
                    {
                        list.Add(allySlotTarget);
                    }
                }
            }

            return list.ToArray();
        }
    }
}
