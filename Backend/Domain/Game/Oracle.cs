using System;
using Backend.Helpers;

namespace Backend.Domain.Game
{
    public class Oracle
    {
        public Oracle()
        {
        }

        public int Id { get; set; }
        public byte Next { get; set; }

        public byte[] ItemOrder { get; set; }
        public int NumElements { get; set; }

        public Oracle(int numElements)
        {
            Next = 0;
            NumElements = numElements;
            ItemOrder = new byte[numElements];
            for (byte i = 0; i < numElements; i++)
            {
                ItemOrder[i] = i;
            }

            var rnd = new Random();
            rnd.Shuffle(ItemOrder);
        }

        public (byte, bool) GetNextIndex()
        {
            if (Next >= NumElements)
            {
                return (0, false);
            }

            return (ItemOrder[Next++], true);
        }
    }
}