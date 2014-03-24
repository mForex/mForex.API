using System;
using System.Threading;

namespace mForex.API.Utils
{
    class EventTimer : ITimer
    {
        public event Action Tick;

        private Timer timer;

        public EventTimer()
        {
            timer = new Timer(OnCallback, null, Timeout.Infinite, Timeout.Infinite);
        }

        private void OnCallback(object state)
        {
            var handler = Tick;

            if (handler != null)
                handler();
        }

        public void Change(int dueTime, int period)
        {
            timer.Change(dueTime, period);
        }

        public void Change(TimeSpan dueTime, TimeSpan period)
        {
            timer.Change(dueTime, period);
        }

        public void Dispose()
        {
            timer.Dispose();
        }
    }
}
