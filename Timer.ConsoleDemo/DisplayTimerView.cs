
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
            Console.WriteLine($"Timer started at {((ITimer)sender).StartTime.ToString("HH':'mm':'ss")}");
        }
        private void TimerEndedHandler(object sender, EventArgs e)
        {
            Console.Clear();
            Console.WriteLine($"Timer ended at {DateTime.Now}");
            DrowRpogressBar(TimeSpan.MinValue, TimeSpan.MinValue);
            Console.WriteLine($"0s. left");
        }
        private void TimerTickedHandler(object sender, EventArgs e)
        {
            var timer = (ITimer)sender;
            var wholeTimeSpann = timer.EndTime - timer.StartTime;
            var timePassedSpann = DateTime.Now - timer.StartTime;

            Console.Clear();
            Console.WriteLine($"Due: {timer.EndTime.ToString("HH':'mm':'ss")}");

            DrowRpogressBar(wholeTimeSpann, timePassedSpann);

            var timeLeft = wholeTimeSpann - timePassedSpann;
            var leftHours = (timeLeft).Hours == 0 ? "0" : (timeLeft).Hours.ToString("#");
            var leftMinutes = (timeLeft).Minutes == 0 ? "0" : (timeLeft).Minutes.ToString("#");
            var leftSeconds = (timeLeft).Seconds == 0 ? "0" : (timeLeft).Seconds.ToString("#");
            Console.WriteLine($"{leftHours}h. {leftMinutes}m. {leftSeconds}s. left");
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

        public DateTime GetEndTimeFromUser()
        {
            Console.CursorVisible = false;
            var hour = DateTime.Now.Hour;
            var minute = DateTime.Now.Minute;

            var startX = 0;
            var startY = 0;
            var digitsGab = 6;

            Console.Clear();

            ShowBordersFor5x5DigitTimeInConsole(ref startX, ref startY);

            Write1Digit();
            Write2Digit();
            Write3Digit();
            Write4Digit();

            bool isHourChosing = true;
            bool isTimeSelected = false;
            while (!isTimeSelected)
            {
                switch (GetSelectionFromUser())
                {
                    case ConsoleKey.LeftArrow:
                        isHourChosing = true;
                        break;
                    case ConsoleKey.UpArrow:
                        if (isHourChosing)
                        {
                            if (hour + 1 < 24)
                                hour++;
                            else
                                hour = 0;
                            Write1Digit();
                            Write2Digit();
                        }
                        else
                        {
                            if (minute + 1 < 60)
                                minute++;
                            else
                                minute = 0;
                            Write3Digit();
                            Write4Digit();
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        isHourChosing = false;
                        break;
                    case ConsoleKey.DownArrow:
                        if (isHourChosing)
                        {
                            if (hour - 1 >= 0)
                                hour--;
                            else
                                hour = 23;
                            Write1Digit();
                            Write2Digit();
                        }
                        else
                        {
                            if (minute - 1 >=0)
                                minute--;
                            else
                                minute = 59;
                            Write3Digit();
                            Write4Digit();
                        }
                        break;
                    case ConsoleKey.Enter:
                        isTimeSelected = true;
                        break;
                }
            }
           

            if (DateTime.Now.Hour > hour|| DateTime.Now.Hour == hour&& DateTime.Now.Minute >= minute)//timer for today or tomorrow
            {
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, hour, minute, 0);
            }
            else
            {
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, 0);
            }
            void Write1Digit() => ConsoleDigitSets.Digits5x5[hour / 10].WriteDigitToConsole(startX, startY);
            void Write2Digit() => ConsoleDigitSets.Digits5x5[hour % 10].WriteDigitToConsole(startX + digitsGab, startY);
            void Write3Digit() => ConsoleDigitSets.Digits5x5[minute / 10].WriteDigitToConsole(startX + digitsGab * 2 + 2, startY);
            void Write4Digit() => ConsoleDigitSets.Digits5x5[minute % 10].WriteDigitToConsole(startX + digitsGab * 3 + 2, startY);

        }

        private void ShowBordersFor5x5DigitTimeInConsole(ref int left, ref int top)//returns int left, int top for first digit
        {
            var upperLowerBorderChar = '=';
            var leftRightBorderChar = '|';
            var timeSeparator = '*';
            var upperLowerBorderLength = 29;
            var leftRightBorderLength = 7;

            var upperLowerBorder = new string(upperLowerBorderChar, upperLowerBorderLength);
            var freeSpaceWithLeftRightBorders = leftRightBorderChar + new string(' ', upperLowerBorderLength - 2) + leftRightBorderChar;
            var freeSpaceWithLeftRightBordersAndSeparator = leftRightBorderChar + new string(' ', (upperLowerBorderLength - 3) / 2) + timeSeparator + new string(' ', (upperLowerBorderLength - 3) / 2) + leftRightBorderChar;

            for (int i = 0; i < leftRightBorderLength; i++)
            {
                Console.CursorLeft = left;
                Console.CursorTop = top + i;
                if (i == 0 || i == leftRightBorderLength - 1)
                    Console.Write(upperLowerBorder);
                else if (i % 2 == 0)
                    Console.Write(freeSpaceWithLeftRightBordersAndSeparator);
                else
                    Console.Write(freeSpaceWithLeftRightBorders);
            }
            left += 2;
            top++;
        }

        private ConsoleKey GetSelectionFromUser()
        {
            while (true)
            {
                var consoleKeyInfo = Console.ReadKey();
                switch (consoleKeyInfo.Key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.Enter:
                        return consoleKeyInfo.Key;
                }
            }
        }

    }
}
