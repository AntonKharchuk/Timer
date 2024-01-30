namespace Timer.Business
{
    public interface ITimer
    {
        bool IsRunning { get; } 
        DateTime StartTime { get; }
        DateTime EndTime { get; }
        TimeSpan Tick { get; }
        void Start();
        void SetEndTime(DateTime endTime);
        void SetEndTimeSpan(TimeSpan timeSpan);
        event EventHandler<EventArgs> TimerStarted;
        event EventHandler<EventArgs> TimerEnded;
        event EventHandler<EventArgs> TimerTicked;

    }
}
