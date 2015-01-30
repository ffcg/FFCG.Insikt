using System;
using System.Linq;

namespace FFCG.Brun7
{
    public static class Randomizer
    {
        public static int[] Randomize(int start, int end)
        {
            var ints = new int[end - start + 1];
            int index = 0;
            while (start <= end)
            {
                ints[index] = start;
                index++;
                start++;
            }

            return ints.OrderBy(x => Guid.NewGuid()).ToArray();
        }
    }
}