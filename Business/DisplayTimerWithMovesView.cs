
using Timer.Business;
using Timer.Domain.Entities.Timers;

namespace Timer.ConsoleDemo
{
    public class DisplayTimerWithMovesView
    {
        private ConsoleWindowSizeMonitor _windowSizeMonitor;
        private DisplayTimerWithMoves _displayTimer;
        private Obstacle _timerObsticle;

        public bool IsTimerShown { get; set; }

        public int FieldLeft { get; set; }
        public int FieldTop { get; set; }
        public int FieldHeight { get; set; }
        public int FieldLength { get; set; }


        public DisplayTimerWithMovesView(ConsoleWindowSizeMonitor windowSizeMonitor, DisplayTimerWithMoves displayTimer, int top, int left, int height, int length)
        {
            IsTimerShown = false;
            _windowSizeMonitor = windowSizeMonitor;
            _displayTimer = displayTimer;
            FieldLeft = top;
            FieldTop = left;
            FieldHeight = height;
            FieldLength = length;
        }

        public void StartFolowingConsoleWindowSize()
        {
            _windowSizeMonitor.SizeChanged += ConsoleSizeChangeHandler;
        }


        private void ConsoleSizeChangeHandler(object sender, EventArgs e)
        {
            var sizeMonitor = (ConsoleWindowSizeMonitor)sender;
            FieldHeight = sizeMonitor.ConsoleWindowHeight;
            FieldLength = sizeMonitor.ConsoleWindowWidth;
            Console.Clear();
            DisplayBorders();
        }

        public void DisplayBorders()
        {

            var upperLowerBorderChar = '=';
            var leftRightBorderChar = '|';
            var upperLowerBorder = new string(upperLowerBorderChar, FieldLength);
            var freeSpaceWithLeftRightBorders = leftRightBorderChar + new string(' ', FieldLength - 2) + leftRightBorderChar;

            lock (_displayTimer.ConsoleLock)
            {
                Console.CursorLeft = FieldLeft;
                Console.CursorTop = FieldTop;
                Console.Write(upperLowerBorder);

                for (int i = 0; i < FieldHeight - 2; i++)
                {
                    Console.CursorLeft = FieldLeft;
                    Console.CursorTop = FieldTop + 1 + i;
                    Console.Write(freeSpaceWithLeftRightBorders);
                }
                Console.CursorLeft = FieldLeft;
                Console.CursorTop = FieldTop + FieldHeight - 1;
                Console.Write(upperLowerBorder);
            }
        }

        public void StartDisplayingTimer(MovingTimer movingTimer)
        {
            movingTimer.TimerTicked += TimerTickedHandler;
        }
        private void TimerTickedHandler(object sender, EventArgs e)
        {
            var timer = (MovingTimer)sender;

            _displayTimer.ClearBordersFor5x5DigitTimeInConsole(timer.Left, timer.Top);
            MoveTimer(timer);
            var timeLeft = timer.EndTime - DateTime.Now;

            try
            {
                if (timeLeft.Hours > 0)
                    _displayTimer.ShowTimerInConsole(timer.Left, timer.Top, timeLeft.Hours, timeLeft.Minutes);
                else
                    _displayTimer.ShowTimerInConsole(timer.Left, timer.Top, timeLeft.Minutes, timeLeft.Seconds);
            }
            
            catch 
            {
                timer.Top = 2;
                timer.Left = 2;
                if (timeLeft.Hours > 0)
                    _displayTimer.ShowTimerInConsole(timer.Left, timer.Top, timeLeft.Hours, timeLeft.Minutes);
                else
                    _displayTimer.ShowTimerInConsole(timer.Left, timer.Top, timeLeft.Minutes, timeLeft.Seconds);
            }
        }

        public async void StartDisplaingTime()
        {
            if (_timerObsticle is not null)
            {
                return;
            }
            int timeHeight = 5;
            int timeLenght = 28;
            int timeTop = (FieldTop + FieldHeight - timeHeight) / 2;
            int timeLeft = (FieldLeft + FieldLength - timeLenght) / 2;

            _timerObsticle = new Obstacle(timeLeft + 1, timeTop + 1, timeLenght, timeHeight);

            IsTimerShown = true; 
            while (IsTimerShown)
            {
                timeTop = (FieldTop + FieldHeight - timeHeight) / 2;
                timeLeft = (FieldLeft + FieldLength - timeLenght) / 2;

                _timerObsticle.Left = timeLeft + 1;
                _timerObsticle.Top = timeTop + 1;

                _displayTimer.ShowTimerInConsole(timeLeft, timeTop, DateTime.Now.Hour, DateTime.Now.Minute);
                await Task.Delay(50);
            }
        }

        private void MoveTimer(MovingTimer movingTimer)
        {
            var left = movingTimer.Left;
            var top = movingTimer.Top;
            var right = movingTimer.Left + movingTimer.Width;
            var down = movingTimer.Top + movingTimer.Height;


            if (right >= _windowSizeMonitor.ConsoleWindowWidth || down >= _windowSizeMonitor.ConsoleWindowHeight)
            {
                movingTimer.Top = 2;
                movingTimer.Left = 2;
            }

            bool isLeftSpeedRandomlyMyltiplied = false;
            bool isTopSpeedRandomlyMyltiplied = false;
            Random random = new Random();
            if (random.Next(100) == 5)
                isLeftSpeedRandomlyMyltiplied = true;
            if (random.Next(100) == 5)
                isTopSpeedRandomlyMyltiplied = true;

            if (isLeftSpeedRandomlyMyltiplied)
                movingTimer.LeftSpeed *= 2;
            if (isTopSpeedRandomlyMyltiplied)
                movingTimer.TopSpeed *= 2;

            //move timer
            left += movingTimer.LeftSpeed;
            top += movingTimer.TopSpeed;
            right += movingTimer.LeftSpeed;
            down += movingTimer.TopSpeed;
            //check if movement is correct
            if (top <= FieldTop)
            {
                movingTimer.TopSpeed = -movingTimer.TopSpeed;
            }
            if (left <= FieldLeft)
            {
                movingTimer.LeftSpeed = -movingTimer.LeftSpeed;
            }

            if (down >= FieldTop + FieldHeight)
            {
                movingTimer.TopSpeed = -movingTimer.TopSpeed;
            }
            if (right >= FieldLeft + FieldLength)
            {
                movingTimer.LeftSpeed = -movingTimer.LeftSpeed;
            }

            if (IsTimerShown == true)
            {
                BounceFromObsticle(movingTimer, _timerObsticle);
            }

            movingTimer.Top += movingTimer.TopSpeed;
            movingTimer.Left += movingTimer.LeftSpeed;

            if (isLeftSpeedRandomlyMyltiplied)
                movingTimer.LeftSpeed /= 2;
            if (isTopSpeedRandomlyMyltiplied)
                movingTimer.TopSpeed /= 2;
        }


        private void BounceFromObsticle(MovingTimer movingTimer, Obstacle obsticle)
        {
            var left = movingTimer.Left;
            var top = movingTimer.Top;
            var right = movingTimer.Left + movingTimer.Width;
            var down = movingTimer.Top + movingTimer.Height;

            //move timer
            left += movingTimer.LeftSpeed;
            top += movingTimer.TopSpeed;
            right += movingTimer.LeftSpeed;
            down += movingTimer.TopSpeed;
            //check if movement is correct
            if (top < obsticle.Top + obsticle.Height && down > obsticle.Top && right >= obsticle.Left && right <= obsticle.Left + obsticle.Width)
            {
                movingTimer.LeftSpeed = -movingTimer.LeftSpeed;
            }
            else if (top < obsticle.Top + obsticle.Height && down > obsticle.Top && left <= obsticle.Left + obsticle.Width && left >= obsticle.Left)
            {
                movingTimer.LeftSpeed = -movingTimer.LeftSpeed;
            }
            else if (left < obsticle.Left + obsticle.Width && right > obsticle.Left && down >= obsticle.Top && down <= obsticle.Top + obsticle.Height)
            {
                movingTimer.TopSpeed = -movingTimer.TopSpeed;
            }
            else if (left < obsticle.Left + obsticle.Width && right > obsticle.Left && top <= obsticle.Top + obsticle.Height && top >= obsticle.Top)
            {
                movingTimer.TopSpeed = -movingTimer.TopSpeed;
            }
        }
        public class Obstacle
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Width { get; }
            public int Height { get; }

            public Obstacle(int left, int top, int width, int height)
            {
                Left = left;
                Top = top;
                Width = width;
                Height = height;
            }
        }

        public void StopDisplayingTimer(MovingTimer movingTimer)
        {
            movingTimer.TimerTicked -= TimerTickedHandler;
        }
    }
}
