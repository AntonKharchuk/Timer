
namespace Timer.ConsoleDemo
{
    public class DisplayTimerWithMoves
    {
        public void ShowTimerInConsole(int left, int top, int hour, int minute)
        {
            var startX = left;
            var startY = top;
            var digitsGab = 6;

            ShowBordersFor5x5DigitTimeInConsole(ref startX, ref startY);

            ConsoleDigitSets.Digits5x5[hour / 10].WriteDigitToConsole(startX, startY);
            ConsoleDigitSets.Digits5x5[hour % 10].WriteDigitToConsole(startX + digitsGab, startY);
            ConsoleDigitSets.Digits5x5[minute / 10].WriteDigitToConsole(startX + digitsGab * 2 + 2, startY);
            ConsoleDigitSets.Digits5x5[minute % 10].WriteDigitToConsole(startX + digitsGab * 3 + 2, startY);

            Console.CursorVisible = false;
        }
        public void ClearBordersFor5x5DigitTimeInConsole(int left, int top)
        {
            var upperLowerBorderLength = 29;
            var leftRightBorderLength = 7;
            var rowOfSpaces = new string(' ', upperLowerBorderLength);

            for (int i = 0; i < leftRightBorderLength; i++)
            {
                Console.CursorLeft = left;
                Console.CursorTop = top + i;
                Console.Write(rowOfSpaces);
            }
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
