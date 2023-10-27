using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FountainGame
{
    internal abstract class Room
    {
        public abstract string? Sense { get; }
        public abstract ConsoleColor BackgroundColor { get; }
        public virtual ConsoleColor ForegroundColor { get; } = ConsoleColor.Black;

        // Derived rooms that shouldn't display a sense when inside/outside of them would just override the corresponsive method
        // Not sure if this is the right approach, the program seems to be quite small, and if it was larger the concrete classes could have had more functionalities

        public virtual void InsideRoomSense()
        {
            if (Sense != null)
                DisplaySense();
        }
        public virtual void AdjacentRoomSense()
        {
            if (Sense != null)
                DisplaySense();
        }

        private void DisplaySense()
        {
            Helpers.GetConsoleColor(out var background, out var foreground);
            Helpers.SetConsoleColor(BackgroundColor, ForegroundColor);
            Console.Write(Sense);
            Helpers.SetConsoleColor(background, foreground);

            Console.Write("\r\n");
        }

        public override string ToString() => GetType().Name;

        // and I tried similar things, but without knowing about dynamic keyword I couldn't solve the occurring problems (now this method always creates new instances for each room)
        public static Room CreateDerivedRoomInstance<T>(T room) where T : Room, new()
        {
            return new T();
        }
    }
}
