
using NAudio.Wave;

using Timer.Business;
using Timer.Domain.Entities.Timers;

namespace Timer.ConsoleDemo
{
    public class ConsoleWorker
    {
        private ConsoleWindowSizeMonitor _consoleWindowSizeMonitor;
        private DisplayTimerWithMoves _displayTimerWithMoves;
        private DisplayTimerWithMovesView _displayTimerWithMovesView;

        public ConsoleWorker(ConsoleWindowSizeMonitor consoleWindowSizeMonitor, DisplayTimerWithMoves displayTimerWithMoves,DisplayTimerWithMovesView displayTimerWithMovesView)
        {
            _consoleWindowSizeMonitor = consoleWindowSizeMonitor;
            _displayTimerWithMoves = displayTimerWithMoves;
            _displayTimerWithMovesView = displayTimerWithMovesView;
        }

        public void ShowInstructions() 
        {
            Console.WriteLine("Timer Manual");
            Console.WriteLine();
            Console.WriteLine("Timer:");
            Console.WriteLine("Use 'SpaceBarr' to Start Crating / Exit Crating new timer");
            Console.WriteLine("Use 'Arrows' to Set the timer");
            Console.WriteLine("Use 'Enter' to Finish Crating the timer");
            Console.WriteLine("Use 'Any Key' to Stop The Alarm");
            Console.WriteLine();
            Console.WriteLine("Program:");
            Console.WriteLine("Use 'Escape' to Close the Program");
            Console.WriteLine();
            Console.WriteLine("Press key to continue...");
            Console.ReadKey();
        }
       
        public void Run() 
        {
            _displayTimerWithMovesView.StartFolowingConsoleWindowSize();
            _displayTimerWithMovesView.DisplayBorders();
            _displayTimerWithMovesView.StartDisplaingTime();

            var TimersTickValue = 0.05;

            var validKeys = new List<ConsoleKey> { ConsoleKey.Spacebar, ConsoleKey.Escape };

            bool isProgramEnded = false;
            while (!isProgramEnded)
            {
                switch (_displayTimerWithMoves.GetSelectionFromUser(validKeys))
                {
                    case ConsoleKey.Spacebar:
                        var endTime = _displayTimerWithMoves.GetEndTimeFromUser(1, 1);
                        if (endTime < DateTime.Now)
                        {
                            break;
                        }
                        MovingTimer newTimer = new MovingTimer();
                        newTimer.Tick = TimeSpan.FromSeconds(TimersTickValue);
                        newTimer.TimerEnded += PlayMusicAtTheEndOfTheTimer;
                        newTimer.SetEndTime(endTime);
                        _displayTimerWithMovesView.StartDisplayingTimer(newTimer);
                        newTimer.Start();

                        break;
                    case ConsoleKey.Escape:
                        isProgramEnded = true;
                        break;
                }
            }
        }
        async void PlayMusicAtTheEndOfTheTimer(object sender, EventArgs e)
        {
            try
            {
                var mediaFileName = @"Mother_Mother-Verbatim.mp3";

                // Get the full path of the file
                var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, mediaFileName);

                using (var reader = new MediaFoundationReader(fullPath))
                {
                    using (var waveOut = new WaveOutEvent())
                    {
                        var latency = 60000;
                        waveOut.DesiredLatency = latency;
                        waveOut.Init(reader);
                        waveOut.Play();
                        Console.ReadKey();
                        waveOut.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}
