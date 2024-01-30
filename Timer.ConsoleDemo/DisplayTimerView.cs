
using Timer.Business;

namespace Timer.ConsoleDemo
{
    public class DisplayTimerView : IDisposable
    {
        private ITimer _timer;
        public bool IsShown { get; private set; }

        public DisplayTimerView(ITimer timer)
        {
            _timer = timer;
            IsShown = false;
        }

        public void ShowTimerView()
        {
            if (!IsShown)
            {
                _timer.TimerStarted += TimerStartedHandler;
                _timer.TimerTicked += TimerTickedHandler;
                _timer.TimerEnded += TimerEndedHandler;
                IsShown = true;
            }
            else
            {
                Console.WriteLine("TimerView is allready shown");
            }
            
        }
        public void HideTimerView()
        {
            Dispose();
            IsShown = false;
        }

        private void TimerStartedHandler(object sender, EventArgs e)
        {
            Console.WriteLine($"Timer started at {((ITimer)sender).StartTime.TimeOfDay}");
        }
        private void TimerEndedHandler(object sender, EventArgs e)
        {
            Console.Clear();
            Console.WriteLine($"Timer ended at {DateTime.Now}");
            DrowRpogressBar(TimeSpan.MinValue,TimeSpan.MinValue);
            Console.WriteLine($"0s. left");

        }
        private void TimerTickedHandler(object sender, EventArgs e)
        {
            var timer = (ITimer)sender;
            var wholeTimeSpann= timer.EndTime - timer.StartTime;
            var timePassedSpann = DateTime.Now - timer.StartTime;

            Console.Clear();
            Console.WriteLine($"Due: {timer.EndTime.TimeOfDay}");
            DrowRpogressBar(wholeTimeSpann, timePassedSpann);
            Console.WriteLine($"{(wholeTimeSpann - timePassedSpann).TotalSeconds}s. left");
        }

        private void DrowRpogressBar(TimeSpan wholeTimeSpann, TimeSpan timePassedSpann)
        {
            var borderChar = '=';
            var starBarChar = '|';
            var barChar = '+';
            var borderLength = 40;
            var maxBarLength = 38;


            int progressLength = (int)(timePassedSpann / wholeTimeSpann * maxBarLength);

            var border = new string(borderChar, borderLength);
            Console.WriteLine(border);
            if (maxBarLength - progressLength > 0)
            {
                Console.WriteLine($"{starBarChar}{new string(barChar, progressLength)}{new string(' ', maxBarLength - progressLength)}{starBarChar}");
                Console.WriteLine($"{starBarChar}{new string(barChar, progressLength)}{new string(' ', maxBarLength - progressLength)}{starBarChar}");
            }
            else
            {
                Console.WriteLine($"{starBarChar}{new string(barChar, maxBarLength)}{starBarChar}");
                Console.WriteLine($"{starBarChar}{new string(barChar, maxBarLength)}{starBarChar}");
            }

            Console.WriteLine(border);
        }

        public void Dispose()
        {
            _timer.TimerStarted -= TimerStartedHandler;
            _timer.TimerTicked -= TimerTickedHandler;
            _timer.TimerTicked -= TimerEndedHandler;
        }
    }
}
