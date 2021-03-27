
namespace Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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
        static int GetInteger(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);

        }
        public static string GetString(int minlength, int maxlength, bool allowNumerics)
        {
                int length = Random_Helper.GetInteger(minlength, maxlength);
            Random random = new Random();

             string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if(allowNumerics)
            {
                chars = chars + "0123456789";
            }
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
