using System;
using System.Collections.Generic;
using System.Text;

namespace FiendishFools
{
    internal class SpiritualEnergyModifier : IntValueModifier
    {
        public SpiritualEnergyModifier(int silly) : base(69)
        {
            this.silly = silly;
        }

        public override int Modify(int value)
        {
            return value + this.silly;
        }

        public readonly int silly;
    }
}

