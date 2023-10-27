using FountainGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HelperMethods
{
    public static class Helpers
    {
        // This class holds some reusable helper methods for the whole game

        public static bool YNInputCheck()
        {
            do
            {
                string choice = Console.ReadKey().KeyChar.ToString().ToLower();

                if (choice == "y")
                    return true;
                else if (choice == "n")
                    return false;
                else
                {
                    Console.Write("\nSorry we couldn't interpret your input, please provide it again, y - for 'yes', n - for 'no': ");
                    continue;
                }
            } while (true);
        }

        public static bool Choice(int first, int last, out int chosenN)
        {
            Console.WriteLine("Your choice: ");
            bool validCh = Int32.TryParse(Console.ReadKey().KeyChar.ToString(), out chosenN);
            Console.WriteLine();

            if (!validCh)
            {
                Console.WriteLine("Please press a button from provided list:\n ");
                return false;
            }
            else if (chosenN < first || chosenN > last)
            {
                Console.WriteLine($"Please press a button between {first} and {last}:\n ");
                return false;
            }
            else
                return true;
        }

        public static void ClearOneCharacter()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', 1));
            Console.SetCursorPosition(0, Console.CursorTop);
        }

        public static void GetConsoleColor(out ConsoleColor background, out ConsoleColor foreground)
        {
            background = Console.BackgroundColor;
            foreground = Console.ForegroundColor;
        }
        public static void SetConsoleColor(ConsoleColor background, ConsoleColor foreground = ConsoleColor.Black)
        {
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
        }

        // this one is used to just fill entire line with the currently set console colors
        public static void FillEntireLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop);
        }

        // Using this method becomes easier to just add centered text and avoid rewriting the padding methods
        public static void WriteLineCenteredText(string text, int centerMaxLength = -1, bool fillLine = true, char surroundWith = ' ')
        {
            if (centerMaxLength == -1)
                centerMaxLength = Console.WindowWidth;

            if (fillLine)
                FillEntireLine();
            Console.WriteLine(text.PadLeft((centerMaxLength + text.Length) / 2, surroundWith).PadRight(centerMaxLength, surroundWith));

            // The same or similar can be achieved with the code below
            // originally it was written like below, but the same can be achieved with the method above and the method above can be used in more places + it's shorter
            /*
            int leftMargin = (Console.WindowWidth - text.Length) / 2;
            Console.SetCursorPosition(leftMargin, Console.CursorTop);
            Console.WriteLine(text);
            */
        }


        // Same but for Console.Write method instead of WriteLine. Should I have placed them in the same method and make an extra parameter to write with Line or no?
        public static void WriteCenteredText(string text, int centerMaxLength = -1, bool fillLine = true, char surroundWith = ' ')
        {
            if (centerMaxLength == -1)
                centerMaxLength = Console.WindowWidth;

            if (fillLine)
                FillEntireLine();
            Console.Write(text.PadLeft((centerMaxLength + text.Length) / 2, surroundWith).PadRight(centerMaxLength, surroundWith));
        }

    }

}