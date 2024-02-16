
namespace Timer.Business
{
    public class ConsoleWindowSizeMonitor
    {
        public int ConsoleWindowHeight { get; set; }
        public int ConsoleWindowWidth { get; set; }
        public int TickEveryMiliseconds { get; set; }
        public bool IsRunning { get; private set; }
        public event EventHandler<EventArgs>? SizeChanged;

        public ConsoleWindowSizeMonitor()
        {
            TickEveryMiliseconds = 500;
        }

        public async void StopMonitoring()
        {
            IsRunning = false;
        }

        public async void StartMonitoring()
        { 
            ConsoleWindowHeight = Console.WindowHeight;
            ConsoleWindowWidth = Console.WindowWidth;
            IsRunning = true;
            while (IsRunning)
            {
                if (ConsoleWindowHeight!= Console.WindowHeight|| ConsoleWindowWidth!= Console.WindowWidth)
                {
                    ConsoleWindowHeight = Console.WindowHeight;
                    ConsoleWindowWidth = Console.WindowWidth;
                    SizeChanged?.Invoke(this, new EventArgs());
                }

                await Task.Delay(TickEveryMiliseconds);
            }
        }
    }
}
