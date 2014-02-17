using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
