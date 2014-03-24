using System;

namespace mForex.API
{
    public static class EventHandler
    {
        public static void RiseSafely(Action action)
        {
            try
            {
                action();
            }
            catch { }
        }
    }
}
