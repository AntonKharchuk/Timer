
namespace Timer.ConsoleDemo
{
    internal class IncorrectConsoleSizeExeption : Exception
    {
        public IncorrectConsoleSizeExeption()
        {
        }

        public IncorrectConsoleSizeExeption(string? message) : base(message)
        {
        }
    }
}
