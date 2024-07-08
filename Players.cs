using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Jump;

namespace Players
{
    public class Player
    {
        public int Health { get; private set; } = 100;
        public int[] ArmorID { get; private set; } = new int[] { 0, 0, 0, 0 };
        public int[][] Inventory { get; private set; } = new int[][] { new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 } }; //ID, QUANTITY
        private Vector2 coords = new Vector2(0, 0);
        private bool isBuilding = false;
        private bool fallingEnabled = true; // Flag to track if falling is enabled

        string[] icons = { "O", "<", ">" };
        uint jumpHeight = 1;

        public Vector2 GetCoords()
        {
            return coords;
        }

        public uint GetJumpHeight()
        {
            return jumpHeight;
        }

        public void UpdateFrame(int direction)
        {
            Console.SetCursorPosition((int)GetCoords().X, (int)GetCoords().Y);
            Console.Write(icons[direction]);
        }

        public void Move(Vector2 displacement)
        {
            if (isBuilding == true) { return; }

            //Console.SetCursorPosition((int)coords.X, (int)coords.Y);
            //Console.Write(' ');
            coords += displacement;
            UpdateFrame(0);


            if (displacement.X > 0)
            {
                //RemoveAllOs();
                //Console.SetCursorPosition((int)(coords.X - displacement.X), 0);
                UpdateFrame(1);
                
            }
            else if (displacement.X < 0)
            {
                //RemoveAllOs();
                UpdateFrame(2);
                
            }
            else
            {
                //RemoveAllOs();
                UpdateFrame(0);
                
            }

        }

        public void Fall(int dist)
        {
            if (!fallingEnabled) return;
            
            Move(new Vector2(0, dist));
            
        }

        public bool IsFalling()
        {
            return fallingEnabled && coords.Y < Console.WindowHeight - 1; // Consider the player as falling if it's not at the bottom
        }

        public void SetCoords(Vector2 newCoords)
        {
            coords = newCoords;
        }

        public void PJump()
        {
            if (Bounds.Bounds.Check(new Bounds.Bounds.COORD((short)this.GetCoords().X, (short)this.GetCoords().Y)).Item1[0] == ' ') { return; }
            Jump.Jump._JUMP(this);
        }

        public bool IsBuilding()
        {
            return isBuilding;
        }

        public void SetBuilding(bool status)
        {
            isBuilding = status;
        }

        public void SetFallingEnabled(bool enabled)
        {
            fallingEnabled = enabled;
        }
    }
}
