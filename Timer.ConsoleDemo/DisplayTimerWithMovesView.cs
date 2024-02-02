
using Timer.Business;

namespace Timer.ConsoleDemo
{
    public class DisplayTimerWithMovesView
    {
        private DisplayTimerWithMoves _displayTimer;

        public int FieldLeft { get; set; }
        public int FieldTop { get; set; }
        public int FieldHeight { get; set; }
        public int FieldLength { get; set; }

        public DisplayTimerWithMovesView(DisplayTimerWithMoves displayTimer, int top, int left, int height, int length)
        {
            _displayTimer = displayTimer;
            FieldLeft = top;
            FieldTop = left;
            FieldHeight = height;
            FieldLength = length;
        }
        public void DisplayBorders()
        {

            var upperLowerBorderChar = '=';
            var leftRightBorderChar = '|';
            var upperLowerBorder = new string(upperLowerBorderChar, FieldLength);
            var freeSpaceWithLeftRightBorders = leftRightBorderChar + new string(' ', FieldLength - 2) + leftRightBorderChar;

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

        private int TimeTop = 20;
        private int TimeLeft = 89;

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
            _displayTimer.ShowTimerInConsole(timer.Left, timer.Top, timeLeft.Minutes, timeLeft.Seconds);
            _displayTimer.ShowTimerInConsole(TimeLeft, TimeTop, DateTime.Now.Hour, DateTime.Now.Minute);

        }

        private void MoveTimer(MovingTimer movingTimer)
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

            BounceFromObsticle(movingTimer, new Obsticle(TimeLeft+1, TimeTop+1, 28, 5));

            movingTimer.Top += movingTimer.TopSpeed;
            movingTimer.Left += movingTimer.LeftSpeed;
        }

        private void BounceFromObsticle(MovingTimer movingTimer, Obsticle obsticle)
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
            else if (left < obsticle.Left + obsticle.Width && right >obsticle.Left && down >= obsticle.Top && down <= obsticle.Top + obsticle.Height)
            {
                movingTimer.TopSpeed = -movingTimer.TopSpeed;
            }
            else if (left < obsticle.Left + obsticle.Width && right > obsticle.Left && top <= obsticle.Top + obsticle.Height && top >= obsticle.Top)
            {
                movingTimer.TopSpeed = -movingTimer.TopSpeed;
            }
        }

        private record Obsticle(int Left, int Top, int Width, int Height);

        public void StopDisplayingTimer(MovingTimer movingTimer)
        {
            movingTimer.TimerTicked -= TimerTickedHandler;
        }
    }
}
