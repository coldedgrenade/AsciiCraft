using System;
using System.Numerics;
using System.Threading;
using Players;

namespace Jump
{
    class Jump
    {
        static void UpdateO(Vector2 oPos)
        {

            Console.SetCursorPosition((int)oPos.X, (int)oPos.Y);
            Console.Write('O');
        }

        private static Vector2 _SP = new(5, 15);

        public static Vector2 SP
        {
            get { return _SP; }
            set
            {
                UpdateO(_SP); // Clear previous position of 'O'
                _SP = value;
                UpdateO(value);
            }
        }

        static void Main(string[] args, Player character)
        {
            ConsoleKeyInfo Key = Console.ReadKey();

        }

        public static void _JUMP(Player character)
        {
            Console.SetCursorPosition((int)character.GetCoords().X, (int)character.GetCoords().Y);
            Console.Write(' ');
            character.SetCoords(new Vector2(character.GetCoords().X, character.GetCoords().Y - character.GetJumpHeight()));
            Console.Write(' ');
        }
    }

}