// See https://aka.ms/new-console-template for more information
using NAudio.Wave;

using System.Net.Http.Headers;
using System.Reflection;

using Timer.Business;
using Timer.ConsoleDemo;

Console.CursorVisible = false;

DisplayTimerWithMoves displayTimerWithMoves = new DisplayTimerWithMoves(new object());
DisplayTimerWithMovesView displayTimerWithMovesView = new DisplayTimerWithMovesView(displayTimerWithMoves, 0, 0, 50, 210);

var TimersTickValue = 0.05;

MovingTimer movingTimer = new MovingTimer();
movingTimer.Tick = TimeSpan.FromSeconds(TimersTickValue);
movingTimer.TimerEnded += PlayMysicAtTheEndOfTheTimer;
var firstEndTime = displayTimerWithMoves.GetEndTimeFromUser(1, 1);

displayTimerWithMovesView.DisplayBorders();
displayTimerWithMovesView.DisplayTime();

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
        var pathToMediaFile = @"C:\Users\Anton\Desktop\Дымок (feat. Игорь цыба) - Ицык Цыпер.m4a";
        using (var reader = new MediaFoundationReader(pathToMediaFile))
        {
            using (var waveOut = new WaveOutEvent())
            {
                var latency = 60000;
                waveOut.DesiredLatency = latency;
                waveOut.Init(reader);
                waveOut.Play();
                await Task.Delay(latency);
                waveOut.Stop();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
};