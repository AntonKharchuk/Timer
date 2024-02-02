// See https://aka.ms/new-console-template for more information
using NAudio.Wave;

using System.Net.Http.Headers;
using System.Reflection;

using Timer.Business;
using Timer.ConsoleDemo;

Console.CursorVisible = false;
MovingTimer movingTimer = new MovingTimer();
movingTimer.Tick = TimeSpan.FromSeconds(0.05);
movingTimer.TimerEnded += async (o, e) =>
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

DisplayTimerView timerView = new DisplayTimerView(new TimerClass());
DisplayTimerWithMoves displayTimerWithMoves = new DisplayTimerWithMoves();
DisplayTimerWithMovesView displayTimerWithMovesView = new DisplayTimerWithMovesView(displayTimerWithMoves,0,0,50,210);
displayTimerWithMovesView.StartDisplayingTimer(movingTimer);

movingTimer.TimerEnded += TimerEndedEnotherTurnAskHandler;

movingTimer.SetEndTime(timerView.GetEndTimeFromUser());
displayTimerWithMovesView.DisplayBorders();
movingTimer.Start();

while (true)
{

}
void TimerEndedEnotherTurnAskHandler(object sender, EventArgs e)
{
    movingTimer.SetEndTime(timerView.GetEndTimeFromUser());
    displayTimerWithMovesView.DisplayBorders();
    movingTimer.Start();
}