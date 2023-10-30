using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Match3
{
    public static class Waiter
    {
        public static async Task WaitForSeconds(float seconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));
        }

        public static async Task WaitForUpdate()
        {
            await Task.Yield();
        }
    }
}