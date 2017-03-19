using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Saper
{
    public class Stopwatch
    {
        int m_elapsed;
        public TimeSpan Elapsed { get => TimeSpan.FromSeconds(m_elapsed); }

        DispatcherTimer m_dispatcherTimer { get; set; }

        public Stopwatch()
        {
            m_dispatcherTimer = new DispatcherTimer();
            m_dispatcherTimer.Tick += dispatcherTimer_Tick;
            m_dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
        }

        public void Start()
        {
            if (!m_dispatcherTimer.IsEnabled) m_dispatcherTimer.Start();
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
