
namespace Timer.Domain.Entities.Timers
{
    public class MovingTimer:TimerClass
    {
        public int Top { get; set; }
        public int Left { get; set; }

        public int TopSpeed { get; set; }
        public int LeftSpeed { get; set; }

        public int Height { get; set; }
        public int Width { get; set; }

        public MovingTimer()
        {
            Top = 1; Left = 1; TopSpeed =1; LeftSpeed =1; Height = 7; Width = 29;
        }
    }
}
