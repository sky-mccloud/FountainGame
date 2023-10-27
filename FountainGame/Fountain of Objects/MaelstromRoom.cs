using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FountainGame
{
    internal class MaelstromRoom : Room
    {
        public override string? Sense => "You hear the growling and groaning of a maelstrom nearby.";
        public override ConsoleColor BackgroundColor => ConsoleColor.Magenta;

        public override void InsideRoomSense()
        {
            // Leaving this as empty. Since it shouldn't display a sense when inside this room.
        }

    }
}
