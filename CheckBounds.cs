using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Bounds
{
    internal class Bounds
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadConsoleOutputCharacter(IntPtr hConsoleOutput, out char lpCharacter, uint nLength, COORD dwReadCoord, out uint lpNumberOfCharsRead);

        [StructLayout(LayoutKind.Sequential)]
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

        const int STD_OUTPUT_HANDLE = -11;
        const int STD_ERROR_HANDLE = -12;
            public static Tuple<char[], bool[]> Check(COORD spot)
            {
                uint NumCharRead1;
                uint NumCharRead2;
                uint NumCharRead3;
                uint NumCharRead4;

                IntPtr hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);
                int cursorLeft = Console.CursorLeft;
                int cursorTop = Console.CursorTop;

                //List<COORD> upReadCoords = new();

                //for (int i = 0; i < 10; i++)
                //{
                //upReadCoords.Add(new COORD { X = (short)(cursorLeft-1), Y = (short)(cursorTop - i) });
                //}

                COORD[] coordinates =
                {
                    new COORD { X = (short)(cursorLeft - 1), Y = (short)(cursorTop + 1) }, //dwReadCoord
                    new COORD { X = (short)(cursorLeft - 2), Y = (short)(cursorTop) },     //leftReadCoord
                    new COORD { X = (short)(cursorLeft), Y = (short)(cursorTop) },          //rightReadCoord
                };

                char charBelowCursor, charLeftCursor, charRightCursor, charCursor;

                bool[] successValues =
                {
                ReadConsoleOutputCharacter(hConsoleOutput, out charBelowCursor, 1, coordinates[0], out NumCharRead1), //1
                ReadConsoleOutputCharacter(hConsoleOutput, out charLeftCursor, 1, coordinates[1], out NumCharRead2),  //2
                ReadConsoleOutputCharacter(hConsoleOutput, out charRightCursor, 1, coordinates[2], out NumCharRead3),  //3
                ReadConsoleOutputCharacter(hConsoleOutput, out charCursor, 1, spot, out NumCharRead4) // 4

            };

                char[] char__Cursor =
                {
                    charBelowCursor,
                    charLeftCursor,
                    charRightCursor,
                    charCursor
                };

                return Tuple.Create(char__Cursor, successValues);
            }
        }
    }

