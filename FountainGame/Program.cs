global using HelperMethods;

namespace FountainGame

{
    internal class Program
    {
        static void Main(string[] args)
        {
            Helpers.SetConsoleColor(ConsoleColor.White, ConsoleColor.Black);
            Console.Clear();

            Game.Run();
        }
    }
}