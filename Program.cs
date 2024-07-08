using Players;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorldGeneration;
using static Sidescroll2.Game.ConsoleListener;
using System.Media;
using GameAudio;
using static GameAudio.GameAudio;
using Bounds;
using static System.Net.Mime.MediaTypeNames;
using Block;

namespace Sidescroll2
{
    public class P1
    {
        Player player1 = new();
        public Player returnPlayer()
        {
            return player1;
        }
    }

    

    class Game
    {
        public static WG wg = new();
        public string Map = "";
        public bool inMenu = true;
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadConsoleOutputCharacter(IntPtr hConsoleOutput, out char lpCharacter, uint nLength, COORD dwReadCoord, out uint lpNumberOfCharsRead);

        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;
        }

        const int STD_OUTPUT_HANDLE = -11;
        const int STD_ERROR_HANDLE = -12;

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        const int VK_LBUTTON = 0x01; // Left mouse button virtual key code
        const int VK_RBUTTON = 0x02; // Right mouse button virtual key code
        const int VK_MBUTTON = 0x04; // middle click yeahj

        static void DrawObject(int id)
        {

        }

        public static class ConsoleListener
        {
            public static event Action<MOUSE_EVENT_RECORD, Player> MouseEvent; // Update event signature

            private static bool Run = false;

            public static void Start()
            {
                if (!Run)
                {
                    Run = true;
                    IntPtr handleIn = GetStdHandle(STD_INPUT_HANDLE);

                    // Enable mouse and window input
                    uint mode = 0;
                    GetConsoleMode(handleIn, ref mode);
                    mode &= ~ENABLE_QUICK_EDIT_MODE; //disable
                    mode |= ENABLE_WINDOW_INPUT; //enable (if you want)
                    mode |= ENABLE_MOUSE_INPUT; //enable
                    SetConsoleMode(handleIn, mode);

                    new Thread(() =>
                    {
                        while (true)
                        {
                            uint numRead = 0;
                            INPUT_RECORD[] record = new INPUT_RECORD[1];
                            record[0] = new INPUT_RECORD();
                            ReadConsoleInput(handleIn, record, 1, ref numRead);
                            if (Run)
                            {
                                switch (record[0].EventType)
                                {
                                    case INPUT_RECORD.MOUSE_EVENT:
                                        MouseEvent?.Invoke(record[0].MouseEvent, new P1().returnPlayer());
                                        break;
                                }
                            }
                            else
                            {
                                uint numWritten = 0;
                                WriteConsoleInput(handleIn, record, 1, ref numWritten);
                                return;
                            }
                        }
                    }).Start();

                    MouseEvent += OnMouseEvent;
                }
            }


            public static void Stop() => Run = false;

            [StructLayout(LayoutKind.Explicit)]
            public struct INPUT_RECORD
            {
                public const ushort KEY_EVENT = 0x0001,
                    MOUSE_EVENT = 0x0002,
                    WINDOW_BUFFER_SIZE_EVENT = 0x0004; //more

                [FieldOffset(0)]
                public ushort EventType;
                [FieldOffset(4)]
                public MOUSE_EVENT_RECORD MouseEvent;
            }

            public struct MOUSE_EVENT_RECORD
            {
                public COORD dwMousePosition;

                public const uint FROM_LEFT_1ST_BUTTON_PRESSED = 0x0001;
                public uint dwButtonState;

                public const int MOUSE_MOVED = 0x0001;
                public uint dwEventFlags;
            }

            public struct COORD
            {
                public short X;
                public short Y;

                public COORD(short x, short y)
                {
                    X = x;
                    Y = y;
                }
            }

            public const uint STD_INPUT_HANDLE = unchecked((uint)-10);

            public const uint ENABLE_MOUSE_INPUT = 0x0010,
                ENABLE_QUICK_EDIT_MODE = 0x0040,
                ENABLE_EXTENDED_FLAGS = 0x0080,
                ENABLE_ECHO_INPUT = 0x0004,
                ENABLE_WINDOW_INPUT = 0x0008; //more

            [DllImport("kernel32.dll")]
            public static extern IntPtr GetStdHandle(uint nStdHandle);

            [DllImport("kernel32.dll")]
            public static extern bool GetConsoleMode(IntPtr hConsoleInput, ref uint lpMode);

            [DllImport("kernel32.dll")]
            public static extern bool SetConsoleMode(IntPtr hConsoleInput, uint dwMode);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
            public static extern bool ReadConsoleInput(IntPtr hConsoleInput, [Out] INPUT_RECORD[] lpBuffer, uint nLength, ref uint lpNumberOfEventsRead);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
            public static extern bool WriteConsoleInput(IntPtr hConsoleInput, INPUT_RECORD[] lpBuffer, uint nLength, ref uint lpNumberOfEventsWritten);
        }

        public static void CleanTrail(Player player1)
        {
            //if(player1.IsBuilding() == true) { return; }

            Console.SetCursorPosition((int)player1.GetCoords().X, (int)player1.GetCoords().Y);
            Console.Write(' ');
        }

        static async Task ReadInput(Player player1, bool CantMove)
        {
            if (CantMove) { return; }
            while (true)
            {
                //if (Console.KeyAvailable)
                //{
                    Tuple<char[], bool[]> collisionMap = Bounds.Bounds.Check(new Bounds.Bounds.COORD());

                    bool success2 = collisionMap.Item2[1]; // left
                    bool success3 = collisionMap.Item2[2]; // right

                    char charLeftCursor = collisionMap.Item1[1];
                    char charRightCursor = collisionMap.Item1[2];
                    var key = Console.ReadKey(true);
                    
                    if (charLeftCursor == 'O' || charLeftCursor == '<' || charLeftCursor == '>')
                    {
                        Console.SetCursorPosition((int)player1.GetCoords().X - 1, (int)player1.GetCoords().Y);
                        Console.Write(' ');
                    }
                    if (charLeftCursor == 'O' || charLeftCursor == '<' || charLeftCursor == '>')
                    {
                        Console.SetCursorPosition((int)player1.GetCoords().X + 1, (int)player1.GetCoords().Y);
                        Console.Write(' ');
                    }

                switch (key.Key)
                    {

                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                            if (Console.GetCursorPosition().Left >= 2)
                            {
                                if (charLeftCursor != ' ') { break; }

                                CleanTrail(player1);
                                player1.Move(new Vector2(-1, 0));
                                Debug.WriteLine($"Player is at {player1.GetCoords()}");
                            }
                            RemoveAllOs();
                            break;
                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                            if (Console.GetCursorPosition().Left <= Console.WindowWidth - 2)
                            {
                                if (charRightCursor != ' ') { break; }
                                CleanTrail(player1);
                                player1.Move(new Vector2(1, 0));
                                Debug.WriteLine($"Player is at {player1.GetCoords()}");
                            }
                            RemoveAllOs();
                            break;
                        case ConsoleKey.UpArrow:
                        case ConsoleKey.Spacebar:
                        //if (player1.GetCoords().Y - 2 >= Console.WindowTop) { return; }

                        //if (player1.IsFalling()) { break; }
                            char upReadCursor;
                            uint LP = 0;
                            
                            ReadConsoleOutputCharacter(GetStdHandle(STD_OUTPUT_HANDLE), out upReadCursor, 1, new COORD { X = (short)(Console.CursorLeft - 1), Y = (short)(Console.CursorTop - 1) }, out LP);
                            
                            if (upReadCursor != ' ')
                            {
                                break;
                            }

                            player1.PJump();
                            RemoveAllOs();
                            break;
                    }
                //}
            }
        }

        public bool CheckIfMenu()
        {
            if (inMenu == true)
            {
                return true;
            }
            return false;
        }

         static async void OnMouseEvent(MOUSE_EVENT_RECORD mouseEvent, Player player1) // Add Player parameter
         {
            if (mouseEvent.dwButtonState == MOUSE_EVENT_RECORD.FROM_LEFT_1ST_BUTTON_PRESSED)
            {
                

                player1.SetBuilding(true);

                //wg.SetBlock(mouseEvent.dwMousePosition.X, mouseEvent.dwMousePosition.Y, wg.GetMap(), 1);
                
                Console.SetCursorPosition(mouseEvent.dwMousePosition.X, mouseEvent.dwMousePosition.Y);
                Console.Write(' ');
                Console.SetCursorPosition((int)player1.GetCoords().X, (int)player1.GetCoords().Y + 1);

                // Wait for 50 milliseconds
                await Task.Delay(50);

                // Turn off the building after 50 milliseconds
                player1.SetBuilding(false);
                
            }

            else if ((GetAsyncKeyState(VK_RBUTTON) & 0x8000) != 0)
            {
                player1.SetBuilding(true);
                Console.SetCursorPosition(mouseEvent.dwMousePosition.X, mouseEvent.dwMousePosition.Y);
                Console.Write('x');
                Console.SetCursorPosition((int)player1.GetCoords().X, (int)player1.GetCoords().Y + 1);

                // Wait for 50 milliseconds
                await Task.Delay(50);

                // Turn off the building after 50 milliseconds
                player1.SetBuilding(false);

            }
            
            else if ((GetAsyncKeyState(VK_MBUTTON) & 0x8000) != 0)
            {
                player1.SetBuilding(true);
                Vector2 Mcoords = new(mouseEvent.dwMousePosition.X, mouseEvent.dwMousePosition.Y);
                


                // Wait for 50 milliseconds
                await Task.Delay(50);

                // Turn off the building after 50 milliseconds
                player1.SetBuilding(false);

            }

            //player1.SetBuilding(false);
        }

        static void PlayMusic(Note[] music)
        {
            GameAudio.GameAudio gA = new();
            gA.Play(music);
        }

        static void RemoveAllOs()
        {
            for (int i = 0; i < Console.WindowLeft; i++)
            {
                for (int j = 0; j < Console.WindowTop; j++)
                {
                    Bounds.Bounds.COORD spot = new();
                    spot.X = (short)i;
                    spot.Y = (short)j;
                    Tuple<char[], bool[]> collisionMap = Bounds.Bounds.Check(spot);

                    Debug.WriteLine($"{collisionMap.Item1[3]}");

                    if (collisionMap.Item1[3] == 'O')
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write(' ');
                    }

                }
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("Welcome to AsciiCraft a1.0");
            Console.WriteLine("Play");
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();
            wg.GenerateMap(20, Console.WindowWidth - 1, 3, 5, 0.5, 5, 0.2, 20);
            //.Block.Block[,] map = WG.new().ReturnMap();

            Task task = Task.Run(() =>
            {
                ConsoleListener.Start();
            });
            Console.OutputEncoding = Encoding.UTF8;

            bool cantMove = false;

            Player player1 = new();
            int position = 0;
            

            Console.Title = "AsciiCraft a1.0";

            bool isFalling = false;

            Task inputTask = Task.Run(() => ReadInput(player1, cantMove));

            //Task startMusic = Task.Run(() => PlayMusic(Sample.Day));

            while (true)
            {
                
                Tuple<char[], bool[]> collisionMap = Bounds.Bounds.Check(new Bounds.Bounds.COORD());
                char charBelowCursor = collisionMap.Item1[0];
                
                bool success1 = collisionMap.Item2[0];

                //Debug.WriteLine($"{charBelowCursor} {charLeftCursor} {charRightCursor}");

                if (success1 && charBelowCursor == ' ' && player1.IsBuilding() == false)
                {
                    isFalling = true;
                }
                else
                {
                    isFalling = false;
                }

                if (isFalling && !player1.IsBuilding())
                {
                    Console.SetCursorPosition((int)player1.GetCoords().X, (int)player1.GetCoords().Y);
                    Console.Write(' ');
                    player1.Fall(1);
                }

                player1.UpdateFrame(0);

                Thread.Sleep(100);
            }
        }
    }
}
