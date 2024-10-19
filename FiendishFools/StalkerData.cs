using System;
using System.Collections.Generic;
using System.Text;

namespace FiendishFools
{
    public class StalkerData
    {
        public int StalkerID;

        public int ObessionID;

        public bool HasObession => ObessionCheck();

        public StalkerData(int S, int B)
        {
            StalkerID = S;
            ObessionID = B;
        }

        public bool ObessionCheck()
        {
            return ObessionID != -1;
        }

        public void ResetObessionID()
        {
            ObessionID = -1;
        }
    }
}
