// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;

using Timer.Business;
using Timer.ConsoleDemo;

Console.WriteLine("Hello, World!");


TimerClass timer = new TimerClass();
//timer.TimerStarted += (o, e) => {
//    Console.WriteLine($"Timer started at {((TimerClass)o).StartTime.TimeOfDay} with Tick {((TimerClass)o).Tick.TotalSeconds}s. ");
//};
//var countDoun = 0;
//timer.TimerTicked += (o, e) => {
//    Console.WriteLine($"Tick {++countDoun}");
//};
//timer.TimerEnded += (o, e) => {
//    Console.WriteLine($"Timer ended at {((TimerClass)o).EndTime.TimeOfDay}");

//};

DisplayTimerView timerView = new DisplayTimerView(timer);
timerView.ShowTimerView();  

timer.SetEndTimeSpan(TimeSpan.FromSeconds(10));
timer.Start();

Console.ReadLine();

