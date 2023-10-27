using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames; 

namespace FountainGame
{
    // Not sure about my OO design of this task. Everything seems to be crammed up into one class
    // If anyone is interested at throwing an eye on it I'll link it to my GitHub, I would really appreciate to hear yours opinion and some criticism

    internal static class Game
    {
        // Main method for creating game instances and running it
        public static void Run()
        {
            // I didn't implement any previous output clearing methods, so that it would be possible to track players' move history by just simply scrolling above and seeing game progress on each move
            // of course another class could handle such thing, but I wanted to do a mvp and move on to learn more
            
            bool retry;

            do
            {
                Console.WriteLine("\nYou have started a new game.");

                BoardData gameData = new(ChooseFieldSize());

                gameData.Play();

                DisplayGameMap(gameData);

                retry = PlayAgain();
            } while (retry);
        }

        private static bool PlayAgain()
        {
            Console.Write("Would you like to play another game or exit the program? (y - to play again, n - to exit the program)");
            return Helpers.YNInputCheck();
        }

        private static int ChooseFieldSize()
        {
            int chosen;
            do
            {
                Console.WriteLine("Please choose the field size for your game:\n");
                Console.WriteLine("1. -- 4x4;\n2. -- 6x6;\n3. -- 8x8;\n");

            } while (!Helpers.Choice(1, 3, out chosen));

            return chosen switch
            {
                2 => 6,
                3 => 8,
                _ => 4,
            };
        }


        // The method openly displays to the player where all the rooms were on a map copy
        private static void DisplayGameMap(BoardData gameData)
        {
            int boundaries = gameData.BoundariesUpper;
            string gameOver = "Game Over";
            Console.WriteLine();
            
            Helpers.WriteLineCenteredText(gameOver, Console.WindowWidth, false, '-');
            //Console.WriteLine(gameOver.PadLeft((Console.WindowWidth + gameOver.Length) / 2, '-').PadRight(Console.WindowWidth, '-'));<-- replaced this with my helper method ^
            Console.WriteLine("The exposed current game map is:");

            for (int row = (boundaries - 1); row >= 0; row--)
            {
                Console.Write("\n");
                Helpers.WriteLineCenteredText("", (boundaries * 14) + 1, false, '-');
                //Console.WriteLine("\n-".PadRight((boundaries * 14) + 2, '-')); <-- replaced this with my helper method ^
                Console.Write("|");
                for (int col = 0; col < boundaries; col++)
                {
                    Helpers.GetConsoleColor(out var background, out var foreground);
                    var room = gameData.Rooms[row, col];

                    Helpers.SetConsoleColor(room.BackgroundColor, room.ForegroundColor);

                    Helpers.WriteCenteredText(room.ToString(), 13, false);
                    //Console.Write($"{cellText.PadLeft((13 + cellText.Length) / 2),-13}");<-- replaced this with my helper method ^

                    Helpers.SetConsoleColor(background, foreground);
                    Console.Write("|");

                }
            }

            Console.Write("\n");
            Helpers.WriteLineCenteredText("", (boundaries * 14) + 1, false, '-');
            Console.WriteLine();
        }

    }




}
