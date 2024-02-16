// See https://aka.ms/new-console-template for more information
using NAudio.Wave;

using Timer.Business;
using Timer.ConsoleDemo;

Console.CursorVisible = false;

ConsoleWindowSizeMonitor consoleWindowSizeMonitor = new ConsoleWindowSizeMonitor();
consoleWindowSizeMonitor.StartMonitoring();


DisplayTimerWithMoves displayTimerWithMoves = new DisplayTimerWithMoves(new object());
DisplayTimerWithMovesView displayTimerWithMovesView = new DisplayTimerWithMovesView(consoleWindowSizeMonitor,displayTimerWithMoves, 0, 0, Console.WindowHeight, Console.WindowWidth);

displayTimerWithMovesView.StartFolowingConsoleWindowSize();
displayTimerWithMovesView.DisplayBorders();

var TimersTickValue = 0.05;

MovingTimer movingTimer = new MovingTimer();
movingTimer.Tick = TimeSpan.FromSeconds(TimersTickValue);
movingTimer.TimerEnded += PlayMysicAtTheEndOfTheTimer;
var firstEndTime = displayTimerWithMoves.GetEndTimeFromUser(1, 1);

displayTimerWithMovesView.StartDisplaingTime();

if (firstEndTime > DateTime.Now) 
{
    movingTimer.SetEndTime(firstEndTime);
    displayTimerWithMovesView.StartDisplayingTimer(movingTimer);
    movingTimer.Start();
}


var validKeys = new List<ConsoleKey> { ConsoleKey.Spacebar, ConsoleKey.Escape };

bool isProgramEnded=false;
while (!isProgramEnded)
{
    switch (displayTimerWithMoves.GetSelectionFromUser(validKeys))
    {
        case ConsoleKey.Spacebar:
            var endTime = displayTimerWithMoves.GetEndTimeFromUser(1, 1);
            if (endTime < DateTime.Now)
            {
                break;
            }
            MovingTimer newTimer = new MovingTimer();
            newTimer.Tick = TimeSpan.FromSeconds(TimersTickValue);
            newTimer.TimerEnded += PlayMysicAtTheEndOfTheTimer;
            newTimer.SetEndTime(endTime);
            displayTimerWithMovesView.StartDisplayingTimer(newTimer);
            newTimer.Start();

            break;
        case ConsoleKey.Escape:
            isProgramEnded = true;
            break;
    }
}

async void PlayMysicAtTheEndOfTheTimer(object sender, EventArgs e)
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
};