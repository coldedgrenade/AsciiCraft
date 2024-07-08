using System;
using System.Collections.Generic;
using System.Diagnostics;
using Blocks;

namespace WorldGeneration
{
    internal class WG
    {
        private Block.Block[,] map;

        // Define a delegate for the event
        public delegate void MapChangedEventHandler(object sender, EventArgs e);
        // Define the event based on the delegate
        public event MapChangedEventHandler MapChanged;

        public Block.Block[,] SetBlock(int x, int y, Block.Block[,] bk, int id)
        {
            bk[x, y] = new Block.Block(x, y, id, new Dictionary<string, object>());

            // Trigger the event when the map is changed
            OnMapChanged();

            return bk;
        }

        // Function to trigger the event
        protected virtual void OnMapChanged()
        {
            MapChanged?.Invoke(this, EventArgs.Empty);
        }

        public void GenerateMap(int WorldHeight, int WorldSize, int grassDepth, int oreSize, double coalProbability, int coalHeight, double ironProbability, int ironHeight)
        {
            map = new Block.Block[WorldSize, WorldHeight];

            Random rand = new Random();

            // gen map
            for (int i = 0; i < WorldSize; i++)
            {
                for (int j = 0; j < WorldHeight; j++)
                {
                    if (j < 10)
                    {
                        SetBlock(i, j, map, 0);
                    }
                    else
                    {
                        SetBlock(i, j, map, 1);
                    }

                }
            }

            // print map
            for (int i = 0; i < WorldHeight; i++)
            {
                for (int j = 0; j < WorldSize; j++)
                {
                    Block.Block foundBlock = map[j, i];
                    Console.SetCursorPosition(j, i);
                    char block = ' ';
                    switch (foundBlock.ID)
                    {
                        case 0:
                            block = ' ';
                            break;
                        case 1:
                            block = 'm';
                            break;
                    }
                    Console.Write(block);
                }
                Console.WriteLine();
            }
        }

        // Function to get the value of the map at given coordinates
        public int GetMapValue(int x, int y)
        {
            if (x < 0 || x >= map.GetLength(0) || y < 0 || y >= map.GetLength(1))
            {
                // Out of bounds
                return -1;
            }
            else
            {
                return map[x, y].ID;
            }
        }
        public Block.Block[,] GetMap()
        {
            return map;
        }
    }
}
