using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Helpers
{
    public static class Random_Helper
    {



        public static long GetLong(long min, long max)
        {
            Random rand = new Random();
            long result = rand.Next((Int32)(min >> 32), (Int32)(max >> 32));
            result = (result << 32);
            result = result | (long)rand.Next((Int32)min, (Int32)max);
            return result;

        }
    }
}
