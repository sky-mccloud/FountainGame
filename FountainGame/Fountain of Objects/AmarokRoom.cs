using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FountainGame
{
    internal class AmarokRoom : Room
    {
        public override ConsoleColor BackgroundColor => AmarokAlive ? ConsoleColor.Red : ConsoleColor.DarkGray;
        public override ConsoleColor ForegroundColor => ConsoleColor.White;

        public override string? Sense => AmarokAlive ? "You can smell the rotten stench of an amarok in nearby room." : null; // we can leave it as null or make the player sense a wasted amarok
        public bool AmarokAlive { get; private set; } = true;

        public void DestroyAmarok()
        {
            AmarokAlive = false;

            Helpers.GetConsoleColor(out var background, out var foreground);
            Helpers.SetConsoleColor(ConsoleColor.DarkGray, ConsoleColor.White);
            Console.Write("You have destroyed an Amarok");
            Helpers.SetConsoleColor(background, foreground);
            Console.WriteLine();
        }

        public override void InsideRoomSense()
        {
            // Leaving this as empty. Since it shouldn't display a sense when inside this room.
        }

        public override string ToString() => AmarokAlive ? base.ToString() : "Wasted Amarok";
    }
}
