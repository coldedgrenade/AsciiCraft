using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Block
{
        public class Block
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int ID { get; set; }
         public Dictionary<string, object> NBT { get; set; }

        public Block(int x, int y, int id, Dictionary<string, object> nbt)
        {
            X = x;
            Y = y;
            ID = id;
            NBT = nbt;
        }

        // Optional: Add methods to manipulate NBT data or perform other operations related to the block

        public bool HasCoordinates(int x, int y)
        {
                return X == x && Y == y;
        }

        public void Modify(int x, int y, int nuid)
        {
            if (HasCoordinates(x, y))
            {
                ID = nuid;
            }
        }
    }
}

