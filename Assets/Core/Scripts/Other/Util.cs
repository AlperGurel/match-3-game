using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public static class Util
    {
        public static string PadNumber(int number)
        {
            if (number >= 1 && number <= 99)
            {
                return number.ToString("00");
            }
            else
            {
                return number.ToString();
            }
        }
    }
}