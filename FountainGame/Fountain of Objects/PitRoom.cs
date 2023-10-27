using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FountainGame
{
    internal class PitRoom : Room
    {
        public override string? Sense => "You feel a draft. There is a pit in a nearby room.";
        public override ConsoleColor BackgroundColor => ConsoleColor.Black;
        public override ConsoleColor ForegroundColor => ConsoleColor.White;

        public override void InsideRoomSense()
        {
            // Leaving this as empty. Since it shouldn't display a sense when inside this room.
        }

    }
}
