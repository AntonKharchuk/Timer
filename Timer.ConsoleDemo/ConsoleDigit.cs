
namespace Timer.ConsoleDemo
{
    public class ConsoleDigit
    {
        public string[] Digit;

        public void WriteDigitToConsole(int startX, int startY)
        {
            for (int i = 0; i < Digit.Length; i++)
            {
                if (Console.WindowWidth<startX||Console.WindowHeight < startY)
                {
                    throw new IncorrectConsoleSizeExeption();
                }
                Console.CursorLeft = startX;
                Console.CursorTop = startY + i;
                Console.Write(Digit[i]);
            }
        }
    }
}
