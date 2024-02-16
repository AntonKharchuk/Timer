// See https://aka.ms/new-console-template for more information
using NAudio.Wave;

using Timer.Business;
using Timer.ConsoleDemo;
using Timer.Domain.Entities.Timers;

Console.CursorVisible = false;

ConsoleWindowSizeMonitor consoleWindowSizeMonitor = new ConsoleWindowSizeMonitor();
consoleWindowSizeMonitor.StartMonitoring();


DisplayTimerWithMoves displayTimerWithMoves = new DisplayTimerWithMoves(new object());
DisplayTimerWithMovesView displayTimerWithMovesView = new DisplayTimerWithMovesView(consoleWindowSizeMonitor,displayTimerWithMoves, 0, 0, Console.WindowHeight, Console.WindowWidth);

ConsoleWorker consoleWorker = new ConsoleWorker(consoleWindowSizeMonitor, displayTimerWithMoves, displayTimerWithMovesView);

consoleWorker.ShowInstructions();
consoleWorker.Run();
