using System;

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
