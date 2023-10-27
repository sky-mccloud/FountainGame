using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FountainGame
{
    internal class Entrance : Room
    {
        public override string? Sense => "You see light coming from the cavern entrance.";
        public override ConsoleColor BackgroundColor => ConsoleColor.Green;

        public override void AdjacentRoomSense()
        {
            // Leaving this as empty. Since it shouldn't display a sense when outside this room.
        }

    }
}
