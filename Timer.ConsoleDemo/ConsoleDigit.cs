
namespace Timer.ConsoleDemo
{
    public class ConsoleDigit
    {
        public string[] Digit;

        public void WriteDigitToConsole(int startX, int startY)
        {
            for (int i = 0; i < Digit.Length; i++)
            {
                Console.CursorLeft = startX;
                Console.CursorTop = startY + i;
                Console.Write(Digit[i]);
            }
        }
    }
}
