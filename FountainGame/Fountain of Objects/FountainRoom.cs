using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FountainGame
{
    internal class FountainRoom : Room
    {
        public bool Activated { get; private set; }
        public override ConsoleColor BackgroundColor => Activated ? ConsoleColor.Cyan : ConsoleColor.Gray;
        public override string? Sense => Activated ? "You hear the rushing waters from the Fountain of Objects. It has been reactivated!"
                    : "You hear water dripping in this room. The Fountain of Objects is here!";


        public void Activate() { Activated = true; }
        public void Deactivate() { Activated = false; }
        public override void AdjacentRoomSense()
        {
            // Leaving this as empty. Since it shouldn't display a sense when outside this room.
        }
    }
}
