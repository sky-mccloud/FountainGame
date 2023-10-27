using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace FountainGame
{
    internal class BoardData
    {
        public Room[,] Rooms { get; private set; }
        public List<RoomCoordinates> TravelHistory { get; private set; } = new();
        public int BoundariesUpper { get; }
        public int BoundariesLower { get; } = 0;
        public int PositionRow { get; private set; }
        public int PositionCol { get; private set; }
        public Room CurrentRoom => Rooms[PositionRow, PositionCol];
        public bool ActivatedFountain { get; private set; } = default;
        public int BulletsLeft { get; private set; }

        // I have decided to put majority of game methods and properties in the same BoardData class.
        // I'm not very sure if this is a correct approach (doesn't really seem like)
        // But these methods that work with the game data just don't seem to fit elsewhere in this small project
        // It kind of seems like I'm creating additional classes just to create them, but they would still mainly work with one class data and have very little if none of uniqueness
        // Maybe that's where I am wrong :D I still need to gather a lot more knowledge about C# and OOP


        public BoardData(int dimensions)
        {
            Rooms = new Room[dimensions, dimensions];
            BoundariesUpper = dimensions;
            BulletsLeft = 7;

            // Creates pairs of randomized coordinates (across the map) and respective derived rooms
            // This way each new game is initialized with randomly placed instances of derived rooms
            Dictionary<RoomCoordinates, dynamic> randomizedMap = CreateCoordinatesDictionary(dimensions);

            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                {
                    RoomCoordinates currentRoom = new(i, j);

                    if (randomizedMap.ContainsKey(currentRoom))
                    {
                        Rooms[i, j] = Room.CreateDerivedRoomInstance(randomizedMap[currentRoom]);
                        if (randomizedMap[currentRoom] is Entrance)
                        {
                            PositionRow = i;
                            PositionCol = j;
                        }
                    }
                    else
                        Rooms[i, j] = new EmptyRoom();
                }
            }

            TravelHistory.Add(new RoomCoordinates(PositionRow, PositionCol));
        }

        private static Dictionary<RoomCoordinates, dynamic> CreateCoordinatesDictionary(int dimensions)
        {
            Random random = new();

            // this dictionary is used for setting up default rooms and their count depending on different dimensions

            // this later on came to be quite problematic: The reason for it is that my later methods just took the same concrete Room instance
            // from the setup dictionary and used the same instance to place it in different places across the map.

            Dictionary<Room, int> defaultMapRoomSetup = new()
            {
                { new Entrance(), 1 },
                { new FountainRoom(), 1 },
                { new PitRoom(), (int)Math.Round(dimensions / 1.5d) },
                { new MaelstromRoom(), (int)Math.Round(dimensions / 4d) },
                { new AmarokRoom(), (int)Math.Round(dimensions / 1.5d) }
            };

            // the issue was solved by just skimming thru the future sections of the book and noticing dynamic objects section which helped me to solve my problem :D

            Dictionary<RoomCoordinates, dynamic> randomizedMapRooms = new();
            RoomCoordinates temp;

            foreach (var pair in defaultMapRoomSetup)
            {
                for (int i = 0; i < pair.Value; i++)
                {
                    do
                    {
                        temp = new RoomCoordinates(random.Next(dimensions), random.Next(dimensions));

                    } while (randomizedMapRooms.ContainsKey(temp));

                    // this is where the issue would have been. I would just be passing old instances instead of creating a new ones
                    // but combined with CreateDerivedRoomInstance<T>() method and passing a dynamic value to it solved my issue
                    // now the dictionary holds only temporary instances of derived classes and creates new ones in after mentioned method

                    randomizedMapRooms.Add(temp, pair.Key);
                }
            }

            return randomizedMapRooms;
        }


        public void Play()
        {
            do
            {
                Console.WriteLine("\n-".PadRight(40, '-'));

                DisplayPosition();
                if (GameFinished())
                    break;
                DisplaySense();

                if (!UpdatePlayerPosition())
                    break;

            } while (true);
        }

        private void DisplayPosition()
        {
            // This method outputs a movement history map that shows where the player used to be and where it currently is at
            // made just to make the game look better and easier for the player to orientate in
            // Since it's easier to see your position visually, I have put more traps into the game itself

            Console.WriteLine($"You are in the room at (Row={PositionRow}, Column={PositionCol})");
            Console.WriteLine("Your current locations is marked as red square in the map.\n");

            for (int row = (BoundariesUpper - 1); row >= 0; row--)
            {
                Console.Write("\n");
                Helpers.WriteLineCenteredText("", (BoundariesUpper * 14) + 1, false, '-');
                //Console.WriteLine("\n-".PadRight((BoundariesUpper * 14) + 2, '-')); <-- instead of this code I chose to use my helper method, which I later used in other places too/

                Console.Write("|");
                for (int col = 0; col < BoundariesUpper; col++)
                {
                    Helpers.GetConsoleColor(out var background, out var foreground);
                    var temp = new RoomCoordinates(row, col);

                    if (TravelHistory.Contains(temp))
                        Helpers.SetConsoleColor(ConsoleColor.DarkYellow);

                    if (row == PositionRow && col == PositionCol)
                        Helpers.SetConsoleColor(ConsoleColor.DarkRed);

                    Helpers.WriteCenteredText(" ", 13, false);
                    //Console.Write($"{" ",-13}");

                    Helpers.SetConsoleColor(background, foreground);
                    Console.Write("|");

                }
            }
            Console.Write("\n");
            Helpers.WriteLineCenteredText("", (BoundariesUpper * 14) + 1, false, '-');
            Console.WriteLine();
        }

        private bool GameFinished()
        {
            Helpers.GetConsoleColor(out var background, out var foreground);

            // Thinking about this method, maybe if the program wasn't so small all of those winning and losing texts and custom colors could be implemented in the derived rooms 
            // Not very proud of this since it seems just crammed with similar code and the solution might not be the that proper

            if (CurrentRoom is Entrance && ActivatedFountain)
            {
                Helpers.SetConsoleColor(ConsoleColor.Green);

                Helpers.WriteLineCenteredText("The Fountain of Objects has been reactivated, and you have escaped with your life!");
                Helpers.WriteCenteredText("Congratulations: You have won!");

                Helpers.SetConsoleColor(background, foreground);
                Console.Write("\r\n");

                return true;
            }
            else if (CurrentRoom is PitRoom)
            {
                Helpers.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);

                Helpers.WriteLineCenteredText("You have fell into the pit.");
                Helpers.WriteCenteredText("The game was lost.");

                Helpers.SetConsoleColor(background, foreground);
                Console.Write("\r\n");

                return true;
            }
            else if (CurrentRoom is MaelstromRoom)
            {
                MaelstromTeleportations();

                Helpers.SetConsoleColor(ConsoleColor.Yellow);

                Helpers.WriteCenteredText("You have been teleported into a new position by Maelstrom winds.");

                Helpers.SetConsoleColor(background, foreground);
                Console.Write("\r\n");

                DisplayPosition();

                // Second check ensures that in case you have been teleported into a pit, amarok or maelstrom room the game would count that as entering it manually and would act accordingly
                return GameFinished();
            }
            else if (CurrentRoom is AmarokRoom amarokRoom && amarokRoom.AmarokAlive)
            {
                Helpers.SetConsoleColor(ConsoleColor.DarkRed);

                Helpers.WriteLineCenteredText("You have been wasted by an Aamarok");
                Helpers.WriteCenteredText("The game was lost.");

                Helpers.SetConsoleColor(background, foreground);
                Console.Write("\r\n");

                return true;
            }
            else
                return false;
        }

        // Scans for whole board and displays current and adjacent room senses
        private void DisplaySense()
        {
            for (int row = 0; row < Rooms.GetLength(0); row++)
            {
                for (int col = 0; col < Rooms.GetLength(1); col++)
                {
                    Room room = Rooms[row, col];

                    if (PositionRow == row && PositionCol == col)
                    {
                        room.InsideRoomSense();
                    }
                    else if (Math.Abs(PositionRow - row) <= 1 && Math.Abs(PositionCol - col) <= 1)
                    {
                        room.AdjacentRoomSense();
                    }
                }
            }
        }

        // A method that places the Maelstrom into a new random location and also teleports the player to random location
        private void MaelstromTeleportations()
        {
            Random random = new();
            int rndRow;
            int rndCol;

            // this code ensures that the player and maelstrom doesn't teleport to the same locations before the teleportation

            do
            {
                rndRow = random.Next(BoundariesUpper);
                rndCol = random.Next(BoundariesUpper);
            } while (Rooms[rndRow, rndCol] is not EmptyRoom);


            Rooms[PositionRow, PositionCol] = new EmptyRoom();
            Rooms[rndRow, rndCol] = new MaelstromRoom();

            PositionCol = random.Next(BoundariesUpper);
            PositionRow = random.Next(BoundariesUpper);

            TravelHistory.Add(new RoomCoordinates(PositionRow, PositionCol));
        }

        // Well I am very unsure about this piece of code. This whole method is a chunk of if statements, some nested in others
        // I am just wondering how bad is this approach, since it seems quite repetitive in some places and overall weird
        private bool UpdatePlayerPosition()
        {
            ConsoleKeyInfo inputKey;

            do
            {
                Console.WriteLine("\nWhat would you like to do?");

                int positionRow = PositionRow;
                int positionCol = PositionCol;

                // found this piece below in documentation. It simply allows the user to use keyboard modifiers to use key combination that include modifiers.
                // this way I was able to let the player shoot using ctrl+arrow keys, walk with arrow keys, and execute commands by typing words
                Console.TreatControlCAsInput = true;
                inputKey = Console.ReadKey();

                var direction = inputKey.Key;

                if ((inputKey.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    // the code below scans for shot direction and checks if there were any amaroks
                    // I made it so that the bullets would cross the whole map | we can provide the user less bullets to make it harder

                    if (BulletsLeft > 0)
                    {

                        if (direction == ConsoleKey.UpArrow)
                        {
                            Console.WriteLine("You have shot in-front of yourself");
                            Console.WriteLine($"Bullets left: {--BulletsLeft}");

                            for (int row = PositionRow; row < BoundariesUpper; row++)
                            {
                                if (Rooms[row, PositionCol] is AmarokRoom amarokRoom && amarokRoom.AmarokAlive)
                                    amarokRoom.DestroyAmarok();
                            }
                        }
                        else if (direction == ConsoleKey.DownArrow)
                        {
                            Console.WriteLine("You have shot behind yourself");
                            Console.WriteLine($"Bullets left: {--BulletsLeft}");

                            for (int row = PositionRow; row >= BoundariesLower; row--)
                            {
                                if (Rooms[row, PositionCol] is AmarokRoom amarokRoom && amarokRoom.AmarokAlive)
                                    amarokRoom.DestroyAmarok();
                            }
                        }
                        else if (direction == ConsoleKey.RightArrow)
                        {
                            Console.WriteLine("You have shot to the right");
                            Console.WriteLine($"Bullets left: {--BulletsLeft}");

                            for (int col = PositionCol; col < BoundariesUpper; col++)
                            {
                                if (Rooms[PositionRow, col] is AmarokRoom amarokRoom && amarokRoom.AmarokAlive)
                                    amarokRoom.DestroyAmarok();
                            }
                        }
                        else if (direction == ConsoleKey.LeftArrow)
                        {
                            Console.WriteLine("You have shot to the left");
                            Console.WriteLine($"Bullets left: {--BulletsLeft}");

                            for (int col = PositionCol; col >= BoundariesLower; col--)
                            {
                                if (Rooms[PositionRow, col] is AmarokRoom amarokRoom && amarokRoom.AmarokAlive)
                                    amarokRoom.DestroyAmarok();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Sorry we couldn't read your input, please try again." +
                                    "\nType 'help' to get info about the game or type: 'exit' to exit the game");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You have ran out of bullets. You can't shoot anymore");
                    }
                }
                else
                {
                    bool isArrowKey = direction switch
                    {
                        ConsoleKey.UpArrow or ConsoleKey.DownArrow or ConsoleKey.RightArrow or ConsoleKey.LeftArrow => true,
                        _ => false
                    };

                    if (isArrowKey)
                    {
                        var moved = direction switch
                        {
                            ConsoleKey.UpArrow => ValidMove(++positionRow, positionCol),
                            ConsoleKey.DownArrow => ValidMove(--positionRow, positionCol),
                            ConsoleKey.RightArrow => ValidMove(positionRow, ++positionCol),
                            ConsoleKey.LeftArrow => ValidMove(positionRow, --positionCol),
                            _ => false,
                        };

                        if (moved)
                            break;
                    }
                    else
                    {
                        Console.TreatControlCAsInput = false;

                        string command = (inputKey.KeyChar.ToString() + Console.ReadLine()).ToLower();

                        FountainRoom? fountainRoom = CurrentRoom as FountainRoom;

                        if (fountainRoom != null && (command == "enable"))
                        {
                            fountainRoom.Activate();
                            ActivatedFountain = true;
                            break;
                        }
                        else if (fountainRoom != null && (command == "disable"))
                        {
                            fountainRoom.Deactivate();
                            ActivatedFountain = false;
                            break;
                        }
                        else if (command == "exit")
                        {
                            return false;
                        }
                        else if (command == "help")
                        {
                            DisplayHelp();
                        }
                        else
                        {
                            Console.WriteLine("Sorry we couldn't read your input, please try again." +
                                "\nType 'help' to get info about the game or type: 'exit' to exit the game");
                        }

                        continue;
                    }
                }
            } while (true);

            Console.TreatControlCAsInput = true;

            // We use travel history to actively display where the player currently is and was
            RoomCoordinates position = new(PositionRow, PositionCol);
            if (!TravelHistory.Contains(position))
                TravelHistory.Add(position);

            return true;
        }

        private void DisplayHelp()
        {
            Console.WriteLine("".PadLeft(40, '-'));

            Console.WriteLine("Your position is displayed as a red square in the map." +
                "\nUse your arrow keys to walk thru the map" +
                "\nYour objective is to comeback to the entrance after you find the fountain room and enable it" +
                "\nUnactivated Fountain will be displayed in gray color in the aftergame display" +
                "\nEach map has dangers that you can sense when in adjacent room to them." +
                "\nYou can destroy an Amarok by holding ctrl and pressing respective arrow key in which direction you would like to shoot (e.g. ctrl+UpArrow shoots at the columns above)" +
                "\nDestroyed Amarok will be displayed in gray color" +

                "\n\nYou can enable the fountain by typing 'enable' while in the room or you can disable it by typing 'disable'" +
                "\nYou can type 'exit' to exit current game");
            Console.WriteLine("".PadLeft(40, '-'));
        }

        private bool ValidMove(int nextMoveRow, int nextMoveCol)
        {
            if ((nextMoveRow >= BoundariesUpper || nextMoveRow < BoundariesLower) || (nextMoveCol >= BoundariesUpper || nextMoveCol < BoundariesLower))
            {
                Console.WriteLine($"Sorry, but we cannot make such move, it exceeds the board boundaries.");
                return false;
            }
            else
            {
                PositionRow = nextMoveRow;
                PositionCol = nextMoveCol;
                return true;
            }
        }
    }

    public record RoomCoordinates(int Row, int Col);
}
