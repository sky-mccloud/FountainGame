using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FountainGame
{
    internal class EmptyRoom : Room
    {
        public override string? Sense { get; } = null;
        public override ConsoleColor BackgroundColor => ConsoleColor.White;

    }
}
