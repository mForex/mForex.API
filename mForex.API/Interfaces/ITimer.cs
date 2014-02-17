using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mForex.API
{
    public interface ITimer
    {
        event Action Tick;

        void Change(int dueTime, int period);
        void Change(TimeSpan dueTime, TimeSpan period);
        void Dispose();
    }
}
