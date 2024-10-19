using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiendishFools.Effects
{
    public class ConsumeColorManaButColorManaEffect : EffectSO
    {
        public ManaColorSO exceptionMana = Pigments.Red;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            JumpAnimationInformation jumpInfo = stats.GenerateUnitJumpInformation(caster.ID, caster.IsUnitCharacter);
            string manaConsumedSound = stats.audioController.manaConsumedSound;
            exitAmount = ConsumeManaButColor(stats.MainManaBar, exceptionMana, entryVariable, jumpInfo, manaConsumedSound);
            return exitAmount >= entryVariable;
        }

        public int ConsumeManaButColor(ManaBar bar, ManaColorSO mana, int amount, JumpAnimationInformation jumpInfo = null, string manaSound = "")
        {
            List<int> list = new List<int>();
            ManaBarSlot[] manaBarSlots = bar.ManaBarSlots;

            for (int i = 0; i < manaBarSlots.Length && list.Count < amount; i++)
            {
                if (!manaBarSlots[i].IsEmpty && manaBarSlots[i].ManaColor != mana)
                {
                    manaBarSlots[i].ConsumeMana();
                    list.Add(manaBarSlots[i].SlotIndex);
                }
            }

            if (list.Count > 0)
                CombatManager.Instance.AddUIAction(new ConsumeVariousManaSlotsUIAction(bar.ID, list.ToArray(), jumpInfo, manaSound));

            if (list.Count > 0)
                bar.FillManaBarWithStoredSlots();

            return list.Count;
        }
    }
}
