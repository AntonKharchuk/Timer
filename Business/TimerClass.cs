
using System.Reflection.Metadata.Ecma335;

namespace Timer.Business
{
    public class TimerClass : ITimer
    {
        public bool IsRunning { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public TimeSpan Tick { get; set; }

        public event EventHandler<EventArgs> TimerStarted;
        public event EventHandler<EventArgs> TimerEnded;
        public event EventHandler<EventArgs> TimerTicked;

        public TimerClass()
        {
            Tick = TimeSpan.FromSeconds(1);
            IsRunning= false;
        }
        public void Start()
        {
            IsRunning = true;
            StartTicks();
            TimerStarted?.Invoke(this, new EventArgs());
        }
        private async void StartTicks()
        {
            while (IsRunning) {
                if (EndTime<DateTime.Now)
                {
                    IsRunning = false;

                    TimerEnded?.Invoke(this, new EventArgs());
                    return;
                }
                else
                {
                    await Task.Delay(Tick);
                    TimerTicked?.Invoke(this, new EventArgs());
                }
            }

        }
       

        public void SetEndTime(DateTime endTime)
        {
            if (endTime >= DateTime.Now)
                EndTime = endTime;
            else 
                throw new ArgumentException(nameof(endTime));

            StartTime = DateTime.Now;
        }

        public void SetEndTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan > TimeSpan.Zero)
                EndTime = DateTime.Now.Add(timeSpan);
            else
                throw new ArgumentException(nameof(timeSpan));
            StartTime = DateTime.Now;
        }

        
    }
}
