using System;
using System.Windows.Threading;

namespace Saper
{
    public class Stopwatch
    {
        DispatcherTimer m_dispatcherTimer;

        int m_elapsed;
        public TimeSpan Elapsed => TimeSpan.FromSeconds(m_elapsed);

        public bool IsEnabled => m_dispatcherTimer.IsEnabled;

        public Stopwatch()
        {
            m_dispatcherTimer = new DispatcherTimer();
            m_dispatcherTimer.Tick += dispatcherTimer_Tick;
            m_dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
        }

        public void Start()
        {
            if (!m_dispatcherTimer.IsEnabled)
            {
                m_elapsed = 0;
                m_dispatcherTimer.Start();
            }
        }

        public void Stop()
        {
            m_dispatcherTimer.Stop();
        }        

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            ++m_elapsed;
            IntervalElapsed?.Invoke(this, null);
        }

        public event EventHandler IntervalElapsed;
    }
}
